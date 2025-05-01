using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventorySlot : MonoBehaviour
{
    public int SlotIndex { get; private set; }
    public Button button;

    [SerializeField] Image spriteRenderer;
    [SerializeField] TextMeshProUGUI ItemName;
    [SerializeField] TextMeshProUGUI DamageQuantity;

    public virtual void SelectSlot()
    {

    }

    public virtual void UpdateSlotData(int slotIndex, Item item)
    {
        button = GetComponent<Button>();
        if (!button) { Debug.LogError($"CANNOT FIND BUTTON ON THIS OBJECT. Name: {this.name}"); return; }
        SlotIndex = slotIndex;
    }
}
