using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeNotifier : MonoBehaviour
{
    public event EventHandler OnChildAdded;
    public event EventHandler OnChildRemoved;


    private void OnTransformChildrenChanged()
    {
        if (transform.childCount > 0)
        {
            OnChildAdded?.Invoke(this, EventArgs.Empty);
            Debug.Log("added as child");
        }
        else
        {
            OnChildRemoved?.Invoke(this, EventArgs.Empty);
            Debug.Log("removed from slot");
        }
    }
}
