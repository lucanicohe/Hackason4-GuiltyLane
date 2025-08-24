using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamBuilder : MonoBehaviour
{
    //--人数の上限--
    private int MaxRoyaltynum = 2;
    private int MaxNobilitynum = 4;
    private int MaxCitizennum = 5;
    private int MaxPrisonernum = 10;

    //--人数の下限
    private int MinRoyaltynum = 1;
    private int MinNobilitynum = 2;
    private int MinCitizennum = 3;
    private int MinPrisonernum = 5;

    //--役職ごとの遺産上限
    private int MaxRoyaltyInheritance = 30;
    private int MaxNobilityInheritance = 12;
    private int MaxCitizenInheritance = 8;
    private int MaxPrisonerInheritance = 3;

    //--役職ごとの遺産の下限
    private int MinRoyaltyInheritance = 15;
    private int MinNobilityInheritance = 6;
    private int MinCitizenInheritance = 3;
    private int MinPrisonerInheritance = 0;
    
    //--役職一覧--
    private List<string> Role = new List<string>()
    {
        "Royal(王族)",
        "Noble(貴族)",
        "Citizen(市民)",
        "Prisoner(囚人)",

    };

    private class HitPersonData
    {
        //--役職の名前--
        public string SelectRole;

        //--ランダムに決まる遺産・人数
        public List<int> Inheritance = new List<int>();
        public int Personnum = 0;

        //最終的なすべての遺産
        public int TotalInheritance = 0;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var right = TeamBuild();
            var left = TeamBuild();
            Debug.Log($"rightrole:{right.SelectRole}\nrightperson:{right.Personnum}\nrightinheritance:{right.Inheritance}\nrighttotalinheritance:{right.TotalInheritance}");
            Debug.Log($"leftrole:{left.SelectRole}\nleftperson:{left.Personnum}\nleftinheritance:{left.Inheritance}\nlefttotalinheritance:{left.TotalInheritance}");
        }
    }
    private HitPersonData TeamBuild()
    {
        var data = new HitPersonData();
        data.SelectRole = Role[UnityEngine.Random.Range(0, 4)];
        switch (data.SelectRole)
        {
            case "Royal(王族)":
                RoleInheritance(data, MinRoyaltynum, MaxRoyaltynum, MinRoyaltyInheritance, MaxRoyaltyInheritance);
                break;
            case "Noble(貴族)":
                RoleInheritance(data, MinNobilitynum, MaxNobilitynum, MinNobilityInheritance, MaxNobilityInheritance);
                break;
            case "Citizen(市民)":
                RoleInheritance(data, MinCitizennum, MaxCitizennum, MinCitizenInheritance, MaxCitizenInheritance);
                break;
            case "Prisoner(囚人)":
                RoleInheritance(data, MinPrisonernum, MaxPrisonernum, MinPrisonerInheritance, MaxPrisonerInheritance);
                break;
        }
        return data;
    }

    private void RoleInheritance(HitPersonData data, int MinNum, int MaxNum, int MinInheritance, int MaxInheritance)
    {
        data.Personnum = UnityEngine.Random.Range(MinNum, MaxNum + 1);
        for (int i = 0; i < data.Personnum; i++)
        {
            int InheritanceValues = UnityEngine.Random.Range(MinInheritance, MaxInheritance + 1);
            data.Inheritance.Add(InheritanceValues);
        }
        for (int i = 0; i < data.Personnum; i++)
        {
            data.TotalInheritance += data.Inheritance[i];
        }
    }
}
