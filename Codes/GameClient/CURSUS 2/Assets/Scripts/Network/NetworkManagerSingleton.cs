using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManagerSingleton : Singleton<NetworkManager>
{
    protected NetworkManagerSingleton() { } //항상 싱글톤 - 생성자 사용 불가

    public NetworkManager Instance
    {
        get
        {
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject); // 해당 스크립트를 삭제
            return;
        }

        _instance = new NetworkManager();
        DontDestroyOnLoad(this);
        Application.targetFrameRate = 60; //최대 프레임수를 60으로 지정
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}
