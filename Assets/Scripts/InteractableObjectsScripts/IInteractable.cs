using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public interface IInteractable
{
    public string InterActionPrompt { get; }

    public bool Interact(InteractScript interactor);
    public void Interact(InteractScript interactor, out bool interactSucessful);
    public void EndInteraction();
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }
  
}
