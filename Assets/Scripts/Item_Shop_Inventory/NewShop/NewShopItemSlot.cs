using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NewShopItemSlot : MonoBehaviour,IPointerClickHandler
{
    [SerializeField] private ItemSOBase itemSO;
    
    public event Action<NewShopItemSlot> OnItemSlotClick;
    private Image _itemImage;
    private Image _itemShader;

    private void Awake()
    {
        _itemImage = GetComponentInChildren<ImageTag>().GetComponent<Image>();
        _itemShader = GetComponentInChildren<ImageShaderTag>().GetComponent<Image>();

        _itemImage.sprite = itemSO.itemSprite;
    }

    public void SetItemShader(bool Active)
    {
        _itemShader.enabled = Active;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        OnItemSlotClick?.Invoke(this);
    }

    public ItemSOBase GetItemSOBase()
    {
        return itemSO;
    }
}
