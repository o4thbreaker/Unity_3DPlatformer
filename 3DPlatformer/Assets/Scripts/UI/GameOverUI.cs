using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] Button mainMenuButton;

    private void Awake()
    {
        mainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenuScene);
            Collectible.totalCollectibles = 0;
        });
    }

    private void Start()
    {
        SanyaGameManager.Instance.OnStateChanged += SanyaGameManager_OnStateChanged;

        Hide();
    }

    private void SanyaGameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (SanyaGameManager.Instance.IsGameOver())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);    
    }
}
