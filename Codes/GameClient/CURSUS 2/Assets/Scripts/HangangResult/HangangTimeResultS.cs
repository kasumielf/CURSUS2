using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangangTimeResultS : MonoBehaviour {

    public TextMesh _TimeText;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        _TimeText.text = Timer.Fullsecond.ToString("00");
    }
}
