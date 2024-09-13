using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] _audioClips;
    [SerializeField] private AudioSource _audioSource;


    public void PlayClip(ClipName clipName, float volume = 1)
    {
        Debug.Log($"Audio played: {clipName.ToString()}");
        
        _audioSource.PlayOneShot(_audioClips[(int)clipName],volume);
    }

}

public enum ClipName
{
    UnitHit,
    UnitDie, 
    Explosion,
}