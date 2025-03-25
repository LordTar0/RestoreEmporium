using System;
using UnityEngine;
using Unity.Collections;
using System.Collections.Generic;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    public int ID;

    public string Name;
    public string Description;

    public int Cost;
}

[System.Serializable]
public class Item
{
    //Item Data
    public int ItemID;
    public string Name;
    public string Description;
    public int Cost;

    //Inventory Specific Data
    public int Damage;
    [ReadOnly] public string InventoryID = Guid.NewGuid().ToString();

    private void Generate()
    {
        InventoryID = Guid.NewGuid().ToString();
    }
}
