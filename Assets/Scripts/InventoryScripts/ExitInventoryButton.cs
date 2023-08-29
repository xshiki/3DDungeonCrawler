using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitInventoryButton : MonoBehaviour
{

    public GameObject playerInventory;
    public FirstPersonController firstPersonController;
    public void OnButtonPressed()
    {
        playerInventory.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        firstPersonController.enabled = true;
    }
}
