using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class WaiterManager : Singleton<WaiterManager>
{
    public GameObject[] waiterArray;
    public List<GameObject> currentWaiters;
    public LayerMask ground;
    public LayerMask collisionMask;
    Vector3 waiterPos;
    bool validPos;
    bool placingWaiter;
    GameObject newWaiter;
    [SerializeField]
    Material canPlaceMat;
    [SerializeField]
    Material cannotPlaceMat;


    [SerializeField]
    Ease placingEase;
    [SerializeField]
    float placingEaseTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (placingWaiter)
        {
            //have chef follow mouse
            //Vector3 mousePos = Input.mousePosition;
            //Vector3 newMousePos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10));
            //Vector3 chefPos = new Vector3(newMousePos.x, 0.5f, newMousePos.z);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, ground))
            {
                waiterPos = new Vector3(hit.point.x, hit.point.y + 1, hit.point.z);
                newWaiter.transform.position = waiterPos;
                validPos = !Physics.CheckSphere(newWaiter.transform.position, 1, collisionMask);

                GameObject collisionVisualiser = newWaiter.transform.Find("CollisionVisualiser").gameObject;

                UpdateCollisionVisualiser(collisionVisualiser, validPos);

                if (validPos)
                {
                    //check if chef is in a valid posistion
                    print("Can place");




                    //check chef is not too close to another chef

                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        newWaiter.transform.Find("Waiter").gameObject.SetActive(true);
                        Destroy(collisionVisualiser);

                        newWaiter.layer = 11;
                        //newWaiter.GetComponent<CapsuleCollider>().isTrigger = false;

                        //place chef
                        placingWaiter = false;

                        Animator newWaiterAnim = newWaiter.GetComponent<WaiterData>().anim;

                        //placing animation
                        //newWaiter.transform.DOMoveY(0, placingEaseTime).SetEase(placingEase);
                        newWaiterAnim.SetTrigger("Spawn");


                        _AM.placingChef.Play();

                        newWaiter.GetComponent<WaiterData>().placed = true;
                        newWaiter.GetComponent<WaiterData>().homePos = newWaiter.transform.position;
                        currentWaiters.Add(newWaiter);
                        //subtract cost of chef from money
                        _GM.money -= newWaiter.GetComponent<WaiterData>().waiterData.hireCost;

                        _GM.event_updateMoney.Invoke();

                    }
                }

            }



            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Destroy(newWaiter);
                //place chef
                placingWaiter = false;
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

    public void CreateNewWaiter(GameObject _waiter)
    {
        placingWaiter = true;
        newWaiter = Instantiate(Resources.Load<GameObject>("Prefabs/Waiters/" + _waiter.GetComponent<WaiterData>().waiterData.name));

    }
}
