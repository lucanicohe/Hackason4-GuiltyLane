using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameFlow/Phase")]
public class GamePhaseSO : ScriptableObject
{
    public string phaseName;
    public float duration = 30f;
    public GamePhaseSO nextPhase;
    public List<PhaseActionSO> actions;
}