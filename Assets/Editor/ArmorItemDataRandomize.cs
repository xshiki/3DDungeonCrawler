using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ArmorItemData))]

public class ArmorItemDataRandomize : Editor
{

    int randomizeLimit = 100;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ArmorItemData armorItemData = (ArmorItemData)target;
    
        //randomizeLimit = EditorGUILayout.IntField("Randomize limit:", randomizeLimit);
        if(GUILayout.Button("Randomize Stats"))
        {
            armorItemData.stamina = Random.Range(0, randomizeLimit);
            armorItemData.strength = Random.Range(0, randomizeLimit);
            armorItemData.intelligence = Random.Range(0, randomizeLimit);
            armorItemData.speed = Random.Range(0, 3);
            armorItemData.armor = Random.Range(0, randomizeLimit);
        }
    }
}
