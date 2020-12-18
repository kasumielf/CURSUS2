#pragma once
#include "../Common/User.h"
#include "../Common/GameRoomPacket.h"
#include "../Common/TrackGamePacket.h"
#include <array>
#include <vector>
#include <bitset>
#include <unordered_map>
#include <chrono>
#include <boost/thread.hpp>

class WorldServer;

struct AI
{
    int id;
    int level;
};

struct Object
{
    Object() : x(0.0f), y(0.0f), z(0.0f), v(0.0f), r(0.0f), track_count(0), distance(0.0f)
    {
    }

    char name[12];
    bool is_computer;
    float x;
    float y;
    float z;
    float v;
    float r;
    bool is_ready;
    short track_count;
    std::chrono::high_resolution_clock::time_point start_time;
    std::chrono::high_resolution_clock::time_point check_time;
    float distance;
};


const int MAX_ROOM_PLAYER = 8;

class GameRoom
{
private:
    boost::thread gameUpdateThread;

private:
    WorldServer* server;

    int map_id;
    short room_index;
    short host_index;
    short player_count;
    bool playing;
    bool is_game_start;
    bool is_game_end;

    short finished_track_count;

    std::array<User*, 8> players;
    
    std::bitset<MAX_ROOM_PLAYER> ready;
    std::bitset<MAX_ROOM_PLAYER> closed;
    std::unordered_map<short, AI*> ais;
    std::unordered_map<short, Object*> ingame_objects;

private:
    int GetEmptyUserSlotIndex();
    int GetUserSlotIndex(User* user);

public:
    GameRoom(WorldServer* _server, User* host, int map, int room_id);
    ~GameRoom();

    bool SetHost(int user_position);
    void AddAI(int position);
    void SetReady(User* user);
    void PlayerExit(User* user);
    void PlayerEnter(int sessionId, User* user);
    void AddUser(int sessionId, User* user);
    void PlayerRemove(int position);
    void GameStart();
    void SetType(int position, char type);

    void SetMapId(int id) { map_id = id; }
    int GetPlayerCount() { return player_count; }
    int GetMapId() { return map_id; }

    bool IsGameEnd() { return is_game_end; }
    bool IsExistPlayer(User* user);
    void SendIngameObjects(int position, int sessionId);
    void SetIngameReady(int position);

    void GameRoomUpdateThread();
    void SetPosition(short position, float x, float y, float z, float v, float r);
    void UpdateTrackCount(short position);
};


