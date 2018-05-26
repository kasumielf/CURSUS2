#include "GameRoom.h"
#include "WorldServer.h"

#include <map>

int GameRoom::GetEmptyUserSlotIndex()
{
	for (int i = 0; i < MAX_ROOM_PLAYER; ++i)
	{
		if (players[i] == nullptr && closed[i] != true)
			return i;
	}

	return -1;
}

int GameRoom::GetUserSlotIndex(User * user)
{
	for (int i = 0; i < MAX_ROOM_PLAYER; ++i)
	{
		if (players[i] == user)
			return i;
	}

	return -1;
}

GameRoom::GameRoom(WorldServer* _server, User* host, int map, int room_id) : server(_server), player_count(0), host_index(0), map_id(map), room_index(room_id)
{
	players[0] = host;
	
	for (int i = 0; i < MAX_ROOM_PLAYER; ++i)
	{
		players[i] = nullptr;
		ready[i] = true;
		closed[i] = false;
	}

	playing = false;
	is_game_start = false;
	is_game_end = false;

	finished_track_count = server->GetTrackData(map)->GetFinishTrackCount();
}


GameRoom::~GameRoom()
{
	gameUpdateThread.join();

	for each(std::pair<int, AI*> v in ais)
	{
		delete v.second;
	}
}

bool GameRoom::SetHost(int user_position)
{
	if (players[user_position] == nullptr)
	{
		return false;
	}
	else
	{
		int old_host = host_index;

		host_index = user_position;

		ready[old_host] = false;
		ready[host_index] = true;

		GameRoomPacket::PACKET_NOTIFY_PLAYER_SET_HOST notify;

		notify.Init();

		notify.room_id = room_index;
		notify.player_index = host_index;
		notify.player_id = players[host_index]->getUserUid();

		char* char_not = (char*)(&notify);

		for (int i = 0; i < MAX_ROOM_PLAYER; ++i)
		{
			if (players[i] != nullptr)
				server->SendData(players[i]->getSessionId(), notify.Size, char_not);
		}

		return true;
	}

	return false;
}
void GameRoom::AddAI(int position)
{
	AI* ai = new AI();

	ais[position] = ai;
}

void GameRoom::SetReady(User* user)
{
	int pindex = GetUserSlotIndex(user);

	if (pindex >= 0)
	{
		ready[pindex] = !ready[pindex];

		for (int i = 0; i < MAX_ROOM_PLAYER; ++i)
		{
			if (players[i] != nullptr)
			{
				GameRoomPacket::PACKET_NOTIFY_PLAYER_READY notify;

				notify.Init();
				notify.player_index = pindex;
				notify.room_id = room_index;
				notify.ready = ready[notify.player_index];
				notify.user_uid = user->getUserUid();

				char* char_not = (char*)(&notify);

				server->SendData(players[i]->getSessionId(), notify.Size, char_not);
			}
		}
	}
}

void GameRoom::PlayerExit(User* user)
{
	int player_index = GetUserSlotIndex(user);
	
	if (player_index >= 0)
	{
		GameRoomPacket::PACKET_NOTIFY_PLAYER_EXIT notify;

		notify.Init();

		notify.player_index = player_index;
		notify.room_id = room_index;
		notify.user_uid = user->getUserUid();

		players[player_index] = nullptr;
		ready[player_index] = false;

		player_count--;

		char* char_not = (char*)(&notify);

		for (int i = 0; i < MAX_ROOM_PLAYER; ++i)
		{
			if (players[i] != nullptr && i != player_index)
				server->SendData(players[i]->getSessionId(), notify.Size, char_not);
		}
	}
}

void GameRoom::PlayerEnter(int sessionId, User* user)
{
	GameRoomPacket::PACKET_RES_ROOM_PLAYER_ENTER res;
	GameRoomPacket::PACKET_NOTIFY_PLAYER_ENTER notify;

	res.Init();
	res.result = 0;
	int index = GetEmptyUserSlotIndex();

	if (index >= 0 && IsExistPlayer(user) == false)
	{
		res.room_id = room_index;
		res.map_id = map_id;
		res.result = 1;
		res.player_index = index;

		ready[index] = false;

		players[index] = user;
		player_count++;

		notify.Init();
		notify.player_index = index;
		notify.room_id = room_index;
		notify.user_uid = user->getUserUid();
		strcpy_s(notify.username, user->getUsername());
		char* not_char = (char*)(&notify);

		GameRoomPacket::PACKET_NOTIFY_ROOM_SET_TYPE not_type;
		not_type.Init();
		not_type.room_id = room_index;

		for (int i = 0; i < MAX_ROOM_PLAYER; ++i)
		{
			if (players[i] != nullptr)
			{
				if (i != index)
				{
					server->SendData(players[i]->getSessionId(), notify.Size, not_char);
				
					GameRoomPacket::PACKET_NOTIFY_PLAYER_ENTER notify_to_entered_player;

					notify_to_entered_player.Init();
					notify_to_entered_player.player_index = i;
					notify_to_entered_player.room_id = room_index;
					notify_to_entered_player.user_uid = players[i]->getUserUid();
					strcpy_s(notify_to_entered_player.username, players[i]->getUsername());

					char* ntf = (char*)(&notify_to_entered_player);

					PACKET_DUMMY dummy;
					dummy.Init();
					server->SendData(user->getSessionId(), notify_to_entered_player.Size, ntf);
					server->SendData(user->getSessionId(), dummy.Size, reinterpret_cast<char*>(&dummy));
				}
			}
			
			if (closed[i] == true)
			{
				not_type.type = 2;

				char* _ntf = (char*)(&not_type);
				server->SendData(user->getSessionId(), not_type.Size, _ntf);
			}

			if (ais[i] != nullptr)
			{
				not_type.type = 1;

				char* _ntf = (char*)(&not_type);
				server->SendData(user->getSessionId(), not_type.Size, _ntf);
			}
		}
	}

	server->SendData(sessionId, res.Size, (char*)(&res));
}

void GameRoom::AddUser(int sessionId, User * user)
{
	int index = GetEmptyUserSlotIndex();

	if (players[index] == nullptr)
	{
		players[index] = user;
		ready[index] = true;
	}
}

void GameRoom::PlayerRemove(int position)
{
	User* target = players[position];

	if (target != nullptr)
		PlayerExit(target);
}

void GameRoom::GameStart()
{
	GameRoomPacket::PACKET_RES_GAME_START res;
	res.Init();
	
	ready[host_index] = true;

	if (ready.all())
	{
		res.room_id = room_index;
		res.result = 1;

		for (int i = 0; i < MAX_ROOM_PLAYER; ++i)
		{
			if (players[i] != nullptr)
			{
				GameRoomPacket::PACKET_NOTIFY_GAME_START not;
				not.Init();
				not.room_id = room_index;
				not.map_id = map_id;

				players[i]->setDuringOnGame(true);

				Object* obj = new Object();

				obj->is_computer = false;
				strcpy_s(obj->name, players[i]->getUsername());
				obj->is_ready = false;
				               
				obj->x = server->GetTrackData(map_id)->GetStartPoint(i)[0];
				obj->y = server->GetTrackData(map_id)->GetStartPoint(i)[1];
				obj->z = server->GetTrackData(map_id)->GetStartPoint(i)[2];
				obj->r = server->GetTrackData(map_id)->GetStartPoint(i)[3];

				ingame_objects[i] = obj;

				not.start_x = obj->x;
				not.start_y = obj->y;
				not.start_z = obj->z;
				not.start_r = obj->r;

				char* char_not = (char*)(&not);

				server->SendData(players[i]->getSessionId(), not.Size, char_not);
			}
		}

		for each(std::pair<short, AI*> ai in ais)
		{
			if (ai.second != nullptr)
			{
				Object* obj = new Object();

				obj->is_computer = true;
				strcpy_s(obj->name, "Computer");
				obj->is_ready = true;

				obj->x = server->GetTrackData(map_id)->GetStartPoint(ai.first)[0];
				obj->y = server->GetTrackData(map_id)->GetStartPoint(ai.first)[1];
				obj->z = server->GetTrackData(map_id)->GetStartPoint(ai.first)[2];
				obj->r = server->GetTrackData(map_id)->GetStartPoint(ai.first)[3];

				ingame_objects[ai.first] = obj;
			}
		}

		playing = true;

		gameUpdateThread = boost::thread(boost::bind(&GameRoom::GameRoomUpdateThread, this));
	}
	else
	{
		res.result = 0;
	}

	server->SendData(players[host_index]->getSessionId(), res.Size, (char*)(&res));
}

void GameRoom::SetType(int position, char type)
{
	//	Open = 0,
	//	Computer = 1,
	//	Closed = 2,
	//	Player = 3,

	switch (type)
	{
		case 0:
		{
			PlayerRemove(position);

			if (ais[position] != nullptr)
			{
				delete ais[position];
			}

			closed[position] = false;
			ready[position] = false;
			break;
		}
		case 1:
		{
			AddAI(position);
			ready[position] = true;
			break;
		}
		case 2:
		{
			PlayerRemove(position);

			if (ais[position] != nullptr)
			{
				delete ais[position];
			}

			closed[position] = true;
			ready[position] = true;
			break;
		}
	}

	GameRoomPacket::PACKET_NOTIFY_ROOM_SET_TYPE not;
	not.Init();
	not.room_id = room_index;
	not.player_index = position;
	not.type = type;
	
	char* char_not = (char*)(&not);

	for (int i = 0; i < MAX_ROOM_PLAYER; ++i)
	{
		if (players[i] != nullptr)
		{
			server->SendData(players[i]->getSessionId(), not.Size, char_not);
		}
	}
}

bool GameRoom::IsExistPlayer(User * user)
{
	for (int i = 0; i < MAX_ROOM_PLAYER; ++i)
	{
		if (players[i] == user)
			return true;
	}

	return false;
}

void GameRoom::SendIngameObjects(int position, int sessionId)
{
	TrackScenePacket::PACKET_RES_INGAME_PLAYER_LIST res;
	res.Init();

	for each(std::pair<short, Object*> obj in ingame_objects)
	{
		if (obj.first != position)
		{
			if (ingame_objects[obj.first] != nullptr)
			{
				res.index[obj.first] = obj.first;
				res.pos_x[obj.first] = obj.second->x;
				res.pos_y[obj.first] = obj.second->y;
				res.pos_z[obj.first] = obj.second->z;
				res.r[obj.first] = obj.second->r;
				strcpy_s(res.username[obj.first], obj.second->name);
			}
		}
	}

	server->SendData(sessionId, res.Size, reinterpret_cast<char*>(&res));
}

void GameRoom::SetIngameReady(int position)
{
	ingame_objects[position]->is_ready = true;
}

void GameRoom::GameRoomUpdateThread()
{
	while (true)
	{
		bool ready = true;

		if (ingame_objects.empty() != false)
		{
			for each(std::pair<short, Object*> obj in ingame_objects)
			{
				ready &= obj.second->is_ready;
			}
		}

		boost::this_thread::sleep(boost::posix_time::milliseconds(1000));

		if (ready == true)
			break;
	}

	TrackScenePacket::PACKET_NOTIFY_READY_COUNT count_packet;
	count_packet.Init();
	char* chr_count_packet = reinterpret_cast<char*>(&count_packet);

	int count = 5;

	while (count >= 0)
	{
		if (ingame_objects.empty())
			return;

		for each(std::pair<short, Object*> obj in ingame_objects)
		{
			if (obj.second != nullptr && obj.second->is_computer == false)
				server->SendData(players[obj.first]->getSessionId(), count_packet.Size, chr_count_packet);
		}

		boost::this_thread::sleep(boost::posix_time::milliseconds(1000));
		count--;
	}

	TrackScenePacket::PACKET_NOTIFY_ROOM_GAME_START start_packet;
	start_packet.Init();
	start_packet.start_time = 0;
	char* chr_start_packet = reinterpret_cast<char*>(&start_packet);

	for each(std::pair<short, Object*> obj in ingame_objects)
	{
		if (obj.second != nullptr)
		{
			obj.second->start_time = std::chrono::high_resolution_clock::now();

			if (obj.second->is_computer == false)
			{
				server->SendData(players[obj.first]->getSessionId(), start_packet.Size, chr_start_packet);
			}
		}
	}

	is_game_start = true;

	std::bitset<8> track_end;
	
	for (int i = 0; i < 8; ++i)
	{
		if (ingame_objects.count(i) > 0)
			track_end[i] = false;
		else
			track_end[i] = true;
	}

	TrackScenePacket::PACKET_NOTIFY_ROOM_GAME_END end_packet;
	end_packet.Init();
	
	srand((unsigned int)time(NULL));

	while (is_game_start)
	{
		for each(std::pair<short, Object*> obj in ingame_objects)
		{
			if (obj.second != nullptr && obj.second->is_computer == false)
			{
				for each(std::pair<short, Object*> _obj in ingame_objects)
				{
					TrackScenePacket::PACKET_NOTIFY_ROOM_PLAYER_POSITION pos;
					pos.Init();
					pos.player_index = _obj.first;
					pos.v = _obj.second->v;
					char* data = reinterpret_cast<char*>(&pos);
					server->SendData(players[obj.first]->getSessionId(), pos.Size, data);
				}

				if (track_end.all())
				{
					is_game_start = false;
				}
				track_end[obj.first] = obj.second->track_count >= finished_track_count;
			}
			else
			{
				// 급한대로 하느라 stl random 안쓰고 걍함...
				float s = 15.0f + (rand() % 10);
				obj.second->v = s;
				obj.second->distance += s;

				if (obj.second->distance >= 1700.0f)
				{
					obj.second->track_count++;
					obj.second->distance = 0.0f;

					if (obj.second->track_count >= finished_track_count)
					{
						auto now = std::chrono::high_resolution_clock::now();
						obj.second->check_time = now;

						track_end[obj.first] = obj.second->track_count >= finished_track_count;
						obj.second->v = 0.0f;
					}
				}
			}
		}

		boost::this_thread::sleep(boost::posix_time::milliseconds(500));
	}
	
	std::map<double, short> ordered_result;

	for each(std::pair<short, Object*> obj in ingame_objects)
	{
		auto v = std::chrono::duration<double, std::milli>(obj.second->check_time - obj.second->start_time);

		ordered_result.insert({v.count(), obj.first});
	}

	int c = 0;
	
	for each(std::pair<double, short> rs in ordered_result)
	{
		end_packet.player_index[c] = rs.second;
		end_packet.records[c] = rs.first;

		if (++c >= 3)
			break;
	}

	is_game_end = true;
	char *chr_end_packet = reinterpret_cast<char*>(&end_packet);

	for each(std::pair<short, Object*> obj in ingame_objects)
	{
		if (obj.second != nullptr && obj.second->is_computer == false)
			server->SendData(players[obj.first]->getSessionId(), end_packet.Size, chr_end_packet);
	}
}

void GameRoom::SetPosition(short position, float x, float y, float z, float v, float r)
{
	if (ingame_objects[position] != nullptr && is_game_start == true)
	{
		ingame_objects[position]->x = x;
		ingame_objects[position]->y = y;
		ingame_objects[position]->z = z;
		ingame_objects[position]->v = v;
		ingame_objects[position]->r = r;
	}
}

void GameRoom::UpdateTrackCount(short position)
{
	ingame_objects[position]->track_count++;

	short track_count = ingame_objects[position]->track_count;

	auto now = std::chrono::high_resolution_clock::now();
	auto v = std::chrono::duration<double, std::milli>(now - ingame_objects[position]->start_time);

	ingame_objects[position]->check_time = now;

	TrackScenePacket::PACKET_NOTIFY_UPDATE_TRACK_COUNT notify;

	char finished = track_count >= finished_track_count ? 1 : 0;
	
	notify.Init();
	notify.player_index = position;
	notify.track_count = track_count;
	notify.is_finished = finished;
	notify.checked_time = v.count();
	char* char_not = reinterpret_cast<char*>(&notify);

	std::cout << "Player " << position << " passed and counted in " << track_count << " ended : " << finished << std::endl;

	for each(std::pair<int, Object*> obj in ingame_objects)
	{
		if (obj.second != nullptr && obj.second->is_computer == false)
		{
			server->SendData(players[obj.first]->getSessionId(), notify.Size, char_not);
		}
	}
}
