using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GameManager : Singleton<GameManager>
{
    public int Gold = 0;
    public int GuiltyPercent = 0;

    public enum RoundLevel
    {
        None,
        First,
        Second,
        Third,
        Fourth,
        Fifth,
        ToEnd,
    }

    public RoundLevel _roundLevel;

    void Update()
    {
        switch (_roundLevel)
        {
            case RoundLevel.None:
                break;
            case RoundLevel.First:
                break;
            case RoundLevel.Second:
                break;
            case RoundLevel.Third:
                break;
            case RoundLevel.Fourth:
                break;
            case RoundLevel.Fifth:
                break;
            case RoundLevel.ToEnd:
                break;
        }
    }

    private void ChangeFirstRound()
    {
        //ラウンドFirstに行く処理
    }
    private void ChangeSecondRound()
    {
        //secondラウンドに行く処理
    }
    private void ChangeThirdRound()
    {
        //thirdラウンドに行く処理
    }
    private void ChangeFourthRound()
    {
        //fourthラウンドに行く処理
    }
    private void ChangeFifthRound()
    {
        //fifthラウンドに行く処理
    }
    private void ChangeToEndRound()
    {
        //toendに行く処理
    }

}
