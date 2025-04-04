using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Apple;
using UnityEngine.Rendering.RenderGraphModule.Util;

//Manager class, setting up the job
public class CustomRendererFeature : ScriptableRendererFeature
{
    //Actual rendering logic, drawing action / worker class!
    class OutlineEffectPass : ScriptableRenderPass
    {
        const string m_OutlinePassName = "OutlineEffectPass";
        Material m_OutlineMaterial;

        // This class stores the data needed by the RenderGraph pass.
        // It is passed as a parameter to the delegate function that executes the RenderGraph pass.
        private class PassData
        {
            //Any additional data that the pass might have to access. Not needed here
        }

        public void SetUp(Material outlineMat)
        {
            m_OutlineMaterial = outlineMat;
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
                    $"a texture input."); //TODO: is this needed?
                return;
            }
            //Texture to use in blit operation
            var source = resourceData.activeColorTexture;

            //Render texture properties in render graph description struct
            var destinationDesc = renderGraph.GetTextureDesc(source);
            //Modify, not start from new!
            destinationDesc.clearBuffer = false;

            //Name of destination texture should match pass
            destinationDesc.name = $"CameraColor-{m_OutlinePassName}";
            //Destination texture handle with set up parameters
            TextureHandle destination = renderGraph.CreateTexture(destinationDesc);

            //Have source and destination, material and default Shader pass 0 for blit operation
            //Helper struct, for outline blit operation
            RenderGraphUtils.BlitMaterialParameters outlineBlit = new(source, destination,
                m_OutlineMaterial, 0);
            renderGraph.AddBlitPass(outlineBlit, passName: m_OutlinePassName);

            resourceData.cameraColor = destination;
        }

        //OnCameraSetUp, Excute, OnCameraCleanup not used in Render graph, will be deprecated
    }

    class HalftoneEffectPass: ScriptableRenderPass
    {
        const string m_HalftonePassName = "HalftoneEffectPass";
        Material m_HalftoneMaterial;

        public void SetUp(Material halftoneMat)
        {
            m_HalftoneMaterial = halftoneMat;
            requiresIntermediateTexture = true; //TODO: Needs to read current color texture of the scene?
        }

        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            var resourceData = frameData.Get<UniversalResourceData>();
            if (resourceData.isActiveTargetBackBuffer)
            {
                Debug.LogError($"Skipping render pass. Needs an intermediate ColorTexture, cannot use BackBuffer as " +
                    $"a texture input."); //TODO: is this needed?
                return;
            }
            //Texture to use in blit operation
            var source = resourceData.activeColorTexture;

            //Render texture properties in render graph description struct
            var destinationDesc = renderGraph.GetTextureDesc(source);
            //Modify, not start from new!
            destinationDesc.clearBuffer = false;

            //Name of destination texture should match pass
            destinationDesc.name = $"CameraColor-{m_HalftonePassName}";
            //Destination texture handle with set up parameters
            TextureHandle destination = renderGraph.CreateTexture(destinationDesc);

            //Have source and destination, material and default Shader pass 0 for blit operation
            //Helper struct, for outline blit operation
            RenderGraphUtils.BlitMaterialParameters halftoneBlit = new(source, destination,
                m_HalftoneMaterial, 0);
            renderGraph.AddBlitPass(halftoneBlit, passName: m_HalftonePassName);

            resourceData.cameraColor = destination;
        }


    }

    //TODO: add 

    public RenderPassEvent outlineInjectionPoint = RenderPassEvent.BeforeRenderingPostProcessing;
    public RenderPassEvent halftoneInjectionPoint = RenderPassEvent.AfterRenderingPostProcessing;
    public Material outlineMaterial;
    public Material halftoneMaterial;

    OutlineEffectPass m_ScriptableOutlinePass;
    HalftoneEffectPass m_ScriptableHalftonePass;

    /// <inheritdoc/>
    public override void Create()
    {
        m_ScriptableOutlinePass = new OutlineEffectPass();

        // Configures where the render pass should be injected.
        m_ScriptableOutlinePass.renderPassEvent = outlineInjectionPoint;

        m_ScriptableHalftonePass = new HalftoneEffectPass();

        // Configures where the render pass should be injected.
        m_ScriptableHalftonePass.renderPassEvent = halftoneInjectionPoint;

    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if(outlineMaterial == null || halftoneMaterial == null)
        {
            Debug.LogWarning($"Outline and halftone Materials must be set. Skipping");
            return;
        }

        m_ScriptableOutlinePass.SetUp(outlineMaterial);
        renderer.EnqueuePass(m_ScriptableOutlinePass);

        m_ScriptableHalftonePass.SetUp(halftoneMaterial);
        renderer.EnqueuePass(m_ScriptableHalftonePass);
    }
}
