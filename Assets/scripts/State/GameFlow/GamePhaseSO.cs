using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameFlow/Phase")]
public class GamePhaseSO : ScriptableObject
{
    public string phaseName;
    public bool isExitTriggerEnter;
    public GamePhaseSO nextPhase;
    public List<PhaseActionSO> actions;
}