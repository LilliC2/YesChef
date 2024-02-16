using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChefManager : Singleton<ChefManager>
{
    public GameObject[] chefArray;

    public List<GameObject> currentChefs;

    bool placingChef;
    GameObject newChef;
    [SerializeField]
    Material canPlaceMat;
    [SerializeField]
    Material cannotPlaceMat;

    [SerializeField]
    Ease placingEase;
    [SerializeField]
    float placingEaseTime;


    bool validPos;
    public LayerMask collisionMask;
    public LayerMask ground;
    Vector3 chefPos;
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
            //Vector3 mousePos = Input.mousePosition;
            //Vector3 newMousePos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10));
            //Vector3 chefPos = new Vector3(newMousePos.x, 0.5f, newMousePos.z);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit,100, ground))
            {
                chefPos = new Vector3(hit.point.x, hit.point.y+1,hit.point.z);
                newChef.transform.position = chefPos;
                validPos = !Physics.CheckSphere(newChef.transform.position, 1, collisionMask);

                GameObject collisionVisualiser = newChef.transform.Find("CollisionVisualiser").gameObject;

                UpdateCollisionVisualiser(collisionVisualiser, validPos);

                if (validPos)
                {
                    //check if chef is in a valid posistion
                    //print("Can place");

                    


                    //check chef is not too close to another chef

                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        newChef.transform.Find("Chef").gameObject.SetActive(true);
                        Destroy(collisionVisualiser);

                        newChef.layer = 8;
                        newChef.GetComponent<CapsuleCollider>().isTrigger = false;

                        //place chef
                        placingChef = false;
                        newChef.GetComponent<ChefData>().placed = true;

                        //placing animation
                        newChef.transform.DOMoveY(0, placingEaseTime).SetEase(placingEase);


                        _AM.placingChef.Play();


                        currentChefs.Add(newChef);

                        //subtract cost of chef from money
                        _GM.money -= newChef.GetComponent<ChefData>().chefData.hireCost;

                        _GM.event_updateMoney.Invoke();

                    }
                }
                
            }

           
           
            if(Input.GetKeyDown(KeyCode.Mouse1))
            {
                Destroy(newChef);
                //place chef
                placingChef = false;
            }
        }
    }

    void UpdateCollisionVisualiser(GameObject _collisionVisuliser, bool _validPos)
    {
        var children = _collisionVisuliser.GetComponentsInChildren<MeshRenderer>();

        foreach (var child in children)
        {
            if (validPos) child.material = canPlaceMat;
            else child.material = cannotPlaceMat;

        }
    }

    public void CreateNewChef(GameObject _chef)
    {
        placingChef = true;
        newChef = Instantiate(Resources.Load<GameObject>("Prefabs/Chefs/" + _chef.GetComponent<ChefData>().chefData.name));

    }
}
