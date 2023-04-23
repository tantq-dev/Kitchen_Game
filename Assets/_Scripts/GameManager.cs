using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePause;
    public event EventHandler OnGameUnpause;
    [SerializeField] private float gamePlayingTimerMax = 20f;

    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
    }
    private State state;

    private float countdownToStartTimer = 3f;
    private float gamePlayingTimer;
    private bool isGamePause = false;

    private void Awake()
    {
        Instance = this;
        state = State.WaitingToStart;
    }
    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;

    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if(state==State.WaitingToStart)
        {
            state = State.CountdownToStart;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {

        PauseGame();
    }

    private void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:
                break;
            case State.CountdownToStart:
                countdownToStartTimer -= Time.deltaTime;
                if (countdownToStartTimer < 0)
                {
                    gamePlayingTimer = gamePlayingTimerMax;
                        state = State.GamePlaying;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);

                }

                break;
            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;

                if (gamePlayingTimer < 0)
                {
                    state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);

                }
                break;
            case State.GameOver:
                break;
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
    public bool IsCountdownToStartActive()
    {
        return state == State.CountdownToStart;
    }
    public float GetCountdownToStartTimer()
    {
        return countdownToStartTimer;
    }
    public float GetGameplayTimerNormalized()
    {
        return 1-(gamePlayingTimer / gamePlayingTimerMax);
    }
    public void PauseGame()
    {
        if (!isGamePause)
        {
            Time.timeScale = 0f;
            OnGamePause?.Invoke(this, EventArgs.Empty);

        }
        else
        {
            Time.timeScale = 1f;
            OnGameUnpause?.Invoke(this, EventArgs.Empty);


        }
        isGamePause = !isGamePause;
    }
}
