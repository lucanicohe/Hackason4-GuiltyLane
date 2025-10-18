using System.Collections.Generic;

public class HitPersonData
{
    //--役職の名前--
    public string SelectRole;
    public string SelectRoleToShow;
    public int RoleID;

    //--ランダムに決まる遺産・人数
    public List<int> Inheritance = new List<int>();
    public int Personnum = 0;
    public int PersonnumToShow = 0;
    //最終的なすべての遺産
    public int TotalInheritance = 0;
    public int TotalInheritanceToShow = 0;

    //罪率
    public List<int> GuiltyPercent = new List<int>();

    //最終的なすべての罪率
    public int TotalGuiltyPercent = 0;
    public int TotalGuiltyPercentToShow = 0;

}