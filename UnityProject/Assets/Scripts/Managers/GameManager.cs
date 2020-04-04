using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class GameManager : Singleton<GameManager>
{
    #region Enums
    public enum GameState
    {
        MainMenu,
        Running,
        Paused
    }
    #endregion

    #region Fields
    [SerializeField] private GameObject[] importantSingletonToInstatiateAtStart;

    private List<AsyncOperation> aoListLoad;
    private List<AsyncOperation> aoListUnload;

    private GameState currentState = GameState.MainMenu;

    [HideInInspector] public string currentLevel = string.Empty;
    #endregion

    #region Events
    public Action<GameState, GameState> onChangeState;

    public Action<string> onSceneLoadComplete;
    public Action<string> onSceneUnloadedComplete;

    public Action onAllSceneLoaded;
    public Action onAllSceneUnloaded;
    #endregion

    #region Initialize Object
    protected override void Awake()
    {
        DontDestroyOnLoad(this);

        base.Awake();

        aoListLoad = new List<AsyncOperation>();
        aoListUnload = new List<AsyncOperation>();

        foreach (var importantSingleton in importantSingletonToInstatiateAtStart)
        {
            Instantiate(importantSingleton);
        }
    }
    #endregion

    #region Scene Load handling
    private void onSceneLoaded(AsyncOperation ao)
    {
        if (!aoListLoad.Contains(ao))
        {
            Debug.LogError("Invalid call to this function, ao is not in the list should not happen");
            return;
        }

        aoListLoad.Remove(ao);

        onSceneLoadComplete?.Invoke(currentLevel);

        if (aoListLoad.Count == 0)
        {
            onAllSceneLoaded?.Invoke();
        }
    }

    public void LoadScene(string sceneName)
    {
        var ao = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        ao.completed += onSceneLoaded;

        currentLevel = sceneName;

        aoListLoad.Add(ao);
    }
    #endregion

    #region Scene Unload handling
    public void onSceneUnloaded(AsyncOperation ao)
    {
        if (!aoListUnload.Contains(ao))
        {
            Debug.LogError("Invalid call to this function, ao is not in the list should not happen");
            return;
        }

        aoListUnload.Remove(ao);

        onSceneUnloadedComplete?.Invoke(currentLevel);

        if (aoListUnload.Count == 0)
        {
            onAllSceneUnloaded?.Invoke();
        }
    }

    public void UnloadScene(string sceneName)
    {
        var ao = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        ao.completed += onSceneUnloaded;

        aoListUnload.Add(ao);
    }
    #endregion

    #region StateHandling
    public void ChangeState(GameState newState)
    {
        var oldState = currentState;
        currentState = newState;

        onChangeState?.Invoke(newState, oldState);
    }
    #endregion
}
