using System;
using UnityEngine;

public class ChoosingState : IState
{   
    private TeamBuilder _teamBuilder;
    private HitPersonData _rightHpd;
    private HitPersonData _leftHpd;
    
    public event Action<HitPersonData,HitPersonData> OnTeamBuildFinished;

    public ChoosingState(TeamBuilder teamBuilder)
    {
        _teamBuilder = teamBuilder;
    }
    public void Enter()
    {
        //get people data 
        BuildPeopleData();
        //instance people prefab
        //show data UI 
        OnTeamBuildFinished?.Invoke(_leftHpd, _rightHpd);
    }

    public void Exit()
    {
        
    }

    public void Execute()
    {
       
    }

    private void BuildPeopleData()
    {
        var R = _teamBuilder.TeamBuild();
        var L = _teamBuilder.TeamBuild();
        if (R.SelectRole != L.SelectRole)
        {
            _rightHpd = R;
            _leftHpd = L;
            Debug.Log(JsonUtility.ToJson(_rightHpd, true));
            Debug.Log(JsonUtility.ToJson(_leftHpd, true));
            return;
        }
        else
        {
            BuildPeopleData();
        }
    }
}