using System;
using System.Collections.Generic;
using UnityEngine;

namespace JFrame.Common
{
    public abstract class ProdLogic<TData> : IObservable<ProdInfo>
    {
        /// <summary>
        /// 指定id的观察者
        /// </summary>
        List<ProdObserver> observers;

        /// <summary>
        /// 不关心id的观察者
        /// </summary>
        List<ProdObserver> observersWithoutUID;

        /// <summary>
        /// 红点数据
        /// </summary>
        protected List<ProdInfo> cachInfos;

        /// <summary>
        /// 数据更新通知器
        /// </summary>
        IProdDataNotifier<TData> dataNotifier = null;

        public ProdLogic()
        {
            observers = new List<ProdObserver>();
            observersWithoutUID = new List<ProdObserver>();

            dataNotifier = GetDataNotifier();
            dataNotifier.Init();
            dataNotifier.onDataChanged += OnDataChanged;

            cachInfos = GetInfoList();
        }

        /// <summary>
        /// 重置缓存数据
        /// </summary>
        public void ResetCacheInfos()
        {
            cachInfos = GetInfoList();
        }

        /// <summary>
        /// 获取数据变更通知器
        /// </summary>
        /// <returns></returns>
        protected abstract IProdDataNotifier<TData> GetDataNotifier();

        /// <summary>
        /// 获取数据的唯一ID
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected abstract string GetUID(TData data);

        /// <summary>
        /// 获取状态
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected abstract ProdStatus GetStatus(TData data);

        /// <summary>
        /// 数据转换成prodinfo，核心方法，就是最底层的 红点逻辑
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual ProdInfo GetCellNodeProdInfo(TData data)
        {
            var status = GetStatus(data);

            var number = GetNumber(status);

            ProdInfo info = new ProdInfo(GetUID(data), status, number);

            return info;
        }


        /// <summary>
        /// 根据其他红点状态创建一个不关注uid的红点数据，就是各种常见的 父亲页签
        /// </summary>
        /// <param name="lstCacheInfo"></param>
        /// <returns></returns>
        protected virtual ProdInfo GetParentNodeProdInfo(List<ProdInfo> lstCacheInfo)
        {
            ProdInfo resultInfo = new ProdInfo("", ProdStatus.Null, 0);

            //遍历红点状态，只要有一个是亮的，就通知亮，否则通知不亮
            for (int i = 0; i < lstCacheInfo.Count; i++)
            {
                var cachInfo = lstCacheInfo[i];

                if (cachInfo.status == ProdStatus.Normal)
                {
                    resultInfo.status = ProdStatus.Number;
                    resultInfo.number += cachInfo.number;
                }
            }
            return resultInfo;
        }


        /// <summary>
        /// 获取数量
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        protected virtual int GetNumber(ProdStatus status)
        {
            return status == ProdStatus.Null ? 0 : 1;
        }

        /// <summary>
        /// 数据发生变化了
        /// </summary>
        /// <param name="obj"></param>
        protected void OnDataChanged(TData data, object arg)
        {
            if (IsValidData(data, arg))
            {
                //更新 tasks , 然后发通知
                var info = UpdateInfo(data);
                var observer = GetObserverInterestUID(data);
                NotifyObserver(observer, info);

                //更新不关注UID的观察者
                var infoWithoutUID = GetParentNodeProdInfo(cachInfos);
                NotifyObserversUninterestUID(infoWithoutUID);
            }
        }


        /// <summary>
        /// 更新红点数据方法
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual ProdInfo UpdateInfo(TData data)
        {
            ProdInfo result = GetNewInfo(data);

            bool needAdd = true;
            for (int i = 0; i < cachInfos.Count; i++)
            {

                if (cachInfos[i].uid.Equals(GetUID(data)))
                {
                    result = GetCellNodeProdInfo(data);
                    cachInfos[i] = result;
                    needAdd = false;

                    break;
                }
            }

            if (needAdd == true)
            {
                cachInfos.Add(result);
            }

            return result;
        }

        /// <summary>
        /// 获得新数据时的红点信息，默认是标记普通红点状态, 子类可以重写
        /// </summary>
        /// <param name="newData"></param>
        /// <returns></returns>
        protected virtual ProdInfo GetNewInfo(TData newData)
        {
            var status = GetStatus(newData);
            ProdInfo result = new ProdInfo(GetUID(newData), status, GetNumber(status));
            return result;
        }

        /// <summary>
        /// 是否有效数据,大部分情况默认即可，像Task消息 maintask和dailytask共用数据的，需要有效数据类型判断区分
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual bool IsValidData(TData data, object arg)
        {
            if (data == null)
                return false;

            return true;
        }

        /// <summary>
        /// 添加观察者
        /// </summary>
        /// <param name="observer"></param>
        /// <returns></returns>
        public IDisposable Subscribe(IObserver<ProdInfo> observer)
        {
            //转换成实际的红点观察者
            var prodObserver = observer as ProdObserver;

            //不关注ID的观察者
            if (prodObserver.UID.Equals(""))
            {
                //加入列表
                if (!observersWithoutUID.Contains(prodObserver))
                {
                    observersWithoutUID.Add(prodObserver);

                    //把缓存数据通知给不关注ID的观察者
                    ProdInfo info = GetParentNodeProdInfo(cachInfos);
                    NotifyObserver(prodObserver, info);
                }

                return new UnSubScriberProd(observersWithoutUID, prodObserver);
            }

            //关注ID的观察者
            if (!observers.Contains(prodObserver))
            {
                observers.Add(prodObserver);

                var info = GetInfoFromCache(prodObserver.UID);
                NotifyObserver(prodObserver, info);
            }

            return new UnSubScriberProd(observers, prodObserver);
        }

        /// <summary>
        /// 通知关注UID的观察者
        /// </summary>
        /// <param name="observer"></param>
        /// <param name="info"></param>
        protected void NotifyObserver(ProdObserver observer, ProdInfo info)
        {
            if (observer == null)
                return;

            observer.OnNext(info);
        }

        /// <summary>
        /// 通知所有不关注UID的观察者，数据变更
        /// </summary>
        /// <param name="info"></param>
        protected void NotifyObserversUninterestUID(ProdInfo info)
        {
            foreach (var observer in observersWithoutUID)
            {
                NotifyObserver(observer, info);
            }
        }

        #region get方法
        /// <summary>
        /// 获取数据，将原始数据按规则转换成红点数据
        /// </summary>
        /// <returns></returns>
        protected List<ProdInfo> GetInfoList()
        {
            var result = new List<ProdInfo>();

            foreach (var data in dataNotifier.GetDataList())
            {
                result.Add(GetCellNodeProdInfo(data));
            }

            return result;
        }

        /// <summary>
        /// 获取关注指定uid的观察者
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        protected ProdObserver GetObserverInterestUID(TData data)
        {
            return observers.Find((observer) => observer.EqualUid(GetUID(data)));
        }

        /// <summary>
        /// 根据uid返回一个info对象
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        ProdInfo GetInfoFromCache(string uid)
        {
            for (int i = 0; i < cachInfos.Count; i++)
            {
                var info = cachInfos[i];

                if (info.uid.Equals(uid))
                    return info;
            }

            return new ProdInfo("", ProdStatus.Null, 0);
        }
        #endregion
    }



}