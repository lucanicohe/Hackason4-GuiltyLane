using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NewInventoryItemSlot : MonoBehaviour, IPointerClickHandler
{
    private ItemSOBase itemSO;
    private int _itemNum;
    public event Action<NewInventoryItemSlot> OnItemSlotClick;
    private Image _itemImage;
    private Image _itemShader;
    private TMP_Text _quantityText;

    private void Awake()
    {
        _itemImage = GetComponentInChildren<ImageTag>().GetComponent<Image>();
        _quantityText = GetComponentInChildren<TMP_Text>();
        _itemShader = GetComponentInChildren<ImageShaderTag>().GetComponent<Image>();
        SetItemShader(false);
    }
    public void SetItemShader(bool Active)
    {
        _itemShader.enabled = Active;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        OnItemSlotClick?.Invoke(this);
    }

    public void SetItemImage()
    {
        _itemImage.sprite = itemSO.itemSprite;
    }
    public ItemSOBase GetItemSOBase()
    {
        return itemSO;
    }
    public void SetItemSOBase(ItemSOBase itemSO,int num)
    {
        if (this.itemSO == itemSO)
        {
            _itemNum += num;
            _quantityText.text = $"x{_itemNum}";
        }
        else
        {
            this.itemSO = itemSO;
            _itemNum += num;
            _quantityText.text = $"x{_itemNum}";
        }
        if (_itemNum == 0)
        {
            this.itemSO = null;
            _itemImage.sprite =  null;
            _quantityText.text = "";
        }
    }
}
