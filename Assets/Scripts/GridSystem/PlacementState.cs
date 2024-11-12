using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlacementState : IBuildingState
{
    int selectedObjectIndex = -1;
    int ID;
    float rotation;
    Grid grid;
    PreviewSystem previewSystem;
    ObjectsDatabaseSO database;
    GridData floorData;
    GridData furnitureData;
    ObjectPlacer objectPlacer;

    public PlacementState(int iD,
                          float rotation,
                          Grid grid,
                          PreviewSystem previewSystem,
                          ObjectsDatabaseSO database,
                          GridData floorData,
                          GridData furnitureData,
                          ObjectPlacer objectPlacer,
                          bool inEditor)
    {
        ID = iD;
        this.rotation = rotation;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.floorData = floorData;
        this.furnitureData = furnitureData;
        this.objectPlacer = objectPlacer;


        //like using a for loop to find the index, if it equals data.ID return ID
        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex > -1)
        {
            if(!inEditor) previewSystem.StartShowingPlacementPreview(database.objectsData[selectedObjectIndex].prefab,
                database.objectsData[selectedObjectIndex].size);

        }
        else
            throw new System.Exception($"No object with int ID {iD}");
        //Add listeners

    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();

    }
    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {

        //check ID to find which data base we need, if there are more floors we need further 
        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;

        return selectedData.CanPlaceObjectAt(gridPosition, database.objectsData[selectedObjectIndex].size);
    }

    public void OnAction(Vector3Int gridPosition)
    {
        //check if valid placement
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

        if (!placementValidity) return;

        int index = objectPlacer.PlaceObject(database.objectsData[selectedObjectIndex].prefab, grid.CellToWorld(gridPosition), rotation);

        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;
        selectedData.AddObjectAt(gridPosition, //add to dictonary
            database.objectsData[selectedObjectIndex].size,
            database.objectsData[selectedObjectIndex].ID,
            index);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }

    public void OnPlaceInEditor(int ID, Vector3Int gridPosition)
    {

        int index = objectPlacer.PlaceObject(database.objectsData[selectedObjectIndex].prefab, grid.CellToWorld(gridPosition), rotation);

        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? floorData : furnitureData; //will need to change this to check if its floor as floor will have multiple possible IDs

        Debug.Log($"index {index}");

        selectedData.AddObjectAt(gridPosition, //add to dictonary
            database.objectsData[selectedObjectIndex].size,
            database.objectsData[selectedObjectIndex].ID,
            index);


    }


    public void UpdateState(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
    }
    public void RotateStructure(float rotation)
    {
        this.rotation = rotation;
    }

}
