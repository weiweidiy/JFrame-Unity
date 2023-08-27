
using Adic;
using Adic.Container;
using Cysharp.Threading.Tasks;
using JFrame.Game.Models;
using Stateless;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace JFrame.Game.View
{
    public class SceneSM
    {
        public event Action<SceneBaseState, SceneBaseState> onSceneTransition;
        public event Action<SceneBaseState> onSceneEnter;
        public event Action<SceneBaseState> onSceneExit;

        enum Trigger
        {
            StartGame,
            StartMenu
        }

        StateMachine<SceneBaseState, Trigger> machine;
        StateMachine<SceneBaseState, Trigger>.TriggerWithParameters<bool> startMenuTrigger;
        StateMachine<SceneBaseState, Trigger>.TriggerWithParameters<PlayerAccount> startGameTrigger;

        [Inject]
        IInjectionContainer container;

        [Inject]
        public void Init()
        {
            Debug.Assert(container != null, "container is null");
            container.Bind<InitState>().ToSingleton();
            container.Bind<MainSceneState>().ToSingleton();
            container.Bind<BattleSceneState>().ToSingleton();
        }

        /// <summary>
        /// 初始化状态机
        /// </summary>
        /// <param name="owner"></param>
        public void Initialize(SceneController owner)
        {
            var initState = container.Resolve<InitState>();
            var menuState = container.Resolve<MainSceneState>();
            var gameState = container.Resolve<BattleSceneState>();
            menuState.Owner = owner;
            gameState.Owner = owner;

            //配置游戏状态机
            machine = new StateMachine<SceneBaseState, Trigger>(initState/*() => _state, s => _state = s*/);
            startMenuTrigger = machine.SetTriggerParameters<bool>(Trigger.StartMenu);
            startGameTrigger = machine.SetTriggerParameters<PlayerAccount>(Trigger.StartGame);

            machine.Configure(initState)
                .Permit(Trigger.StartMenu, menuState)
                .Permit(Trigger.StartGame, gameState);

            machine.Configure(menuState)
                .OnEntryFromAsync(startMenuTrigger, async (isRestart) => { await OnEnterMain(menuState, isRestart); })
                .OnExitAsync( async () => { await OnExitMain(menuState); })
                .Permit(Trigger.StartGame, gameState);

            machine.Configure(gameState)
                .OnEntryFromAsync(startGameTrigger, async (playerAccount) => { await OnEnterBattle(gameState, playerAccount); })
                .Permit(Trigger.StartMenu, menuState);

            machine.OnTransitioned(OnTransition);
        }



        /// <summary>
        /// 切换到游戏状态
        /// </summary>
        public void SwitchToBattle(PlayerAccount account)
        {
            machine.FireAsync(startGameTrigger, account);
        }

        /// <summary>
        /// 切换到菜单状态
        /// </summary>
        /// <param name="isRestart"></param>
        public void SwitchToMain(bool isRestart)
        {
            machine.FireAsync(startMenuTrigger, isRestart);
        }


        /// <summary>
        /// 状态切换时响应方法
        /// </summary>
        /// <param name="obj"></param>
        private void OnTransition(StateMachine<SceneBaseState, Trigger>.Transition obj)
        {
            Debug.Log("OnTransition " + obj.Source + " / " + obj.Destination);
            onSceneTransition?.Invoke(obj.Source, obj.Destination);
        }

        /// <summary>
        /// 进入游戏状态了
        /// </summary>
        /// <param name="state"></param>
        /// <param name="playerAccount"></param>
        private async UniTask OnEnterBattle(BattleSceneState state, PlayerAccount playerAccount)
        {
            await state.OnEnter(playerAccount);
            onSceneEnter?.Invoke(state);
        }

        /// <summary>
        /// 进入菜单状态了
        /// </summary>
        /// <param name="state"></param>
        /// <param name="isRestart"></param>
        private async UniTask OnEnterMain(MainSceneState state, bool isRestart)
        {
            await state.OnEnter(isRestart);
            onSceneEnter?.Invoke(state); 
        }

        /// <summary>
        /// 推出菜单场景状态机了
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private async UniTask OnExitMain(MainSceneState state)
        {
            await state.OnExit();
            onSceneExit?.Invoke(state);
        }

    }
}

