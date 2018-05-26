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

public class OpenWorld : MonoBehaviour {
    public enum Forecast
    {
        Sunny = 0,
        Rain = 1,
        Snow = 2,
    }

    public NetworkManagerSingleton nm;
    private MessageQueue msgQueue;

    public MessageQueue MessageQueue { get { return msgQueue; } }
    public Text ForecastDebugText;
    public GameObject player_object;
    public OtherPlayerPrafab other_player_prefab;
    public DummyPlayerPrefab dummy_player_prefab;
    public WeatherBox weather;
    public WeatherDisplayUI weatherUI;
    public RankResultPage rankPage;
    public ReplayButton[] replayButtons;
    public Text playerName;

    private string login_id;
    private string login_pwd;
    private string server_ip;
    private short server_port;
    private PacketUser user;
    private Forecast current_forecast;
    private Dictionary<int, OtherPlayerPrafab> other_players;
    private Dictionary<int, Map> map_data;

    public ReplayManager replayManager;
    public HandleModeIconScript handle;
    public Pathes pathes;
	public GameObject reset_button;

    public GameObject uiCanvas;

    public NetworkManager NetworkManager { get{ return nm.Instance; } }

    public PacketUser GetMyUser() { return user; }

    private void Awake()
    {
        other_player_prefab = Resources.Load("Prefabs/OtherPlayerPrefab", typeof(OtherPlayerPrafab)) as OtherPlayerPrafab;
        dummy_player_prefab = Resources.Load("Prefabs/DummyPlayerPrefab", typeof(DummyPlayerPrefab)) as DummyPlayerPrefab;
                                                      
        nm = GameObject.FindWithTag("Network").GetComponent<NetworkManagerSingleton>();

        NetworkManager.ClearPacketProcessHandler();
        NetworkManager.SetPacketProcessHandler(packetProcess);

        msgQueue = MessageQueue.getInstance;
        player_object = GameObject.FindWithTag("Player");//.GetComponent<Transform>() as GameObject;
        other_players = new Dictionary<int, OtherPlayerPrafab>();
        map_data = new Dictionary<int, Map>();

        /*
        Map HanGang = new Map();

        HanGang.Id = 100;
        HanGang.Name = "한강";
        HanGang.Thumbnail = "Sprite/MapImages/Hangang";
        HanGang.SceneName = "TrackGameHangangMap";
        
        map_data[HanGang.Id] = HanGang;*/

        Map Track = new Map();

        Track.Id = 101;
        Track.Name = "원형 트랙";
        Track.Thumbnail = "Sprite/MapImages/Track";
        Track.SceneName = "TrackScene";
        map_data[Track.Id] = Track;

		SetActiveResetButton (false);
    }

    private void OnDestroy()
    {
        Debug.Log("Open World Scene Destroy call");
        StopCoroutine(checkMessageQueue());
        StopCoroutine(PositionUpdateRoutine());
        StopCoroutine(CreateDummyPlayer());
    }

    IEnumerator CreateDummyPlayer()
    {
        while(true)
        {
            DummyPlayerPrefab dp = Instantiate(dummy_player_prefab, new Vector3(0.0f, 10.2f, 2.0f), Quaternion.identity) as DummyPlayerPrefab;
            dp.SetReady(UnityEngine.Random.Range(16.0f, 20.0f), pathes.nonplayer_front_path);
            yield return new WaitForSeconds(30.0f);
        }
    }
    

    private void ProcessMessage(Message msg)
    {
        switch (msg.Type)
        {
            case MessageType.SCENE_CHANGE_LOGIN_TO_LOBBY:
                {
                    Message _msg = new Message(MessageType.SCENE_CHANGE_LOGIN_TO_LOBBY);

                    server_ip = msg.GetParam(0).ToString();
                    server_port = (short)msg.GetParam(1);

                    NetworkManager.Connect(server_ip, server_port);

                    while(NetworkManager.isConnected() != true)
                    {
                    }

                    PACKET_REQ_PLAYER_ENTER packet = new PACKET_REQ_PLAYER_ENTER();

                    packet.Init();

                    login_id = msg.GetParam(2).ToString();
                    login_pwd = msg.GetParam(3).ToString();

                    packet.login_id = login_id;
                    packet.password = login_pwd;

                    byte[] data = Utility.ToByteArray((object)packet);

                    NetworkManager.Send(data);

                    break;
                }
            case MessageType.SCENE_CHANGE_TRACKGAME_TO_WORLD:
                {
                    Vector3 pos = (Vector3)msg.GetParam(0);

                    player_object.transform.position = pos;

                    Debug.Log("Return from Track Game!");

                    break;
                }
            case MessageType.NOTIFY_PLAYER_ENTER:
                {
                    int uid = (int)msg.GetParam(0);

                    if (other_players.ContainsKey(uid) == false)
                    {
                        OtherPlayerPrafab op = Instantiate(other_player_prefab, new Vector3(-1, 10.2f ,-1), Quaternion.identity) as OtherPlayerPrafab;
                        op.name.text = msg.GetParam(1).ToString();
                        float x = (float)msg.GetParam(2);
                        float y = (float)msg.GetParam(3);
                        float z = (float)msg.GetParam(4);

                        op.SetPosition(x, y, z);
                        op.SetPath(pathes.nonplayer_front_path);  

                        other_players.Add(uid, op);
                    }

                    break;
                }
            case MessageType.NOTIFY_PLAYER_EXIT:
                {
                    int uid = (int)msg.GetParam(0);

                    if (other_players.ContainsKey(uid))
                    {
                        Destroy(other_players[uid].gameObject);
                        other_players.Remove(uid);
                    }

                    break;
                }
            case MessageType.UPDATE_USER_POSITION:
                {
                    float x = (float)msg.GetParam(1);
                    float y = (float)msg.GetParam(2);
                    float z = (float)msg.GetParam(3);
                    float v = (float)msg.GetParam(4);

                    user.x = x;
                    user.y = y;
                    user.z = z;

                    if ((int)msg.GetParam(0) == user.userUid)
                    {
                        //float cz = player_object.transform.position.y;

                        //Vector3 v = new Vector3(x, cz, y);
                        //player_object.transform.position = v;
                    }
                    else
                    {
                        int uid = (int)msg.GetParam(0);

                        if (other_players.ContainsKey(uid))
                        {
                            //other_players[uid].UpdatePosition(x, y, z);
                            other_players[uid].Speed = v;
                        }
                    }

                    break;
                }
            case MessageType.UPDATE_FORECAST_INFO:
                {
                    switch((byte)msg.GetParam(0))
                    {
                        case 1:
                            current_forecast = Forecast.Rain;
                            weather.SetRain(0.0f);
                            break;
                        case 2:
                            current_forecast = Forecast.Snow;
                            weather.SetSnow(0.0f);
                            break;
                        case 3:
                            current_forecast = Forecast.Snow;
                            weather.SetSnow(0.0f);
                            break;
                        default:
                            current_forecast = Forecast.Sunny;
                            weather.SetSunny();
                            break;
                    }

                    try
                    {
                        // Param의 Index 0은 current_forecast에 사용.
                        // 나머진 1부터 시작해서 총 10까지의 인덱스로 조회한다.
                        /*
                        [{"type":"LGT","value":1}, 낙뢰
                        {"type":"PTY","value":1}, 강수 형태 없음(0), 비(1), 비/눈(2), 눈(3)
                        {"type":"REH","value":90}, 습도   %
                        {"type":"RN1","value":4.6}, 강수량 mm/h
                        {"type":"SKY","value":4}, 하늘 상태  1 맑음 2,3 구름 4 흐림
                        {"type":"T1H","value":22.5}, 기온    섭씨
                        {"type":"UUU","value":-1.2}, 동서 풍속
                        {"type":"VEC","value":99}, 풍향     
                        {"type":"VVV","value":0.2}, 남북 풍속
                        {"type":"WSD","value":1.3}] 풍속  m/s
                         */
                        weatherUI.DisplayUpdate((int)current_forecast,
                            (float)msg.GetParam(4),
                            (float)msg.GetParam(10),
                            (float)msg.GetParam(6),
                            (float)msg.GetParam(3));
                    }
                    catch(System.Exception)
                    {

                    }

                    break;
                }
            case MessageType.RES_PLAYER_ENTER:
                {
                    player_object.GetComponent<BikeControlMachine>().player_name = user.username;
                    playerName.text = user.username;
					
					InitGamePlay ();

                    break;
                }
            case MessageType.UPDATE_FORECAST_HUD_INFO:
                {
                    break;
                }
            case MessageType.GET_MY_REPLAY_DATA:
                {
                    int index = (int)msg.GetParam(0);
                    byte[] records = (byte[])msg.GetParam(1);
                    double record_time = (double)msg.GetParam(2);

                    replayManager.Put(index, records, record_time);

                    if(replayButtons[index].Index < 0)
                    {
                        replayButtons[index].Index = index;
                        replayButtons[index].SetTimeString(record_time);
                        replayButtons[index].play_event += OpenWorld_play_event;
                        replayButtons[index].stop_event += OpenWorld_stop_event;
                        replayButtons[index].Show();
                    }

                    break;
                }
            case MessageType.GET_MY_RECORD_DATA:
                {
                    double record_time = (double)msg.GetParam(0);
                    rankPage.SetMyBestRecord(record_time);
                    Debug.Log("Get My Record " + record_time);
                    break;
                }
        }
    }

    private void OpenWorld_stop_event(int index)
    {
        PlayReplay(index);
    }

    private void OpenWorld_play_event(int index)
    {
        StopReplay(index);
    }

    public void PlayReplay(int index)
    {
        replayManager.SetReplay(index);
        replayManager.StartReplay();
    }

    public void StopReplay(int index)
    {
        replayManager.SetReplay(index);
        replayManager.StopReplay();
    }

    private IEnumerator checkMessageQueue()
    {
        //0.5초 주기로 메시지 탐색
        WaitForSeconds waitSec = new WaitForSeconds(0.5f);

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
    void Start()
    {
        NetworkManager.SetPacketProcessHandler(packetProcess);

        StartCoroutine(checkMessageQueue());
        StartCoroutine(PositionUpdateRoutine());
        StartCoroutine(CreateDummyPlayer());

        player_object.GetComponent<BikeControl>().SetMyPath(pathes.player_front_path);

        weather.SetSunny();

        //uiCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.H))
        {
            player_object.GetComponent<BikeControl>().ToggleHandleMode();
            handle.SetIcon(player_object.GetComponent<BikeControl>().IsHandleMode);
        }
    }

    public Map GetMapData(int index)
    {
        return map_data[index];
    }

    public Dictionary<int, Map> GetMaps()
    {
        return map_data;
    }

	public void SetActiveResetButton(bool active)
	{
		reset_button.SetActive (active);
	}

	public void InitGamePlay()
	{
		PACKET_REQ_RANKUSER_RECORD_DATA req = new PACKET_REQ_RANKUSER_RECORD_DATA();
		req.Init();
		NetworkManager.Send(Utility.ToByteArray((object)req));

		PACKET_REQ_GET_RECORD_DATA req_get_record = new PACKET_REQ_GET_RECORD_DATA();
		req_get_record.Init();
		NetworkManager.Send(Utility.ToByteArray((object)req_get_record));

		PACKET_REQ_GET_REPLAY_RECORD_DATA req_replay = new PACKET_REQ_GET_REPLAY_RECORD_DATA();
		req_replay.Init();

		NetworkManager.Send(Utility.ToByteArray((object)req_replay));
	}

	public void ResetGamePlay()
	{
		// set player position to start point
		player_object.GetComponent<BikeControl>().Reset();

		// reset result board data

		// hide reset button
		SetActiveResetButton(false);

		// do init
		InitGamePlay();
	}

    IEnumerator PositionUpdateRoutine()
    {
        yield return new WaitForSeconds(10.0f);
        float x = 0.0f, y = 0.0f , r = 0.0f, v = 0.0f;

        PACKET_REQ_PLAYER_MOVE movePacket = new PACKET_REQ_PLAYER_MOVE();
        movePacket.Init();

        BikeControl bike = player_object.GetComponent<BikeControl>();

        while (true)
        {
            v = bike.MoveSpeed;

            movePacket.x = bike.GetDistance();
            //movePacket.y = player_object.transform.position.z;
            movePacket.v = v;
            //movePacket.r = r;

            if(v >= 1.0)
            {
                byte[] data = Utility.ToByteArray((object)movePacket);
                /*
                 * REMIND : ToByteArray는 아마도 오버헤드를 많이 먹을 것이므로 일단 임시로 이렇게 해둔 다음에,
                 *  바이트 배열에 직접 x,y,r,v를 집어넣는 식으로 바꾸는걸 고려해보자.
                 */
                NetworkManager.Send(data);
            }
            yield return new WaitForSeconds(1.0f);
        }

        yield return null;
    }

    public void onClickUIToggle()
    {
        bool toggle = uiCanvas.activeInHierarchy;

        uiCanvas.SetActive(!toggle);
    }

    private void SceneChange()
    {
        PACKET_REQ_PLAYER_EXIT packet = new PACKET_REQ_PLAYER_EXIT();

        packet.Init();
        byte[] data = Utility.ToByteArray((object)packet);

        NetworkManager.Send(data);

        Message _msg = new Message(MessageType.SCENE_CHANGE_LOGIN_TO_LOBBY);
        _msg.Push(server_ip);
        _msg.Push(server_port.ToString());

        _msg.Push(login_id);
        _msg.Push(login_pwd);

        MessageQueue.getInstance.Push(_msg);

        NetworkManager.Disconnect();

        while (NetworkManager.isConnected() != false)
        {
        }
    }

    public void RecordMyReplay(List<byte> data)
    {
        Packet.PACKET_REQ_UPDATE_REPLAY_RECORD_DATA req = new Packet.PACKET_REQ_UPDATE_REPLAY_RECORD_DATA();

        req.Init();

        if(user != null)
        {
            req.user_uid = GetMyUser().userUid;

            int i = 0;

            foreach (byte b in data)
            {
                if(i < 120)
                    req.records[i++] = b;
            }

            byte[] byte_data = Utility.ToByteArray((object)req);
            NetworkManager.Send(byte_data);
            Debug.Log("Record Replay Data");
        }
    }

    public void OpenWorldToHanMap()
    {
        SceneChange();

        /*Message msg_for_update_pos = new global::Message(MessageType.UPDATE_USER_POSITION);
        msg_for_update_pos.Push(user.x.ToString());
        msg_for_update_pos.Push(user.y.ToString());
        MessageQueue.getInstance.Push(msg_for_update_pos);

        SceneManager.LoadScene("Cycle-HanGangScene");*/
    }

    public void HanMapToOpenWorld()
    {
        SceneChange();
        SceneManager.LoadScene("OpenWorldScene");
    }

    private void packetProcess(byte[] data)
    {
        int _type = BitConverter.ToUInt16(new byte[2] { data[0], data[1] }, 0);

        PacketId id = (PacketId)_type;

        Debug.Log(id);
        switch (id)
        {
            case PacketId.DUMMY_PACKET:
                {
                    Debug.Log("Dummy receive");
                    break;
                }
            case PacketId.NTF_FORECAST_INFO:
                {
                    PACKET_NOTIFY_FORECAST_INFO res = (PACKET_NOTIFY_FORECAST_INFO)Utility.ByteArrayToObject(data, typeof(PACKET_NOTIFY_FORECAST_INFO));

                    Message forecast_message = new Message(MessageType.UPDATE_FORECAST_INFO);

                    forecast_message.Push(res.forecast);

                    int len = res.value.Length;
                    for (int i = 0; i < len; i++)
                    {
                        forecast_message.Push(res.value[i]);
                    }

                    MessageQueue.getInstance.Push(forecast_message);

                    break;
                }
            case PacketId.RES_PLAYER_ENTER:
                {
                    PACKET_RES_PLAYER_ENTER res = (PACKET_RES_PLAYER_ENTER)Utility.ByteArrayToObject(data, typeof(PACKET_RES_PLAYER_ENTER));

                    user = new PacketUser();
                   
                    user.userUid = res.userUid;
                    user.id = res.id;
                    user.username = res.username;
                    user.weight = res.weight;
                    user.gender = res.gender;
                    user.current_map = res.current_map;

                    Message msg = new Message(MessageType.RES_PLAYER_ENTER);

                    msg.Push(user.userUid);

                    MessageQueue.getInstance.Push(msg);

                    break;
                }
            case PacketId.NTF_PLAYER_ENTER:
                {
                    PACKET_NTF_PLAYER_ENTER ntf = (PACKET_NTF_PLAYER_ENTER)Utility.ByteArrayToObject(data, typeof(PACKET_NTF_PLAYER_ENTER));

                    Message msg = new Message(MessageType.NOTIFY_PLAYER_ENTER);

                    msg.Push(ntf.user_id);
                    msg.Push(ntf.username);
                    msg.Push(ntf.x);
                    msg.Push(ntf.y);
                    msg.Push(ntf.z);

                    MessageQueue.getInstance.Push(msg);

                    break;
                }
            case PacketId.NTF_PLAYER_MOVE:
                {
                    PACKET_NTF_PLAYER_MOVE ntf = (PACKET_NTF_PLAYER_MOVE)Utility.ByteArrayToObject(data, typeof(PACKET_NTF_PLAYER_MOVE));

                    Message msg = new Message(MessageType.UPDATE_USER_POSITION);

                    msg.Push(ntf.user_id.ToString());
                    msg.Push(ntf.x);
                    msg.Push(ntf.y);
                    msg.Push(ntf.z);
                    msg.Push(ntf.v);

                    MessageQueue.getInstance.Push(msg);

                    break;
                }
            case PacketId.NTF_PLAYER_EXIT:
                {
                    PACKET_NTF_PLAYER_EXIT ntf = (PACKET_NTF_PLAYER_EXIT)Utility.ByteArrayToObject(data, typeof(PACKET_NTF_PLAYER_EXIT));

                    Message msg = new Message(MessageType.NOTIFY_PLAYER_EXIT);
                    msg.Push(ntf.user_id);
                    MessageQueue.getInstance.Push(msg);

                    break;
                }
            case PacketId.NOTIFY_GAME_START:
                {
                    GameRoomPacket.PACKET_NOTIFY_GAME_START ntf = (GameRoomPacket.PACKET_NOTIFY_GAME_START)Utility.ByteArrayToObject(data, typeof(GameRoomPacket.PACKET_NOTIFY_GAME_START));
                    Message msg = new Message(MessageType.NOTIFY_GAME_START);
                    msg.Push(ntf.room_id);
                    msg.Push(ntf.start_x);
                    msg.Push(ntf.start_y);
                    msg.Push(ntf.start_z);
                    msg.Push(ntf.start_r);
                    msg.Push(ntf.map_id);

                    MessageQueue.getInstance.Push(msg);

                    break;
                }
            case PacketId.NOTIFY_PLAYER_ROOM_ENTER:
                {
                    GameRoomPacket.PACKET_NOTIFY_PLAYER_ROOM_ENTER ntf = (GameRoomPacket.PACKET_NOTIFY_PLAYER_ROOM_ENTER)Utility.ByteArrayToObject(data, typeof(GameRoomPacket.PACKET_NOTIFY_PLAYER_ROOM_ENTER));
                    Message msg = new Message(MessageType.NOTIFY_PLAYER_ROOM_ENTER);

                    msg.Push(ntf.room_id);
                    msg.Push(ntf.user_uid);
                    msg.Push(ntf.username);
                    msg.Push(ntf.player_index);
                    MessageQueue.getInstance.Push(msg);

                    break;
                }
            case PacketId.NOTIFY_PLAYER_ROOM_EXIT:
                {
                    GameRoomPacket.PACKET_NOTIFY_PLAYER_ROOM_EXIT ntf = (GameRoomPacket.PACKET_NOTIFY_PLAYER_ROOM_EXIT)Utility.ByteArrayToObject(data, typeof(GameRoomPacket.PACKET_NOTIFY_PLAYER_ROOM_EXIT));
                    Message msg = new Message(MessageType.NOTIFY_PLAYER_ROOM_EXIT);

                    msg.Push(ntf.room_id);
                    msg.Push(ntf.user_uid);
                    msg.Push(ntf.player_index);

                    MessageQueue.getInstance.Push(msg);

                    break;
                }
            case PacketId.NOTIFY_PLAYER_ROOM_READY:
                {
                    GameRoomPacket.PACKET_NOTIFY_PLAYER_ROOM_READY ntf = (GameRoomPacket.PACKET_NOTIFY_PLAYER_ROOM_READY)Utility.ByteArrayToObject(data, typeof(GameRoomPacket.PACKET_NOTIFY_PLAYER_ROOM_READY));
                    Message msg = new Message(MessageType.NOTIFY_PLAYER_ROOM_READY);

                    msg.Push(ntf.room_id);
                    msg.Push(ntf.user_uid);
                    msg.Push(ntf.player_index);
                    msg.Push(ntf.ready);

                    MessageQueue.getInstance.Push(msg);

                    break;
                }
            case PacketId.NOTIFY_PLAYER_SET_HOST:
                {
                    GameRoomPacket.PACKET_NOTIFY_PLAYER_SET_HOST ntf = (GameRoomPacket.PACKET_NOTIFY_PLAYER_SET_HOST)Utility.ByteArrayToObject(data, typeof(GameRoomPacket.PACKET_NOTIFY_PLAYER_SET_HOST));
                    Message msg = new Message(MessageType.NOTIFY_PLAYER_SET_HOST);

                    msg.Push(ntf.room_id);
                    msg.Push(ntf.player_index);

                    MessageQueue.getInstance.Push(msg);

                    break;
                }
            case PacketId.RES_CREATE_ROOM_INFO:
                {
                    GameRoomPacket.PACKET_RES_CREATE_ROOM_INFO res = (GameRoomPacket.PACKET_RES_CREATE_ROOM_INFO)Utility.ByteArrayToObject(data, typeof(GameRoomPacket.PACKET_RES_CREATE_ROOM_INFO));
                    Message msg = new Message(MessageType.RES_CREATE_ROOM_INFO);

                    msg.Push(res.room_id);
                    msg.Push(res.map_id);
                    msg.Push(res.result);

                    MessageQueue.getInstance.Push(msg);

                    break;
                }
            case PacketId.RES_GAME_START:
                {
                    GameRoomPacket.PACKET_RES_GAME_START res = (GameRoomPacket.PACKET_RES_GAME_START)Utility.ByteArrayToObject(data, typeof(GameRoomPacket.PACKET_RES_GAME_START));
                    Message msg = new Message(MessageType.RES_GAME_START);

                    msg.Push(res.room_id);
                    msg.Push(res.result);

                    MessageQueue.getInstance.Push(msg);

                    break;
                }
            case PacketId.RES_ROOM_INFO:
                {
                    GameRoomPacket.PACKET_RES_ROOM_INFO res = (GameRoomPacket.PACKET_RES_ROOM_INFO)Utility.ByteArrayToObject(data, typeof(GameRoomPacket.PACKET_RES_ROOM_INFO));
                    Message msg = new Message(MessageType.RES_ROOM_INFO);

                    msg.Push(res.room_id);
                    msg.Push(res.map_id);
                    msg.Push(res.user_count);

                    MessageQueue.getInstance.Push(msg);

                    break;
                }
            case PacketId.RES_PLAYER_ROOM_ENTER:
                {
                    GameRoomPacket.PACKET_RES_PLAYER_ROOM_ENTER res = (GameRoomPacket.PACKET_RES_PLAYER_ROOM_ENTER)Utility.ByteArrayToObject(data, typeof(GameRoomPacket.PACKET_RES_PLAYER_ROOM_ENTER));
                    Message msg = new Message(MessageType.RES_ROOM_PLAYER_ENTER);

                    msg.Push(res.room_id);
                    msg.Push(res.map_id);
                    msg.Push(res.result);
                    msg.Push(res.player_index);

                    MessageQueue.getInstance.Push(msg);

                    break;
                }
            case PacketId.RES_GET_RECORD_DATA:
                {
                    Packet.PACKET_RES_GET_RECORD_DATA res = (Packet.PACKET_RES_GET_RECORD_DATA)Utility.ByteArrayToObject(data, typeof(Packet.PACKET_RES_GET_RECORD_DATA));

                    Message msg = new Message(MessageType.GET_MY_RECORD_DATA);
                    msg.Push(res.record_time);
                    MessageQueue.getInstance.Push(msg);

                    break;
                }
            case PacketId.NOTIFY_ROOM_SET_TYPE:
                {
                    GameRoomPacket.PACKET_NOTIFY_ROOM_SET_TYPE res = (GameRoomPacket.PACKET_NOTIFY_ROOM_SET_TYPE)Utility.ByteArrayToObject(data, typeof(GameRoomPacket.PACKET_NOTIFY_ROOM_SET_TYPE));
                    Message msg = new Message(MessageType.NOTIFY_ROOM_SET_TYPE);

                    msg.Push(res.room_id);
                    msg.Push(res.player_index);
                    msg.Push(res.type);

                    MessageQueue.getInstance.Push(msg);

                    break;
                }
            case PacketId.RES_RANKUSER_RECORD_DATA:
                {
                    if(rankPage != null)
                    {
                        Packet.PACKET_RES_RANKUSER_RECORD_DATA res = (Packet.PACKET_RES_RANKUSER_RECORD_DATA)Utility.ByteArrayToObject(data, typeof(Packet.PACKET_RES_RANKUSER_RECORD_DATA));

                        for (int i = 0; i < 5; ++i)
                        {
                            if (i >= res.item_count)
                            {
                                rankPage.SetEmptyRank(i);
                            }
                            else
                            {
                                rankPage.SetRankedPlayerRecord(i, res.username[i].ToString(), res.record_time[i]);
                            }
                        }
                    }

                    break;
                }
            case PacketId.RES_GET_REPLY_RECORD_DATA:
                {
                    Packet.PACKET_RES_GET_REPLAY_RECORD_DATA res = (Packet.PACKET_RES_GET_REPLAY_RECORD_DATA)Utility.ByteArrayToObject(data, typeof(Packet.PACKET_RES_GET_REPLAY_RECORD_DATA));

                    Message msg = new Message(MessageType.GET_MY_REPLAY_DATA);

                    msg.Push(res.index);
                    msg.Push(res.records);
                    msg.Push(res.record_time);

                    Debug.Log("Get Replay Record " + res.index);

                    MessageQueue.getInstance.Push(msg);
                    break;
                }
            case PacketId.RES_UPDATE_REPLY_RECORD_DATA:
                {
                    break;
                }
        }
    }
}
