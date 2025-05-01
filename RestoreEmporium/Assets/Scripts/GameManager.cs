using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Database Database;

    public List<ItemData> ItemsUnlocked; 

    PlayerManager playerManager;
    ComputerManager computerManager;

    GameDetails gameDetails = new();

    private void Awake()
    {
        playerManager = FindAnyObjectByType<PlayerManager>();
        computerManager = FindAnyObjectByType<ComputerManager>();
    }

    private void Start()
    {
        StartNewDay();
    }

    public void StartNewDay()
    {
        gameDetails.Day++;

        NewShopInventory();
        WeatherUpdate();
    }

    private void NewShopInventory()
    {
        int maxSlotCount = computerManager.inventory.SlotCount;

        for (int i = 0; i < maxSlotCount; i++)
        {
            ComputerInvSlot slot;

            if (computerManager.inventory.inventorySlots.Count < maxSlotCount) { computerManager.inventory.inventorySlots.Add(new()); Debug.Log($"Adding new slot to slot {i} as it did not contain any data"); }

            slot = computerManager.inventory.inventorySlots[i];

            if (!slot.IsSold) { slot.GenerateDiscount(Database); Debug.Log($"Slot: {i} has not been sold, recalculating for a discount on item"); continue; }

            computerManager.AddItemToMarket(slot, GenerateItem(), 0); //Generates and adds an item to the market //add damage system later!
            slot.IsSold = false; //Tells the slot its now for sale
        }
    }

    private void WeatherUpdate()
    {

    }

    private Item GenerateItem()
    {
        int itemChosen = Random.Range(0, ItemsUnlocked.Count);

        ItemData data = ItemsUnlocked[itemChosen];

        Item item = new Item();

        item.GetItemData(Database, data.ID);

        return item;
    }
}

public class GameDetails
{
    public int Day;
    public int currentWeather;
}

[System.Serializable]
public class InventorySlot
{
    public Item Item;
}