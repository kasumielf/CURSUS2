using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSection : MonoBehaviour {

    public TextMesh _SectionText;
    public int SectionNum;
    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update ()
    {
        SectionNum = Timer.WatchLevel + 1;
        _SectionText.text = SectionNum.ToString("D1");
    }
}
