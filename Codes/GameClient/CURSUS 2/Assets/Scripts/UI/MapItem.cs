using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapItem : MonoBehaviour {

    public int mapId { get; set; }
    public Image mapImage;
    public Text mapName;
    public GameMakePopup popup;
    public TrackGameSelectUI selectUi;
    private bool Selected { get; set; }
    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetMap()
    {
        popup.SelectedMapId = mapId;
        selectUi.MakeGameRoom();
    }
}
