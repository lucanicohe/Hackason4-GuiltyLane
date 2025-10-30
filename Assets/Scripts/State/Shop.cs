using System.Collections;
using TMPro;
using UnityEngine;
public class Shop : MonoBehaviour
{
    [SerializeField] private GameObject _shopAlertImage;
    [SerializeField] private ShopManager _shopManager;
    [SerializeField] private InventoryManager _inventoryManager;
    
    public void SetShop(bool Active)
    {
        if (Active)
        {
            StartCoroutine(ShowShopAlert(3.0f));
        }
        if (_shopManager) _shopManager.shop.SetActive(Active);
    }

    public void SetInventory(bool Active)
    {
        if (_inventoryManager) _inventoryManager.InventoryActive = Active;
    }

    private IEnumerator ShowShopAlert(float time)
    {
        if (_shopAlertImage) _shopAlertImage.SetActive(true);
        yield return new WaitForSeconds(time);
        if (_shopAlertImage) _shopAlertImage.SetActive(false);
    }

    public void BindShop(GameContext gameContext)
    {
        _shopManager.BindContext(gameContext);
    }
}
