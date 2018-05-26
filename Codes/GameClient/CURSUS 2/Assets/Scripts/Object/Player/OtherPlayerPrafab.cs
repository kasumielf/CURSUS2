using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayerPrafab : BaseNPCPlayer {
    public TextMesh name;
    private float distance;
    private Vector3[] my_path;
    private float pathLength;

	private bool sss;
	private float ttt;
    // Use this for initialization
    void Start()
    {
		ttt = 0.0f;
    }

    public void SetPath(Vector3[] path)
    {
        my_path = path;
        pathLength = iTween.PathLength(my_path);
        distance = 0;
    }

	public void START()
	{
		sss = true;
		StartCoroutine (UpdateDistance ());
	}

	public void STOP()
	{
		sss = false;
	}

	IEnumerator UpdateDistance()
	{
		while(sss)
		{
			ttt += Speed;
			yield return new WaitForSeconds (1.0f);
		}

		Debug.Log ("name : " + name.text + "/ dis : " + ttt);
	}

    // Update is called once per frame
    void Update () {
        distance += Speed * Time.deltaTime;
        float percentage = (distance / pathLength) % 1.0f;
        iTween.PutOnPath(gameObject, my_path, percentage);
        transform.LookAt(iTween.PointOnPath(my_path, percentage + .01f));
    }

    public void SetPosition(float x, float y, float z)
    {
        distance = x;
        //gameObject.transform.position = new Vector3(x, z, y);
    }

    private void OnDestroy()
    {

    }
}
