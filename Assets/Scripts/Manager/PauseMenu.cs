using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private AudioSource music;

    // Private references
    private GameObject[] damageAnim;
    private GameObject levelAnim;
    private GameManager gameManager;

    [HideInInspector] public bool gameIsPaused = false;
    [HideInInspector] public bool inOptions = false;

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
        AudioManager.instance.Play("ClickUI");
        gameIsPaused = false;

        for (int i = 0; i < damageAnim.Length; i++)
        {
            damageAnim[i].SetActive(true);
        }

        if (levelAnim != null)
            levelAnim.SetActive(true);
    }

    public void Pause()
    {
        damageAnim = GameObject.FindGameObjectsWithTag("DamageAnim");

        for (int i = 0; i < damageAnim.Length; i++)
        {
            damageAnim[i].SetActive(false);
        }

        levelAnim = GameObject.FindWithTag("LevelAnim");
        if (levelAnim != null)
            levelAnim.SetActive(false);

        pauseMenuUI.SetActive(true);
        AudioManager.instance.Play("Pause");
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void MainMenu()
    {
        AudioManager.instance.Play("ClickUI");
        SceneManager.LoadScene("Main menu");
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
