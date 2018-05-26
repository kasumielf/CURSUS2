using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Scripts.Utility;
using Assets.Scripts.Object;
using UnityEngine.SceneManagement;

public class TrackGameSelectUI : MonoBehaviour {
    private int room_count;

    public OpenWorld world;
    public GameObject GameFindUI;
    public GameObject GameMakeMapSelectContentView;
    public GameObject gameRoomScrollView;

    public GameMakePopup GameMakeUI;
    public GameRoomInfoUI CurrentGameRoomInfoUI;

    public MapItem mapItemPrefab;
    public GameRoomUI gameroomPrefab;

    // Use this for initialization

    public void Show()
    {
        this.gameObject.SetActive(true);
    }
    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Toggle()
    {
        this.gameObject.SetActive(!this.gameObject.activeSelf);
    }

    private void OnEnable()
    {
        RefreshGameRoom();
    }

    private void Awake()
    {
    }
    void Start () {
        int i = 0;
        room_count = 0;

        foreach (KeyValuePair<int, Map> v in world.GetMaps())
        {
            MapItem new_map = Instantiate(mapItemPrefab, new Vector3((-185.0f + 130 * i), 67.0f, 0.0f), Quaternion.identity) as MapItem;

            new_map.mapId = v.Key;
            new_map.mapImage.sprite = Resources.Load<Sprite>(v.Value.Thumbnail);
            new_map.mapName.text = v.Value.Name;
            new_map.popup = GameMakeUI;
            new_map.selectUi = this;

            new_map.transform.parent = GameMakeMapSelectContentView.transform;
            new_map.transform.localPosition = new Vector3((-185.0f + 130 * i) + 250.0f, -90.0f, 0.0f);
            new_map.transform.localScale = new Vector3(1.0f, 1.0f);
            new_map.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);

            i++;
        }

        StartCoroutine(checkMessageQueue());
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void OpenGameFindUI()
    {
        GameFindUI.SetActive(true);
    }

    public void CloseGameFindUI()
    {
        GameFindUI.SetActive(false);
    }

    public void SetPlayerType(int room_id, int index, UserType type)
    {
        if(type != UserType.Player)
        {
            GameRoomPacket.PACKET_REQ_ROOM_SET_TYPE req = new GameRoomPacket.PACKET_REQ_ROOM_SET_TYPE();

            req.Init();
            req.room_id = (short)room_id;
            req.player_index = (short)index;
            req.type = (byte)type;

            world.NetworkManager.Send(Utility.ToByteArray((object)req));
        }
    }

    public void SetReady(int index, int room_id, int my_index)
    {
        if (index == my_index)
        {
            GameRoomPacket.PACKET_REQ_PLAYER_READY req = new GameRoomPacket.PACKET_REQ_PLAYER_READY();

            req.Init();
            req.room_id = (short)room_id;
            req.room_index = my_index;
            req.user_uid = world.GetMyUser().userUid;

            world.NetworkManager.Send(Utility.ToByteArray((object)req));
        }
    }

    public void RefreshGameRoom()
    {
        GameRoomPacket.PACKET_REQ_ROOM_INFO req = new GameRoomPacket.PACKET_REQ_ROOM_INFO();
        req.Init();

        GameRoomUI[] rooms = gameRoomScrollView.GetComponentsInChildren<GameRoomUI>();

        foreach (GameRoomUI room in rooms)
        {
            GameObject.DestroyImmediate(room);
        }

        world.NetworkManager.Send(Utility.ToByteArray((object)req));
    }

    public void MakeGameRoom()
    {
        int map_id = GameMakeUI.SelectedMapId;

        Debug.Log("MakeGameRoom " + map_id);

        if(map_id > 0)
        {
            if(world.GetMaps().ContainsKey(map_id) == true)
            {
                GameMakeUI.Hide();

                GameRoomPacket.PACKET_REQ_CREATE_ROOM_INFO req = new GameRoomPacket.PACKET_REQ_CREATE_ROOM_INFO();

                req.Init();
                req.map_id = map_id;

                world.NetworkManager.Send(Utility.ToByteArray((object)req));
            }
        }
    }

    public void GameStart(int room_index)
    {
        GameRoomPacket.PACKET_REQ_GAME_START req = new GameRoomPacket.PACKET_REQ_GAME_START();
        req.Init();
        req.room_id = (short)room_index;
        world.NetworkManager.Send(Utility.ToByteArray((object)req));
    }

    public void LeaveGame(int room_index)
    {
        GameRoomPacket.PACKET_REQ_PLAYER_EXIT req = new GameRoomPacket.PACKET_REQ_PLAYER_EXIT();
        req.Init();
        req.room_id = (short)room_index;
        world.NetworkManager.Send(Utility.ToByteArray((object)req));
    }

    public void JoinGame(int room_index)
    {
        GameRoomPacket.PACKET_REQ_PLAYER_ROOM_ENTER req = new GameRoomPacket.PACKET_REQ_PLAYER_ROOM_ENTER();

        req.Init();
        req.room_id = (short)room_index;

        world.NetworkManager.Send(Utility.ToByteArray((object)req));
    }

    public void FindGame(InputField username)
    {
        if (username.text.Equals(""))
        {
            return;
        }

        Debug.Log("username : " + username.text);
    }

    private void ProcessMessage(Message msg)
    {
        switch (msg.Type)
        {
            case MessageType.NOTIFY_GAME_START:
                {
                    Debug.Log("Game Start!");

                    int room_id = (short)msg.GetParam(0);
                    Message start_msg = new Message(MessageType.GAMEROOM_START);
                    start_msg.Push(room_id);
                    start_msg.Push(CurrentGameRoomInfoUI.MyIndex);

                    start_msg.Push((float)msg.GetParam(1));// x
                    start_msg.Push((float)msg.GetParam(2));// y
                    start_msg.Push((float)msg.GetParam(3));// z
                    start_msg.Push((float)msg.GetParam(4));// z
                    int map_id = (int)msg.GetParam(5);

                    start_msg.Push(CurrentGameRoomInfoUI.MapName.text);// z

                    start_msg.Push(world.player_object.transform.position); // return position
                    start_msg.Push(world.GetMyUser().username);

                    DontDestroyOnLoad(world.nm);

                    world.MessageQueue.Push(start_msg);

                    SceneManager.LoadScene(world.GetMapData(map_id).SceneName);

                    break;
                }
            case MessageType.NOTIFY_PLAYER_EXIT:
                {
                    int room_id = (short)msg.GetParam(0);
                    int user_uid = (int)msg.GetParam(1);
                    int player_index = (short)msg.GetParam(3);

                    if (CurrentGameRoomInfoUI.RoomId == room_id)
                    {
                        CurrentGameRoomInfoUI.RemovePlayer(player_index);
                    }

                    break;
                }
            case MessageType.NOTIFY_PLAYER_ROOM_ENTER:
                {
                    short room_id = (short)msg.GetParam(0);
                    int user_uid = (int)msg.GetParam(1);
                    string username = msg.GetParam(2).ToString();
                    short player_index = (short)msg.GetParam(3);

                    if(CurrentGameRoomInfoUI.RoomId == room_id)
                        CurrentGameRoomInfoUI.AddPlayer(player_index, username, UserType.Player);

                    break;
                }
            case MessageType.NOTIFY_PLAYER_ROOM_EXIT:
                {
                    Debug.Log("Game NOTIFY_PLAYER_ROOM_EXIT!");

                    int room_id = (short)msg.GetParam(0);
                    int user_uid = (int)msg.GetParam(1);
                    int player_index = (short)msg.GetParam(3);

                    if (CurrentGameRoomInfoUI.RoomId == room_id)
                    {
                        CurrentGameRoomInfoUI.RemovePlayer(player_index);
                    }

                    break;
                }
            case MessageType.NOTIFY_PLAYER_SET_HOST:
                {
                    int room_id = (short)msg.GetParam(0);
                    int player_index = (short)msg.GetParam(3);


                    break;
                }
            case MessageType.NOTIFY_PLAYER_ROOM_READY:
                {
                    Debug.Log("Game NOTIFY_PLAYER_ROOM_READY!");

                    int room_id = (short)msg.GetParam(0);
                    int user_uid = (int)msg.GetParam(1);
                    int player_index = (short)msg.GetParam(2);
                    bool ready = (bool)msg.GetParam(3);

                    if (CurrentGameRoomInfoUI.RoomId == room_id)
                    {
                        CurrentGameRoomInfoUI.SetReady(player_index, ready);
                    }

                    break;
                }

            case MessageType.NOTIFY_ROOM_SET_TYPE:
                {
                    int room_id = (short)msg.GetParam(0);
                    int player_index = (short)msg.GetParam(1);
                    byte user_type = (byte)msg.GetParam(2);

                    if (CurrentGameRoomInfoUI.RoomId == room_id)
                    {
                        CurrentGameRoomInfoUI.SetPlayerType(player_index, (UserType)user_type);
                    }

                    break;
                }

            case MessageType.RES_CREATE_ROOM_INFO:
                {
                    int result = (byte)msg.GetParam(2);

                    if(result == 1)
                    {
                        int map_id = (int)msg.GetParam(1);

                        Map map = world.GetMapData(map_id);

                        CurrentGameRoomInfoUI.RoomId = (short)msg.GetParam(0);
                        CurrentGameRoomInfoUI.MapId = map_id;
                        CurrentGameRoomInfoUI.HostIndex = 0;
                        CurrentGameRoomInfoUI.MyIndex = 0;
                        CurrentGameRoomInfoUI.MapImage.sprite = Resources.Load<Sprite>(map.Thumbnail);
                        CurrentGameRoomInfoUI.MapName.text = map.Name;

                        CurrentGameRoomInfoUI.players[0].name = world.GetMyUser().username;
                        CurrentGameRoomInfoUI.players[0].usertype = UserType.Player;
                        CurrentGameRoomInfoUI.players[0].SetReady(true);
                        CurrentGameRoomInfoUI.players[0].dbPlayer.options.Add(new Dropdown.OptionData() { text = world.GetMyUser().username });
                        CurrentGameRoomInfoUI.players[0].dbPlayer.value = (int)UserType.Player;

                        for(int i=1;i<8;i++)
                        {
                            CurrentGameRoomInfoUI.players[i].dbPlayer.interactable = true;
                        }
                        
                        RefreshGameRoom();
                        CurrentGameRoomInfoUI.Show();
                    }
                    else
                    {
                        Debug.Log("failed to make gameroom");
                    }
                    

                    break;
                }

            case MessageType.RES_GAME_START:
                {
                    int result = (byte)msg.GetParam(1);

                    if(result >= 1)
                    {
                        /*
                        Message start_msg = new Message(MessageType.GAMEROOM_START);
                        start_msg.Push(CurrentGameRoomInfoUI.RoomId);
                        start_msg.Push(CurrentGameRoomInfoUI.MyIndex);

                        world.MessageQueue.Push(start_msg);

                        SceneManager.LoadScene("TrackScene");

                        Debug.Log("Game Start!");*/
                    }
                    else
                    {
                        Debug.Log("Not ready!");
                    }
                    break;
                }
            case MessageType.RES_ROOM_INFO:
                {
                    Map map = world.GetMapData((int)msg.GetParam(1));

                    GameRoomUI room = Instantiate(gameroomPrefab, Vector3.zero, Quaternion.identity) as GameRoomUI;
                    
                    room.RoomId = (short)msg.GetParam(0);
                    room.map_id = map.Id;
                    room.MapName.text = map.Name;
                    room.MapImage.sprite = Resources.Load<Sprite>(map.Thumbnail);
                    room.count = (short)msg.GetParam(2);
                    room.gameui = this;
                    room.transform.parent = gameRoomScrollView.transform;

                    room.transform.localPosition = new Vector3((-240.0f + 200 * room_count) + 340.0f, -170.0f, 0.0f);
                    room.transform.localScale = new Vector3(1.0f, 1.0f);

                    room_count++;

                    break;
                }
            case MessageType.RES_ROOM_PLAYER_ENTER:
                {
                    byte result = (byte)msg.GetParam(2);

                    Debug.Log("join result : " + result);

                    if(result == 1)
                    {
                        int map_id = (int)msg.GetParam(1);
                        int index = (short)msg.GetParam(3);

                        Map map = world.GetMapData(map_id);
                        CurrentGameRoomInfoUI.RoomId = (short)msg.GetParam(0);
                        CurrentGameRoomInfoUI.MapName.text = map.Name;
                        CurrentGameRoomInfoUI.MapImage.sprite = Resources.Load<Sprite>(map.Thumbnail);
                        CurrentGameRoomInfoUI.MyIndex = index;
                        CurrentGameRoomInfoUI.AddPlayer(index, world.GetMyUser().username, UserType.Player);
                        CurrentGameRoomInfoUI.Show();

                        Debug.Log("Join to room");
                    }
                    else
                    {
                        Debug.Log("Room is full");
                    }
                    break;
                }
        }
    }

    private IEnumerator checkMessageQueue()
    {
        WaitForSeconds waitSec = new WaitForSeconds(0.01f);

        while (true)
        {
            Message msg = MessageQueue.getInstance.getData();

            if (msg != null)
                ProcessMessage(msg);

            yield return waitSec;
        }

        yield return 0;
    }

}
