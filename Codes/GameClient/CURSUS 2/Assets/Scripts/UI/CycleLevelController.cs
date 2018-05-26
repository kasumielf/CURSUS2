using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CycleLevelController : MonoBehaviour {
    public Slider roller_slider;
	public GameObject player;

	private SerialPortController spController;
    private int current_index;
    private int min_index;
    private int max_index;

    private void Awake()
    {
    }

    // Use this for initialization
    void Start ()
    {
        min_index = (int)roller_slider.minValue;
        max_index = (int)roller_slider.maxValue;

        current_index = 3;
        roller_slider.value = current_index;

		spController = player.GetComponent<SerialPortController> ();
    }

    // Update is called once per frame
    void Update () {

        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
			UpPress ();
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
			DownPress ();
        }
        else
        {
        }
	}

	public void UpPress()
	{
		if (++current_index > max_index)
			current_index = max_index;

		roller_slider.value = current_index;
		spController.SetLevel(current_index);
	}

	public void DownPress()
	{
		if (--current_index < min_index)
			current_index = min_index;

		roller_slider.value = current_index;
		spController.SetLevel(current_index);
	}


    public void onchangeSlider(Int32 v)
    {
		//spController.SetLevel(v);
    }
}
