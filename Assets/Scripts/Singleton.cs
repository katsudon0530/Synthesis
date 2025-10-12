using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

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

    public static async Task<T> InstanceAsync()
    {
        if (_instance != null)
            return _instance;

        _instance = FindObjectOfType<T>();
        if (_instance == null)
        {
            // Addressables で読み込み
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(typeof(T).Name);
            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject obj = Object.Instantiate(handle.Result);
                _instance = obj.GetComponent<T>();
                Debug.Log(_instance);
                return _instance;
            }

            // Prefab が見つからなかった場合は空のオブジェクトで作る
            Debug.LogWarning($"[Singleton] Prefab for {typeof(T).Name} not found, creating new object.");
            GameObject newObj = new GameObject(typeof(T).Name);
            _instance = newObj.AddComponent<T>();
        }

        return _instance;
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
