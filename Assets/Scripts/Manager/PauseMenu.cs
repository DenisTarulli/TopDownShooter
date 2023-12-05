using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool gameIsPaused = false;
    public bool inOptions = false;

    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private AudioSource music;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && gameManager.gameStarted)
        {            
            if (gameIsPaused && !inOptions)
            {
                AudioManager.instance.Play("ClickUI");
                Resume();
            }
            else if (!gameIsPaused && !inOptions)
                Pause();
            else if (gameIsPaused && inOptions)
            {
                pauseMenuUI.SetActive(true);
                optionsMenu.SetActive(false);
                Options();
            }
        }

        if (gameIsPaused || !gameManager.gameStarted)
            music.volume = 0.02f;
        else
            music.volume = 0.06f;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;        
        gameIsPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        AudioManager.instance.Play("Pause");
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void MainMenu()
    {
        AudioManager.instance.Play("ClickUI");
        SceneManager.LoadScene("Main menu");
        gameIsPaused = false;
    }

    public void Options()
    {
        if (!inOptions)
        {
            inOptions = true;
            AudioManager.instance.Play("ClickUI");
        }
        else
        {
            inOptions = false;
            AudioManager.instance.Play("BackUI");
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        AudioManager.instance.Play("ClickUI");
        Application.Quit();
    }
}
