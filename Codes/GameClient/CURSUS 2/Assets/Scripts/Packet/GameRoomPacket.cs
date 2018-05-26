using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Objects;


namespace GameRoomPacket
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
    class PACKET_NOTIFY_GAME_START : PACKET_HEADER
	{
		public void Init()
		{
			Id = PacketId.NOTIFY_GAME_START;
			Size = (short)Marshal.SizeOf(typeof(PACKET_NOTIFY_GAME_START));
		}
        public short room_id;
        public float start_x;
        public float start_y;
        public float start_z;
        public float start_r;
        public int map_id;
    };

	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	class PACKET_NOTIFY_PLAYER_ROOM_ENTER : PACKET_HEADER
	{
		public void Init()
		{
			Id = PacketId.NOTIFY_PLAYER_ROOM_ENTER;
			Size = (short)Marshal.SizeOf(typeof(PACKET_NOTIFY_PLAYER_ROOM_ENTER));
		}

        public short room_id;
        public int user_uid;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string username;
        public short player_index;
	};

	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	class PACKET_NOTIFY_PLAYER_ROOM_EXIT : PACKET_HEADER
	{
		public void Init()
		{
			Id = PacketId.NOTIFY_PLAYER_ROOM_EXIT;
			Size = (short)Marshal.SizeOf(typeof(PACKET_NOTIFY_PLAYER_ROOM_EXIT));
		}

        public short room_id;
        public int user_uid;
        public short player_index;
	};

	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	class PACKET_NOTIFY_PLAYER_ROOM_READY : PACKET_HEADER
	{
		public void Init()
		{
			Id = PacketId.NOTIFY_PLAYER_ROOM_READY;
			Size = (short)Marshal.SizeOf(typeof(PACKET_NOTIFY_PLAYER_ROOM_READY));
		}

        public short room_id;
        public int user_uid;
        public short player_index;
        public bool ready;
	};

	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	class PACKET_NOTIFY_PLAYER_SET_HOST : PACKET_HEADER
	{
		public void Init()
		{
			Id = PacketId.NOTIFY_PLAYER_SET_HOST;
			Size = (short)Marshal.SizeOf(typeof(PACKET_NOTIFY_PLAYER_SET_HOST));
		}

        public short room_id;
        public int player_id;
        public short player_index;
	};

	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	class PACKET_REQ_CREATE_ROOM_INFO : PACKET_HEADER
	{
		public void Init()
		{
			Id = PacketId.REQ_CREATE_ROOM_INFO;
			Size = (short)Marshal.SizeOf(typeof(PACKET_REQ_CREATE_ROOM_INFO));
		}

        public int map_id;
	};

	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	class PACKET_RES_CREATE_ROOM_INFO : PACKET_HEADER
	{
		public void Init()
		{
			Id = PacketId.RES_CREATE_ROOM_INFO;
			Size = (short)Marshal.SizeOf(typeof(PACKET_RES_CREATE_ROOM_INFO));
		}

        public short room_id;
        public int map_id;
        public byte result;
	};

	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	class PACKET_REQ_ROOM_INFO : PACKET_HEADER
	{
		public void Init()
		{
			Id = PacketId.REQ_ROOM_INFO;
			Size = (short)Marshal.SizeOf(typeof(PACKET_REQ_ROOM_INFO));
		}
	};

	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	class PACKET_RES_ROOM_INFO : PACKET_HEADER
	{
		public void Init()
		{
			Id = PacketId.RES_ROOM_INFO;
			Size = (short)Marshal.SizeOf(typeof(PACKET_RES_ROOM_INFO));
		}

        public short room_id;
        public int map_id;
        public short user_count;
	};

	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PACKET_REQ_ROOM_PLAYERS : PACKET_HEADER
    {
        public void Init()
        {
            Id = PacketId.REQ_ROOM_PLAYERS;
            Size = (short)Marshal.SizeOf(typeof(PACKET_REQ_ROOM_PLAYERS));
        }
    };

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PACKET_RES_ROOM_PLAYERS : PACKET_HEADER
    {
        public void Init()
        {
            Id = PacketId.RES_ROOM_PLAYERS;
            Size = (short)Marshal.SizeOf(typeof(PACKET_RES_ROOM_PLAYERS));
        }

        public short room_id;
        public int user_uid;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string username;
        public short player_index;
    };


    [Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	class PACKET_REQ_PLAYER_READY : PACKET_HEADER
	{
		public void Init()
		{
			Id = PacketId.REQ_PLAYER_READY;
			Size = (short)Marshal.SizeOf(typeof(PACKET_REQ_PLAYER_READY));
		}

        public short room_id;
        public int user_uid;
        public int room_index;
	};

	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	class PACKET_REQ_PLAYER_EXIT : PACKET_HEADER
	{
		public void Init()
		{
			Id = PacketId.REQ_ROOM_PLAYER_EXIT;
			Size = (short)Marshal.SizeOf(typeof(PACKET_REQ_PLAYER_EXIT));
		}

        public short room_id;
	};

	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	class PACKET_REQ_PLAYER_ROOM_ENTER : PACKET_HEADER
	{
		public void Init()
		{
			Id = PacketId.REQ_ROOM_PLAYER_ENTER;
			Size = (short)Marshal.SizeOf(typeof(PACKET_REQ_PLAYER_ROOM_ENTER));
		}
        public short room_id;
	};

	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	class PACKET_RES_PLAYER_ROOM_ENTER : PACKET_HEADER
	{
		public void Init()
		{
			Id = PacketId.RES_PLAYER_ROOM_ENTER;
			Size = (short)Marshal.SizeOf(typeof(PACKET_RES_PLAYER_ROOM_ENTER));
		}
        public short room_id;
        public int map_id;
        public byte result;
        public short player_index;
    };

	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	class PACKET_REQ_GAME_START : PACKET_HEADER
	{
		public void Init()
		{
			Id = PacketId.REQ_GAME_START;
			Size = (short)Marshal.SizeOf(typeof(PACKET_REQ_GAME_START));
		}

        public short room_id;
	};

	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	class PACKET_RES_GAME_START : PACKET_HEADER
	{
		public void Init()
		{
			Id = PacketId.RES_GAME_START;
			Size = (short)Marshal.SizeOf(typeof(PACKET_RES_GAME_START));
		}
        public short room_id;
        public byte result;
	};
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PACKET_REQ_ROOM_SET_TYPE : PACKET_HEADER
    {
        public void Init()
        {
            Id = PacketId.REQ_ROOM_SET_TYPE;
            Size = (short)Marshal.SizeOf(typeof(PACKET_REQ_ROOM_SET_TYPE));
        }

        public short room_id;
        public short player_index;
        public byte type;
    };

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PACKET_RES_ROOM_SET_TYPE : PACKET_HEADER
    {
        public void Init()
        {
            Id = PacketId.RES_ROOM_SET_TYPE;
            Size = (short)Marshal.SizeOf(typeof(PACKET_RES_ROOM_SET_TYPE));
        }

        public byte result;
    };

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PACKET_NOTIFY_ROOM_SET_TYPE : PACKET_HEADER
    {
        public void Init()
        {
            Id = PacketId.NOTIFY_ROOM_SET_TYPE;
            Size = (short)Marshal.SizeOf(typeof(PACKET_NOTIFY_ROOM_SET_TYPE));
        }

        public short room_id;
        public short player_index;
        public byte type;
    };

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PACKET_REQ_ROOM_PLAYER_BAN : PACKET_HEADER
    {
        public void Init()
        {
            Id = PacketId.REQ_ROOM_PLAYER_BAN;
            Size = (short)Marshal.SizeOf(typeof(PACKET_REQ_ROOM_PLAYER_BAN));
        }

        public short room_id;
        public short player_index;
    };

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PACKET_RES_ROOM_PLAYER_BAN : PACKET_HEADER
    {
        public void Init()
        {
            Id = PacketId.RES_ROOM_PLAYER_BAN;
            Size = (short)Marshal.SizeOf(typeof(PACKET_RES_ROOM_PLAYER_BAN));
        }

        public byte result;
    };

}


