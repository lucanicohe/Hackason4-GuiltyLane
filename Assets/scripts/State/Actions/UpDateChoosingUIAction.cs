using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "CreateAction/UpDateChoosingUIAction",fileName = "UpDateChoosingUIAction")]
public class UpDateChoosingUIAction : PhaseActionSO
{
    private const int BE_DISGUISED_INT = -1;
    private const string BE_DISGUISED_STRING = "■■■■";
    public override IEnumerator Execute(GameContext gameContext)
    {
        //update UI 
        yield return gameContext.mainUIViewer.UpdateTrainText();
        var randomNum = UnityEngine.Random.Range(0, 2);
        switch (gameContext.Round.Value)
        {
            case RoundLevel.First:
                break;
            case RoundLevel.Second:
                DisguisedPersonData(randomNum == 0 ? gameContext.TeamA :  gameContext.TeamB, gameContext);
                break;
            case RoundLevel.Third:
                //両方の身分が分からなくなる
                DisguisedPersonData(gameContext.TeamA, gameContext);
                DisguisedPersonData(gameContext.TeamB, gameContext);
                break;
            case RoundLevel.Fourth:
                DisguisedPersonData(randomNum == 0 ? gameContext.TeamA :  gameContext.TeamB, gameContext);
                break;
            case RoundLevel.Fifth:
                DisguisedPersonData(gameContext.TeamA, gameContext);
                DisguisedPersonData(gameContext.TeamB, gameContext);
                break;
            case RoundLevel.Sixth:
                //自分の罪率と金が分からなくなる
                DisguisedPersonData(gameContext.TeamA, gameContext);
                DisguisedPersonData(gameContext.TeamB, gameContext);
                break;
        }
        gameContext.mainUIViewer.SetUpdateTrainText(gameContext.TeamA, gameContext.TeamB);
    }
    
    private HitPersonData DisguisedPersonData(HitPersonData data,GameContext gameContext)
    {
        data = InitialiZateData(data);
        switch (gameContext.Round.Value)
        {
            case RoundLevel.First:
                return data;
            case RoundLevel.Second:
                //どちらかの身分が分からなくなる
                //お金もわからくなる
                data.SelectRoleToShow = BE_DISGUISED_STRING;
                data.TotalInheritanceToShow = BE_DISGUISED_INT;
                return data;
            case RoundLevel.Third:
                //両方の身分が分からなくなる
                data.SelectRoleToShow = BE_DISGUISED_STRING;
                data.TotalInheritanceToShow = BE_DISGUISED_INT;
                return data;
            case RoundLevel.Fourth:
            case RoundLevel.Fifth:
            case RoundLevel.Sixth:
                //自分の罪率と金が分からなくなる TODO add 
                data.SelectRoleToShow = BE_DISGUISED_STRING;
                data.PersonnumToShow = BE_DISGUISED_INT;
                data.TotalInheritanceToShow = BE_DISGUISED_INT;
                data.TotalGuiltyPercentToShow = BE_DISGUISED_INT;
                return data;
            default: return data;
        }
    }
    
    private HitPersonData InitialiZateData(HitPersonData data)
    {
        data.SelectRoleToShow = data.SelectRole;
        data.PersonnumToShow = data.Personnum;
        data.TotalInheritanceToShow = data.TotalInheritance;
        data.TotalGuiltyPercentToShow = data.TotalGuiltyPercent;
        return data;
    }
}