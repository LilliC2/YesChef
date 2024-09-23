using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSaveData 
{
    public int day;
    public int level;
    public int currentExp;
    public float resturantRating;
    public int resturantLevel;
    public string resturantName;

    public string[] menuOrderIDS;


    public PlayerSaveData(GameManager gameManager, FoodManager foodManager)
    {
        day = gameManager.dayCount;
        level = gameManager.playerLevel;
        currentExp = gameManager.currentPlayerEXP;
        resturantRating = gameManager.resturantRating;
        resturantName = gameManager.resturantName;
        
        menuOrderIDS = new string[foodManager.menu.Count];

        for (int i = 0; i < menuOrderIDS.Length; i++)
        {
            menuOrderIDS[i] = foodManager.menu[i].GetComponent<FoodData>().order.orderID;
        }

        //resturantLevel = gameManager.res not added yet
    }
    
}

public class StaffSaveData
{
    public int[] ID_001; //tanuki

    /* 0 = ID
     * 1 = Hired (bool 0,1)
     * 2 = Active (bool 0,1)
     * 3 friendship level
     */


    public StaffSaveData(StaffData staffData)
    {

    }
}




