using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance = null;
    private static object _syncobj = new object();
    protected static bool appIsClosing = false;

    public static T Instance
    {
        get
        {
            if (Init())
                return _instance;
            else
                return null;
        }
    }

    protected static bool Init()
    {
        if (appIsClosing)
            return false;

        lock (_syncobj)
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();

                if (_instance == null)
                {
                    string componentName = typeof(T).ToString();
                    GameObject findObject = GameObject.Find(componentName);

                    if (findObject == null)
                    {
                        findObject = new GameObject(componentName);
                    }

                    _instance = findObject.AddComponent<T>();
                }
                DontDestroyOnLoad(_instance);
            }

            return true;
        }
    }

    public virtual void Set()
    {

    }

    protected virtual void OnApplicationQuit()
    {
        appIsClosing = true;
    }

    public virtual void OnDestroy()
    {
        appIsClosing = true;
    }
}
