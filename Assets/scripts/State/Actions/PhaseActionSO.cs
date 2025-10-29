using System.Collections;
using UnityEngine;

public abstract class PhaseActionSO : ScriptableObject
{
    public abstract IEnumerator Execute(GameContext gameContext);
}