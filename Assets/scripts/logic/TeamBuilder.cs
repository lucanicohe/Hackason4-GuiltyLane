using System.Collections.Generic;
using UnityEngine;

public class TeamBuilder : MonoBehaviour
{
    private PeopleData _royalData;
    private PeopleData _nobilityData;
    private PeopleData _citizenData;
    private PeopleData _prisonerData;

    private void Awake()
    {
        LoadPeopleFromScriptableObjects();
    }

    [SerializeField] private PeopleData[] _peopleDatas;
    private void LoadPeopleFromScriptableObjects()
    {
        foreach (var people in _peopleDatas)
        {
            string name = people.Name;
            // Debug.Log($"name : {name}");
            switch (name)
            {
                case "Royal":
                    _royalData = people;
                    break;
                case "Nobility":
                    _nobilityData = people;
                    break;
                case "Citizen":
                    _citizenData = people;
                    break;
                case "Prisoner":
                    _prisonerData = people;
                    break;
            }
        }
    }

    //--役職一覧--
    private List<string> Role = new List<string>()
    {
        "Royal",
        "Noble",
        "Citizen",
        "Prisoner",
    };

    /// <summary>
    /// 役職と人数と罪率をランダムに生成する
    /// </summary>
    /// <returns></returns>
    public HitPersonData TeamBuild()
    {
        var data = new HitPersonData();
        data.RoleID = UnityEngine.Random.Range(0, 4);
        data.SelectRole = Role[data.RoleID];
        switch (data.SelectRole)
        {
            case "Royal":
                RoleInheritance(data, _royalData);
                break;
            case "Noble":
                RoleInheritance(data, _nobilityData);
                break;
            case "Citizen":
                RoleInheritance(data, _citizenData);
                break;
            case "Prisoner":
                RoleInheritance(data, _prisonerData);
                break;
        }
        data.PersonnumToShow = data.Personnum;
        return data;
    }

    public void RoleInheritance(HitPersonData data, PeopleData peopleData)
    {
        data.Personnum = UnityEngine.Random.Range(peopleData.MinNum, peopleData.MaxNum + 1);
        for (int i = 0; i < data.Personnum; i++)
        {
            //遺産
            int InheritanceValues = UnityEngine.Random.Range(peopleData.MinInheritance, peopleData.MaxInheritance + 1);
            data.Inheritance.Add(InheritanceValues);
            data.TotalInheritance += data.Inheritance[i];
            //罪率
            int GuiltyPercentValues = UnityEngine.Random.Range(peopleData.MinGuiltyPercent, peopleData.MaxGuiltyPercent + 1);
            data.GuiltyPercent.Add(GuiltyPercentValues);
            data.TotalGuiltyPercent += data.GuiltyPercent[i];
        }
    }
}