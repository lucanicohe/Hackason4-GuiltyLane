using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject shop;
    private bool _shopActivated;
    [SerializeField, Header("Item Slot List")] public ShopItemSlot[] shopItemSlot;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private InventoryManager _inventoryManager;


    void Update()
    {
        // == Shop SetActive toggle ==//
        if (Input.GetKeyDown(KeyCode.P) && _shopActivated)
        {
            shop.SetActive(false);
            _shopActivated = false;
        }
        else if (Input.GetKeyDown(KeyCode.P) && !_shopActivated)
        {
            shop.SetActive(true);
            _shopActivated = true;
        }
    }

    public void DeselectAllSlots()
    {
        for (int i = 0; i < shopItemSlot.Length; i++)
        {
            shopItemSlot[i].selectedShader.SetActive(false);
            shopItemSlot[i].thisItemSelected = false;
        }
    }

    public void OnPurchase()
    {
        for (int i = 0; i < shopItemSlot.Length; i++)
        {
            if (shopItemSlot[i].thisItemSelected)
            {
                _gameManager.Gold -= shopItemSlot[i].itemPrice;

                int leftOverItems = _inventoryManager.AddItem(
                    shopItemSlot[i].itemName,
                    1,
                    shopItemSlot[i].itemSprite,
                    shopItemSlot[i].itemDescription
                );

                if (leftOverItems <= 0)
                {
                    Debug.Log("Purchase success. No leftOverItems");
                }
                else
                {
                    Debug.Log($"leftOverItems is {leftOverItems}");
                    shopItemSlot[i].quantity = leftOverItems;
                }

                shopItemSlot[i].selectedShader.SetActive(false);
                shopItemSlot[i].thisItemSelected = false;
            }
        }
    }
}
