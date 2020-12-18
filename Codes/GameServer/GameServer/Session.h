#pragma once

#include <boost/asio.hpp>
#include <boost/bind.hpp>
#include <array>
#include <deque>

#include <boost/thread/mutex.hpp>
#include <boost/asio/streambuf.hpp>
#include "BasePacketDefine.h"

class BaseServer;

class Session
{
private:

    bool is_connected;
    int m_sessionId;
    boost::asio::ip::tcp::socket m_socket;
    boost::mutex m_lock;
    BaseServer *m_server;

    std::array<char, MaxBufferSize> m_recvBuffer;
    std::array<char, MaxBufferSize> m_packetBuffer;
    std::deque<char*> m_sendQueue;
    
//    boost::asio::streambuf m_packetBuffer;
//    boost::asio::streambuf m_recvBuffer;

    unsigned int m_packetBufferSize;

public:
    explicit Session(int sessionId, boost::asio::io_service& io_service, BaseServer* server);
    ~Session();

    void Init();
    void PostRecv();
    void PostSend(const bool immediately, const int size, char* data);

    int getSessionId() { return m_sessionId; }
    boost::asio::ip::tcp::socket& getSocket() { return m_socket; }

    bool isConnected() { return is_connected; }
    void setConnected(bool conn) { is_connected = conn; }

private:
    void handle_send(const boost::system::error_code& err, size_t bytes_transferred);
    void handle_recv(const boost::system::error_code& err, size_t bytes_transferred);
};

