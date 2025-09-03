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
        var leftOverItems = 4;
        return leftOverItems;
    }

    void Start()
    {
        if (_inventoryActivated && !_goldAndGuiltActivated)
        {
            _inventory.SetActive(false);
            _inventoryActivated = false;

            _goldAndGuilt.SetActive(true);
            _goldAndGuiltActivated = true;
        }
        else if (!_goldAndGuiltActivated && !_inventoryActivated)
        {
            _goldAndGuilt.SetActive(true);
            _goldAndGuiltActivated = true;
        }
        else if (_goldAndGuiltActivated && _inventoryActivated)
        {
            _inventory.SetActive(false);
            _inventoryActivated = false;
        }
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
