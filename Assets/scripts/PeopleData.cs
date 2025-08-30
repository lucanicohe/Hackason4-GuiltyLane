using UnityEngine;

[System.Serializable , CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PeopleData", order = 2)]
public class PeopleData : ScriptableObject
{
    public string Name;
    public int MaxNum;
    public int MinNum;
    public int MaxInheritance;
    public int MinInheritance;
    public int MaxGuiltyPercent;
    public int MinGuiltyPercent;
}

[System.Serializable , CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PeopleList", order = 1)]
public class PeopleList : ScriptableObject
{
    public PeopleData[] Data;
}
