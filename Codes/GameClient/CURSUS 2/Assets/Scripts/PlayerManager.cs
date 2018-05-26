using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    protected PlayerManager() { } //항상 싱글톤 - 생성자 사용 불가


    void Awake()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject); // 해당 스크립트를 삭제
            return;
        }

        _instance = this;
        //DontDestroyOnLoad(this);
        Application.targetFrameRate = 60; //최대 프레임수를 60으로 지정

        //DontDestroyOnLoad(this.gameObject);
        //DontDestroyOnLoad(gameObject); //씬 전환되어도 파괴 안됨
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }
}
