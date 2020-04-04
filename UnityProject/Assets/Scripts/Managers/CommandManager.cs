using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CommandManager : MonoBehaviour
{
    #region Fields
    public readonly List<ICommand> commandsList = new List<ICommand>();
    #endregion

    #region Command manager minimal implementation
    public void AddCommand(ICommand command)
    {
        commandsList.Add(command);
    }

    public void ClearList()
    {
        commandsList.Clear();
    }

    public IEnumerator PlayList()
    {
        foreach (var command in commandsList)
        {
            command.Execute();
            yield return new WaitForSeconds(1);
        }
    }

    public IEnumerator RewindList()
    {
        foreach (var command in Enumerable.Reverse(commandsList))
        {
            command.Execute();
            yield return new WaitForSeconds(1);
        }
    }
    #endregion
}
