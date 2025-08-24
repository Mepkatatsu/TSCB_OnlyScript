using System.Collections.Generic;
using UnityEngine;

namespace CoreLib
{
    public static class SimpleGameObjectPool
    {
        private static readonly Dictionary<string, Stack<GameObject>> SimpleObjectPool = new();

        public static GameObject AllocObject(GameObject gameObject, Transform parent)
        {
            var name = gameObject.name;
        
            if (!SimpleObjectPool.TryGetValue(name, out var objects))
            {
                objects = new Stack<GameObject>();
                SimpleObjectPool.Add(name, objects);
            }

            if (objects.TryPop(out var obj))
                return obj;
        
            obj = Object.Instantiate(gameObject, Vector3.zero, Quaternion.identity, parent);
            obj.name = name;

            return obj;
        }

        public static void ReleaseObject(GameObject gameObject)
        {
            if (!SimpleObjectPool.TryGetValue(gameObject.name, out var objects))
            {
                objects = new Stack<GameObject>();
                SimpleObjectPool.Add(gameObject.name, objects);
            }
        
            gameObject.SetActive(false);
        
            objects.Push(gameObject);
        }
    }
}
