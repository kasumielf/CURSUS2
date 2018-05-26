using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class STOPWATCH
{
    public int[] StopWatch;    // 하위에 넣어질 배열 변수  
}




public class TimeCheck : MonoBehaviour
{
    public STOPWATCH[] arraySTOPWATCH;

    int WatchLevel; //구간 통과시 ++ 
    public bool Timecheck = false;
    // Use this for initialization
    void Start()
    {
        WatchLevel = 0;
    }

    // Update is called once per frame
    void Update()
    {
        WatchLevel = 0;

        //일정 구간 달성 시 배열에 기록.
        if (this.transform.position.z >= 1080 && WatchLevel == 0)   //안양합수부 - 성산대교0
        {
            Timecheck = true;
        }
        if (this.transform.position.z >= 2560 && WatchLevel == 1)   //성산대교-양화대교1
        {
            Timecheck = true;
        }
        if (this.transform.position.z >= 3620 && WatchLevel == 2)   //양화대교-샛강2
        {
            Timecheck = true;
        }
        if (this.transform.position.z >= 4865 && WatchLevel == 3)   //샛강-서강대교3
        {
            Timecheck = true;
        }
        if (this.transform.position.z >= 5730 && WatchLevel == 4)   //서강대교-마포대교4
        {
            Timecheck = true;
        }

        if (Timecheck == true)
        {
            //STOPWATCH arraySTOPWATCH[WatchLevel, 0] = Timer.Hour;
            //STOPWATCH[WatchLevel, 1] = Minute;
            //STOPWATCH[WatchLevel, 2] = Second;

            WatchLevel++;
            Timecheck = false;
        }
    }
}
