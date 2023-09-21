using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using UnityEngine;


[System.Serializable]
public class Stat
{
    [SerializeField]
    private int baseValue;

    public bool statModified = false;
    private List<int> modifiers = new List<int>();


   public int GetValue()
    {
        int finalValue = baseValue; 
        modifiers.ForEach(x  => finalValue += x);
        return finalValue;
    }

    public void SetValue(int newValue)
    {
        baseValue = newValue;
    }
    public void IncreaseValue(int incrementValue)
    {
        baseValue += incrementValue;

    }

    public void AddModifier(int modifier)
    {
        if(modifier != 0)
        {
            modifiers.Add(modifier);
            statModified = true;
        }
    }

    public void RemoveModifier(int modifier)
    {
        if(modifier != 0)
        {
            modifiers.Remove(modifier);
            if(modifiers.Count == 0)
            {
                statModified = false;
            }
        }
    }

}
