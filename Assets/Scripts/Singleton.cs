using Unity.VisualScripting;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;
    private static readonly object _lock = new object();

    protected virtual bool IsPersistent => false;
    
    public static T Instance
    {
        get
        {
            lock(_lock) //同時アクセスを防ぐ
            {
                if (_instance == null)
                {
                    _instance = (T)FindObjectOfType(typeof(T));
                    
                    if (_instance == null)
                    {
                        GameObject obj = new GameObject(typeof(T).Name);
                        _instance = obj.AddComponent<T>();
                    }
                }
                return _instance;
            }
        }
    }

    protected virtual void Awake()
    {
        RemoveDuplicates();
    }

    public void OnApplicationQuit()
    {
        _instance = null;
    }

    void RemoveDuplicates()
    {
        if (_instance == null)
        {
            _instance = this as T;
            if (IsPersistent)
                DontDestroyOnLoad(gameObject);
        }
        else if(_instance != this)
        {
            Destroy(gameObject);
        }
    }
}
