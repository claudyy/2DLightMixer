using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class LightMixer : MonoBehaviour {
    public enum LightMixType
    {
        Add,
        Multiply,
        Mix,
        MixShadowLayer
    }
    [HideInInspector]
    public RenderTexture rtLight;
    [HideInInspector]
    public Camera lightCam;
    [HideInInspector]
    public Camera targetcam;
    [HideInInspector]
    public Material m;
    public LightMixType mixType;
    public LayerMask lightLayer;

    public float lightAdd = 1f;

    public float multiplyFactor = 1f;
    public Color mulitpyColor;

    public float mixColorValue = 1f;
    public Color mixColor;


    public LayerMask CullingLayer;
    public Color cullingBackgroundColor;
    public float cullingMixValue = 1f;
    public Color cullingColor;
    public float colorMultiplyValue = 0.5f;


    [HideInInspector]
    public RenderTexture rtCulling;
    Camera cullingCam;
    // Use this for initialization
    void Start () {
       
    }
	
	// Update is called once per frame
	void Update () {
        UpdateCamera();
    }
    void InitCamera()
    {
        targetcam = GetComponent<Camera>();
        if (targetcam == null)
            targetcam = Camera.main;
        rtLight = new RenderTexture(targetcam.pixelHeight, targetcam.pixelHeight, 0);

        if(lightCam == null)
            lightCam = new GameObject().AddComponent<Camera>();
        lightCam.targetTexture = rtLight;
        lightCam.transform.SetParent(targetcam.transform);
        lightCam.transform.localPosition = Vector3.zero;
        lightCam.transform.localEulerAngles = Vector3.zero;
        lightCam.backgroundColor = Color.black;
        lightCam.cullingMask = lightLayer;
        lightCam.clearFlags = CameraClearFlags.Color;
        ApplyCameraSetting(targetcam, lightCam);

        if(targetcam.cullingMask == (targetcam.cullingMask |  lightLayer))
            targetcam.cullingMask ^= lightLayer;



    }
    void InitAdd()
    {
        InitCamera();
        m = new Material(Shader.Find("Hidden/AddLight"));
    }
    void InitMultiply()
    {
        InitCamera();
        m = new Material(Shader.Find("Hidden/MultiplyLight"));
    }
    void InitMixColor()
    {
        InitCamera();
        m = new Material(Shader.Find("Hidden/MixColorShadow"));
    }
    void InitShadowLayer()
    {
        InitCamera();
        rtCulling = new RenderTexture(targetcam.pixelHeight, targetcam.pixelHeight, 0);

        if(cullingCam == null)
            cullingCam = new GameObject().AddComponent<Camera>();
        cullingCam.targetTexture = rtCulling;
        cullingCam.transform.SetParent(targetcam.transform);
        cullingCam.transform.localPosition = Vector3.zero;
        cullingCam.transform.localEulerAngles = Vector3.zero;
        cullingCam.backgroundColor = cullingBackgroundColor;
        cullingCam.cullingMask = CullingLayer;
        cullingCam.clearFlags = CameraClearFlags.Color;
        ApplyCameraSetting(targetcam, cullingCam);
        m = new Material(Shader.Find("Hidden/MixWithCamera"));
    }
    void UpdateCamera()
    {
        if (lightCam == null)
            return;
        lightCam.aspect = targetcam.aspect;
    }
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (rtLight == null)
            return;
        switch (mixType)
        {
            case LightMixType.Add:
                LightAdd(source, destination);
                break;
            case LightMixType.Multiply:
                LightMultiply(source, destination);
                break;
            case LightMixType.Mix:
                LightMixColor(source, destination);
                break;
            case LightMixType.MixShadowLayer:
                LightMixWithLayer(source, destination);
                break;
            default:
                break;
        }
        
    }
    void LightAdd(RenderTexture source, RenderTexture destination)
    {
        m.SetFloat("_AddValue", lightAdd);

        m.SetTexture("_LightLayer", rtLight);
        Graphics.Blit(source, destination, m);
    }
    void LightMultiply(RenderTexture source, RenderTexture destination)
    {
        m.SetFloat("_MultiplyValue", multiplyFactor);
        m.SetColor("_ShadowColor", mulitpyColor);

        m.SetTexture("_LightLayer", rtLight);
        Graphics.Blit(source, destination, m);
    }
    void LightMixColor(RenderTexture source, RenderTexture destination)
    {
        m.SetFloat("_MixValue", mixColorValue);
        m.SetColor("_ShadowColor", mixColor);

        m.SetTexture("_LightLayer", rtLight);
        Graphics.Blit(source, destination, m);
    }
    void LightMixWithLayer(RenderTexture source, RenderTexture destination)
    {
        m.SetFloat("_MixValue", cullingMixValue);
        m.SetFloat("_ColorMultiply", colorMultiplyValue);
        m.SetColor("_Color", cullingColor);


        m.SetTexture("_OtherCamera", rtCulling);
        m.SetTexture("_LightLayer", rtLight);
        Graphics.Blit(source, destination, m);
    }
    void ApplyCameraSetting(Camera from,Camera to)
    {
        to.orthographic = from.orthographic;
        to.orthographicSize = from.orthographicSize;
        to.fieldOfView = from.fieldOfView;
        to.aspect = from.aspect;

    }
    public void UpdateMixer()
    {
        switch (mixType)
        {
            case LightMixType.Add:
                InitAdd();
                break;
            case LightMixType.Multiply:
                InitMultiply();
                break;
            case LightMixType.Mix:
                InitMixColor();
                break;
            case LightMixType.MixShadowLayer:
                InitShadowLayer();
                break;
            default:
                break;
        }
    }
}
