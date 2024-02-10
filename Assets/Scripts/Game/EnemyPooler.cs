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
        private readonly Mage _mage;
        private readonly Transform _poolContainer;
        private readonly List<Enemy> _enemyObjects = new ();
        
        private int _currentIndex;

        
        public EnemyPooler(IGamePool gamePool, GameConfig gameConfig, Mage mage, Transform poolContainer)
        {
            _gamePool = gamePool;
            _gameConfig = gameConfig;
            _mage = mage;
            _poolContainer = poolContainer;
        }
        
        protected override void OnInit()
        {
            base.OnInit();
            
            PoolAllEnemyTypes();
            
            ActivateEnemyPeriodically()
                .AddTo(Disposables);
        }

        private void PoolAllEnemyTypes()
        {
            _gameConfig.Enemies.ForEach(arg =>
            {
               var spawnedObject = _gamePool.Spawn(arg as Enemy, _poolContainer);
                spawnedObject.gameObject.SetActive(false);
                _enemyObjects.Add(spawnedObject);
            });
            
            if (_enemyObjects.Count > 0)
            {
                _enemyObjects[_currentIndex].gameObject.SetActive(true);

                _enemyObjects[_currentIndex]
                    .Init(new UnitBase.BaseModel(0, 0, 0, 5))
                    .AddAction(() => _enemyObjects[_currentIndex].InjectMage(_mage))
                    .AddTo(Disposables);
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
                var inactiveObj = _enemyObjects.FirstOrDefault(obj => !obj.gameObject.activeSelf);
                if (inactiveObj != null)
                {
                    inactiveObj.gameObject.SetActive(true);
                    
                    inactiveObj
                        .Init(new UnitBase.BaseModel(0,0,0,5))
                        .AddAction(() => inactiveObj.InjectMage(_mage))
                        .AddTo(Disposables);
                }
                else
                {
                    var spawnedObject = _gamePool.Spawn(
                        _gameConfig.Enemies.ElementAtOrDefault(Random.Range(_gameConfig.EnemyPool.StartRandomIndex,
                            _gameConfig.Enemies.Count)) as Enemy,
                        _poolContainer);

                    if (spawnedObject != null)
                    {
                        _enemyObjects.Add(spawnedObject);
                        spawnedObject.gameObject.SetActive(true);
                        
                        spawnedObject
                            .Init(new UnitBase.BaseModel(0,0,0,5))
                            .AddAction(() => spawnedObject.InjectMage(_mage))
                            .AddTo(Disposables);
                    }
                }

            }).AddTo(Disposables);
        }
    }
}