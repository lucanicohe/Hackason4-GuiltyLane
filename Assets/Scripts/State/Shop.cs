using System.Collections;
using TMPro;
using UnityEngine;
public class Shop : MonoBehaviour
{
    [SerializeField] private GameObject _shopAlertImage;
    [SerializeField] private NewShopManager _shopManager;
    [SerializeField] private NewInventoryManager _inventoryManager;
    [SerializeField] private GameObject _shop;
    [SerializeField] private GameObject _inventory;
    
    public void SetShop(bool Active)
    {
        if (Active)
        {
            StartCoroutine(ShowShopAlert(3.0f));
        }
        if (_shop) _shop.SetActive(Active);
    }

    public void SetInventory(bool Active)
    {
        if (_inventory) _inventory.SetActive(Active);
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
