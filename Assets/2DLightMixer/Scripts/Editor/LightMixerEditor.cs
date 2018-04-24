using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
[CustomEditor(typeof(LightMixer))]
public class LightMixerEditor : Editor {
    private void OnEnable() {

    }
    public override void OnInspectorGUI() {
        LightMixer myTarget = (LightMixer)target;
        myTarget.mixType = (LightMixer.LightMixType)EditorGUILayout.EnumPopup("Light mix type:", myTarget.mixType);
        LayerMask tempMask = EditorGUILayout.MaskField(InternalEditorUtility.LayerMaskToConcatenatedLayersMask(myTarget.lightLayer), InternalEditorUtility.layers);
        myTarget.lightLayer = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(tempMask);
        LightMixer.LightMixType type = myTarget.mixType;
        DisplayModifiers(myTarget, myTarget.lightModifers);


        switch (type) {
            case LightMixer.LightMixType.Add:
                myTarget.lightAdd = EditorGUILayout.FloatField("light add value", myTarget.lightAdd);
                break;
            case LightMixer.LightMixType.Multiply:
                myTarget.multiplyFactor = EditorGUILayout.FloatField("multiply Factor", myTarget.multiplyFactor);
                myTarget.mulitpyColor = EditorGUILayout.ColorField("multiply Color", myTarget.mulitpyColor);

                break;
            case LightMixer.LightMixType.Mix:
                myTarget.mixColorValue = EditorGUILayout.FloatField("mix value", myTarget.mixColorValue);
                myTarget.mixColor = EditorGUILayout.ColorField("mix Color", myTarget.mixColor);
                break;
            case LightMixer.LightMixType.Darken:
                myTarget.lightAdd = EditorGUILayout.FloatField("light add value", myTarget.lightAdd);
                break;
            case LightMixer.LightMixType.Screen:
                myTarget.lightAdd = EditorGUILayout.FloatField("light add value", myTarget.lightAdd);
                break;
            case LightMixer.LightMixType.Lighten:
                myTarget.lightAdd = EditorGUILayout.FloatField("light add value", myTarget.lightAdd);
                break;
            case LightMixer.LightMixType.Difference:
                myTarget.lightAdd = EditorGUILayout.FloatField("light add value", myTarget.lightAdd);
                break;
            case LightMixer.LightMixType.Negation:
                myTarget.lightAdd = EditorGUILayout.FloatField("light add value", myTarget.lightAdd);
                break;
            case LightMixer.LightMixType.Exclusion:
                myTarget.lightAdd = EditorGUILayout.FloatField("light add value", myTarget.lightAdd);
                break;
            case LightMixer.LightMixType.Overlay:
                myTarget.lightAdd = EditorGUILayout.FloatField("light add value", myTarget.lightAdd);
                break;
            case LightMixer.LightMixType.HardLight:
                myTarget.lightAdd = EditorGUILayout.FloatField("light add value", myTarget.lightAdd);
                break;
            case LightMixer.LightMixType.SoftLight:
                myTarget.lightAdd = EditorGUILayout.FloatField("light add value", myTarget.lightAdd);
                break;
            case LightMixer.LightMixType.Dodge:
                myTarget.lightAdd = EditorGUILayout.FloatField("light add value", myTarget.lightAdd);
                break;
            case LightMixer.LightMixType.MixShadowLayer:
                LayerMask tempMask2 = EditorGUILayout.MaskField(InternalEditorUtility.LayerMaskToConcatenatedLayersMask(myTarget.CullingLayer), InternalEditorUtility.layers);
                myTarget.CullingLayer = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(tempMask2);
                myTarget.cullingMixValue = EditorGUILayout.FloatField("mix Factor", myTarget.cullingMixValue);
                myTarget.colorMultiplyValue = EditorGUILayout.FloatField("multiply factor", myTarget.colorMultiplyValue);
                myTarget.cullingColor = EditorGUILayout.ColorField("multiply Color", myTarget.cullingColor);

                break;
            default:
                break;


        }
        if (GUILayout.Button("Update"))
            UpdateLightMixer(myTarget);

    }
   
    void UpdateLightMixer(LightMixer mixer) {
 
        mixer.UpdateMixer();
    }
    void DisplayModifiers(LightMixer myTarget, List<LightModifier> modifiers) {
        myTarget.modiferType = (LightModifier.ModifierType)EditorGUILayout.EnumPopup("Light mix type:", myTarget.modiferType);
        if (GUILayout.Button("Add modifer"))
            modifiers.Add(new LightModifier(myTarget.modiferType));
        for (int i = 0; i < modifiers.Count; i++) {
            modifiers[i].OnInspector();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Up"))
                ModifierMoveDown(i,modifiers);
            if (GUILayout.Button("Down"))
                ModifierMoveUp(i, modifiers);
            if (GUILayout.Button("Remove"))
                RemoveModifier(i, modifiers);
            GUILayout.EndHorizontal();
        }
    }
    void RemoveModifier(int index, List<LightModifier> modifiers) {
        modifiers.RemoveAt(index);
    }
    void ModifierMoveUp(int index, List<LightModifier> modifiers) {
        if (index == modifiers.Count - 1)
            return;
        var temp = modifiers[index + 1];
        modifiers[index + 1] = modifiers[index];
        modifiers[index] = temp;
    }
    void ModifierMoveDown(int index, List<LightModifier> modifiers) {
        if (index == 0)
            return;
        var temp = modifiers[index - 1];
        modifiers[index - 1] = modifiers[index];
        modifiers[index] = temp;
    }

}
