using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject rulesUI;
    [HideInInspector] public bool gameStarted = false;

    private void Awake()
    {
        Time.timeScale = 0f;
    }

    private void Start()
    {
        gameStarted = false;
        rulesUI.SetActive(true);
    }

    public void StartGame()
    {
        rulesUI.SetActive(false);
        Time.timeScale = 1f;
        gameStarted = true;
    }
}
