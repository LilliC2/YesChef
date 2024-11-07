using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

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

    private void Start()
    {
        characterActionState = new CharacterActionState(null, holdItemTransform,null, null);
        characterController = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {
        characterController.Move(movmentV3 * movementSpeed * Time.deltaTime);
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
    public void OnInteractAction(InputAction.CallbackContext context)
    {
        //check if item is on furniture

        //check if item corelates with furniture piece

        //do action

    }

    /// <summary>
    /// Picking up and placing object
    /// </summary>
    public void OnObjectAction(InputAction.CallbackContext context)
    {
        //check if is already holding object
        if (characterActionState.IsHoldingItem)
        {
            //if HOLDING
            //check if looking at avalible space to place item
            if (highlightInteractable.highlightPrefab == null)
                return;
            GameObject targetObject = highlightInteractable.highlightPrefab;

            if (targetObject.TryGetComponent(out FurnitureItemHolder furnitureItemHolder))
            {
                //if SPACE IS FREE
                if (furnitureItemHolder.ReturnStatus == FurnitureItemHolder.Status.Unoccupied)
                {
                    //place item
                    characterActionState.PlaceOrder(furnitureItemHolder.ReturnHoldSpot);
                    furnitureItemHolder.SetStatus = FurnitureItemHolder.Status.Occupied;
                }
            }
            else return;
        }
        //if NOT HOLDING
        else
        {
            if (highlightInteractable.highlightPrefab == null)
                return;
            GameObject targetObject = highlightInteractable.highlightPrefab;

            //check if looking at object that can be picked up 
            if (!targetObject.CompareTag("PickUp"))
                return;

            //if IF LOOKING AT OBJECT
            //pick up object
            FoodData foodData = targetObject.GetComponent<FoodData>();

            FoodClass foodClass = foodData.order.foodClass;
            characterActionState = new CharacterActionState(targetObject, holdItemTransform, foodData,foodClass);
            characterActionState.PickUpOrder();
            
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
