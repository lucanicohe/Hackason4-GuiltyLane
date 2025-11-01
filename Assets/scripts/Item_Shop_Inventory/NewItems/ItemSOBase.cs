using UnityEngine;

public abstract class ItemSOBase : ScriptableObject
{ 
        [SerializeField, Header("Item Data")] public Sprite itemSprite; 
        [SerializeField] public string itemName;
        [SerializeField] public int itemID;
        [TextArea] public string itemDescription;
        [SerializeField] public int price;
        [SerializeField] public bool permanent;
        [SerializeField] public int quantity;

        public abstract void UseItem();
}