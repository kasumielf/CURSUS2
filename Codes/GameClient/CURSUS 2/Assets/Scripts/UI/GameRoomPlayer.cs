using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum UserType
{
    Open,
    Computer,
    Closed,
    Player,
}

public class GameRoomPlayer : MonoBehaviour {
    public string username { get; set; }
    public UserType usertype { get; set; }
    private bool ready;

    public Image imgUser;
    public Dropdown dbPlayer;
    public Button btnSetReady;
    private ColorBlock redColor;
    private ColorBlock greenColor;

	// Use this for initialization
	void Start () {
        username = "";
        usertype = UserType.Open;

        redColor = btnSetReady.colors;
        greenColor = btnSetReady.colors;

        redColor.normalColor = Color.red;
        greenColor.normalColor = Color.green;
    }
	
	// Update is called once per frame
	void Update () {
        if(ready)
            btnSetReady.colors = greenColor;
        else
            btnSetReady.colors = redColor;
	}

    public void SetReady(bool _ready)
    {
        ready = _ready;
    }
}
