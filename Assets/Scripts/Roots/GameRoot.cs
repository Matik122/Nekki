using System;
using Windows;
using Core;
using Game;
using Pool;
using Services.WindowService;
using SO;
using Support;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Roots
{
    public class GameRoot : DisposableBehaviour<GameRoot.Model>
    {
        public class Model
        {
            public readonly GameConfig GameConfig;
            public readonly IGamePool GamePool;
            public readonly Action OnGameAction;
            public readonly Action Restart;
            public readonly WindowsService WindowsService;
            public readonly WindowResolver WindowResolver;

            public Model(GameConfig gameConfig, 
                         Action onGameAction,
                         Action restart,
                         IGamePool gamePool,
                         WindowsService windowsService,
                         WindowResolver windowResolver)
            {
                OnGameAction = onGameAction;
                Restart = restart;
                GameConfig = gameConfig;
                GamePool = gamePool;
                WindowsService = windowsService;
                WindowResolver = windowResolver;
            }
        }

        
        [SerializeField] private Mage _mainMage;
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _poolContainer;
        [SerializeField] private Button _closeButton;
        [SerializeField] private HealthView _healthView;
        
        private readonly ReactiveProperty<float> _currentHealth = new ();
        
        protected override void OnInit()
        {
            base.OnInit();
            
            var failWindow =
                ActiveModel.WindowResolver.GetFailWindow(ActiveModel.OnGameAction, ActiveModel.Restart);
            
            InitMageComponents(failWindow);

            new EnemyPooler(ActiveModel.GamePool, ActiveModel.GameConfig, _mainMage, _poolContainer)
                .Init().
                AddTo(Disposables);
            
            var optionsWindow =
                ActiveModel.WindowResolver.GetOptionsWindowModel(ActiveModel.OnGameAction);

            _closeButton
                .OnClickAsObservable()
                .SafeSubscribe(_ => ActiveModel.WindowsService.Open(optionsWindow, false))
                .AddTo(Disposables);

            _healthView
                .Init(new HealthView.Model(ActiveModel.GameConfig.MainPlayer.Mage.Health, _currentHealth))
                .AddTo(Disposables);
        }

        private void InitMageComponents(FailWindow.Model model)
        {
            _mainMage
                .Init(new Mage.MageModel(ActiveModel.GameConfig.MainPlayer.Mage.Health, 
                                         ActiveModel.GameConfig.MainPlayer.Mage.Damage,
                                         ActiveModel.GameConfig.MainPlayer.Mage.Defence,
                                         ActiveModel.GameConfig.MainPlayer.Mage.Speed,
                                         ActiveModel.GameConfig.MainPlayer.RotationSpeed,
                                         ActiveModel.GamePool,
                                         ActiveModel.GameConfig.Spells,
                                         _currentHealth,
                                         ActiveModel.WindowsService,
                                         model))
                .AddTo(Disposables);

            new CameraFollow(_camera, _mainMage.transform, 
                    ActiveModel.GameConfig.Camera.CameraOffset, 
                    ActiveModel.GameConfig.Camera.SmoothSpeed)
                .Init()
                .AddTo(Disposables);
        }
    }
}