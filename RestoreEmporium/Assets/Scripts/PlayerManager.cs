using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private InputSystem_Actions inputSystem;

    public PlayerInventory inventory = new();
    public Camera cam;
    public float lerpSpeed;

    public Transform cam_desk, cam_computer, cam_workstation;

    Coroutine camLerp;

    private void Start()
    {
        cam = GetComponentInChildren<Camera>();
        inputSystem = new();
        inputSystem.Enable();

        inputSystem.Player.Select.performed += WorldInput;
        FrontDeskSelect();
    }

    public void WorldInput(InputAction.CallbackContext context)
    {
        Vector2 ScreenPos = inputSystem.Player.ScreenPosition.ReadValue<Vector2>();
    }

    public void FrontDeskSelect()
    {
        MoveCamera(cam_desk.position, cam_desk.rotation, 60);
    }

    public void ComputerSelect()
    {
        MoveCamera(cam_computer.position, cam_computer.rotation, 30);
    }

    public void WorkStation()
    {
        MoveCamera(cam_workstation.position, cam_workstation.rotation, 30);
    }

    public void MoveCamera(Vector3 newLocation, Quaternion rotation ,float zoom)
    {
        if (camLerp != null) {StopCoroutine(camLerp);}
        camLerp = StartCoroutine(MoveCamLerp(newLocation, rotation, zoom));
    }

    private IEnumerator MoveCamLerp(Vector3 newLocation, Quaternion rotation, float zoom)
    {
        while (cam.transform.position != newLocation || cam.fieldOfView != zoom)
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, newLocation, lerpSpeed * Time.deltaTime);
            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, rotation, lerpSpeed * Time.deltaTime);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, zoom, lerpSpeed * Time.deltaTime);

            yield return null;
        }
    }
}

[System.Serializable]
public class PlayerInventory
{
    public int Funds;
    public int InventorySize;
    public List<InventorySlot> Slots = new();
}