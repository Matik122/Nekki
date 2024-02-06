using System;
using Core;
using Support;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Roots
{
    public class LobbyRoot : DisposableBehaviour<LobbyRoot.Model>
    {
        public class Model
        {
            public readonly Action OnGameAction;

            public Model(Action onGameAction)
            {
                OnGameAction = onGameAction;
            }
        }

        [SerializeField] private Button _startGameButton;
        

        protected override void OnInit()
        {
            base.OnInit();

            _startGameButton
                .OnClickAsObservable()
                .SafeSubscribe(_ => ActiveModel.OnGameAction?.Invoke())
                .AddTo(Disposables);
        }
    }
}