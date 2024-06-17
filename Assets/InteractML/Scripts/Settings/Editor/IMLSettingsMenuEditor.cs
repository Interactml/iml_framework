using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace InteractML.IMLEditor
{
    /// <summary>
    /// Shows a dropdown in the unity main editor to configure the IMLSettings file in Resources
    /// </summary>
    public class IMLSettingsMenuEditor
    {
        private const string SettingMenuPath = "InteractML/UseTestingState";

        private static bool IsSettingEnabled
        {
            //get => EditorPrefs.GetBool(SettingPrefKey);
            //set => EditorPrefs.SetBool(SettingPrefKey, value);
            get => IMLSettings.Instance.UseTestingState;
            set => IMLSettings.Instance.UseTestingState = value;
        }

        [MenuItem(SettingMenuPath, priority = 1)]
        private static void Setting()
        {
            IsSettingEnabled = !IsSettingEnabled;
        }

        [MenuItem(SettingMenuPath, true)]
        private static bool SettingValidate()
        {
            Menu.SetChecked(SettingMenuPath, IsSettingEnabled);
            return true;
        }
    }
}