using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangangTimeResultM : MonoBehaviour {

    public TextMesh _TimeText;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        _TimeText.text = Timer.Fullminute.ToString("00");
    }
}
