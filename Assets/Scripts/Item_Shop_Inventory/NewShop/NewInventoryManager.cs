using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewInventoryManager : MonoBehaviour
{   
    [SerializeField] private  List<NewInventoryItemSlot> _normalItemSlots;
    [SerializeField] private  List<NewInventoryItemSlot> _permanentInventoryItemSlots;
    
    private NewInventoryItemSlot _currentItemSlot;
    private ItemSOBase _currentItemSO; 
    
    //== ITEM DESCRIPTION SLOT ==//
    [SerializeField] private Image ItemDescriptionImage;
    [SerializeField] private TMP_Text ItemDescriptionNameText;
    [SerializeField] private TMP_Text ItemDescriptionText;
    private void Awake()
    {
        foreach (var v in _normalItemSlots)
        {
            v.OnItemSlotClick += VOnItemSlotClick;
        }
        foreach (var v in _permanentInventoryItemSlots)
        {
            v.OnItemSlotClick += VOnItemSlotClick;
        }
    }

    private void VOnItemSlotClick(NewInventoryItemSlot obj)
    {
        if (_currentItemSlot != null)
        {
            _currentItemSlot.SetItemShader(false);
        }
        _currentItemSlot = obj;
        _currentItemSlot.SetItemShader(true);
        var isob = obj.GetItemSOBase();
        UpdateCurrentItem(isob);
    }
    
    public void AddItemInInventory(ItemSOBase item)
    {
        if (item.permanent)
        {
            var iso = CheckSlot(item,_permanentInventoryItemSlots);
            iso.SetItemSOBase(item,1);
            iso.SetItemImage();
        }
        else
        {
            var iso = CheckSlot(item,_normalItemSlots);
            iso.SetItemSOBase(item,1);
            iso.SetItemImage();
        }
    }

    public void UseItem()//bind to button
    {
        if (_currentItemSO == null)
        {   
            Debug.Log("No item selected");
            return;
        }
        _currentItemSO.UseItem(null);//TODO add context
        _currentItemSlot.SetItemShader(false);
        _currentItemSlot.SetItemSOBase(_currentItemSO,-1);
        _currentItemSlot = null;
        _currentItemSO =  null;
    }

    private NewInventoryItemSlot CheckSlot(ItemSOBase item,List<NewInventoryItemSlot> list)
    {   
        foreach (NewInventoryItemSlot slot in list)
        {
            var ib = slot.GetItemSOBase();
            if (ib == null)return slot;
            if (item.itemID.Equals(ib.itemID))return slot;
        }
        return null;
    }
    
    private void UpdateCurrentItem(ItemSOBase itemSO)
    {   
        this._currentItemSO = itemSO;
        UpdateItemDescription(itemSO);
    }
    private void UpdateItemDescription(ItemSOBase itemSO)
    {   
        if(itemSO == null)return;
        ItemDescriptionImage.sprite = itemSO.itemSprite;
        ItemDescriptionNameText.text =  itemSO.itemName;
        ItemDescriptionText.text = itemSO.itemDescription;
    }
}
