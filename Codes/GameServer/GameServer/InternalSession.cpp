#include "InternalSession.h"
#include "BasePacketDefine.h"
#include <iostream>

InternalSession::InternalSession(int sessionId, boost::asio::io_service& io_service, BaseServer* server)
    : Session(sessionId, io_service, server)
{
}

InternalSession::~InternalSession()
{
}

void InternalSession::PostConnect(std::string ip, const unsigned short port)
{
    auto endpoint = boost::asio::ip::tcp::endpoint(boost::asio::ip::address::from_string(ip), port);
    
    getSocket().async_connect(endpoint,
        boost::bind(&InternalSession::handle_connect, this,
            boost::asio::placeholders::error));
}

void InternalSession::handle_connect(const boost::system::error_code& err)
{
    if (!err)
    {
        std::cout << "Server connect complete" << std::endl;

        PostRecv();
    }
    else
    {
        std::cout << err.value() << std::endl;
    }
}