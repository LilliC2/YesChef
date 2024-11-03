using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlacementSystem : GameBehaviour
{
    [SerializeField]
    GameObject mouseIndicator;
    [SerializeField]
    InputManager inputManager;
    [SerializeField]
    Grid grid;

    [SerializeField]
    ObjectsDatabaseSO databaseSO;
    int selectedObjectIndex = -1; //-1 means nothing is selected

    [SerializeField]
    GameObject gridVisualisation;

    //two different ones so that you can place furniture on the floor
    GridData floorData, furnitureData;

    List<GameObject> placedGameObjects = new();
    [SerializeField]
    PreviewSystem preview;

    Vector3Int lastDetectionPosition = Vector3Int.zero;

    private void Start()
    {
        StopPlacement();
        floorData = new();
        furnitureData = new();
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();


        //like using a for loop to find the index, if it equals data.ID return ID
        selectedObjectIndex = databaseSO.objectsData.FindIndex(data => data.ID == ID);
        if(selectedObjectIndex < 0)
        {
            //returns -1 if not reutrning an object
            Debug.LogError($"No ID found {ID}");
            return;
        }

        gridVisualisation.SetActive(true);
        preview.StartShowingPlacementPreview(databaseSO.objectsData[selectedObjectIndex].prefab,
            databaseSO.objectsData[selectedObjectIndex].size);
        //Add listeners

        inputManager.OnClicked += () => PlaceStructure();
        inputManager.OnExit += () => StopPlacement();
    }

    private void PlaceStructure()
    {
        if(inputManager.IsPointerOverUI())
        {
            return;
        }
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition); //get cell position

        //check if valid placement
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

        if (!placementValidity) return;

        //add audio call here

        GameObject structure = Instantiate(databaseSO.objectsData[selectedObjectIndex].prefab);
        structure.transform.position = grid.CellToWorld(gridPosition); //covert back to world pos
        placedGameObjects.Add(structure);
        GridData selectedData = databaseSO.objectsData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;
        selectedData.AddObjectAt(gridPosition, //add to dictonary
            databaseSO.objectsData[selectedObjectIndex].size,
            databaseSO.objectsData[selectedObjectIndex].ID,
            placedGameObjects.Count-1); 

        preview.UpdatePosition(grid.CellToWorld(gridPosition),false);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        
        //check ID to find which data base we need, if there are more floors we need further 
        GridData selectedData = databaseSO.objectsData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;

        return selectedData.CanPlaceObjectAt(gridPosition, databaseSO.objectsData[selectedObjectIndex].size);
    }

    private void StopPlacement()
    {
        selectedObjectIndex = -1;

        gridVisualisation.SetActive(false);
        preview.StopShowingPreview();
        //Remove listeners
        inputManager.OnClicked -= () => PlaceStructure();
        inputManager.OnExit -= () => StopPlacement();
        lastDetectionPosition = Vector3Int.zero;

    }

    // Update is called once per frame
    void Update()
    {
        if(selectedObjectIndex < 0)
            return;
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition); //get cell position
        if(lastDetectionPosition != gridPosition)
        {
            bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);


            mouseIndicator.transform.position = mousePosition;
            preview.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
            lastDetectionPosition = gridPosition;   
        }
      
    }
}
