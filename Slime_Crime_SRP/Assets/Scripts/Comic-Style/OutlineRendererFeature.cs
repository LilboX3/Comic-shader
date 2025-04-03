using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Apple;
using UnityEngine.Rendering.RenderGraphModule.Util;

//Manager class, setting up the job
public class OutlineRendererFeature1 : ScriptableRendererFeature
{
    //Actual rendering logic, drawing action / worker class!
    class OutlineEffectPass : ScriptableRenderPass
    {
        const string m_PassName = "OutlineEffectPass";
        Material m_OutlineMaterial;

        // This class stores the data needed by the RenderGraph pass.
        // It is passed as a parameter to the delegate function that executes the RenderGraph pass.
        private class PassData
        {
            //Any additional data that the pass might have to access. Not needed here
        }

        public void SetUp(Material mat)
        {
            m_OutlineMaterial = mat;
            requiresIntermediateTexture = true; //TODO: Needs to read current color texture of the scene?
        }

        // RecordRenderGraph is where the RenderGraph handle can be accessed, through which render passes can be added to the graph.
        // FrameData is a context container through which URP resources can be accessed and managed.
        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            var resourceData = frameData.Get<UniversalResourceData>();
            if (resourceData.isActiveTargetBackBuffer)
            {
                Debug.LogError($"Skipping render pass. Needs an intermediate ColorTexture, cannot use BackBuffer as " +
                    $"a texture input."); //TODO: needed?
                return;
            }
            //Texture to use in blit operation
            var source = resourceData.activeColorTexture;
            //Render texture properties in render graph description struct
            var destinationDesc = renderGraph.GetTextureDesc(source);

            //Name of destination texture should match pass
            destinationDesc.name = $"CameraColor-{m_PassName}";
            //Modify, not start from new!
            destinationDesc.clearBuffer = false;

            //Destination texture handle with set up parameters
            TextureHandle destination = renderGraph.CreateTexture(destinationDesc);

            //Have source and destination, material and default Shader pass 0 for blit operation
            //Helper struct
            RenderGraphUtils.BlitMaterialParameters para = new(source, destination, m_OutlineMaterial, 0);
            renderGraph.AddBlitPass(para, passName: m_PassName);

            resourceData.cameraColor = destination;
        }

        //OnCameraSetUp, Excute, OnCameraCleanup not used in Render graph, will be deprecated
    }

    public RenderPassEvent injectionPoint = RenderPassEvent.AfterRenderingPostProcessing;
    public Material material;

    OutlineEffectPass m_ScriptablePass;

    /// <inheritdoc/>
    public override void Create()
    {
        m_ScriptablePass = new OutlineEffectPass();

        // Configures where the render pass should be injected.
        m_ScriptablePass.renderPassEvent = injectionPoint;
        //RenderPassEvent.BeforeRenderingPostProcessing: Change scene after normal rendering, but before post processing
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if(material == null)
        {
            Debug.LogWarning($"No material found. Skipping");
            return;
        }
        m_ScriptablePass.SetUp(material);
        renderer.EnqueuePass(m_ScriptablePass);
    }
}
