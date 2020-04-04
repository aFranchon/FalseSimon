using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : Singleton<UIManager>
{
    #region Enums
    private enum buttonOrder
    {
        Play,
        Clue
    }
    #endregion

    #region Fields
    [SerializeField] private Button[] buttons;

    [SerializeField] private GameObject menuCamera;

    [SerializeField] private GameObject menuUI;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject pauseUI;
    #endregion

    #region Events
    public Action playGameButton;
    public Action clueButton;
    #endregion

    #region Initiliaze Object
    private void Start()
    {
        playGameButton += onPlayButtonClicked;
        clueButton += onClueButtonClick;

        GameManager.Instance.onChangeState += onChangeState;
        GameManager.Instance.onSceneLoadComplete += onSceneLoaded;
    }
    #endregion

    #region Subscribers for gameLogic button clicked Actions
    private void onPlayButtonClicked()
    {
        buttons[(int)buttonOrder.Play].interactable = false;
        buttons[(int)buttonOrder.Clue].interactable = true;
    }

    private void onClueButtonClick()
    {
        buttons[(int)buttonOrder.Clue].interactable = false;
    }
    #endregion

    #region Subsriber for gameLogic end of game
    private void onGameEnd(bool hasPlayerWin)
    {
        buttons[(int)buttonOrder.Play].interactable = true;
        buttons[(int)buttonOrder.Clue].interactable = true;
    }
    #endregion

    #region Subscribers for state handling of the GameManager
    private void onChangeState(GameManager.GameState newState, GameManager.GameState oldState)
    {
        if (newState == GameManager.GameState.Running && oldState == GameManager.GameState.MainMenu)
        {
            //code for a transition between state
            menuUI.SetActive(false);
            gameUI.SetActive(true);
        }
        else if (newState == GameManager.GameState.MainMenu && oldState == GameManager.GameState.Running)
        {
            menuUI.SetActive(true);
            gameUI.SetActive(false);
        }
    }
    #endregion

    #region Subscribers for scene handling of the GameManager
    private void onSceneLoaded(string sceneName)
    {
        if (sceneName == "GameScene")
        {
            Debug.Log("GameScene Loading, subscribing to the click methods of the GameLogicManager");
            menuCamera.SetActive(false);
            GameLogicManager.Instance.hasPlayerWon += onGameEnd;
        }
        else if (sceneName == "StartScene")
        {
            menuCamera.SetActive(true);
        }
    }
    #endregion

    #region MenuClick handling
    public void OnPlayButton()
    {
        Debug.Log("Button click");
        GameManager.Instance.ChangeState(GameManager.GameState.Running);
        GameManager.Instance.LoadScene("GameScene");
    }
    #endregion

    #region GameClick handling
    public void OnPlayGameClick()
    {
        playGameButton?.Invoke();
    }

    public void OnClueClick()
    {
        clueButton?.Invoke();
    }
    #endregion
}
