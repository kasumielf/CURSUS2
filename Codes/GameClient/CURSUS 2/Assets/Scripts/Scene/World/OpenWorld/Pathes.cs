using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathes : MonoBehaviour {
    public Vector3[] player_front_path;
    public Vector3[] player_back_path;
    public Vector3[] nonplayer_front_path;
    public Vector3[] nonplayer_back_path;

    // Use this for initialization
    void Start () {
        /*
        player_front_path = LoadPath("PF_Sector ", 1, 5).ToArray();
        player_back_path = LoadPath("PB_Sector ", 5, 1).ToArray();
        nonplayer_front_path = LoadPath("NF_Sector ", 1, 5).ToArray();
        nonplayer_back_path = LoadPath("NB_Sector ", 5, 1).ToArray();*/

        player_front_path = iTweenPath.GetPath("PlayerFrontPath");
        nonplayer_front_path = iTweenPath.GetPath("NonPlayerFrontPath");
    }

    private List<Vector3> LoadPath(string path_name, int from, int to)
    {
        List<Vector3> pathes = new List<Vector3>();

        int c = from;

        while (true)
        {
            Vector3[] vec = iTweenPath.GetPath(path_name + c);

            int _c = 0;
            int len = vec.Length;
             foreach (Vector3 v in vec)
             {
                if(_c < len-1)
                    pathes.Add(v);
                _c++;
             }

            if (from < to)
            {
                if (++c > to)
                   break;
            }
            else
            {
                if (--c < to)
                    break;
            }

        }

        return pathes;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

