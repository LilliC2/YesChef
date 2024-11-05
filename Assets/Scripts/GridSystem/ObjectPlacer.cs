using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField]
    List<GameObject> placedGameObjects = new();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="position"></param>
    /// <returns>Index of object in placedGameObjects</returns>
    public int PlaceObject(GameObject prefab, Vector3 position, float rotation)
    {
        GameObject structure = Instantiate(prefab);
        structure.transform.position = position; //covert back to world pos
        structure.transform.GetChild(0).eulerAngles = new Vector3(0,rotation, 0); //set rotation
        placedGameObjects.Add(structure);

        return placedGameObjects.Count - 1;
    }
}
