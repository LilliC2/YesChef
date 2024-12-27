using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class CameraController : GameBehaviour
{
    [SerializeField]
    float panSpeed, minZ, maxZ;

    Camera cam;
    // Mouse Input Vars
    private Vector3 dragOrigin;
    private Vector3 cameraDragOrigin;
    private Vector3 prevPos;
    Vector3 lastMousePosition;
    [SerializeField] float defaultCameraSize,zOffset, yOffset, xOffset;

    public enum CameraState { PlayerControlled, TalkToStaff, DisablePlayerControl, ZoomOnPlayer}
    public CameraState state;

    Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case CameraState.PlayerControlled:
                ButtonInputs();
                break;
        }
        

    }
    [ContextMenu("Zoom On Player")] 


    public void ZoomCameraOut()
    {
        transform.DOMove(prevPos, 1);
        cam.DOOrthoSize(defaultCameraSize, 1);
    }

    public void CameraFocusStaff(GameObject _staff)
    {
        prevPos = cam.transform.position;
        state = CameraState.TalkToStaff;
        Vector3 focusPos = new Vector3(_staff.transform.position.x - 6, transform.position.y, _staff.transform.position.z - 6);

        float orthographicSize = 2.8f;
        
        transform.DOMove(focusPos, 1);
        cam.DOOrthoSize(orthographicSize, 1);

        
    }


    void ButtonInputs()
    {
        float inputZ = 0f;
        float inputX = 0f;

        if (Input.GetKey("w"))
        {
            inputZ += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s"))
        {
            inputZ -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d"))
        {
            inputX += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a"))
        {
            inputX -= panSpeed * Time.deltaTime;
        }

        MoveCamera(inputX, inputZ);
    }

    void MouseInputs()
    {
        if (Input.GetMouseButtonDown(1))
        {
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(1))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;

            MoveCamera(delta.x, delta.y);

            lastMousePosition = Input.mousePosition;
        }
    }

    void MoveCamera(float xInput, float zInput)
    {
        float zMove = Mathf.Cos(transform.eulerAngles.y * Mathf.PI / 180) * zInput - Mathf.Sin(transform.eulerAngles.y * Mathf.PI / 180) * xInput;
        float xMove = Mathf.Sin(transform.eulerAngles.y * Mathf.PI / 180) * zInput + Mathf.Cos(transform.eulerAngles.y * Mathf.PI / 180) * xInput;

        transform.position = transform.position + new Vector3(xMove, 0, zMove);
    }
}
