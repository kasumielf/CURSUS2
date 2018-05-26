using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangangResult_OnOff : MonoBehaviour {

    public GameObject HangangResultobj;
    public bool Result = false;
    // Use this for initialization
    void Start ()
    {

        Result = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (!Result)
        {
            // 이외의 상황일시 Off
            if (Timer.WatchLevel < 5)
            {
                HangangResultobj.SetActive(false);
                Result = false;
            }
            // 플레이어가 종료선 통과 시 결과창 On
            if (Timer.WatchLevel >= 5 && Result == false)
            {
                HangangResultobj.SetActive(true);
                Result = true;
            }
        }
    }
}
