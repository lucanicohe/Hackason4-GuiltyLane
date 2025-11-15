using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject _inventory;      // The inventory panel
    [SerializeField] private AudioSource _usesound; 
    [SerializeField, Header("Item Slot List")] public InventoryItemSlot[] inventoryItemSlots;
    [SerializeField] public InventoryItemSlot[] permanentInventoryItemSlots;

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
        for (int i = 0; i < inventoryItemSlots.Length; i++)
        {
            var slot = inventoryItemSlots[i];
            if (slot.itemName == itemName && !slot.isFull)
            {
                remaining = slot.AddItem(itemName, remaining, itemSprite, itemDescription);
                if (remaining <= 0) return 0;
            }
        }

        // Add to empty slots
        for (int i = 0; i < inventoryItemSlots.Length; i++)
        {
            var slot = inventoryItemSlots[i];
            if (slot.itemName == "" || slot.quantity == 0)
            {
                remaining = slot.AddItem(itemName, remaining, itemSprite, itemDescription);
                if (remaining <= 0) return 0;
            }
        }

        return remaining;
    }

public void AddPermanentItem(string itemName, Sprite itemSprite, string itemDescription, bool permanent)
{
    for (int i = 0; i < permanentInventoryItemSlots.Length; i++)
    {
        var slot = permanentInventoryItemSlots[i];
        if (slot.itemName == itemName)
        {
            slot.AddPermanentItem(itemName, itemSprite, itemDescription, permanent);
            return;
        }
    }

    for (int i = 0; i < permanentInventoryItemSlots.Length; i++)
    {
        var slot = permanentInventoryItemSlots[i];
        if (string.IsNullOrEmpty(slot.itemName))
        {
            slot.AddPermanentItem(itemName, itemSprite, itemDescription, permanent);
            return;
        }
    }
}


    public void OnUseItem()
    {
        if (currentSelectedSlot == null) return;
        foreach (var itemSO in itemSOs)
        {
            if (itemSO.itemName == currentSelectedSlot.itemName)
            {
                if (currentSelectedSlot.permanent) return;
                
                // itemSO.UseItem();
                // _usesound.Play(); 
                currentSelectedSlot.ReduceQuantity(1);

                // itemSO.UseItem();
                // if (!currentSelectedSlot.permanent)
                // {
                //     currentSelectedSlot.ReduceQuantity(1);
                // }
                // else
                // {
                //     Debug.Log("This item is permanent.");
                // }
            }
        }
    }

    public void OnPurchasePermanentItem(string itemName)
    {
        foreach (var itemSO in itemSOs)
        {
            if (itemSO.itemName == itemName)
            {
                // itemSO.UseItem();
            }
        }
    }

    private void Start()
    {
        if (_inventory != null) _inventory.SetActive(true);
    }

    public void DeselectAllSlots()
    {
        for (int i = 0; i < inventoryItemSlots.Length; i++)
        {
            inventoryItemSlots[i].selectedShader.SetActive(false);
            inventoryItemSlots[i].thisItemSelected = false;
        }
        for (int i = 0; i < permanentInventoryItemSlots.Length; i++)
        {
            permanentInventoryItemSlots[i].selectedShader.SetActive(false);
            permanentInventoryItemSlots[i].thisItemSelected = false;
        }
    }
}