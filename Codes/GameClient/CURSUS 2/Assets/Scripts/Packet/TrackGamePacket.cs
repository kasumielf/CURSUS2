using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Objects;

namespace TrackGamePacket
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
    class PACKET_REQ_INGAME_PLAYER_LIST : PACKET_HEADER
    {
    	public void Init()
        { 
            Id = PacketId.REQ_INGAME_PLAYER_LIST;
            Size = (short)Marshal.SizeOf(typeof(PACKET_REQ_INGAME_PLAYER_LIST));
        }

        public short room_id;
        public short player_index;
    };

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class Username
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string user_name;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PACKET_RES_INGAME_PLAYER_LIST : PACKET_HEADER
    {
        public void Init()
        {
            Id = PacketId.RES_INGAME_PLAYER_LIST;
            Size = (short)Marshal.SizeOf(typeof(PACKET_RES_INGAME_PLAYER_LIST));
        }

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public short[] index = new short[8];
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public Username[] username = new Username[8];
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public float[] pos_x = new float[8];
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public float[] pos_y = new float[8];
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public float[] pos_z = new float[8];
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public float[] r = new float[8];
    };

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PACKET_REQ_ROOM_UPDATE_PLAYER_POSITION : PACKET_HEADER
    {
    	public void Init()
        {
            Id = PacketId.REQ_ROOM_UPDATE_PLAYER_POSITION;
            Size = (short)Marshal.SizeOf(typeof(PACKET_REQ_ROOM_UPDATE_PLAYER_POSITION));
        }

        public short room_id;
        public short player_index;
        public float x;
        public float y;
        public float z;
        public float v;
        public float r;
    };

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PACKET_NOTIFY_ROOM_PLAYER_POSITION : PACKET_HEADER
    {
	    public void Init()
        {
            Id = PacketId.NOTIFY_ROOM_PLAYER_POSITION;
            Size = (short)Marshal.SizeOf(typeof(PACKET_NOTIFY_ROOM_PLAYER_POSITION));
        }

        public short player_index;
        public float x;
        public float y;
        public float z;
        public float v;
        public float r;
    };

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PACKET_REQ_INGAME_READY : PACKET_HEADER
    {
	    public void Init()
        {
            Id = PacketId.REQ_INGAME_READY;
            Size = (short)Marshal.SizeOf(typeof(PACKET_REQ_INGAME_READY));
        }

        public short room_id;
        public short player_index;
    };

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PACKET_NOTIFY_INGAME_READY : PACKET_HEADER
    {
	    public void Init()
        {
            Id = PacketId.NOTIFY_INGAME_READY;
            Size = (short)Marshal.SizeOf(typeof(PACKET_NOTIFY_INGAME_READY));
        }

        public short player_index;
    };

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PACKET_NOTIFY_ROOM_GAME_START : PACKET_HEADER
    {
	    public void Init()
        {
            Id = PacketId.NOTIFY_ROOM_GAME_START;
            Size = (short)Marshal.SizeOf(typeof(PACKET_NOTIFY_ROOM_GAME_START));
        }
        public double start_time;
    };

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PACKET_NOTIFY_ROOM_GAME_END : PACKET_HEADER
    {
	    public void Init()
        {
            Id = PacketId.NOTIFY_ROOM_GAME_END;
            Size = (short)Marshal.SizeOf(typeof(PACKET_NOTIFY_ROOM_GAME_END));
        }

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public short[] player_index = new short[3];
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public double[] records = new double[3];
    };

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PACKET_REQ_UPDATE_TRACK_COUNT : PACKET_HEADER
    {
        public void Init()
        {
            Id = PacketId.REQ_UPDATE_TRACK_COUNT;
            Size = (short)Marshal.SizeOf(typeof(PACKET_REQ_UPDATE_TRACK_COUNT));
        }
        public short room_id;
        public short player_index;
    };

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PACKET_NOTIFY_UPDATE_TRACK_COUNT : PACKET_HEADER
    {
        public void Init()
        {
            Id = PacketId.NOTIFY_UPDATE_TRACK_COUNT;
            Size = (short)Marshal.SizeOf(typeof(PACKET_NOTIFY_UPDATE_TRACK_COUNT));
        }
        public short player_index;
        public short track_count;
        public double checked_time;
        public byte is_finished;
    };

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PACKET_NOTIFY_PLAYER_TRACK_FINISHED : PACKET_HEADER
    {
        public void Init()
        {
            Id = PacketId.NOTIFY_PLAYER_TRACK_FINISHED;
            Size = (short)Marshal.SizeOf(typeof(PACKET_NOTIFY_PLAYER_TRACK_FINISHED));
        }
        public short player_index;
        public double finished_time;
    };

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PACKET_NOTIFY_READY_COUNT : PACKET_HEADER
    {
        public void Init()
        {
            Id = PacketId.NOTIFY_READY_COUNT;
            Size = (short)Marshal.SizeOf(typeof(PACKET_NOTIFY_READY_COUNT));
        }
    };


}
