using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "CreateAction/GameOverAction",fileName = "GameOverAction")]
public class GameOverAction : PhaseActionSO
{
    // [SerializeField] private AudioSource _gunshot;
    // [SerializeField] private AudioSource _sad;
    // [SerializeField] private PostProcessVolume _postProcessVolume;
    // [SerializeField] private Image _fadeImage;
    // [SerializeField] private GameObject _fadeobject; 
    public override IEnumerator Execute(GameContext gameContext)
    {
        yield break;
    }
    
    // private IEnumerator Sewerslide()
    // {
    //     _sad.Play();
    //     yield return new WaitForSeconds(3f);
    //     _gunshot.Play();
    //     yield return new WaitForSeconds(1f);
    //     ChangeCamera(5);
    //     
    //     
    //
    //     if (_fadeImage != null)
    //     {
    //         float elapsed = 0f;
    //         Color color = _fadeImage.color;
    //         color.a = 0f;
    //         _fadeImage.color = color;
    //
    //         while (elapsed < 1f)
    //         {
    //             elapsed += Time.deltaTime;
    //             color.a = Mathf.Clamp01(elapsed / 1f);
    //             _fadeImage.color = color;
    //             yield return null;
    //         }
    //
    //         color.a = 1f;
    //         _fadeImage.color = color;
    //     }
    // }
}