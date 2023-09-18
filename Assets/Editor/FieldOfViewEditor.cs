using System.Collections;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(EnemyAI),true)]
public class FieldOfViewEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
    private void OnSceneGUI()
    {
        EnemyAI enemyAI = (EnemyAI)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(enemyAI.transform.position, Vector3.up, Vector3.forward, 360, enemyAI.viewRadius);

        Vector3 viewAngle01 = DirectionFromAngle(enemyAI.transform.eulerAngles.y, -enemyAI.viewAngle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(enemyAI.transform.eulerAngles.y, enemyAI.viewAngle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(enemyAI.transform.position, enemyAI.transform.position + viewAngle01 * enemyAI.viewRadius);
        Handles.DrawLine(enemyAI.transform.position, enemyAI.transform.position + viewAngle02 * enemyAI.viewRadius);
    }


    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0 , Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
