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
        DisplayModifiers(myTarget, myTarget.lightModifers);

        EditorGUILayout.LabelField("LightMixer", EditorStyles.boldLabel);
        myTarget.mixType = (LightMixer.LightMixType)EditorGUILayout.EnumPopup("Light mix type:", myTarget.mixType);
        LayerMask tempMask = EditorGUILayout.MaskField(InternalEditorUtility.LayerMaskToConcatenatedLayersMask(myTarget.lightLayer), InternalEditorUtility.layers);
        myTarget.lightLayer = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(tempMask);
        LightMixer.LightMixType type = myTarget.mixType;

        switch (type) {
            case LightMixer.LightMixType.Add:
                myTarget.lightAdd = EditorGUILayout.Slider("light add value", myTarget.lightAdd,0, 1);
                break;
            case LightMixer.LightMixType.Multiply:
                myTarget.multiplyFactor = EditorGUILayout.Slider("multiply Factor", myTarget.multiplyFactor,0, 1);
                myTarget.mulitpyColor = EditorGUILayout.ColorField("multiply Color", myTarget.mulitpyColor);
                break;
            case LightMixer.LightMixType.Mix:
                myTarget.mixColorValue = EditorGUILayout.Slider("mix value", myTarget.mixColorValue,0, 1);
                myTarget.mixColor = EditorGUILayout.ColorField("mix Color", myTarget.mixColor);
                break;
            case LightMixer.LightMixType.Darken:
                myTarget.lightAdd = EditorGUILayout.Slider("light add value", myTarget.lightAdd,0, 1);
                break;
            case LightMixer.LightMixType.Screen:
                myTarget.lightAdd = EditorGUILayout.Slider("light add value", myTarget.lightAdd,0, 1);
                break;
            case LightMixer.LightMixType.Lighten:
                myTarget.lightAdd = EditorGUILayout.Slider("light add value", myTarget.lightAdd,0, 1);
                break;
            case LightMixer.LightMixType.Difference:
                myTarget.lightAdd = EditorGUILayout.Slider("light add value", myTarget.lightAdd,0, 1);
                break;
            case LightMixer.LightMixType.Negation:
                myTarget.lightAdd = EditorGUILayout.Slider("light add value", myTarget.lightAdd, 0, 1);

                break;
            case LightMixer.LightMixType.Exclusion:
                myTarget.lightAdd = EditorGUILayout.Slider("light add value", myTarget.lightAdd, 0, 1);

                break;
            case LightMixer.LightMixType.Overlay:
                myTarget.lightAdd = EditorGUILayout.Slider("light add value", myTarget.lightAdd, 0, 1);

                break;
            case LightMixer.LightMixType.HardLight:
                myTarget.lightAdd = EditorGUILayout.Slider("light add value", myTarget.lightAdd, 0, 1);

                break;
            case LightMixer.LightMixType.SoftLight:
                myTarget.lightAdd = EditorGUILayout.Slider("light add value", myTarget.lightAdd, 0, 1);

                break;
            case LightMixer.LightMixType.Dodge:
                myTarget.lightAdd = EditorGUILayout.Slider("light add value", myTarget.lightAdd, 0, 1);

                break;
            case LightMixer.LightMixType.MixShadowLayer:
                LayerMask tempMask2 = EditorGUILayout.MaskField(InternalEditorUtility.LayerMaskToConcatenatedLayersMask(myTarget.CullingLayer), InternalEditorUtility.layers);
                myTarget.CullingLayer = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(tempMask2);
                myTarget.cullingMixValue = EditorGUILayout.Slider("mix Factor", myTarget.cullingMixValue,0, 1);
                myTarget.colorMultiplyValue = EditorGUILayout.Slider("multiply factor", myTarget.colorMultiplyValue,0, 1);
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
        GUILayout.Space(4);
        EditorGUILayout.LabelField("Modifiers ", EditorStyles.boldLabel);
        for (int i = 0; i < modifiers.Count; i++) {

            GUILayout.Space(4);
            EditorGUILayout.LabelField(modifiers[i].type.ToString(), EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Up"))
                ModifierMoveDown(i,modifiers);
            if (modifiers[i].active == false) {
                if (GUILayout.Button("activate"))
                    modifiers[i].active = true;
            } else {
                if (GUILayout.Button("deactivate"))
                    modifiers[i].active = false;
            }
            GUILayout.EndHorizontal();

            modifiers[i].OnInspector();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Down"))
                ModifierMoveUp(i, modifiers);
            if (GUILayout.Button("Remove"))
                RemoveModifier(i, modifiers);
            GUILayout.EndHorizontal();
        }
        GUILayout.BeginHorizontal();
        myTarget.modiferType = (LightModifier.ModifierType)EditorGUILayout.EnumPopup("Light mix type:", myTarget.modiferType);
        if (GUILayout.Button("Add modifer"))
            modifiers.Add(new LightModifier(myTarget.modiferType));
        GUILayout.EndHorizontal();
        GUILayout.Space(4);
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
