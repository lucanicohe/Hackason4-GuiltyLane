using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{   
    public enum BGMType
    {
       
    }
    public enum SEType
    {
        
    }

    private AudioClip[] _arrayBGMClips;
    private Dictionary<string, AudioSource> _BGMdictionary = new Dictionary<string, AudioSource>();
    private string audioBGMPath = "Audios/BGM";

    AudioClip[] _arraySEClips;
    private Dictionary<string, AudioSource> _SEdictionary = new Dictionary<string, AudioSource>();
    private string audioSEPath = "Audios/SE";

    void Start()
    {
        _arrayBGMClips = Resources.LoadAll<AudioClip>(audioBGMPath);
        foreach (var BGMClips in _arrayBGMClips)
        {
            AudioSource source = this.gameObject.AddComponent<AudioSource>();
            source.clip = BGMClips;
            source.playOnAwake = false;
            source.loop = true;

            _BGMdictionary.Add(BGMClips.name, source);
        }

        _arraySEClips = Resources.LoadAll<AudioClip>(audioSEPath);
        foreach (var SEClips in _arraySEClips)
        {
            AudioSource source = this.gameObject.AddComponent<AudioSource>();
            source.clip = SEClips;
            source.playOnAwake = false;

            _SEdictionary.Add(SEClips.name, source);
        }
        
    }

    public void PlayBGM(BGMType BGMName,float volume = 0.3f)
    {
        string Name = BGMName.ToString();

        _BGMdictionary[Name].volume = volume;
        _BGMdictionary[Name].Play();
    }

    public void PlaySE(SEType SEName,float volume = 0.7f)
    {     
        string Name = SEName.ToString();

        _SEdictionary[Name].volume = volume;
        _SEdictionary[Name].Play();
    }

    public void StopPlayBGM(BGMType BGMName)
    {   
        string Name = BGMName.ToString();
        _BGMdictionary[Name].Stop();
    }

    public void StopPlaySE(SEType SEName)
    {   
        string Name = SEName.ToString();
        _SEdictionary[Name].Stop();
    }
}
