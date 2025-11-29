using System.Collections;
using TMPro;
using UnityEngine;
public class Shop : MonoBehaviour
{
    [SerializeField] private NewShopManager _shopManager;
    
    public void BindShop(GameContext gameContext)
    {
        _shopManager.BindContext(gameContext);
    }
}
