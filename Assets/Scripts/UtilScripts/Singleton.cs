using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<Obj> : MonoBehaviour where Obj : MonoBehaviour
{
    public static Obj _instance;
    public static Obj Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Obj>();

                if (_instance == null)
                {
                    Debug.LogError($"No instance of {typeof(Obj)} found in the scene.");
                }
            }

            return _instance;
        }
    }
    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as Obj;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
}
