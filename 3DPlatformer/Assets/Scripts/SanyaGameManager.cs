using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanyaGameManager : MonoBehaviour
{
    private enum State
    {
        WaitingToStart,
        GamePlaying,
        GameOver
    }

    public static SanyaGameManager Instance { get; private set; }

    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;
    public event EventHandler OnStateChanged;

    private bool isGamePaused = false;
    private State state;

    private void Awake()
    {
        Instance = this;
        
        state = State.WaitingToStart;
    }

    private void Start()
    {
        PlayerStateMachine.Instance.OnPause += PlayerStateMachine_OnPause;
    }

    private void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:
                state = State.GamePlaying;
                OnStateChanged?.Invoke(this, EventArgs.Empty);
                break;
            case State.GamePlaying:
                if (CollectibleCount.Instance.IsAllCollected())
                {
                    state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                break;
        }
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
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;
    }

    public bool IsGameOver()
    {
        return state == State.GameOver;
    }
}
