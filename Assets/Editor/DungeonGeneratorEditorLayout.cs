using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(ProceduralDungeonGenerator))]
public class DungeonGeneratorEditorLayout : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ProceduralDungeonGenerator dunGen = (ProceduralDungeonGenerator)target;
        if (GUILayout.Button("GenerateSeed"))
        {
            dunGen.seed = System.Environment.TickCount;
        }
    }


}
