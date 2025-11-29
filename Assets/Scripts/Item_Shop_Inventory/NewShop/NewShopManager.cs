using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewShopManager : MonoBehaviour
{
    // [SerializeField] private GameObject shop; // The shop panel
    // [SerializeField] private NewInventoryManager _inventoryManager; 
    [SerializeField] private List<NewShopItemSlot> ShopItemSlots;
    
    private GameContext _gameContext;
    private NewShopItemSlot _currentItemSlot;
    private ItemSOBase _currentItemSO;
    private List<ItemSOBase> SoldPermanentItems = new();
    
    //== ITEM DESCRIPTION SLOT ==//
    [SerializeField] private Image ItemDescriptionImage;
    [SerializeField] private TMP_Text ItemDescriptionNameText;
    [SerializeField] private TMP_Text ItemDescriptionText;
    [SerializeField] private TMP_Text ItemDescriptionPrice;

    private void Start()
    {
        // Always keep shop disabled at start
        // if (shop != null) shop.SetActive(false);
        foreach (var item in ShopItemSlots)
        {
            item.OnItemSlotClick += ItemClickEvent;
        }
    }
    public void BindContext(GameContext gameContext)
    {
        _gameContext =  gameContext;
    }

    private void ItemClickEvent(NewShopItemSlot ItemSlot)
    {
        if (_currentItemSlot != null)
        {
            _currentItemSlot.SetItemShader(false);
        }
        _currentItemSlot =  ItemSlot;
        _currentItemSlot.SetItemShader(true);
        UpdateCurrentItem(ItemSlot.GetItemSOBase());
    }

    private bool Payment(int itemPrice)
    {
        if (_gameContext.Gold.Value < itemPrice)
        {
            Debug.Log("Wallet says: nope.");
            return false;
        }
        Debug.Log("Purchase success.");
        _gameContext.Gold.Value -= itemPrice;
        return true;
    }

    private void UpdateCurrentItem(ItemSOBase itemSO)
    {   
        this._currentItemSO = itemSO;
        UpdateItemDescription(itemSO);
    }
    private void UpdateItemDescription(ItemSOBase itemSO)
    {
        ItemDescriptionImage.sprite = itemSO.itemSprite;
        ItemDescriptionNameText.text =  itemSO.itemName;
        ItemDescriptionText.text = itemSO.itemDescription;
        ItemDescriptionPrice.text = $"${itemSO.price}";
    }

    private void ResetItemDescription()
    {
        ItemDescriptionImage.sprite = null;
        ItemDescriptionNameText.text =  "";
        ItemDescriptionText.text = "";
        ItemDescriptionPrice.text = "$";
    }

    public void OnPurchase()
    {
        // check if is permanent & is solded
        if (_currentItemSlot == null || _currentItemSO  == null)
        {   
            Debug.Log("No current item slot.");
            return;
        }
        if (SoldPermanentItems.Contains(_currentItemSO))
        {
            Debug.Log("item is already exist");
            return;
        }
        if (!Payment(_currentItemSO.price))
        {
            Debug.Log("Payment failed");
            return;
        }
        if (_currentItemSO.permanent)
        {
            SoldPermanentItems.Add(_currentItemSO);
        }
        //add item to inventory
        // _inventoryManager.AddItemInInventory(_currentItemSO);
        Debug.Log($"Add {_currentItemSO.itemName} to inventory");
        foreach (var item in ShopItemSlots)
        {
            item.SetItemShader(false);
        }
        _currentItemSlot = null;
        _currentItemSO = null;
        ResetItemDescription();
    }
}
