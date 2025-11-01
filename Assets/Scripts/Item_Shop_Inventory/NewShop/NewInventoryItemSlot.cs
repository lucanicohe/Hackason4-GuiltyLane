
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NewInventoryItemSlot : MonoBehaviour, IPointerClickHandler
{
    // public Sprite emptySprite;

    //== Item Data ==//  
    [SerializeField, Header("・Item data")]
    public string itemName;
    public int itemPrice;
    public int quantity;
    public Sprite itemSprite;
    public string itemDescription;
    public bool permanent;

    [SerializeField] private int _maxNumberOfItems;

    //== Item SLOT ==//  
    [SerializeField, Header("・Item slot")] private Image _itemImage;
    [SerializeField] private TMP_Text _quantityText;
    [SerializeField] public GameObject selectedShader;

    //== ITEM DESCRIPTION SLOT ==//
    [SerializeField, Header("・Item description slot")] public Image ItemDescriptionImage;
    [SerializeField] public TMP_Text ItemDescriptionNameText;
    [SerializeField] public TMP_Text ItemDescriptionText;
    [SerializeField] public TMP_Text ItemDescriptionQuantity;

    //== Other ==//
    [SerializeField, Header("・Other")] private ShopManager _shopManager;
    [SerializeField] private InventoryManager _inventoryManager;
    [SerializeField] private GameObject _useButton;
    public bool thisItemSelected;
    public bool isFull;

    public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription)
    {
        //Check to see if the slot is already full
        if (isFull) return quantity;

        // Update Name
        this.itemName = itemName;

        // Update Image
        this.itemSprite = itemSprite;
        _itemImage.sprite = itemSprite;

        // Update Permanent

        // Update Description
        this.itemDescription = itemDescription;

        // Update Quantity
        this.quantity += quantity;
        if (this.quantity >= _maxNumberOfItems)
        {
            _quantityText.text = $"x{_maxNumberOfItems}";
            _quantityText.enabled = true;
            isFull = true;

            // Return the LeftOvers
            int extraItems = this.quantity - _maxNumberOfItems;
            this.quantity = _maxNumberOfItems;
            return extraItems;
        }
        // Update Quantity Text
        _quantityText.text = $"x{this.quantity}";
        _quantityText.enabled = true;

        return 0;
    }

    public void AddPermanentItem(string itemName, Sprite itemSprite, string itemDescription, bool permanent)
    {
        //Check to see if the slot is already full
        if (isFull) return;

        // Update Name
        this.itemName = itemName;

        // Update Image
        this.itemSprite = itemSprite;
        _itemImage.sprite = itemSprite;

        // Update Description
        this.itemDescription = itemDescription;

        this.quantity = 1;
        this.permanent = permanent;
        isFull = true;     
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
    }

    public void OnLeftClick()
    {
        
    }
}
