using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BikeControlMachine : MonoBehaviour
{
    public SerialPortController spController;

    //public class BikeControl : Singleton<BikeControl>{
    public string player_name;
    public static float MoveSpeedM = 0.0f;
    //public float MaxSpeedM = 20.0f;
    //public float DownSpeed = -0.1f;
    public float RotateSpeedM = 30f;
    public bool turn_left = false;
    public bool Handling;

    public double Machine; //기계에서 받아오는 값 임시 변수
    public bool Idle { get; set; }
    public int MachineRot = 0; //기계에서 받아오는 핸들 회전 값 임시 변수

    //public CharacterController Bike;

    void Awake()
    {
        spController = gameObject.GetComponent<SerialPortController>();

        if (spController == null)
            spController = new SerialPortController();
        //DontDestroyOnLoad(this.gameObject);
    }
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        MachineRot = spController.Angle * (spController.left == true ? +1 : -1);


        //전진  //Machin 값은 임시 수치
        if (Idle == false)
        {
            if (Machine != 0 && Machine <= 5)  //속도 값 0초과 5이하
            {
                this.transform.Translate(Vector3.forward * MoveSpeedM * Time.deltaTime);
                if (MoveSpeedM < 5f) //5f까지 속도가 상승
                {
                    MoveSpeedM += 0.5f;
                }
                else if (MoveSpeedM > 5f) //5f까지 속도가 하락
                {
                    MoveSpeedM -= 0.5f;
                }
                else                //최고 5f 유지
                {
                    MoveSpeedM = 5f;
                }
            }
            else if (Machine > 5 && Machine <= 10)  //속도 값 5초과 10이하
            {
                this.transform.Translate(Vector3.forward * MoveSpeedM * Time.deltaTime);
                if (MoveSpeedM < 10f) //10f까지 속도가 상승
                {
                    MoveSpeedM += 0.5f;
                }
                else if (MoveSpeedM > 10f) //10f까지 속도가 하락
                {
                    MoveSpeedM -= 0.5f;
                }
                else                //최고 10f 유지
                {
                    MoveSpeedM = 10f;
                }
            }
            else if (Machine > 10 && Machine <= 15)  //속도 값 10초과 15이하
            {
                this.transform.Translate(Vector3.forward * MoveSpeedM * Time.deltaTime);
                if (MoveSpeedM < 15f) //15f까지 속도가 상승
                {
                    MoveSpeedM += 0.5f;
                }
                else if (MoveSpeedM > 15f) //15f까지 속도가 하락
                {
                    MoveSpeedM -= 0.5f;
                }
                else                //최고 15f 유지
                {
                    MoveSpeedM = 15f;
                }
            }
            else if (Machine > 15 && Machine <= 20)  //속도 값 15초과 20이하
            {
                this.transform.Translate(Vector3.forward * MoveSpeedM * Time.deltaTime);
                if (MoveSpeedM < 20f) //20f까지 속도가 상승
                {
                    MoveSpeedM += 0.5f;
                }
                else if (MoveSpeedM > 20f) //20f까지 속도가 하락
                {
                    MoveSpeedM -= 0.5f;
                }
                else                //최고 20f 유지
                {
                    MoveSpeedM = 20f;
                }
            }
            else if (Machine > 20)  //속도 값 20초과
            {
                this.transform.Translate(Vector3.forward * MoveSpeedM * Time.deltaTime);
                if (MoveSpeedM < 25f) //25f까지 속도가 상승
                {
                    MoveSpeedM += 0.5f;
                }
                else if (MoveSpeedM > 25f) //25f까지 속도가 하락
                {
                    MoveSpeedM -= 0.5f;
                }
                else                //최고 25f 유지
                {
                    MoveSpeedM = 25f;
                }
            }
            else
            { }

            if (MoveSpeedM > 0.0f && Machine == 0) //전진 중 입력 값 0일때 자연 감속
            {
                this.transform.Translate(Vector3.forward * MoveSpeedM * Time.deltaTime);
                MoveSpeedM -= 0.5f;
                if (MoveSpeedM < 0.0f)
                {
                    MoveSpeedM = 0.0f;
                }
            }

            if (Machine < 0)            //후진
            {
                if (MoveSpeedM > 0)  //전진 중에 후진 시 급감속
                {
                    MoveSpeedM -= 1.0f;
                }
                else
                {
                    this.transform.Translate(Vector3.back * 5.0f * Time.deltaTime);
                }
            }
        }


        if (Input.GetKey(KeyCode.Q) && MachineRot > -30) //키보드 조작 좌회전
        {

            MachineRot -= 1;
        }
        if (Input.GetKey(KeyCode.E) && MachineRot < 30) // 키보드 조작 우회전
        {
            MachineRot += 1;
        }

        //핸들 조작 회전. 
        //MoveSpeedM > 0 는 이동할때만 회전하도록 하는 조건문.
        if (/*MoveSpeedM > 0 &&*/ Handling == false && (MachineRot < -3 || MachineRot > 3))
        {
            Handling = true;
        }

        if(Handling == true && MachineRot >= -3 && MachineRot <= 3)
        {
            Handling = false;
        }

        if(Handling == true)
        {
            this.transform.Rotate(0.0f, 4.0f * MachineRot * Time.deltaTime, 0.0f);
        }

    }
}
