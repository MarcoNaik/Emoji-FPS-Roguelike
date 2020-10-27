//////////////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        27.10.19
// Date last edited:    09.11.19
//////////////////////////////////////////////////

using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Retro_3D_Shader_Pack_for_Unity.Shaders
{
    [Serializable]
    [PostProcess(typeof(RetroPostProcessEffectRenderer), PostProcessEvent.AfterStack, "Retro Effects")] // Tells Unity that this class holds the data for a post-processing effect.
    public sealed class RetroPostProcessEffect : PostProcessEffectSettings
    {   
        [Tooltip("Whether the pixelation effect uses a fixed vertical resolution or a pixel size multiplier value.")]
        public BoolParameter usesFixedResolution = new BoolParameter { value = false };
        [UnityEngine.Min(1.0f), Tooltip("The value used to scale the width and height of each pixel")]
        public IntParameter pixelScale = new IntParameter { value = 4 };
        [UnityEngine.Min(1), Tooltip("The vertical resolution of the image output by the pixelation effect.")]
        public IntParameter fixedVerticalResolution = new IntParameter { value = 480 };
    
        [Range(0.0f, 1.0f), Tooltip("Determines the range of the color palette applied to the image.")]
        public FloatParameter colorDepth = new FloatParameter { value = 0.15f };

        [DisplayName("Dither Pattern"), Tooltip("The pattern used to implement ordered dithering.")]
        public TextureParameter ditherPattern = new TextureParameter { value = null, defaultState = TextureParameterDefault.None };
        [Tooltip("The scale multiplier for the dither pattern (the dither pattern size is also automatically scaled according to the pixelation effect scaling).")]
        public IntParameter ditherPatternScale = new IntParameter { value = 1 };
        [Range(0.0f, 1.0f), Tooltip("The threshold used to control the range of colors that are affected by dithering.")]
        public FloatParameter ditherThreshold = new FloatParameter { value = 0.75f };
        [Range(0.0f, 1.0f), Tooltip("The intensity of the dithering effect.")]
        public FloatParameter ditherIntensity = new FloatParameter { value = 0.15f };    
    }


    public sealed class RetroPostProcessEffectRenderer : PostProcessEffectRenderer<RetroPostProcessEffect> // The class used to handle the C# side of the post-process effect rendering.
    {
        // Renders the image output by the camera using the post-process shader and outputs it to the appropriate destination.
        public override void Render(PostProcessRenderContext context)
        {
            // The property sheet used to handle the post-process shader code.
            PropertySheet sheet = context.propertySheets.Get(Shader.Find("Retro 3D Shader Pack/Post-Process"));

            // Depending on whether fixed resolution mode is being used, passes either the resolution or scaling value and sets the other to -1.
            if (settings.usesFixedResolution)
            {
                sheet.properties.SetInt("_PixelScale", -1);
                sheet.properties.SetInt("_FixedVerticalResolution", settings.fixedVerticalResolution);
            }
            else
            {
                sheet.properties.SetInt("_PixelScale", settings.pixelScale);
                sheet.properties.SetInt("_FixedVerticalResolution", -1);
            }
            sheet.properties.SetFloat("_SourceRenderWidth", context.width);
            sheet.properties.SetFloat("_SourceRenderHeight", context.height);

            sheet.properties.SetFloat("_ColorDepth", settings.colorDepth);

            if (settings.ditherPattern.value)
            {
                sheet.properties.SetTexture("_DitherPattern", settings.ditherPattern);
                sheet.properties.SetInt("_DitherPatternScale", settings.ditherPatternScale);
                sheet.properties.SetFloat("_DitherThreshold", settings.ditherThreshold);
                sheet.properties.SetFloat("_DitherIntensity", settings.ditherIntensity);            
            }          
        
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0); // Applies the shader to the source image and outputs the result to the appropriate destination.
        }
    }
}