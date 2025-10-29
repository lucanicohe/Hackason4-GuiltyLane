using UniRx;
public class GameContext
{      
    public SpawnPeopleFactory spawnPeopleFactory;
    public Shop Shop;
    public TeamBuilder teamBuilder;
    public MainUIViewer mainUIViewer;
    public HitPersonData TeamA;
    public HitPersonData TeamB;
    
    public Direction LastTrainDirection = Direction.Right;
    
    public IntReactiveProperty Gold = new(0);
    public IntReactiveProperty GuiltyPercent = new(0);
    public int Salary = 12;
    
    public ReactiveProperty<RoundLevel> Round = new(RoundLevel.None);
    public ReactiveProperty<TurnLevel> Turn = new(TurnLevel.None);
    public IntReactiveProperty Toll = new(0);
}
