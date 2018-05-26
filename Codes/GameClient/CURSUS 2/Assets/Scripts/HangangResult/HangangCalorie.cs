using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangangCalorie : MonoBehaviour {

    public OpenWorld world;

    float Cal;      //소모 칼로리 결과값
    float Weight=60;   //몸무게 - 기본값은 60kg
    //float SpeedCal = 0.113;
    public TextMesh _TimeText;
    // Use this for initialization
    void Start ()
    {
        if (world.GetMyUser() != null)
        {
            Weight = world.GetMyUser().weight;
        }
        else
        {
            Weight = 70.0f;
        }
    }


    //소비 칼로리 공식
    // 몸무게 * 속도의 칼로리 소비량* 운동시간
    //
    //속도의 칼로리 소비량은 평균 22로 잡고 칼로리 소비 계수 = 113cal/kg/분
    //
    //몸무게를 DB에서 불러와 Weight에 저장.
    //운동시간은 Timer.Fullminute를 불러옴.(분 단위만)
    //
    // Update is called once per frame
    void Update () {

        Cal = Weight * Timer.Fullminute * 113;
        _TimeText.text = Cal.ToString("n0");
    }
}
