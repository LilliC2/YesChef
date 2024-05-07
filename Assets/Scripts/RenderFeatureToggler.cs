using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[System.Serializable]

[ExecuteAlways]
public class RenderFeatureToggler : Singleton<RenderFeatureToggler>
{
    [SerializeField]
    public ScriptableRendererFeature feature;

    public void ToggleSpeedLines(bool _bool)
    {
        feature.SetActive(_bool);
    }

}
