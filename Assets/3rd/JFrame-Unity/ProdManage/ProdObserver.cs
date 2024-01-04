using System;

namespace JFrame.Common
{
    public class ProdObserver : IObserver<ProdInfo>
    {
        /// <summary>
        /// 实际的委托对象
        /// </summary>
        Action<string, ProdInfo, string> action;

        /// <summary>
        /// 红点key
        /// </summary>
        string key;

        /// <summary>
        /// 关心的唯一id, 如果为""，则表示不关心具体的，只要有一个符合就是符合
        /// </summary>
        public string UID { get; private set; }


        public ProdObserver(string key, Action<string, ProdInfo, string> action, string uid)
        {
            this.key = key;
            this.action = action;
            this.UID = uid;
        }

        /// <summary>
        /// 判断是否拥有委托
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool HasAction(Action<string, ProdInfo, string> action)
        {
            return this.action.Equals(action);
        }

        /// <summary>
        /// 判断是否是自己关心的
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public bool EqualUid(string uid)
        {
            return this.UID.Equals(uid);
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(ProdInfo info)
        {
            action.Invoke(this.key,info, this.UID);
        }
    }

}