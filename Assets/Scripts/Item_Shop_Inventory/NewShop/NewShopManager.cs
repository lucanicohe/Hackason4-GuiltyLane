using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NewShopManager : MonoBehaviour,IPointerClickHandler
{
    [SerializeField] private GameObject shop; // The shop panel
    [SerializeField] private InventoryManager _inventoryManager; 
    
    private GameContext _gameContext;
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
        if (shop != null) shop.SetActive(false);
    }
    public void BindContext(GameContext gameContext)
    {
        _gameContext =  gameContext;
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

    public void OnPurchase()
    {
        // check if is permanent & is solded
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
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
    }
}
