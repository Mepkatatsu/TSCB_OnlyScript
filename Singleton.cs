using System;
using UnityEngine;

namespace SingletonPattern
{
    public class Singleton<T> : MonoBehaviour where T: Component
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance != null) 
                    return _instance;
                
                _instance = FindObjectOfType<T>();

                if (_instance != null)
                    return _instance;
                
                return null;
            }
        }

        public virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}