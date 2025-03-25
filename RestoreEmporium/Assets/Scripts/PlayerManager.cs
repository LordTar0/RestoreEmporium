using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Inventory inventory;
}

[System.Serializable]
public class Inventory
{
    public int Funds;
    public int InventorySize;
    public List<InventorySlot> Slots;
}

[System.Serializable]
public class InventorySlot
{
    public Item Item;
}