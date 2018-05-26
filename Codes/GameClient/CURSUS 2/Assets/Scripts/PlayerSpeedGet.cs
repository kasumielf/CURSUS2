using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpeedGet : MonoBehaviour {

    //var fwdSpeed = Vector3.Dot(rigidbody.velocity, transform.forward);
    BikeControl bikeController;

    public float BikeSpeed;
    // Use this for initialization
    void Start () {
        bikeController = GameObject.FindWithTag("Player").GetComponent<BikeControl>();
    }
	
	// Update is called once per frame
	void Update () {

        BikeSpeed = bikeController.MoveSpeed;

    }
}
