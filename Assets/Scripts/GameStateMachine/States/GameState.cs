using System;
using Pool;
using Roots;
using Services.WindowService;
using SO;
using Support;
using UniRx;

namespace GameStateMachine.States
{
    public class GameState : GameStateBase<Unit>
    {
        private readonly GameMachine _gameMachine;
        private readonly GameConfig _gameConfig;
        private readonly IGamePool _gamePool;
        private readonly WindowsService _windowsService;
        private readonly WindowResolver _windowResolver;
        private readonly CompositeDisposable _rootDisposable = new();

        private const string StateSceneName = "Game";
        
        public GameState(GameMachine gameMachine, 
                         GameConfig gameConfig, 
                         IGamePool gamePool, 
                         WindowsService windowsService, 
                         WindowResolver windowResolver)
        {
            _gameMachine = gameMachine;
            _gameConfig = gameConfig;
            _gamePool = gamePool;
            _windowsService = windowsService;
            _windowResolver = windowResolver;
        }

        protected override void Init()
        {
            SceneExtensions.LoadScene(StateSceneName)
                .SafeSubscribe(_ => OnSceneLoaded())
                .AddTo(_rootDisposable);
        }
        
        private void OnSceneLoaded()
        {
            InitControllers()
                .AddTo(_rootDisposable);
        }
        
        private IDisposable InitControllers()
        {
            var subscriptions = new CompositeDisposable();

            var gameRoot = SceneExtensions.LoadSceneRoot<GameRoot>();

            gameRoot
                .Init(new GameRoot.Model(_gameConfig, OnExit, Restart, _gamePool, _windowsService, _windowResolver))
                .AddTo(subscriptions);

            return subscriptions;
        }

        private void OnExit()
        {
            _gameMachine.ChangeState<LobbyState>();
        }
        
        private void Restart()
        {
            _gameMachine.ChangeState<GameState>();
        }

        protected override void Deinit()
        {
            _rootDisposable.Clear();
        }
    }
}