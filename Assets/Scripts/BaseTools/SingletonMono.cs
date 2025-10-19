using UnityEngine;

namespace BaseTools
{
    public class SingletonMono<T> : MonoBehaviour where T : Component
    {
        private static bool _inDestroying = false;
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_inDestroying)
                    return null;


                if (_instance == null)
                {
                    // 尝试在场景中查找已存在的实例
                    _instance = FindObjectOfType<T>();
                    // 如果仍未找到，则创建一个新的 GameObject 并挂载该组件
                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject($"[{typeof(T).Name}]");
                        _instance = singletonObject.AddComponent<T>();
                        // 可选：取消注释以在场景切换时保留
                        // DontDestroyOnLoad(singletonObject);
                    }
                }

                return _instance;
            }
        }

        /// <summary>
        /// 在 Awake 阶段确保单例的唯一性。
        /// </summary>
        protected virtual void Awake()
        {
            // 如果已有实例且不是当前对象，则销毁当前对象
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            // 设置当前实例为唯一实例
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
            // 可选：取消注释以支持跨场景持久化
            // Don'tDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// 可选：在 OnDestroy 中清理实例引用（防止编辑器下热重载导致的残留）
        /// </summary>
        protected virtual void OnDestroy()
        {
            _inDestroying = true;
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }
}