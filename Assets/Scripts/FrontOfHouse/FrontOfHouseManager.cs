using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FrontOfHouseManager : Singleton<FrontOfHouseManager>
{
    public List<GameObject> unoccupiedTables = new List<GameObject>();
    public List<GameObject> dirtyTables = new List<GameObject>();

    public int numOfChairs;
    // Start is called before the first frame update
    void Start()
    {
        //get all objects with table scripts
        var tables = (GameObject.FindGameObjectsWithTag("Table")).ToList();
        unoccupiedTables = (tables).ToList();
        
    }

    public void ChangeToOccupied(GameObject _table)
    {
        _table.GetComponent<Table>().status = Table.Status.Occupied;
        if (unoccupiedTables.Contains(_table)) unoccupiedTables.Remove(_table);

    }

    public void ChangeToUnoccupied(GameObject _table)
    {
        _table.GetComponent<Table>().status = Table.Status.Unoccupied;
        _table.GetComponent<Table>().UnoccupiedTableReset();
        if(!unoccupiedTables.Contains(_table) ) unoccupiedTables.Add(_table);

    }
    
    public void ChangeToDirty(GameObject _table)
    {
        _table.GetComponent<Table>().status = Table.Status.Dirty;
        if(!dirtyTables.Contains(_table) ) unoccupiedTables.Add(_table);
        if (unoccupiedTables.Contains(_table)) unoccupiedTables.Remove(_table);

    }

    public GameObject FindClosestTable(GameObject _waiter)
    {
        float distance = 10000;
        GameObject closestTable = null;

        foreach (var table in unoccupiedTables) 
        {
            if(Vector3.Distance(_waiter.transform.position, table.transform.position) < distance)
            {
                distance = Vector3.Distance(_waiter.transform.position, table.transform.position);
                closestTable = table;
            }
        }

        return closestTable;
    }
}
