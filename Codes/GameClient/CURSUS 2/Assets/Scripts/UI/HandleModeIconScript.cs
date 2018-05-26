using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandleModeIconScript : MonoBehaviour {
    public GameObject image;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetIcon(bool set)
    {
        image.SetActive(set);
    }
}
