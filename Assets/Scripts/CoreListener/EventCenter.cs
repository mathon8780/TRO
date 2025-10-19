using System;
using System.Collections.Generic;
using BaseTools;
using UnityEngine;
using UnityEngine.Events;

namespace CoreListener
{
    public class EventCenter : SingletonMono<EventCenter>
    {
        private readonly Dictionary<Type, Delegate> _eventRecord = new(100);

        /// <summary>
        /// 添加监听器
        /// </summary>
        /// <param name="callBack">回调函数</param>
        /// <typeparam name="T">监听类型</typeparam>
        public void AddListener<T>(UnityAction<T> callBack)
        {
            if (callBack == null)
                Debug.LogError("null callback");

            Type eventType = typeof(T);
            _eventRecord.TryAdd(eventType, null);
            _eventRecord[eventType] = Delegate.Combine(_eventRecord[eventType], callBack);
        }

        /// <summary>
        /// 移除监听器
        /// </summary>
        /// <param name="callBack">回调函数</param>
        /// <typeparam name="T">监听类型</typeparam>
        public void RemoveListener<T>(UnityAction<T> callBack)
        {
            if (callBack == null)
                Debug.LogError("null callback");

            Type eventType = typeof(T);

            if (!_eventRecord.TryGetValue(eventType, out var del))
            {
                return;
            }

            var result = Delegate.Remove(del, callBack);
            if (result == null)
            {
                _eventRecord.Remove(eventType);
            }
            else
            {
                _eventRecord[eventType] = result;
            }
        }

        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="eventParam">事件类型</param>
        /// <typeparam name="T">事件类型</typeparam>
        public void TriggerEvent<T>(T eventParam)
        {
            if (_eventRecord.TryGetValue(typeof(T), out var del)
                && del is UnityAction<T> callback)
            {
                callback.Invoke(eventParam);
            }
        }
    }
}