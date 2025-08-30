using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PrefabController : MonoBehaviour
{
    // "Royal(王族)",
    // "Noble(貴族)",
    // "Citizen(市民)",
    // "Prisoner(囚人)",
    [Header("Prefabs")]
    [SerializeField]
    public GameObject Royal;

    [SerializeField]
    public GameObject Noble;

    [SerializeField]
    public GameObject Citizen;

    [SerializeField]
    public GameObject Prisoner;

    [Header("Settings")]

    [SerializeField] // prefabが動く速度
    private float _moveSpeed = 1f;
    
    // prefabがDestroyされているか
    public bool IsPrefabDestroy = true;

    private GameObject RightTeam;
    private GameObject _cloneRightTeam; // Clone用
    private GameObject LeftTeam;
    private GameObject _cloneLeftTeam; // Clone用

    // インスタンス化した複数のprefabを入れる配列
    private List<GameObject> CloneRightArr = new List<GameObject>();
    private List<GameObject> CloneLeftArr = new List<GameObject>();

    // 右側チームの生成地点 (値は適当)
    private Vector3 _posStartRightTeam = new Vector3(3f, 5.7f, 30f);

    // 左側チームの生成地点 (値は適当)
    private Vector3 _posStartLeftTeam = new Vector3(-2f, 5.7f, 30f);

    // 右側チームをDestroyする地点 (値は適当)
    private Vector3 _posEndRightTeam = new Vector3(3f, 5.7f, 5.5f);

    // 左側チームをDestroyする地点 (値は適当)
    private Vector3 _posEndLeftTeam = new Vector3(-2f, 5.7f, 5.5f);

    /// <summary>
    /// HitPersonData.SelectRoleから、どのロールをインスタンス化するか仕分ける
    /// TeamBuilder内に書くのもあるけど、複雑になりそうなのでやめました。
    /// </summary>
    /// <param name="_team"></param>
    /// <param name="_teamSide"></param>
    /// <returns></returns>
    private GameObject DivideRole(TeamBuilder.HitPersonData _team, GameObject _teamSide)
    {
        switch (_team.SelectRole)
        {
            case "Royal":
                _teamSide = Royal;
                break;
            case "Noble":
                _teamSide = Noble;
                break;
            case "Citizen":
                _teamSide = Citizen;
                break;
            case "Prisoner":
                _teamSide = Prisoner;
                break;
        }

        return _teamSide;
    }

    public void SetPrefabRole(TeamBuilder.HitPersonData _roleRightTeam, TeamBuilder.HitPersonData _roleLeftTeam)
    {
        RightTeam = DivideRole(_roleRightTeam, RightTeam);
        LeftTeam = DivideRole(_roleLeftTeam, LeftTeam);
    }

    /// <summary>
    /// Prefabの生成、それらを配列に入れる
    /// </summary>
    /// <param name="_personNumRight"></param>
    /// <param name="_personNumLeft"></param>
    public void MakePrefab(TeamBuilder.HitPersonData _personNumRight, TeamBuilder.HitPersonData _personNumLeft)
    {
        // 配列の初期化
        CloneRightArr.Clear();
        CloneLeftArr.Clear();

        // 人数分のprefabの生成
        for (float i = 0; i < _personNumRight.Personnum; i++)
        {
            _cloneRightTeam = Instantiate(RightTeam, _posStartRightTeam + new Vector3(i * 1.5f, 0f, 0f), Quaternion.identity);
            CloneRightArr.Add(_cloneRightTeam);
        }

        for (float i = 0; i < _personNumLeft.Personnum; i++)
        {
            _cloneLeftTeam = Instantiate(LeftTeam, _posStartLeftTeam + new Vector3(-i * 1.5f, 0f, 0f), Quaternion.identity);
            CloneLeftArr.Add(_cloneLeftTeam);
        }
        IsPrefabDestroy = false;
    }

    /// <summary>
    /// Prefabを電車に轢かれる感じの位置まで動かす
    /// </summary>
    /// <param name="_personNumRight"></param>
    /// <param name="_personNumLeft"></param>
    /// <returns></returns>
    public IEnumerator MoveRightPrefab(TeamBuilder.HitPersonData _personNumRight, TeamBuilder.HitPersonData _personNumLeft)
    {
        for (int i = 0; i < _personNumRight.Personnum; i++) // prefabの人数分動かす
        {
            Vector3 pos = _posStartRightTeam;

            while (pos.z > _posEndRightTeam.z && CloneRightArr[i] != null)
            {
                for (int j = 0; j < _personNumRight.Personnum; j++) // prefabたちの足並みを揃える
                {
                    if (CloneRightArr[j] != null) // nullチェック
                    {
                        pos = CloneRightArr[j].transform.position;
                        pos.z += _moveSpeed * 60f * -1 * Time.deltaTime;
                        CloneRightArr[j].transform.position = pos;
                    }
                }

                yield return null;
            }

            Destroy(CloneRightArr[i]);
            CloneRightArr[i] = null;
        }

        for (int i = 0; i < _personNumLeft.Personnum; i++) Destroy(CloneLeftArr[i]);

        IsPrefabDestroy = true;

        //　参考に
        // bool isAllActive = true;
        // while (isAllActive)
        // {
        //     isAllActive = false;
        //     foreach (var prefab in CloneRightArr)
        //     {
        //         if (prefab == null) {continue;}
        //         Vector3 pos = prefab.transform.position;
        //         if (pos.z >= _posEndRightTeam.z)
        //         {
        //             pos.z += _moveSpeed * -1 * Time.deltaTime;
        //             prefab.transform.position = pos;
        //             isAllActive = true;
        //         }
        //         else
        //         {
        //             Destroy(prefab);
        //             continue;
        //         }
        //     }
        //     yield return null;
        // }
        // for (int i = 0; i < CloneLeftArr.Count; i++) Destroy(CloneLeftArr[i]);
        // IsPrefabDestroy = true;
    }

    public IEnumerator MoveLeftPrefab(TeamBuilder.HitPersonData _personNumRight, TeamBuilder.HitPersonData _personNumLeft)
    {
        for (int i = 0; i < _personNumLeft.Personnum; i++) // prefabの人数分動かす
        {
            Vector3 pos = _posStartLeftTeam;

            while (pos.z > _posEndLeftTeam.z && CloneLeftArr[i] != null)
            {
                for (int j = 0; j < _personNumLeft.Personnum; j++) // prefabたちの足並みを揃える
                {
                    if (CloneLeftArr[j] != null) // nullチェック
                    {
                        pos = CloneLeftArr[j].transform.position;
                        pos.z += _moveSpeed * 60f * -1 * Time.deltaTime;
                        CloneLeftArr[j].transform.position = pos;
                    }
                }

                yield return null;
            }

            Destroy(CloneLeftArr[i]);
            CloneLeftArr[i] = null;
        }

        for (int i = 0; i < _personNumRight.Personnum; i++) Destroy(CloneRightArr[i]);

        IsPrefabDestroy = true;
    }


    /*
    private bool IsFade = false;
    [SerializeField]
    private UnityEngine.UI.Image _fadeImage;

    private float _fadeTime = 0.5f;

    private float _elapsedTime = 0f;

    private IEnumerator OnHitFade()
    {
        _fadeImage.raycastTarget = true;
        while (_elapsedTime < _fadeTime)
        {
            _elapsedTime += Time.deltaTime;
            float per = _elapsedTime / _fadeTime;

            Color C = _fadeImage.color;
            C.a = per;
            _fadeImage.color = C;
            yield return null;
        }

        _elapsedTime = 0f;

        while (_elapsedTime < _fadeTime)
        {
            _elapsedTime += Time.deltaTime;
            float per = 1 - (_elapsedTime / _fadeTime);

            Color C = _fadeImage.color;
            C.a = per;
            _fadeImage.color = C;
            yield return null;
        }
        _fadeImage.raycastTarget = false;
    }
    */
}
