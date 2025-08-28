
[System.Serializable]
public class PeopleData
{
    public string Name;
    public int MaxNum;
    public int MinNum;
    public int MaxInheritance;
    public int MinInheritance;
    public int MaxGuiltyPercent;
    public int MinGuiltyPercent;
}

[System.Serializable]
public class PeopleList
{
    public PeopleData[] Data;
}
