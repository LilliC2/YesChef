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

    public void LoadGame()
    {
        PlayerSaveData playerData = SaveSystem.LoadPlayerProgress();

        _GM.dayCount = playerData.day;
        _GM.resturantName = playerData.resturantName;
        _GM.resturantRating = playerData.resturantRating;
        _GM.playerLevel = playerData.level;
        _GM.currentPlayerEXP = playerData.currentExp;

        //Load Staff Data

        StaffSaveData staffSaveData = SaveSystem.LoadStaffData();

        staffSaveData.AssignStaffID();

        foreach (var item in staffSaveData.staffStrArrays)
        {
            var staffData = ReturnStaffBasedOnID(item[0]);

            //hired
            if (item[1] == "True") _SM.totalHiredStaff.Add(staffData.gameObject);
            //active
            if (item[1] == "True") _SM.ActivateStaffOnLoad(staffData.gameObject);
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
