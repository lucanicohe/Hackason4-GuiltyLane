using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

public class CustomButton : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerClickHandler
{
    [SerializeField] private Image _targetImage;

    [SerializeField] private Sprite _sprite1;
    [SerializeField] private Sprite _sprite2;
    [SerializeField] private Sprite _sprite3;

    [SerializeField] private float _frameTime = 0.1f;

[SerializeField] private CoolMenu _coolMenu;

    private Coroutine _transitionRoutine;

    void Awake()
    {
        _targetImage.sprite = _sprite1;
    }

public void OnPointerEnter(PointerEventData eventData)
{
   
      //  _coolMenu.Pause(true);

    StartTransition(HoverIn());
}

public void OnPointerExit(PointerEventData eventData)
{
  
       // _coolMenu.Pause(false);

    StartTransition(HoverOut());
}

    public void OnPointerClick(PointerEventData eventData)
    {
        SceneManager.LoadScene("MainGameScene");
    }

    private void StartTransition(IEnumerator routine)
    {
        if (_transitionRoutine != null)
            StopCoroutine(_transitionRoutine);

        _transitionRoutine = StartCoroutine(routine);
    }

    private IEnumerator HoverIn()
    {
        _targetImage.sprite = _sprite2;
        yield return new WaitForSeconds(_frameTime);
        _targetImage.sprite = _sprite3;
    }

    private IEnumerator HoverOut()
    {
        _targetImage.sprite = _sprite2;
        yield return new WaitForSeconds(_frameTime);
        _targetImage.sprite = _sprite1;
    }
}