using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSound : MonoBehaviour {

    private AudioSource audio;
    public AudioClip Roadsound;
    //public bool RoadsoundOn = false;
    public bool RoadsoundClose = true;

    // Use this for initialization
    void Start () {

        this.audio = this.gameObject.AddComponent<AudioSource>();
        this.audio.clip = this.Roadsound;
        this.audio.volume = 0.0f;
        this.audio.loop = true;


        //RoadsoundOn = false;
        RoadsoundClose = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (this.transform.position.z > 90 && this.transform.position.z < 95 && RoadsoundClose == true)
        {
            RoadsoundClose = false;
            this.audio.volume = 0.05f;
            this.audio.Play();
        }
        if (this.transform.position.z > 95 && this.transform.position.z < 100 && RoadsoundClose == false)
        {
            RoadsoundClose = true;
            this.audio.volume = 0.1f;
            this.audio.Play();
        }
        if (this.transform.position.z > 100 && this.transform.position.z < 105 && RoadsoundClose == true)
        {
            RoadsoundClose = false;
            this.audio.volume = 0.15f;
        }
        if (this.transform.position.z > 105 && this.transform.position.z < 110 && RoadsoundClose == false)
        {
            RoadsoundClose = true;
            this.audio.volume = 0.2f;
        }
        if (this.transform.position.z > 110 && this.transform.position.z < 115 && RoadsoundClose == true)
        {
            RoadsoundClose = false;
            this.audio.volume = 0.25f;
        }
        if (this.transform.position.z > 115 && this.transform.position.z < 1370 && RoadsoundClose == false)
        {
            RoadsoundClose = true;
            this.audio.volume = 0.3f;
        }

        if (this.transform.position.z > 1370 && this.transform.position.z < 1375 && RoadsoundClose == true)
        {
            RoadsoundClose = false;
            this.audio.volume = 0.25f;
        }
        if (this.transform.position.z > 1375 && this.transform.position.z < 1600 && RoadsoundClose == false)
        {
            RoadsoundClose = true;
            this.audio.volume = 0.2f;
        }

        if (this.transform.position.z > 1600 && this.transform.position.z < 1605 && RoadsoundClose == true)
        {
            RoadsoundClose = false;
            this.audio.volume = 0.25f;
        }
        if (this.transform.position.z > 1605 && this.transform.position.z < 1800 && RoadsoundClose == false)
        {
            RoadsoundClose = true;
            this.audio.volume = 0.3f;
        }


        if (this.transform.position.z > 1800 && this.transform.position.z < 1805 && RoadsoundClose == true)
        {
            RoadsoundClose = false;
            this.audio.volume = 0.25f;
        }
        if (this.transform.position.z > 1805 && this.transform.position.z < 2200 && RoadsoundClose == false)
        {
            RoadsoundClose = true;
            this.audio.volume = 0.2f;
        }

        if (this.transform.position.z > 2200 && this.transform.position.z < 2205 && RoadsoundClose == true)
        {
            RoadsoundClose = false;
            this.audio.volume = 0.25f;
        }
        if (this.transform.position.z > 2205 && this.transform.position.z < 2700 && RoadsoundClose == false)
        {
            RoadsoundClose = true;
            this.audio.volume = 0.3f;
        }


        if (this.transform.position.z > 2700 && this.transform.position.z < 2705 && RoadsoundClose == true)
        {
            RoadsoundClose = false;
            this.audio.volume = 0.25f;
        }
        if (this.transform.position.z > 2705 && this.transform.position.z < 3150 && RoadsoundClose == false)
        {
            RoadsoundClose = true;
            this.audio.volume = 0.2f;
        }


        if (this.transform.position.z > 3150 && this.transform.position.z < 3155 && RoadsoundClose == true)
        {
            RoadsoundClose = false;
            this.audio.volume = 0.25f;
        }
        if (this.transform.position.z > 3155 && this.transform.position.z < 4050 && RoadsoundClose == false)
        {
            RoadsoundClose = true;
            this.audio.volume = 0.3f;
        }


        if (this.transform.position.z > 4050 && this.transform.position.z < 4055 && RoadsoundClose == true)
        {
            RoadsoundClose = false;
            this.audio.volume = 0.25f;
        }
        if (this.transform.position.z > 4055 && this.transform.position.z < 4340 && RoadsoundClose == false)
        {
            RoadsoundClose = true;
            this.audio.volume = 0.2f;
        }


        if (this.transform.position.z > 4340 && this.transform.position.z < 4345 && RoadsoundClose == true)
        {
            RoadsoundClose = false;
            this.audio.volume = 0.15f;
        }
        if (this.transform.position.z > 4345 && this.transform.position.z < 4350 && RoadsoundClose == false)
        {
            RoadsoundClose = true;
            this.audio.volume = 0.1f;
        }
        if (this.transform.position.z > 4350 && this.transform.position.z < 4355 && RoadsoundClose == true)
        {
            RoadsoundClose = false;
            this.audio.volume = 0.05f;
        }
        if (this.transform.position.z > 4355 && RoadsoundClose == false)
        {
            RoadsoundClose = true;
            this.audio.Stop();
        }
    }
}
