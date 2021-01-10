using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AggroChecker aggroChecker;
    [SerializeField] private Ratchet ratchet;
    [SerializeField] private AudioSource[] musicSelection;

    private bool _isInCombat;
    private void Awake()
    {
        aggroChecker.OnAggroChanged += CheckForChange;
        ratchet.OnRevive += PlayNormalMusic;
    }

    private void CheckForChange(bool isInCombat)
    {
        if (isInCombat == _isInCombat)
            return;
        _isInCombat = isInCombat;
        ChangeMusic(isInCombat);
    }

    private void ChangeMusic(bool isInCombat)
    {
        if (isInCombat)
        {
            musicSelection[0].Stop();
            musicSelection[1].Play();
        }
        else
        {
            musicSelection[1].Stop();
        }
    }

    private void PlayNormalMusic()
    {
        musicSelection[0].Play();
    }
}
