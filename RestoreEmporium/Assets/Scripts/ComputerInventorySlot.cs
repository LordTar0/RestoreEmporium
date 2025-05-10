using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class ComputerInventorySlot : UIInventorySlot
{

    [SerializeField] private DiscountBar discountBar;
    [SerializeField] private GameObject soldBar;

    public void UpdateComputerSlotData(int slotIndex, ComputerInvSlot slot)
    {
        //Gets the item from the slot (Not the raw reference data from the scriptable object but the save inventory kind)
        Item item = slot.Slot.Item;

        //Updates the slot base class using the data to display the icon, name etc...
        base.UpdateSlotData(slotIndex, item);

        //Updates the discount for the display and if it displays or not (cause if its 0% off, kind of pointless huh)
        discountBar.UpdateQuantity(slot.PercentageOff);

        //Debugs the data display
        Debug.Log($"UpdatedSlotData on slot: {slotIndex}.SlotItem: {item.NameAndDescription.Name}. ID: {item.ItemID}. InventoryID: {item.InventoryID}"); 
    }

    public void IsSold(bool isSold)
    {
        soldBar.SetActive(isSold);
        button.interactable = !isSold;
    }
}

[System.Serializable]
public class DiscountBar
{
    public GameObject RootObject;
    public TextMeshProUGUI DiscountQuantity;

    public void UpdateQuantity(float discount)
    {
        if (discount > 100 || discount < 0) { Debug.LogError($"DISCOUNT APPLIED IS TOO HIGH, PLEASE CHANGE ASAP! (The game will still run normally however this should be fixed asap!)"); }

        if (discount > 0)
        {
            RootObject.SetActive(true);
            DiscountQuantity.text = $"{discount}%";
        }
        else
        {
            RootObject.SetActive(false);
        }
    }
}