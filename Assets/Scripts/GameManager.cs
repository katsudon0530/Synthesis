using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.CullingGroup;

public enum GameState
{
    title, 
    costom,
    singleGame,
    dungeon,
    result,
}

public class GameManager : Singleton<GameManager>
{
    protected override bool IsPersistent => true;

    public static GameState gameState { get; private set; }
    public static event Action<GameState> OnStateChanged;

    protected override void Awake()
    {
        base.Awake();

        OnStateChanged += SceneChange;
    }

    public static void ChangeState(GameState newState)
    {
        if (gameState != newState)
        {
            gameState = newState;
            OnStateChanged?.Invoke(newState);
        }
    }

    private void SceneChange(GameState newState)
    {
        switch (newState)
        {
            case GameState.title:
                SceneManager.LoadScene("TitleScene");
                break;
            case GameState.costom:
                SceneManager.LoadScene("CustomScene");
                break;
            case GameState.singleGame:
                SceneManager.LoadScene("GameScene");
                break;
            case GameState.dungeon:
                SceneManager.LoadScene("DungeonScene");
                break;

        }
    }

    private void OnDestroy()
    {
        OnStateChanged -= SceneChange;
    }
}
