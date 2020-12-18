#pragma once

#include <string>
#include <deque>
#include <unordered_map>
#include <boost/thread.hpp>

#include "../GameServer/BaseServer.h"
#include "../Common/commonpacket.h"
#include "../Common/GameRoomPacket.h"
#include "../Common/TrackGamePacket.h"

#include "Forecast.h"
#include "GameWorld.h"
#include "GameRoom.h"
#include "TrackMapData.h"

const int MAX_GAMEROOM_SIZE = 999;

class WorldServer : public BaseServer
{
private:
    boost::thread forecastThread;
    boost::thread playdataUpdateThread;
    Forecast current_forecast;
    bool forecast_refresh;
    
    std::deque<int> gameroom_ids;
    std::unordered_map<int, int> session_ids;
    std::unordered_map<short, GameRoom* > gamerooms;
    std::unordered_map<int, TrackMapData*> track_datas;

    GameWorld *gameWorld;
    CommonPacket::PACKET_RES_CHANNEL_INFO_S2S channel_info_packet;
    CommonPacket::PACKET_NTF_FORECAST_INFO forecast_packet;

public:
    WorldServer(const int port, const int maxCount, boost::asio::io_service &io_service);
    ~WorldServer();

    virtual void ProcessPacket(const int sessionId, char* data);
    void UpdateForecastInfoThread();
    void UpdatePlayDataThread();

    void NotifyToMySector(User* user, PACKET_HEADER packet);
    void NotifyForecastInfo();

    void Start();
    void SetForecastToSnow();
    void SetForecastToRain();
    void SetForecastToSunny();
    void SetForecastToUpdating();

    void InitMapData();
    void AddGameRoomNumberId(int id) { gameroom_ids.push_back(id); }
    GameWorld* GetWorldPtr() { return gameWorld; }
    TrackMapData* GetTrackData(int map_id) { return track_datas[map_id]; }
};

