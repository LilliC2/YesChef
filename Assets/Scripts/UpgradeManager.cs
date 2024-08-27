using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeInformation
{
    public string name;
    public string description;
    public float costToUnlock;
    public bool active;
}

public class UpgradeManager : Singleton<UpgradeManager>
{
    public GameObject[] recipePosters;
    public UpgradeInformation[] resturantUpgradeInformation;
    public UpgradeInformation[] staffUpgradeInformation;

    public float chefUpgradeCostMultiplier;

    [SerializeField] private GameObject[] tables;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public void PurchaseUpgrade(int _upgradeID)
    //{
    //    var currentUpgrade = resturantUpgradeInformation[_upgradeID];

    //    if (currentUpgrade.name.Contains("+1 Table"))
    //    {
    //        TableUpgrades(currentUpgrade);
    //    }



    //}

    //void TableUpgrades(UpgradeInformation _currentUpgrade)
    //{

    //    int index = System.Array.IndexOf(resturantUpgradeInformation, _currentUpgrade);
    //    switch(index)
    //    {
    //        case 0:

    //            tables[0].SetActive(true);
    //            for (int i = 0; i < tables[0].transform.childCount; i++)
    //            {
    //                if (tables[0].transform.GetChild(i).name.Contains("Chair")) _CustM.emptyChairQueue.Add(tables[0].transform.GetChild(i).gameObject);
    //            }

    //        break;
    //        case 1:

    //            tables[1].SetActive(true);
    //            for (int i = 0; i < tables[1].transform.childCount; i++)
    //            {
    //                if (tables[1].transform.GetChild(i).name.Contains("Chair")) _CustM.emptyChairQueue.Add(tables[1].transform.GetChild(i).gameObject);
    //            }


    //            break;
    //        case 2:

    //            tables[2].SetActive(true);
    //            for (int i = 0; i < tables[2].transform.childCount; i++)
    //            {
    //                if (tables[2].transform.GetChild(i).name.Contains("Chair")) _CustM.emptyChairQueue.Add(tables[2].transform.GetChild(i).gameObject);
    //            }


    //            break;
    //        case 5:

    //            tables[3].SetActive(true);
    //            for (int i = 0; i < tables[3].transform.childCount; i++)
    //            {
    //                if (tables[3].transform.GetChild(i).name.Contains("Chair")) _CustM.emptyChairQueue.Add(tables[3].transform.GetChild(i).gameObject);
    //            }


    //            break;
    //        case 6:

    //            tables[4].SetActive(true);
    //            for (int i = 0; i < tables[4].transform.childCount; i++)
    //            {
    //                if (tables[4].transform.GetChild(i).name.Contains("Chair")) _CustM.emptyChairQueue.Add(tables[4].transform.GetChild(i).gameObject);
    //            }


    //            break;
    //        case 7:

    //            tables[5].SetActive(true); for (int i = 0; i < tables[5].transform.childCount; i++)
    //            {
    //                if (tables[5].transform.GetChild(i).name.Contains("Chair")) _CustM.emptyChairQueue.Add(tables[5].transform.GetChild(i).gameObject);
    //            }


    //            break;
    //    }
    //}
}
