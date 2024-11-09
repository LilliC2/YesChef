using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class PlayerController : Singleton<PlayerController>
{
    
    CharacterController characterController;
    ICharacterActionState characterActionState;
    Vector3 movmentV3;
    [SerializeField]
    float movementSpeed;
    [SerializeField] LayerMask interactableLayerMask;
    [SerializeField] HighlightInteractable highlightInteractable;
    [SerializeField] Transform holdItemTransform;
    bool placeDelay; //add delay so you don't pick up an item as soon as you place it
    [SerializeField] float placeDelayTime;
    private void Start()
    {
        characterActionState = new CharacterActionState(null, holdItemTransform,null, null);
        characterController = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {
        characterController.Move(movmentV3 * movementSpeed * Time.deltaTime);

        if(characterActionState == null )
            return;
        if(characterActionState.IsHoldingItem)
            characterActionState.UpdateOrderPosition();
    }

    public void OnMove(InputAction.CallbackContext context)
    {

        Vector2 move = context.ReadValue<Vector2>();
        Vector3 toConvert = new Vector3(move.x, 0, move.y);
        movmentV3 = IsoVectorConvert(toConvert);
        transform.rotation = Quaternion.LookRotation(movmentV3);

    }
    /// <summary>
    /// Interacting with target object such as workstation
    /// </summary>
    /// <param name="context"></param>
    public void OnWorkStationInteraction(InputAction.CallbackContext context)
    {
        //check if item is on furniture

        //check if item corelates with furniture piece

        //do action

    }

    /// <summary>
    /// Picking up and placing object
    /// </summary>
    public void OnPickUpInteraction(InputAction.CallbackContext context)
    {
        //if NOT HOLDING
        if(!characterActionState.IsHoldingItem & !placeDelay)
        {
            if (highlightInteractable.highlightPrefab == null)
                return;
            GameObject targetObject = highlightInteractable.highlightPrefab;

            //check if looking at object that can be picked up 
            if (!targetObject.CompareTag("PickUp"))
                return;

            //if IF LOOKING AT OBJECT
            //pick up object
            print("Pick up item");
            FoodData foodData = targetObject.GetComponent<FoodData>();
            foodData.FurnitureHolder.SetStatus = FurnitureItemHolder.Status.Unoccupied;

            foodData.FurnitureHolder = null;

            FoodClass foodClass = foodData.order.foodClass;
            characterActionState = new CharacterActionState(targetObject, holdItemTransform, foodData,foodClass);
            characterActionState.PickUpOrder();
            ExecuteAfterSeconds(placeDelayTime, () => placeDelay = true);

            return;
            
        }
        else if(placeDelay)
        {
            //if HOLDING
            //check if looking at avalible space to place item
            if (highlightInteractable.highlightPrefab == null)
                return;
            GameObject targetObject = highlightInteractable.highlightPrefab;
            print("Place");
            if (targetObject.TryGetComponent(out FurnitureItemHolder furnitureItemHolder))
            {
                //if SPACE IS FREE
                if (furnitureItemHolder.ReturnStatus == FurnitureItemHolder.Status.Unoccupied)
                {
                    placeDelay = true;
                    //place item
                    furnitureItemHolder.SetStatus = FurnitureItemHolder.Status.Occupied;

                    //get held item here!!!
                    var foodData = characterActionState.OrderData;
                    print(foodData);
                    foodData.FurnitureHolder = furnitureItemHolder;
                    characterActionState.PlaceOrder(furnitureItemHolder.ReturnHoldSpotV3);
                    ExecuteAfterSeconds(placeDelayTime, ()=>placeDelay = false);


                }
                return;
            }
            else return;
        }


    }

    /// <summary>
    /// Convert movement to align in isometric space
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    Vector3 IsoVectorConvert(Vector3 vector)
    {
        Quaternion rotation = Quaternion.Euler(0, 45, 0);
        Matrix4x4 isoMatrix = Matrix4x4.Rotate(rotation);
        Vector3 result = isoMatrix.MultiplyPoint3x4(vector);
        return result;
    }



}
