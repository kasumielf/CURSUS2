using Packet;
using Scripts.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Objects;
using Assets.Scripts.Object;

public class TrackScene : MonoBehaviour {
    public iTweenPath[] playerPathes;

    private Vector3 return_position;
    private int ready_count;
    private double start_time;

    public BitArray checkpoint_passed;

    public TrackGameResultUI resultUI;
    public TrackInfoUI trackUI;
    public CheckPointTrigger[] checkPoints;
    public NetworkManagerSingleton nm;
    private MessageQueue msgQueue;
    private int RoomId { get; set; }
    private int MyIndex { get; set; }

    public GameObject player_object;
    public GameObject CountdownOverlay;
    public Text txtCountdown;

    public PlayerPrefs player;
    public OtherPlayerPrafab[] other_players;

    public NetworkManager NetworkManager { get { return nm.Instance; } }

    private void Awake()
    {
        ready_count = 5;
        checkpoint_passed = new BitArray(checkPoints.Length, false);

        Application.runInBackground = true;

        for (int i = 0; i < 8; i++)
        {
            other_players[i].gameObject.SetActive(false);
        }

        nm = GameObject.FindWithTag("Network").GetComponent<NetworkManagerSingleton>();
        msgQueue = MessageQueue.getInstance;
        
        NetworkManager.ClearPacketProcessHandler();
        NetworkManager.SetPacketProcessHandler(packetProcess);

        StartCoroutine(checkMessageQueue());

        player_object.GetComponent<BikeControl>().Idle = true;
    }

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log("RUN!");
            player_object.GetComponent<BikeControl>().Auto = true;
            player_object.GetComponent<BikeControl>().Idle = false;
        }
    }

    public void SetPlayer(int index)
    {
        other_players[index].gameObject.SetActive(false);
        player_object.GetComponent<BikeControl>().SetMyPath(playerPathes[index].nodes.ToArray());
        player_object.GetComponent<BikeControl>().Idle = false;
    }

    public void SetOtherPlayer(int index, string name, float speed)
    {
        other_players[index].name.text = name;
        other_players[index].gameObject.SetActive(true);
        other_players[index].SetPath(playerPathes[index].nodes.ToArray());
        other_players[index].Speed = speed;
    }

    public void UpdateCheckpoint(int checkpoint)
    {
        checkpoint_passed[checkpoint] = true;
    }

    IEnumerator LastCountDown()
    {
        yield return new WaitForSeconds(1.0f);
        CountdownOverlay.SetActive(false);
        trackUI.gameObject.SetActive(true);
    }

    private void ProcessMessage(Message msg)
    {
        switch (msg.Type)
        {
            case MessageType.NOTIFY_READY_COUNT:
                {
                    if(ready_count <= 0)
                    {
                        txtCountdown.fontSize = 100;
                        txtCountdown.text = "Go!";
                        StartCoroutine(LastCountDown());
                    }
                    else
                    {
                        txtCountdown.text = ready_count.ToString();
                    }

                    ready_count--;
                    break;
                }
            case MessageType.NOTIFY_PLAYER_TRACK_FINISHED:
                {
                    short player_index = (short)msg.GetParam(0);
                    double finished_time = (double)msg.GetParam(1);

                    System.TimeSpan span = System.TimeSpan.FromMilliseconds(finished_time);
                    string t = string.Format("{0:00}:{1:00}:{2:00}.{3:000}", span.Hours, span.Minutes, span.Seconds, span.Milliseconds);

                    Debug.Log("Player " + player_index + " was finished in " + t);
                    break;
                }
            case MessageType.NOTIFY_TRACK_COUNT_UPDATE:
                {
                    int player_index = (short)msg.GetParam(0);
                    int track_count = (short)msg.GetParam(1);
                    double checked_time = (double)msg.GetParam(2);
                    byte is_finished = (byte)msg.GetParam(3);

                    trackUI.info[player_index].track = track_count;

                    Debug.Log("Track Count Updated : " + track_count + " ended : " + is_finished + " my : " + MyIndex);

                    if (is_finished == 1 && player_index == MyIndex)
                    {
                        Debug.Log("Go Rest!");
                        player_object.GetComponent<BikeControlMachine>().Idle = true;
                        player_object.GetComponent<BikeControl>().Idle = true;
                    }

                    System.TimeSpan span = System.TimeSpan.FromMilliseconds(checked_time);
                    string t = string.Format("{0:00}:{1:00}:{2:00}.{3:000}", span.Hours, span.Minutes, span.Seconds, span.Milliseconds);

                    Debug.Log("Player " + player_index + " has passed in " + t);

                    break;
                }
            case MessageType.INGAME_POSITION_UPDATE:
                {
                    int index = (short)msg.GetParam(0);

                    float v = (float)msg.GetParam(4);
                    trackUI.info[index].speed = v;
                    other_players[index].Speed = v;

                    break;
                }

            case MessageType.INGAME_PLAYER_ADD:
                {
                    short[] index = (short[])msg.GetParam(0);
                    TrackGamePacket.Username[] name = (TrackGamePacket.Username[])msg.GetParam(1);
                    //float[] x = (float[])msg.GetParam(2);
                    //float[] y = (float[])msg.GetParam(3);
                    //float[] z = (float[])msg.GetParam(4);
                    //float[] r = (float[])msg.GetParam(5);

                    for(int i=0;i<8;i++)
                    {
                        if(name[i] != null)
                        {
                            Debug.Log("name : " + name[i] + " index : " + index[i]);

                            trackUI.info[index[i]].name = name[i].ToString();
                            trackUI.info[index[i]].index = index[i];

                            SetOtherPlayer(i, name[i].ToString(), 0.0f);
                        }
                    }

                    break;
                }

            case MessageType.INGAME_START:
                {
                    CountdownOverlay.SetActive(false);
                    trackUI.gameObject.SetActive(true);

                    start_time = (double)msg.GetParam(0);

                    //player_object.GetComponent<BikeControlMachine>().Idle = false;
                    player_object.GetComponent<BikeControl>().Idle = false;

                    StartCoroutine(UpdateMyPosition());
                    StartCoroutine(checkTrackCheckPoints());

                    break;
                }

            case MessageType.INGAME_END:
                {
                    short[] player_index = (short[])msg.GetParam(0);
                    double[] record = (double[])msg.GetParam(1);

                    for(int i=0;i<3;i++)
                    {
                        if(player_index[i] >= 0)
                        {
                            string name = trackUI.info[player_index[i]].name;
                            resultUI.UpdateResult(i, name, record[i]);
                        }
                        else
                        {
                            resultUI.UpdateResult(i, "", 0);
                        }
                    }

                    StartCoroutine(GameEndRoutine());

                    break;
                }

            case MessageType.GAMEROOM_START:
                {
                    RoomId = (int)msg.GetParam(0);
                    MyIndex = (int)msg.GetParam(1);

                    //float start_x = (float)msg.GetParam(2);
                    //float start_y = (float)msg.GetParam(3);
                    //float start_z = (float)msg.GetParam(4);
                    //float start_r = (float)msg.GetParam(5);

                    trackUI.mapName.text = msg.GetParam(6).ToString();

                    return_position = (Vector3)msg.GetParam(7);
                    
                    trackUI.info[MyIndex].name = msg.GetParam(8).ToString();
                    trackUI.info[MyIndex].index = MyIndex;

                    Debug.Log("Room Id is " + RoomId);
                    //Debug.Log("Starting Position is " + start_x + "/" + start_z + "/" + start_y);

                    SetPlayer(MyIndex);

                    TrackGamePacket.PACKET_REQ_INGAME_PLAYER_LIST req = new TrackGamePacket.PACKET_REQ_INGAME_PLAYER_LIST();
                    req.Init();
                    req.room_id = (short)RoomId;
                    req.player_index = (short)MyIndex;

                    NetworkManager.Send(Utility.ToByteArray((object)req));

                    TrackGamePacket.PACKET_REQ_INGAME_READY req_ready = new TrackGamePacket.PACKET_REQ_INGAME_READY();
                    req_ready.Init();
                    req_ready.room_id = (short)RoomId;
                    req_ready.player_index = (short)MyIndex;

                    NetworkManager.Send(Utility.ToByteArray((object)req_ready));

                    break;
                }
        }
    }

    private void packetProcess(byte[] data)
    {
        int _type = BitConverter.ToUInt16(new byte[2] { data[0], data[1] }, 0);

        PacketId id = (PacketId)_type;

        switch (id)
        {
            case PacketId.DUMMY_PACKET:
                {
                    Debug.Log("Dummy receive");
                    break;
                }
            case PacketId.NOTIFY_READY_COUNT:
                {
                    Message msg = new Message(MessageType.NOTIFY_READY_COUNT);
                    MessageQueue.getInstance.Push(msg);
                    Debug.Log("Ready Count CALL!");
                    break;
                }
            case PacketId.NOTIFY_PLAYER_TRACK_FINISHED:
                {
                    TrackGamePacket.PACKET_NOTIFY_PLAYER_TRACK_FINISHED res = (TrackGamePacket.PACKET_NOTIFY_PLAYER_TRACK_FINISHED)Utility.ByteArrayToObject(data, typeof(TrackGamePacket.PACKET_NOTIFY_PLAYER_TRACK_FINISHED));
                    Message msg = new Message(MessageType.NOTIFY_PLAYER_TRACK_FINISHED);

                    msg.Push(res.player_index);
                    msg.Push(res.finished_time);

                    MessageQueue.getInstance.Push(msg);

                    break;
                }
            case PacketId.RES_INGAME_PLAYER_LIST:
                {
                    TrackGamePacket.PACKET_RES_INGAME_PLAYER_LIST not = (TrackGamePacket.PACKET_RES_INGAME_PLAYER_LIST)Utility.ByteArrayToObject(data, typeof(TrackGamePacket.PACKET_RES_INGAME_PLAYER_LIST));

                    Message msg = new Message(MessageType.INGAME_PLAYER_ADD);

                    msg.Push(not.index);
                    msg.Push(not.username);
                    msg.Push(not.pos_x);
                    msg.Push(not.pos_y);
                    msg.Push(not.pos_z);
                    msg.Push(not.r);

                    MessageQueue.getInstance.Push(msg);

                    break;
                }
            case PacketId.NOTIFY_ROOM_GAME_START:
                {
                    TrackGamePacket.PACKET_NOTIFY_ROOM_GAME_START not = (TrackGamePacket.PACKET_NOTIFY_ROOM_GAME_START)Utility.ByteArrayToObject(data, typeof(TrackGamePacket.PACKET_NOTIFY_ROOM_GAME_START));

                    Debug.Log("Game Start!");

                    Message msg = new Message(MessageType.INGAME_START);
                    msg.Push(not.start_time);
                    MessageQueue.getInstance.Push(msg);

                    break;
                }
            case PacketId.NOTIFY_ROOM_PLAYER_POSITION:
                {
                    TrackGamePacket.PACKET_NOTIFY_ROOM_PLAYER_POSITION not = (TrackGamePacket.PACKET_NOTIFY_ROOM_PLAYER_POSITION)Utility.ByteArrayToObject(data, typeof(TrackGamePacket.PACKET_NOTIFY_ROOM_PLAYER_POSITION));

                    Message msg = new Message(MessageType.INGAME_POSITION_UPDATE);
                    msg.Push(not.player_index);
                    msg.Push(not.x);
                    msg.Push(not.y);
                    msg.Push(not.z);
                    msg.Push(not.v);
                    msg.Push(not.r);

                    MessageQueue.getInstance.Push(msg);
                    break;
                }
            case PacketId.NOTIFY_UPDATE_TRACK_COUNT:
                {
                    TrackGamePacket.PACKET_NOTIFY_UPDATE_TRACK_COUNT not = (TrackGamePacket.PACKET_NOTIFY_UPDATE_TRACK_COUNT)Utility.ByteArrayToObject(data, typeof(TrackGamePacket.PACKET_NOTIFY_UPDATE_TRACK_COUNT));

                    Message msg = new Message(MessageType.NOTIFY_TRACK_COUNT_UPDATE);
                    msg.Push(not.player_index);
                    msg.Push(not.track_count);
                    msg.Push(not.checked_time);
                    msg.Push(not.is_finished);
                    MessageQueue.getInstance.Push(msg);

                    break;
                }
            case PacketId.NOTIFY_ROOM_GAME_END:
                {
                    TrackGamePacket.PACKET_NOTIFY_ROOM_GAME_END not = (TrackGamePacket.PACKET_NOTIFY_ROOM_GAME_END)Utility.ByteArrayToObject(data, typeof(TrackGamePacket.PACKET_NOTIFY_ROOM_GAME_END));

                    Message msg = new Message(MessageType.INGAME_END);
                    msg.Push(not.player_index);
                    msg.Push(not.records);
                    MessageQueue.getInstance.Push(msg);

                    break;
                }
        }
    }

    private IEnumerator UpdateMyPosition()
    {
        WaitForSeconds waitSec = new WaitForSeconds(0.5f);
        TrackGamePacket.PACKET_REQ_ROOM_UPDATE_PLAYER_POSITION update_packet = new TrackGamePacket.PACKET_REQ_ROOM_UPDATE_PLAYER_POSITION();
        update_packet.Init();
        update_packet.room_id = (short)RoomId;
        update_packet.player_index = (short)MyIndex;

        BikeControl speedGet = player_object.GetComponent<BikeControl>();

        while (true)
        {
            //update_packet.x = player_object.transform.position.x;
            //update_packet.y = player_object.transform.position.z;
            //update_packet.z = player_object.transform.position.y;
            update_packet.v = speedGet.MoveSpeed;
            //update_packet.r = player_object.transform.rotation.y;

            trackUI.info[MyIndex].speed = speedGet.MoveSpeed;

            NetworkManager.Send(Utility.ToByteArray((object)update_packet));

            yield return waitSec;
        }

        yield return 0;
    }

    private IEnumerator GameEndRoutine()
    {
        Message msg = new Message(MessageType.SCENE_CHANGE_TRACKGAME_TO_WORLD);
        msg.Push(return_position);
        resultUI.gameObject.SetActive(true);
        // 게임 종료 연출 로직 추가
        yield return new WaitForSeconds(15.0f);
        SceneManager.LoadScene("Cycle-HanGangScene");
    }

    private IEnumerator checkTrackCheckPoints()
    {
        int len = checkpoint_passed.Length;

        while (true)
        {
            bool check = true;

            for(int i=0;i<len;++i)
            {
                check &= checkpoint_passed[i];
            }

            if(check == true)
            {
                Debug.Log("Passed 1 cycle track.");
                TrackGamePacket.PACKET_REQ_UPDATE_TRACK_COUNT req = new TrackGamePacket.PACKET_REQ_UPDATE_TRACK_COUNT();
                req.Init();
                req.room_id = (short)RoomId;
                req.player_index = (short)MyIndex;
                byte[] data = Utility.ToByteArray((object)req);

                NetworkManager.Send(data);
                checkpoint_passed.SetAll(false);
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator checkMessageQueue()
    {
        WaitForSeconds waitSec = new WaitForSeconds(0.1f);

        while (true)
        {
            Message msg = msgQueue.getData();

            if (msg != null)
                ProcessMessage(msg);

            yield return waitSec;

        }

        yield return 0;
    }

    public void StopGame()
    {
        StopCoroutine(checkTrackCheckPoints());
        StopCoroutine(checkMessageQueue());

        SceneManager.LoadScene("Cycle-HanGangScene");
    }
}