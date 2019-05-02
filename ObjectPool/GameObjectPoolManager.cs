﻿/*************************************************************************
 *  Copyright © 2018 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  GameObjectPoolManager.cs
 *  Description  :  Manager of gameobject pool.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  0.1.0
 *  Date         :  2/9/2018
 *  Description  :  Initial development version.
 *************************************************************************/

using MGS.Common.DesignPattern;
using MGS.Common.Logger;
using System.Collections.Generic;
using UnityEngine;

namespace MGS.ObjectPool
{
    /// <summary>
    /// Manager of gameobject pool.
    /// </summary>
    [AddComponentMenu("MGS/ObjectPool/GameObjectPoolManager")]
    public sealed class GameObjectPoolManager : SingleMonoBehaviour<GameObjectPoolManager>
    {
        #region Field and Property
        /// <summary>
        /// Settings of pools.
        /// </summary>
        [SerializeField]
        private List<GameObjectPoolSettings> poolsSettings = new List<GameObjectPoolSettings>();

        /// <summary>
        /// Dictionary store pools info(name and pool).
        /// </summary>
        private Dictionary<string, GameObjectPool> poolsInfo = new Dictionary<string, GameObjectPool>();
        #endregion

        #region Protected Method
        protected override void SingleAwake()
        {
            foreach (var poolSettings in poolsSettings)
            {
#if UNITY_EDITOR
                if (poolsInfo.ContainsKey(poolSettings.name))
                {
                    LogUtility.LogError("[GameObjectPoolManager] SingleAwake error: the pool name {0} configured in the Pools Settings is not unique in this manager.", poolSettings.name);
                    continue;
                }
#endif
                CreatePool(poolSettings);
            }
        }
        #endregion

        #region Public Method
        /// <summary>
        /// Create a pool in this manager.
        /// </summary>
        /// <param name="name">Name of GameObjectPool.</param>
        /// <param name="prefab">Prefab of GameObjectPool.</param>
        /// <param name="maxCount">Max count limit of gameobjects in pool.</param>
        /// <returns>Pool created base on parameters.</returns>
        public GameObjectPool CreatePool(string name, GameObject prefab, int maxCount = 100)
        {
            if (string.IsNullOrEmpty(name))
            {
                LogUtility.LogError("[GameObjectPoolManager] Create pool is failed : The pool name can not be null or empty.");
                return null;
            }

            if (poolsInfo.ContainsKey(name))
            {
                LogUtility.LogWarning("[GameObjectPoolManager] Create pool is cancelled : The pool that name is {0} already exist in this manager.", name);
                return poolsInfo[name];
            }

            if (prefab == null)
            {
                LogUtility.LogError("[GameObjectPoolManager] Create pool is failed : The prefab of pool can not be null.");
                return null;
            }

            //Create new root for pool.
            var poolRoot = new GameObject(name);
            poolRoot.transform.parent = transform;

            //Create new pool.
            var newPool = new GameObjectPool(poolRoot.transform, prefab, maxCount);
            poolsInfo.Add(name, newPool);

#if UNITY_EDITOR
            var settings = new GameObjectPoolSettings(name, prefab, maxCount);
            if (!poolsSettings.Contains(settings))
            {
                poolsSettings.Add(settings);
            }
#endif
            return newPool;
        }

        /// <summary>
        /// Create a pool in this manager.
        /// </summary>
        /// <param name="poolSettings">Settings of pool.</param>
        /// <returns>Pool created base on settings.</returns>
        public GameObjectPool CreatePool(GameObjectPoolSettings poolSettings)
        {
            return CreatePool(poolSettings.name, poolSettings.prefab, poolSettings.maxCount);
        }

        /// <summary>
        /// Find GameObjectPool by name.
        /// </summary>
        /// <param name="name">Name of GameObjectPool.</param>
        /// <returns>Name match GameObjectPool.</returns>
        public GameObjectPool FindPool(string name)
        {
            if (poolsInfo.ContainsKey(name))
            {
                return poolsInfo[name];
            }
            else
            {
                LogUtility.LogWarning("[GameObjectPoolManager] Find pool is failed : The pool that name is {0} does not exist in this manager.", name);
                return null;
            }
        }

        /// <summary>
        /// Delete GameObjectPool by name.
        /// </summary>
        /// <param name="name">Name of GameObjectPool.</param>
        public void DeletePool(string name)
        {
            if (poolsInfo.ContainsKey(name))
            {
                Destroy(poolsInfo[name].root.gameObject);
                poolsInfo.Remove(name);

#if UNITY_EDITOR
                foreach (var poolSettings in poolsSettings)
                {
                    if (poolSettings.name == name)
                    {
                        poolsSettings.Remove(poolSettings);
                        break;
                    }
                }
#endif
            }
            else
            {
                LogUtility.LogWarning("[GameObjectPoolManager] Delete pool is failed : The pool that name is {0} does not exist in this manager.", name);
            }
        }
        #endregion
    }
}