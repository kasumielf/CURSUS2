using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class Singleton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}*/


/// 싱글톤이 아닌 생성자를 막지는 못함
/// 다음처럼 사용 -> `T myT = new T();
/// 이것을 막으려면, 싱글톤 클래스에'protected PlayerManager () {}'를 추가
/// Coroutines이 필요하기 때문에 MonoBehaviour로 제작
/// 
/// 씬이 변환 되도 파괴되지 않는 싱글톤을 만들고 싶다면 상속받은 클래스의 awake 함수에 아래와 같이 선언
/// DontDestroyOnLoad(this.gameObject);
/// 


///플레이어 싱글톤.
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    //private static PlayerManager _instance = null;
    public static T _instance = null;
    private static object _syncobj = new object();
    private static bool appIsClosing = false;

    public static T Instance
    {
        get
        {
            if (appIsClosing)
                return null;

            lock (_syncobj)
            {
                if (_instance == null)
                {
                    T[] objs = FindObjectsOfType<T>();

                    if (objs.Length > 0)
                        _instance = objs[0];

                    if (objs.Length > 1)
                        Debug.LogError("There is more than one " + typeof(T).Name + " in the scene.");

                    if (_instance == null)
                    {
                        string goName = typeof(T).ToString();
                        GameObject go = GameObject.Find(goName);
                        if (go == null)
                            go = new GameObject(goName);
                        _instance = go.AddComponent<T>();
                    }
                }
                return _instance;
            }
        }
    }
    /// Unity는 종료되면 무작위 순서로 객체를 파괴
    /// 원칙적으로 싱글톤은 응용 프로그램이 종료 될 때만 파괴
    /// 어떤 스크립트가 인스턴스가 파괴 된 후에 인스턴스를 호출하면,
    /// 응용 프로그램 재생을 중지 한 후에도 편집기 장면에 머무를 buggy ghost object를 만듬
    /// 그래서, 이것은 우리가 buggy ghost object를 생성하지 않는다는 것을 확신하도록 만듬
    protected virtual void OnApplicationQuit()
    {
        // release reference on exit
        appIsClosing = true;
    }
}