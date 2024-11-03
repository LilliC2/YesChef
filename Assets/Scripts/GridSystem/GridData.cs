using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData 
{
    //need to add save for this
    Dictionary<Vector3Int, PlacementData> placedObjects = new();

    /// <summary>
    /// Add object to dictonary
    /// </summary>
    /// <param name="gridPostion"></param>
    /// <param name="objectSize"></param>
    /// <param name="ID"></param>
    /// <param name="placedObjectIndex"></param>
    /// <exception cref="Exception"></exception>
    public void AddObjectAt(Vector3Int gridPostion, Vector2Int objectSize, int ID, int placedObjectIndex)
    {
        List<Vector3Int> positionsToOccupy = CalculatePositions(gridPostion, objectSize);

        PlacementData data = new PlacementData(positionsToOccupy, ID, placedObjectIndex);

        foreach (var pos in positionsToOccupy)
        {
            //check if dictonary does not contain
            if (placedObjects.ContainsKey(pos))
                throw new Exception($"Dictonary already contrains this cell position {pos}");
            placedObjects[pos] = data;
        }

    }

    /// <summary>
    /// Calculate positions of object depending on size of object
    /// </summary>
    /// <param name="gridPostion"></param>
    /// <param name="objectSize"></param>
    /// <returns></returns>
    private List<Vector3Int> CalculatePositions(Vector3Int gridPostion, Vector2Int objectSize)
    {
        List<Vector3Int> returnVal = new();

        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                returnVal.Add(gridPostion + new Vector3Int(x,0,y));
            }
        }

        return returnVal;
    }

    /// <summary>
    /// Check if any existing objets occupy desired space
    /// </summary>
    /// <param name="gridPostion"></param>
    /// <param name="objectSize"></param>
    /// <returns></returns>
    public bool CanPlaceObjectAt(Vector3Int gridPostion, Vector2Int objectSize)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPostion, objectSize);

        foreach (var pos in positionToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
                return false;
        }

        return true;
    }
}

public class PlacementData
{
    public List<Vector3Int> occupiedPosition;

    public int ID { get; set; }

    public int placeObjectIndex { get; set; }

    //constructor
    public PlacementData(List<Vector3Int> occupiedPosition, int iD, int placeObjectIndex)
    {
        this.occupiedPosition = occupiedPosition;
        ID = iD;
        this.placeObjectIndex = placeObjectIndex;
    }
}
