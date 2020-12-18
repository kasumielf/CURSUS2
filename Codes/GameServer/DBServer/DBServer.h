#pragma once

#include <string>
#include "../GameServer/BaseServer.h"
#include <boost/asio.hpp>
#include "../Common/User.h"
#include "DBManager.h"

class DBServer : public BaseServer
{
//private:
public:
    DBManager m_db;
public:
    DBServer(const int port, const int maxCount, boost::asio::io_service& io_service,
        const std::string db_ip, const std::string db_id, const std::string db_pwd, const std::string db_scheme);

    virtual void ProcessPacket(const int sessionId, char* data);

    int CreateAccount(short serverId, short sessionId, const char* id, const char* password, const char* username, const char& weight, const bool& gender, int& birthday);
    int LoginAccount(short resSessionId, int &sessionId, const char* username, const char* password);
    const User* GetAccountInfo(const char* id, const char* password);
};

