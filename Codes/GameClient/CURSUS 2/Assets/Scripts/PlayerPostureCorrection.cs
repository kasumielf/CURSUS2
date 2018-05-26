using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPostureCorrection : MonoBehaviour {
    float RotZ;
    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        RotZ = this.transform.eulerAngles.z;
        //플레이어가 넘어지면 바로 세우기.
        //if(RotZ != 0)
        //{
            RotZ = 0;
        //}
	}
}
