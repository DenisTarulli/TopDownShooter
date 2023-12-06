using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject rulesUI;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject winText;
    [SerializeField] private GameObject loseText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI killsText;
    [SerializeField] private TextMeshProUGUI expText;
    [SerializeField] private TextMeshProUGUI hitsText;
    [SerializeField] private GameObject spawner;
    [SerializeField] private Spawner spawnerScript;
    [HideInInspector] public bool gameStarted = false;

    [SerializeField] private float startingTime = 180f;
    [SerializeField] private Slider volumeSlider;
    private PlayerStats playerStats;
    private PauseMenu pauseMenu;
    private float remainingTime;
    private bool gameIsOver;

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
        if (remainingTime <= 15)
            SetSpawnRate(0.15f, 0.5f);

        else if (remainingTime <= 60)
            SetSpawnRate(0.2f, 1.2f);

        else if (remainingTime <= 120)
            SetSpawnRate(0.5f, 1.5f);
    }

    private void GameOver()
    {
        gameIsOver = true;
        Time.timeScale = 0f;
        pauseMenu.gameIsPaused = true;
        gameOverScreen.SetActive(true);

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
