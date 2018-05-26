using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void onPlayerTriggerEvent(GameObject obj);
public delegate void onOtherPlayerTriggerEvent(GameObject obj);

public class OpenWorldCheckPointTrigger : MonoBehaviour {

    public event onPlayerTriggerEvent player_trigger;
    public event onOtherPlayerTriggerEvent other_player_trigger;

    private void Awake()
    {
    }

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            if (player_trigger != null)
                player_trigger(other.gameObject);
        }
        else if (other.tag.Equals("OtherPlayer"))
        {
            if (other_player_trigger != null)
                other_player_trigger(other.gameObject);
        }
        else
        {
        }
    }
}

