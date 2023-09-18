using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    Animator animator;
    public bool isExitDoor = false;


    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public UnityAction<IInteractable> OnInteractionComplete { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    string IInteractable.InterActionPrompt => _prompt;

    public void EndInteraction()
    {
        Debug.Log("closed chest");
    }

    public void Interact(InteractScript interactor, out bool interactSucessfull)
    {


        Debug.Log("Opening door!");
        interactSucessfull = true;
        bool isOpen = animator.GetBool("isOpen");
        animator.SetBool("isOpen", !isOpen);
        if (isExitDoor)
        {
            SceneManager.LoadScene("LoadNewMap");
        }

    }

    bool IInteractable.Interact(InteractScript interactor)
    {
        Debug.Log("Opening door!");
        return true;
    }
}
