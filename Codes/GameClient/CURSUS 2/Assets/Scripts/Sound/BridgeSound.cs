using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeSound : MonoBehaviour
{
    private AudioSource audio;
    public AudioClip Bridgesound;
    public bool BridgesoundDown = false;

    // Use this for initialization
    void Start()
    {
        this.audio = this.gameObject.AddComponent<AudioSource>();
        this.audio.clip = this.Bridgesound;
        this.audio.volume = 0.6f;
        this.audio.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        float BikeSpeed = gameObject.GetComponent<PlayerSpeedGet>().BikeSpeed;

        if (BikeSpeed != 0.0f)
        {
            if (this.transform.position.z > 1066 && this.transform.position.z < 1093 && BridgesoundDown == false)
            {
                BridgesoundDown = true;
                this.audio.Play();
            }
            if (this.transform.position.z > 1093 && this.transform.position.z < 2099 && BridgesoundDown == true)
            {
                BridgesoundDown = false;
                this.audio.Stop();
            }

            if (this.transform.position.z > 2099 && this.transform.position.z < 2106 && BridgesoundDown == false)
            {
                BridgesoundDown = true;
                this.audio.Play();
            }
            if (this.transform.position.z > 2016 && this.transform.position.z < 2548 && BridgesoundDown == true)
            {
                BridgesoundDown = false;
                this.audio.Stop();
            }

            if (this.transform.position.z > 2548 && this.transform.position.z < 2579 && BridgesoundDown == false)
            {
                BridgesoundDown = true;
                this.audio.Play();
            }
            if (this.transform.position.z > 2579 && this.transform.position.z < 3013 && BridgesoundDown == true)
            {
                BridgesoundDown = false;
                this.audio.Stop();
            }

            if (this.transform.position.z > 3013 && this.transform.position.z < 3024 && BridgesoundDown == false)
            {
                BridgesoundDown = true;
                this.audio.Play();
            }
            if (this.transform.position.z > 3024 && this.transform.position.z < 3956 && BridgesoundDown == true)
            {
                BridgesoundDown = false;
                this.audio.Stop();
            }

            if (this.transform.position.z > 3956 && this.transform.position.z < 3979 && BridgesoundDown == false)
            {
                this.audio.volume = 0.8f;
                BridgesoundDown = true;
                this.audio.Play();
            }
            if (this.transform.position.z > 3979 && this.transform.position.z < 4850 && BridgesoundDown == true)
            {
                this.audio.volume = 0.6f;
                BridgesoundDown = false;
                this.audio.Stop();
            }

            if (this.transform.position.z > 4850 && this.transform.position.z < 4876 && BridgesoundDown == false)
            {
                BridgesoundDown = true;
                this.audio.Play();
            }
            if (this.transform.position.z > 4876 && this.transform.position.z < 5688 && BridgesoundDown == true)
            {
                BridgesoundDown = false;
                this.audio.Stop();
            }

            if (this.transform.position.z > 5688 && this.transform.position.z < 5714 && BridgesoundDown == false)
            {
                BridgesoundDown = true;
                this.audio.Play();
            }
            if (this.transform.position.z > 5714 && this.transform.position.z < 5721 && BridgesoundDown == true)
            {
                BridgesoundDown = false;
                this.audio.Stop();
            }

            if (this.transform.position.z > 5721 && this.transform.position.z < 5747 && BridgesoundDown == false)
            {
                BridgesoundDown = true;
                this.audio.Play();
            }
            if (this.transform.position.z > 5747 && BridgesoundDown == true)
            {
                BridgesoundDown = false;
                this.audio.Stop();
            }
        }
        else
        {
            BridgesoundDown = false;
            this.audio.Stop();
        }
    }
}
