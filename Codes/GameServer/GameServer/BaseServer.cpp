#include "BaseServer.h"
#include <iostream>
#include <cstdarg>

BaseServer::BaseServer(const unsigned int port, const unsigned int maxSessionCount, boost::asio::io_service& io_service)
    : m_acceptor(io_service, boost::asio::ip::tcp::endpoint(boost::asio::ip::tcp::v4(), port)), m_maxSessionCount(maxSessionCount)
{
    m_isAccepting = false;
    m_maxSessionIdCount = 10000;

    boost::asio::ip::tcp::resolver resolver(io_service);
    boost::asio::ip::tcp::resolver::query query(boost::asio::ip::host_name(), "");
    boost::asio::ip::tcp::resolver::iterator it = resolver.resolve(query);

    while (it != boost::asio::ip::tcp::resolver::iterator())
    {
        boost::asio::ip::address addr = (it++)->endpoint().address();
        if (addr.is_v6() == false)
        {
            m_addrs.push_back(addr.to_string());
        }
    }
}

BaseServer::~BaseServer()
{
    auto iter_b = m_sessionList.begin();
    auto iter_e = m_sessionList.end();

    for (iter_b; iter_b != iter_e; iter_b++)
    {
        if ((*iter_b).second->getSocket().is_open())
        {
            (*iter_b).second->setConnected(false);
            (*iter_b).second->getSocket().close();
        }

        delete (*iter_b).second;
    }
}

void BaseServer::Init()
{
    for(int j = 0; j < m_maxSessionIdCount; ++j)
    {
        m_sessionIdQueue.push_back(j);
    }
}

void BaseServer::Start()
{
    Logging("Server start");

    PostAccept();
}

void BaseServer::Connect(const std::string name, const std::string ip, const unsigned short port)
{
    int sessionId = m_internalSessionList.size();

    InternalSession* int_session = new InternalSession(sessionId, m_acceptor.get_io_service(), this);

    int_session->PostConnect(ip, port);

    m_internalSessionList.insert({sessionId, int_session});
    m_internalSessionId.insert({ name, sessionId });

    Logging("InternalSession is created and accepted with session id is " + sessionId);
}

void BaseServer::CloseSession(const int sessionId)
{
    RemoveSession(sessionId);

    PostAccept();
}
void BaseServer::RemoveSession(const int sessionId)
{
    if (m_sessionList[sessionId] != nullptr)
    {
        m_sessionList[sessionId]->setConnected(false);
        m_sessionList[sessionId]->getSocket().close();

        m_sessionList[sessionId] = nullptr;
        delete m_sessionList[sessionId];

        m_sessionList.erase(sessionId);
    }
}

bool BaseServer::PostAccept()
{
    if (m_sessionIdQueue.empty())
    {
        m_isAccepting = false;
        return false;
    }

    m_isAccepting = true;

    int sessionId = m_sessionIdQueue.front();
    m_sessionIdQueue.pop_front();

    Session *session = new Session(sessionId, m_acceptor.get_io_service(), this);
    m_sessionList[sessionId] = session;

    m_acceptor.async_accept(session->getSocket(), boost::bind(&BaseServer::handle_accept, this, session, boost::asio::placeholders::error));

    return true;
}

void BaseServer::handle_accept(Session *session, const boost::system::error_code& error)
{
    if (!error)    
    {
        Logging("session is accepted from session id " + session->getSessionId());

        session->Init();
        session->PostRecv();
        session->setConnected(true);
        PostAccept();
    }
    else
    {
        CloseSession(session->getSessionId());
        Logging(error);
    }
}

void BaseServer::InternalSendQueueWorkerThread()
{
}

void BaseServer::SendBoradCast(char* data, const int size)
{
    auto iter_b = m_sessionList.begin();
    auto iter_e = m_sessionList.end();

    for(;iter_b!=iter_e;++iter_b)
    {
        if((*iter_b).second != nullptr && (*iter_b).second->isConnected())
        (*iter_b).second->PostSend(false, size, data);
    }
}


void BaseServer::SendData(const int sessionId, const int size, char* data)
{
    if ((m_sessionList[sessionId] != nullptr && m_sessionList[sessionId]->isConnected()))
    {
        lock.lock();
        m_sessionList[sessionId]->PostSend(false, size, data);
        lock.unlock();
    }
}

void BaseServer::SendDataToInternalServer(const std::string name, const int size, char* data)
{
    // ToDo : SendData�� ���� Data Send Queue�� ����� �ϳ��� ó������.
    short val = m_internalSessionId.find(name)->second;
    /*InternalSendData int_send;

    memcpy(int_send.data, data, size);
    int_send.size = size;
    int_send.name = name;
    
    m_internalSessionSendQueue.push_back(int_send);
*/
    m_internalSessionList[val]->PostSend(false, size, data);
}

void BaseServer::SendDataToInternalServer(const short internal_id, const int size, char * data)
{
    m_internalSessionList[internal_id]->PostSend(false, size, data);
}


void BaseServer::Logging(const char* message, ...)
{
    char buf[512] = { 0, };
    va_list ap;
    va_start(ap, message);
#ifdef _MSC_VER
    vsprintf_s(buf, message, ap);
#else
    vsprintf(buf, message, ap);
#endif

    va_end(ap); puts(buf);
}

void BaseServer::Logging(const boost::system::error_code &ec)
{
    std::cout << ec.value() << std::endl;
}