#include "Session.h"
#include "BaseServer.h"

Session::Session(int sessionId, boost::asio::io_service& io_service, BaseServer* server)
    : m_sessionId(sessionId), m_socket(io_service), m_server(server)
{
    m_packetBufferSize = 0;
    setConnected(false);
}

Session::~Session()
{
    while (m_sendQueue.empty() == false)
    {
        delete[] m_sendQueue.front();
        m_sendQueue.pop_front();
    }
}

void Session::Init()
{
}

void Session::PostRecv()
{
    memset(&m_recvBuffer, '\0', sizeof(m_recvBuffer));

    m_socket.async_read_some(boost::asio::buffer(m_recvBuffer),
        boost::bind(&Session::handle_recv, this,
            boost::asio::placeholders::error,
            boost::asio::placeholders::bytes_transferred)
        );
}

void Session::PostSend(const bool immediately, const int size, char* data)
{
    char* sendData = nullptr;

    if (immediately)
    {
        sendData = data;
    }
    else
    {
        sendData = new char[size];
        memcpy(sendData, data, size);

        m_sendQueue.push_back(sendData);
    }
    
    if (immediately == false && m_sendQueue.size() > 1)
    {
        return;
    }

    boost::asio::async_write(m_socket, boost::asio::buffer(sendData, size),
        boost::bind(&Session::handle_send, this,
            boost::asio::placeholders::error,
            boost::asio::placeholders::bytes_transferred)
        );
}

void Session::handle_send(const boost::system::error_code& err, size_t byte_transferred)
{
    if (m_sendQueue.size() > 0)
    {
        //delete[] m_sendQueue.front();
        m_sendQueue.pop_front();
    }
    char* data = nullptr;

    if (m_sendQueue.empty() == false)
    {
        data = m_sendQueue.front();
    }
        
    if (data != nullptr)
    {
        PACKET_HEADER* pHeader = (PACKET_HEADER*)data;
        PostSend(true, pHeader->Size, data);
    }
}

void Session::handle_recv(const boost::system::error_code& err, size_t byte_transferred)
{
    // ��Ŷ �������ϴ°� �ٽ� ��������.
    // ������ : ������ �������� ���� ���� ���µ� ���� ���ǿ� ���� ���� Ŀ�ؼ��� ���� ��, Ŀ�ؼ� ���� �������� ���� ����.
    // ���ۿ� ���̴� �����Ϳ� ���� ���� �������� ������ ���, ������ �������� ������ ������ ������ �� ����.
    // ��¥�� ���� �� ���ǿ� ���Ǵ� �κ��̶� ���� ���� ���ص� �������� �Ǳ�� �ϰ����� �ʿ��� ��찡 ���� �����ؾߵ�.
    if (err)
    {
        m_server->CloseSession(m_sessionId);
    }
    else
    {
        memcpy(&m_packetBuffer[m_packetBufferSize], m_recvBuffer.data(), byte_transferred);

        int nPacketData = m_packetBufferSize + byte_transferred;
        int nReadData = 0;
    
        while (nPacketData > 0)
        {
            if (nPacketData < sizeof(PACKET_HEADER))
            {
                break;
            }

            PACKET_HEADER *pHeader = (PACKET_HEADER*)&m_packetBuffer[nReadData];

            if (pHeader->Size <= nPacketData)
            {
                m_server->ProcessPacket(m_sessionId, &m_packetBuffer[nReadData]);
                nPacketData -= pHeader->Size;
                nReadData += pHeader->Size;
            }
            else
            {
                break;
            }
        }

        if (nPacketData > 0)
        {
            char tempBuffer[MaxBufferSize] = { 0, };
            memcpy(&tempBuffer[0], &m_packetBuffer[nReadData], nPacketData);
            memcpy(&m_packetBuffer[0], &tempBuffer[0], nPacketData);
        }

        m_packetBufferSize = nPacketData;
        PostRecv();
    }
}