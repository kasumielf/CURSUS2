#pragma once
#pragma pack(push, 1)

#include <string>
#include "../GameServer/BasePacketDefine.h"

const short NOTIFY_GAME_START = 1000;
const short NOTIFY_PLAYER_ENTER = 1001;
const short NOTIFY_PLAYER_EXIT = 1002;
const short NOTIFY_PLAYER_READY = 1003;
const short NOTIFY_PLAYER_SET_HOST = 1004;
const short REQ_CREATE_ROOM_INFO = 1005;
const short RES_CREATE_ROOM_INFO = 1006;
const short REQ_ROOM_INFO = 1007;
const short RES_ROOM_INFO = 1008;
const short REQ_ROOM_PLAYER_READY = 1009;
const short REQ_ROOM_PLAYER_EXIT = 1010;
const short RES_ROOM_PLAYER_EXIT = 1011;
const short REQ_ROOM_PLAYER_ENTER = 1012;
const short RES_ROOM_PLAYER_ENTER = 1013;
const short REQ_GAME_START = 1014;
const short RES_GAME_START = 1015;
const short REQ_ROOM_PLAYERS = 1016;
const short RES_ROOM_PLAYERS = 1017;
const short REQ_ROOM_SET_TYPE = 1018;
const short RES_ROOM_SET_AI = 1019;
const short NOTIFY_ROOM_SET_AI = 1020;
const short REQ_ROOM_PLAYER_BAN = 1021;
const short RES_ROOM_PLAYER_BAN = 1022;

namespace GameRoomPacket
{
	struct PACKET_NOTIFY_GAME_START : public PACKET_HEADER
	{
		void Init()
		{
			Id = NOTIFY_GAME_START;
			Size = sizeof(PACKET_NOTIFY_GAME_START);
		}
		short room_id;
		float start_x;
		float start_y;
		float start_z;
		float start_r;
		int map_id;
	};

	struct PACKET_NOTIFY_PLAYER_ENTER : public PACKET_HEADER
	{
		void Init()
		{
			Id = NOTIFY_PLAYER_ENTER;
			Size = sizeof(PACKET_NOTIFY_PLAYER_ENTER);
		}

		short room_id;
		int user_uid;
		char username[12];
		short player_index;
	};

	struct PACKET_NOTIFY_PLAYER_EXIT : public PACKET_HEADER
	{
		void Init()
		{
			Id = NOTIFY_PLAYER_EXIT;
			Size = sizeof(PACKET_NOTIFY_PLAYER_EXIT);
		}

		short room_id;
		int user_uid;
		short player_index;
	};

	struct PACKET_NOTIFY_PLAYER_READY : public PACKET_HEADER
	{
		void Init()
		{
			Id = NOTIFY_PLAYER_READY;
			Size = sizeof(PACKET_NOTIFY_PLAYER_READY);
		}

		short room_id;
		int user_uid;
		short player_index;
		bool ready;
	};

	struct PACKET_NOTIFY_PLAYER_SET_HOST : public PACKET_HEADER
	{
		void Init()
		{
			Id = NOTIFY_PLAYER_SET_HOST;
			Size = sizeof(PACKET_NOTIFY_PLAYER_SET_HOST);
		}

		short room_id;
		int player_id;
		short player_index;
	};

	struct PACKET_REQ_CREATE_ROOM_INFO : public PACKET_HEADER
	{
		void Init()
		{
			Id = REQ_CREATE_ROOM_INFO;
			Size = sizeof(PACKET_REQ_CREATE_ROOM_INFO);
		}

		int map_id;
	};

	struct PACKET_RES_CREATE_ROOM_INFO : public PACKET_HEADER
	{
		void Init()
		{
			Id = RES_CREATE_ROOM_INFO;
			Size = sizeof(PACKET_RES_CREATE_ROOM_INFO);
		}

		short room_id;
		int map_id;
		char result;
	};

	struct PACKET_REQ_ROOM_INFO : public PACKET_HEADER
	{
		void Init()
		{
			Id = REQ_ROOM_INFO;
			Size = sizeof(PACKET_REQ_ROOM_INFO);
		}
	};

	struct PACKET_RES_ROOM_INFO : public PACKET_HEADER
	{
		void Init()
		{
			Id = RES_ROOM_INFO;
			Size = sizeof(PACKET_RES_ROOM_INFO);
		}

		short room_id;
		int map_id;
		short user_count;
	};


	struct PACKET_REQ_ROOM_PLAYER_READY : public PACKET_HEADER
	{
		void Init()
		{
			Id = REQ_ROOM_PLAYER_READY;
			Size = sizeof(PACKET_REQ_ROOM_PLAYER_READY);
		}

		short room_id;
		int user_uid;
		int room_index;
	};

	struct PACKET_REQ_ROOM_PLAYER_EXIT : public PACKET_HEADER
	{
		void Init()
		{
			Id = REQ_ROOM_PLAYER_EXIT;
			Size = sizeof(PACKET_REQ_ROOM_PLAYER_EXIT);
		}

		short room_id;
	};

	struct PACKET_REQ_ROOM_PLAYER_ENTER : public PACKET_HEADER
	{
		void Init()
		{
			Id = REQ_ROOM_PLAYER_ENTER;
			Size = sizeof(PACKET_REQ_ROOM_PLAYER_ENTER);
		}
		short room_id;
	};

	struct PACKET_RES_ROOM_PLAYER_ENTER : public PACKET_HEADER
	{
		void Init()
		{
			Id = RES_ROOM_PLAYER_ENTER;
			Size = sizeof(PACKET_RES_ROOM_PLAYER_ENTER);
		}
		short room_id;
		int map_id;
		char result;
		short player_index;
	};

	struct PACKET_REQ_ROOM_GAME_START : public PACKET_HEADER
	{
		void Init()
		{
			Id = REQ_GAME_START;
			Size = sizeof(PACKET_REQ_ROOM_GAME_START);
		}

		short room_id;
	};

	struct PACKET_RES_GAME_START : public PACKET_HEADER
	{
		void Init()
		{
			Id = RES_GAME_START;
			Size = sizeof(PACKET_RES_GAME_START);
		}
		short room_id;
		char result;
	};

	struct PACKET_REQ_ROOM_SET_TYPE : public PACKET_HEADER
	{
		void Init()
		{
			Id = REQ_ROOM_SET_TYPE;
			Size = sizeof(PACKET_REQ_ROOM_SET_TYPE);
		}

		short room_id;
		short player_index;
		char type;
	};

	struct PACKET_RES_ROOM_SET_TYPE : public PACKET_HEADER
	{
		void Init()
		{
			Id = RES_ROOM_SET_AI;
			Size = sizeof(PACKET_RES_ROOM_SET_TYPE);
		}

		char result;
	};

	struct PACKET_NOTIFY_ROOM_SET_TYPE : public PACKET_HEADER
	{
		void Init()
		{
			Id = NOTIFY_ROOM_SET_AI;
			Size = sizeof(PACKET_NOTIFY_ROOM_SET_TYPE);
		}

		short room_id;
		short player_index;
		char type;
	};

	struct PACKET_REQ_ROOM_PLAYER_BAN : public PACKET_HEADER
	{
		void Init()
		{
			Id = REQ_ROOM_PLAYER_BAN;
			Size = sizeof(PACKET_REQ_ROOM_PLAYER_BAN);
		}

		short room_id;
		short player_index;
	};

	struct PACKET_RES_ROOM_PLAYER_BAN : public PACKET_HEADER
	{
		void Init()
		{
			Id = RES_ROOM_PLAYER_BAN;
			Size = sizeof(PACKET_RES_ROOM_PLAYER_BAN);
		}

		char result;
	};
}

#pragma pack(pop)