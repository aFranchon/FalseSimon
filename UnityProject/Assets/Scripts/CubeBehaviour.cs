using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CubeBehaviour : MonoBehaviour
{
    #region Fields
    [SerializeField] private CommandManager commandManager;
    #endregion

    #region Events
    [HideInInspector] public Action<ICommand> onCubeClicked;
    #endregion

    #region Repetitive Methods
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //getting the origin of the ray (position and direction we want the ray to point)
            Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);
            //this will be the ref to get THe gameobject that hit the ray
            RaycastHit hitInfos;

            //cast the ray to see if he points to an object (not passing distance param means very large distance i assume)
            if (Physics.Raycast(rayOrigin, out hitInfos))
            {
                if (hitInfos.collider.tag == "Cube")
                {
                    ICommand clickCommand = new ClickCommand(
                        hitInfos.collider.gameObject.GetComponent<MeshRenderer>(), 
                        new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value),
                        hitInfos.collider.name);

                    onCubeClicked?.Invoke(clickCommand);
                }
            }
        }
    }
    #endregion
}
