using UnityEngine;

public class BuyBallsButton : MonoBehaviour
{
    public void OnClickBuyBalls()
    {
        GameManager.Instance.BuyBalls();
    }
}