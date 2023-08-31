using System;
using System.Collections.Generic;
using UnityEngine;


namespace JFrame.Common
{
    /// <summary>
    /// 红点管理:
    /// 负责监视数据的状态变化
    /// 负责监视事件的通知
    /// </summary>
    public abstract class RedDotManager 
    {   
        /// <summary>
        /// 红点逻辑映射
        /// </summary>
        protected Dictionary<string, IObservable<RedDotInfo>> mapObservables;

        /// <summary>
        /// 红点观察者列表，用于反注册
        /// </summary>
        protected Dictionary<string, List<UnSubScriberRedDot>> mapObservers;


        /// <summary>
        /// 初始化方法，在所有data manager初始化之后再初始化，因为要从他们中初始化数据
        /// </summary>
        public void Init()
        {
            mapObservables = new Dictionary<string, IObservable<RedDotInfo>>();
            mapObservers = new Dictionary<string, List<UnSubScriberRedDot>>();

            OnInit();
        }

        protected abstract void OnInit();

        /// <summary>
        /// 注册观察者
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        public void Regist(string key, Action<string, RedDotInfo, string> action, string uid)
        {
            //创建一个观察者
            var observer = new RedDotObserver(key, action, uid);

            //订阅指定类型的红点事件
            var unsubscribe = mapObservables[key].Subscribe(observer);

            //缓存指定类型订阅者
            if(!mapObservers.ContainsKey(key))
            {
                mapObservers.Add(key, new List<UnSubScriberRedDot>());
            }

            if (mapObservers[key].Contains(unsubscribe as UnSubScriberRedDot))
            {
                Debug.LogError("不能重复注册观察者 ");
                return;
            }

            mapObservers[key].Add(unsubscribe as UnSubScriberRedDot);
          
        }

        /// <summary>
        /// 移除观察者
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        public void UnRegist(string key, Action<string, RedDotInfo, string> action)
        {
            if (!mapObservers.ContainsKey(key))
            {
                Debug.LogError("没有红点类型 " + key);
                return;
            }

            var unsubscribes = mapObservers[key];
            foreach (var unsubscribe in unsubscribes)
            {
                if (unsubscribe.HasAction(action))
                {
                    unsubscribe.Dispose();
                    mapObservers[key].Remove(unsubscribe);
                    break;
                }
            }
        }
    }

}