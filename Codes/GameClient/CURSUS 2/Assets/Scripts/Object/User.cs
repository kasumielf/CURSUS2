using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Objects
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class PacketUser
    {
        public int userUid;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string id;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string username;
        public byte weight;
        public byte gender;
        //public uint birthday;
        public float x;
        public float y;
        public float z;
        public float velocity;
        public float rad;
        public int current_map;
    }
}
