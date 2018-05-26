#pragma once
#pragma once
#pragma pack(push, 1)

#include <string>
#include "../GameServer/BasePacketDefine.h"

const short REQ_INGAME_PLAYER_LIST = 2000;
const short RES_INGAME_PLAYER_LIST = 2001;
const short REQ_ROOM_UPDATE_PLAYER_POSITION = 2002;
const short NOTIFY_ROOM_PLAYER_POSITION = 2003;

const short REQ_INGAME_READY = 2004;
const short NOTIFY_INGAME_READY = 2005;
const short REQ_UPDATE_TRACK_COUNT = 2006;
const short NOTIFY_UPDATE_TRACK_COUNT = 2007;

const short NOTIFY_PLAYER_TRACK_FINISHED = 2008;

const short NOTIFY_ROOM_GAME_START = 2100;
const short NOTIFY_ROOM_GAME_END = 2101;
const short NOTIFY_READY_COUNT = 2200;

const short NOTIFY_GAME_RESULT = 2201;

namespace TrackScenePacket
{
	struct PACKET_REQ_INGAME_PLAYER_LIST : public PACKET_HEADER
	{
		void Init()
		{
			Id = REQ_INGAME_PLAYER_LIST;
			Size = sizeof(PACKET_REQ_INGAME_PLAYER_LIST);
		}

		short room_id;
		short player_index;
	};

	struct PACKET_RES_INGAME_PLAYER_LIST : public PACKET_HEADER
	{
		void Init()
		{
			Id = RES_INGAME_PLAYER_LIST;
			Size = sizeof(PACKET_RES_INGAME_PLAYER_LIST);
		}

		short index[8];
		char username[8][12];
		float pos_x[8];
		float pos_y[8];
		float pos_z[8];
		float r[8];
	};


	struct PACKET_REQ_ROOM_UPDATE_PLAYER_POSITION : public PACKET_HEADER
	{
		void Init()
		{
			Id = REQ_ROOM_UPDATE_PLAYER_POSITION;
			Size = sizeof(PACKET_REQ_ROOM_UPDATE_PLAYER_POSITION);
		}

		short room_id;
		short player_index;
		float x;
		float y;
		float z;
		float v;
		float r;
	};


	struct PACKET_NOTIFY_ROOM_PLAYER_POSITION : public PACKET_HEADER
	{
		void Init()
		{
			Id = NOTIFY_ROOM_PLAYER_POSITION;
			Size = sizeof(PACKET_NOTIFY_ROOM_PLAYER_POSITION);
		}

		short player_index;
		float x;
		float y;
		float z;
		float v;
		float r;
	};

	struct PACKET_REQ_INGAME_READY : public PACKET_HEADER
	{
		void Init()
		{
			Id = REQ_INGAME_READY;
			Size = sizeof(PACKET_REQ_INGAME_READY);
		}

		short room_id;
		short player_index;
	};

	struct PACKET_NOTIFY_INGAME_READY : public PACKET_HEADER
	{
		void Init()
		{
			Id = NOTIFY_INGAME_READY;
			Size = sizeof(PACKET_NOTIFY_INGAME_READY);
		}

		short player_index;
	};

	struct PACKET_NOTIFY_ROOM_GAME_START : public PACKET_HEADER
	{
		void Init()
		{
			Id = NOTIFY_ROOM_GAME_START;
			Size = sizeof(PACKET_NOTIFY_ROOM_GAME_START);
		}

		double start_time;
	};

	struct PACKET_NOTIFY_ROOM_GAME_END : public PACKET_HEADER
	{
		void Init()
		{
			Id = NOTIFY_ROOM_GAME_END;
			Size = sizeof(PACKET_NOTIFY_ROOM_GAME_END);
		}

		short player_index[3] = { -1, -1, -1 };
		double records[3];
	};

	struct PACKET_REQ_UPDATE_TRACK_COUNT : PACKET_HEADER
	{
		void Init()
		{
			Id = REQ_UPDATE_TRACK_COUNT;
			Size = sizeof(PACKET_REQ_UPDATE_TRACK_COUNT);
		}
		short room_id;
		short player_index;
	};

	struct PACKET_NOTIFY_UPDATE_TRACK_COUNT : PACKET_HEADER
	{
		void Init()
		{
			Id = NOTIFY_UPDATE_TRACK_COUNT;
			Size = sizeof(PACKET_NOTIFY_UPDATE_TRACK_COUNT);
		}
		short player_index;
		short track_count;
		double checked_time;
		char is_finished;
	};

	struct PACKET_NOTIFY_PLAYER_TRACK_FINISHED : PACKET_HEADER
	{
		void Init()
		{
			Id = NOTIFY_PLAYER_TRACK_FINISHED;
			Size = sizeof(PACKET_NOTIFY_PLAYER_TRACK_FINISHED);
		}
		short player_index;
		double finished_time;
	};

	struct PACKET_NOTIFY_READY_COUNT : PACKET_HEADER
	{
		void Init()
		{
			Id = NOTIFY_READY_COUNT;
			Size = sizeof(PACKET_NOTIFY_READY_COUNT);
		}
	};

	struct PACKET_NOTIFY_GAME_RESULT : PACKET_HEADER
	{
		void Init()
		{
			Id = NOTIFY_GAME_RESULT;
			Size = sizeof(PACKET_NOTIFY_GAME_RESULT);
		}

		double finished_time[8];
	};

}
#pragma pack(pop)