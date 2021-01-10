using System.Collections;
using UnityEngine;

public static class DAudio
{
    
    public static void PlayRandomizedClip(AudioClip[] clipArray, AudioSource source, float pitchMin, float pitchMax, float volumeMin = 1, float volumeMax = 1)
    {
        int randomIndex = Random.Range(0, clipArray.Length);
        var selectedClip = clipArray[randomIndex];

        var randomPitch = Random.Range(pitchMin, pitchMax);
        source.pitch = randomPitch;
        var randomVolume = Random.Range(volumeMin, volumeMax);
        source.volume = randomVolume;
        
        source.PlayOneShot(selectedClip);
    }

    public static void PlayClip(AudioClip clip, AudioSource source, float pitch = 1, float volume = 1)
    {
        source.pitch = pitch;
        source.volume = volume;
        
        if(!source.isPlaying) 
            source.PlayOneShot(clip);
    }
    
    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume, bool stopAfterFade)
    {
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        if(stopAfterFade)
            audioSource.Stop();
    }
    
    
}