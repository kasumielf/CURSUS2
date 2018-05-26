using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyPlayerPrefab : BaseNPCPlayer
{
    private float distance;
    private Vector3[] my_path;
    private float pathLength;
    public bool path_front;
    private bool ready;


    void Awake()
    {
        ready = false;
        path_front = true;
        //DontDestroyOnLoad(this.gameObject);
    }
    // Use this for initialization
    void Start()
    {
    }

    public void SetReady(float _speed, Vector3[] path)
    {
        Speed = _speed;
        my_path = path;
        pathLength = iTween.PathLength(my_path);
        ready = true;
        distance = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (ready)
        {
            distance += Speed * Time.deltaTime;
            float percentage = distance / pathLength;

            iTween.PutOnPath(gameObject, my_path, percentage);
            transform.LookAt(iTween.PointOnPath(my_path, percentage + .02f));
        }
    }

    public void onEnterEndPoint()
    {
    }
}