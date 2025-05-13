using UnityEngine;
using UnityEngine.InputSystem;

public class DragDrop : MonoBehaviour
{
    [SerializeField] private InputAction clickAction;
    [SerializeField] private InputAction pointerPosition;
    [SerializeField] private string requiredTag = "Draggable";

    private Transform selectedObject;
    private float distance;

    private void OnEnable()
    {
        clickAction.Enable();
        pointerPosition.Enable();

        clickAction.performed += OnClick;
        clickAction.canceled += OnRelease;
    }

    private void OnDisable()
    {
        clickAction.Disable();
        pointerPosition.Disable();

        clickAction.performed -= OnClick;
        clickAction.canceled -= OnRelease;
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        if (!DragDropManager.DragDropEnabled)
        {
            Debug.Log("Drag Mode not enabled.");
            return;
        }

        Vector2 mousePosition = pointerPosition.ReadValue<Vector2>();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log("Ray hit: " + hit.transform.name);

            if (hit.transform.CompareTag(requiredTag))
            {
                selectedObject = hit.transform;
                distance = Vector3.Distance(Camera.main.transform.position, selectedObject.position);
                Debug.Log("Object selected for dragging: " + selectedObject.name);
            }
            else
            {
                Debug.Log("Object hit does not have required tag: " + requiredTag);
            }
        }
        else
        {
            Debug.Log("Raycast did not hit any object.");
        }
    }

    private void OnRelease(InputAction.CallbackContext context)
    {
        if (selectedObject != null)
        {
            Debug.Log("Released object: " + selectedObject.name);
        }
        selectedObject = null;
    }

    private void Update()
    {
        if (!DragDropManager.DragDropEnabled || selectedObject == null)
            return;

        Vector2 mousePosition = pointerPosition.ReadValue<Vector2>();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        Vector3 point = ray.GetPoint(distance);
        selectedObject.position = point;
    }
}
