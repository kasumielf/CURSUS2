using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public delegate void openSuccessHandler();
public delegate void openFailedHandler();

public class SerialPortController : MonoBehaviour {
    private readonly string[] PortList = { "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9", "COM10" };
    private SerialPort sp;
    private string port;
    private int rate;
    private bool start;
    public bool left;
    private bool update;
    public int Velocity { get; set; }
    public int OldAngle { get; set; }
	public int Angle{get; set;}
	public bool DuringChangeRoller{get; set;}
	public int CurrentRollerLevel{get; set;}

	bool open = false;

	public BikeControl bikeController;

    private event openSuccessHandler open_success_handler;
    private event openFailedHandler open_faied_handler;


    private void Awake()
    {
        left = false;
        update = false;
        rate = 9600;
        Velocity = 0;
        Angle = 0;
		DuringChangeRoller = false;
		CurrentRollerLevel = 3;
    }

    // Use this for initialization
    void Start()
	{
		if(bikeController == null)
		{
			bikeController = GameObject.FindWithTag("Player").GetComponent<BikeControl>();
		}
    }

    public void Connect(int target_port_index, openSuccessHandler success, openFailedHandler failed)
    {
        port = PortList[target_port_index];
        sp = new SerialPort(port, rate);

        try
        {
            sp.Open();
        }
        catch (System.Exception)
        {
            failed();
        }
        finally
        {
            open = sp.IsOpen;
        }

        if (open)
        {
            sp.ReadTimeout = 1;
            success();
            start = open;
        }
        else
        {
            failed();
        }
    }

    // Update is called once per frame
    void Update () {
        if (start)
        {
            try
            {
                string line = sp.ReadLine();
                string[] val = line.Split(",".ToCharArray());

                //if (val.Length >= 2)
                {
                    Velocity = int.Parse(val[0]) * 5;
                    Angle = -(int.Parse(val[1]) - 180);

					if(bikeController.is_ant_used == false)
					{
						Debug.Log("Speed : " + val[0]);
						bikeController.MoveSpeed = Velocity;
					}

                    if(Angle < 0 && Angle > -6 && update == false)
                    {
                        left = !left;
                        update = true;
                    }
                    else if(Angle > 0)
                    {
                        update = false;
                    }
                }
            }
            catch (System.Exception)
            {

            }
        }
    }

	public void SetLevel(int level)
	{
		//if(DuringChangeRoller == false)
		StartCoroutine (UpdateRollerLevel (level));
	}

	IEnumerator UpdateRollerLevel(int level)
	{
		DuringChangeRoller = true;

		int modify = level - CurrentRollerLevel;
		string set_modifier = "";

		if (modify < 0)
		{
			Debug.Log ("Set Roller Down to " + modify);
			set_modifier = "d";
		}
		else if (modify > 0)
		{
			Debug.Log ("Set Roller Up to " + modify);
			set_modifier = "u";
		}
		else
		{
		}

		CurrentRollerLevel = level;

		if (set_modifier.Equals ("") != true)
		{
			int value = Math.Abs (modify);

			for (int i = 0; i < value; ++i)
			{
				sp.Write (set_modifier);
				Debug.Log ("Serial Port Roller Set " + set_modifier);
				yield return new WaitForSeconds (1.0f);
			}
		}
			
		DuringChangeRoller = false;
	}


    private void OnDestroy()
    {
        StopCoroutine(updateDeviceInfo());
        sp.Close();
    }

    IEnumerator updateDeviceInfo()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
        }
    }
}
