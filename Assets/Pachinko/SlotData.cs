using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Slot")]
public class SlotData : ScriptableObject
{
    public List<SlotParam> slotParams = new();    
}

[System.Serializable]
public class SlotParam
{
    [SerializeField] public Sprite[] numbers;
    [SerializeField] public int id;
    [SerializeField] public int probability;
}
