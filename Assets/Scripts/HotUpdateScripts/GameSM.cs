
using Cysharp.Threading.Tasks;
using Stateless;
using System;
using UnityEngine;

namespace JFrame.Game.HotUpdate
{
    public class GameSM
    {
        enum Trigger
        {
            StartGame,
            StartMenu
        }

        StateMachine<State, Trigger> machine;
        StateMachine<State, Trigger>.TriggerWithParameters<bool> startMenuTrigger;
        StateMachine<State, Trigger>.TriggerWithParameters<PlayerAccount> startGameTrigger;

        /// <summary>
        /// 初始化状态机
        /// </summary>
        /// <param name="owner"></param>
        public void Initialize(GameManager owner)
        {
            var initState = new InitState();
            var menuState = new MenuState(owner);
            var gameState = new GameState(owner);

            //配置游戏状态机
            machine = new StateMachine<State, Trigger>(initState/*() => _state, s => _state = s*/);
            startMenuTrigger = machine.SetTriggerParameters<bool>(Trigger.StartMenu);
            startGameTrigger = machine.SetTriggerParameters<PlayerAccount>(Trigger.StartGame);

            machine.Configure(initState)
                .Permit(Trigger.StartMenu, menuState)
                .Permit(Trigger.StartGame, gameState);

            machine.Configure(menuState)
                .OnEntryFromAsync(startMenuTrigger, async (isRestart) => { await OnEnterMenu(menuState, isRestart); })
                .Permit(Trigger.StartGame, gameState);

            machine.Configure(gameState)
                .OnEntryFromAsync(startGameTrigger, async (playerAccount) => { await OnEnterGame(gameState, playerAccount); })
                .Permit(Trigger.StartMenu, menuState);

            machine.OnTransitioned(OnTransition);
        }

        /// <summary>
        /// 切换到游戏状态
        /// </summary>
        public void SwitchToGame(PlayerAccount account)
        {
            machine.Fire(startGameTrigger, account);
        }

        /// <summary>
        /// 切换到菜单状态
        /// </summary>
        /// <param name="isRestart"></param>
        public void SwitchToMenu(bool isRestart)
        {
            machine.FireAsync(startMenuTrigger, isRestart);
        }


        /// <summary>
        /// 状态切换时响应方法
        /// </summary>
        /// <param name="obj"></param>
        private void OnTransition(StateMachine<State, Trigger>.Transition obj)
        {
            Debug.Log("OnTransition " + obj.Source + " / " + obj.Destination);
        }

        /// <summary>
        /// 进入游戏状态了
        /// </summary>
        /// <param name="state"></param>
        /// <param name="playerAccount"></param>
        private UniTask OnEnterGame(GameState state, PlayerAccount playerAccount)
        {
            return state.OnEnter(playerAccount);
        }

        /// <summary>
        /// 进入菜单状态了
        /// </summary>
        /// <param name="state"></param>
        /// <param name="isRestart"></param>
        private UniTask OnEnterMenu(MenuState state, bool isRestart)
        {
            return state.OnEnter(isRestart);
        }


    }
}

