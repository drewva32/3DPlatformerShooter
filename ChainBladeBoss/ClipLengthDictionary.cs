using System;
using System.Collections.Generic;
using UnityEngine;

public class ClipLengthDictionary : MonoBehaviour
{
    private Animator _animator;
    private Dictionary<string, float> _clipLengths = new Dictionary<string, float>();

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        foreach (var animationClip in _animator.runtimeAnimatorController.animationClips)
        {
            _clipLengths.Add(animationClip.name,animationClip.length);
        }
    }

    public float GetClipLength(string clipName)
    {
        return _clipLengths[clipName];
    }
}