using System;
using Core;
using Game;
using SO;
using UniRx;
using UnityEngine;

namespace Roots
{
    public class GameRoot : DisposableBehaviour<GameRoot.Model>
    {
        public class Model
        {
            public readonly GameConfig GameConfig;
            public readonly Action OnGameAction;

            public Model(GameConfig gameConfig, Action onGameAction)
            {
                OnGameAction = onGameAction;
                GameConfig = gameConfig;
            }
        }

        
        [SerializeField] private Mage _mainMage;
        [SerializeField] private Camera _camera;
        
        protected override void OnInit()
        {
            base.OnInit();
            
            _mainMage
                .Init(new UnitBase.BaseModel(0,0,0,5))
                .AddTo(Disposables);

            new CameraFollow(_camera, _mainMage.transform, ActiveModel.GameConfig.Camera.CameraOffset, ActiveModel.GameConfig.Camera.SmoothSpeed)
                .Init()
                .AddTo(Disposables);
        }
    }
}