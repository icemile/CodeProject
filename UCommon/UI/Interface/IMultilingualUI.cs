﻿/*************************************************************************
 *  Copyright © 2019 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  IMultilingualUI.cs
 *  Description  :  Interface for multilingual UI.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  0.1.0
 *  Date         :  6/10/2019
 *  Description  :  Initial development version.
 *************************************************************************/

namespace MGS.UCommon.UI
{
    /// <summary>
    /// Interface for multilingual UI.
    /// </summary>
    public interface IMultilingualUI
    {
        #region Method
        /// <summary>
        /// Set language of UI.
        /// </summary>
        /// <param name="name">Language name.</param>
        void Language(string name);
        #endregion
    }
}