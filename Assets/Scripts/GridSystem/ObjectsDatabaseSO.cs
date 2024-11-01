using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ObjectsDatabaseSO : ScriptableObject
{
    public List<ObjectData> objectsData;
    
}

[Serializable]
public class ObjectData
{
    //this allows it do be displayed in inspector. setter is private so nothing outside the class can define it
    [field: SerializeField]
    public string name { get; set; }
    [field: SerializeField]
    //each item will have a unquie ID
    public int ID { get; set; }
    [field: SerializeField]
    public Vector2 size { get; set; }
    [field: SerializeField]
    public GameObject prefab { get; set; }
}