using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void onClickPlayButton(int index);
public delegate void onClickStopButton(int index);

public class ReplayButton : MonoBehaviour {

    public event onClickPlayButton play_event;
    public event onClickStopButton stop_event;

    public int Index { get; set; }
    private bool playing { get; set; }
    public Material PlayMaterial;
    public Material StopMaterial;
    public Button Button;
    public Text Text;

	// Use this for initialization
	void Start () {
        playing = false;
        RefreshButton();
        Index = -1;
        gameObject.SetActive(false);
        Button.onClick.AddListener(Toggle);
    }
	
	// Update is called once per frame
	void Update () {
	}

    public void SetTimeString(double date)
    {
        System.DateTime t = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        t = t.AddSeconds(date).ToLocalTime();
        Text.text = string.Format("{0:0000}/{1:00}/{2:00}\n{3:00}:{4:00}:{5:00}", t.Year, t.Month, t.Day, t.Hour, t.Minute, t.Second);
    }

    public void Toggle()
    {
        playing = !playing;
        RefreshButton();

        if (playing && Index >= 0)
        {
            stop_event(Index);
        }
        else
        {
            play_event(Index);
        }
    }

    private void RefreshButton()
    {
        if (playing)
        {
            //Button.GetComponent<Renderer>().material = StopMaterial;
        }
        else
        {
            //Button.GetComponent<Renderer>().material = PlayMaterial;
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}