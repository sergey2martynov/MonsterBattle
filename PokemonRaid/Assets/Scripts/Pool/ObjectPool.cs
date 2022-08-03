using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Pool
{
    public class ObjectPool<TObj>
        where TObj : MonoBehaviour, IObjectToPool
    {
        private readonly Queue<TObj> _objects;

        public ObjectPool(int capacity, TObj prefab, Transform parent)
        {
            _objects = new Queue<TObj>(capacity);
            
            for (var i = 0; i < capacity; i++)
            {
                var obj = Object.Instantiate(prefab, parent, false);
                obj.SetObjectActive(false);
                _objects.Enqueue(obj);
            }
        }

        public TObj TryPoolObject()
        {
            if (_objects.Count == 0)
            {
                throw new IndexOutOfRangeException("Object Pool is empty. Increase pool capacity");
            }

            var obj = _objects.Dequeue();
            obj.SetObjectActive(true);
            return obj;
        }

        public void ReturnToPool(TObj obj)
        {
            obj.SetObjectActive(false);
            _objects.Enqueue(obj);
        }
    }
}