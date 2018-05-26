using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public Transform target;    //추적대상 = 플레이어
    public float dist = 0.0f;
    public float height = 1.7f;
    public float dampRotate = 5.0f;

    private Transform tr;

    //카메라 방향좌표
    private float x;
    private float y;

    //카메라 마우스회전속도
    private float xspeed = 200f;
    private float yspeed = 200f;


    //GameObject player;
    //Vector3 offset;
    public float RotateSpeed = 30f;

    // Use this for initialization
    void Start () {
        tr = GetComponent<Transform>(); //변화 컴포넌트 겟.

        Vector3 angles = transform.eulerAngles;
        x = angles.x;
        y = angles.y;

        //player = GameObject.FindWithTag("Player");
        //offset = player.transform.position - transform.position;
    }
	
	// Update is called once per frame
	void Update ()
    {
    }

    private void LateUpdate()
    {
        float currYAngle = Mathf.LerpAngle(tr.eulerAngles.y, target.eulerAngles.y, dampRotate * Time.deltaTime);

        Quaternion rot = Quaternion.Euler(0, currYAngle, 0);

        tr.position = target.position - (rot * Vector3.forward * dist) + (Vector3.up * height);
        //tr.LookAt(target.position);

        //카메라 회전 속도
        x += Input.GetAxis("Mouse X") * xspeed * 0.02f;
        y -= Input.GetAxis("Mouse Y") * yspeed * 0.02f;

        if (Input.GetKey(KeyCode.A))            //좌회전
        {
            x += -1.0f * RotateSpeed * Time.deltaTime;
            //this.transform.Rotate(0.0f, -1.0f * RotateSpeed * Time.deltaTime, 0.0f);
        }
        if (Input.GetKey(KeyCode.D))            //우회전
        {
            x += RotateSpeed * Time.deltaTime;
            //this.transform.Rotate(0.0f, RotateSpeed * Time.deltaTime, 0.0f);
        }

        Quaternion rotation = Quaternion.Euler(y, x, 0);

        transform.rotation = rotation;
        ////Quaternion rot = Quaternion.Euler (0, currY)
        //transform.position = player.transform.position - offset;
        ////transform.position = player.transform.position - offset;
    }
}
