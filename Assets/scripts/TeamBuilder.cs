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
    //--右の役職の名前--
    private string RightSelectRole;

    //--左の役職の名前--
    private string LeftSelectRole;
    private List<string> Role = new List<string>()
    {
        "Royal(王族)",
        "Noble(貴族)",
        "Citizen(市民)",
        "Prisoner(囚人)",

    };

    //--右のランダムに決まる遺産・人数
    int RightInheritance = 0;
    int RightPersonnum = 0;
    //--左のランダムに決まる遺産・人数
    int LeftInheritance = 0;
    int LeftPersonnum = 0;

    //最終的なすべての遺産
    int RightTotalInheritance = 0;
    int LeftTotalInheritance = 0;


    private void Start()
    {
        RightTeamBulid();
        LeftTeamBuild();
    }
    public void RightTeamBulid()
    {
        RightSelectRole = Role[UnityEngine.Random.Range(0, 3)];
        switch (LeftSelectRole)
        {
            case "Royal(王族)":
                RightPersonnum = UnityEngine.Random.Range(MinRoyaltynum, MaxRoyaltynum + 1);
                RightInheritance = UnityEngine.Random.Range(MinRoyaltyInheritance, MaxRoyaltyInheritance + 1);
                RightTotalInheritance = RightInheritance * RightPersonnum;
                break;
            case "Noble(貴族)":
                RightPersonnum = UnityEngine.Random.Range(MinNobilitynum, MaxNobilitynum + 1);
                RightInheritance = UnityEngine.Random.Range(MinNobilityInheritance, MaxNobilityInheritance + 1);
                RightTotalInheritance = RightInheritance * RightPersonnum;
                break;
            case "Citizen(市民)":
                RightPersonnum = UnityEngine.Random.Range(MinCitizennum, MaxCitizennum + 1);
                RightInheritance = UnityEngine.Random.Range(MinCitizenInheritance, MaxCitizenInheritance + 1);
                RightTotalInheritance = RightInheritance * RightPersonnum;
                break;
            case "Prisoner(囚人)":
                RightPersonnum = UnityEngine.Random.Range(MinPrisonernum, MaxPrisonernum + 1);
                RightInheritance = UnityEngine.Random.Range(MinPrisonerInheritance, MaxPrisonerInheritance + 1);
                RightTotalInheritance = RightInheritance * RightPersonnum;
                break;
        }
    }

    private void LeftTeamBuild()
    {
        LeftSelectRole = Role[UnityEngine.Random.Range(0, 3)];
        switch (RightSelectRole)
        {
            case "Royal(王族)":
                LeftPersonnum = UnityEngine.Random.Range(MinRoyaltynum, MaxRoyaltynum + 1);
                LeftInheritance = UnityEngine.Random.Range(MinRoyaltyInheritance, MaxRoyaltyInheritance + 1);
                LeftTotalInheritance = LeftInheritance * LeftPersonnum;
                break;
            case "Noble(貴族)":
                LeftPersonnum = UnityEngine.Random.Range(MinNobilitynum, MaxNobilitynum + 1);
                LeftInheritance = UnityEngine.Random.Range(MinNobilityInheritance, MaxNobilityInheritance + 1);
                LeftTotalInheritance = LeftInheritance * LeftPersonnum;
                break;
            case "Citizen(市民)":
                LeftPersonnum = UnityEngine.Random.Range(MinCitizennum, MaxCitizennum + 1);
                LeftInheritance = UnityEngine.Random.Range(MinCitizenInheritance, MaxCitizenInheritance + 1);
                LeftTotalInheritance = LeftInheritance * LeftPersonnum;
                break;
            case "Prisoner(囚人)":
                LeftPersonnum = UnityEngine.Random.Range(MinPrisonernum, MaxPrisonernum + 1);
                LeftInheritance = UnityEngine.Random.Range(MinPrisonerInheritance, MaxPrisonerInheritance + 1);
                LeftTotalInheritance = LeftInheritance * LeftPersonnum;
                break;
        }
    }
}
