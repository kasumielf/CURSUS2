using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour {
    public Text Message;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Close() {
        gameObject.SetActive(false);
    }

    public void Show(string message)  {
        gameObject.SetActive(true);
        Message.text = message;
    }
}
