using System.Collections.Generic;
using UnityEngine;

public class HitState : IState
{   
    private List<GameObject> _spawnPeopleList;
    private MainUIManager _mainUIManager;
    public HitState(List<GameObject> spawnPeopleList, MainUIManager mainUIManager)
    {
        _spawnPeopleList =  spawnPeopleList;
        _mainUIManager = mainUIManager;
    }
    public void Enter()
    {
        //clear data UI 
        _mainUIManager.UpdateTrainNormalText();
    }

    public void Exit()
    {
        //delete people prefab
        foreach (GameObject go in _spawnPeopleList)
        {
            GameObject.Destroy(go);
        }
        _spawnPeopleList.Clear();
    }

    public void Execute()
    {
        
    }
}