using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Utility;

public class OpenWorldCheckPoint : MonoBehaviour {

    public OpenWorld world;
    public OpenWorldCheckPointTrigger start_trigger;
    public OpenWorldCheckPointTrigger end_trigger;

    public long start_time;
    public long end_time;
    
    public long Duration { get; set; }

    private void Awake()
    {
        start_trigger.player_trigger += onPlayerStartTrigger;
        start_trigger.other_player_trigger += onOtherPlayerStartTrigger;

        end_trigger.player_trigger += onPlayerEndTrigger;
        end_trigger.other_player_trigger += onOtherPlayerEndTrigger;
    }

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnDestroy()
    {
    }

    public void onPlayerStartTrigger(GameObject obj)
    {
        obj.GetComponent<PlayerSpeedRecorder>().StartRecord();

        start_time = System.DateTime.Now.Ticks;
    }

    public void onPlayerEndTrigger(GameObject obj)
    {
        obj.GetComponent<PlayerSpeedRecorder>().StopRecord();
        obj.GetComponent<BikeControl>().Idle = true;

        world.RecordMyReplay(obj.GetComponent<PlayerSpeedRecorder>().GetRecord());

        end_time = System.DateTime.Now.Ticks;

        Duration = end_time - start_time;

        System.TimeSpan span = new System.TimeSpan(Duration);

        world.rankPage.SetMyCurrentRecord(span.TotalMilliseconds);
		world.SetActiveResetButton (true);

        Debug.Log("Duration : " + span.TotalMilliseconds);

        Packet.PACKET_REQ_UPDATE_RECORD_DATA packet = new Packet.PACKET_REQ_UPDATE_RECORD_DATA();
        packet.Init();
        packet.record_time = span.TotalMilliseconds;
        packet.checked_time = span.TotalMilliseconds;

        GameObject.FindWithTag("Network").GetComponent<NetworkManagerSingleton>().Instance.Send(Utility.ToByteArray((object)packet));

        start_time = 0;
        end_time = 0;
    }

    public void onOtherPlayerStartTrigger(GameObject obj)
    {

    }

    public void onOtherPlayerEndTrigger(GameObject obj)
    {
    }
}

