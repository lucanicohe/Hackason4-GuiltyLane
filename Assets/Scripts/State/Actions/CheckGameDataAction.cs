using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "CreateAction/CheckGameDataAction",fileName = "CheckGameDataAction")]
public class CheckGameDataAction : PhaseActionSO
{
    private readonly int[] _tollRate = { 0, 15, 15, 30, 30, 40, 40 };
    public override IEnumerator Execute(GameContext gameContext)
    {
        gameContext.Gold.Value += gameContext.Salary;
        if (gameContext.GuiltyPercent.Value >= 100)
        {
            GameFlowController.Instance.StopGameFlow("TOO GUILTY");
        }
        gameContext.Toll.Value = (int)gameContext.Round.Value * _tollRate[(int)gameContext.Round.Value];
        //check it need to Switch Round
        gameContext.Turn.Value ++;
        if (gameContext.Turn.Value == TurnLevel.SwitchRound)
        {
            var tollGold = gameContext.Toll.Value;
            if (gameContext.IsFinancialExpert)
            {
                tollGold = Mathf.RoundToInt(tollGold * 0.8f);
                gameContext.IsFinancialExpert =  false;
            }
            if (gameContext.Gold.Value < tollGold)
            {
                GameFlowController.Instance.StopGameFlow("NO MONEY");
                yield break;
            }
            gameContext.Gold.Value -= tollGold;
            gameContext.Turn.Value = TurnLevel.First;
            gameContext.Round.Value++;
        }
        if (gameContext.Round.Value == RoundLevel.ToEnd)
        {
            GameFlowController.Instance.StopGameFlow("GAME CLEAR");
        }
        yield break;
    }
}