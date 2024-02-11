using System;
using GameStateMachine;
using GameStateMachine.States;
using Pool;
using Services.WindowService;
using SO;
using Support;
using UniRx;
using UnityEngine;

namespace Roots
{
    public class MainRoot : MonoBehaviour
    {
        [SerializeField] private GameConfig _gameConfig;
        [SerializeField] private WindowsService _windowsService;
        
        private GameMachine _gameMachine;
        private IGamePool _gamePool;

        private readonly CompositeDisposable _rootDisposable = new();

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            StartRoot();
        }

        private void StartRoot()
        {
            Observable.ReturnUnit()
                .ContinueWith(_ => InitServices())
                .ContinueWith(_ => InitStates())
                .SafeSubscribe(_ => LoadStage())
                .AddTo(_rootDisposable);
        }

        private IObservable<Unit> InitStates()
        {
            _gamePool = new LeanPoolWrapper();
            _gameMachine = new GameMachine();

            _gameMachine
                .Init()
                .AddTo(_rootDisposable);
            
            var windowResolver = new WindowResolver(_windowsService);

            _gameMachine.AddState(new LobbyState(_gameMachine));
            _gameMachine.AddState(new GameState(_gameMachine, _gameConfig, _gamePool, _windowsService, windowResolver));

            return Observable.ReturnUnit();
        }

        private IObservable<Unit> InitServices()
        {
            _windowsService
                .Init(new WindowsService.Model())
                .AddTo(_rootDisposable);
            
            return Observable.ReturnUnit();
        }

        private void LoadStage()
        {
            _gameMachine.ChangeState<LobbyState>();
        }

        private void OnDestroy()
        {
            _rootDisposable.Clear();
        }
    }
}