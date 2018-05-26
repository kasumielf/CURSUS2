using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Network;
using Scripts.Utility;
using Packet;

public class NetworkManager : MonoBehaviour
{
    SocketClass socket = new SocketClass();

    private void Awake()
    {
        
    }
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void Connect(string ip, short port)
    {
        Debug.Log("Socket Connect to" + ip + ":" + port);
        socket.Connect(ip, port);
    }

    public bool isConnected()
    {
        return socket.isConnected();
    }

    public void Disconnect()
    {
        socket.Disconnect();
    }

    public void SetPacketProcessHandler(packetProcess pp)
    {
        socket.SetPacketProcessHandler(pp);
    }
    public void ClearPacketProcessHandler()
    {
        socket.ClearePacketProcessHandler();
    }

    public void Send(byte[] data)
	{
		socket.SendData(data);
	}

	private void OnApplicationQuit()
	{
		socket.Disconnect();
	}

}
