using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField]
    float previewYOffset = 0.06f;

    [SerializeField]
    GameObject cellIndicator;
    GameObject previewGameObject;

    [SerializeField]
    Material previewMaterialPrefab;
    Material previewMaterialInstance; //instante when you start game so you dont edit material in project

    Renderer cellIndicatorRenderer;
    IBuildingState buildingState;


    private void Start()
    {
        previewMaterialInstance = new Material(previewMaterialPrefab);
        cellIndicator.SetActive(false);
        cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }

    public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size)
    {
        previewGameObject = Instantiate(prefab);
        PreparePreview(previewGameObject);
        PrepareCursor(size);
        cellIndicator.SetActive(true);
    }

    /// <summary>
    /// Increase size of cell indicator
    /// </summary>
    /// <param name="size"></param>
    private void PrepareCursor(Vector2Int size)
    {
        if(size.x > 0 || size.y > 0)
        {
            cellIndicator.transform.localScale = new Vector3(size.x,1,size.y);
            cellIndicatorRenderer.material.mainTextureScale = size;
        }
    }

    /// <summary>
    /// Set preview materials
    /// </summary>
    /// <param name="previewGameObject"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void PreparePreview(GameObject previewGameObject)
    {
        Renderer[] renderers = previewGameObject.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            //change each material to transperent material
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = previewMaterialInstance;
            }
            renderer.materials = materials;
        }

    }

    public void RotatePreview(IBuildingState buildingState)
    {
        //get gameobject
        Transform previewChild = previewGameObject.transform.GetChild(0);

        float rotateEnd = previewChild.rotation.eulerAngles.y == 270 ? 0 : previewChild.rotation.eulerAngles.y + 90;

        previewChild.DORotate(new Vector3(0, rotateEnd, 0), 1);
        buildingState.RotateStructure(rotateEnd);
    }

    public void ResetPreviewRotation()
    {
        Transform previewChild = previewGameObject.transform.GetChild(0);
        previewChild.rotation = Quaternion.identity;
    }

    public void StopShowingPreview()
    {

        cellIndicator.SetActive(false );
        Destroy( previewGameObject );
    }

    public void UpdatePosition(Vector3 position, bool validity)
    {
        MovePreview(position);
        MoveCursor(position);
        ApplyFeedback(validity);
    }

    private void MoveCursor(Vector3 position)
    {
        cellIndicator.transform.position = position;
    }

    private void ApplyFeedback(bool validity)
    {
        Color c = validity ? Color.white : Color.red;
        cellIndicatorRenderer.material.color = c;
        c.a = 0.5f; //set alpha
        previewMaterialInstance.color = c; //this will affect alll renderers

    }

    private void MovePreview(Vector3 position)
    {
        previewGameObject.transform.position = new Vector3(position.x,position.y + previewYOffset,position.z);

    }
}
