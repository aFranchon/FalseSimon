using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickCommand : ICommand
{
    #region Fields
    public readonly string cubeName;
    private MeshRenderer cube;
    private Color currentColor;
    private Color lastColor;
    #endregion

    #region Constructors
    public ClickCommand(MeshRenderer cube, Color color, string cubeName)
    {
        this.cubeName = cubeName;
        this.cube = cube;
        this.currentColor = color;
    }
    #endregion

    #region Command Design Pattern implementation
    public void Execute()
    {
        lastColor = cube.material.color;
        cube.material.color = currentColor;
    }

    public void Undue()
    {
        cube.material.color = lastColor;
    }
    #endregion
}
