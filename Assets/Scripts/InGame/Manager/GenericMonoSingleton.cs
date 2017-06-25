using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 어느 클래스를 받아서 그 클래스를 싱글톤으로 전환하는
// 제네릭 싱글톤이다. 단 너무 남발하지 않도록 주의.
public class GenericMonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance = null;

    public static T Instance
    {
        get
        {
            if (null == instance)
            {
                instance = GameObject.FindObjectOfType<T>();

                if (null == instance)
                {
                    var go = new GameObject(typeof(T).ToString() + "Singleton");
                    instance = go.AddComponent<T>();
                    DontDestroyOnLoad(go);
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (null != instance && this != instance)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            instance = this as T;
            DoAwake();
        }
    }

    protected virtual void DoAwake()
    {
        // nothing
    }

    public void Myname()
    {
        Debug.Log("GenericMonoSingleton");
    }
}

