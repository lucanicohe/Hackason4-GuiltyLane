using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class GameContext
{      
    public SpawnPeopleFactory spawnPeopleFactory;
    public Shop Shop;
    public TeamBuilder teamBuilder;
    public MainUIViewer mainUIViewer;
    public NewTrainController NewTrainController;
    public HitPersonData TeamA;
    public HitPersonData TeamB;
    public Direction LastTrainDirection = Direction.Right;
    public List<GameObject> TeamAPeoplePrefabs;    
    public List<GameObject> TeamBPeoplePrefabs;
    public Transform SpawnPointTeamA;
    public Transform SpawnPointTeamB;
    
    public IntReactiveProperty Gold = new(10);
    public IntReactiveProperty GuiltyPercent = new(10);
    public int Salary = 12;
    
    public ReactiveProperty<RoundLevel> Round = new(RoundLevel.First);
    public ReactiveProperty<TurnLevel> Turn = new(TurnLevel.First);
    public IntReactiveProperty Toll = new(0);
    
    
}
