using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject _inventory;
    [SerializeField] private GameObject _goldAndGuilt;
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
}
