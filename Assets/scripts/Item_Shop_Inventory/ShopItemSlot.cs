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
    [SerializeField,Header("Item Data")]
    public string itemName;
    public int itemPrice;
    public int quantity = 1;
    public Sprite itemSprite;
    public string itemDescription;
    public bool permanent;
    [SerializeField, Header("Settings")] private ItemSO[] _itemSoElements;
    public ItemSO[] itemSoElements => _itemSoElements;
    [SerializeField] private SelectItem _selectedItem = new SelectItem();

    private enum SelectItem
    {
        // GOV.
        OfficialIdentityHint,
        OfficialNumberHint,
        // Prison
        Deal,
        InheritanceSystem,
        Guarantee,
        // Law
        DefenseAttorney,
        FinancialLawyer,
        FinancialExpert,
        // Church
        BlessingOfThePriest,
        ChurchsBlessing,
        PapalAmnesty,
        // Corp.
        FamilyDeal,
        Collector,
        Investment,
        GoldLoan,
    }

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

    void Start()
    {
        string selectedName = _selectedItem.ToString();
        foreach (var item in _itemSoElements)
        {
            if (item.itemName == selectedName)
            {
                itemName = item.itemName;
                itemPrice = item.price;
                itemSprite = item.itemSprite;
                itemDescription = item.itemDescription;
                _itemImage.sprite = item.itemSprite;
                permanent = item.permanent;
                break;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {   
        var go = eventData.lastPress;
        Debug.Log(go);
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
