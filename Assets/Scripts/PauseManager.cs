using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu;

    void Start()
    {
        pauseMenu.SetActive(false);
    }

        void Update()
    {
        // Check if the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Toggle pause/unpause and show/hide the pause menu
            TogglePause();
        }
    }

    public void BacktoMenu() { SceneManager.LoadScene("menu"); }

    public void TogglePause()
    {
        // Check if the game is currently paused
        bool isPaused = Time.timeScale == 0;

        if (isPaused)
        {
            // Unpause the game
            Time.timeScale = 1f;
            // Disable the pause menu GameObject
            pauseMenu.SetActive(false);
        }
        else
        {
            // Pause the game
            Time.timeScale = 0;
            // Enable the pause menu GameObject
            pauseMenu.SetActive(true);
        }
    }
}
