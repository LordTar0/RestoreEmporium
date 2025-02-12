using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private Grid placementGrid;

    [SerializeField] private InputManager inputManager;

    [SerializeField] private ObjectDataBaseSO database;

    [SerializeField] private GameObject gridVisual;

    private GridData floorData, furnitureData;

    [SerializeField] PreviewSystem previewSystem;

    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    [SerializeField] private ObjectPlacer objectPlacer;

    IBuildingState buildingState;

    private void Start()
    {
        StopPlacement();
        floorData = new();
        furnitureData = new();
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();

        gridVisual.SetActive(true);

        buildingState = new PlacementState(ID, placementGrid, previewSystem, database, floorData, furnitureData, objectPlacer);

        inputManager.OnClicked += PlaceStructure;

        inputManager.OnExit += StopPlacement;
    }

    public void StartRemoving()
    {
        StopPlacement();

        gridVisual.SetActive(true);

        buildingState = new RemovingState(placementGrid, previewSystem, floorData, furnitureData, objectPlacer);

        inputManager.OnClicked += PlaceStructure;

        inputManager.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI()) { return; }

        Vector3 selectPos = inputManager.GetSelectMapPosition();
        Vector3Int gridPos = placementGrid.WorldToCell(selectPos);

        buildingState.OnAction(gridPos);
    }

    private void StopPlacement()
    {
        if (buildingState == null) { return; }

        gridVisual.SetActive(false);

        buildingState.EndState();

        inputManager.OnClicked -= PlaceStructure;

        inputManager.OnExit -= StopPlacement;

        lastDetectedPosition = Vector3Int.zero;

        buildingState = null;
    }

    private void Update()
    {
        if (buildingState == null) { return; }

        Vector3 selectPos = inputManager.GetSelectMapPosition();
        Vector3Int gridPos = placementGrid.WorldToCell(selectPos);

        if (lastDetectedPosition != gridPos)
        {
            buildingState.UpdateState(gridPos);

            lastDetectedPosition = gridPos;
        }
    }
}
