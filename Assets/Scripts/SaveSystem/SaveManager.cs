using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    public void SaveGame()
    {
        SaveSystem.SavePlayerProgress(_GM,_FM);
        SaveSystem.SaveStaffData(_SM);
        
    }

    public void NewSaveFile()
    {

    }

    public void LoadGame()
    {
        PlayerSaveData playerData = SaveSystem.LoadPlayerProgress();

        _GM.dayCount = playerData.day;
        _GM.resturantName = playerData.resturantName;
        _GM.resturantRating = playerData.resturantRating;
        _GM.playerLevel = playerData.level;
        _GM.currentPlayerEXP = playerData.currentExp;
        _GM.money = playerData.money;

        _FM.protienTotal_produce = playerData.produce_protein;
        _FM.grainTotal_produce = playerData.produce_grain;
        _FM.dairyTotal_produce = playerData.produce_diary;
        _FM.vegTotal_produce = playerData.produce_veg;
        _FM.fruitTotal_produce = playerData.produce_fruit;

        //Load Staff Data

        StaffSaveData staffSaveData = SaveSystem.LoadStaffData();

        staffSaveData.AssignStaffID();

        foreach (var item in staffSaveData.staffStrArrays)
        {
            var staffData = ReturnStaffBasedOnID(item[0]);

            print(item[0] + " " + item[1] +" "+ item[2] + " " + item[3]);
            //hired
            if (item[1] == "True") _SM.totalHiredStaff.Add(staffData.gameObject);
            else if (item[1] == "False") if (_SM.totalHiredStaff.Contains(staffData.gameObject))
                    _SM.totalHiredStaff.Remove(staffData.gameObject);
            //active
            if (item[2] == "True") _SM.ActivateStaffOnLoad(staffData.gameObject);
            else if (item[1] == "False") _SM.DeactivateStaff(staffData.gameObject);

            staffData.friendshipLevel = int.Parse(item[3]);
        }


        

    }


    StaffData ReturnStaffBasedOnID(string ID)
    {
        foreach (var staff in _SM.allStaffData)
        {
            if (staff.ID == ID)
                return staff;
        }

        return null;
    }
}
