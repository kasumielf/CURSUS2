using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TimeMinute : MonoBehaviour
{
    //Text SecondText;
    public TextMesh _TimeText;
    // Use this for initialization

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _TimeText.text = Timer.Minute.ToString("00");
    }
}
