#pragma once
#pragma pack(push, 1)

#include <string>
#include "../GameServer/BasePacketDefine.h"

const short REQ_LOGIN = 100;
const short RES_LOGIN = 101;
const short REQ_CREATE_ACCOUNT = 102;
const short RES_CREATE_ACCOUNT = 103;
const short RES_USER_INFO = 201;
const short REQ_USER_INFO = 202;
const short RES_CREATE_ACCOUNT_S2S = 104;
const short REQ_CREATE_ACCOUNT_S2S = 105;
const short REQ_LOGIN_S2S = 106;
const short RES_LOGIN_S2S = 107;

const short REQ_CHANNEL_LIST = 201;
const short RES_CHANNEL_LIST = 202;
const short REQ_ENTER_CHANNEL = 203;
const short RES_ENTER_CHANNEL = 204;
const short REQ_CHANNEL_INFO_S2S = 210;
const short RES_CHANNEL_INFO_S2S = 211;

const short NTF_FORECAST_INFO = 300;
const short NTF_PLAYER_ENTER = 400;
const short NTF_PLAYER_EXIT = 401;
const short NTF_PLAYER_MOVE = 402;
const short REQ_PLAYER_MOVE = 403;
const short RES_PLAYER_MOVE = 404;
const short REQ_PLAYER_ENTER = 405;
const short RES_PLAYER_ENTER = 406;
const short REQ_PLAYER_EXIT = 407;
const short RES_PLAYER_EXIT = 408;
const short REQ_PLAYER_DATA_UPDATE_S2S = 409;
const short RES_PLAYER_DATA_UPDATE_S2S = 410;

const short REQ_UPDATE_RECORD_DATA = 500;
const short RES_UPDATE_RECORD_DATA = 501;
const short REQ_GET_RECORD_DATA = 502;
const short RES_GET_RECORD_DATA = 503;
const short REQ_RANKUSER_RECORD_DATA = 504;
const short RES_RANKUSER_RECORD_DATA = 505;

const short REQ_UPDATE_RECORD_DATA_S2S = 510;
const short RES_UPDATE_RECORD_DATA_S2S = 511;
const short REQ_GET_RECORD_DATA_S2S = 512;
const short RES_GET_RECORD_DATA_S2S = 513;
const short REQ_RANKUSER_RECORD_DATA_S2S = 514;
const short RES_RANKUSER_RECORD_DATA_S2S = 515;

const short REQ_GET_REPLAY_RECORD_DATA = 520;
const short REQ_UPDATE_REPLAY_RECORD_DATA = 521;
const short RES_GET_REPLY_RECORD_DATA = 522;
const short REQ_GET_REPLAY_RECORD_DATA_S2S = 530;
const short RES_GET_REPLAY_RECORD_DATA_S2S = 531;
const short REQ_UPDATE_REPLAY_RECORD_DATA_S2S = 532;
const short RES_UPDATE_REPLAY_RECORD_DATA_S2S = 533;
const short RES_UPDATE_REPLY_RECORD_DATA = 534;

namespace CommonPacket
{
	struct PACEKT_REQ_CREATE_ACCOUNT : public PACKET_HEADER
	{
		void Init()
		{
			Id = REQ_CREATE_ACCOUNT;
			Size = sizeof(PACEKT_REQ_CREATE_ACCOUNT);
		}

		char login_id[12];
		char password[12];
		char username[12];

		char weight;
		int birthday;
		bool gender;
	};

	struct PACKET_RES_CREATE_ACCOUNT : public PACKET_HEADER
	{
		void Init()
		{
			Id = RES_CREATE_ACCOUNT;
			Size = sizeof(PACKET_RES_CREATE_ACCOUNT);
		}

		short result;
	};

	struct PACKET_REQ_LOGIN : public PACKET_HEADER
	{
		void Init()
		{
			Id = REQ_LOGIN;
			Size = sizeof(PACKET_REQ_LOGIN);
		}

		char login_id[12];
		char password[12];
	};

	struct PACKET_RES_LOGIN : public PACKET_HEADER
	{
		void Init()
		{
			Id = RES_LOGIN;
			Size = sizeof(PACKET_RES_LOGIN);
		}

		short result;
		char lobby_ip[15];
		short port;
	};

	struct PACKET_REQ_USER_INFO : public PACKET_HEADER
	{
		void Init()
		{
			Id = REQ_USER_INFO;
			Size = sizeof(PACKET_REQ_USER_INFO);
		}

		int session_id;

		char login_id[12];
		char password[12];
	};

	struct PACKET_RES_USER_INFO : public PACKET_HEADER
	{
		void Init()
		{
			Id = RES_USER_INFO;
			Size = sizeof(PACKET_RES_USER_INFO);
		}

		int session_id;
		int user_uid;
		char id[12];
		char username[12];
		char weight;
		char gender;
		unsigned int birthday;
		float x;
		float y;
		float z;
		int current_map;
	};

	struct PACKET_REQ_CREATE_ACCOUNT_S2S : public PACKET_HEADER
	{
		void Init()
		{
			Id = REQ_CREATE_ACCOUNT_S2S;
			Size = sizeof(PACKET_REQ_CREATE_ACCOUNT_S2S);
		}

		int session_id;
		char login_id[12];
		char password[12];
		char username[12];
		char weight;
		int birthday;
		bool gender;
	};

	struct PACKET_RES_CREATE_ACOUNT_S2S : public PACKET_HEADER
	{
		void Init()
		{
			Id = RES_CREATE_ACCOUNT_S2S;
			Size = sizeof(PACKET_RES_CREATE_ACOUNT_S2S);
		}

		int session_id;
		int result;
	};

	struct PACKET_REQ_LOGIN_S2S : public PACKET_HEADER
	{
		void Init()
		{
			Id = REQ_LOGIN_S2S;
			Size = sizeof(PACKET_REQ_LOGIN_S2S);
		}

		int session_id;
		char login_id[12];
		char password[12];
	};

	struct PACKET_RES_LOGIN_S2S : public PACKET_HEADER
	{
		void Init()
		{
			Id = RES_LOGIN_S2S;
			Size = sizeof(PACKET_RES_LOGIN_S2S);
		}
		int session_id;
		short result;
	};


	struct PACKET_REQ_CHANNEL_LIST : public PACKET_HEADER
	{
		void Init()
		{
			Id = REQ_CHANNEL_LIST;
			Size = sizeof(PACKET_REQ_CHANNEL_LIST);
		}
	};

	struct PACKET_RES_CHANNEL_LIST : public PACKET_HEADER
	{
		void Init()
		{
			Id = RES_CHANNEL_LIST;
			Size = sizeof(PACKET_RES_CHANNEL_LIST);
		}

		char channel_name[2][12];
		char channel_id[2];
		short channel_user_count[2];
	};

	struct PACKET_REQ_ENTER_CHANNEL : public PACKET_HEADER
	{
		void Init()
		{
			Id = REQ_ENTER_CHANNEL;
			Size = sizeof(PACKET_REQ_ENTER_CHANNEL);
		}

		char selected_channel_id;
	};

	struct PACKET_RES_ENTER_CHANNEL : public PACKET_HEADER
	{
		void Init()
		{
			Id = RES_ENTER_CHANNEL;
			Size = sizeof(PACKET_RES_ENTER_CHANNEL);
		}

		short result;
		char channel_ip[15];
		short port;
	};

	struct PACEKT_REQ_CHANNEL_INFO_S2S : public PACKET_HEADER
	{
		void Init()
		{
			Id = REQ_CHANNEL_INFO_S2S;
			Size = sizeof(PACEKT_REQ_CHANNEL_INFO_S2S);
		}

		short channel_index;
	};

	struct PACKET_RES_CHANNEL_INFO_S2S : public PACKET_HEADER
	{
		void Init()
		{
			Id = RES_CHANNEL_INFO_S2S;
			Size = sizeof(PACKET_RES_CHANNEL_INFO_S2S);
		}

		short channel_index;
		short capacity;
		char status;
	};

	struct PACKET_NTF_FORECAST_INFO : public PACKET_HEADER
	{
		void Init()
		{
			Id = NTF_FORECAST_INFO;
			Size = sizeof(PACKET_NTF_FORECAST_INFO);
		}

		//char wtype[10][3];
		float value[10];
		char forecast;
	};

	struct PACKET_NTF_PLAYER_ENTER : public PACKET_HEADER
	{
		void Init()
		{
			Id = NTF_PLAYER_ENTER;
			Size = sizeof(PACKET_NTF_PLAYER_ENTER);
		}

		int user_id;
		char username[12];
		float x;
		float y;
		float z;
	};

	struct PACKET_NTF_PLAYER_EXIT : public PACKET_HEADER
	{
		void Init()
		{
			Id = NTF_PLAYER_EXIT;
			Size = sizeof(PACKET_NTF_PLAYER_EXIT);
		}

		int user_id;
	};

	struct PACKET_NTF_PLAYER_MOVE : public PACKET_HEADER
	{
		void Init()
		{
			Id = NTF_PLAYER_MOVE;
			Size = sizeof(PACKET_NTF_PLAYER_MOVE);
		}
		int user_id;
		float x;
		float y;
		float z;
		float v;
	};

	struct PACKET_REQ_PLAYER_MOVE : public PACKET_HEADER
	{
		void Init()
		{
			Id = REQ_PLAYER_MOVE;
			Size = sizeof(PACKET_REQ_PLAYER_MOVE);
		}

		float x;
		float y;
		float z;
		float r;
		float v;
	};

	struct PACKET_REQ_PLAYER_WORLD_ENTER : public PACKET_HEADER
	{
		void Init()
		{
			Id = REQ_PLAYER_ENTER;
			Size = sizeof(PACKET_REQ_PLAYER_WORLD_ENTER);
		}

		char login_id[12];
		char password[12];
	};

	struct PACKET_RES_PLAYER_ENTER : public PACKET_HEADER
	{
		void Init()
		{
			Id = RES_PLAYER_ENTER;
			Size = sizeof(PACKET_RES_PLAYER_ENTER);
		}

		int userUid;
		char id[12];
		char username[12];
		char weight;
		char gender;
		int current_map;
	};

	struct PACKET_REQ_PLAYER_EXIT : public PACKET_HEADER
	{
		void Init()
		{
			Id = REQ_PLAYER_EXIT;
			Size = sizeof(PACKET_REQ_PLAYER_EXIT);
		}
	};

	struct PACKET_RES_PLAYER_EXIT : public PACKET_HEADER
	{
		void Init()
		{
			Id = RES_PLAYER_EXIT;
			Size = sizeof(PACKET_RES_PLAYER_EXIT);
		}

		short result;
	};

	struct PACKET_REQ_PLAYER_DATA_UPDATE_S2S : public PACKET_HEADER
	{
		void Init()
		{
			Id = REQ_PLAYER_DATA_UPDATE_S2S;
			Size = sizeof(PACKET_REQ_PLAYER_DATA_UPDATE_S2S);
		}

		int userUid;
		float x;
		float y;
		int map_id;
	};

	struct PACKET_RES_PLAYER_DATA_UPDATE_S2S : public PACKET_HEADER
	{
		void Init()
		{
			Id = RES_PLAYER_DATA_UPDATE_S2S;
			Size = sizeof(PACKET_RES_PLAYER_DATA_UPDATE_S2S);
		}

		short result;
	};

	struct PACKET_REQ_UPDATE_RECORD_DATA : public PACKET_HEADER
	{
		void Init()
		{
			Id = REQ_UPDATE_RECORD_DATA;
			Size = sizeof(PACKET_REQ_UPDATE_RECORD_DATA);
		}
		int user_uid;
		double record_time;
		double checked_time;
	};

	struct PACKET_RES_UPDATE_RECORD_DATA : public PACKET_HEADER
	{
		void Init()
		{
			Id = RES_UPDATE_RECORD_DATA;
			Size = sizeof(PACKET_RES_UPDATE_RECORD_DATA);
		}

		int result;
	};

	struct PACKET_REQ_GET_RECORD_DATA : public PACKET_HEADER
	{
		void Init()
		{
			Id = REQ_GET_RECORD_DATA;
			Size = sizeof(PACKET_REQ_GET_RECORD_DATA);
		}

		int user_uid;
	};

	struct PACKET_RES_GET_RECORD_DATA : public PACKET_HEADER
	{
		void Init()
		{
			Id = RES_GET_RECORD_DATA;
			Size = sizeof(PACKET_RES_GET_RECORD_DATA);
		}

		double record_time;
		double checked_time;

	};

	struct PACKET_REQ_RANKUSER_RECORD_DATA : public PACKET_HEADER
	{
		void Init()
		{
			Id = REQ_RANKUSER_RECORD_DATA;
			Size = sizeof(PACKET_REQ_RANKUSER_RECORD_DATA);
		}
	};

	struct PACKET_RES_RANKUSER_RECORD_DATA : public PACKET_HEADER
	{
		void Init()
		{
			Id = RES_RANKUSER_RECORD_DATA;
			Size = sizeof(PACKET_RES_RANKUSER_RECORD_DATA);
		}

		int item_count;
		char username[5][12];
		double record_time[5];
		double checked_time[5];
	};

	struct PACKET_REQ_UPDATE_RECORD_DATA_S2S : public PACKET_HEADER
	{
		void Init()
		{
			Id = REQ_UPDATE_RECORD_DATA_S2S;
			Size = sizeof(PACKET_REQ_UPDATE_RECORD_DATA_S2S);
		}
		int user_uid;
		double record_time;
		double checked_time;

		int session_id;

	};

	struct PACKET_RES_UPDATE_RECORD_DATA_S2S : public PACKET_HEADER
	{
		void Init()
		{
			Id = RES_UPDATE_RECORD_DATA_S2S;
			Size = sizeof(PACKET_RES_UPDATE_RECORD_DATA_S2S);
		}

		int result;
		int session_id;

	};

	struct PACKET_REQ_GET_RECORD_DATA_S2S : public PACKET_HEADER
	{
		void Init()
		{
			Id = REQ_GET_RECORD_DATA_S2S;
			Size = sizeof(PACKET_REQ_GET_RECORD_DATA_S2S);
		}
		int user_uid;
		int session_id;
	};

	struct PACKET_RES_GET_RECORD_DATA_S2S : public PACKET_HEADER
	{
		void Init()
		{
			Id = RES_GET_RECORD_DATA_S2S;
			Size = sizeof(PACKET_RES_GET_RECORD_DATA_S2S);
		}

		double record_time;
		double checked_time;
		int session_id;
	};

	struct PACKET_REQ_RANKUSER_RECORD_DATA_S2S : public PACKET_HEADER
	{
		void Init()
		{
			Id = REQ_RANKUSER_RECORD_DATA_S2S;
			Size = sizeof(PACKET_REQ_RANKUSER_RECORD_DATA_S2S);
		}
		int session_id;
	};

	struct PACKET_RES_RANKUSER_RECORD_DATA_S2S : public PACKET_HEADER
	{
		void Init()
		{
			Id = RES_RANKUSER_RECORD_DATA_S2S;
			Size = sizeof(PACKET_RES_RANKUSER_RECORD_DATA_S2S);
		}

		char username[10][12];
		double record_time[10];
		double checked_time[10];
		int session_id;
		int item_count;
	};

	struct PACKET_REQ_GET_REPLAY_RECORD_DATA : public PACKET_HEADER
	{
		void Init()
		{
			Id = REQ_GET_REPLAY_RECORD_DATA;
			Size = sizeof(PACKET_RES_RANKUSER_RECORD_DATA);
		}

		int user_uid;
	};

	struct PACKET_REQ_UPDATE_REPLAY_RECORD_DATA : public PACKET_HEADER
	{
		void Init()
		{
			Id = REQ_UPDATE_REPLAY_RECORD_DATA;
			Size = sizeof(PACKET_REQ_UPDATE_REPLAY_RECORD_DATA);
		}

		int user_uid;
		unsigned char records[120];
	};

	struct PACKET_RES_GET_REPLAY_RECORD_DATA : public PACKET_HEADER
	{
		void Init()
		{
			Id = RES_GET_REPLY_RECORD_DATA;
			Size = sizeof(PACKET_RES_GET_REPLAY_RECORD_DATA);
		}

		int index;
		char records[120];
		double record_time;
	};

	struct PACKET_RES_UPDATE_REPLAY_RECORD_DATA : public PACKET_HEADER
	{
		void Init()
		{
			Id = RES_UPDATE_REPLY_RECORD_DATA;
			Size = sizeof(PACKET_RES_UPDATE_REPLAY_RECORD_DATA);
		}

		int result;
	};

	struct PACKET_REQ_GET_REPLAY_RECORD_DATA_S2S : public PACKET_HEADER
	{
		void Init()
		{
			Id = REQ_GET_REPLAY_RECORD_DATA_S2S;
			Size = sizeof(PACKET_REQ_GET_REPLAY_RECORD_DATA_S2S);
		}

		int user_uid;
		int session_id;
	};

	struct PACKET_RES_GET_REPLAY_RECORD_DATA_S2S : public PACKET_HEADER
	{
		void Init()
		{
			Id = RES_GET_REPLAY_RECORD_DATA_S2S;
			Size = sizeof(PACKET_RES_GET_REPLAY_RECORD_DATA_S2S);
		}

		int user_uid;
		int session_id;
		int index;
		unsigned char records[120];
		long record_time;
	};

	struct PACKET_REQ_UPDATE_REPLAY_RECORD_DATA_S2S : public PACKET_HEADER
	{
		void Init()
		{
			Id = REQ_UPDATE_REPLAY_RECORD_DATA_S2S;
			Size = sizeof(PACKET_REQ_UPDATE_REPLAY_RECORD_DATA_S2S);
		}

		int user_uid;
		int session_id;
		unsigned char records[120];
	};

	struct PACKET_RES_UPDATE_REPLAY_RECORD_DATA_S2S : public PACKET_HEADER
	{
		void Init()
		{
			Id = RES_UPDATE_REPLAY_RECORD_DATA_S2S;
			Size = sizeof(PACKET_RES_UPDATE_REPLAY_RECORD_DATA_S2S);
		}

		int user_uid;
		int session_id;
		int result;
	};
}
#pragma pack(pop)