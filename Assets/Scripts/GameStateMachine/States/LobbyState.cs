using System;
using Roots;
using Support;
using UniRx;

namespace GameStateMachine.States
{
    public class LobbyState : GameStateBase<Unit>
    {
        private readonly GameMachine _gameMachine;
        private readonly CompositeDisposable _rootDisposable = new();

        private const string StateSceneName = "Lobby";

        public LobbyState(GameMachine gameMachine)
        {
            _gameMachine = gameMachine;
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

            var lobbyRoot = SceneExtensions.LoadSceneRoot<LobbyRoot>();

            lobbyRoot
                .Init(new LobbyRoot.Model(OnGameStartRequested))
                .AddTo(subscriptions);

            return subscriptions;
        }

        private void OnGameStartRequested()
        {
            _gameMachine.ChangeState<GameState>();
        }

        protected override void Deinit()
        {
            _rootDisposable.Clear();
        }
    }
}