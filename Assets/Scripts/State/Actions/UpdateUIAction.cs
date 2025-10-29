using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Action", menuName = "UpdateUIAction")]
public class UpdateUIAction : PhaseActionSO
{   
    [SerializeField] private MainUIViewer mainUIViewer;
    public override IEnumerator Execute(GameContext gameContext)
    {
        throw new System.NotImplementedException();
    }
}