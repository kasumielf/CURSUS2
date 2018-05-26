using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Objects;

enum PacketId : short
{
    DUMMY_PACKET = 1,
    REQ_LOGIN = 100,
    RES_LOGIN = 101,
    REQ_CREATE_ACCOUNT = 102,
    RES_CREATE_ACCOUNT = 103,
    RES_USER_INFO = 201,
    REQ_USER_INFO = 202,
    RES_CREATE_ACCOUNT_S2S = 104,
    REQ_CREATE_ACCOUNT_S2S = 105,
    REQ_LOGIN_S2S = 106,
    RES_LOGIN_S2S = 107,

    REQ_CHANNEL_LIST = 201,
    RES_CHANNEL_LIST = 202,
    REQ_ENTER_CHANNEL = 203,
    RES_ENTER_CHANNEL = 204,
    NTF_FORECAST_INFO = 300,

    NTF_PLAYER_ENTER = 400,
    NTF_PLAYER_EXIT = 401,
    NTF_PLAYER_MOVE = 402,
    REQ_PLAYER_MOVE = 403,
    REQ_PLAYER_ENTER = 405,
    RES_PLAYER_ENTER = 406, 
    REQ_PLAYER_EXIT = 407,
    RES_PLAYER_EXIT = 408,

    REQ_UPDATE_RECORD_DATA = 500,
    RES_UPDATE_RECORD_DATA = 501,
    REQ_GET_RECORD_DATA = 502,
    RES_GET_RECORD_DATA = 503,
    REQ_RANKUSER_RECORD_DATA = 504,
    RES_RANKUSER_RECORD_DATA = 505,

    REQ_GET_REPLAY_RECORD_DATA = 520,
    REQ_UPDATE_REPLAY_RECORD_DATA = 521,
    RES_GET_REPLY_RECORD_DATA = 522,
    RES_UPDATE_REPLY_RECORD_DATA = 534,

    NOTIFY_GAME_START = 1000,
    NOTIFY_PLAYER_ROOM_ENTER = 1001,
    NOTIFY_PLAYER_ROOM_EXIT = 1002,
    NOTIFY_PLAYER_ROOM_READY = 1003,
    NOTIFY_PLAYER_SET_HOST = 1004,
    REQ_CREATE_ROOM_INFO = 1005,
    RES_CREATE_ROOM_INFO = 1006,
    REQ_ROOM_INFO = 1007,
    RES_ROOM_INFO = 1008,
    REQ_PLAYER_READY = 1009,
    REQ_ROOM_PLAYER_EXIT = 1010,
    RES_ROOM_PLAYER_EXIT = 1011,
    REQ_ROOM_PLAYER_ENTER = 1012,
    RES_PLAYER_ROOM_ENTER = 1013,
    REQ_GAME_START = 1014,
    RES_GAME_START = 1015,
    REQ_ROOM_PLAYERS = 1016,
    RES_ROOM_PLAYERS = 1017,
    REQ_ROOM_SET_TYPE = 1018,
    RES_ROOM_SET_TYPE = 1019,
    NOTIFY_ROOM_SET_TYPE = 1020,
    REQ_ROOM_PLAYER_BAN = 1021,
    RES_ROOM_PLAYER_BAN = 1022,
    NOTIFY_ROOM_PLAYER_BAN = 1023,
    REQ_INGAME_PLAYER_LIST = 2000,
    RES_INGAME_PLAYER_LIST = 2001,
    REQ_ROOM_UPDATE_PLAYER_POSITION = 2002,
    NOTIFY_ROOM_PLAYER_POSITION = 2003,
    REQ_INGAME_READY = 2004,
    NOTIFY_INGAME_READY = 2005,
    REQ_UPDATE_TRACK_COUNT = 2006,
    NOTIFY_UPDATE_TRACK_COUNT = 2007,
    NOTIFY_PLAYER_TRACK_FINISHED = 2008,
    NOTIFY_ROOM_GAME_START = 2100,
    NOTIFY_ROOM_GAME_END = 2101,
    NOTIFY_READY_COUNT = 2200,


}

namespace Packet
{

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PACKET_HEADER
    {
        public PacketId Id;
        public short Size;
    };

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PACKET_DUMMY : PACKET_HEADER
    {
        public void Init()
        {
            Id = PacketId.DUMMY_PACKET;
            Size = (short)Marshal.SizeOf(typeof(PACKET_DUMMY));
        }
        public int data;
    };

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PACEKT_REQ_CREATE_ACCOUNT : PACKET_HEADER
    {
        public void Init()
        {
            Id = PacketId.REQ_CREATE_ACCOUNT;
            Size = (short)Marshal.SizeOf(typeof(PACEKT_REQ_CREATE_ACCOUNT));
        }
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string login_id;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string password;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string username;

        public byte weight;
        public int birthday;
        public bool gender;
    };

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PACKET_RES_CREATE_ACCOUNT : PACKET_HEADER
    {
        public void Init()
        {
            Id = PacketId.RES_CREATE_ACCOUNT;
            Size = Size = (short)Marshal.SizeOf(typeof(PACKET_RES_CREATE_ACCOUNT));
        }

        public short result;
    };

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PACKET_REQ_LOGIN : PACKET_HEADER
    {
        public void Init()
        {
            Id = PacketId.REQ_LOGIN;
            Size = (short)Marshal.SizeOf(typeof(PACKET_REQ_LOGIN));

        }

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string login_id;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string password;
    };

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PACKET_RES_LOGIN : PACKET_HEADER
    {
        public void Init()
        {
            Id = PacketId.RES_LOGIN;
            Size = (short)Marshal.SizeOf(typeof(PACKET_RES_LOGIN));
        }

        public short result;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
        public string lobby_ip;
        public short port;

    };

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PACEKT_REQ_USER_INFO : PACKET_HEADER
    {
        public void Init()
        {
            Id = PacketId.RES_USER_INFO;
            Size = (short)Marshal.SizeOf(typeof(PACEKT_REQ_USER_INFO));
        }

        public short session_id;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
        public string id;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
        public string password;
    };

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PACKET_RES_USER_INFO : PACKET_HEADER
    {
        public void Init()
        {
            Id = PacketId.RES_USER_INFO;
            Size = (short)Marshal.SizeOf(typeof(PACKET_RES_USER_INFO));
        }

        public short session_id;
        public PacketUser user;
    };

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PACKET_REQ_CHANNEL_LIST : PACKET_HEADER
    {
        public void Init()
        {
            Id = PacketId.REQ_CHANNEL_LIST;
            Size = (short)Marshal.SizeOf(typeof(PACKET_REQ_CHANNEL_LIST));
        }
    };

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class ChannelName
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string channel_name;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class Username
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string user_name;
    }


    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PACKET_RES_CHANNEL_LIST : PACKET_HEADER
    {
        public void Init()
        {
            Id = PacketId.RES_CHANNEL_LIST;
            Size = (short)Marshal.SizeOf(typeof(PACKET_RES_CHANNEL_LIST));
        }
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public ChannelName[] channel_name = new ChannelName[2];
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] channel_id = new byte[2];
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public short[] channel_user_count = new short[2];
    };

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PACKET_REQ_ENTER_CHANNEL : PACKET_HEADER
    {
        public void Init()
        {
            Id = PacketId.REQ_ENTER_CHANNEL;
            Size = (short)Marshal.SizeOf(typeof(PACKET_REQ_ENTER_CHANNEL));
        }

        public byte selected_channel_id;
    };

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PACKET_RES_ENTER_CHANNEL : PACKET_HEADER
    {
        public void Init()
        {
            Id = PacketId.RES_ENTER_CHANNEL;
            Size = (short)Marshal.SizeOf(typeof(PACKET_RES_ENTER_CHANNEL));
        }

        public short result;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
        public string channel_ip;
        public short port;
    };


    /*
     * 주석 처리 이유 : 날씨 정보의 헤더 스트링까지 가져오는거였는데 그냥 안가져오고
     * 인덱스 순서에 따라 날씨 정보(하늘 상태, 풍속, 풍향 등)를 가져오도록 수정.
     * 헤더값을 날려 대역 낭비할 필요 없음
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class ForecastTypeName
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
        public string fore;
    }*/

    /*
     * 기상 정보 인덱스
     * 0 : 낙뢰
     * 1 : 강수 형태(0:X, 1:비, 2:비/눈)
     * 2 : 습도
     * 3 : 강수량
     * 4 : 하늘 상태(1:맑음, 2:구름 적음, 3:구름 많음, 4: 흐림)
     * 5 : 기온
     * 6 : 동서방향 성분
     * 7 : 풍향
     * 8 : 남북바람 성분
     * 9 : 풍속
     */
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PACKET_NOTIFY_FORECAST_INFO : PACKET_HEADER
    {
    	public void Init()
        {
            Id = PacketId.NTF_FORECAST_INFO;
            Size = (short)Marshal.SizeOf(typeof(PACKET_NOTIFY_FORECAST_INFO));
        }

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public float[] value = new float[10];
        public byte forecast;
    };

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PACKET_NTF_PLAYER_ENTER : PACKET_HEADER
    {
        public void Init()
        {
            Id = PacketId.NTF_PLAYER_ENTER;
            Size = (short)Marshal.SizeOf(typeof(PACKET_NTF_PLAYER_ENTER));
        }

        public int user_id;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string username;
        public float x;
        public float y;
        public float z;
    };

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PACKET_NTF_PLAYER_EXIT : PACKET_HEADER
    {
        public void Init()
        {
            Id = PacketId.NTF_PLAYER_EXIT;
            Size = (short)Marshal.SizeOf(typeof(PACKET_NTF_PLAYER_EXIT));
        }

        public short user_id;
    };

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PACKET_NTF_PLAYER_MOVE : PACKET_HEADER
    {
        public void Init()
        {
            Id = PacketId.NTF_PLAYER_MOVE;
            Size = (short)Marshal.SizeOf(typeof(PACKET_NTF_PLAYER_MOVE));
        }

        public int user_id;
        public float x;
        public float y;
        public float z;
        public float v;
    };


    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PACKET_REQ_PLAYER_MOVE : PACKET_HEADER
    {
        public void Init()
        {
            Id = PacketId.REQ_PLAYER_MOVE;
            Size = (short)Marshal.SizeOf(typeof(PACKET_REQ_PLAYER_MOVE));
        }

        public float x;
        public float y;
        public float z;
        public float r;
        public float v;
    };

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PACKET_REQ_PLAYER_ENTER : PACKET_HEADER
    {
        public void Init()
        {
            Id = PacketId.REQ_PLAYER_ENTER;
            Size = (short)Marshal.SizeOf(typeof(PACKET_REQ_PLAYER_ENTER));
        }

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string login_id;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string password;
    };

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PACKET_RES_PLAYER_ENTER : PACKET_HEADER
    {
        public void Init()
        {
            Id = PacketId.RES_PLAYER_ENTER;
            Size = (short)Marshal.SizeOf(typeof(PACKET_RES_PLAYER_ENTER));
        }

        public int userUid;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string id;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string username;
        public byte weight;
        public byte gender;
        public int current_map;
    };

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PACKET_REQ_PLAYER_EXIT : PACKET_HEADER
    {
        public void Init()
        {
            Id = PacketId.REQ_PLAYER_EXIT;
            Size = (short)Marshal.SizeOf(typeof(PACKET_REQ_PLAYER_EXIT));
        }
    };

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PACKET_RES_PLAYER_EXIT : PACKET_HEADER
    {
        public void Init()
        {
            Id = PacketId.RES_PLAYER_EXIT;
            Size = (short)Marshal.SizeOf(typeof(PACKET_RES_PLAYER_EXIT));
        }

        short result;
    };

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PACKET_REQ_GET_RECORD_DATA  :  PACKET_HEADER
	{
        public void Init()
        {
            Id = PacketId.REQ_GET_RECORD_DATA;
            Size = (short)Marshal.SizeOf(typeof(PACKET_REQ_GET_RECORD_DATA));
        }
        public int user_uid;
    };

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PACKET_RES_GET_RECORD_DATA  :  PACKET_HEADER
	{
        public void Init()
        {
            Id = PacketId.RES_GET_RECORD_DATA;
            Size = (short)Marshal.SizeOf(typeof(PACKET_RES_GET_RECORD_DATA));
        }

        public double record_time;
	};

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]

    class PACKET_REQ_UPDATE_RECORD_DATA : PACKET_HEADER
	{
		public void Init()
        {
            Id = PacketId.REQ_UPDATE_RECORD_DATA;
            Size = (short)Marshal.SizeOf(typeof(PACKET_REQ_UPDATE_RECORD_DATA));
        }

        public int user_uid;
        public double record_time;
        public double checked_time;
    };

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PACKET_RES_UPDATE_RECORD_DATA : PACKET_HEADER
	{
        public void Init()
        {
            Id = PacketId.RES_UPDATE_RECORD_DATA;
            Size = (short)Marshal.SizeOf(typeof(PACKET_RES_UPDATE_RECORD_DATA));
        }

        public int result;
	};

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PACKET_REQ_RANKUSER_RECORD_DATA  : PACKET_HEADER
	{
        public void Init()
        {
            Id = PacketId.REQ_RANKUSER_RECORD_DATA;
            Size = (short)Marshal.SizeOf(typeof(PACKET_REQ_RANKUSER_RECORD_DATA));
        }
	};

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PACKET_RES_RANKUSER_RECORD_DATA  :  PACKET_HEADER
	{
        public void Init()
        {
            Id = PacketId.RES_RANKUSER_RECORD_DATA;
            Size = (short)Marshal.SizeOf(typeof(PACKET_RES_RANKUSER_RECORD_DATA));
        }

        public int item_count;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public Username[] username = new Username[5];
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public double[] record_time = new double[5];
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public double[] checked_time = new double[5];
    };

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PACKET_REQ_GET_REPLAY_RECORD_DATA : PACKET_HEADER
	{
        public void Init()
        {
            Id = PacketId.REQ_GET_REPLAY_RECORD_DATA;
            Size = (short)Marshal.SizeOf(typeof(PACKET_REQ_GET_REPLAY_RECORD_DATA));
        }

        public int user_uid;
    };

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PACKET_REQ_UPDATE_REPLAY_RECORD_DATA : PACKET_HEADER
	{
		public void Init()
        {
            Id = PacketId.REQ_UPDATE_REPLAY_RECORD_DATA;
            Size = (short)Marshal.SizeOf(typeof(PACKET_REQ_UPDATE_REPLAY_RECORD_DATA));
        }

        public int user_uid;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 120)]
        public byte[] records = new byte[120];
	};

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PACKET_RES_GET_REPLAY_RECORD_DATA : PACKET_HEADER
	{
        public void Init()
        {
            Id = PacketId.RES_GET_REPLY_RECORD_DATA;
            Size = (short)Marshal.SizeOf(typeof(PACKET_RES_GET_REPLAY_RECORD_DATA));
        }

        public int index;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 120)]
        public byte[] records = new byte[120];
        public double record_time;
	};

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PACKET_RES_UPDATE_REPLAY_RECORD_DATA : PACKET_HEADER
	{
        public void Init()
        {
            Id = PacketId.RES_UPDATE_REPLY_RECORD_DATA;
            Size = (short)Marshal.SizeOf(typeof(PACKET_RES_UPDATE_REPLAY_RECORD_DATA));
        }

        public int result;
	};
}