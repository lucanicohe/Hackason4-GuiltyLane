using UnityEngine;

public class HitPersonData
{
    //--役職の名前--
    public string SelectRole = "String.Empty";
    public string SelectRoleToShow = "String.Empty";
    public int RoleID = -1;
    public GameObject Prefab = null;
    
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