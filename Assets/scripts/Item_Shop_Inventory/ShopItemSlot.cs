using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopItemSlot : MonoBehaviour, IPointerClickHandler
{
    // public Sprite emptySprite;

    //== Item Data ==//  
    [SerializeField] public string itemName;
    [SerializeField] public int itemPrice;
    [SerializeField] public int quantity = 1;
    [SerializeField] public Sprite itemSprite;
    [TextArea]
    [SerializeField] public string itemDescription;

    //== Item SLOT ==//  
    [SerializeField] private Image _itemImage;

    //== ITEM DESCRIPTION SLOT ==//
    public Image ItemDescriptionImage;
    public TMP_Text ItemDescriptionNameText;
    public TMP_Text ItemDescriptionText;
    public TMP_Text ItemDescriptionPrice;

    [SerializeField] public GameObject selectedShader;
    [SerializeField] private ShopManager _shopManager;
    [SerializeField] private InventoryManager _inventoryManager;
    public bool thisItemSelected;

    public void AddItem()
    {
        int leftOverItems = _inventoryManager.AddItem(itemName, quantity, itemSprite, itemDescription);
        if (leftOverItems <= 0) ;
        else quantity = leftOverItems;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }
    }

    public void OnLeftClick()
    {
        _shopManager.DeselectAllSlots();
        selectedShader.SetActive(true);
        thisItemSelected = true;
        ItemDescriptionNameText.text = itemName;
        ItemDescriptionText.text = itemDescription;
        ItemDescriptionImage.sprite = itemSprite;
        ItemDescriptionPrice.text = $"{itemPrice} Gold";
        // if (ItemDescriptionImage.sprite == null) ItemDescriptionImage.sprite = emptySprite;
    }

    public void OnRightClick()
    {

    }
}
