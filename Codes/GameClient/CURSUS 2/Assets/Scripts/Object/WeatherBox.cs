using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherBox : MonoBehaviour {

    public RainEffect rain;
    public SnowEffect snow;
    public GameObject player;

    public Material SkyDefault;
    public Material SkyCloudy;

    Vector3 position;
	// Use this for initialization
	void Start () {
        if(player != null)
        {
            StartCoroutine(positionUpdate());
        }
    }
	
	// Update is called once per frame
	void Update () {
    }

    private void OnDestroy()
    {
        StopCoroutine(positionUpdate());
    }

    IEnumerator positionUpdate()
    {
        while(true)
        {
            position = gameObject.transform.position;

            position.x = player.transform.position.x;
            position.z = player.transform.position.z;

            gameObject.transform.position = position;
            
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void SetCloudy()
    {
        RenderSettings.fog = true;

        // Rain or Snow : RGB 100 100 100 Density : 0.1
        RenderSettings.fogColor = new Color(0.4f, 0.4f, 0.4f);
        RenderSettings.fogDensity = 0.01f;
        RenderSettings.skybox = SkyCloudy;
    }

    public void SetNormal()
    {
        
        RenderSettings.fog = true;

        // Default Fog Color : RGB 170 220 255 Density : 0.01
        RenderSettings.fogColor = new Color(0.55f, 0.8f, 1.0f);
        RenderSettings.fogDensity = 0.001f;
        RenderSettings.skybox = SkyDefault;
    }

    public void SetSnow(float amount)
    {
        rain.Hide();
        snow.Show();
        SetCloudy();
    }

    public void SetRain(float amount)
    {
        rain.Show();
        snow.Hide();
        SetCloudy();
    }
    public void SetSunny()
    {
        rain.Hide();
        snow.Hide();
        SetNormal();
    }
}
