using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Texture2D defaultCursor;
    private Vector2 defaultHotspot = Vector2.zero;

    private void Start()
    {
        Cursor.SetCursor(defaultCursor, defaultHotspot, CursorMode.Auto);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Arena");
        ClickSound();
    }

    public void ClickSound()
    {
        AudioManager.instance.Play("ClickUI");
    }

    public void BackSound()
    {
        AudioManager.instance.Play("BackUI");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting...");
        Application.Quit();
    }
}
