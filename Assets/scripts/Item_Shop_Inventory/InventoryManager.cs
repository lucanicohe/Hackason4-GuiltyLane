using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject _inventory;
    [SerializeField] private GameObject _goldAndGuilt;
    [SerializeField, Header("Item Slot List")] public InventoryItemSlot[] inventoryItemSlot;

    private bool _inventoryActivated;
    private bool _goldAndGuiltActivated;
    public string itemSlotCurrentSelectItemName;

    [SerializeField, Header("ItemSOs")] public ItemSO[] itemSOs;

    public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription)
    {
        int remaining = quantity;

        for (int i = 0; i < inventoryItemSlot.Length; i++)
        {
            var slot = inventoryItemSlot[i];
            if (slot.itemName == itemName && !slot.isFull)
            {
                remaining = slot.AddItem(itemName, remaining, itemSprite, itemDescription);
                if (remaining <= 0) return 0;
            }
        }

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
        Debug.Log("OnUseItem");
        for (int i = 0; i < itemSOs.Length; i++)
        {
            if (itemSOs[i].itemName == itemSlotCurrentSelectItemName)
            {
                itemSOs[i].UseItem();
            }
        }
    }


    void Start()
    {
        _inventory.SetActive(false);
        _inventoryActivated = false;
        _goldAndGuilt.SetActive(true);
        _goldAndGuiltActivated = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !_inventoryActivated)
        {
            if (_goldAndGuiltActivated)
            {
                _goldAndGuilt.SetActive(false);
                _goldAndGuiltActivated = false;
            }
            _inventory.SetActive(true);
            _inventoryActivated = true;
        }
        else if (Input.GetKeyDown(KeyCode.E) && _inventoryActivated)
        {
            _inventory.SetActive(false);
            _inventoryActivated = false;

            _goldAndGuilt.SetActive(true);
            _goldAndGuiltActivated = true;
        }
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
