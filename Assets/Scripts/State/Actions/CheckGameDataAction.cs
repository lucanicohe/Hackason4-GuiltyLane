using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "CreateAction/CheckGameDataAction",fileName = "CheckGameDataAction")]
public class CheckGameDataAction : PhaseActionSO
{
    private readonly int[] _tollRate = { 0, 15, 15, 30, 30, 40, 40 };
    public override IEnumerator Execute(GameContext gameContext)
    {
        if (gameContext.GuiltyPercent.Value <= 100)
        {
            GameFlowController.Instance.StopGameFlow();
        }
        gameContext.Toll.Value = (int)gameContext.Round.Value * _tollRate[(int)gameContext.Round.Value];
        //check it need to Switch Round
        gameContext.Turn.Value ++;
        if (gameContext.Turn.Value == TurnLevel.SwitchRound)
        {
            if (gameContext.Gold.Value < gameContext.Toll.Value)
            {
                GameFlowController.Instance.StopGameFlow();
            }
            gameContext.Turn.Value = TurnLevel.First;
            gameContext.Round.Value++;
        }
        if (gameContext.Round.Value == RoundLevel.ToEnd)
        {
            GameFlowController.Instance.StopGameFlow();
        }
        yield break;
    }
}