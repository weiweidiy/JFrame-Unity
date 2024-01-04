using System;

namespace JFrame.Common
{
    public abstract class ProdCellNode : ProdMono
    {
        /// <summary>
        /// 当不是父亲节点时，该属性代表唯一id标识符
        /// </summary>
        public string Uid;

        protected override void DoRegist(string key, Action<string, ProdInfo, string> action)
        {
            GetProdManager().Regist(key, OnStatusUpdate, Uid);
        }

        protected override void DoUnRegist(string key, Action<string, ProdInfo, string> onStatusUpdate)
        {
            GetProdManager().UnRegist(key, OnStatusUpdate);
        }

        protected override string GetTextContent(ProdInfo showInfo)
        {
            return showInfo.status == ProdStatus.Number ? showInfo.number.ToString() : "";
        }

        protected override int GetSpriteIndex(ProdInfo info)
        {
            switch (info.status)
            {
                case ProdStatus.New:
                    return 1;
                case ProdStatus.Full:
                    return 2;
                case ProdStatus.Ad:
                    return 3;
                default:
                    return 0;
            }
        }
    }

}