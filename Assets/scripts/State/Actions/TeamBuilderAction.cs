using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "CreateAction/TeamBuilderAction",fileName = "TeamBuilderAction")]
public class TeamBuilderAction : PhaseActionSO
{
    private HitPersonData TeamA;
    private HitPersonData TeamB;
    private TeamBuilder teamBuilder;
    public override IEnumerator Execute(GameContext gameContext)
    {   
        teamBuilder = gameContext.teamBuilder;
        yield return BuildTeams();
        gameContext.TeamA = TeamA;
        gameContext.TeamB = TeamB;
        Debug.Log(JsonUtility.ToJson(gameContext.TeamA,true));
        Debug.Log(JsonUtility.ToJson(gameContext.TeamB,true));
    }

    private IEnumerator BuildTeams()
    {
        HitPersonData a;
        HitPersonData b;
        do
        {
            a = teamBuilder.TeamBuild();
            b = teamBuilder.TeamBuild();
            yield return null;
        } while (a.RoleID == b.RoleID);

        TeamA = a;
        TeamB = b;
    }
}