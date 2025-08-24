using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int Gold = 0;
    public int GuiltyPercent = 0;

    void Start()
    {
        AudioManager.Instance.PlayBGM(AudioManager.E_BGMType.罪人の街);
    }
}
