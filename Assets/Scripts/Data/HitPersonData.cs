using System.Collections.Generic;
using UnityEngine;

public class HitPersonData
{
    //--役職の名前--
    public string SelectRole;
    public string SelectRoleToShow;
    public int RoleID;
    public GameObject Prefab;
    
    //--ランダムに決まる遺産・人数
    public int Personnum = 0;
    public int PersonnumToShow = 0;
    //最終的なすべての遺産
    public int TotalInheritance = 0;
    public int TotalInheritanceToShow = 0;

    //最終的なすべての罪率
    public int TotalGuiltyPercent = 0;
    public int TotalGuiltyPercentToShow = 0;

}