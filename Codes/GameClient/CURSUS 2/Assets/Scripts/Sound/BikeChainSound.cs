using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeChainSound : MonoBehaviour
{
    private PlayerSpeedGet BikeSpeed;
    private AudioSource audio;
    public AudioClip ChainSound;
    bool SoundOn = false;

    // Use this for initialization
    void Start () {
        this.audio = this.gameObject.AddComponent<AudioSource>();
        this.audio.clip = this.ChainSound;
        this.audio.volume = 0.3f;
        this.audio.loop = true;
    }

// Update is called once per frame
void Update () {
        float BikeSpeed = gameObject.GetComponent<PlayerSpeedGet>().BikeSpeed;
        double Machine = gameObject.GetComponent<BikeControlMachine>().Machine;

        //기계에서 받아온 값으로 분별
        if (BikeSpeed != 0.0f && Machine == 0 && SoundOn == false)
        {
            SoundOn = true;
            this.audio.Play();
        }
        if (BikeSpeed == 0.0f || Machine != 0)
        {
            SoundOn = false;
            this.audio.Stop();
        }

        //키보드 키로 분별
        if (BikeSpeed != 0.0f && (!Input.GetKey(KeyCode.W) || !Input.GetKey(KeyCode.S)) && SoundOn == false)
        {
            SoundOn = true;
            this.audio.Play();
        }
        if (BikeSpeed == 0.0f || (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)))
        {
            SoundOn = false;
            this.audio.Stop();
        }

    }
}
