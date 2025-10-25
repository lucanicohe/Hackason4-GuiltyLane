using System;
using System.Collections.Generic;
using UnityEngine;

public class CSVRender : MonoBehaviour
{
    public static CSVRender Instance { get; private set; }

    // 読み込むcsvファイル
    [SerializeField] private TextAsset _csvTextAssetData;

    // メインの文字格納エリア
    private Dictionary<int, string> _currentLangWords = new Dictionary<int, string>();

    // 現在の言語コード（0=ENG, 1=JP）
    private int _currentLangCode = 0;
    // 外からもロードされている状態が確認できるように。 currentLangCodeを外で読む
    public int currentLangCode => _currentLangCode;

    public void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        LoadLanguage();
    }

    /// <summary>
    /// 指定言語のデータをCSVから読み込み
    /// </summary>
    public void LoadLanguage()
    {
        var c = _currentLangCode;
        var wList = _currentLangWords;

        if (_csvTextAssetData == null)
        {
            Debug.LogError("CSV file not assigned!");
            return;
        }

        // _currentLangWordsをリセット
        wList.Clear();

        // Splitで文字列を区切り、\nを区切り文字に指定
        // StringSplitOptions.RemoveEmptyEntriesは空の行をスキップするオプション
        string[] lines = _csvTextAssetData.text.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

        // 1行目はヘッダーをスキップ
        for (int i = 1; i < lines.Length; i++)
        {
            //　中身を整理するためにi行を一旦全て書き出し
            string[] fields = lines[i].Split(',');

            // 存在しない列があっても止まらないように　（一部言語を書き忘れていたなどに対応）
            if (fields.Length <= c + 1)
                continue;

            // idに対応した文字を返す為に準備。idが数字じゃなかった場合に処理が止まらないようにTryParseを使用。
            if (int.TryParse(fields[0], out int id))
            {
                string word = fields[c + 1].Trim();
                wList[id] = word;
            }
            else Debug.Log($"{_csvTextAssetData.name}の{i}列のTextIDが数字じゃないよ? ({id})");
        }

        Debug.Log($"Loaded language code {c} ({wList.Count} entries)");
    }

    /// <summary>
    /// 現在の言語で単語を取得
    /// </summary>
    public string getTxtByID(int id)
    {
        if (_currentLangWords.TryGetValue(id, out string word))
            return replace(word);
        return $"<Missing:{id}>";
    }

    public void ShiftLang()
    {
        if (_currentLangCode == 0)
            _currentLangCode = 1;
        else if (_currentLangCode == 1)
            _currentLangCode = 0;
        else return;
        LoadLanguage();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            ShiftLang();
    }

    public string replace(string text)
    {
        char oldChar = '`';
        char newChar = ',';
        string replacedTxt = text.Replace(oldChar, newChar);

        return replacedTxt; 
    }
}