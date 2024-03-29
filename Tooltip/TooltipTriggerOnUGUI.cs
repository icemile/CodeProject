﻿/*************************************************************************
 *  Copyright © 2019 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  TooltipTriggerOnUGUI.cs
 *  Description  :  Trigger for Tooltip on UGUI.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  0.1.0
 *  Date         :  7/2/2019
 *  Description  :  Initial development version.
 *************************************************************************/

using MGS.UIForm;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MGS.Tooltip
{
    /// <summary>
    /// Trigger for Tooltip on UGUI.
    /// </summary>
    [AddComponentMenu("MGS/Tooltip/TooltipTriggerOnUGUI")]
    [RequireComponent(typeof(UIBehaviour))]
    public class TooltipTriggerOnUGUI : TooltipTrigger, IPointerEnterHandler, IPointerExitHandler
    {
        #region Protected Method
        /// <summary>
        /// On mouse pointer enter UI.
        /// </summary>
        /// <param name="eventData">Event data.</param>
        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            UIFormManager.Instance.OpenForm<TextTooltipForm>(tipContent);
        }

        /// <summary>
        /// On mouse pointer exit UI.
        /// </summary>
        /// <param name="eventData">Event data.</param>
        public virtual void OnPointerExit(PointerEventData eventData)
        {
            UIFormManager.Instance.CloseForm<TextTooltipForm>();
        }
        #endregion
    }
}