using System.Collections;
using UnityEngine;

public class TeamBuilderAction : PhaseActionSO
{
    [SerializeField] private TeamBuilder teamBuilder;
    private HitPersonData TeamA;
    private HitPersonData TeamB;
    public override IEnumerator Execute(GameContext gameContext)
    {   
        gameContext.TeamA = null;
        gameContext.TeamB = null;
        yield return BuildTeams();
        gameContext.TeamA = TeamA;
        gameContext.TeamB = TeamB;
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