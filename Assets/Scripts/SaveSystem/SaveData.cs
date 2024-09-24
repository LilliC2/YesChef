using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public class PlayerSaveData 
{
    public int day;
    public int level;
    public int currentExp;
    public float resturantRating;
    public int resturantLevel;
    public float money;
    public string resturantName;

    public string[] menuOrderIDS;

    public int produce_diary;
    public int produce_protein;
    public int produce_grain;
    public int produce_veg;
    public int produce_fruit;


    public PlayerSaveData(GameManager gameManager, FoodManager foodManager)
    {
        day = gameManager.dayCount;
        level = gameManager.playerLevel;
        currentExp = gameManager.currentPlayerEXP;
        resturantRating = gameManager.resturantRating;
        resturantName = gameManager.resturantName;
        money = gameManager.money;
        
        menuOrderIDS = new string[foodManager.menu.Count];
        produce_diary = foodManager.dairyTotal_produce;
        produce_fruit = foodManager.fruitTotal_produce;
        produce_grain = foodManager.grainTotal_produce;
        produce_protein = foodManager.protienTotal_produce;
        produce_veg = foodManager.vegTotal_produce;

        for (int i = 0; i < menuOrderIDS.Length; i++)
        {
            menuOrderIDS[i] = foodManager.menu[i].GetComponent<FoodData>().order.orderID;
        }

        //resturantLevel = gameManager.res not added yet
    }
    

}

[System.Serializable]
public class StaffSaveData
{
    public List<string[]> staffStrArrays = new List<string[]>();
    string[] ID_001 = new string[] { "001", "0", "0", "0" }; //tanuki/temp chef
    string[] ID_002 = new string[] { "002", "0", "0", "0" }; //shiba/temp waiter

    /* 0 = ID
     * 1 = Hired (bool 0,1)
     * 2 = Active (bool 0,1)
     * 3 friendship level
     */

    public void AssignStaffID()
    {
        staffStrArrays.Add(ID_001);
        staffStrArrays.Add(ID_002);
    }

    public string[] GetStaffInArrayOnID(string id)
    {

        foreach (var item in staffStrArrays)
        {
            if (item[0] == id)
                return item;
        }

        return null;
    }
    public StaffSaveData(List<StaffData> staffData, StaffManager staffManager)
    {
        //get storage int
        AssignStaffID();


        foreach (var item in staffData)
        {
            var storageStrArray = GetStaffInArrayOnID(item.ID);

            Debug.Log(storageStrArray);

            if (storageStrArray != null)
            {
                storageStrArray[1] = staffManager.totalHiredStaff.Contains(item.gameObject) ? "True" : "False";
                storageStrArray[2] = staffManager.totalActiveStaff.Contains(item.gameObject) ? "True" : "False";
                storageStrArray[3] = item.friendshipLevel.ToString();
            }
        }
        
    }
}




