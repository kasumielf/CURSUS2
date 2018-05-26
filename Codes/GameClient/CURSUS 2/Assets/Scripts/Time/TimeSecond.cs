using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TimeSecond : MonoBehaviour {
    //Text SecondText;
    public TextMesh _TimeText;
    //public float Second;
    //public static float Minute;
    //int Second = (int)(Time.time - curTime);
    // Use this for initialization

    void Start () {
        //Second = 0;
        //Minute = 0;
    }
	
	// Update is called once per frame
	void Update () {
        /*if(Timer.TimerOn == false && this.transform.position.z < 10)
        {
            Second = 0;
            Minute = 0;
        }
        if(Timer.TimerOn == true)
        {
            Second += Time.deltaTime;
        }
        if(Second>59)
        {
            Minute++;
            Second = 0;
        }*/

        _TimeText.text = Timer.Second.ToString("00");
        //_TimeText.text = Timer.Second.ToString("00.00");
        //_TimeText = _TimeText.Replace(".", ":");
        
        //string.Format("{0:00} : {1:00} : {2:00}", Timer.Hour, Timer.Minute, Timer.Second);
    }
}
