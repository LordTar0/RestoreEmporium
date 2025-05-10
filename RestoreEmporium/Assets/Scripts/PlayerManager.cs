using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Interactor))]
public class PlayerManager : MonoBehaviour
{
    public PlayerManager _instance { get; private set; }

    private InputSystem_Actions inputSystem;

    public PlayerInventory inventory = new();
    public Camera cam;
    public float lerpSpeed;

    public InteractionPoints cam_desk, cam_computer, cam_workstation;

    Interactor interactor;

    DepthOfField depthFocus;

    Coroutine camLerp;
    Vector3 lastPos;

    //Makes sure there's only one of this object
    private void Singleton()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        Singleton();
        cam = GetComponentInChildren<Camera>();
        inputSystem = new();
        inputSystem.Enable();
        interactor = GetComponent<Interactor>();

        Volume volume = GetComponent<Volume>();
        volume.profile.TryGet<DepthOfField>(out DepthOfField DOF);
        if (DOF != null) depthFocus = DOF;
        else { Debug.LogError($"Depth of field component could not be found. Please fix."); }

        FrontDeskSelect();
    }

    public void WorldInput(InputAction.CallbackContext context)
    {
        Vector2 ScreenPos = inputSystem.FrontDesk.ScreenPosition.ReadValue<Vector2>();

        IInteractable interactable;

        interactable = interactor.GetInteractable(GetSelectedPosition(ScreenPos));

        if (interactable == null) { return; }

        switch (interactable)
        {
            case ComputerManager: ComputerSelect(); Debug.Log("turn on pc");
                break;

            case NPCManager: Debug.Log("Talking to NPC");
                break;

            case OpenSignManager: Debug.Log("Switching Sign");
                break;

            default : FrontDeskSelect(); Debug.Log("going to desk");
                break;
        }
    }

    public void EndInteractionInput(InputAction.CallbackContext context)
    {
        interactor.EndInteraction();
        FrontDeskSelect();
    }

    public void FrontDeskSelect()
    {
        EnableFrontDeskInput(true);
        MoveCamera(cam_desk.Anchor.position, cam_desk.Anchor.rotation, cam_desk.Zoom, cam_desk.FocusDepth, cam_desk.FocusLength);
    }

    public void ComputerSelect()
    {
        EnableFrontDeskInput(false);
        MoveCamera(cam_computer.Anchor.position, cam_computer.Anchor.rotation, cam_computer.Zoom, cam_computer.FocusDepth, cam_computer.FocusLength);
    }

    public void WorkStation()
    {
        EnableFrontDeskInput(false);
        MoveCamera(cam_workstation.Anchor.position, cam_workstation.Anchor.rotation, cam_workstation.Zoom, cam_workstation.FocusDepth, cam_workstation.FocusLength);
    }

    public void MoveCamera(Vector3 newLocation, Quaternion rotation ,float zoom, float focusdistance, int focusLength)
    {
        if (camLerp != null) {StopCoroutine(camLerp);}
        camLerp = StartCoroutine(MoveCamLerp(newLocation, rotation, zoom, focusdistance, focusLength));
    }

    public Vector3 GetSelectedPosition(Vector2 input)
    {
        Vector3 selectPos = new Vector3(input.x, input.y, cam.nearClipPlane);

        Ray ray = cam.ScreenPointToRay(selectPos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            lastPos = hit.point;
            return lastPos;
        }

        return Vector3.zero;
    }

    void EnableFrontDeskInput(bool activate)
    {
        if (activate)
        {
            inputSystem.FrontDesk.Select.performed += WorldInput;
            inputSystem.ExitInteraction.Exit.performed -= EndInteractionInput;
        }
        else
        {
            inputSystem.FrontDesk.Select.performed -= WorldInput;
            inputSystem.ExitInteraction.Exit.performed += EndInteractionInput;
        }
    }

    private IEnumerator MoveCamLerp(Vector3 newLocation, Quaternion rotation, float zoom, float focusdistance, int focusLength)
    {
        while (cam.transform.position != newLocation || cam.fieldOfView != zoom || depthFocus.focusDistance.value != focusdistance)
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, newLocation, lerpSpeed * Time.deltaTime);
            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, rotation, lerpSpeed * Time.deltaTime);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, zoom, lerpSpeed * Time.deltaTime);
            depthFocus.focusDistance.value = Mathf.Lerp(depthFocus.focusDistance.value, focusdistance, lerpSpeed * 2 * Time.deltaTime);
            depthFocus.focalLength.value = Mathf.Lerp(depthFocus.focalLength.value, focusLength, lerpSpeed * 2 * Time.deltaTime);

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

[System.Serializable]
public class InteractionPoints
{
    public Transform Anchor;
    public float Zoom = 60f;
    public float FocusDepth = 10f;
    public int FocusLength = 1;
}