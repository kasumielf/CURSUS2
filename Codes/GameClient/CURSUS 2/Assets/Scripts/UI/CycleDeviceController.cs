using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CycleDeviceController : MonoBehaviour {
    public GameObject player;
    public Dropdown port_dropdown;
    public Image sign_light;

	// Use this for initialization
	void Start () {
        sign_light.color = Color.red;
        player.GetComponent<SerialPortController>().Connect(port_dropdown.value, onConnectSuccess, onFailedSuccess);
    }
	
	// Update is called once per frame
	void Update () {
	}

    public void onchangeSlider(Int32 val)
    {
        Debug.Log("Port Set to " + val);

        player.GetComponent<SerialPortController>().Connect(val, onConnectSuccess, onFailedSuccess);
    }

    public void onConnectSuccess()
    {
        Debug.Log("CycleDevice Connection Success");
        sign_light.color = Color.green;

    }

    public void onFailedSuccess()
    {
        Debug.Log("CycleDevice Connection Failed");
        sign_light.color = Color.red;
    }

}
