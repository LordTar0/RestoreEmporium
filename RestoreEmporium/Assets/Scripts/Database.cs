using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Restore Emporium/New Database")]
public class Database : ScriptableObject
{
    [SerializeField] private List<ItemData> Items;
    [SerializeField] private List<Weather> Weather;

    [ContextMenu("Set IDs")]
    public void SetIDs()
    {
        Items = new();
        Weather = new();

        Items = GetItems();
        Weather = GetWeather();

        Debug.Log("Successfully added items to database.");
    }

    private List<ItemData> GetItems()
    {
        var list = new List<ItemData>();

        var foundItems = Resources.LoadAll<ItemData>("Items").OrderBy(i => i.ID).ToList();
        var hasIDInRange = foundItems.Where(i => i.ID != -1 && i.ID < foundItems.Count).OrderBy(i => i.ID).ToList();
        var hasIDNotInRange = foundItems.Where(i => i.ID != -1 && i.ID >= foundItems.Count).OrderBy(i => i.ID).ToList();
        var noID = foundItems.Where(i => i.ID <= -1).ToList();

        var index_Items = 0;

        for (int i = 0; i < foundItems.Count; i++)
        {
            ItemData _itemToAdd;
            _itemToAdd = hasIDInRange.Find(d => d.ID == i);

            if (_itemToAdd != null)
            {
                list.Add(_itemToAdd);
            }
            else if (index_Items < noID.Count)
            {
                noID[index_Items].ID = i;
                _itemToAdd = noID[index_Items];
                index_Items++;
                list.Add(_itemToAdd);
            }
        }

        foreach (var item_NoRange in hasIDNotInRange)
        {
            list.Add(item_NoRange);
            Debug.Log($"Added {item_NoRange.Name} to the list as it was not in ID range.");
        }

        return list;
    }

    private List<Weather> GetWeather()
    {
        var list = new List<Weather>();

        var foundItems = Resources.LoadAll<Weather>("Weather").OrderBy(i => i.ID).ToList();
        var hasIDInRange = foundItems.Where(i => i.ID != -1 && i.ID < foundItems.Count).OrderBy(i => i.ID).ToList();
        var hasIDNotInRange = foundItems.Where(i => i.ID != -1 && i.ID >= foundItems.Count).OrderBy(i => i.ID).ToList();
        var noID = foundItems.Where(i => i.ID <= -1).ToList();

        var index_Items = 0;

        for (int i = 0; i < foundItems.Count; i++)
        {
            Weather _itemToAdd;
            _itemToAdd = hasIDInRange.Find(d => d.ID == i);

            if (_itemToAdd != null)
            {
                list.Add(_itemToAdd);
            }
            else if (index_Items < noID.Count)
            {
                noID[index_Items].ID = i;
                _itemToAdd = noID[index_Items];
                index_Items++;
                list.Add(_itemToAdd);
            }
        }

        foreach (var item_NoRange in hasIDNotInRange)
        {
            list.Add(item_NoRange);
            Debug.Log($"Added {item_NoRange.Name} to the list as it was not in ID range.");
        }

        return list;
    }

    public ItemData GetItemData(int ID)
    {
        ItemData item_requested = Items.Find(i => i.ID == ID);

        if (item_requested) { return item_requested; }

        Debug.LogError($"Could not find item ID: {ID}. Check Database or request ticket");
        return null;
    }
}
