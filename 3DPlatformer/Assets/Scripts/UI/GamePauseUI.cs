using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button optionsButton;

    private void Awake()
    {
        resumeButton.onClick.AddListener(() =>
        {
            SanyaGameManager.Instance.TogglePauseGame();
        });

        mainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenuScene);
            Collectible.totalCollectibles = 0;
        });

        optionsButton.onClick.AddListener(() =>
        {
            OptionsUI.Instance.Show();
        });

    }

    private void Start()
    {
        SanyaGameManager.Instance.OnGamePaused += SanyaGameManager_OnGamePaused;
        SanyaGameManager.Instance.OnGameUnpaused += SanyaGameManager_OnGameUnpaused;

        Hide();
    }

    private void SanyaGameManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void SanyaGameManager_OnGamePaused(object sender, System.EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
