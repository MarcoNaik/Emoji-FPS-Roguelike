using System.Collections;
using System.Collections.Generic;
using Retro_3D_Shader_Pack_for_Unity.Shaders;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


[RequireComponent(typeof(Camera))]
public class ExampleSceneCamera : MonoBehaviour
{
    public PostProcessVolume retroPostProcessVolume;

    private RetroPostProcessEffect postProcessEffect = null;
   

    private void Start()
    {
        retroPostProcessVolume.profile.TryGetSettings(out postProcessEffect);
    }

    private void Update()
    {
        UpdatePostProcessEffects();        
    }

    private void UpdatePostProcessEffects()
    {
        if (Input.GetKeyDown(KeyCode.P))
            postProcessEffect.enabled.value = !postProcessEffect.enabled.value;
    }
}