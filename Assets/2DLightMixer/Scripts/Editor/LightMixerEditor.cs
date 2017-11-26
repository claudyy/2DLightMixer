using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
[CustomEditor(typeof(LightMixer))]
public class LightMixerEditor : Editor {

    public override void OnInspectorGUI() {
        LightMixer myTarget = (LightMixer)target;

        myTarget.mixType = (LightMixer.LightMixType)EditorGUILayout.EnumPopup("Light mix type:", myTarget.mixType);
        //myTarget.lightLayer = EditorGUILayout.LayerField("Player Flags", myTarget.lightLayer);
        LayerMask tempMask = EditorGUILayout.MaskField(InternalEditorUtility.LayerMaskToConcatenatedLayersMask(myTarget.lightLayer), InternalEditorUtility.layers);
        myTarget.lightLayer = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(tempMask);
        LightMixer.LightMixType type = myTarget.mixType;
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
    void UpdateLightMixer(LightMixer mixer) {
        mixer.UpdateMixer();
    }
}
