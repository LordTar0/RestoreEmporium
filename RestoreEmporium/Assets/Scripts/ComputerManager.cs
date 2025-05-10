using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ComputerManager : MonoBehaviour, IInteractable
{
    public ComputerInventory inventory;
    [SerializeField] private PCMonitorHandler myScreen;
    bool isOn;

    private void Awake()
    {
        isOn = true;
        TurnPCOn();
    }

    public void Interact(Interactor interactor, out bool isSuccessful)
    {
        TurnPCOn();

        if (isOn) { isSuccessful = true; }
        else { isSuccessful = false; }
    }

    public void EndInteraction()
    {
        TurnPCOn();
    }

    public void AddItemToMarket(ComputerInvSlot slot, Item item, int damage_amount)
    {
        if (slot == null|| item == null || damage_amount < 0)
        { Debug.LogError($"Failed to add item. Item: {item.NameAndDescription.Name}, Damage amount: {damage_amount}"); return; }
    }

    public void RemoveItemFromMarket(ComputerInvSlot slot)
    {
        if (slot == null) 
        { Debug.LogError($"This item does not exist! Slot number: {CheckSlotNumber(slot)}"); return; }


    }

    public void PurchaseItem(ComputerInvSlot slot)
    {
        if (slot.IsSold || slot == null) 
        { Debug.LogError($"Purchase failed. Slot number: {CheckSlotNumber(slot)}"); return; }


    }

    public void AddDiscountToItem(ComputerInvSlot slot, Database database)
    {
        if (slot == null || database == null)
        { Debug.LogError($"Discount Addition Failed. Slot number: {CheckSlotNumber(slot)}. Check Reference to see if slot and/or database was referenced correctly!"); return; }

        slot.GenerateDiscount(database);
    }

    public void UpgradeStorage()
    {
        inventory.SlotCount++;
        inventory.inventorySlots.Capacity++;
    }

    private int CheckSlotNumber(ComputerInvSlot slot)
    {
        int number = inventory.inventorySlots.FindIndex(inventory.inventorySlots.Count, i => i.Slot.Item.InventoryID == slot.Slot.Item.InventoryID);
        return number;
    }

    public void TurnPCOn()
    {
        isOn = !isOn;
        myScreen.gameObject.SetActive(isOn);

        if (isOn)
        {
        }
        else
        {
        }
    }
}

[System.Serializable]
public class ComputerInventory
{
    public int SlotCount;
    public List<ComputerInvSlot> inventorySlots;
}

[System.Serializable]
public class ComputerInvSlot
{
    public bool IsSold = true;
    public float PercentageOff = 0;
    public InventorySlot Slot;

    public void GenerateDiscount(Database database)
    {
        ItemData itemData = database.GetItemData(Slot.Item.ItemID);

        float discountRoll = Random.Range(0, 100);

        float discount = itemData.PotentialDiscounts.Evaluate(discountRoll/100);

        PercentageOff = discount;

        Debug.Log($"Discount roll: {discountRoll}, Discount: {discount}");
    }
}