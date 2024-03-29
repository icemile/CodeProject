﻿/*************************************************************************
 *  Copyright © 2018-2019 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  UIFormSettingsEditor.cs
 *  Description  :  Editor for UI form settings.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  0.1.0
 *  Date         :  3/20/2018
 *  Description  :  Initial development version.
 *************************************************************************/

using MGS.UIForm;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace MGS.UIFormEditor
{
    [CustomEditor(typeof(UIFormSettings), true)]
    public class UIFormSettingsEditor : Editor
    {
        #region Field and Property
        protected UIFormSettings Target { get { return target as UIFormSettings; } }
        #endregion

        #region Protected Method
        [MenuItem("Tool/UI Form Settings &F")]
        protected static void FocusSettings()
        {
            var settingsPath = string.Format("Assets/Resources/{0}.asset", UIFormManager.SETTINGS_PATH);
            var settings = AssetDatabase.LoadAssetAtPath(settingsPath, typeof(UIFormSettings)) as UIFormSettings;
            if (settings == null)
            {
                var fullPath = string.Format("{0}/Resources/{1}.asset", Application.dataPath, UIFormManager.SETTINGS_PATH);
                RequireDirectory(fullPath);

                settings = CreateInstance<UIFormSettings>();
                AssetDatabase.CreateAsset(settings, settingsPath);
            }
            Selection.activeObject = settings;
        }

        protected static void RequireDirectory(string path)
        {
            var dir = Path.GetDirectoryName(path);
            if (Directory.Exists(dir))
            {
                return;
            }
            Directory.CreateDirectory(dir);
        }

        protected bool CheckRepeated<T>(ICollection<T> collections)
        {
            var hashSet = new HashSet<T>(collections);
            return collections.Count != hashSet.Count;
        }

        protected bool CheckNullOrEmpty(IEnumerable<string> enums)
        {
            foreach (var item in enums)
            {
                if (string.IsNullOrEmpty(item))
                {
                    return true;
                }
            }
            return false;
        }

        protected void CreateFolderForPrefab(List<string> layers)
        {
            foreach (var layer in layers)
            {
                var layerDir = string.Format(UIFormManager.PREFAB_PATH_FORMAT, layer, string.Empty);
                var prefabDir = string.Format("{0}/Resources/{1}", Application.dataPath, layerDir);
                RequireDirectory(prefabDir);
            }
        }
        #endregion

        #region Public Method
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (Target.layers.Count == 0)
            {
                return;
            }

            if (CheckNullOrEmpty(Target.layers))
            {
                EditorGUILayout.HelpBox("The elements in the Layers can not be empty.", MessageType.Error, true);
                return;
            }

            if (CheckRepeated(Target.layers))
            {
                EditorGUILayout.HelpBox("The elements in the Layers can not be repeated.", MessageType.Error, true);
                return;
            }

            if (GUILayout.Button("Create Folder For Prefab"))
            {
                CreateFolderForPrefab(Target.layers);
                AssetDatabase.Refresh();
            }
        }
        #endregion
    }
}