using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    public void SaveGame()
    {
        SaveSystem.SavePlayerProgress(_GM,_FM);
    }

    public void LoadGame()
    {
        PlayerSaveData playerData = SaveSystem.LoadPlayerProgress();

        _GM.dayCount = playerData.day;
        _GM.resturantName = playerData.resturantName;
        _GM.resturantRating = playerData.resturantRating;
        _GM.playerLevel = playerData.level;
        _GM.currentPlayerEXP = playerData.currentExp;
    }
}
