using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CreatGame
{
    public class CoroutineManager : MonoBehaviour
    {
        #region Instance
        private static CoroutineManager _instance;

        public static CoroutineManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    var gameObj = new GameObject("CoroutineManager");
                    _instance = gameObj.AddComponent<CoroutineManager>();
                    DontDestroyOnLoad(gameObj);
                }
                return _instance;
            }
        }

        #endregion
        
        private ulong coroutineId = 0;
        private ulong GetNextId()
        {
            return ++coroutineId;
        }
        
        private Dictionary<ulong,Coroutine> coroutines = new Dictionary<ulong,Coroutine>();

        public ulong StartC(IEnumerator routine)
        {
            var id = GetNextId();
            var coroutine = StartCoroutine(routine);
            coroutines.Add(id, coroutine);
            return id;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="time">间隔时间</param>
        /// <param name="isFirst">是否立即执行</param>
        /// <param name="loop">是否循环</param>
        /// <param name="action">方法</param>
        public ulong StartC(float time,bool isFirst , bool loop, Action action)
        {
            var id = StartC(LoopCoroutine(time,isFirst,loop,action));
            return id;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IEnumerator LoopCoroutine(float time,bool isFirst ,bool loop, Action action)
        {
            if (isFirst)
            {
                action.Invoke();
            }

            while (true)
            {
                yield return new WaitForSeconds(time);
                action.Invoke();
                if (!loop)
                {
                    break;
                }
            }
        }

        public void StopC(ulong id)
        {
            if (coroutines.ContainsKey(id))
            {
                StopCoroutine(coroutines[id]);
                coroutines.Remove(id);
            }
        }

        private void OnDestroy()
        {
            
        }
    }
}