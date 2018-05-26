using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeControl : MonoBehaviour {
//public class BikeControl : Singleton<BikeControl>{
	public bool is_ant_used{get; set;}

    public float MoveSpeed = 0.0f;
    public float MaxSpeed = 200.0f;
    public float RotateSpeed = 30f;
    public float Angle = 0.0f;
    public bool Idle { get; set; }

    private float distance = 0;
    private Vector3[] my_path;
    private float pathLength;
    public bool path_front = true;

    public bool Auto { get; set; }
    public TextMesh VelocityText;

    //public float DownSpeed = -0.1f;
    //public CharacterController Bike;

    private bool HandleMode = false;

	public void Reset()
	{
        Idle = false;

		distance = 0.0f;
		MoveSpeed = 0.0f;

		distance += MoveSpeed * Time.deltaTime;
		float percentage = (distance / pathLength) % 1.0f;

		iTween.PutOnPath(gameObject, my_path, percentage);
		transform.LookAt(iTween.PointOnPath(my_path, percentage + .01f));
	}

    public float GetDistance()
    {
        return distance;
    }

    public void SetMyPath(Vector3[] path)
    {
        my_path = path;
        pathLength = iTween.PathLength(my_path);

        StartCoroutine(PlayerMoveUpdate());
        gameObject.GetComponent<Rigidbody>().isKinematic = !HandleMode;
    }

    void Awake()
    {
        Auto = false;
        //DontDestroyOnLoad(this.gameObject);
    }
    // Use this for initialization
    void Start () {
    }

    public void ToggleHandleMode()
    {
        HandleMode = !HandleMode;
        gameObject.GetComponent<Rigidbody>().isKinematic = !HandleMode;
    }

    public bool IsHandleMode
    {
        get
        {
            return HandleMode;
        }
    }

    IEnumerator PlayerMoveUpdate()
    {
        while(true)
        {
            if (Auto && Idle == false)
            {
                if (MoveSpeed < 15.0f) //전진키 입력 지속 시 가속 
                {
                    MoveSpeed = 20.0f;
                }
            }

            if (!IsHandleMode && !Idle)
            {
                distance += MoveSpeed * Time.deltaTime;
                float percentage = (distance / pathLength) % 1.0f;

                iTween.PutOnPath(gameObject, my_path, percentage);
                transform.LookAt(iTween.PointOnPath(my_path, percentage + .01f));
            }

            yield return new WaitForEndOfFrame();
        }
    }

	// Update is called once per frame
	void Update () {
        VelocityText.text = MoveSpeed.ToString("N2");

        if (Input.GetKey(KeyCode.F1))
        {
            Auto = !Auto;
            Idle = false;
        }

        if (Idle == false)
        {
            if (Input.GetKey(KeyCode.W))             //전진
            {
                //Bike.Move(transform.forward * MoveSpeed *Time.deltaTime);

                if(IsHandleMode)
                {
                    this.transform.Translate(Vector3.forward * MoveSpeed * Time.deltaTime);
                }
                else
                {
                }
                if (MoveSpeed < MaxSpeed) //전진키 입력 지속 시 가속 
                {
                    MoveSpeed = 30.0f;
                }
            }
            if (MoveSpeed > 0.0f && !Input.GetKey(KeyCode.W)) //전진 키 미입력 시 감속
            {
                if (IsHandleMode)
                {
                    this.transform.Translate(Vector3.forward * MoveSpeed * Time.deltaTime);
                }
                else
                {
                }
                
                MoveSpeed -= 0.1f;
                if (MoveSpeed < 0.0f)
                {
                    MoveSpeed = 0.0f;
                }
            }

            if (Input.GetKey(KeyCode.S))            //후진
            {
                //transform.Translate(0, 0, -1f);
                //Bike.Move(transform.forward * - 1f * MoveSpeed *Time.deltaTime);
                //this.transform.Translate(Vector3.back * MoveSpeed * Time.deltaTime);
                this.transform.Translate(Vector3.back * 5.0f * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.A))            //좌회전
            {
                //transform.Rotate(0, -1f, 0);
                //Bike.Move(transform.right * -1f * RotateSpeed * Time.deltaTime);
                this.transform.Rotate(0.0f, -1.0f * RotateSpeed * Time.deltaTime, 0.0f);
            }
            if (Input.GetKey(KeyCode.D))            //우회전
            {
                //Bike.Move(transform.right * RotateSpeed * Time.deltaTime);
                this.transform.Rotate(0.0f, RotateSpeed * Time.deltaTime, 0.0f);
            }
        }
    }

}
