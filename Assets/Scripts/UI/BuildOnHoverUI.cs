using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildOnHoverUI : GameBehaviour
{
    [SerializeField] TMP_Text objectName, objectSize, objectPrice;
    [SerializeField] ObjectsDatabaseSO database;
    public void UpdateText(int ID)
    {
        var i = database.objectsData.FindIndex(data => data.ID == ID);
        var data = database.objectsData[i];

        _UI.objectHoveredOver = data;

        objectName.text = data.name;
        objectSize.text = data.size.ToString();
        objectPrice.text = "Not added yet";

    }


}
