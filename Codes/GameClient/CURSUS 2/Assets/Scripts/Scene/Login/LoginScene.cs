using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Packet;
using Scripts.Utility;
using UnityEngine.SceneManagement;

public class LoginScene : MonoBehaviour {

    public InputField loginIdField;
    public InputField loginPasswordField;
    public NetworkManager nm;
    private MessageQueue msgQueue;
    public MessageBox msgBox;

    public InputField IPAField;
    public InputField IPBField;
    public InputField IPCField;
    public InputField IPDField;
    public InputField PortField;

    // Use this for initialization
    void Start () {
        Application.runInBackground = true;

        String addr = String.Format("{0}.{1}.{2}.{3}", IPAField.text, IPBField.text, IPCField.text, IPDField.text);
        short port = short.Parse(PortField.text);

        nm.Connect(addr, port);
        nm.SetPacketProcessHandler(packetProcess);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void Awake()
    {
        msgQueue = MessageQueue.getInstance;

        StartCoroutine(checkMessageQueue());
    }

    public void onClickLogin()
    {
        Debug.Log("login start");

		if (nm.isConnected ())
		{
			PACKET_REQ_LOGIN packet = new PACKET_REQ_LOGIN();

			packet.Init();

			packet.login_id = loginIdField.text;
			packet.password = loginPasswordField.text;

			byte[] data = Utility.ToByteArray((object)packet);

			nm.Send(data);
		}
		else
		{
			Message _msg = new Message (MessageType.SOCKET_CLOSED);
			MessageQueue.getInstance.Push (_msg);
		}
    }

    private void ProcessMessage(Message msg)
    {
        switch(msg.Type)
        {
			case MessageType.SOCKET_CLOSED:
				{
					msgBox.Show("네트워크 연결이 끊어져 있습니다.");
					break;
				}
            case MessageType.AFTER_LOGIN_SCENE_CHANGE:
                {
                    nm.Disconnect();

                    Message _msg = new Message(MessageType.SCENE_CHANGE_LOGIN_TO_LOBBY);
                    _msg.Push(msg.GetParam(0));
                    _msg.Push(msg.GetParam(1));

                    _msg.Push(loginIdField.text);
                    _msg.Push(loginPasswordField.text);

                    MessageQueue.getInstance.Push(_msg);
                    
                    SceneManager.LoadScene("LobbyScene");
                    break;
                }
            case MessageType.AFTER_LOGIN_FAILED:
                {
                    msgBox.Show("Login Failed!");
                    break;
                }
            case MessageType.AFTER_CREATE_ACCOUNT_SUCCESS:
                {
                    msgBox.Show("Register Success!");
                    break;
                }
            case MessageType.AFTER_CREATE_ACCOUNT_FAILED:
                {
                    msgBox.Show("Register Failed!");
                    break;
                }
        }
    }

    private IEnumerator checkMessageQueue()
    {
        //1초 주기로 탐색
        WaitForSeconds waitSec = new WaitForSeconds(1);

        while (true)
        {
            Message msg = msgQueue.getData();

            if (msg != null)
                ProcessMessage(msg);

            yield return waitSec;
        }
    }

    private void packetProcess(byte[] data)
    {
        int _type = BitConverter.ToUInt16(new byte[2] { data[0], data[1] }, 0);

        PacketId id = (PacketId)_type;

        switch (id)
        {
            case PacketId.RES_LOGIN:
                {
                    PACKET_RES_LOGIN res = (PACKET_RES_LOGIN)Utility.ByteArrayToObject(data, typeof(PACKET_RES_LOGIN));

                    short result = res.result;

                    if (result < 0)
                    {
                        Message msg = new Message(MessageType.AFTER_LOGIN_FAILED);
                        MessageQueue.getInstance.Push(msg);
                    }
                    else
                    {
                        Message msg = new Message(MessageType.AFTER_LOGIN_SCENE_CHANGE);
                        msg.Push(res.lobby_ip);
                        msg.Push(res.port);

                        MessageQueue.getInstance.Push(msg);
                    }

                }
                break;
            case PacketId.RES_CREATE_ACCOUNT:
                {
                    PACKET_RES_CREATE_ACCOUNT res = (PACKET_RES_CREATE_ACCOUNT)Utility.ByteArrayToObject(data, typeof(PACKET_RES_CREATE_ACCOUNT));

                    short result = res.result;
                    Message msg = new Message(result > 0 ? MessageType.AFTER_CREATE_ACCOUNT_SUCCESS : MessageType.AFTER_CREATE_ACCOUNT_FAILED);
                    MessageQueue.getInstance.Push(msg);
                }
                break;
        }
    }
}