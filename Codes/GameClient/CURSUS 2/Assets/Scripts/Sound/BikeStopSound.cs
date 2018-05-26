using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeStopSound : MonoBehaviour
{
    private PlayerSpeedGet BikeSpeed;
    private AudioSource audio;
    public AudioClip StopSound;
    bool GoStop = false;

    // Use this for initialization
    void Start()
    {
        this.audio = this.gameObject.AddComponent<AudioSource>();
        this.audio.clip = this.StopSound;
        this.audio.volume = 0.4f;
        this.audio.loop = false;



    }

    // Update is called once per frame
    void Update()
    {
        float BikeSpeed = gameObject.GetComponent<PlayerSpeedGet>().BikeSpeed;

        if (BikeSpeed > 0.0f)
        {
            GoStop = true;
        }
        if (BikeSpeed == 0.0f && GoStop == true)
        {

            this.audio.Play();
            GoStop = false;
        }
    }
}
