using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReplayPlayerPrefab : BaseNPCPlayer
{
    private float distance;
    private Vector3[] my_path;
    private float pathLength;
    public bool path_front;
    private bool ready;

    public Text velocity_text;
    public Text date_text;

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

    public void SetReady(double date, Vector3[] path)
    {
        my_path = path;
        pathLength = iTween.PathLength(my_path);
        distance = 0;

        System.DateTime t = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        t = t.AddSeconds(date).ToLocalTime();
        date_text.text = string.Format("{0:0000}/{1:00}/{2:00} {3:00}:{4:00}:{5:00}", t.Year, t.Month, t.Day, t.Hour, t.Minute, t.Second);

        distance += Speed * Time.deltaTime;
        float percentage = distance / pathLength;

        iTween.PutOnPath(gameObject, my_path, percentage);

        StartCoroutine(WaitingReady());
    }

    IEnumerator WaitingReady()
    {
        yield return new WaitForSeconds(5.0f);
        ready = true;
    }

    public void SetSpeed(float _speed)
    {
        Speed = _speed;
        velocity_text.text = Speed.ToString();
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