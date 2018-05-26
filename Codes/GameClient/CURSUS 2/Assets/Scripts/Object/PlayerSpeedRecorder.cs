using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpeedRecorder : MonoBehaviour {
    private GameObject player_object;
    private BikeControl bikeController;

    private List<byte> speeds;
    private float speed;
    public bool update;

    IEnumerator UpdateRecord()
    {
        while(update)
        {
            speeds.Add((byte)bikeController.MoveSpeed);
            yield return new WaitForSeconds(5.0f);
        }
    }

    private void Awake()
    {
        speeds = new List<byte>();
    }

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public List<byte> GetRecord() { return speeds; }

    public void StartRecord()
    {
        Debug.Log("Player Speed record start");

        player_object = GameObject.FindWithTag("Player");
        bikeController = player_object.GetComponent<BikeControl>();

        update = true;
        StartCoroutine(UpdateRecord());
    }

    public void StopRecord()
    {
        Debug.Log("Player Speed record end");
        update = false;
        StopCoroutine(UpdateRecord());
    }

    public void SetSpeed(float v)
    {
        speed = v;
    }
}
