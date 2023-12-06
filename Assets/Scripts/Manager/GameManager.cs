using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Game over")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject winText;
    [SerializeField] private GameObject loseText;
    [SerializeField] private TextMeshProUGUI killsText;
    [SerializeField] private TextMeshProUGUI expText;
    [SerializeField] private TextMeshProUGUI hitsText;

    [Header("Gameplay UI")]
    [SerializeField] private GameObject rulesUI;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject[] difficulties;

    [Header("Cursors")]
    [SerializeField] private Texture2D crosshairCursor;
    [SerializeField] private Texture2D defaultCursor;
    private Vector2 crosshairHotspot;
    private Vector2 defaultHotspot;

    [Header("Variables & references")]
    [SerializeField, Range(15f, 181f)] private float startingTime;
    [SerializeField] private GameObject spawner;
    [SerializeField] private Spawner spawnerScript;
    [SerializeField] private Slider volumeSlider;

    [HideInInspector] public bool gameStarted = false;
    private PlayerStats playerStats;
    private PauseMenu pauseMenu;
    private float remainingTime;
    private bool gameIsOver;

    // Bools to avoid constant spawn rate update
    private bool easy = true;
    private bool medium = false;
    private bool hard = false;

    private void Awake()
    {
        Time.timeScale = 0f;
        remainingTime = startingTime;
        rulesUI.SetActive(true);
        gameStarted = false;
    }

    private void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        pauseMenu = FindObjectOfType<PauseMenu>();

        crosshairHotspot = new Vector2(crosshairCursor.width / 2, crosshairCursor.height / 2);
        defaultHotspot = new Vector2(0, 0);

        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            Load();
        }
        else
            Load();
    }

    private void Update()
    {
        if (gameStarted && !gameIsOver && !pauseMenu.gameIsPaused)
            Cursor.SetCursor(crosshairCursor, crosshairHotspot, CursorMode.Auto);
        else
            Cursor.SetCursor(defaultCursor, defaultHotspot, CursorMode.Auto);        

        if (gameStarted)
            TimeUpdate();

        if ((playerStats.currentHealth <= 0 || remainingTime <= 1) && !gameIsOver)
            GameOver();

        UpdateSpawnRate();
    }

    public void StartGame()
    {
        rulesUI.SetActive(false);
        Time.timeScale = 1f;
        AudioManager.instance.Play("ClickUI");
        gameStarted = true;
        spawner.SetActive(true);
    }

    private void TimeUpdate()
    {
        remainingTime -= Time.deltaTime;
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (remainingTime <= 16)
            timerText.color = Color.red;     
    }

    private void SetSpawnRate(float minRate, float maxRate)
    {
        spawnerScript.minSpawnRate = minRate;
        spawnerScript.maxSpawnRate = maxRate;
    }

    private void UpdateSpawnRate()
    {
        if (remainingTime <= 16 && hard)
        {
            hard = false;
            SetSpawnRate(0.1f, 0.38f);
            difficulties[2].SetActive(false);
            difficulties[3].SetActive(true);
        }

        else if (remainingTime <= 61 && medium)
        {
            medium = false;
            hard = true;
            SetSpawnRate(0.2f, 1.2f);
            difficulties[1].SetActive(false);
            difficulties[2].SetActive(true);
        }

        else if (remainingTime <= 121 && easy)
        {
            easy = false;
            medium = true;
            SetSpawnRate(0.5f, 1.5f);
            difficulties[0].SetActive(false);
            difficulties[1].SetActive(true);
        }
    }

    private void GameOver()
    {
        gameIsOver = true;
        Time.timeScale = 0f;
        pauseMenu.gameIsPaused = true;
        gameOverScreen.SetActive(true);

        GameObject[] damageAnim = GameObject.FindGameObjectsWithTag("DamageAnim");

        for (int i = 0;  i < damageAnim.Length; i++)
        {
            damageAnim[i].SetActive(false);
        }

        GameObject levelAnim = GameObject.FindWithTag("LevelAnim");
        if (levelAnim != null)
            levelAnim.SetActive(false);
        

        if (playerStats.currentHealth <= 0)
        {
            AudioManager.instance.Play("Lose");
            loseText.SetActive(true);
        }
        else
        {
            AudioManager.instance.Play("Win");
            winText.SetActive(true);
        }

        killsText.text = $"Total kills: {playerStats.totalKills}";
        expText.text = $"EXP gained: {playerStats.totalExperience}";
        hitsText.text = $"Hits taken: {playerStats.hitsTaken}";
    }

    public void Restart()
    {
        SceneManager.LoadScene("Arena");
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        Save();
    }

    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }
}
