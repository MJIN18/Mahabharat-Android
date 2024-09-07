using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T instance;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this as T;
        }
        else
        {
            if (instance != this)
                Debug.Log("me");
            Destroy(gameObject);
        }
    }




    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<T>();

                if(instance == null)
                {
                    GameObject gameObject = new GameObject("SingletonController");
                    instance = gameObject.AddComponent<T>();
                }
            }

            return instance;
        }
    }
}
