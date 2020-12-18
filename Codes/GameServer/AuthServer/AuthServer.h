#pragma once
#include "../GameServer/BaseServer.h"
#include "../Common/User.h"
#include <boost/asio.hpp>

class AuthServer : public BaseServer
{
public:
    AuthServer(const int port, const int maxCount, boost::asio::io_service& io_service);

    virtual void ProcessPacket(const int sessionId, char* data);

    int CreateAccount(short sessionId, const char* id, const char* password, const char* username, const char& weight, const bool& gender, const int& birthday);
    int LoginAccount(short sessionId, const char* username, const char* password);
};

