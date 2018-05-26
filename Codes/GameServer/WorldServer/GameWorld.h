#pragma once

#include "../Common/User.h"
#include "../Common/Defines.h"
#include "Map.h"
#include <list>
#include <unordered_map>

class GameWorld
{
private:
	std::unordered_map<int, User*> users;
	Map* current_map;

public:
	GameWorld();
	~GameWorld();

	void Init();

	bool isExistUser(const int user_uid);
	void AddUser(const int session_id, User *user);
	void RemoveUser(const int session_id);
	void SetPosition(const int session_id, float x, float y, float z, float v, float r);
	const int GetCurrentPlayerCount(){ return (int)users.size(); }
	User* GetUserInfo(const int session_id) { return users[session_id]; }
	Map* GetCurrentMap() { return current_map; }

	std::unordered_map<int, User*>::iterator getPlayersBegin() { return users.begin(); }
	std::unordered_map<int, User*>::iterator getPlayersEnd() { return users.end(); }

	bool isNear(const int from, const int to);
};

