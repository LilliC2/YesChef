using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeInformation
{
    public string name;
    public string description;
    public bool active;
}

public class UpgradeManager : Singleton<UpgradeManager>
{
    public GameObject[] recipePosters;
    public UpgradeInformation[] resturantUpgradeInformation;
    public UpgradeInformation[] staffUpgradeInformation;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PurchaseUpgrade(int _upgradeID)
    {
        var currentUpgrade = resturantUpgradeInformation.FindIndex(_upgradeID);

        currentUpgrade.active = true;
    
    }
}
