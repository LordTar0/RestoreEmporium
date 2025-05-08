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
    [SerializeField] private Canvas myCanvas;
    [SerializeField] private GameDetailsVisuals gameDetailVisuals;
    [SerializeField] private Transform Window;

    public void OpenStoreTab()
    {
        bool isActive = store.Tab.activeInHierarchy;
        store.Tab.SetActive(!isActive);
        store.Tab.transform.SetAsLastSibling();
    }

    public void OpenInventoryTab()
    {
        bool isActive = inventory.Tab.activeInHierarchy;
        inventory.Tab.SetActive(!isActive);
        inventory.Tab.transform.SetAsLastSibling();
    }

    private void Start()
    {
        myCanvas = GetComponent<Canvas>();
        myCanvas.worldCamera = Camera.main;
    }
}

[System.Serializable]
public class StoreTab : DescriptionTab
{
    public GameObject Tab;

    public Button PurchaseButton;
    public TextMeshProUGUI ButtonText;

    public void UpdateStoreTab(string name, string description, int price, int damagequantity, int discountamount, bool issold)
    {
        UpdateTab(name, description, price, damagequantity, discountamount);

        if (issold)
        {
            PurchaseButton.interactable = false;
            ButtonText.text = $"Item sold";
        }
        else
        {
            PurchaseButton.interactable = true;
            ButtonText.text = $"Purchase Item";
        }
    }
}

[System.Serializable]
public class InventoryTab : DescriptionTab
{
    public GameObject Tab;

    public Button RepairButton;
    public TextMeshProUGUI ButtonText;

    public override void UpdateTab(string name, string description, int price, int damagequantity, int discountamount)
    {
        base.UpdateTab(name, description, price, damagequantity, discountamount);

        if (damagequantity > 0)
        {
            RepairButton.interactable = true;
            ButtonText.text = $"Repair item";
        }
        else
        {
            RepairButton.interactable = false;
            ButtonText.text = $"Item is already fixed";
        }
    }
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

    public virtual void UpdateTab(string name, string description, int price, int damagequantity, int discountamount)
    {
        Name.text = name;
        Description.text = description;
        Price.text = $"£{price}";
        DamageQuantity.text = $"{damagequantity}%";

        DiscountBar.UpdateQuantity(discountamount);
    }
}

public class TabElement
{
    public Animator myAnimator;

    public void OpenWindow(bool isOpen)
    {
        myAnimator.SetBool("OpenWindow",isOpen);
    }
}