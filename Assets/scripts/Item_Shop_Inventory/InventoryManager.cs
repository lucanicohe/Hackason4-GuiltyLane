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

    public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription)
    {
        int remaining = quantity;

        // 既存の同じアイテムにスタックできるか確認
        for (int i = 0; i < inventoryItemSlot.Length; i++)
        {
            var slot = inventoryItemSlot[i];
            if (slot.itemName == itemName && !slot.isFull)
            {
                remaining = slot.AddItem(itemName, remaining, itemSprite, itemDescription);
                if (remaining <= 0) return 0; // 全て入りきった
            }
        }

        // 空きスロットに追加
        for (int i = 0; i < inventoryItemSlot.Length; i++)
        {
            var slot = inventoryItemSlot[i];
            if (slot.itemName == "" || slot.quantity == 0)
            {
                remaining = slot.AddItem(itemName, remaining, itemSprite, itemDescription);
                if (remaining <= 0) return 0;
            }
        }

        // ここまでで入りきらなかった分を返す
        return remaining;
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
