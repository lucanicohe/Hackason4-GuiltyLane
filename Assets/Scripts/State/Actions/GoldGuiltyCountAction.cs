using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "CreateAction/GoldGuiltyCountAction",fileName = "GoldGuiltyCountAction")]
public class GoldGuiltyCountAction : PhaseActionSO
{      
    private HitPersonData _chooseData;
    public override IEnumerator Execute(GameContext gameContext)
    {
        switch (gameContext.LastTrainDirection)
        {
            case Direction.Right:
                _chooseData = gameContext.TeamA;
                break;
            case Direction.Left:
                _chooseData = gameContext.TeamB;
                break;
            default:
                _chooseData = null;
                Debug.Log("<color=red>ERROR NO TEAM CHOOSE</color>");
                break;
        }
        if (_chooseData != null)
        {
            gameContext.Gold.Value += _chooseData.TotalInheritance;
            gameContext.GuiltyPercent.Value += _chooseData.TotalGuiltyPercent;
        }
        yield break;
    }
}