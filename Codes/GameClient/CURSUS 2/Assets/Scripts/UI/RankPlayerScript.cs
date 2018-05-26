using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankPlayerScript : MonoBehaviour {
    public Text Name;
    public Text Speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void SetPlayer(string name, float speed)
    {
        Name.text = name;
    }

    void SetSpeed(float speed)
    {
        Speed.text = speed.ToString("N2");
    }
}
