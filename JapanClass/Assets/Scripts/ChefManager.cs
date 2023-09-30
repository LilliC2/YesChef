using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChefManager : Singleton<ChefManager>
{
    public GameObject[] chefArray;
    bool placingChef;
    GameObject newChef;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(placingChef)
        {
            //have chef follow mouse
            Vector3 mousePos = Input.mousePosition;
            Vector3 newMousePos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10));
            Vector3 chefPos = new Vector3(newMousePos.x, 1, newMousePos.z);

            newChef.transform.position = chefPos;

            if(Input.GetKeyDown(KeyCode.Mouse0))
            {

                //check if chef is in a valid posistion

                //check chef is not too close to another chef


                //place chef
                placingChef = false;
                //subtract cost of chef from money
                _GM.money -= newChef.GetComponent<ChefData>().chefData.hireCost;

                _UI.UpdateMoney();

            }
            if(Input.GetKeyDown(KeyCode.Mouse1))
            {
                Destroy(newChef);
                //place chef
                placingChef = false;
            }
        }
    }

    public void CreateNewChef(GameObject _chef)
    {
        placingChef = true;
        newChef = Instantiate(Resources.Load<GameObject>("Prefabs/" + _chef.GetComponent<ChefData>().chefData.name));

    }
}
