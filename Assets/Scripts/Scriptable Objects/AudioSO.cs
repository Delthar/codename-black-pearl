using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioSO", menuName = "AudioSO", order = 0)]
public class AudioSO : ScriptableObject 
{
    public List<AudioClip> audioClips;

    public AudioClip GetRandomAudioClip() => audioClips[Random.Range(0, audioClips.Count)];
}