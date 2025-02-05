using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject selectionPosIndicator, gridPosIndicator;

    [SerializeField] private Grid placementGrid;

    [SerializeField] private InputManager inputManager;

    [SerializeField] private ObjectDataBaseSO database;
    private int selectedObjectIndex = -1;

    [SerializeField] private GameObject gridVisual;

    private void Start()
    {
        StopPlacement();
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();

        selectedObjectIndex = database.objectData.FindIndex(data => data.ID == ID);

        if (selectedObjectIndex < 0)
        {
            Debug.LogError($"No ID found: {ID}");
            return;
        }

        gridVisual.SetActive(true);
        gridPosIndicator.SetActive(true);

        inputManager.OnClicked += PlaceStructure;

        inputManager.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI()) { return; }

        Vector3 selectPos = inputManager.GetSelectMapPosition();
        Vector3Int gridPos = placementGrid.WorldToCell(selectPos);

        GameObject newStructureGO = Instantiate(database.objectData[selectedObjectIndex].Prefab);

        newStructureGO.transform.position = placementGrid.CellToWorld(gridPos);
    }

    private void StopPlacement()
    {
        selectedObjectIndex = -1;

        gridVisual.SetActive(false);
        gridPosIndicator.SetActive(false);

        inputManager.OnClicked -= PlaceStructure;

        inputManager.OnExit -= StopPlacement;
    }

    private void Update()
    {
        if (selectedObjectIndex < 0) { return; }

        Vector3 selectPos = inputManager.GetSelectMapPosition();
        Vector3Int gridPos = placementGrid.WorldToCell(selectPos);
        selectionPosIndicator.transform.position = selectPos;
        gridPosIndicator.transform.position = placementGrid.CellToWorld(gridPos);
    }
}
