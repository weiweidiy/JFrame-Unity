using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JFrame.Common
{
    public abstract class ProdMono : MonoBehaviour
    {
        [SerializeField] Image img;
        [SerializeField] protected TextMeshProUGUI txt;

        /// <summary>
        /// 红点类型对应的资源
        /// </summary>
        [SerializeField] Sprite[] prods; // 0 normal, 1 new, 2 full, 3 ad 

        /// <summary>
        /// 是否初始化过
        /// </summary>
        bool init = false;

        /// <summary>
        /// 感兴趣的事
        /// </summary>
        protected List<string> interesting = new List<string>();

        private void Awake()
        {
            img.gameObject.SetActive(false);
            interesting = GetInterestingKeys();
        }

        private void OnEnable()
        {
            if (init == true)
                Regist();
        }


        private void Start()
        {
            //Debug.LogError("红点注册");
            Regist();

            init = true;
        }

        /// <summary>
        /// 子类提供
        /// </summary>
        /// <returns></returns>
        protected abstract ProdManager GetProdManager();

        /// <summary>
        /// 获取感兴趣的红点配置
        /// </summary>
        /// <returns></returns>
        protected abstract List<string> GetInterestingKeys();

        /// <summary>
        /// 获取红点样式索引
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected abstract int GetSpriteIndex(ProdInfo info);


        /// <summary>
        /// 红点显示规则
        /// </summary>
        /// <param name="showInfo"></param>
        /// <returns></returns>
        protected bool GetProdEnable(ProdInfo showInfo)
        {
            return showInfo.status != ProdStatus.Null;
        }

        /// <summary>
        /// 获取文本内容
        /// </summary>
        /// <param name="showInfo"></param>
        /// <returns></returns>
        protected abstract string GetTextContent(ProdInfo showInfo);

        /// <summary>
        /// 注册
        /// </summary>
        protected void Regist()
        {
            foreach (var key in interesting)
            {
                DoRegist(key, OnStatusUpdate);
            }
        }

        protected abstract void DoRegist(string key, Action<string, ProdInfo, string> action);

        /// <summary>
        /// 反注册
        /// </summary>
        protected void UnRegist()
        {
            foreach (var key in interesting)
            {
                DoUnRegist(key, OnStatusUpdate);
            }
        }

        protected abstract void DoUnRegist(string key, Action<string, ProdInfo, string> onStatusUpdate);


        /// <summary>
        /// 动态添加感兴趣的事件
        /// </summary>
        /// <param name="key"></param>
        /// <param name="isParentNode"></param>
        public void AddInteresting(string key)
        {
            if (interesting == null)
                interesting = new List<string>();

            interesting.Add(key);
        }

        /// <summary>
        /// 状态更新
        /// </summary>
        /// <param name="type"></param>
        /// <param name="info"></param>
        /// <param name="uid"></param>
        protected virtual void OnStatusUpdate(string type, ProdInfo info, string uid)
        {
            img.gameObject.SetActive(GetProdEnable(info));
            txt.text = GetTextContent(info);
            int iconIndex = GetSpriteIndex(info);
            img.sprite = prods[iconIndex];
        }

        private void OnDisable()
        {
            UnRegist();
        }
    }
}