using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackGameResultUI : MonoBehaviour
{
    public Text[] Names;
    public Text[] Records;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void UpdateResult(int rank, string name, double record)
    {
        if (rank >= 0 && rank <= 2)
        {
            System.TimeSpan span = System.TimeSpan.FromMilliseconds(record);
            Names[rank].text = name;
            Records[rank].text = string.Format("{0:00}:{1:00}:{2:00}.{3:000}", span.Hours, span.Minutes, span.Seconds, span.Milliseconds);
        }

        if (name.Equals("") && record < 0)
        {
            Names[rank].text = "";
            Records[rank].text = "";
        }
    }
}
