using System;
using Services.WindowService;
using Support;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Windows
{
    public class FailWindow : WindowBase<FailWindow.Model>
    {
        public class Model
        {
            public Action ToLobby;
            public Action Restart;
            public readonly WindowsService WindowsService;

            public Model(Action toLobby, Action restart, WindowsService windowsService)
            {
                ToLobby = toLobby;
                Restart = restart;
                WindowsService = windowsService;
            }
        }
        
        [SerializeField] private Button _toLobbyButton;
        [SerializeField] private Button _restartButton;

        protected override void OnOpen()
        {
            _toLobbyButton
                .OnClickAsObservable()
                .SafeSubscribe(_ =>
                {
                    ActiveModel.ToLobby?.Invoke();
                    ActiveModel.WindowsService.Close();
                })
                .AddTo(Disposables);

            _restartButton
                .OnClickAsObservable()
                .SafeSubscribe(_ =>
                {
                    ActiveModel.Restart?.Invoke();
                    ActiveModel.WindowsService.Close();
                })
                .AddTo(Disposables);
        }
    }
}