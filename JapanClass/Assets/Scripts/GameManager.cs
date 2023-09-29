using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public float dayCount;
    public float money;
    public float reputation = 100;

    // Start is called before the first frame update
    void Start()
    {
        _UI.UpdateDay();
        _UI.UpdateMoney();
        _UI.UpdateReputationSlider();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
