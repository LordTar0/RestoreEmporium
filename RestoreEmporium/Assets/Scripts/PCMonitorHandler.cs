using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PCMonitorHandler : MonoBehaviour
{
    [SerializeField] private GameObject InventorySlotPrefab;

    [SerializeField] private StoreTab store;
    [SerializeField] private InventoryTab inventory;
}

[System.Serializable]
public class StoreTab
{
    public GameObject Tab;
    public DescriptionTab DescriptionTab;
    public Button PurchaseButton;
    public TextMeshProUGUI ButtonText;

    public void UpdateTab(string name, string description, int price, int damagequantity, int discountamount)
    {
        DescriptionTab.UpdateTab(name, description, price, damagequantity, discountamount);

    }
}

[System.Serializable]
public class InventoryTab
{
    public GameObject Tab;
    public DescriptionTab DescriptionTab;
}

[System.Serializable]
public class DescriptionTab
{
    public DiscountBar DiscountBar;

    public TextMeshProUGUI Name;
    public TextMeshProUGUI Description;
    public TextMeshProUGUI Price;
    public TextMeshProUGUI DamageQuantity;

    public Image Icon;

    public void UpdateTab(string name, string description, int price, int damagequantity, int discountamount)
    {
        Name.text = name;
        Description.text = description;
        Price.text = $"£{price}";
        DamageQuantity.text = $"{damagequantity}%";

        DiscountBar.UpdateQuantity(discountamount);
    }
}