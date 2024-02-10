using System;
using System.Collections.Generic;
using Core;
using Pool;
using SO;
using Support;
using UniRx;
using UnityEngine;

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
            return Observable.Interval(TimeSpan.FromSeconds(3));
        }

        private IDisposable ActivateEnemyPeriodically()
        {
            return TimerObservable().Subscribe(_ =>
            {
                int activeCount = 0;
        
                for (int i = 0; i < _enemyObjects.Count; i++)
                {
                    if (_enemyObjects[i].activeSelf)
                    {
                        activeCount++;
                    }
                    else
                    {
                        _enemyObjects[i].SetActive(true);
                        return;
                    }
                }

                if (activeCount == _enemyObjects.Count) 
                {
                    _enemyObjects[_currentIndex].SetActive(false);

                    _currentIndex++;
                    if (_currentIndex >= _enemyObjects.Count)
                    {
                        var spawnedObject = _gamePool.Spawn(_gameConfig.Enemies[0], _poolContainer);
                        _enemyObjects.Add(spawnedObject.gameObject);
                        if (_enemyObjects.Count > 1)
                        {
                            _enemyObjects[0].SetActive(true);
                        }
                        _currentIndex = 0;
                    }
                    else
                    {
                        _enemyObjects[_currentIndex].SetActive(true);
                    }
                }
            });
        }
    }
}