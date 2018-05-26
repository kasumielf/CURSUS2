using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

    public Animator player_animator;
    public BikeControl bikeController;
    private float speed;

    // Use this for initialization
    void Start () {
        if (bikeController == null)
        {
            bikeController = GameObject.FindWithTag("Player").GetComponent<BikeControl>();
        }
    }
	
	// Update is called once per frame
	void Update () {
        speed = bikeController.MoveSpeed;

        if (Input.GetKeyDown(KeyCode.W))
        {
            player_animator.SetBool("start", true);
            player_animator.SetBool("work", true);
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            player_animator.SetBool("work", false);
        }

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

        if (Input.GetKeyDown(KeyCode.A))
        {
            player_animator.SetBool("left", true);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            player_animator.SetBool("left", false);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            player_animator.SetBool("right", true);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            player_animator.SetBool("right", false);
        }
        //if(Input.GetKeyDown(KeyCode.Keypad6))
        //{
        //    player_animator.SetBool("is_hand_up", false);
        //
        //}
    }
}
