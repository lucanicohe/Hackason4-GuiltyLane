using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "CreateAction/GoldGuiltyCountAction",fileName = "GoldGuiltyCountAction")]
public class GoldGuiltyCountAction : PhaseActionSO
{      
    private HitPersonData _chooseData;
    private HitPersonData _noneChooseData;
    public override IEnumerator Execute(GameContext gameContext)
    {
        switch (gameContext.LastTrainDirection)
        {
            case Direction.Right:
                _chooseData = gameContext.TeamA;
                _noneChooseData =  gameContext.TeamB;
                break;
            case Direction.Left:
                _chooseData = gameContext.TeamB;
                _noneChooseData = gameContext.TeamA;
                break;
            default:
                _chooseData = null;
                Debug.Log("<color=red>ERROR NO TEAM CHOOSE</color>");
                break;
        }
         
        if (_chooseData != null)
        {   
            _chooseData = CheckItemEffect(gameContext, _chooseData);
            if (gameContext.IsInheritanceSystemEffect)
            {
                if (!_chooseData.RoleID.Equals(2) && _noneChooseData.RoleID.Equals(2))
                {
                    gameContext.Gold.Value += _noneChooseData.TotalInheritance;
                    gameContext.GuiltyPercent.Value += _noneChooseData.TotalGuiltyPercent;
                }
                else
                {
                    if (gameContext.IsFamilyDealEffect)
                    {   
                        _chooseData = UseFamilyDeal(gameContext,_chooseData);
                        gameContext.Gold.Value += _noneChooseData.TotalInheritance;
                        gameContext.GuiltyPercent.Value += _noneChooseData.TotalGuiltyPercent;
                    }
                }
            }
            else if (gameContext.IsFamilyDealEffect)
            {   
                _chooseData = UseFamilyDeal(gameContext,_chooseData);
                gameContext.Gold.Value += _noneChooseData.TotalInheritance;
                gameContext.GuiltyPercent.Value += _noneChooseData.TotalGuiltyPercent;
            }
            gameContext.Gold.Value += _chooseData.TotalInheritance;
            gameContext.GuiltyPercent.Value += _chooseData.TotalGuiltyPercent;
        }
        yield break;
    }
    
    private HitPersonData CheckItemEffect(GameContext context,HitPersonData data)
    {
        if (context.IsBlessingOfThePriestEffect)
        {
            data = UseBlessingOfThePriest(context,data);
        }
        if (context.IsCollectorEffect)
        {   
            data = UseCollector(context,data);
        }
        if (context.IsDealEffect)
        {   
            data = UseDeal(context,data);
        }
        if (context.IsDefenseAttorneyEffect)
        {   
            data = UseDefenseAttorneyEffect(context,data);
        }
        if (context.IsInvestmentEffect)
        {   
            data = UseInvestmentEffect(context,data);
        }
        return data; 
    }
    
    private HitPersonData UseFamilyDeal(GameContext context,HitPersonData data)
    {   
        data.TotalGuiltyPercent = Mathf.RoundToInt(data.TotalGuiltyPercent * 1.1f); 
        return data;  
    }
    private HitPersonData UseDefenseAttorneyEffect(GameContext context,HitPersonData data)
    {   
        data.TotalInheritance = Mathf.RoundToInt(data.TotalInheritance * 1.2f);//TODO magic number
        return data;
    }

    private HitPersonData UseBlessingOfThePriest(GameContext context,HitPersonData data)
    {   
        data.TotalGuiltyPercent /= 2;
        return data;
    }
    
    private HitPersonData UseCollector(GameContext context,HitPersonData data)
    {
        var gp = context.GuiltyPercent.Value;
        var per = 1.0f + (gp * 2.0f/100);
        data.TotalInheritance = Mathf.RoundToInt(data.TotalInheritance * per);
        return data;
    }
    
    private HitPersonData UseDeal(GameContext context,HitPersonData data)
    {
        if (data.RoleID.Equals(2))
        {
            data.TotalInheritance = Mathf.RoundToInt(data.TotalInheritance * 1.2f);//TODO magic number
        }
        return data;
    }

    private HitPersonData UseInvestmentEffect(GameContext context, HitPersonData data)
    {
        data.TotalInheritance *= 5;
        return data;
    }
}