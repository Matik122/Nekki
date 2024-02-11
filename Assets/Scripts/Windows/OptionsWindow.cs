using System;
using Services.WindowService;
using Support;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Windows
{
    public class OptionsWindow : WindowBase<OptionsWindow.Model>
    {
        public class Model
        {
            public Action OnClick;
            public readonly WindowsService WindowsService;

            public Model(Action onClick, WindowsService windowsService)
            {
                OnClick = onClick;
                WindowsService = windowsService;
            }
        }
        
        [SerializeField] private Button _quitButton;
        [SerializeField] private Button _toMainMenuButton;

        protected override void OnOpen()
        {
            _toMainMenuButton
                .OnClickAsObservable()
                .SafeSubscribe(_ =>
                {
                    ActiveModel.OnClick?.Invoke();
                    ActiveModel.WindowsService.Close();
                })
                .AddTo(Disposables);

            _quitButton
                .OnClickAsObservable()
                .SafeSubscribe(_ => ActiveModel.WindowsService.Close())
                .AddTo(Disposables);
        }
    }
}