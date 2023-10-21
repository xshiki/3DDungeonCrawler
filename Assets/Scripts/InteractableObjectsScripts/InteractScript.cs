using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractScript : MonoBehaviour
{
 
    [SerializeField] private LayerMask _interactableMask;
    [SerializeField] private InteractionPromptUI _interactionPromptUI;
    public bool IsInteracting { get; private set; }
    private IInteractable _interactable;
    [SerializeField]
    private Camera _playerCam;
    [SerializeField]
    private float distance = 1f;

    private Highlight highlight;
    private void Update()
    {
        Ray ray = new Ray(_playerCam.transform.position, _playerCam.transform.forward);
        RaycastHit hitInfo;
        if (_interactionPromptUI.IsDisplayed) { _interactionPromptUI.Close(); }
        if (Physics.Raycast(ray, out hitInfo, distance, _interactableMask))
        {
            if(hitInfo.collider.GetComponent<IInteractable>() != null)
            {
                
                _interactable = hitInfo.collider.GetComponent<IInteractable>();
              
                if (!_interactionPromptUI.IsDisplayed)
                {
                    _interactionPromptUI.SetUp(_interactable.InterActionPrompt);
                    highlight = hitInfo.collider.GetComponent<Highlight>();
                    hitInfo.collider.GetComponent<Highlight>()?.ToggleHighlight(true);
                }
                if (Keyboard.current.fKey.wasPressedThisFrame) { _interactable.Interact(this, out bool interactSucessful); }
            }

        }
        else
        {

            if (_interactable != null)
            {
                if(highlight != null)
                {
                    highlight.ToggleHighlight(false);
                    highlight = null;
                }
               
                _interactable = null;
            }
        }
      


        /*
        _numFound = Physics.OverlapSphereNonAlloc(_interactionPoint.position, _interactionPointRadius, _colliders, _interactableMask);
        //var colliders = Physics.OverlapSphere(InteractionPoint.position, InteracitonRadius, InteractionLayer)
        if (_numFound > 0)
        {
            _interactable = _colliders[0].GetComponent<IInteractable>();
            if (_interactable != null) {
                if (!_interactionPromptUI.IsDisplayed)
                {
                    _interactionPromptUI.SetUp(_interactable.InterActionPrompt);

                }
                if (Keyboard.current.fKey.wasPressedThisFrame) { _interactable.Interact(this, out bool interactSucessful); }
            }
        }
        else
        {
            if (_interactable != null) { _interactable = null;  }
            if (_interactionPromptUI.IsDisplayed) { _interactionPromptUI.Close(); }
        }

        */
    }

    void StartInteraction(IInteractable interactable)
    {

        interactable.Interact(this, out bool interactSuccesful);
        IsInteracting = true;
        //disable movement etc
    }


    void EndInteraction()
    {

        IsInteracting = false;
    }


    private void OnDrawGizmos()
    {
        
    }
}
