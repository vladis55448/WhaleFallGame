using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using System;

[Serializable, VolumeComponentMenu("Shader Graphs/FogShader")]
public sealed class FogProcessingEffect : CustomPostProcessVolumeComponent, IPostProcessComponent
{
    [SerializeField]
    private FloatParameter _distance = new FloatParameter(0);
    [SerializeField]
    private ColorParameter _color = new ColorParameter(Color.black);
    [SerializeField]
    private FloatParameter _ditherSpread = new FloatParameter(0);
    [SerializeField]
    private IntParameter _colorResolution = new IntParameter(0);
    [SerializeField]
    private ClampedFloatParameter _colorFalloff = new ClampedFloatParameter(0f, 0f, 1f);
    [SerializeField]
    private FloatParameter _fogPower = new FloatParameter(0);

    Material m_Material;

    public bool IsActive() => m_Material != null;

    // Do not forget to add this post process in the Custom Post Process Orders list (Project Settings > Graphics > HDRP Global Settings).
    public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

    const string kShaderName = "Shader Graphs/FogShader";

    public override void Setup()
    {
        if (Shader.Find(kShaderName) != null)
            m_Material = new Material(Shader.Find(kShaderName));
        else
            Debug.LogError($"Unable to find shader '{kShaderName}'. Post Process Volume FogProcessingEffect is unable to load. To fix this, please edit the 'kShaderName' constant in FogProcessingEffect.cs or change the name of your custom post process shader.");
    }

    public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
    {
        if (m_Material == null)
            return;

        m_Material.SetFloat("_Distance", _distance.value);
        m_Material.SetColor("_Color", _color.value);
        m_Material.SetFloat("_DitherSpread", _ditherSpread.value);
        m_Material.SetInt("_ColorResolution", _colorResolution.value);
        m_Material.SetFloat("_ColorFalloff", _colorFalloff.value);
        m_Material.SetFloat("_FogPower", _fogPower.value);
        HDUtils.DrawFullScreen(cmd, m_Material, destination, shaderPassId: 0);
    }

    public override void Cleanup()
    {
        CoreUtils.Destroy(m_Material);
    }
}
