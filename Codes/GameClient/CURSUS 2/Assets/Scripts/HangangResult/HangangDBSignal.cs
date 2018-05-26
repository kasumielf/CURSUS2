using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangangDBSignal : MonoBehaviour
{

    public OpenWorld world;

    public static bool DBCheck = false; //한강맵 결과창 무한 갱신 방지
    //안양천합수부-성산대교0 / 성산대교-양화대교1 / 양화대교-샛강2 / 샛강-서강대교3 / 서강대교-마포대교4 / 안양천합수부-마포대교5(총 주행 시간) // 분,초

    //플레이어 구간별 최고 기록 / 이전 주행 시간.
    public float[,] PlayerDBSection = new float[10, 2] { { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 } };   //DB용
    public float[,] PlayerOldSection = new float[10, 2] { { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 } };  //결과비교용

    //플레이어 구간별 현재 기록 / 현재 주행 시간. - Timer.Stopwatch
    //public static float[,] Stopwatch = new float[6, 2] { { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 } };


    //플레이어 랭킹 분,초 / 1~5위
    public float[,] PlayerRankNum = new float[5, 2]
    { { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }};

    //플레이어 랭킹 이름 / 1~5위
    public string[,] PlayerRankName = new string[5, 1]
    { { null }, { null }, { null }, { null }, { null }};

    //현 플레이어 이름
    public string[,] PlayerName = new string[1, 1] { { null } };


    //랭킹 갱신용 임시 배열들
    public float[,] PlayerRankChange1 = new float[1, 2] { { 0, 0 } };
    public float[,] PlayerRankChange2 = new float[1, 2] { { 0, 0 } };
    public string[,] PlayerRankChangeName1 = new string[1, 1] { { null } };
    public string[,] PlayerRankChangeName2 = new string[1, 1] { { null } };

    // Use this for initialization
    void Start()
    {

        //한강맵 시작 시

        //이전 데이터들을 DB에서 불러와 PlayerDBSection , PlayerRankNum , PlayerRankName 에 저장
        /* 사용 안함
        for (int i = 0; i < 6; i++)   //플레이어 구간별 최고 기록 / 이전 주행 시간.
        {
            for (int j = 0; j < 2; j++)
            {
                PlayerDBSection[i, j] = DBB;    //DB에 저장할 것
                PlayerOldSection[i, j] = DBB;   //결과 비교하기 위해 이전 자료 저장한 것
            }
        }

        for (int i = 0; i < 5; i++)   //플레이어 랭킹 분,초 / 1~5위
        {
            PlayerRankNum[i, 0] = DBB;  //분
            PlayerRankNum[i, 1] = DBB;  //초
        }

        for (int i = 0; i < 5; i++)   //플레이어 랭킹 이름 / 1~5위
        {
            PlayerRankName[i, 0] = DB;
        }
        */

        //현 플레이어의 이름을 DB에서 불러와 PlayerName 에 저장

        if(world.GetMyUser() != null)
        {
            PlayerName[0, 0] = world.GetMyUser().username;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 플레이어가 종료선을 통과.
        if (Timer.WatchLevel >= 5 && DBCheck == false)
        {

            for (int i = 0; i < 6; i++)   //플레이어 구간별 최고 기록, 주행 시간 갱신
            {
                if (Timer.Stopwatch[5, 0] < PlayerDBSection[i, 0])   // 전보다 분이 작을때 갱신
                {
                    PlayerDBSection[i, 0] = Timer.Stopwatch[i, 0];
                    PlayerDBSection[i, 1] = Timer.Stopwatch[i, 1];
                }
                else if (Timer.Stopwatch[i, 0] == PlayerDBSection[i, 0] && Timer.Stopwatch[i, 1] < PlayerDBSection[i, 1])   // 전보다 초가 작을때 갱신
                {
                    PlayerDBSection[i, 1] = Timer.Stopwatch[i, 1];
                }
                else { }    //이외는 통과
            }
            ///////////////////////////////////////////////////////////////////////


            for (int i = 0; i < 5; i++)   //플레이어 랭킹 갱신
            {
                if (Timer.Stopwatch[5, 0] < PlayerRankNum[i, 0])   // 전보다 분이 작을때 갱신
                {
                    //임시 저장
                    PlayerRankChange1[0, 0] = PlayerRankNum[i, 0];
                    PlayerRankChange1[0, 1] = PlayerRankNum[i, 1];
                    PlayerRankChangeName1[0, 0] = PlayerRankName[i, 0];
                    //갱신
                    PlayerRankNum[i, 0] = Timer.Stopwatch[5, 0];    //분
                    PlayerRankNum[i, 1] = Timer.Stopwatch[5, 1];    //초
                    PlayerRankName[i, 0] = PlayerName[0, 0];        //이름

                    for (int x = i + 1; x < 5; x++) //랭킹 갱신 후 한줄씩 밀기
                    {
                        PlayerRankChange2[0, 0] = PlayerRankNum[x, 0];
                        PlayerRankChange2[0, 1] = PlayerRankNum[x, 1];
                        PlayerRankChangeName2[0, 0] = PlayerRankName[x, 0];

                        PlayerRankNum[x, 0] = PlayerRankChange1[0, 0];
                        PlayerRankNum[x, 1] = PlayerRankChange1[0, 1];
                        PlayerRankName[x, 0] = PlayerRankChangeName1[0, 0];
                        PlayerRankChange1[0, 0] = PlayerRankChange2[0, 0];
                        PlayerRankChange1[0, 1] = PlayerRankChange2[0, 1];
                        PlayerRankChangeName1[0, 0] = PlayerRankChangeName2[0, 0];

                    }
                    break;//플레이어 랭킹 갱신 종료
                }
                else if (Timer.Stopwatch[5, 0] == PlayerDBSection[i, 0] && Timer.Stopwatch[5, 1] < PlayerDBSection[i, 1])   // 전보다 초가 작을때 갱신
                {
                    //임시 저장
                    PlayerRankChange1[0, 0] = PlayerDBSection[i, 0];
                    PlayerRankChange1[0, 1] = PlayerDBSection[i, 1];
                    PlayerRankChangeName1[0, 0] = PlayerRankName[i, 0];
                    //갱신
                    PlayerRankNum[i, 1] = Timer.Stopwatch[5, 1]; //초
                    PlayerRankName[i, 0] = PlayerName[0, 0];        //이름
                    for (int x = i + 1; x < 5; x++) //랭킹 갱신 후 한줄씩 밀기
                    {
                        PlayerRankChange2[0, 0] = PlayerRankNum[x, 0];
                        PlayerRankChange2[0, 1] = PlayerRankNum[x, 1];
                        PlayerRankChangeName2[0, 0] = PlayerRankName[x, 0];

                        PlayerRankNum[x, 0] = PlayerRankChange1[0, 0];
                        PlayerRankNum[x, 1] = PlayerRankChange1[0, 1];
                        PlayerRankName[x, 0] = PlayerRankChangeName1[0, 0];
                        PlayerRankChange1[0, 0] = PlayerRankChange2[0, 0];
                        PlayerRankChange1[0, 1] = PlayerRankChange2[0, 1];
                        PlayerRankChangeName1[0, 0] = PlayerRankChangeName2[0, 0];
                    }
                    break;//플레이어 랭킹 갱신 종료
                }
                else { }    //이외는 통과
            }
            ///////////////////////////////////////////////////////////


            //갱신된 데이터들 DB로 전송
            // 0부터 차례대로 전송
            /* 사용 안함.
            for (int i = 0; i < 6; i++)   //플레이어 구간별 최고 기록 / 주행 시간.
            {
                DBB = PlayerDBSection[i, 0];    //분
                DBB = PlayerDBSection[i, 1];    //초
            }

            for (int i = 0; i < 5; i++)   //플레이어 랭킹 분,초 / 1~5위
            {
                DBB = PlayerRankNum[i, 0];  //분
                DBB = PlayerRankNum[i, 0];  //초
            }

            for (int i = 0; i < 5; i++)   //플레이어 랭킹 이름 / 1~5위
            {
                DB = PlayerRankName[i, 0];
            }
            DBCheck = true;
            */
        }
    }
}
