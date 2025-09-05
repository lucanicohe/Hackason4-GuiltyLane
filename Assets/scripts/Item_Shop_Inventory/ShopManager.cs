using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject shop; // The shop panel
    [SerializeField, Header("Item Slot List")] public ShopItemSlot[] shopItemSlot;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private InventoryManager _inventoryManager;
    private string[] _soldOutPermanentItems;

    private void Start()
    {
        // Always keep shop disabled at start
        if (shop != null) shop.SetActive(false);
    }

    // --- Methods for GameManager to control shop --- //
    public void OpenShop()
    {
        if (shop != null) shop.SetActive(true);
    }

    public void CloseShop()
    {
        if (shop != null) shop.SetActive(false);
    }

    public bool IsShopOpen()
    {
        return shop != null && shop.activeSelf;
    }

    // --- Other existing methods --- //
    public void DeselectAllSlots()
    {
        for (int i = 0; i < shopItemSlot.Length; i++)
        {
            shopItemSlot[i].selectedShader.SetActive(false);
            shopItemSlot[i].thisItemSelected = false;
        }
    }

    public bool SoldOutPermanentItemsBool(string itemName)
    {
        if (_soldOutPermanentItems == null) return false;
        for (int i = 0; i < _soldOutPermanentItems.Length; i++)
        {
            if (_soldOutPermanentItems[i] == itemName)
            {
                return true;
            }
        }
        return false;
    }

    public void Payment(int itemPrice)
    {
        if (_gameManager.Gold < itemPrice)
        {
            Debug.Log("Wallet says: nope.");
            return;
        }
        _gameManager.Gold -= itemPrice;        
    }

    public void OnPurchase()
    {
        for (int i = 0; i < shopItemSlot.Length; i++)
        {
            if (shopItemSlot[i].thisItemSelected)
            {
                if (shopItemSlot[i].permanent)
                {
                    if (SoldOutPermanentItemsBool(shopItemSlot[i].itemName))
                    {
                        Debug.Log("Purchased permanent items");
                        return;
                    }
                    Payment(shopItemSlot[i].itemPrice);

                    _inventoryManager.AddPermanentItem(
                        shopItemSlot[i].itemName,
                        shopItemSlot[i].itemSprite,
                        shopItemSlot[i].itemDescription,
                        shopItemSlot[i].permanent
                    );

                    if (_soldOutPermanentItems == null)
                        _soldOutPermanentItems = new string[0];
                    _soldOutPermanentItems = _soldOutPermanentItems.Append(
                        shopItemSlot[i].itemName
                        ).ToArray();
                }
                else
                {
                    Payment(shopItemSlot[i].itemPrice);

                    int leftOverItems = _inventoryManager.AddItem(
                        shopItemSlot[i].itemName,
                        1,
                        shopItemSlot[i].itemSprite,
                        shopItemSlot[i].itemDescription
                    );

                    if (leftOverItems <= 0)
                        Debug.Log("Purchase success. No leftOverItems");
                    else
                        shopItemSlot[i].quantity = leftOverItems;
                }

                shopItemSlot[i].selectedShader.SetActive(false);
                shopItemSlot[i].thisItemSelected = false;
            }
        }
    }
}