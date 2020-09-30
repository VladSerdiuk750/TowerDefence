using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T>: MonoBehaviour where T: Singleton<T> // Класс для подгрузки данных
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
            }
            else if (instance != FindObjectOfType<T>()) // если ситуация что Manager не должен создаваться
            {
                Destroy(FindObjectOfType<T>()); // то его нужно удалить
            }

            DontDestroyOnLoad(FindObjectOfType<T>()); // при загрузке Manager всегда должен оставатся на сцене

            return instance;
        }
    }
    protected virtual void Awake()
    {
        if (instance != null)
        {
            Debug.LogErrorFormat("[Singleton] Trying to instantiate a second instance of singleton class {0}", GetType().Name);
        }
        else
        {
            instance = (T) this;
        }
    }

    public static bool IsInitialized
    {
        get
        {
            return instance != null;
        }
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
