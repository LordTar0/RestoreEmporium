using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    PlayerInputController controller;

    [SerializeField] private Camera cam;

    private Vector3 lastPos;

    [SerializeField] private LayerMask placementLayer;

    public event Action OnClicked, OnExit;

    private void Awake()
    {
        controller = new();
        controller.BuildMode.Enable();

        controller.BuildMode.OnClickAction.performed += OnClickAction;
        controller.BuildMode.ExitBuildMenu.performed += OnExitAction;
    }

    private void OnClickAction(InputAction.CallbackContext context)
    {
        OnClicked?.Invoke();
    }

    private void OnExitAction(InputAction.CallbackContext context)
    {
        OnExit?.Invoke();
    }

    public bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();

    public Vector3 GetSelectMapPosition()
    {
        Vector2 SelectScreenPos = controller.BuildMode.SelectPos.ReadValue<Vector2>();
        Vector3 SelectPos = new Vector3(SelectScreenPos.x, SelectScreenPos.y, cam.nearClipPlane);

        Ray ray = cam.ScreenPointToRay(SelectPos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, placementLayer))
        {
            lastPos = hit.point;
        }

        return lastPos;
    }
}
