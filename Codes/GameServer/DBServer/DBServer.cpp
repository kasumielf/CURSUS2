#include "DBServer.h"
#include "../Common/commonpacket.h"
#include <memory>
#include <cassert>

DBServer::DBServer(const int port, const int maxCount, boost::asio::io_service & io_service,
                    const std::string db_ip, const std::string db_id, const std::string db_pwd, const std::string db_scheme)
    : BaseServer(port, maxCount, io_service), m_db(db_ip, db_id, db_pwd, db_scheme)
{
    try
    {
        m_db.Connect();
    }
    catch(std::runtime_error &e)
    {
        std::cout << e.what() << std::endl;
        assert(1);
    }
}

void DBServer::ProcessPacket(const int sessionId, char* data)
{
    PACKET_HEADER *pheader = (PACKET_HEADER*)data;
    
    switch (pheader->Id)
    {
    case REQ_CREATE_ACCOUNT_S2S:
    {
        CommonPacket::PACKET_REQ_CREATE_ACCOUNT_S2S* req = (CommonPacket::PACKET_REQ_CREATE_ACCOUNT_S2S*)data;
        Logging("Create Account Request(id : %s)", req->login_id);
        CreateAccount(sessionId, req->session_id, req->login_id, req->password, req->username, req->weight, req->gender, req->birthday);
        break;
    }
    case REQ_LOGIN_S2S:
    {
        CommonPacket::PACKET_REQ_LOGIN_S2S *req = (CommonPacket::PACKET_REQ_LOGIN_S2S*)data;
        Logging("Check User Auth Request(id : %s)", req->login_id);
        LoginAccount(sessionId, req->session_id, req->login_id, req->password);
        break;
    }
    
    case REQ_USER_INFO:
    {
        CommonPacket::PACKET_REQ_USER_INFO *req = reinterpret_cast<CommonPacket::PACKET_REQ_USER_INFO*>(data);

        CommonPacket::PACKET_RES_USER_INFO packet;

        packet.Init();
        packet.session_id = req->session_id;

        UserUniquePtr ptr = m_db.SelectUserInfo(req->login_id, req->password);

        packet.user_uid = ptr->getUserUid();
        strcpy_s(packet.id, ptr->getId());
        strcpy_s(packet.username, ptr->getUsername());
        packet.weight = ptr->getWeight();
        packet.gender = ptr->getGender();
        packet.birthday = ptr->getBirthday();
        packet.x = ptr->getX();
        packet.y = ptr->getY();
        packet.current_map = ptr->getCurrentMap();

        SendData(sessionId, packet.Size, reinterpret_cast<char*>(&packet));
        break;
    }
    case REQ_PLAYER_DATA_UPDATE_S2S:
    {
        CommonPacket::PACKET_REQ_PLAYER_DATA_UPDATE_S2S *req = reinterpret_cast<CommonPacket::PACKET_REQ_PLAYER_DATA_UPDATE_S2S*>(data);

        m_db.UpdateUserInfo(req->userUid, req->x, req->y, req->map_id);
        break;
    }

    case REQ_GET_RECORD_DATA_S2S:
    {
        CommonPacket::PACKET_REQ_GET_RECORD_DATA_S2S *req = reinterpret_cast<CommonPacket::PACKET_REQ_GET_RECORD_DATA_S2S*>(data);
        CommonPacket::PACKET_RES_GET_RECORD_DATA_S2S res;
        res.Init();
        res.session_id = req->session_id;

        m_db.SelectMyRecord(req->user_uid, res.record_time, res.checked_time);
            
        SendData(sessionId, res.Size, reinterpret_cast<char*>(&res));

        break;
    }
    case REQ_UPDATE_RECORD_DATA_S2S:
    {
        CommonPacket::PACKET_REQ_UPDATE_RECORD_DATA_S2S *req = reinterpret_cast<CommonPacket::PACKET_REQ_UPDATE_RECORD_DATA_S2S*>(data);
        CommonPacket::PACKET_RES_UPDATE_RECORD_DATA_S2S res;
        res.Init();
        res.session_id = req->session_id;
        res.result = m_db.UpdateMyRecord(req->user_uid, req->record_time, req->checked_time);

        SendData(sessionId, res.Size, reinterpret_cast<char*>(&res));

        break;
    }
    case REQ_RANKUSER_RECORD_DATA_S2S:
    {
        CommonPacket::PACKET_REQ_RANKUSER_RECORD_DATA_S2S *req = reinterpret_cast<CommonPacket::PACKET_REQ_RANKUSER_RECORD_DATA_S2S*>(data);
        CommonPacket::PACKET_RES_RANKUSER_RECORD_DATA_S2S res;
        res.Init();
        res.session_id = req->session_id;

        m_db.SelectRankUserRecord(res.username, res.record_time, res.checked_time, res.item_count);

        SendData(sessionId, res.Size, reinterpret_cast<char*>(&res));
        break;
    }
    
    case REQ_GET_REPLAY_RECORD_DATA_S2S:
    {
        CommonPacket::PACKET_REQ_GET_REPLAY_RECORD_DATA_S2S *req = reinterpret_cast<CommonPacket::PACKET_REQ_GET_REPLAY_RECORD_DATA_S2S*>(data);

        int size;
        unsigned char data[5][120];
        double record_time[5];

        m_db.SelectMyReplayRecords(req->user_uid, data, size, record_time);

        for (int i = 0; i < size; ++i)
        {
            CommonPacket::PACKET_RES_GET_REPLAY_RECORD_DATA_S2S res;
            res.Init();
            res.session_id = req->session_id;
            res.index = i;

            for (int j = 0; j < 120; ++j)
            {
                res.records[j] = data[i][j];
            }
            
            res.record_time = record_time[i];

            SendData(sessionId, res.Size, reinterpret_cast<char*>(&res));
        }

        break;
    }
    case REQ_UPDATE_REPLAY_RECORD_DATA_S2S:
    {
        CommonPacket::PACKET_REQ_UPDATE_REPLAY_RECORD_DATA_S2S *req = reinterpret_cast<CommonPacket::PACKET_REQ_UPDATE_REPLAY_RECORD_DATA_S2S*>(data);
        CommonPacket::PACKET_RES_UPDATE_REPLAY_RECORD_DATA_S2S res;
        res.Init();

        res.result = m_db.UpdateMyReplayRecords(req->user_uid, req->records);
        res.session_id = req->session_id;

        SendData(sessionId, res.Size, reinterpret_cast<char*>(&res));
        break;
    }
    default:
        Logging("Invalid AuthServer Request!");
        this->CloseSession(sessionId);
        break;
    }
}

int DBServer::CreateAccount(short serverId, short sessionId, const char* id, const char* password, const char* username, const char& weight, const bool& gender, int& birthday)
{
    CommonPacket::PACKET_RES_CREATE_ACOUNT_S2S resPacket;
    
    resPacket.Init();
    resPacket.session_id = sessionId;

    UserUniquePtr uptr = m_db.SelectUserInfo(id);

    if(uptr == nullptr)
    {
        resPacket.result = m_db.InsertUserInfo(sessionId, id, password, username, weight, gender, birthday);
        resPacket.result = 1;
    }
    else
    {
        resPacket.result = -1;
    }

    SendData(serverId, resPacket.Size, (char*)&resPacket);

    return 0;
}

int DBServer::LoginAccount(short resSessionId, int & sessionId, const char * id, const char * password)
{
    
    CommonPacket::PACKET_RES_LOGIN_S2S resPacket;

    resPacket.Init();
    resPacket.session_id = sessionId;
    resPacket.result =  m_db.SelectUserInfo(id, password) == nullptr ? -1 : 1;

    SendData(resSessionId, resPacket.Size, (char*)&resPacket);

    return 0;
}