using UnityEngine;

public abstract class ItemSOBase : ScriptableObject
{ 
        [SerializeField, Header("Item Data")] public Sprite itemSprite; 
        [SerializeField] public string itemName;
        [TextArea] public string itemDescription;
        [SerializeField] public int price;
        [SerializeField] public bool permanent;

        public abstract void UseItem();
}