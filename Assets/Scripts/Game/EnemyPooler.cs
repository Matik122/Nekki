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
        private readonly List<(Enemy enemy, GameConfig.UnitBaseConfig config)> _enemyObjects = new ();
        
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
               var spawnedObject = _gamePool.Spawn(arg.EnemyPrefab as Enemy, _poolContainer);
                spawnedObject.gameObject.SetActive(false);
                _enemyObjects.Add((spawnedObject, arg.Enemy));
            });
            
            if (_enemyObjects.Count > 0)
            {
                _enemyObjects[_currentIndex].enemy.gameObject.SetActive(true);
                InitEnemy(_enemyObjects[_currentIndex]);
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
                var inactiveObj = _enemyObjects.FirstOrDefault(obj => !obj.enemy.gameObject.activeSelf);
                
                if (inactiveObj.enemy != null)
                {
                    inactiveObj.enemy.gameObject.SetActive(true);
                    InitEnemy(inactiveObj);
                }
                else
                {
                    if (_enemyObjects.Count < _gameConfig.EnemyPool.LimitPoolCount)
                    {
                        var enemyConfig = _gameConfig.Enemies.ElementAt(Random.Range(
                            _gameConfig.EnemyPool.StartRandomIndex,
                            _gameConfig.Enemies.Count));
                        
                        var spawnedObject = _gamePool.Spawn(enemyConfig.EnemyPrefab as Enemy, _poolContainer);

                        if (spawnedObject != null)
                        {
                            _enemyObjects.Add((spawnedObject,enemyConfig.Enemy));
                            spawnedObject.gameObject.SetActive(true);
                            InitEnemy(_enemyObjects.Last());
                        }
                    }
                }

            }).AddTo(Disposables);
        }

        private void InitEnemy((Enemy enemy,GameConfig.UnitBaseConfig config) enemyConfig)
        {
            enemyConfig.enemy
                .Init(new UnitBase.BaseModel(enemyConfig.config.Health, 
                                             enemyConfig.config.Damage, 
                                             enemyConfig.config.Defence, 
                                             enemyConfig.config.Speed))
                .AddAction(() => enemyConfig.enemy.InjectMage(_mage))
                .AddTo(Disposables);
        }
    }
}