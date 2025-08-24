using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    private AudioClip[] _arrayBGMClips;
    private Dictionary<string, AudioSource> _BGMdictionary = new Dictionary<string, AudioSource>();
    private string _audioBGMPath = "Audios/BGM";

    AudioClip[] _arraySEClips;
    private Dictionary<string, AudioSource> _SEdictionary = new Dictionary<string, AudioSource>();
    private string _audioSEPath = "Audios/SE";

    public enum E_BGMType
    {
        罪人の街,
    }
    public enum E_SEType
    {
        悲鳴,
        悲鳴_2,
        悲鳴_3,
    }

    public new void Awake()
    {   
        base.Awake();

        _arrayBGMClips = Resources.LoadAll<AudioClip>(_audioBGMPath);
        List<string> bgmNameList = new List<string>();
        foreach (var bgmClips in _arrayBGMClips)
        {
            AudioSource source = this.gameObject.AddComponent<AudioSource>();
            source.clip = bgmClips;
            source.playOnAwake = false;
            source.loop = true;

            _BGMdictionary.Add(bgmClips.name, source);
            bgmNameList.Add(bgmClips.name);
        }

        _arraySEClips = Resources.LoadAll<AudioClip>(_audioSEPath);
        List<string> seNameList = new List<string>();
        foreach (var seClips in _arraySEClips)
        {
            AudioSource source = this.gameObject.AddComponent<AudioSource>();
            source.clip = seClips;
            source.playOnAwake = false;

            _SEdictionary.Add(seClips.name, source);
            seNameList.Add(seClips.name);
        }
        
// #if UNITY_EDITOR
//         EnumCreater(bgmNameList, "E_BGMType");
//         EnumCreater(seNameList, "E_SEType");  
// #endif
    }

    public void PlayBGM(E_BGMType bgmType, float volume = 0.3f)
    {
        var bgmName = bgmType.ToString();

        _BGMdictionary[bgmName].volume = volume;
        _BGMdictionary[bgmName].Play();
    }

    public void PlaySE(E_SEType seType, float volume = 0.7f)
    {
        var seName = seType.ToString();

        _SEdictionary[seName].volume = volume;
        _SEdictionary[seName].Play();
    }

    public void StopPlayBGM(E_BGMType bgmType)
    {
        var bgmName = bgmType.ToString();
        _BGMdictionary[bgmName].Stop();
    }

    public void StopPlayAllBGM()
    {
        foreach (var v in _BGMdictionary)
        {
            v.Value.Stop();
        }
    }
    public void StopPlaySE(E_SEType seType)
    {
        string seName = seType.ToString();
        _SEdictionary[seName].Stop();
    }
    public void ChangePitchBGM(E_BGMType bgmType, float pitch = 1)
    {
        string bgmName = bgmType.ToString();
        _BGMdictionary[bgmName].pitch = pitch;
    }

    private static void EnumCreater(List<string> items, string enumName)
    {
        string enumCode = $"public enum {enumName}\n{{\n";
        enumCode += string.Join(",\n", items.Select(n => $"\t{n}"));
        enumCode += "\n}";

        File.WriteAllText(Application.dataPath + $"/scripts/Enum/{enumName}.cs", enumCode);
        Console.WriteLine(enumCode);
    }
    
    public bool CheckedSEIsPlaying(E_SEType seType)
    {   
        string seName = seType.ToString();

        return _SEdictionary[seName].isPlaying;
    }
}
