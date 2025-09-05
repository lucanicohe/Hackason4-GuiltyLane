using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject _inventory;      // The inventory panel
    [SerializeField, Header("Item Slot List")] public InventoryItemSlot[] inventoryItemSlot;

    public InventoryItemSlot currentSelectedSlot;

    [SerializeField, Header("ItemSOs")] public ItemSO[] itemSOs;

    // Public property to show/hide inventory
    public bool InventoryActive
    {
        get => _inventory.activeSelf;
        set
        {
            if (_inventory != null) _inventory.SetActive(value);
        }
    }

    public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription)
    {
        int remaining = quantity;

        // Try adding to existing slots first
        for (int i = 0; i < inventoryItemSlot.Length; i++)
        {
            var slot = inventoryItemSlot[i];
            if (slot.itemName == itemName && !slot.isFull)
            {
                remaining = slot.AddItem(itemName, remaining, itemSprite, itemDescription);
                if (remaining <= 0) return 0;
            }
        }

        // Add to empty slots
        for (int i = 0; i < inventoryItemSlot.Length; i++)
        {
            var slot = inventoryItemSlot[i];
            if (slot.itemName == "" || slot.quantity == 0)
            {
                remaining = slot.AddItem(itemName, remaining, itemSprite, itemDescription);
                if (remaining <= 0) return 0;
            }
        }

        return remaining;
    }

    public void OnUseItem()
    {
        if (currentSelectedSlot == null) return;

        foreach (var itemSO in itemSOs)
        {
            if (itemSO.itemName == currentSelectedSlot.itemName)
            {
                itemSO.UseItem();
                currentSelectedSlot.ReduceQuantity(1);
            }
        }
    }

    private void Start()
    {
        if (_inventory != null) _inventory.SetActive(true);
    }

    public void DeselectAllSlots()
    {
        for (int i = 0; i < inventoryItemSlot.Length; i++)
        {
            inventoryItemSlot[i].selectedShader.SetActive(false);
            inventoryItemSlot[i].thisItemSelected = false;
        }
    }
}