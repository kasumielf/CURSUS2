using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeatherDisplayUI : MonoBehaviour {
    public Text Weather;
    public Text WindSpeed;
    public Text Temper;
    public Text Humidity;
    public TextMesh HUDTemper;

    public GameObject WeatherIconMesh;
    // 0 : Sun, 1 : Cloud 2 : Rain 3 : Snow
    public Material[] WeatherIcons;
    
    Dictionary<float, string> weatherInfo;

    private void Awake()
    {
        weatherInfo = new Dictionary<float, string>();

        weatherInfo.Add(0.0f, "맑음");
        weatherInfo.Add(2.0f, "구름(적음)");
        weatherInfo.Add(3.0f, "구름(많음)");
        weatherInfo.Add(4.0f, "흐림");
    }

    // Use this for initialization
    void Start () {
        SetWeatherIcon(0);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void SetWeatherIcon(int v)
    {
        if(v < WeatherIcons.Length && WeatherIconMesh != null)
            WeatherIconMesh.GetComponent<Renderer>().material = WeatherIcons[v];
    }

    public void DisplayUpdate(int weather_1, float weather_2, float windspeed, float temper, float humidity)
    {
        switch (weather_1)
        {
            case 1:
                {
                    Weather.text = "비 " + weather_2.ToString() + "mm/h";
                    SetWeatherIcon(2);
                    break;
                }
            case 2:
                {
                    Weather.text = "눈 " + weather_2.ToString() + "mm/h";
                    SetWeatherIcon(3);
                    break;
                }
            case 3:
                {
                    Weather.text = "눈 " + weather_2.ToString() + "mm/h";
                    SetWeatherIcon(3);
                    break;
                }
            default:
                {
                    Weather.text = weatherInfo[weather_2];

                    if(weather_2 > 0.0f)
                    {
                        SetWeatherIcon(1);
                    }
                    else
                    {
                        SetWeatherIcon(0);
                    }

                    break;
                }
        }

        WindSpeed.text = windspeed.ToString();
        Temper.text = temper.ToString();
        HUDTemper.text = temper.ToString();
        Humidity.text = humidity.ToString();
    }
}
