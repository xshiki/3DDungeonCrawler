using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseGame : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;

    [SerializeField]
    private PlayerInput playerInput;
    PlayerInput.PlayerActions input;

    private bool GameIsPaused = false;

    private void Awake()
    {
        playerInput =  new PlayerInput();
        settingsMenuUI = GameObject.Find("Settings Panel");
        settingsMenuUI.SetActive(false);
        input = playerInput.Player;

        input.Pause.performed += PauseResume;

    }
 



    private void PauseResume(InputAction.CallbackContext context)
    {
        GameIsPaused = !GameIsPaused;
        var player = GameObject.Find("Player").GetComponent<FirstPersonController>();
        var playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        if (GameIsPaused)
        {
            Debug.Log("game paused");
          
            player.enabled = false;
            playerController.enabled = false;
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            pauseMenuUI.SetActive(true);
            return;
        }
        player.enabled = true;
        playerController.enabled = true;
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;

    }
    

    public void PauseResume()
    {
        GameIsPaused = !GameIsPaused;
        var player = GameObject.Find("Player").GetComponent<FirstPersonController>();
        var playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        if (GameIsPaused)
        {
            player.enabled = false;
            playerController.enabled = false;
            Debug.Log("game paused");
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            pauseMenuUI.SetActive(true);
            return;
        }
        player.enabled = true;
        playerController.enabled = true;
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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
