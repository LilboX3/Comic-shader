using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OutlinePass : ScriptableRenderPass
{
    readonly Color m_Color;
    RTHandle m_CameraColorTarget;
    Material m_Material;
    static int colorID = Shader.PropertyToID("_OutlineColor");

    public OutlinePass(Material material, Color color)
    {
        m_Material = material;
        m_Color = color;

        //Tell renderer where to inject outline pass in the pipeline (before post processing)
        renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
    }

    /*public void SetTarget(RTHandle colorHandle)
    {
        m_CameraColorTarget = colorHandle;
    }*/

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        base.OnCameraSetup(cmd, ref renderingData);

        //Used RTHandle instead of RenderTargetIdentifier (obsolete) (looked for alternative method in scriptable render pass)
        m_CameraColorTarget = renderingData.cameraData.renderer.cameraColorTargetHandle;
        RTHandle depthHandle = renderingData.cameraData.renderer.cameraDepthTargetHandle;
        ConfigureTarget(m_CameraColorTarget, depthHandle);
    }

    // Take assigned camera color buffer
    // Create command buffer that has it as render target
    // Then draw a full screen quad with the material applied to it
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        var camera = renderingData.cameraData.camera;
        if (camera.cameraType != CameraType.Game) return;

        if (m_Material == null) return;

        CommandBuffer cb = CommandBufferPool.Get("OutlinePass");
        //To see pass in Frame Debugger (does not affect functionality)
        cb.BeginSample("Outline Pass");

        //Assign material shader properties
        m_Material.SetColor(colorID, m_Color);

        //Set command buffers render target to camera's color buffer
        cb.SetRenderTarget(new RenderTargetIdentifier(m_CameraColorTarget, 0, CubemapFace.Unknown, -1));
        //Render full screen quad, assign the material to it
        cb.DrawMesh(RenderingUtils.fullscreenMesh, Matrix4x4.identity, m_Material);

        cb.EndSample("Outline Pass");
        //Execute Command buffer against rendering context
        context.ExecuteCommandBuffer(cb);

        //Tidy up
        cb.Clear();
        CommandBufferPool.Release(cb);
    }
}
