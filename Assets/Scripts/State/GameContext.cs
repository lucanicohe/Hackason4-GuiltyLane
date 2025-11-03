using UniRx;
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
    
    public IntReactiveProperty Gold = new(10);
    public IntReactiveProperty GuiltyPercent = new(10);
    public int Salary = 12;
    
    public ReactiveProperty<RoundLevel> Round = new(RoundLevel.First);
    public ReactiveProperty<TurnLevel> Turn = new(TurnLevel.First);
    public IntReactiveProperty Toll = new(0);
    
    
}
