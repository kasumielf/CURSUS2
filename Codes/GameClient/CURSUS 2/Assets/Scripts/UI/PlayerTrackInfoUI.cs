using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTrackInfoUI : MonoBehaviour {
    public int index { get; set; }
    public string name { get; set; }
    public float speed { get; set; }
    public int track { get; set; }

    public Text PlayerNo;
    public Text PlayerName;
    public Text PlayerSpeed;
    public Text PlayerTrackCount;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (name == null)
            this.gameObject.SetActive(false);
        else
            this.gameObject.SetActive(true);

        PlayerNo.text = (index+1).ToString();
        PlayerName.text = name;
        PlayerSpeed.text = speed.ToString("N2");
        PlayerTrackCount.text = track.ToString();
	}
}
