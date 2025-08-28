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

    [SerializeField]
    public GameObject Royal;

    [SerializeField]
    public GameObject Noble;

    [SerializeField]
    public GameObject Citizen;

    [SerializeField]
    public GameObject Prisoner;

    // 役職が割り振られたか
    public bool IsRoleDecide = false;

    // prefabがDestroyされたか
    public bool IsPrefabDestroy = true;

    private GameObject RightTeam;
    private GameObject _rightTeam; // Clone用
    private GameObject LeftTeam;
    private GameObject _leftTeam; // Clone用

    // 右側チームの生成地点 (値は適当)
    private Vector3 _posStartRightTeam = new Vector3(3f, 1.7f, 30f);

    // 左側チームの生成地点 (値は適当)
    private Vector3 _posStartLeftTeam = new Vector3(-2f, 1.7f, 30f);

    // 右側チームをDestroyする地点 (値は適当)
    private Vector3 _posEndRightTeam = new Vector3(3f, 1.7f, 5.5f);

    // 左側チームをDestroyする地点 (値は適当)
    private Vector3 _posEndLeftTeam = new Vector3(-2f, 1.7f, 5.5f);


    /// <summary>
    /// GameManagerによって決められたRoleに合わせて生成するPrefabを決める
    /// </summary>
    /// <param name="_roleRightTeam"></param>
    /// <param name="_roleLeftTeam"></param>
    public void SetPrefabRole(TeamBuilder.HitPersonData _roleRightTeam, TeamBuilder.HitPersonData _roleLeftTeam)
    {
        switch (_roleRightTeam.SelectRole)
        {
            case "Royal(王族)":
                RightTeam = Royal;
                break;
            case "Noble(貴族)":
                RightTeam = Noble;
                break;
            case "Citizen(市民)":
                RightTeam = Citizen;
                break;
            case "Prisoner(囚人)":
                RightTeam = Prisoner;
                break;
        }

        switch (_roleLeftTeam.SelectRole)
        {
            case "Royal(王族)":
                LeftTeam = Royal;
                break;
            case "Noble(貴族)":
                LeftTeam = Noble;
                break;
            case "Citizen(市民)":
                LeftTeam = Citizen;
                break;
            case "Prisoner(囚人)":
                LeftTeam = Prisoner;
                break;
        }
        IsRoleDecide = true;
    }

    // Prefabの生成
    public void MakePrefab()
    {
        _rightTeam = Instantiate(RightTeam, _posStartRightTeam, Quaternion.identity);
        _leftTeam = Instantiate(LeftTeam, _posStartLeftTeam, Quaternion.identity);
        IsPrefabDestroy = false;
    }

    // 右側のチームを列車に向かって移動させる
    public IEnumerator MoveRightPrefab()
    {
        Vector3 pos = _posStartRightTeam;
        while (pos.z > _posEndRightTeam.z && _rightTeam != null)
        {
            pos = _rightTeam.transform.position;
            pos.z += 1.5f * 60f * -1 * Time.deltaTime;
            _rightTeam.transform.position = pos;

            yield return null;
        }

        // 列車に近づいたらどちらのチームもDestroyする
        Destroy(_rightTeam);
        Destroy(_leftTeam);
        _rightTeam = null;
        IsPrefabDestroy = true;
        IsRoleDecide = false;
    }

    // 左側のチームを列車に向かって移動させる
    public IEnumerator MoveLeftPrefab()
    {
        // 列車に向けて移動
        Vector3 pos = _posStartLeftTeam;
        while (pos.z > _posEndLeftTeam.z && _leftTeam != null)
        {
            pos = _leftTeam.transform.position;
            pos.z += 1.5f * 60f * -1 * Time.deltaTime;
            _leftTeam.transform.position = pos;

            yield return null;
        }

        // 列車に近づいたらどちらのチームもDestroyする
        Destroy(_leftTeam);
        Destroy(_rightTeam);
        _leftTeam = null;
        IsPrefabDestroy = true;
        IsRoleDecide = false;
    }
}
