using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlacementSystem : GameBehaviour
{
    [SerializeField]
    GameObject mouseIndicator, cellIndicator;
    [SerializeField]
    InputManager inputManager;
    [SerializeField]
    Grid grid;

    [SerializeField]
    ObjectsDatabaseSO databaseSO;
    int selectedObjectIndex = -1; //-1 means nothing is selected

    [SerializeField]
    GameObject gridVisualisation;

    private void Start()
    {
        StopPlacement();
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
        cellIndicator.SetActive(true);
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

        //add audio call here

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition); //get cell position
        GameObject structure = Instantiate(databaseSO.objectsData[selectedObjectIndex].prefab);
        structure.transform.position = grid.CellToWorld(gridPosition); //covert back to world pos
    }

    private void StopPlacement()
    {
        selectedObjectIndex = -1;

        gridVisualisation.SetActive(false);
        cellIndicator.SetActive(false);
        //Remove listeners
        inputManager.OnClicked -= () => PlaceStructure();
        inputManager.OnExit -= () => StopPlacement();

    }

    // Update is called once per frame
    void Update()
    {
        if(selectedObjectIndex < 0)
            return;
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition); //get cell position
        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition); //covert back to world pos
    }
}
