using UnityEngine;
using UnityEngine.VR;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using LlockhamIndustries.ExtensionMethods;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LlockhamIndustries.Decals
{
    /**
    * \mainpage Welcome to the documentation
    * To keep things from being too overwhelming I've only documented what's necessary to script with the system.
    * For more advanced users, everything in the system is commented and built to be expanded on. Dig through the code to your hearts content.
    * If you have any questions or get stuck at any stage I'm always available at Support@LlockhamIndustries.com.
    */

    /**
    * The core class of the system, responsible for the majority of the systems functionality. 
    * For scripting purposes, it's almost entirely a black box, you should rarely need to access or modify anything within it.
    * It's well stuctured and commented all the same though, so if your interested, open it up and have a look around.
    */
    [ExecuteInEditMode]
    public class DynamicDecals : MonoBehaviour
    {
        //MultiScene Editor Singleton
        public static bool Initialized
        {
            get { return system != null; }
        }
        public static DynamicDecals System
        {
            get
            {
                if (system == null)
                {
                    //Create our system
                    GameObject go = new GameObject("Dynamic Decals");
                    go.hideFlags = HideFlags.DontSaveInEditor | HideFlags.DontSaveInBuild | HideFlags.HideInHierarchy | HideFlags.HideInInspector;
                    go.AddComponent<DynamicDecals>();
                }
                return system;
            }
        }
        private static DynamicDecals system;

        private void OnEnable()
        {
            //Singleton
            if (system == null) system = this;
            else if (system != this)
            {
                if (Application.isPlaying) Destroy(gameObject);
                else DestroyImmediate(gameObject, true);
                return;
            }

            //Initialize the system
            Initialize();
        }
        private void OnDisable()
        {
            //Terminate the system
            Terminate();
        }
        private void Start()
        {
            if (Application.isPlaying) DontDestroyOnLoad(gameObject);
        }

        #if UNITY_EDITOR
        private void OnApplicationQuit()
        {
            //Reset the system when transitioning back to edit mode
            Terminate();
            Initialize();
        }
        #endif

        //Settings
        public DynamicDecalSettings Settings
        {
            get
            {
                //Try load our settings
                if (settings == null) settings = Resources.Load<DynamicDecalSettings>("Settings");

                //If not found create them
                if (settings == null) settings = ScriptableObject.CreateInstance<DynamicDecalSettings>();
                return settings;
            }
        }
        private DynamicDecalSettings settings;

        #region Rendering
        //Rendering Path
        public SystemPath SystemPath
        {
            get { return renderingPath; }
        }
        public SystemPath renderingPath;

        //Texture formats;
        private RenderTextureFormat depthFormat;
        private RenderTextureFormat normalFormat;
        private RenderTextureFormat maskFormat;

        //Instanced
        public bool Instanced
        {
            get { return SystemInfo.supportsInstancing; }
        }

        //Methods
        private void UpdateSystemPath()
        {
            //Get our primary camera
            Camera target = null;
            if (Camera.main != null) target = Camera.main;
            else if (Camera.current != null) target = Camera.current;

            if (target != null)
            {
                //Determine our rendering method
                if (target.actualRenderingPath == RenderingPath.Forward || target.actualRenderingPath == RenderingPath.DeferredShading)
                {
                    SystemPath newPath = SystemPath.Forward;
                    if (target.actualRenderingPath == RenderingPath.DeferredShading) newPath = SystemPath.Deferred;

                    if (renderingPath != newPath)
                    {
                        renderingPath = newPath;
                        UpdateRenderers();
                    }
                }
                else Debug.LogWarning("Current Rendering Path not supported! Please use either Forward or Deferred");
            }
        }
        public void RestoreDepthTextureModes()
        {
            //Iterate over every camera and restore it to it's original depth texture mode
            for (int i = 0; i < cameraData.Count; i++)
            {
                Camera camera = cameraData.ElementAt(i).Key;
                if (camera != null) cameraData.ElementAt(i).Value.RestoreDepthTextureMode(camera);
            }
        }
        #endregion
        #region Projections
        //Projections
        private List<ProjectionData> Projections;
        private ProjectionData GetProjectionData(Projection Projection)
        {
            for (int i = 0; i < Projections.Count; i++)
            {
                if (Projections[i].projection == Projection)
                {
                    return Projections[i];
                }
            }
            return null;
        }
        private void UpdateProjectionData()
        {
            for (int i = 0; i < Projections.Count; i++)
            {
                Projections[i].Update();
            }
        }

        //Registration
        public bool Register(ProjectionRenderer Instance)
        {
            if (Instance != null)
            {
                //Determine our projection
                Projection projection = Instance.Projection;

                //Check if our projection has been registered
                ProjectionData data = GetProjectionData(projection);
                if (data != null)
                {
                    data.Add(Instance);
                    return true;
                }
                else
                {
                    //Create and register our projection data
                    data = new ProjectionData(projection);
                    data.Add(Instance);

                    //If we are lower priority than a projection in the list, insert ourself before them
                    for (int i = 0; i < Projections.Count; i++)
                    {
                        if (projection.Priority < Projections[i].projection.Priority)
                        {
                            Projections.Insert(i, data);
                            return true;
                        }
                    }

                    //If we are higher priority than everything in the list, just add ourself
                    Projections.Add(data);
                    return true;
                }
            }
            return false;
        }
        public void Deregister(ProjectionRenderer Instance)
        {
            if (Instance != null)
            {
                //Determine our projection
                Projection projection = Instance.Projection;

                //Check if our projection has been registered
                for (int i = 0; i < Projections.Count; i++)
                {
                    if (Projections[i].projection == projection)
                    {
                        //Remove instance from projection
                        Projections[i].Remove(Instance);

                        //Remove empty projections
                        if (Projections[i].instances.Count == 0)
                        {
                            Projections.RemoveAt(i);
                        }
                        return;
                    }
                }
            }
        }
        public void Reorder(Projection Projection)
        {
            ProjectionData data = GetProjectionData(Projection);
            if (data != null)
            {
                //Remove ourself from the list
                Projections.Remove(data);

                //Insert ourself back in the correct position
                for (int i = 0; i < Projections.Count; i++)
                {
                    if (Projection.Priority < Projections[i].projection.Priority)
                    {
                        Projections.Insert(i, data);
                        return;
                    }
                }

                //If no correct position found add ourself to the end
                Projections.Add(data);

                //Reorder renderers
                OrderRenderers();
            }
        }

        //Order renderers
        public void OrderRenderers()
        {
            if (renderersMarked && Projections != null)
            {
                int i = 1;
                foreach (ProjectionData projection in Projections)
                {
                    projection.AssertOrder(ref i);
                }
            }
        }
        public void MarkRenderers()
        {
            renderersMarked = true;
        }
        private bool renderersMarked;

        //Update all renderers
        public void UpdateRenderers()
        {
            if (Projections != null)
            {
                for (int i = 0; i < Projections.Count; i++)
                {
                    Projections[i].UpdateRenderers();
                }
            }
        }
        public void UpdateRenderers(Projection Projection)
        {
            if (Projections != null)
            {
                for (int i = 0; i < Projections.Count; i++)
                {
                    if (Projections[i].projection == Projection)
                    {
                        Projections[i].UpdateRenderers();
                        return;
                    }
                }
            }
        }

        //Debug
        public int ProjectionCount
        {
            get { return Projections.Count; }
        }
        public int RendererCount
        {
            get
            {
                int count = 0;
                for (int i = 0; i < Projections.Count; i++) count += Projections[i].instances.Count;
                return count;
            }
        }
        #endregion
        #region Meshes & Shaders
        public Mesh Cube
        {
            get
            {
                if (cube == null)
                {
                    cube = Resources.Load<Mesh>("Decal");
                    cube.name = "Projection";
                }
                return cube;
            }
        }

        public Shader DepthShader
        {
            get
            {
                if (depthShader == null)
                {
                    depthShader = Shader.Find("Projection/Internal/Depth");
                }
                return depthShader;
            }

        }
        public Shader NormalShader
        {
            get
            {
                if (normalShader == null)
                {
                    normalShader = Shader.Find("Projection/Internal/Normal");
                }
                return normalShader;
            }
        }
        public Shader MaskShader
        {
            get
            {
                if (maskShader == null)
                {
                    maskShader = Shader.Find("Projection/Internal/Mask");
                }
                return maskShader;
            }
        }
        
        public Shader DepthNormalShader
        {
            get
            {
                if (depthNormalShader == null)
                {
                    depthNormalShader = Shader.Find("Projection/Internal/DepthNormal");
                }
                return depthNormalShader;
            }
        }
        public Shader NormalMaskShader
        {
            get
            {
                if (normalMaskShader == null)
                {
                    normalMaskShader = Shader.Find("Projection/Internal/NormalMask");
                }
                return normalMaskShader;
            }
        }

        public Shader DepthNormalMaskShader
        {
            get
            {
                if (depthNormalMaskShader == null)
                {
                    depthNormalMaskShader = Shader.Find("Projection/Internal/DepthNormalMask");
                }
                return depthNormalMaskShader;
            }
        }

        public Material StereoBlitLeft
        {
            get
            {
                if (stereoBlitLeft == null)
                {
                    stereoBlitLeft = new Material(Shader.Find("Projection/Internal/StereoBlitLeft"));
                }
                return stereoBlitLeft;
            }
        }
        public Material StereoBlitRight
        {
            get
            {
                if (stereoBlitRight == null)
                {
                    stereoBlitRight = new Material(Shader.Find("Projection/Internal/StereoBlitRight"));
                }
                return stereoBlitRight;
            }
        }
        public Material StereoDepthBlitLeft
        {
            get
            {
                if (stereoDepthBlitLeft == null)
                {
                    stereoDepthBlitLeft = new Material(Shader.Find("Projection/Internal/StereoDepthBlitLeft"));
                }
                return stereoDepthBlitLeft;
            }
        }
        public Material StereoDepthBlitRight
        {
            get
            {
                if (stereoDepthBlitRight == null)
                {
                    stereoDepthBlitRight = new Material(Shader.Find("Projection/Internal/StereoDepthBlitRight"));
                }
                return stereoDepthBlitRight;
            }
        }

        //Backing Fields
        private Mesh cube;

        private Shader depthShader;
        private Shader normalShader;
        private Shader maskShader;

        private Shader depthNormalShader;
        private Shader normalMaskShader;

        private Shader depthNormalMaskShader;

        private Material stereoBlitLeft;
        private Material stereoBlitRight;
        private Material stereoDepthBlitLeft;
        private Material stereoDepthBlitRight;
        #endregion
        #region Cameras
        //Camera data
        internal Dictionary<Camera, CameraData> cameraData = new Dictionary<Camera, CameraData>();
        internal CameraData GetData(Camera Camera)
        {
            //Declare our Camera Data
            CameraData data = null;

            //Check if this camera already has camera data
            if (!cameraData.TryGetValue(Camera, out data))
            {
                //Generate data
                data = new CameraData(Camera);

                //Store data
                cameraData[Camera] = data;
            }

            //Initialize if required
            if (data != null)
            {
                if (!data.initialized && Camera.GetComponent<ProjectionBlocker>() == null) data.Initialize(Camera, this);
                else if (data.initialized && Camera.GetComponent<ProjectionBlocker>() != null) data.Terminate(Camera);
            }


            //Return our updated Camera Data
            return data;
        }

        //Default cameraRect
        public Rect FullRect = new Rect(0, 0, 1, 1);

        //Shader replacement
        public Camera CustomCamera
        {
            get
            {
                if (customCamera == null)
                {
                    GameObject cameraObject = new GameObject("Custom Camera");
                    customCamera = cameraObject.AddComponent<Camera>();
                    cameraObject.AddComponent<ProjectionBlocker>();
                    cameraObject.hideFlags = HideFlags.DontSaveInEditor | HideFlags.DontSaveInBuild | HideFlags.HideInHierarchy | HideFlags.HideInInspector;
                    cameraObject.SetActive(false);

                    if (Application.isPlaying) GameObject.DontDestroyOnLoad(cameraObject);
                }
                return customCamera;
            }
        }
        private Camera customCamera;

        public RenderTexture depthBuffer;
        public RenderTexture normalBuffer;
        public RenderTexture maskBuffer;
        #endregion
        #region Pools
        private Dictionary<int, ProjectionPool> Pools;
        internal ProjectionPool PoolFromInstance(PoolInstance Instance)
        {
            //Make sure we are initialized
            if (Pools == null) Pools = new Dictionary<int, ProjectionPool>();

            ProjectionPool pool;
            if (!Pools.TryGetValue(Instance.id, out pool))
            {
                pool = new ProjectionPool(Instance);
                Pools.Add(Instance.id, pool);
            }
            return pool;
        }

        public ProjectionPool DefaultPool
        {
            get { return PoolFromInstance(Settings.pools[0]); }
        }

        /**
         * Returns a pool with the specified name, if it exists. If it doesn't, returns the default pool.
         * @param Title The title of the pool to be returned.
         */
        public ProjectionPool GetPool(string Title)
        {
            //Check Settings for an ID
            for (int i = 0; i < Settings.pools.Length; i++)
            {
                if (settings.pools[i].title == Title)
                {
                    return PoolFromInstance(settings.pools[i]);
                }
            }
            //No valid pool set up, log a Warning and return the default pool
            Debug.LogWarning("No valid pool with the title : " + Title + " found. Returning default pool");
            return PoolFromInstance(settings.pools[0]);
        }
        /**
         * Returns a pool with the specified ID, if it exists. If it doesn't, returns the default pool.
         * @param ID The ID of the pool to be returned.
         */
        public ProjectionPool GetPool(int ID)
        {
            //Check Settings for an ID
            for (int i = 0; i < Settings.pools.Length; i++)
            {
                if (settings.pools[i].id == ID)
                {
                    return PoolFromInstance(settings.pools[i]);
                }
            }
            //No valid pool set up, log a Warning and return the default pool
            Debug.LogWarning("No valid pool with the ID : " + ID + " found. Returning default pool");
            return PoolFromInstance(settings.pools[0]);
        }
        #endregion

        //Initialize / Terminate
        private void Initialize()
        {
            #if UNITY_EDITOR
            Settings.CalculateVR();

            SceneView.onSceneGUIDelegate += OnSceneGUI;
            Undo.undoRedoPerformed += UndoRedo;
            #endif

            //Determine texture formats
            depthFormat = RenderTextureFormat.Depth;
            normalFormat = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGB2101010) ? RenderTextureFormat.ARGB2101010 : RenderTextureFormat.ARGB32;
            maskFormat = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGB32) ? RenderTextureFormat.ARGB32 : RenderTextureFormat.ARGB32;

            //Register our projection events to all cameras
            Camera.onPreRender += PreRender;
            Camera.onPostRender += PostRender;

            //Initialize projections
            if (Projections == null) Projections = new List<ProjectionData>();
            else
            {
                for(int i = 0; i < Projections.Count; i++)
                {
                    Projections[i].EnableRenderers();
                }
            }
        }
        private void Terminate()
        {
            #if UNITY_EDITOR
            SceneView.onSceneGUIDelegate -= OnSceneGUI;
            Undo.undoRedoPerformed -= UndoRedo;
            #endif

            //Deregister our projection events
            Camera.onPreRender -= PreRender;
            Camera.onPostRender -= PostRender;

            //Iterate over our camera data
            foreach (var cb in cameraData)
            {
                //Terminate camera data
                cb.Value.Terminate(cb.Key);
            }

            //Clear camera Data
            cameraData.Clear();

            //Disable our projections
            if (Projections != null)
            {
                for (int i = 0; i < Projections.Count; i++)
                {
                    Projections[i].DisableRenderers();
                }
            }
        }

        //Primary Methods
        private void LateUpdate()
        {
            #if UNITY_EDITOR
            //In editor settings subject to change. Update constantly
            settings = Resources.Load<DynamicDecalSettings>("Settings");
            #endif

            //Check our system path
            UpdateSystemPath();

            //Update projections
            UpdateProjectionData();

            //Order renderers
            OrderRenderers();
        }
        private void PreRender(Camera Camera)
        {
            //Grab our camera data
            CameraData data = GetData(Camera);

            //Only run on initialized cameras
            if (data.initialized && (data.sceneCamera || data.previewCamera || Camera.isActiveAndEnabled))
            {
                //Update to the correct rendering path
                data.UpdateRenderingMethod(Camera, this);

                //Shader replacement
                PreShaderReplacement(Camera, data);
            }
        }
        private void PostRender(Camera Camera)
        {
            //Grab our camera data
            CameraData data = GetData(Camera);

            //Only run on initialized cameras
            if (data.initialized && (data.sceneCamera || data.previewCamera || Camera.isActiveAndEnabled))
            {
                PostShaderReplacementStart(Camera, data);
            }
        }

        //Shader Replacement
        public bool shaderReplacement = true;
        private void PreShaderReplacement(Camera Camera, CameraData Data)
        {
            if (Projections.Count > 0 && shaderReplacement)
            {
                //Grab a reference to custom camera
                Camera customCamera = CustomCamera;

                //Set up camera
                customCamera.CopyFrom(Camera);
                customCamera.renderingPath = RenderingPath.Forward;
                customCamera.depthTextureMode = DepthTextureMode.None;
                customCamera.useOcclusionCulling = false;
                customCamera.allowMSAA = false;
                customCamera.allowHDR = false;
                customCamera.rect = FullRect;

                //Get render texture/s
                GetTextures(Camera, customCamera, Data.replacement);

                //Render into textures
                if (UnityEngine.XR.XRSettings.enabled && Camera.stereoTargetEye != StereoTargetEyeMask.None && Settings.SinglePassVR)
                {
                    //Create render textures for eyes
                    RenderTexture eyeDepth = (Camera.actualRenderingPath == RenderingPath.DeferredShading)? RenderTexture.GetTemporary(UnityEngine.XR.XRSettings.eyeTextureWidth, UnityEngine.XR.XRSettings.eyeTextureHeight, 24, depthFormat) : null;
                    RenderTexture eyeNormals = RenderTexture.GetTemporary(UnityEngine.XR.XRSettings.eyeTextureWidth, UnityEngine.XR.XRSettings.eyeTextureHeight, 24, normalFormat);
                    RenderTexture eyeMask = RenderTexture.GetTemporary(UnityEngine.XR.XRSettings.eyeTextureWidth, UnityEngine.XR.XRSettings.eyeTextureHeight, 24, maskFormat);

                    //Left eye
                    if (Camera.stereoTargetEye == StereoTargetEyeMask.Both || Camera.stereoTargetEye == StereoTargetEyeMask.Left)
                    {
                        //Position & rotation
                        customCamera.transform.position = Camera.transform.parent.TransformPoint(UnityEngine.XR.InputTracking.GetLocalPosition(UnityEngine.XR.XRNode.LeftEye));
                        customCamera.transform.rotation = Camera.transform.rotation * UnityEngine.XR.InputTracking.GetLocalRotation(UnityEngine.XR.XRNode.LeftEye);

                        //Projection matrix
                        customCamera.projectionMatrix = Camera.GetStereoProjectionMatrix(Camera.StereoscopicEye.Left);

                        //View matrix
                        Matrix4x4 worldToCamera = Camera.worldToCameraMatrix;
                        customCamera.worldToCameraMatrix = worldToCamera;

                        //Render
                        RenderToTextures(Camera, customCamera, Data.replacement, eyeDepth, eyeNormals, eyeMask, Camera.cullingMask);

                        //Blit into buffer
                        if (Camera.actualRenderingPath == RenderingPath.DeferredShading)
                        {
                            StereoDepthBlitLeft.SetTexture("_DepthTex", eyeDepth);
                            Graphics.Blit(eyeDepth, depthBuffer, StereoDepthBlitLeft);
                        }
                        Graphics.Blit(eyeNormals, normalBuffer, StereoBlitLeft);
                        Graphics.Blit(eyeMask, maskBuffer, StereoBlitLeft);
                    }

                    //Right eye
                    if (Camera.stereoTargetEye == StereoTargetEyeMask.Both || Camera.stereoTargetEye == StereoTargetEyeMask.Right)
                    {
                        //Position & rotation
                        customCamera.transform.position = Camera.transform.parent.TransformPoint(UnityEngine.XR.InputTracking.GetLocalPosition(UnityEngine.XR.XRNode.RightEye));
                        customCamera.transform.rotation = Camera.transform.rotation * UnityEngine.XR.InputTracking.GetLocalRotation(UnityEngine.XR.XRNode.RightEye);

                        //Projection matrix
                        customCamera.projectionMatrix = Camera.GetStereoProjectionMatrix(Camera.StereoscopicEye.Right);

                        //View matrix
                        Matrix4x4 worldToCamera = Camera.worldToCameraMatrix;
                        worldToCamera.m03 -= Camera.stereoSeparation;
                        customCamera.worldToCameraMatrix = worldToCamera;

                        //Render
                        RenderToTextures(Camera, customCamera, Data.replacement, eyeDepth, eyeNormals, eyeMask, Camera.cullingMask);

                        //Blit into buffer
                        if (Camera.actualRenderingPath == RenderingPath.DeferredShading)
                        {
                            StereoDepthBlitRight.SetTexture("_DepthTex", eyeDepth);
                            Graphics.Blit(eyeDepth, depthBuffer, StereoDepthBlitRight);
                        }
                        Graphics.Blit(eyeNormals, normalBuffer, StereoBlitRight);
                        Graphics.Blit(eyeMask, maskBuffer, StereoBlitRight);
                    }

                    //Release eye render textures
                    if (Camera.actualRenderingPath == RenderingPath.DeferredShading) eyeDepth.Release();
                    eyeNormals.Release();
                    eyeMask.Release();
                }
                else RenderToTextures(Camera, customCamera, Data.replacement, depthBuffer, normalBuffer, maskBuffer, Camera.cullingMask);

                //Set completed render texture/s as global shader buffers
                SetAsBuffers(Camera, Data.replacement);
            }
        }
        private void PostShaderReplacementStart(Camera Camera, CameraData Data)
        {
            if (Projections.Count > 0 && shaderReplacement)
            {
                //Release render texture/s
                ReleaseTextures(Data.replacement);
            }
        }

        private void RenderToTextures(Camera OriginalCamera, Camera CustomCamera, ShaderReplacement Replacement, RenderTexture depth, RenderTexture normal, RenderTexture mask, LayerMask CullingMask)
        {
            switch (Replacement)
            {
                case ShaderReplacement.Classic:
                    //Render to mask buffer
                    CustomCamera.targetTexture = mask;
                    DrawMaskPasses(CustomCamera, MaskShader, CullingMask);

                    //Tell shaders not to use custom or precision depth/normals
                    Shader.DisableKeyword("_PrecisionDepthNormals");
                    Shader.DisableKeyword("_CustomDepthNormals");
                    break;

                case ShaderReplacement.TriplePass:
                    //Set culling layers
                    CustomCamera.cullingMask = CullingMask;

                    //Render to depth buffer
                    if (OriginalCamera.actualRenderingPath == RenderingPath.DeferredShading)
                    {
                        CustomCamera.targetTexture = depth;
                        DrawRegualarPass(CustomCamera, DepthShader);
                    }

                    //Render to normal buffer
                    CustomCamera.targetTexture = normal;
                    DrawRegualarPass(CustomCamera, NormalShader);

                    //Render to mask buffer
                    CustomCamera.targetTexture = mask;
                    DrawMaskPasses(CustomCamera, MaskShader, CullingMask);

                    if (OriginalCamera.actualRenderingPath == RenderingPath.DeferredShading)
                    {
                        //Tell shaders to use custom depth/normals
                        Shader.DisableKeyword("_PrecisionDepthNormals");
                        Shader.EnableKeyword("_CustomDepthNormals");
                    }
                    else
                    {
                        //Tell shaders to use precision depth/normals
                        Shader.EnableKeyword("_PrecisionDepthNormals");
                        Shader.DisableKeyword("_CustomDepthNormals");
                    }
                    break;

                case ShaderReplacement.DoublePass:

                    //Render to depth buffer
                    if (OriginalCamera.actualRenderingPath == RenderingPath.DeferredShading)
                    {
                        CustomCamera.cullingMask = CullingMask;
                        CustomCamera.targetTexture = depth;
                        DrawRegualarPass(CustomCamera, DepthShader);
                    }

                    //Render to normal and mask buffers at once
                    RenderBuffer[] doublePassBuffers = new RenderBuffer[] { normal.colorBuffer, mask.colorBuffer };
                    CustomCamera.SetTargetBuffers(doublePassBuffers, normal.depthBuffer);
                    DrawMaskPasses(CustomCamera, NormalMaskShader, CullingMask);

                    if (OriginalCamera.actualRenderingPath == RenderingPath.DeferredShading)
                    {
                        //Tell shaders to use custom depth/normals
                        Shader.DisableKeyword("_PrecisionDepthNormals");
                        Shader.EnableKeyword("_CustomDepthNormals");
                    }
                    else
                    {
                        //Tell shaders to use precision depth/normals
                        Shader.EnableKeyword("_PrecisionDepthNormals");
                        Shader.DisableKeyword("_CustomDepthNormals");
                    }
                    break;

                case ShaderReplacement.SinglePass:
                    //Render to depth, normal and mask buffers at once
                    RenderBuffer[] singlePassBuffers = new RenderBuffer[] { depth.colorBuffer, normal.colorBuffer, mask.colorBuffer };
                    CustomCamera.SetTargetBuffers(singlePassBuffers, depth.depthBuffer);
                    DrawMaskPasses(CustomCamera, DepthNormalMaskShader, CullingMask);

                    //Tell shaders to use custom depth/normals
                    Shader.DisableKeyword("_PrecisionDepthNormals");
                    Shader.EnableKeyword("_CustomDepthNormals");
                    break;
            }

            //Tell camera to stop rendering to render texture/s
            CustomCamera.targetTexture = null;
        }
        private void DrawRegualarPass(Camera CustomCamera, Shader ReplacementShader)
        {
            //Clear before drawing
            CustomCamera.clearFlags = CameraClearFlags.SolidColor;
            CustomCamera.backgroundColor = Color.clear;

            //Render into temporary render texture
            CustomCamera.RenderWithShader(ReplacementShader, "RenderType");
        }
        private void DrawMaskPasses(Camera CustomCamera, Shader ReplacementShader, LayerMask CullingMask, bool RenderInvalid = true)
        {
            //Grab mask passes
            List<ReplacementPass> passes = Settings.Passes;

            //Clear on the first pass
            CustomCamera.clearFlags = CameraClearFlags.SolidColor;
            CustomCamera.backgroundColor = Color.clear;

            //Render mask passes
            for (int i = 0; i < passes.Count; i++)
            {
                if (passes[i].vector != Vector4.zero || RenderInvalid)
                {
                    //Set culling mask
                    CustomCamera.cullingMask = passes[i].layers & CullingMask;

                    //Set mask vector
                    Shader.SetGlobalVector("_MaskWrite", passes[i].vector);

                    //Render into temporary render texture/s
                    CustomCamera.RenderWithShader(ReplacementShader, "RenderType");

                    //Only clear on first render
                    CustomCamera.clearFlags = CameraClearFlags.Nothing;
                }
            }
        }

        private void GetTextures(Camera OriginalCamera, Camera CustomCamera, ShaderReplacement Replacement)
        {
            //Calculate width and height
            int width = OriginalCamera.pixelWidth;
            int height = OriginalCamera.pixelHeight;

            if (UnityEngine.XR.XRSettings.enabled && OriginalCamera.stereoTargetEye != StereoTargetEyeMask.None)
            {
                width = (Settings.SinglePassVR) ? UnityEngine.XR.XRSettings.eyeTextureWidth * 2 : UnityEngine.XR.XRSettings.eyeTextureWidth;
                height = UnityEngine.XR.XRSettings.eyeTextureHeight;
            }

            //Get depth & normal buffers
            if (Replacement == ShaderReplacement.SinglePass)
            {
                depthBuffer = RenderTexture.GetTemporary(width, height, 24, depthFormat);
                normalBuffer = RenderTexture.GetTemporary(width, height, 0, normalFormat);
            }

            //Get normal buffer
            if (Replacement == ShaderReplacement.DoublePass || Replacement == ShaderReplacement.TriplePass)
            {
                if (OriginalCamera.actualRenderingPath == RenderingPath.DeferredShading)
                {
                    depthBuffer = RenderTexture.GetTemporary(width, height, 24, depthFormat);
                }
                normalBuffer = RenderTexture.GetTemporary(width, height, 24, normalFormat);
            }

            //Get mask buffer
            maskBuffer = RenderTexture.GetTemporary(width, height, (Replacement == ShaderReplacement.TriplePass || Replacement == ShaderReplacement.Classic) ? 24 : 0, maskFormat);
        }
        private void SetAsBuffers(Camera OriginalCamera, ShaderReplacement Replacement)
        {
            //Depth-Normal
            if (Replacement == ShaderReplacement.SinglePass)
            {
                depthBuffer.SetGlobalShaderProperty("_CustomDepthTexture");
                normalBuffer.SetGlobalShaderProperty("_CustomNormalTexture");
            }

            //Normal
            if (Replacement == ShaderReplacement.TriplePass || Replacement == ShaderReplacement.DoublePass)
            {
                if (OriginalCamera.actualRenderingPath == RenderingPath.DeferredShading) depthBuffer.SetGlobalShaderProperty("_CustomDepthTexture");
                normalBuffer.SetGlobalShaderProperty("_CustomNormalTexture");
            }

            //Mask buffers
            maskBuffer.SetGlobalShaderProperty("_MaskBuffer_0");
        }
        private void ReleaseTextures(ShaderReplacement Replacement)
        {
            if (depthBuffer != null && depthBuffer.IsCreated()) RenderTexture.ReleaseTemporary(depthBuffer);
            if (normalBuffer != null && normalBuffer.IsCreated()) RenderTexture.ReleaseTemporary(normalBuffer);
            RenderTexture.ReleaseTemporary(maskBuffer);
        }

        //Editor Scene Placement
        #if UNITY_EDITOR
        private List<GameObject> dragables = new List<GameObject>();
        private void OnSceneGUI(SceneView sceneView)
        {
            //Drag Update
            if (Event.current.type == EventType.DragUpdated)
            {
                //Check for projection renderers among selection
                if (dragables.Count == 0)
                {
                    foreach (UnityEngine.Object o in DragAndDrop.objectReferences)
                    {
                        GameObject go = o as GameObject;
                        if (go != null)
                        {
                            if (go.GetComponent<ProjectionRenderer>() != null)
                            {
                                //Create our dragable
                                GameObject dragable = PrefabUtility.InstantiatePrefab(go) as GameObject;
                                dragable.name = go.name;
                                dragable.hideFlags = HideFlags.HideInHierarchy;

                                //Register to list
                                dragables.Add(dragable);
                            }
                        }
                    }
                }

                //Position dragables
                if (dragables.Count > 0)
                {
                    RaycastHit hit;
                    Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

                    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    {
                        foreach (GameObject go in dragables)
                        {
                            go.transform.position = hit.point;
                            go.transform.rotation = Quaternion.LookRotation(-hit.normal);
                        }
                    }
                    else
                    {
                        foreach (GameObject go in dragables)
                        {
                            go.transform.position = Vector3.zero;
                            go.transform.rotation = Quaternion.LookRotation(-Vector3.up);
                        }
                    }

                    //Set mode
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                    //Use event
                    Event.current.Use();
                }
            }

            //Drag Perform
            if (Event.current.type == EventType.DragPerform && dragables.Count > 0)
            {
                //Show dragables
                foreach (GameObject go in dragables)
                {
                    //Show objects in heirarchy
                    go.hideFlags = HideFlags.None;

                    //Register Undo
                    Undo.RegisterCreatedObjectUndo(go, "Instantiate Prefab");
                }

                //No longer require dragables
                dragables.Clear();

                //Use event
                Event.current.Use();
            }

            //Drag Exit
            if (Event.current.type == EventType.DragExited && dragables.Count > 0)
            {
                //Destroy all dragables
                foreach (GameObject go in dragables) DestroyImmediate(go);
                dragables.Clear();

                //Use event
                Event.current.Use();
            }
        }
        private void UndoRedo()
        {
            dragables.Clear();
        }
        #endif
    }

    //System path
    public enum SystemPath { Forward, Deferred };

    //Camera management
    internal class CameraData
    {
        //Camera type
        public bool sceneCamera;
        public bool previewCamera;

        //Depth texture mode
        public DepthTextureMode? originalDTM, desiredDTM = null;

        //Shader replacement
        public ShaderReplacement replacement;

        //Constructor
        public CameraData(Camera Camera)
        {
            sceneCamera = (Camera.name == "SceneCamera");
            previewCamera = (Camera.name == "Preview Camera");
        }

        //Camera state
        public bool initialized;
        public void Initialize(Camera Camera, DynamicDecals System)
        {
            //Register it to our rendering path
            InitializeRenderingMethod(Camera);

            //Enable
            initialized = true;
        }
        public void Terminate(Camera Camera)
        {
            //Restore cameras depthTexture mode
            RestoreDepthTextureMode(Camera);

            //Disable
            initialized = false;
        }

        public void InitializeRenderingMethod(Camera Camera)
        {
            //Scenes cameras always render in high-precision
            if (replacement != ShaderReplacement.SinglePass && (sceneCamera || previewCamera)) replacement = ShaderReplacement.SinglePass;

            switch (replacement)
            {
                case ShaderReplacement.Classic:
                    //Low precision depth & normals
                    desiredDTM = DepthTextureMode.DepthNormals;
                    SetDepthTextureMode(Camera);
                    break;

                case ShaderReplacement.TriplePass:
                case ShaderReplacement.DoublePass:

                    if (Camera.actualRenderingPath == RenderingPath.DeferredShading)
                    {
                        //Use custom depth, Unity's depth renders too late
                        RestoreDepthTextureMode(Camera);
                    }
                    else
                    {
                        //High precision depth
                        desiredDTM = DepthTextureMode.Depth;
                        SetDepthTextureMode(Camera);
                    }
                    
                    break;

                case ShaderReplacement.SinglePass:
                    //Restore our depth texture mode
                    RestoreDepthTextureMode(Camera);
                    break;
            }
        }
        public void UpdateRenderingMethod(Camera Camera, DynamicDecals System)
        {
            //Use standard shader replacement by default
            ShaderReplacement shaderReplacement = ShaderReplacement.SinglePass;

            //If we can't render to at least 3 render textures perform shade replacement in 2 passes
            if (SystemInfo.supportedRenderTargetCount < 3) shaderReplacement = ShaderReplacement.DoublePass;

            //Android / Mac bug with single pass replacement
            #if (UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX || UNITY_ANDROID)
            shaderReplacement = ShaderReplacement.DoublePass;
            #endif

            //If rendering a VR eye, use VR replacement
            if (UnityEngine.XR.XRSettings.enabled && Camera.stereoTargetEye != StereoTargetEyeMask.None) shaderReplacement = ShaderReplacement.TriplePass;

            //If on old device, use mobile replacement
            if (SystemInfo.graphicsShaderLevel <= 20 || SystemInfo.supportedRenderTargetCount < 2) shaderReplacement = ShaderReplacement.Classic;

            //Determine our rendering method
            if (replacement != shaderReplacement)
            {
                //Update our rendering method
                replacement = shaderReplacement;

                //Add ourself to the new rendering method
                InitializeRenderingMethod(Camera);
            }
        }

        public void SetDepthTextureMode(Camera Camera)
        {
            //If we have a desired value change to it.
            if (desiredDTM.HasValue)
            {
                if (Camera.depthTextureMode != desiredDTM)
                {
                    //If we haven't already, Cache the original depth texture mode, otherwise revert to it.
                    if (!originalDTM.HasValue) originalDTM = Camera.depthTextureMode;
                    else Camera.depthTextureMode = originalDTM.Value;

                    //Add our desired depth texture mode.
                    Camera.depthTextureMode |= desiredDTM.Value;
                }
            }
            //If we have no desired value, switch back to the original value.
            else RestoreDepthTextureMode(Camera);
        }
        public void RestoreDepthTextureMode(Camera Camera)
        {
            //Restore the depth texture mode to the cached
            if (originalDTM.HasValue && Camera != null)
            {
                Camera.depthTextureMode = originalDTM.Value;
            }
        }
    }

    //Projection management
    public class ProjectionData
    {
        //Projection
        public Projection projection;
        public void Update()
        {
            projection.Update();
        }

        //Instances
        public List<ProjectionRenderer> instances;

        public void Add(ProjectionRenderer Instance)
        {
            //If the instance isn't already in the list, add it
            if (!instances.Contains(Instance))
            {
                //Add our instance
                instances.Add(Instance);

                //Notify instance of data change
                Instance.Data = this;

                //Mark renderers to change
                DynamicDecals.System.MarkRenderers();
            }

        }
        public void Remove(ProjectionRenderer Instance)
        {
            //Attempt to remove the instance from the list
            instances.Remove(Instance);

            //Notify instance of data change
            if (Instance.Data == this) Instance.Data = null;
        }

        public void MoveToTop(ProjectionRenderer Instance)
        {
            //Remove the instance from the list
            instances.Remove(Instance);

            //Add instance to top of list
            instances.Add(Instance);

            //Mark renderers to change
            DynamicDecals.System.MarkRenderers();
        }
        public void MoveToBottom(ProjectionRenderer Instance)
        {
            //Remove the instance from the list
            instances.Remove(Instance);

            //Add instance to top of list
            instances.Insert(0, Instance);

            //Mark renderers to change
            DynamicDecals.System.MarkRenderers();
        }

        //Constructor
        public ProjectionData(Projection Projection)
        {
            //Initialize Projection
            projection = Projection;
            instances = new List<ProjectionRenderer>();
        }

        //Order
        public void AssertOrder(ref int Order)
        {
            if (projection.Instanced)
            {
                foreach (ProjectionRenderer renderer in instances)
                {
                    renderer.Renderer.sortingOrder = Order;
                }
                Order++;
            }
            else
            {
                foreach (ProjectionRenderer renderer in instances)
                {
                    renderer.Renderer.sortingOrder = Order;
                    Order++;
                }
            }
        }

        //Renderers
        public void EnableRenderers()
        {
            for (int j = 0; j < instances.Count; j++)
            {
                instances[j].InitializeRenderer(true);
            }
        }
        public void DisableRenderers()
        {
            for (int j = 0; j < instances.Count; j++)
            {
                instances[j].TerminateRenderer();
            }
        }
        public void UpdateRenderers()
        {
            for (int j = 0; j < instances.Count; j++)
            {
                instances[j].UpdateProjection();
            }
        }
    }
}