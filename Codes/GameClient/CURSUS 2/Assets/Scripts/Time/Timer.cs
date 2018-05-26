using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Timer : MonoBehaviour {

    public static float Second, Fullsecond; //구간별 시간, 전체 시간
    public static float Minute, Fullminute;
    //public static float Hour;
    public  static bool TimerOn = false;

    //안양천합수부-성산대교0 / 성산대교-양화대교1 / 양화대교-샛강2 / 샛강-서강대교3 / 서강대교-마포대교4 / 안양천합수부-마포대교 // /시/,분,초
    //public float[,] Stopwatch = new float[6,3] { { 0, 0, 0}, { 0, 0, 0}, { 0, 0, 0}, { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
    public static float[,] Stopwatch = new float[6, 2] { { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 } };
    public static int WatchLevel = 0; //구간 통과시 ++ 
    public bool TimeCheck = false;

    // Use this for initialization
    void Start () {
        Second = 0;
        Fullsecond = 0;
        Minute = 0;
        Fullminute = 0;
        //Hour = 0;
        WatchLevel = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(TimerOn == false && this.transform.position.z >= 57.0f && this.transform.position.z < 60.0f) // 출발선 통과 시 타이머 켜기
        {
            Second = 0;
            Fullsecond = 0;
            Minute = 0;
            Fullminute = 0;
            //Hour = 0;
            WatchLevel = 0;
            TimerOn = true; //초기화 후 켬
        }
        if (TimerOn == true) //타이머가 켜져있으면 작동
        {
            Second += Time.deltaTime; //초마다 증가
            Fullsecond += Time.deltaTime;

            if (Second > 59.5) //초->분 변환
            {
                Minute ++;
                Second -= 60;
            }
            if (Fullsecond >= 60) //초->분 변환
            {
                Fullminute++;
                Fullsecond -= 60;
            }
            //if (Minute > 59.5) //분->시 변환
            //{
            //    Hour ++;
            //    Minute -= 60;
            //}
            if (this.transform.position.z >= 5750) //종료 선 통과 시 타이머 정지
            {
                TimerOn = false;
            }
        }

        //일정 구간 달성 시 배열에 기록.
        if (this.transform.position.z >= 1080 && WatchLevel == 0)   //안양합수부 - 성산대교0
        {
            TimeCheck = true;
        }
        if (this.transform.position.z >= 2560 && WatchLevel == 1)   //성산대교-양화대교1
        {
            TimeCheck = true;
        }
        if (this.transform.position.z >= 3620 && WatchLevel == 2)   //양화대교-샛강2
        {
            TimeCheck = true;
        }
        if (this.transform.position.z >= 4865 && WatchLevel == 3)   //샛강-서강대교3
        {
            TimeCheck = true;
        }
        if (this.transform.position.z >= 5730 && WatchLevel == 4)   //서강대교-마포대교4
        {
            TimeCheck = true;
        }

        if (TimeCheck == true) //구간별 시간 기록
        {
            //Stopwatch[WatchLevel, 0] = Hour;
            Stopwatch[WatchLevel, 0] = Minute;
            Stopwatch[WatchLevel, 1] = Second;

            Second = 0; //기록 후 시간 초기화
            Minute = 0;

            WatchLevel++;

            if( WatchLevel >= 5)    //주행 시간 기록
            {
                Stopwatch[WatchLevel, 0] = Fullminute;
                Stopwatch[WatchLevel, 1] = Fullsecond;
            }
            TimeCheck = false;
        }
    }
}
