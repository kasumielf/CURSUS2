using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimationController : MonoBehaviour {

    public Animator player_animator;
    public BaseNPCPlayer base_object;

    private float speed;

    // Use this for initialization
    void Start () {
        player_animator.SetBool("start", true);
        player_animator.SetBool("work", true);
    }

    // Update is called once per frame
    void Update () {
        if(base_object != null)
            speed = base_object.Speed;

        if (0.0f < speed && speed <= 5.0f)
        {
            player_animator.SetFloat("Speed", 1);
        }
        else if(5.0f < speed && speed <= 12.0f)
        {
            player_animator.SetFloat("Speed", 2);
        }
        else if (12.0f < speed && speed <= 20.0f)
        {
            player_animator.SetFloat("Speed", 3);
        }
        else if (20.0f < speed)
        {
            player_animator.SetFloat("Speed", 4);
        }
        else if (speed == 0.0f)
        {
            player_animator.SetBool("start", false);
            player_animator.SetFloat("Speed", 0);
        }
        else
        {
        }
    }
}
