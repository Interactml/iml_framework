using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Abrusle; //Required for scriptable object singleton 

[AssetPath("IMLSettings")]
public class IMLSettings : ScriptableObjectSingleton<IMLSettings>
{
    /// <summary>
    /// Controls if Regression and DTW nodes show, and if the testing interface is shown on MLSystems
    /// </summary>
    [Tooltip("Controls if Regression and DTW nodes show, and if the testing interface is shown on MLSystems")]
    public bool UseTestingState;

}
