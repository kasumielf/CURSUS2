using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMakePopup : MonoBehaviour {
    public int SelectedMapId { get; set; }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
