using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Pool;
using SO;
using Support;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public class EnemyPooler : DisposableClass
    {
        private readonly IGamePool _gamePool;
        private readonly GameConfig _gameConfig;
        private readonly Transform _poolContainer;
        
        private readonly List<GameObject> _enemyObjects = new ();
        private int _currentIndex;

        
        public EnemyPooler(IGamePool gamePool, GameConfig gameConfig, Transform poolContainer)
        {
            _gamePool = gamePool;
            _gameConfig = gameConfig;
            _poolContainer = poolContainer;
        }
        
        protected override void OnInit()
        {
            base.OnInit();
            
            PoolAllEnemyTypes();
            ActivateEnemyPeriodically().AddTo(Disposables);
        }

        private void PoolAllEnemyTypes()
        {
            _gameConfig.Enemies.ForEach(arg =>
            {
               var spawnedObject = _gamePool.Spawn(arg, _poolContainer);
                spawnedObject.gameObject.SetActive(false);
                _enemyObjects.Add(spawnedObject.gameObject);
            });
            
            if (_enemyObjects.Count > 0)
            {
                _enemyObjects[_currentIndex].SetActive(true);
            }
        }
        
        private IObservable<long> TimerObservable()
        {
            return Observable.Interval(TimeSpan.FromSeconds(_gameConfig.EnemyPool.IntervalForPool));
        }

        private IDisposable ActivateEnemyPeriodically()
        {
            return TimerObservable().SafeSubscribe(_ =>
            {
                _enemyObjects
                    .Where(obj => !obj.activeSelf)
                    .Take(1)
                    .ToList()
                    .ForEach(obj =>
                    {
                        obj.SetActive(true);
                    });

                if (!_enemyObjects.All(obj => obj.activeSelf)) 
                    return;
                
                var spawnedObject = _gamePool.Spawn(
                    _gameConfig.Enemies.ElementAtOrDefault(Random.Range(_gameConfig.EnemyPool.StartRandomIndex, _gameConfig.Enemies.Count)),
                    _poolContainer);

                if (spawnedObject == null) 
                    return;
                    
                _enemyObjects.Add(spawnedObject.gameObject);
                spawnedObject.gameObject.SetActive(true);
            }).AddTo(Disposables);
        }
    }
}