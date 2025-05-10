using System;
using UnityEngine;
using Unity.Collections;

[CreateAssetMenu(menuName = "Restore Emporium/New Item")]
public class ItemData : ScriptableObject
{
    public int ID = -1;

    public Sprite Icon;
    public NameAndDescription NameAndDescription;
    [Space(10)]

    public int Cost;

    public AnimationCurve PotentialDiscounts;
}

[System.Serializable]
public class Item
{
    //Item Data
    public int ItemID;
    public NameAndDescription NameAndDescription = new();
    public int Cost;
    public Sprite Icon;

    //Inventory Specific Data
    public int Damage;
    [ReadOnly] public string InventoryID = Guid.NewGuid().ToString();

    private void GenerateUniqueID()
    {
        InventoryID = Guid.NewGuid().ToString();
    }

    public void GetItemData(Database database, int ID)
    {
        ItemData data = database.GetItemData(ID);

        if (!data) { Debug.LogError($"Could not find item with ID: {ID}. Please check the get item data reference from item data."); return; }

        ItemID = ID;
        NameAndDescription.Name = data.NameAndDescription.Name;
        NameAndDescription.Description = data.NameAndDescription.Description;
        Cost = data.Cost;
        Icon = data.Icon;

        GenerateUniqueID();
    }
}

[System.Serializable]
public class InventorySlot
{
    public Item Item;
}