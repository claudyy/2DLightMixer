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
        Darken,
        Screen,
        Lighten,
        Difference,
        Negation,
        Exclusion,
        Overlay,
        HardLight,
        SoftLight,
        Dodge,
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
    [HideInInspector]
    public Camera cullingCam;

    public LightModifier.ModifierType modiferType;
    public List<LightModifier> lightModifers = new List<LightModifier>();
    // Use this for initialization
    void Start () {
        UpdateMixer();
        
    }
	
	// Update is called once per frame
	void Update () {
        UpdateCamera();
    }
    #region Init
    void InitCamera()
    {
        targetcam = GetComponent<Camera>();
        if (targetcam == null)
            targetcam = Camera.main;
        rtLight = new RenderTexture(targetcam.pixelWidth, targetcam.pixelHeight, 0);

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
    void InitScreen() {
        InitCamera();
        m = new Material(Shader.Find("Hidden/ScreenLight"));
    }
    void InitDarken() {
        InitCamera();
        m = new Material(Shader.Find("Hidden/DarkenLight"));
    }
    void InitLighten() {
        InitCamera();
        m = new Material(Shader.Find("Hidden/LightenLight"));
    }
    void InitDifference() {
        InitCamera();
        m = new Material(Shader.Find("Hidden/DifferenceLight"));
    }
    void InitNegation() {
        InitCamera();
        m = new Material(Shader.Find("Hidden/NegationLight"));
    }
    void InitExclusion() {
        InitCamera();
        m = new Material(Shader.Find("Hidden/ExclusionLight"));
    }
    void InitOverlay() {
        InitCamera();
        m = new Material(Shader.Find("Hidden/OverlayLight"));
    }
    void InitHardLight() {
        InitCamera();
        m = new Material(Shader.Find("Hidden/HardLightLight"));
    }
    void InitSoftLight() {
        InitCamera();
        m = new Material(Shader.Find("Hidden/SoftLightLight"));
    }
    void InitDodge() {
        InitCamera();
        m = new Material(Shader.Find("Hidden/DodgeLight"));
    }
    void InitShadowLayer()
    {
        InitCamera();
        rtCulling = new RenderTexture(targetcam.pixelWidth, targetcam.pixelHeight, 0);

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
        cullingCam.backgroundColor = targetcam.backgroundColor;
        m = new Material(Shader.Find("Hidden/MixWithCamera"));
    }
#endregion
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
        for (int i = 0; i < lightModifers.Count; i++) {
            lightModifers[i].ApplyModifier(source, destination, rtLight);
        }
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
            case LightMixType.Darken:
                LightAdd(source, destination);
                break;
            case LightMixType.Screen:
                LightAdd(source, destination);
                break;
            case LightMixType.Lighten:
                LightAdd(source, destination);
                break;
            case LightMixType.Difference:
                LightAdd(source, destination);
                break;
            case LightMixType.Negation:
                LightAdd(source, destination);
                break;
            case LightMixType.Exclusion:
                LightAdd(source, destination);
                break;
            case LightMixType.Overlay:
                LightAdd(source, destination);
                break;
            case LightMixType.HardLight:
                LightAdd(source, destination);
                break;
            case LightMixType.SoftLight:
                LightAdd(source, destination);
                break;
            case LightMixType.Dodge:
                LightAdd(source, destination);
                break;
            case LightMixType.MixShadowLayer:
                LightMixWithLayer(source, destination);
                break;
            default:
                break;
        }
        
    }
    #region Blits
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
    void LightDarken(RenderTexture source, RenderTexture destination) {
        m.SetFloat("_AddValue", lightAdd);

        m.SetTexture("_LightLayer", rtLight);
        Graphics.Blit(source, destination, m);
    }
    void LightScreen(RenderTexture source, RenderTexture destination) {
        m.SetFloat("_AddValue", lightAdd);

        m.SetTexture("_LightLayer", rtLight);
        Graphics.Blit(source, destination, m);
    }
    void LightDifference(RenderTexture source, RenderTexture destination) {
        m.SetFloat("_AddValue", lightAdd);

        m.SetTexture("_LightLayer", rtLight);
        Graphics.Blit(source, destination, m);
    }
    void LightNegation(RenderTexture source, RenderTexture destination) {
        m.SetFloat("_AddValue", lightAdd);

        m.SetTexture("_LightLayer", rtLight);
        Graphics.Blit(source, destination, m);
    }
    void LightExclusion(RenderTexture source, RenderTexture destination) {
        m.SetFloat("_AddValue", lightAdd);

        m.SetTexture("_LightLayer", rtLight);
        Graphics.Blit(source, destination, m);
    }
    void LightOverlay(RenderTexture source, RenderTexture destination) {
        m.SetFloat("_AddValue", lightAdd);

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
    #endregion
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
            case LightMixType.Darken:
                InitDarken();
                break;
            case LightMixType.Screen:
                InitScreen();
                break;
            case LightMixType.Lighten:
                InitLighten();
                break;
            case LightMixType.Difference:
                InitDifference();
                break;
            case LightMixType.Negation:
                InitNegation();
                break;
            case LightMixType.Exclusion:
                InitExclusion();
                break;
            case LightMixType.Overlay:
                InitOverlay();
                break;
            case LightMixType.HardLight:
                InitHardLight();
                break;
            case LightMixType.SoftLight:
                InitSoftLight();
                break;
            case LightMixType.Dodge:
                InitDodge();
                break;
            case LightMixType.MixShadowLayer:
                InitShadowLayer();
                break;
            default:
                break;
        }
    }

    #region Modifiers
    public void ActivateAllModifier() {
        for (int i = 0; i < lightModifers.Count; i++) {
            SetActivateModifier(i, true);
        }
    }
    public void DeactivateAllModifier() {
        for (int i = 0; i < lightModifers.Count; i++) {
            SetActivateModifier(i, false);
        }
    }
    public void SetActivateModifier(int index, bool active) {
        if (lightModifers.Count >= index)
            return;
        lightModifers[index].active = active;
    }
    #endregion

}
