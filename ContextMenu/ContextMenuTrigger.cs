﻿/*************************************************************************
 *  Copyright © 2017-2019 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  ContextMenuTrigger.cs
 *  Description  :  Trigger of context menu.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  0.1.0
 *  Date         :  3/12/2018
 *  Description  :  Initial development version.
 *************************************************************************/

using MGS.Common.Logger;
using MGS.UCommon.DesignPattern;
using MGS.UCommon.Utility;
using UnityEngine;

namespace MGS.ContextMenu
{
    /// <summary>
    /// Trigger of context menu.
    /// </summary>
    [AddComponentMenu("MGS/ContextMenu/ContextMenuTrigger")]
    [RequireComponent(typeof(Camera))]
    public sealed class ContextMenuTrigger : SingleMonoBehaviour<ContextMenuTrigger>, IContextMenuTrigger
    {
        #region Field and Property
        /// <summary>
        /// LayerMask of raycast.
        /// </summary>
        [Tooltip("LayerMask of raycast.")]
        [SerializeField]
        private LayerMask layerMask = 1;

        /// <summary>
        /// Max distance of raycast.
        /// </summary>
        [Tooltip("Max distance of raycast.")]
        [SerializeField]
        private float maxDistance = 100;

        /// <summary>
        /// Handler of contex menu trigger.
        /// </summary>
        [Tooltip("Handler of contex menu trigger.")]
        [SerializeField]
        private ContextMenuTriggerHandler handler = null;

        /// <summary>
        /// Camera to raycast.
        /// </summary>
        public Camera RayCamera { private set; get; }

        /// <summary>
        /// LayerMask of raycast.
        /// </summary>
        public LayerMask LayerMask
        {
            set { layerMask = value; }
            get { return layerMask; }
        }

        /// <summary>
        /// Max distance of raycast.
        /// </summary>
        public float MaxDistance
        {
            set { maxDistance = value; }
            get { return maxDistance; }
        }

        /// <summary>
        /// Handler of contex menu trigger.
        /// </summary>
        public IContextMenuTriggerHandler Handler { set; get; }

        /// <summary>
        /// Context menu form of trigger.
        /// </summary>
        private IContextMenuForm menuForm;
        #endregion

        #region Protected Method
        /// <summary>
        /// Awake component.
        /// </summary>
        protected override void SingleAwake()
        {
            base.SingleAwake();

            RayCamera = GetComponent<Camera>();
            Handler = handler;
        }
        #endregion

        #region Private Method
        /// <summary>
        /// Update component.
        /// </summary>
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (menuForm == null || menuForm.IsDisposed)
                {
                    return;
                }

                if (EventSystemUtility.CheckPointerOverGameObject(menuForm.rectTransform.gameObject))
                {
                    return;
                }
                OnMenuTriggerExit();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                if (menuForm != null && !menuForm.IsDisposed)
                {
                    if (EventSystemUtility.CheckPointerOverGameObject(menuForm.rectTransform.gameObject))
                    {
                        return;
                    }
                    OnMenuTriggerExit();
                }

                var ray = RayCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance, layerMask))
                {
                    OnMenuTriggerEnter(hitInfo);
                }
            }
        }

        /// <summary>
        /// On context menu trigger enter.
        /// </summary>
        /// <param name="hitInfo">Raycast hit info of target.</param>
        private void OnMenuTriggerEnter(RaycastHit hitInfo)
        {
            if (Handler == null)
            {
                LogUtility.LogWarning(0, "Do nothing on menu trigger enter: The handler of menu trigger is null.");
                return;
            }
            menuForm = Handler.OnMenuTriggerEnter(hitInfo);
        }

        /// <summary>
        /// On context menu trigger exit.
        /// </summary>
        /// <returns></returns>
        private void OnMenuTriggerExit()
        {
            if (Handler == null)
            {
                LogUtility.LogWarning(0, "Do nothing on menu trigger exit: The handler of menu trigger is null.");
                return;
            }
            Handler.OnMenuTriggerExit(menuForm);
            menuForm = null;
        }
        #endregion
    }
}