#include "WorldServer.h"

#include <curl/curl.h>
#include <iostream>
#include "json/json.h"

size_t write_html(void *ptr, size_t size, size_t count, void *stream)
{
    ((string*)stream)->append((char*)ptr, 0, size*count);

    return size*count;
}

WorldServer::WorldServer(const int port, const int maxCount, boost::asio::io_service &io_service) : BaseServer(port, maxCount, io_service)
{
    gameWorld = new GameWorld();

    gameWorld->Init();

    forecast_refresh = true;
    current_forecast = Forecast::Sunny;

    for (int i = 100; i < MAX_GAMEROOM_SIZE; ++i)
    {
        gameroom_ids.push_back(i);
    }

    channel_info_packet.Init();
}

WorldServer::~WorldServer()
{
    delete gameWorld;
}

void WorldServer::ProcessPacket(const int sessionId, char* data)
{
    PACKET_HEADER* pheader = (PACKET_HEADER*)data;

    switch (pheader->Id)
    {
        case REQ_CHANNEL_INFO_S2S:
        {
            CommonPacket::PACEKT_REQ_CHANNEL_INFO_S2S *pPacket = reinterpret_cast<CommonPacket::PACEKT_REQ_CHANNEL_INFO_S2S*>(data);

            channel_info_packet.capacity = gameWorld->GetCurrentPlayerCount();
            channel_info_packet.status = 1;

            SendData(sessionId, channel_info_packet.Size, (char*)&channel_info_packet);
            break;
        }
        case REQ_PLAYER_ENTER:
        {
            CommonPacket::PACKET_REQ_PLAYER_WORLD_ENTER *pPacket = reinterpret_cast<CommonPacket::PACKET_REQ_PLAYER_WORLD_ENTER*>(data);

            CommonPacket::PACKET_REQ_USER_INFO sendPacket;
            
            sendPacket.Init();
            sendPacket.session_id = sessionId;

            memcpy(sendPacket.login_id, pPacket->login_id, strlen(pPacket->login_id)+1);
            memcpy(sendPacket.password, pPacket->password, strlen(pPacket->password)+1);

            SendDataToInternalServer("DB", sendPacket.Size, (char*)&sendPacket);

            break;
        }
        case RES_USER_INFO:
        {
            CommonPacket::PACKET_RES_USER_INFO *pPacket = reinterpret_cast<CommonPacket::PACKET_RES_USER_INFO*>(data);
            CommonPacket::PACKET_RES_PLAYER_ENTER sendPacket;

            //if (gameWorld->isExistUser(pPacket->user_uid))
            //{
            //    sendPacket.userUid = -1;
            //}
            //else
            {
                User *user = new User();

                user->setUserUid(pPacket->user_uid);
                user->setId(pPacket->id);
                user->setUsername(pPacket->username);
                user->setWeight(pPacket->weight);
                user->setPosition(pPacket->x, pPacket->y, 0.0f);
                user->setCurrentMap(pPacket->current_map);
                user->setSessionId(pPacket->session_id);

                sendPacket.Init();
                sendPacket.userUid = user->getUserUid();
                strcpy_s(sendPacket.id, user->getId());
                strcpy_s(sendPacket.username, user->getUsername());
                sendPacket.weight = user->getWeight();
                sendPacket.gender = user->getGender();
                sendPacket.current_map = user->getCurrentMap();

                gameWorld->AddUser(pPacket->session_id, user);

                session_ids[user->getUserUid()] = pPacket->session_id;

                CommonPacket::PACKET_NTF_PLAYER_ENTER ntf;
                ntf.Init();
                ntf.user_id = user->getUserUid();
                strcpy_s(ntf.username, user->getUsername());
                ntf.x = user->getX();
                ntf.y = user->getY();
                ntf.z = user->getZ();

                auto iter_b = gameWorld->GetCurrentMap()->GetUsersIterator_Begin(user->getCurrentMap());
                auto iter_e = gameWorld->GetCurrentMap()->GetUsersIterator_End(user->getCurrentMap());

                for (; iter_b != iter_e; ++iter_b)
                {
                    if ((*iter_b)->getUserUid() != user->getUserUid())
                    {
                        CommonPacket::PACKET_NTF_PLAYER_ENTER ntf_to_me;
                        ntf_to_me.Init();
                        ntf_to_me.user_id = (*iter_b)->getUserUid();
                        strcpy_s(ntf_to_me.username, (*iter_b)->getUsername());
                        ntf_to_me.x = (*iter_b)->getX();
                        ntf_to_me.y = (*iter_b)->getY();
                        ntf_to_me.z = (*iter_b)->getZ();

                        SendData(pPacket->session_id, ntf_to_me.Size, reinterpret_cast<char*>(&ntf_to_me));
                        SendData(session_ids[(*iter_b)->getUserUid()], ntf.Size, reinterpret_cast<char*>(&ntf));
                    }
                }

                SendData(pPacket->session_id, sendPacket.Size, reinterpret_cast<char*>(&sendPacket));
                SendData(pPacket->session_id, forecast_packet.Size, reinterpret_cast<char*>(&forecast_packet));
            }

            break;
        }
        case REQ_PLAYER_MOVE:
        {
            CommonPacket::PACKET_REQ_PLAYER_MOVE *pPacket = (CommonPacket::PACKET_REQ_PLAYER_MOVE*)data;

            User* user = gameWorld->GetUserInfo(sessionId);

            if (user != nullptr)
            {
                gameWorld->SetPosition(sessionId, pPacket->x, pPacket->y, pPacket->z, pPacket->v, pPacket->r);

                CommonPacket::PACKET_NTF_PLAYER_MOVE ntf;

                ntf.Init();
                ntf.user_id = user->getUserUid();
                ntf.x = user->getX();
                ntf.y = user->getY();
                ntf.z = user->getZ();
                ntf.v = user->getSpeed();

                auto iter_b = gameWorld->GetCurrentMap()->GetUsersIterator_Begin(user->getCurrentMap());
                auto iter_e = gameWorld->GetCurrentMap()->GetUsersIterator_End(user->getCurrentMap());

                for (; iter_b != iter_e; ++iter_b)
                {
                    if ((*iter_b)->getUserUid() != user->getUserUid() && (*iter_b)->isDuringOnGame() == false)
                    {
                        if(m_sessionList[session_ids[(*iter_b)->getUserUid()]] != nullptr && m_sessionList[session_ids[(*iter_b)->getUserUid()]]->isConnected())
                            SendData(session_ids[(*iter_b)->getUserUid()], ntf.Size, (char*)(&ntf));
                    }
                }

            }
            
            break;
        }
        case REQ_PLAYER_EXIT:
        {
            User* user = gameWorld->GetUserInfo(sessionId);

            if (user != nullptr)
            {
                CommonPacket::PACKET_NTF_PLAYER_EXIT ntf;
                ntf.Init();
                ntf.user_id = user->getUserUid();

                // 월드에서 부터 제거
                auto iter_b = gameWorld->GetCurrentMap()->GetUsersIterator_Begin(user->getCurrentMap());
                auto iter_e = gameWorld->GetCurrentMap()->GetUsersIterator_End(user->getCurrentMap());

                for (; iter_b != iter_e; ++iter_b)
                {
                    if ((*iter_b)->getUserUid() != user->getUserUid())
                    {
                        SendData(session_ids[(*iter_b)->getUserUid()], ntf.Size, (char*)(&ntf));
                    }
                }

                // 게임 룸으로 부터 제거

                for each(std::pair<int, GameRoom*> room in gamerooms)
                {
                    room.second->PlayerExit(user);
                }


                this->RemoveSession(sessionId);
                gameWorld->RemoveUser(sessionId);
                delete user;

                Logging("Player %d is quit from world.", sessionId);
            }

            break;
        }
        case REQ_CREATE_ROOM_INFO:
        {
            GameRoomPacket::PACKET_REQ_CREATE_ROOM_INFO *req = (GameRoomPacket::PACKET_REQ_CREATE_ROOM_INFO*)data;
            GameRoomPacket::PACKET_RES_CREATE_ROOM_INFO res;
            res.Init();

            int room_id = gameroom_ids.front();
            gameroom_ids.pop_front();

            User* user = gameWorld->GetUserInfo(sessionId);
                    
            GameRoom* new_room = new GameRoom(this, gameWorld->GetUserInfo(sessionId), req->map_id, room_id);
            new_room->AddUser(sessionId, user);
            gamerooms[room_id] = new_room;
            res.room_id = room_id;
            res.map_id = req->map_id;
            res.result = 1;

            SendData(sessionId, res.Size, reinterpret_cast<char*>(&res));

            break;
        }
        case REQ_ROOM_INFO:
        {
            for each(std::pair<short , GameRoom*> room in gamerooms)
            {
                if (room.second != nullptr && room.second->IsGameEnd() == false)
                {
                    GameRoomPacket::PACKET_RES_ROOM_INFO res;
                    res.Init();

                    res.room_id = room.first;
                    res.map_id = room.second->GetMapId();
                    res.user_count = room.second->GetPlayerCount();

                    SendData(sessionId, res.Size, (char*)(&res));
                }
            }

            break;
        }
        case REQ_ROOM_PLAYER_READY:
        {
            GameRoomPacket::PACKET_REQ_ROOM_PLAYER_READY *req = (GameRoomPacket::PACKET_REQ_ROOM_PLAYER_READY*)data;

            if (gamerooms[req->room_id] != nullptr)
            {
                User* user = gameWorld->GetUserInfo(sessionId);

                if (user != nullptr)
                    gamerooms[req->room_id]->SetReady(user);
            }

            break;
        }
        case REQ_ROOM_PLAYER_EXIT:
        {
            GameRoomPacket::PACKET_REQ_ROOM_PLAYER_EXIT *req = (GameRoomPacket::PACKET_REQ_ROOM_PLAYER_EXIT*)data;

            if (gamerooms[req->room_id] != nullptr)
            {
                User* user = gameWorld->GetUserInfo(sessionId);

                if (user != nullptr)
                {
                    gamerooms[req->room_id]->PlayerExit(user);
                    
                    if (gamerooms[req->room_id]->GetPlayerCount() <= 0)
                    {
                        delete gamerooms[req->room_id];
                        gamerooms.erase(req->room_id);

                        AddGameRoomNumberId(req->room_id);
                    }
                }
            }

            break;
        }
        case REQ_ROOM_SET_TYPE:
        {
            GameRoomPacket::PACKET_REQ_ROOM_SET_TYPE *req = (GameRoomPacket::PACKET_REQ_ROOM_SET_TYPE*)data;

            if (gamerooms[req->room_id] != nullptr)
            {
                gamerooms[req->room_id]->SetType(req->player_index, req->type);
            }

            break;
        }

        case REQ_ROOM_PLAYER_ENTER:
        {
            GameRoomPacket::PACKET_REQ_ROOM_PLAYER_ENTER *req = (GameRoomPacket::PACKET_REQ_ROOM_PLAYER_ENTER*)data;

            if (gamerooms[req->room_id] != nullptr)
            {
                User* user = gameWorld->GetUserInfo(sessionId);

                if (user != nullptr)
                    gamerooms[req->room_id]->PlayerEnter(sessionId, user);
            }

            break;
        }
        case REQ_GAME_START:
        {
            GameRoomPacket::PACKET_REQ_ROOM_GAME_START *req = (GameRoomPacket::PACKET_REQ_ROOM_GAME_START*)data;

            if (gamerooms[req->room_id] != nullptr)
            {
                gamerooms[req->room_id]->GameStart();
            }
            break;
        }
        case REQ_INGAME_PLAYER_LIST:
        {
            TrackScenePacket::PACKET_REQ_INGAME_PLAYER_LIST *req = (TrackScenePacket::PACKET_REQ_INGAME_PLAYER_LIST*)data;
            
            if (gamerooms[req->room_id] != nullptr)
            {
                gamerooms[req->room_id]->SendIngameObjects(req->player_index, sessionId);
            }

            break;
        }
        case REQ_INGAME_READY:
        {
            TrackScenePacket::PACKET_REQ_INGAME_READY *req = (TrackScenePacket::PACKET_REQ_INGAME_READY*)data;

            if (gamerooms[req->room_id] != nullptr)
            {
                gamerooms[req->room_id]->SetIngameReady(req->player_index);
            }

            break;
        }
        case REQ_ROOM_UPDATE_PLAYER_POSITION:
        {
            TrackScenePacket::PACKET_REQ_ROOM_UPDATE_PLAYER_POSITION *req = (TrackScenePacket::PACKET_REQ_ROOM_UPDATE_PLAYER_POSITION*)data;

            if (gamerooms[req->room_id] != nullptr)
            {
                gamerooms[req->room_id]->SetPosition(req->player_index, req->x, req->y, req->z, req->v, req->r);
            }

            break;
        }
        case REQ_UPDATE_TRACK_COUNT:
        {
            TrackScenePacket::PACKET_REQ_UPDATE_TRACK_COUNT *req = (TrackScenePacket::PACKET_REQ_UPDATE_TRACK_COUNT*)data;

            if (gamerooms[req->room_id] != nullptr)
            {
                gamerooms[req->room_id]->UpdateTrackCount(req->player_index);
            }

            break;
        }
        case REQ_GET_RECORD_DATA:
        {
            if (GetWorldPtr()->GetUserInfo(sessionId) != nullptr)
            {
                CommonPacket::PACKET_REQ_GET_RECORD_DATA *req = reinterpret_cast<CommonPacket::PACKET_REQ_GET_RECORD_DATA*>(data);
                CommonPacket::PACKET_REQ_GET_RECORD_DATA_S2S req_to_db;

                req_to_db.Init();
                req_to_db.session_id = sessionId;
                req_to_db.user_uid = GetWorldPtr()->GetUserInfo(sessionId)->getUserUid();

                SendDataToInternalServer("DB", req_to_db.Size, (char*)&req_to_db);
            }

            break;
        }
        case RES_GET_RECORD_DATA_S2S:
        {
            CommonPacket::PACKET_RES_GET_RECORD_DATA_S2S *req = reinterpret_cast<CommonPacket::PACKET_RES_GET_RECORD_DATA_S2S*>(data);
            CommonPacket::PACKET_RES_GET_RECORD_DATA res;

            res.Init();
            res.record_time = req->record_time;
            res.checked_time = req->checked_time;
            
            SendData(req->session_id, res.Size, reinterpret_cast<char*>(&res));

            break;
        }
        case REQ_UPDATE_RECORD_DATA:
        {
            if (GetWorldPtr()->GetUserInfo(sessionId) != nullptr)
            {
                CommonPacket::PACKET_REQ_UPDATE_RECORD_DATA *req = reinterpret_cast<CommonPacket::PACKET_REQ_UPDATE_RECORD_DATA*>(data);
                CommonPacket::PACKET_REQ_UPDATE_RECORD_DATA_S2S req_to_db;

                req_to_db.Init();
                req_to_db.session_id = sessionId;
                req_to_db.user_uid = GetWorldPtr()->GetUserInfo(sessionId)->getUserUid();
                req_to_db.record_time = req->record_time;
                req_to_db.checked_time = req->checked_time;

                SendDataToInternalServer("DB", req_to_db.Size, reinterpret_cast<char*>(&req_to_db));
            }

            break;
        }
        case RES_UPDATE_RECORD_DATA_S2S:
        {
            CommonPacket::PACKET_RES_UPDATE_RECORD_DATA_S2S *req = reinterpret_cast<CommonPacket::PACKET_RES_UPDATE_RECORD_DATA_S2S*>(data);
            CommonPacket::PACKET_RES_UPDATE_RECORD_DATA res;

            res.Init();
            res.result = req->result;
            SendData(req->session_id, res.Size, reinterpret_cast<char*>(&res));

            break;
        }
        case REQ_RANKUSER_RECORD_DATA:
        {
            CommonPacket::PACKET_REQ_RANKUSER_RECORD_DATA_S2S req_to_db;
            req_to_db.Init();
            req_to_db.session_id = sessionId;

            SendDataToInternalServer("DB", req_to_db.Size, reinterpret_cast<char*>(&req_to_db));

            break;
        }
        case RES_RANKUSER_RECORD_DATA_S2S:
        {
            CommonPacket::PACKET_RES_RANKUSER_RECORD_DATA_S2S *res = reinterpret_cast<CommonPacket::PACKET_RES_RANKUSER_RECORD_DATA_S2S*>(data);
            CommonPacket::PACKET_RES_RANKUSER_RECORD_DATA res_to_client;

            res_to_client.Init();
            res_to_client.item_count = res->item_count;
            
            for (int i = 0; i < res->item_count; ++i)
            {
                res_to_client.record_time[i] = res->record_time[i];
                res_to_client.checked_time[i] = res->checked_time[i];
                strcpy_s(res_to_client.username[i], res->username[i]);
            }

            SendData(res->session_id, res_to_client.Size, reinterpret_cast<char*>(&res_to_client));

            break;
        }
        case REQ_GET_REPLAY_RECORD_DATA:
        {
            CommonPacket::PACKET_REQ_GET_REPLAY_RECORD_DATA *req = reinterpret_cast<CommonPacket::PACKET_REQ_GET_REPLAY_RECORD_DATA*>(data);
            CommonPacket::PACKET_REQ_GET_REPLAY_RECORD_DATA_S2S req_to_db;

            req_to_db.Init();
            req_to_db.session_id = sessionId;
            req_to_db.user_uid = GetWorldPtr()->GetUserInfo(sessionId)->getUserUid();

            SendDataToInternalServer("DB", req_to_db.Size, reinterpret_cast<char*>(&req_to_db));

            break;
        }
        case REQ_UPDATE_REPLAY_RECORD_DATA:
        {
            CommonPacket::PACKET_REQ_UPDATE_REPLAY_RECORD_DATA *req = reinterpret_cast<CommonPacket::PACKET_REQ_UPDATE_REPLAY_RECORD_DATA*>(data);
            CommonPacket::PACKET_REQ_UPDATE_REPLAY_RECORD_DATA_S2S req_to_db;

            req_to_db.Init();
            req_to_db.session_id = sessionId;
            req_to_db.user_uid = req->user_uid;

            for (int i = 0; i < 120; i++)
            {
                req_to_db.records[i] = req->records[i];
            }

            SendDataToInternalServer("DB", req_to_db.Size, reinterpret_cast<char*>(&req_to_db));

            break;
        }

        case RES_GET_REPLAY_RECORD_DATA_S2S:
        {
            CommonPacket::PACKET_RES_GET_REPLAY_RECORD_DATA_S2S *res = reinterpret_cast<CommonPacket::PACKET_RES_GET_REPLAY_RECORD_DATA_S2S*>(data);
            CommonPacket::PACKET_RES_GET_REPLAY_RECORD_DATA res_to_client;
            res_to_client.Init();
            res_to_client.index = res->index;

            for (int i = 0; i < 120; i++)
            {
                res_to_client.records[i] = res->records[i];
            }

            res_to_client.record_time = res->record_time;

            std::cout << "GET REPLAY FROM DB! " << res->index << std::endl;

            char* data = reinterpret_cast<char*>(&res_to_client);

            PACKET_DUMMY dummy;
            dummy.Init();

            std::cout << "size : " << res_to_client.Size << std::endl;

            SendData(res->session_id, res_to_client.Size, data);
            SendData(res->session_id, dummy.Size, reinterpret_cast<char*>(&dummy));

            break;
        }
        case RES_UPDATE_REPLAY_RECORD_DATA_S2S:
        {
            CommonPacket::PACKET_RES_UPDATE_REPLAY_RECORD_DATA_S2S *res = reinterpret_cast<CommonPacket::PACKET_RES_UPDATE_REPLAY_RECORD_DATA_S2S*>(data);
            CommonPacket::PACKET_RES_UPDATE_REPLAY_RECORD_DATA res_to_client;

            res_to_client.Init();
            res_to_client.result = res->result;

            SendData(res->session_id, res_to_client.Size, reinterpret_cast<char*>(&res_to_client));            
            break;
        }

        default:
        {
            Logging("Invalid WorldServer Request! %d", pheader->Id);
            this->CloseSession(sessionId);
            gameWorld->RemoveUser(sessionId);

            break;
        }
    }    
}

void WorldServer::UpdatePlayDataThread()
{
    while (true)
    {
        auto iter_b = gameWorld->getPlayersBegin();
        auto iter_e = gameWorld->getPlayersEnd();

        CommonPacket::PACKET_REQ_PLAYER_DATA_UPDATE_S2S packet;
        packet.Init();

        // 접속 해제된 유저 처리 안되어있음.
        for (; iter_b != iter_e; ++iter_b)
        {
            if ((*iter_b).second != nullptr)
            {
                if ((*iter_b).second->isDuringOnGame() == false)
                {
                    packet.userUid = (*iter_b).second->getUserUid();
                    packet.x = (*iter_b).second->getX();
                    packet.y = (*iter_b).second->getY();
                    packet.map_id = (*iter_b).second->getCurrentMap();

                    SendDataToInternalServer("DB", packet.Size, (char*)&packet);
                }
            }
        }

        boost::this_thread::sleep(boost::posix_time::milliseconds(60000));
    }
}

void WorldServer::NotifyToMySector(User * user, PACKET_HEADER packet)
{
}

void WorldServer::NotifyForecastInfo()
{
    forecast_packet.forecast = (char)current_forecast;
    SendBoradCast(reinterpret_cast<char*>(&forecast_packet), forecast_packet.Size);
}

void WorldServer::UpdateForecastInfoThread()
{
    // 타이머 스레드를 돌려서 1분에 1번 씩 접속한 클라이언트 전원에게
    // node에서 받아온 기상 정보를 전송한다.

    // 공유 메모리에 접근한다.
    // 스레드 관리 주의해야함.
    // 주의해야 할 점 : 스레드 내에서 각각의 클라에다가 정보를 던지는데 다른 스레드에서 해당 클라이언트를 erase 할 수 있다.
    // erase 된 객체에 이쪽 스레드에서 send 를 날리면 안됨.
    // 이걸로 락 걸기는 좀 그렇다. 근데 임의 객체에 기상 정보를 날린다고 해도 어짜피 변조 의미가 없는 정보니 상관 없을수도 있음.
    // 아무튼 주의해야함.

    forecast_packet.Init();
    CURL *curl;
    CURLcode res;

    curl = curl_easy_init();

    std::string data;
    Json::Reader reader;
    Json::Value json_value;

    while (true)
    {
        if (curl)
        {
            curl_easy_setopt(curl, CURLOPT_URL, "http://127.0.0.1:1337/forecast");
            res = curl_easy_perform(curl);

            curl_easy_setopt(curl, CURLOPT_WRITEFUNCTION, write_html);
            curl_easy_setopt(curl, CURLOPT_WRITEDATA, &data);

            reader.parse(data, json_value);
        }

        int len = json_value.size();

        for (int i=0;i<10;++i)
        {
            if (i >= len)
            {
                forecast_packet.value[i] = 0;
            }
            else
            {
                forecast_packet.value[i] = json_value[i]["value"].asFloat();
            }
        }

        int forecast_val = json_value[1]["value"].asInt();

        if (forecast_refresh)
        {
            switch(forecast_val)
            {
            case 1:
                current_forecast = Forecast::Rain;
                break;
            case 2:
                current_forecast = Forecast::Snow;
                break;
            default:
                current_forecast = Forecast::Sunny;
                break;
            }
        }

        NotifyForecastInfo();
        boost::this_thread::sleep(boost::posix_time::milliseconds(30000));
    }

    curl_easy_cleanup(curl);
}

void WorldServer::Start()
{
    BaseServer::Start();

    forecastThread = boost::thread(boost::bind(&WorldServer::UpdateForecastInfoThread, this));
    playdataUpdateThread = boost::thread(boost::bind(&WorldServer::UpdatePlayDataThread, this));
}

void WorldServer::SetForecastToSnow()
{
    current_forecast = Forecast::Snow;
    forecast_refresh = false;
    NotifyForecastInfo();
}

void WorldServer::SetForecastToRain()
{
    current_forecast = Forecast::Rain;
    forecast_refresh = false;
    NotifyForecastInfo();
}

void WorldServer::SetForecastToSunny()
{
    current_forecast = Forecast::Sunny;
    forecast_refresh = false;
    NotifyForecastInfo();
}

void WorldServer::SetForecastToUpdating()
{
    forecast_refresh = true;
}

void WorldServer::InitMapData()
{
    float track_start_point[8][4] = {
        { 95.0f, -10.0f, 58.32f, 180.0f },
        { 90.0f, -5.0f, 57.65f, 180.0f },
        { 85.0f, 0.0f, 57.34f, 180.0f },
        { 80.0f, 5.0f, 57.17f, 180.0f },
        { 75.0f, 10.0f, 57.17f, 180.0f },
        { 70.0f, 15.0f, 57.29f, 180.0f },
        { 65.0f, 20.0f, 57.91f, 180.0f },
        { 60.0f, 25.0f, 58.18f, 180.0f },
    };

    float hangang_start_point[8][4] = {
        { 0.4f, 16.05f, 10.12f, -8.72f},
        { -1.31f, 15.78f, 10.12f, -8.72f },
        { -3.21f, 15.48f, 10.12f, -8.72f },
        { -5.28f, 15.17f, 10.12f, -8.72f },
        { 0.87f, 13.53f, 10.12f, -8.72f },
        { -0.92f, 13.26f, 10.12f, -8.72f },
        { -2.82f, 12.96f, 10.12f, -8.72f },
        { -4.89f, 12.65f, 10.12f, -8.72f },

    };

    TrackMapData* hangang = new TrackMapData(100, 1, hangang_start_point);
    TrackMapData* track = new TrackMapData(101, 2, track_start_point);

    track_datas[100] = hangang;
    track_datas[101] = track;
}


