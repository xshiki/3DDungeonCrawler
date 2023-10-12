using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    Animator animator;
    public Animator transition;
    public bool isExitDoor = false;
    public Collider doorCollider;


    void Start()
    {
        doorCollider = GetComponent<Collider>();
        animator = GetComponent<Animator>();
        transition = GameObject.Find("CrossFade").GetComponent<Animator>();
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
            StartCoroutine(LoadLevel());
           
        }

    }

    IEnumerator LoadLevel() 
    {
        transition.Play("Crossfade_Start");
        yield return new WaitForSeconds(0.9f);

        SceneManager.LoadScene("LoadNewMap");
    }
    public void DoorColliderOff()

    {

        doorCollider.enabled = false;

    }



    public void DoorColliderOn()

    {

        doorCollider.enabled = true;

    }

    bool IInteractable.Interact(InteractScript interactor)
    {
        Debug.Log("Opening door!");
        return true;
    }
}
