using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Player_Instance_Iuput : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        GameObject prefab = Resources.Load("Prefabs/PlayerPrefab") as GameObject;
        // Prefabs/PlayerPrefab.prefab 로드
        GameObject Player = MonoBehaviour.Instantiate(prefab) as GameObject;
        // 실제 인스턴스 생성. GameObject name의 기본값은 Player (clone)
        Player.name = "Player"; // name을 변경
        //camera.transform.parent = Player.transform;
        // camera을 player에 입양하는등 초기화작업 수행
    }
}
