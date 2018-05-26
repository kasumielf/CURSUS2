using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotationGet : MonoBehaviour {

    public float RotX;
    public float PRad;  //플레이어 각도

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update ()
    {

        //RotX = this.transform.rotation.eulerAngles.x;
        RotX = this.transform.eulerAngles.x;
        //RotX = this.transform.localEulerAngles.x; 
        //RotX = PlayerRotX;

        if(RotX>180)
        {
            PRad = -(RotX-360);
        }
        else if(RotX<0.0005)
        {
            PRad = 0;
        }
        else
        {
            PRad = -RotX;
        }
    }
}
