using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject shop;
    private bool _shopActivated;
    [SerializeField, Header("Item Slot List")] public ShopItemSlot[] shopItemSlot;
    [SerializeField] private GameManager _gameManager;


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
                // _gameManager.gold -= shopItemSlot[i].itemPrice;
                shopItemSlot[i].AddItem();
                shopItemSlot[i].selectedShader.SetActive(false);
                shopItemSlot[i].thisItemSelected = false;
            }
        }
    }
}
