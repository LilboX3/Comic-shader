using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable]
public class OutlineSettings
{
    //Configurable values for full screen shader
    public Shader OutlineShader;
    public Color OutlineColor;
}

public class OutlineRendererFeature : ScriptableRendererFeature
{
    [SerializeField]
    private OutlineSettings Settings;

    OutlinePass m_OutlinePass;
    Material m_Material;

    public override void Create()
    {
        //Apply material with shader
        if (Settings.OutlineShader != null)
            m_Material = new Material(Settings.OutlineShader);

        m_OutlinePass = new OutlinePass(m_Material,
            Settings.OutlineColor);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        //Effect will only apply to Game camera
        if (renderingData.cameraData.cameraType != CameraType.Game) return;

        //Configure input for the pass to be the color buffer
        m_OutlinePass.ConfigureInput(ScriptableRenderPassInput.Color);
        //Output target: the camera's color buffer
        //m_OutlinePass.SetTarget(renderer.cameraColorTargetHandle);
        renderer.EnqueuePass(m_OutlinePass);
    }

    protected override void Dispose(bool disposing)
    {
        CoreUtils.Destroy(m_Material);
    }

}
