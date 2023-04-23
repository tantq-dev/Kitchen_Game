using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        playButton.onClick.AddListener(PlayClick);
        quitButton.onClick.AddListener(QuitClick);
        Reset();
    }
    private void PlayClick()
    {
        Loader.Load(Loader.Scene.GameScene);
    }
    private void QuitClick()
    {
        Application.Quit();
    }
    private void Reset()
    {
        Time.timeScale = 1;
    }
}
