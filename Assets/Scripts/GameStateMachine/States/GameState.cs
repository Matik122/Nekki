using System;
using Pool;
using Roots;
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
        private readonly CompositeDisposable _rootDisposable = new();

        private const string StateSceneName = "Game";
        
        public GameState(GameMachine gameMachine, GameConfig gameConfig, IGamePool gamePool)
        {
            _gameMachine = gameMachine;
            _gameConfig = gameConfig;
            _gamePool = gamePool;
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
                .Init(new GameRoot.Model(_gameConfig, OnExit, _gamePool))
                .AddTo(subscriptions);

            return subscriptions;
        }

        private void OnExit()
        {
            _gameMachine.ChangeState<LobbyState>();
        }

        protected override void Deinit()
        {
            _rootDisposable.Clear();
        }
    }
}