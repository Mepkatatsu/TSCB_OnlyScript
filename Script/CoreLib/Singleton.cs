using System;
using UnityEngine;

namespace CoreLib
{
    public class Singleton<T> : MonoBehaviour where T: Component
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance) 
                    return _instance;
                
                _instance = FindObjectOfType<T>();

                if (_instance)
                    return _instance;
                
                return null;
            }
        }

        public virtual void Awake()
        {
            if (!_instance)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}