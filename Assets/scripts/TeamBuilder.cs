using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TeamBuilder : MonoBehaviour
{
    // //--役職ごとの人数の上限--
    // private int MaxRoyaltynum = 2;
    // private int MaxNobilitynum = 4;
    // private int MaxCitizennum = 5;
    // private int MaxPrisonernum = 10;

    // //--役職ごとの人数の下限
    // private int MinRoyaltynum = 1;
    // private int MinNobilitynum = 2;
    // private int MinCitizennum = 3;
    // private int MinPrisonernum = 5;

    // //--役職ごとの遺産上限
    // private int MaxRoyaltyInheritance = 30;
    // private int MaxNobilityInheritance = 12;
    // private int MaxCitizenInheritance = 8;
    // private int MaxPrisonerInheritance = 3;

    // //--役職ごとの遺産の下限
    // private int MinRoyaltyInheritance = 15;
    // private int MinNobilityInheritance = 6;
    // private int MinCitizenInheritance = 3;
    // private int MinPrisonerInheritance = 0;

    // //--役職ごとの罪率の上限
    // private int MaxRoyaltyGuiltyPercent = 10;
    // private int MaxNobilityGuiltyPercent = 6;
    // private int MaxCitizenGuiltyPercent = 4;
    // private int MaxPrisonerGuiltyPercent = 2;

    // //--役職ごとの罪率の下限
    // private int MinRoyaltyGuiltyPercent = 6;
    // private int MinNobilityGuiltyPercent = 3;
    // private int MinCitizenGuiltyPercent = 1;
    // private int MinPrisonerGuiltyPercent = 0;

    private PeopleData Royal;
    private PeopleData Nobility;
    private PeopleData Citizen;
    private PeopleData Prisoner;

    private void Start()
    {
        LoadPeopleDataFromJson();
    }

    private void LoadPeopleDataFromJson()
    {
        string Path = Application.dataPath + "/PeopleDate.json";
        PeopleList peopleList = SaveLoadManager.Instance.LoadDataFromJson<PeopleList>(Path);
        if (peopleList == null)
        {
            Debug.Log($"not founded peopleList in {Path}");
            return;
        }
        List<PeopleData> peopleDatas = peopleList.peopleDatas;
        foreach (var people in peopleDatas)
        {
            string name = people.Name;
            switch (name){
                case "Royal":
                    Royal = people;
                    break;
                case "Nobility":
                    Nobility = people;
                    break;
                case "Citizen":
                    Citizen = people;
                    break;
                case "Prisoner":
                    Prisoner = people;
                    break;
            }
        }
    }

    //--役職一覧--
    private List<string> Role = new List<string>()
    {
        "Royal(王族)",
        "Noble(貴族)",
        "Citizen(市民)",
        "Prisoner(囚人)",

    };

    public class HitPersonData
    {
        //--役職の名前--
        public string SelectRole;

        //--ランダムに決まる遺産・人数
        public List<int> Inheritance = new List<int>();
        public int Personnum = 0;

        //最終的なすべての遺産
        public int TotalInheritance = 0;

        //罪率
        public List<int> GuiltyPercent = new List<int>();

        //最終的なすべての罪率
        public int TotalGuiltyPercent = 0;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var right = TeamBuild();
            var left = TeamBuild();
            Debug.Log($"rightrole:{right.SelectRole}\nrightperson:{right.Personnum}\nrightinheritance:{right.Inheritance}\nrighttotalinheritance:{right.TotalInheritance}\nrightGuiltyPercent:{right.GuiltyPercent}\nrighttotalGuiltypercent:{right.TotalGuiltyPercent}");
            Debug.Log($"leftrole:{left.SelectRole}\nleftperson:{left.Personnum}\nleftinheritance:{left.Inheritance}\nlefttotalinheritance:{left.TotalInheritance}\nleftGuiltyPercent:{left.GuiltyPercent}\nlefttotalGuiltypercent:{left.TotalGuiltyPercent}");
        }
    }

    /// <summary>
    /// 役職と人数と罪率をランダムに生成する
    /// </summary>
    /// <returns></returns>
    public HitPersonData TeamBuild()
    {
        var data = new HitPersonData();
        data.SelectRole = Role[UnityEngine.Random.Range(0, 4)];
        switch (data.SelectRole)
        {
            case "Royal(王族)":
                RoleInheritance(data, Royal);
                break;
            case "Noble(貴族)":
                RoleInheritance(data, Nobility);
                break;
            case "Citizen(市民)":
                RoleInheritance(data, Citizen);
                break;
            case "Prisoner(囚人)":
                RoleInheritance(data, Prisoner);
                break;
        }
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
