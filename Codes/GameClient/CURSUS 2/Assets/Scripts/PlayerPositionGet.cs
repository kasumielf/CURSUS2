using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionGet : MonoBehaviour {
    //public Transform playerPos;
    public GameObject player_object;

    // Use this for initialization
    void Start ()
    {
        //playerPos = GameObject.FindWithTag("Player").GetComponent<Transform>();
        player_object = GameObject.FindWithTag("Player");//.GetComponent<Transform>() as GameObject;
    }
	
	// Update is called once per frame
	void Update ()
    {


        //var PlayerPos = player_object.GetComponents<Transform>();
        var PlayerPosX = player_object.transform.position.x;    //Player x좌표값
        var PlayerPosY = player_object.transform.position.y;    //Player y좌표값
        var PlayerPosZ = player_object.transform.position.z;    //Player z좌표값
    }
}
