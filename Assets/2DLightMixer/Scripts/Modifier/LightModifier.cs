using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class LightModifier  {
    public enum ModifierType {
        Blur,
        Cut,
        lookUp,
        MixTexture
    }
    public ModifierType type;
    public Material material;
    public RenderTexture copyrtLight;

    public bool active;

    //Blur
    [Range(0, 10)]
    public int iterations;
    [Range(0, 4)]
    public int downRes;
    //Cut
    float cut;
    float cutSmoothness;
    //MixTexture
    Texture blendTexture;
    float blendTextureAmount;

    public LightModifier(ModifierType type) {
        this.type = type;
        Init();
    }
    public void Init() {
        switch (type) {
            case ModifierType.Blur:
                material = new Material(Shader.Find("Hidden/Blur"));
                break;
            case ModifierType.Cut:
                material = new Material(Shader.Find("Hidden/Cut"));
                break;
            case ModifierType.lookUp:
                break;
            case ModifierType.MixTexture:
                break;
            default:
                break;
        }
        
    }
    public void ApplyModifier(RenderTexture source, RenderTexture destination, RenderTexture rtLight) {
        if (active == false)
            return;

        switch (type) {
            case ModifierType.Blur:
                Blur(source, destination,rtLight);
                break;
            case ModifierType.Cut:
                Cut(source, destination, rtLight);
                break;
            default:
                break;
        }
    }
    void Blur(RenderTexture source, RenderTexture destination,RenderTexture rtLight) {
        int width = rtLight.width >> downRes;
        int height = rtLight.height >> downRes;

        RenderTexture rt = RenderTexture.GetTemporary(width, height);
        Graphics.Blit(rtLight, rt);

        for (int i = 0; i < iterations; i++) {
            RenderTexture rt2 = RenderTexture.GetTemporary(width, height);
            Graphics.Blit(rt, rt2, material);
            RenderTexture.ReleaseTemporary(rt);
            rt = rt2;
        }

        Graphics.Blit(rt, rtLight);
        RenderTexture.ReleaseTemporary(rt);

    }
    void Cut(RenderTexture source, RenderTexture destination, RenderTexture rtLight) {
        RenderTexture rt = RenderTexture.GetTemporary(rtLight.width, rtLight.height);
        material.SetFloat("_Cut", cut);
        material.SetFloat("_Smoothness", cut);
        Graphics.Blit(rtLight, rt, material);
        Graphics.Blit(rt, rtLight);
        RenderTexture.ReleaseTemporary(rt);
    }
    public void OnInspector() {
#if UNITY_EDITOR
        if(active == false) {
            if (GUILayout.Button("activate"))
                active = true;
            UnityEngine.GUI.backgroundColor = Color.red;
        } else {
            if (GUILayout.Button("deactivate"))
                active = false;
        }


        switch (type) {
            case ModifierType.Blur:
                iterations = UnityEditor.EditorGUILayout.IntField("iteration", iterations);
                downRes = UnityEditor.EditorGUILayout.IntField("down resolution", downRes);
                break;
            case ModifierType.Cut:
                cut = UnityEditor.EditorGUILayout.FloatField("cut", cut);
                cutSmoothness = UnityEditor.EditorGUILayout.FloatField("smoothness", cutSmoothness);
                break;
            case ModifierType.lookUp:
                break;
            case ModifierType.MixTexture:
                break;
            default:
                break;
        }
        UnityEngine.GUI.backgroundColor = Color.white;

#endif
    }
}
