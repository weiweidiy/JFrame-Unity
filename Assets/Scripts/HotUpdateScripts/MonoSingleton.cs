using UnityEngine;

namespace JFrame.Game.View
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        public static T Ins;
        protected virtual void Awake()
        {
            if (Ins != null)
            {
                Debug.LogError("重复单例 :" + Ins.gameObject.name);
                Destroy(gameObject);
                return;
            }
            Ins = (T)this;
        }

        protected virtual void OnDestroy()
        {
            Ins = null;
        }
    }
}