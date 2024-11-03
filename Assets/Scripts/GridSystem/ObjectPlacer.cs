using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    List<GameObject> placedGameObjects = new();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="position"></param>
    /// <returns>Index of object in placedGameObjects</returns>
    internal int PlaceObject(GameObject prefab, Vector3 position)
    {
        GameObject structure = Instantiate(prefab);
        structure.transform.position = position; //covert back to world pos
        placedGameObjects.Add(structure);

        return placedGameObjects.Count - 1;
    }
}
