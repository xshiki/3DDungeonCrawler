using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeNotifier : MonoBehaviour
{
    public event EventHandler OnChildAdded;
    public event EventHandler OnChildRemoved;

    public Image icon;

    private void OnTransformChildrenChanged()
    {
        if (transform.childCount > 0)
        {
            OnChildAdded?.Invoke(this, EventArgs.Empty);
            icon = GetComponent<Image>();
            icon.enabled = false;
        }
        else
        {
            OnChildRemoved?.Invoke(this, EventArgs.Empty);
            Debug.Log("removed from slot");
            icon = GetComponent<Image>();
            icon.enabled = true;
        }
    }
}
