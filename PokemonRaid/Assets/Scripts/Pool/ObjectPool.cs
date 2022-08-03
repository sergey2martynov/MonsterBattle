using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Pool
{
    public class ObjectPool<TObj>
        where TObj : MonoBehaviour, IObjectToPool
    {
        private readonly Queue<TObj> _objects;
        private readonly TObj _prefab;

        public ObjectPool(int capacity, TObj prefab)
        {
            _prefab = prefab;
            _objects = new Queue<TObj>(capacity);
            
            for (var i = 0; i < capacity; i++)
            {
                var obj = Object.Instantiate(prefab, Vector3.zero, quaternion.identity);
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