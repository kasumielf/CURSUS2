using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Daylight : MonoBehaviour {
    public Material[] skyboxes;

	public int hour; // 0 ~ 23
	public GameObject DaylightObject;
    public GameObject SunlightObject;
    public GameObject MoonlightObject;

    public Light sunlight;
	public Light sunlightOverlay;
	public Light moonlight;
	public Light moonlightOverlay;
    public Camera mainCamera;
    Skybox skybox;


    // Use this for initialization

    void RefreshDaylight()
    {
        if (hour >= 5 && hour <= 19)
        {
            skybox.material = skyboxes[hour];
            SunlightObject.SetActive(true);
        }
        else
        {
            skybox.material = skyboxes[0];
            SunlightObject.SetActive(false);
        }

        DaylightObject.transform.rotation = Quaternion.Euler((float)((hour - 6) * 15), 0, 0);
    }
    void Start () {
		hour = DateTime.Now.Hour;
        //skybox = mainCamera.GetComponent<Skybox>();

        RefreshDaylight();

        StartCoroutine(daylightUpdate());
    }

    private void OnDestroy()
    {
        StopCoroutine(daylightUpdate());
    }

    IEnumerator daylightUpdate()
    {
        while(true)
        {
            SetHour ();
            // Update Moonlight and sunglight overlay in 17:00 and 00:00
            RefreshDaylight();
            yield return new WaitForSeconds(60);
        }
    }
	
	// Update is called once per frame
	void Update ()
	{
    }

    void SetHour()
	{
		hour = DateTime.Now.Hour;
	}

	public void SetHourForTestSlider(Slider s)
	{
		if (s.value >= 0.0f && s.value <= 23.0f)
		{
			hour = (int)s.value;
		}
	}

}
