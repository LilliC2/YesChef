using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class EditorPlacer : MonoBehaviour
{
    public int objectID = 123;
    [SerializeField]
    PlacementSystem placementSystem;
    [ContextMenu("Place Item")]
    public void Place()
    {
        placementSystem.PlaceInEditor(objectID, gameObject);
    }

   
}
