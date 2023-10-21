using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Scaling Config", menuName = "Scaling Config")]
public class ScalingScriptableObject : ScriptableObject
{
    public AnimationCurve healthCurve;
    public AnimationCurve damageCurve;
    public AnimationCurve speedCurve;
    public AnimationCurve strCurve;
    public AnimationCurve intCurve;
    public AnimationCurve armorCurve;
    public AnimationCurve experienceCurve;
    public AnimationCurve spawnRateCurve;
    public AnimationCurve spawnCountCurve;
   
}
