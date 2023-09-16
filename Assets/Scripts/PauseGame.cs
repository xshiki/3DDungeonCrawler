using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseGame : MonoBehaviour
{
    public GameObject pauseMenuUI;

    [SerializeField]
    private PlayerInput playerInput;
    PlayerInput.PlayerActions input;

    private bool GameIsPaused = false;

    private void Awake()
    {
        playerInput =  new PlayerInput();
        input = playerInput.Player;

        input.Pause.performed += PauseResume;

    }
 



    private void PauseResume(InputAction.CallbackContext context)
    {
        GameIsPaused = !GameIsPaused;
        if (GameIsPaused)
        {
            Debug.Log("game paused");
            Time.timeScale = 0;
            pauseMenuUI.SetActive(true);
            return;
        }
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;

    }
    

    private void OnEnable()
    {
        input.Enable();

    }


    private void OnDisable()
    {
        input.Disable();
    }


}
