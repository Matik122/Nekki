using System;
using Core;
using Game;
using Pool;
using SO;
using Support;
using UniRx;
using UnityEngine;

namespace Roots
{
    public class GameRoot : DisposableBehaviour<GameRoot.Model>
    {
        public class Model
        {
            public readonly GameConfig GameConfig;
            public readonly IGamePool GamePool;
            public readonly Action OnGameAction;

            public Model(GameConfig gameConfig, Action onGameAction, IGamePool gamePool)
            {
                OnGameAction = onGameAction;
                GameConfig = gameConfig;
                GamePool = gamePool;
            }
        }

        
        [SerializeField] private Mage _mainMage;
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _poolContainer;
        
        protected override void OnInit()
        {
            base.OnInit();
            
            InitMageComponents();

            new EnemyPooler(ActiveModel.GamePool, ActiveModel.GameConfig, _mainMage, _poolContainer)
                .Init().
                AddTo(Disposables);
        }

        private void InitMageComponents()
        {
            _mainMage
                .Init(new UnitBase.BaseModel(ActiveModel.GameConfig.MageConfig.Health, 
                    ActiveModel.GameConfig.MageConfig.Damage,
                    ActiveModel.GameConfig.MageConfig.Defence,
                    ActiveModel.GameConfig.MageConfig.Speed)).
                AddTo(Disposables);

            new CameraFollow(_camera, _mainMage.transform, 
                    ActiveModel.GameConfig.Camera.CameraOffset, 
                    ActiveModel.GameConfig.Camera.SmoothSpeed)
                .Init()
                .AddTo(Disposables);
        }
    }
}