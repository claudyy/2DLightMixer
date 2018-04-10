using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
[CustomEditor(typeof(LightMixer))]
public class LightMixerEditor : Editor {
    SerializedProperty modifier;
    SerializedProperty mixer;
    private void OnEnable() {
        modifier = serializedObject.FindProperty("lightModifer");

    }
    public override void OnInspectorGUI() {
        LightMixer myTarget = (LightMixer)target;
        myTarget.mixType = (LightMixer.LightMixType)EditorGUILayout.EnumPopup("Light mix type:", myTarget.mixType);
        //myTarget.lightLayer = EditorGUILayout.LayerField("Player Flags", myTarget.lightLayer);
        LayerMask tempMask = EditorGUILayout.MaskField(InternalEditorUtility.LayerMaskToConcatenatedLayersMask(myTarget.lightLayer), InternalEditorUtility.layers);
        myTarget.lightLayer = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(tempMask);
        LightMixer.LightMixType type = myTarget.mixType;

        //EditorGUILayout.PropertyField(serializedObject.FindProperty("copyrtLight"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("blurM"));
        serializedObject.ApplyModifiedProperties();
        //ShowRelativeProperty(modifier, "type");
        //ShowRelativeProperty(modifier, "m");
        //ShowRelativeProperty(modifier, "copyrtLight");
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
        //myTarget.experience = EditorGUILayout.IntField("Experience", myTarget.experience);
        //EditorGUILayout.LabelField("Level", myTarget.Level.ToString());
    }
    void ShowRelativeProperty(SerializedProperty serializedProperty, string propertyName) {
        SerializedProperty property = serializedProperty.FindPropertyRelative(propertyName);
        if (property != null) {
            EditorGUI.indentLevel++;
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(property, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
            EditorGUIUtility.LookLikeControls();
            EditorGUI.indentLevel--;
        }
    }
    void UpdateLightMixer(LightMixer mixer) {
        mixer.UpdateMixer();
    }
}
