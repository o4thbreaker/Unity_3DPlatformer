using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanyaGameManager : MonoBehaviour
{
    public static SanyaGameManager Instance { get; private set; }

    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;

    private bool isGamePaused = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PlayerStateMachine.Instance.OnPause += PlayerStateMachine_OnPause;
    }

    private void PlayerStateMachine_OnPause(object sender, System.EventArgs e)
    {
        TogglePauseGame();
    }

    public void TogglePauseGame()
    {
        isGamePaused = !isGamePaused;

        if (isGamePaused)
        {
            Debug.Log("Pause invoked");
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }
}
