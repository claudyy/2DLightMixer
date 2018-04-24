using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class LightModifier  {
    public enum ModifierType {
        Blur,
        Cut,
        CellShading,
        MixTexture
    }
    public ModifierType type;
    public Material material;
    public RenderTexture copyrtLight;
    //Blur
    [Range(0, 10)]
    public int iterations;
    [Range(0, 4)]
    public int downRes;



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
                break;
            case ModifierType.CellShading:
                break;
            case ModifierType.MixTexture:
                break;
            default:
                break;
        }
        
    }
    public void ApplyModifier(RenderTexture source, RenderTexture destination, RenderTexture rtLight) {


        switch (type) {
            case ModifierType.Blur:
                Blur(source, destination,rtLight);
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

    public void OnInspector() {
#if UNITY_EDITOR
        switch (type) {
            case ModifierType.Blur:
                iterations = UnityEditor.EditorGUILayout.IntField("iteration", iterations);
                downRes = UnityEditor.EditorGUILayout.IntField("down resolution", downRes);
                break;
            case ModifierType.Cut:
                break;
            case ModifierType.CellShading:
                break;
            case ModifierType.MixTexture:
                break;
            default:
                break;
        }



#endif
    }
}
