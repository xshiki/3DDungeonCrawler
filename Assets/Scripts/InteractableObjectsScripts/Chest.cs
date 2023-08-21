using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Chest : MonoBehaviour, IInteractable
{

    [SerializeField] private string _prompt;

    public UnityAction<IInteractable> OnInteractionComplete { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    string IInteractable.InterActionPrompt => _prompt;

    public void EndInteraction()
    {
        Debug.Log("closed chest!");
    }

    public void Interact(InteractScript interactor, out bool interactSucessful)
    {
        Debug.Log("Opening chest!");
        interactSucessful = true;
    }

    bool IInteractable.Interact(InteractScript interactor)
    {
        Debug.Log("Opening chest!");
        return true;    
    }
}
