using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Objects;

public class GameRoomInfoUI : MonoBehaviour {
    public TrackGameSelectUI gameui;

    public int RoomId { get; set; }
    public int MapId { get; set; }
    public Image MapImage;
    public Text MapName;
    public GameRoomPlayer[] players;
    public int HostIndex { get; set; }
    public int MyIndex { get; set; }
    public Button StartButton;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(HostIndex == MyIndex)
        {
            StartButton.gameObject.SetActive(true);
        }
        else
        {
            StartButton.gameObject.SetActive(false);
        }
	}

    public void SetReady(int position, bool ready)
    {
        players[position].SetReady(ready);
    }

    public void SetPlayerType(int position, UserType type)
    {
        players[position].usertype = type;
        players[position].dbPlayer.value = (int)type;
    }

    public void AddPlayer(int position,  string username, UserType user_type)
    {
        players[position].username = username;
        players[position].usertype = UserType.Player;
        players[position].SetReady(false);
        players[position].dbPlayer.options.Add(new Dropdown.OptionData() { text = username });
        players[position].dbPlayer.value = (int)UserType.Player;
    }
    public void RemovePlayer(int position)
    {
        if(players[position] != null)
        {
            players[position].dbPlayer.options.RemoveAt(3);

            if (position == MyIndex)
            {
                gameObject.SetActive(false);
                RoomId = -1;
                MyIndex = -1;
            }
        }
    }

    public void onClickGameStart()
    {
        gameui.GameStart(RoomId);
    }

    public void onClickLeave()
    {
        gameui.LeaveGame(RoomId);

        Hide();
    }

    public void onValueChangePlayerType(int value)
    {
        if(HostIndex == MyIndex)
        {
            gameui.SetPlayerType(RoomId, value, (UserType)players[value].dbPlayer.value);
        }
    }

    public void onClickReadyButton(int value)
    {
        gameui.SetReady(value, RoomId, MyIndex);
    }

    public void Show() {
        this.gameObject.SetActive(true);
    }
    public void Hide()
    {
        int len = players.Length;
        for(int i=0;i<len;++i)
        {
            players[i].usertype = UserType.Open;
        }
        this.gameObject.SetActive(false);
    }
}
