using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    #region Enums
    private enum buttonOrder
    {
        Play,
        Clue,
        Restart,
        Start
    }
    #endregion

    #region Fields
    [SerializeField] private Button[] buttons;
    #endregion

    #region Initiliaze Object
    private void Start()
    {
        GameLogicManager.Instance.onPlayButtonClick += onPlayButtonClicked;
        GameLogicManager.Instance.onClueButtonClick += onClueButtonClick;
        GameLogicManager.Instance.hasPlayerWon += onGameEnd;
    }
    #endregion

    #region Subscribers to gameLogic button clicked Actions
    private void onPlayButtonClicked()
    {
        buttons[(int)buttonOrder.Play].interactable = false;
        buttons[(int)buttonOrder.Start].interactable = false;
        buttons[(int)buttonOrder.Restart].interactable = false;
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
        buttons[(int)buttonOrder.Start].interactable = true;
        buttons[(int)buttonOrder.Restart].interactable = true;
        buttons[(int)buttonOrder.Clue].interactable = true;
    }
    #endregion
}
