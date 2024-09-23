using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayerProgress(GameManager gameManager, FoodManager foodManager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.yesChefSave";
        FileStream stream = new FileStream(path, FileMode.Create);

        //call constructor
        PlayerSaveData playerData = new PlayerSaveData(gameManager, foodManager);

        //insert into file
        formatter.Serialize(stream, playerData);

        //close file after
        stream.Close();
    }

    public static PlayerSaveData LoadPlayerProgress()
    {
        string path = Application.persistentDataPath + "/player.yesChefSave";

        //check if file exists
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            //open exisiting save file
            FileStream stream = new FileStream(path, FileMode.Open);
            PlayerSaveData playerData = formatter.Deserialize(stream) as PlayerSaveData;

            stream.Close();

            return playerData;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }

    }
}
