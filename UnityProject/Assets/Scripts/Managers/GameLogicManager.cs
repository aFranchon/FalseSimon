using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameLogicManager : Singleton<GameLogicManager>
{
    #region Enums
    private enum cubeOrder
    {
        Top,
        Right,
        Bottom,
        Left
    }
    #endregion

    #region Fields
    [SerializeField] private MeshRenderer[] cubesMesh;
    [SerializeField] private CommandManager commandManager;
    [SerializeField] private CubeBehaviour cubeBehaviour;
    
    //index of the number of clicked done by the player
    private int index = 0;
    [SerializeField] private int numberOfCommands = 3;
    #endregion

    #region Events
    //buttons events
    [HideInInspector] public Action onPlayButtonClick;
    [HideInInspector] public Action onClueButtonClick;
    [HideInInspector] public Action onStartButtonClick;
    [HideInInspector] public Action onRestartButtonClick;

    //end of game event
    [HideInInspector] public Action<bool> hasPlayerWon;
    #endregion

    #region Initilize Object
    protected override void Awake()
    {
        base.Awake();

        UIManager.Instance.playGameButton += PlayGameButton;
        UIManager.Instance.clueButton += ClueButton;
    }

    private void Start()
    {
        cubeBehaviour.onCubeClicked += onCubeClicked;
        hasPlayerWon += onGameEnded;
    }
    #endregion

    #region ButtonHandling
    public void PlayGameButton()
    {
        commandManager.ClearList();

        for (var i = 0; i < numberOfCommands; i++)
        {
            var meshNumber = UnityEngine.Random.Range(0, cubesMesh.Length);

            var mesh = cubesMesh[meshNumber];
            commandManager.AddCommand(new ClickCommand(
                mesh,
                new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value),
                mesh.gameObject.name));
        }

        StartCoroutine(commandManager.PlayList());
        StartCoroutine(ClearGame((float)numberOfCommands + 0.5f));

        onPlayButtonClick?.Invoke();
    }

    public void ClueButton()
    {
        StartCoroutine(commandManager.RewindList());
        StartCoroutine(ClearGame((float)numberOfCommands + 0.5f));

        onClueButtonClick?.Invoke();
    }

    public IEnumerator ClearGame(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);

        foreach (var mesh in cubesMesh)
        {
            mesh.material.color = Color.white;
        }
    }
    #endregion

    #region gameLogic methods
    private void onCubeClicked(ICommand clickCommand)
    {
        var listCommand = (ClickCommand)commandManager.commandsList[index];
        var playerCommand = (ClickCommand)clickCommand;

        if (listCommand.cubeName == playerCommand.cubeName)
        {
            commandManager.commandsList[index].Execute();
            index++;

            if (index == numberOfCommands)
            {
                hasPlayerWon?.Invoke(true);
            }
        }
        else
        {
            hasPlayerWon?.Invoke(false);
        }
    }

    private void onGameEnded(bool hasPlayerWin)
    {
        index = 0;
        StartCoroutine(ClearGame(1f));
    }
    #endregion

    #region Destroy Object
    protected override void OnDestroy()
    {
        base.OnDestroy();

        UIManager.Instance.playGameButton -= PlayGameButton;
        UIManager.Instance.clueButton -= ClueButton;
    }
    #endregion
}
