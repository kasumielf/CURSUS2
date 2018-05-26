using Scripts.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankResultPage : MonoBehaviour
{
    private string[] names;
    private string[] times;
    public TextMesh[] Rankernames;
    public TextMesh[] RankerRecords;

    public TextMesh MyBestRecord;
    public TextMesh MyCurrentRecord;

    // Use this for initialization
    void Start()
    {
        names = new string[5];
        times = new string[5];
    }

    // Update is called once per frame
    void Update()
    {
        // PacketProcess에서 처리하는 부분이라 MessageQueue로 해야되는건데 귀찮아서 이렇게 해둔거니 둘것.
        for(int i=0;i<5;++i)
        {
            Rankernames[i].text = names[i];
            RankerRecords[i].text = times[i];
        }
    }

    public void SetEmptyRank(int index)
    {
        names[index] = "";
        times[index] = "";
    }

    public void SetRankedPlayerRecord(int index, string name, double time)
    {
        names[index] = name;
        times[index] = Utility.GetTimeFormat(time);
    }

    public void SetMyBestRecord(double t)
    {
        MyBestRecord.text = Utility.GetTimeFormat(t);
    }

    public void SetMyCurrentRecord(double t)
    {
        MyCurrentRecord.text = Utility.GetTimeFormat(t);
    }

}
