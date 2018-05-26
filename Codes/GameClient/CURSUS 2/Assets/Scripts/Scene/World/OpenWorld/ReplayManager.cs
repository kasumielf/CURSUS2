using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Replay
{
    public List<byte> data;
    public double date;
}

public class ReplayManager : MonoBehaviour {

    public OpenWorld world;
    public ReplayPlayerPrefab replayPlayer;

    private int current_replay_index;
    private List<Replay> replay_data;
    private bool replaying_now;

    private void Awake()
    {
        current_replay_index = -1;
        replaying_now = false;
        replay_data = new List<Replay>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartReplay()
    {
        replaying_now = true;
        replayPlayer.gameObject.SetActive(true);
        StartCoroutine(Replay());
    }

    public void StopReplay()
    {
        replaying_now = false;
        replayPlayer.gameObject.SetActive(false);
        StopCoroutine(Replay());
    }

    public void SetReplay(int index)
    {
        current_replay_index = index;
    }

    IEnumerator Replay()
    {
        Replay replay = replay_data[current_replay_index];

        int i = 0;
        int len = replay.data.Count;
        bool during = true;

        replayPlayer.SetReady(replay_data[current_replay_index].date, world.pathes.nonplayer_front_path);

        while (during)
        {
            if (i >= len - 1)
            {
                StopReplay();
            }
            else
            {
                byte speed = replay.data[i++];
                replayPlayer.SetSpeed(speed);
            }

            yield return new WaitForSeconds(5.0f);
        }
    }

    public void Put(int index, byte[] data, double date)
    {
        Replay replay = new Replay();
        replay.data = new List<byte>();

        int len = data.Length;

        for (int i = 0; i < len; ++i)
        {
            replay.data.Add(data[i]);
        }

        replay.date = date;
            
        replay_data.Add(replay);
    }
}
