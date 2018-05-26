using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointTrigger : MonoBehaviour {

    public int CheckPointNumber;
    public TrackScene trackScene;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
		if (other.tag.Equals ("Player"))
		{
			trackScene.UpdateCheckpoint (CheckPointNumber);
		}
		/*
		else if (other.tag.Equals ("OtherPlayer"))
		{
			if (CheckPointNumber == 0)
			{
				other.gameObject.GetComponent<OtherPlayerPrafab> ().START ();
			}

			if (CheckPointNumber == 5) {
				other.gameObject.GetComponent<OtherPlayerPrafab> ().STOP ();

			}
		}
		else
		{
		}*/
    }
}
