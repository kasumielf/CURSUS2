using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameRoomUI : MonoBehaviour {
    public TrackGameSelectUI gameui;

    public int RoomId { get; set; }
    public int map_id { get; set; }
    public Image MapImage;
    public Text MapName;
    public Text PlayerCount;

    public int count { get; set; }
    // Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        PlayerCount.text = count.ToString();
	}

    public void onClickJoin()
    {
        if(RoomId >= 0)
            gameui.JoinGame(RoomId);
    }
}
