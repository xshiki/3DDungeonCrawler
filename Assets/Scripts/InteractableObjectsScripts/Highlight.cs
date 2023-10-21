using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



/// <summary>
///  Creates a highlight material on interactable objects
/// </summary>
public class Highlight : MonoBehaviour
{



    [SerializeField]
    private List<Renderer> renderers;
    [SerializeField]
    private Color color = Color.white;

    //helper list to cache all the materials ofd this object
    private List<Material> materials;
    private void Awake()
    {
        if(renderers.Count == 0)
        {
            renderers = GetComponentsInChildren<Renderer>().ToList();
        }
      
        materials = new List<Material>();
        foreach (var renderer in renderers)
        {
            //A single child-object might have mutliple materials on it
            //that is why we need to all materials with "s"
            materials.AddRange(new List<Material>(renderer.materials));
        }
    }


    public void ToggleHighlight(bool val)
    {
        if (val)
        {
            foreach (var material in materials)
            {
                //We need to enable the EMISSION
                material.EnableKeyword("_EMISSION");
                //before we can set the color
                material.SetColor("_EmissionColor", color);
            }
        }
        else
        {
            foreach (var material in materials)
            {
                //we can just disable the EMISSION
                //if we don't use emission color anywhere else
                material.DisableKeyword("_EMISSION");
            }
        }
    }
}
