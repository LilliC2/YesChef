using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : GameBehaviour
{
    [SerializeField]
    Camera sceneCamera;
    Vector3 lastPostion;
    [SerializeField]
    LayerMask placementLayerMask;

    public event Action OnClicked, OnExit;

    private void Update()
    {
         if(Input.GetMouseButtonDown(0))
            OnClicked?.Invoke();
         if(Input.GetKey(KeyCode.Escape))
            OnExit?.Invoke();

    }

    //Returns true or false if pointer is over gameobejct, so the above does not affect UI
    public bool IsPointerOverUI()
        => EventSystem.current.IsPointerOverGameObject();

    /// <summary>
    /// Get mouse position via camera raycast
    /// </summary>
    /// <returns></returns>
    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit,100, placementLayerMask))
        {
            lastPostion = hit.point;

        }
        return lastPostion;

    }

}
