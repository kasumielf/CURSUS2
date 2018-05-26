using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Packet;
using Scripts.Utility;
using UnityEngine.SceneManagement;

public class LobbyScene : MonoBehaviour {
    public NetworkManager nm;
    private MessageQueue msgQueue;
    public GameObject channels;
    public GameObject prefChannel;
    public RectTransform ParentPanel;
    public GameObject t;

    private string login_id;
    private string login_password;

    private void Awake()
    {
        msgQueue = MessageQueue.getInstance;

        StartCoroutine(checkMessageQueue());
    }

    private void ProcessMessage(Message msg)
    {
        switch (msg.Type)
        {
            case MessageType.SCENE_CHANGE_LOGIN_TO_LOBBY:
                {
                    if (nm == null)
                        nm = new NetworkManager();

                    nm.Connect(msg.GetParam(0).ToString(), (short)msg.GetParam(1));
                    nm.SetPacketProcessHandler(packetProcess);

                    login_id = msg.GetParam(2).ToString();
                    login_password = msg.GetParam(3).ToString();

                    RequestChannelList();

                    break;
                }
            case MessageType.AFTER_RECV_CHANNEL_LIST:
                {
                    GameObject channel = Instantiate(prefChannel) as GameObject;

                    channel.transform.SetParent(ParentPanel, false);

                    Text[] text = channel.GetComponentsInChildren<Text>();

                    /*
                     *0 : 버튼 라벨, 1 : 현재 접속자 수, 2 : 채널 이름
                     */
                    
                    text[2].text = msg.GetParam(0).ToString();
                    text[1].text = msg.GetParam(2).ToString();

                    Button tempButton = channel.GetComponentInChildren<Button>();

                    tempButton.onClick.AddListener(() =>
                    {
                        PACKET_REQ_ENTER_CHANNEL packet = new PACKET_REQ_ENTER_CHANNEL();

                        packet.Init();
                        packet.selected_channel_id = (byte)msg.GetParam(1);

                        byte[] data = Utility.ToByteArray((object)packet);

                        nm.Send(data);
                    });

                    break;
                }
            case MessageType.AFTER_ENTER_CHANNEL_SUCCESS:
                {
                    Message _msg = new Message(MessageType.SCENE_CHANGE_LOGIN_TO_LOBBY);

                    _msg.Push(msg.GetParam(0));
                    _msg.Push(msg.GetParam(1));

                    _msg.Push(login_id);
                    _msg.Push(login_password);

                    MessageQueue.getInstance.Push(_msg);

                    SceneManager.LoadScene("Cycle-HanGangScene");

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

        yield return 0;
    }

    // Use this for initialization
    void Start () {
        nm.SetPacketProcessHandler(packetProcess);
	}

	// Update is called once per frame
	void Update () {
		
	}

    private void packetProcess(byte[] data)
    {
        int _type = BitConverter.ToUInt16(new byte[2] { data[0], data[1] }, 0);

        PacketId id = (PacketId)_type;

        switch (id)
        {
            case PacketId.RES_CHANNEL_LIST:
                {
                    PACKET_RES_CHANNEL_LIST res = (PACKET_RES_CHANNEL_LIST)Utility.ByteArrayToObject(data, typeof(PACKET_RES_CHANNEL_LIST));

                    for(int i=0;i<res.channel_name.GetLength(0);++i)
                    {
                        if(res.channel_name[i].Equals("") == false)
                        {
                            Message msg = new Message(MessageType.AFTER_RECV_CHANNEL_LIST);
                            msg.Push(res.channel_name[i]);
                            msg.Push(res.channel_id[i]);
                            msg.Push(res.channel_user_count[i]);

                            MessageQueue.getInstance.Push(msg);
                        }
                    }

                }
                break;
            case PacketId.RES_ENTER_CHANNEL:
                {
                    PACKET_RES_ENTER_CHANNEL res = (PACKET_RES_ENTER_CHANNEL)Utility.ByteArrayToObject(data, typeof(PACKET_RES_ENTER_CHANNEL));
                    Message msg = new Message(MessageType.AFTER_ENTER_CHANNEL_FAILED);

                    if (res.result == 1)
                    {
                        msg = new Message(MessageType.AFTER_ENTER_CHANNEL_SUCCESS);
                        msg.Push(res.channel_ip);
                        msg.Push(res.port);
                    }

                    MessageQueue.getInstance.Push(msg);
                }
                break;
        }
    }

    public void RequestChannelList()
    {
        PACKET_REQ_CHANNEL_LIST req = new PACKET_REQ_CHANNEL_LIST();

        req.Init();

        nm.Send(Utility.ToByteArray((object)req));
    }
}
