using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangangTimecompare : MonoBehaviour {

    public TextMesh _TimeText1;
    public TextMesh _TimeText2;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        _TimeText1.text = Timer.Fullminute.ToString("00");
        _TimeText2.text = Timer.Fullminute.ToString("00");

    }
}
