using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlacementSystem : GameBehaviour
{

    [SerializeField]
    InputManager inputManager;
    [SerializeField]
    Grid grid;

    [SerializeField]
    ObjectsDatabaseSO databaseSO;

    [SerializeField]
    GameObject gridVisualisation;

    //two different ones so that you can place furniture on the floor
    GridData floorData, furnitureData;

    [SerializeField]
    PreviewSystem preview;

    Vector3Int lastDetectionPosition = Vector3Int.zero;
    [SerializeField] ObjectPlacer objectPlacer;

    IBuildingState buildingState;

    private void Start()
    {
        StopPlacement();
        floorData = new();
        furnitureData = new();
    }

    public void PlaceInEditor(int ID, GameObject calledObject)
    {
        buildingState = new PlacementState(ID,
                                           calledObject.transform.eulerAngles.y, //set rotation
                                           grid,
                                           preview,
                                           databaseSO,
                                           floorData,
                                           furnitureData,
                                           objectPlacer,
                                           true);

        Vector3Int gridPosition = grid.WorldToCell(calledObject.transform.position); //get cell position

        buildingState.OnPlaceInEditor(ID,gridPosition);
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        gridVisualisation.SetActive(true);

        buildingState = new PlacementState(ID,
                                           0f, //default rotation
                                           grid,
                                           preview,
                                           databaseSO,
                                           floorData,
                                           furnitureData,
                                           objectPlacer,
                                           false);
        
        //Add listeners
        inputManager.OnClicked += () => PlaceStructure();
        inputManager.OnExit += () => StopPlacement();
        inputManager.OnRotate += () => preview.RotatePreview(buildingState);
    }

    private void PlaceStructure()
    {
        if (buildingState == null) return;
        if(inputManager.IsPointerOverUI()) return;

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition); //get cell position

        preview.ResetPreviewRotation();
        buildingState.OnAction(gridPosition);
    }

    private void StopPlacement()
    {
        if (buildingState == null) return;
        gridVisualisation.SetActive(false);
        buildingState.EndState();
        
        //Remove listeners
        inputManager.OnClicked -= () => PlaceStructure();
        inputManager.OnExit -= () => StopPlacement();
        inputManager.OnRotate -= () => preview.RotatePreview(buildingState);
        print("Removed listeners");
        lastDetectionPosition = Vector3Int.zero;
        buildingState = null;

    }

    // Update is called once per frame
    void Update()
    {
        if(buildingState == null) //if not building
            return;
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition); //get cell position
        if(lastDetectionPosition != gridPosition)
        {
            buildingState.UpdateState(gridPosition);
            lastDetectionPosition = gridPosition;   
        }
      
    }
}
