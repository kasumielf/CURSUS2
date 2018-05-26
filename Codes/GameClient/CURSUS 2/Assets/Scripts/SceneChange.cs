using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour {

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
    public void SceneChange1()
    {
        //if (Input.GetKey(KeyCode.Alpha1))
        //{
            SceneManager.LoadScene("TrackScene");
        //}
    }
    public void SceneChange2()
    {
        //if (Input.GetKey(KeyCode.Alpha0))
        //{
            SceneManager.LoadScene("OpenWorldScene");
        //}
    }
    public void SceneChange3()
    {
        //if (Input.GetKey(KeyCode.Alpha0))
        //{

        SceneManager.LoadScene("Cycle-HanGangScene");
        //}
    }

}
