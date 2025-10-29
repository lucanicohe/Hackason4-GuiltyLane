using UniRx;
public class GameContext
{   
    public ReactiveProperty<HitPersonData> TeamA = new();
    public ReactiveProperty<HitPersonData> TeamB = new();
    
    public Direction LastTrainDirection = Direction.Right;
    
    public IntReactiveProperty Gold = new IntReactiveProperty(0);
    public IntReactiveProperty GuiltyPercent = new IntReactiveProperty(0);
    public ReactiveProperty<(int gold, int guilty)> GoldGuiltySum = new((0,0));  
    public int Salary = 12;
    
    public ReactiveProperty<RoundLevel> Round = new(RoundLevel.None);
    public ReactiveProperty<TurnLevel> Turn = new(TurnLevel.None);
    public IntReactiveProperty Toll = new IntReactiveProperty(0);
    public ReactiveProperty<(int toll, RoundLevel Round, TurnLevel Turn)> TollForkSum = new((0,0,0));
}
