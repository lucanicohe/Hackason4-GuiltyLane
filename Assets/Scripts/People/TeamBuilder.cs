using System.Collections.Generic;
using UnityEngine;

public class TeamBuilder : MonoBehaviour
{
    [SerializeField] private PeopleData[] _peopleDatas;
    //
    public HitPersonData TeamBuild()
    {
        var data = new HitPersonData
        {
            RoleID = UnityEngine.Random.Range(0, 4)
        };
        RoleInheritance(data);
        data.PersonnumToShow = data.Personnum;
        return data;
    }

    private void RoleInheritance(HitPersonData data)
    {
        PeopleData peopleData = _peopleDatas[data.RoleID];
        data.Personnum = UnityEngine.Random.Range(peopleData.MinNum, peopleData.MaxNum + 1);
        data.SelectRole = peopleData.Name;
        data.Prefab = peopleData.prefab;
        for (int i = 0; i < data.Personnum; i++)
        {
            //遺産
            int InheritanceValues = UnityEngine.Random.Range(peopleData.MinInheritance, peopleData.MaxInheritance + 1);
            //罪率
            int GuiltyPercentValues = UnityEngine.Random.Range(peopleData.MinGuiltyPercent, peopleData.MaxGuiltyPercent + 1);
            data.TotalGuiltyPercent += GuiltyPercentValues;
            data.TotalInheritance +=  InheritanceValues;
        }
    }
}