using System;
using System.Collections.Generic;
using Enemy;
using Factories;
using StaticData;
using UnityEngine;
using UpdateHandlerFolder;

namespace Pool
{
    public class EnemyPool : MonoBehaviour
    {
        [SerializeField] private EnemyStats _stats;
        [SerializeField] private List<BaseEnemyView> _enemyViews;
        [SerializeField] private UpdateHandler _updateHandler;
        [SerializeField] private Transform _camera;

        private EnemyFactory _factory;

        private void Awake()
        {
            _factory = new EnemyFactory(_updateHandler, _camera);

            foreach (var enemyView in _enemyViews)
            {
                _factory.CreateInstance(enemyView, Vector3.zero, _stats, null, 1, out var view);
            }
        }
    }
}