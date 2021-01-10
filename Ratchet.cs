using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using PlayerWeaponSystems;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Ratchet : MonoBehaviour
{
    [SerializeField] private int maxHealth = 30;
    [SerializeField] private float invulnTimeOnHit = 1.5f;
    
    [SerializeField] private Image healthBar;
    [Header("Audio")]
    [SerializeField] private  AudioClip fallToDeathSound;
    [SerializeField] private  AudioClip[] getHitSounds;
    [Header("Respawn Point")]
    [SerializeField] private  CheckPointManager checkPointManager;
    [SerializeField] private UnityEvent onRatchetHit;
    public event Action<float, float> OnHealthChanged;
    public event Action OnDie;
    public event Action OnRevive;
    public bool Dead => _dead;
    
    private Animator _animator;
    private AudioSource _audioSource;
    private TaterCharacterController _taterCharacterController;
    private Inventory _inventory;
    private float _currentHealth;
    private bool _invincible;
    private bool _dead;
    private static readonly int Die1 = Animator.StringToHash("Die");
    private static readonly int Revive = Animator.StringToHash("Revive");

    private void Awake()
    {
        _currentHealth = maxHealth;
        _animator = GetComponentInChildren<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _taterCharacterController = GetComponent<TaterCharacterController>();
        _inventory = GetComponent<Inventory>();
    }

    public float CurrentHealth
    {
        get => _currentHealth;
        set => _currentHealth = Mathf.Max(0, value);
    }

    public void TakeDamage(float damage)
    {
        if (_invincible || _dead)
            return;
        _invincible = true;
        onRatchetHit?.Invoke();
        StartCoroutine(WaitThenBecomeVulnerableAgain());
        
        DAudio.PlayRandomizedClip(getHitSounds,_audioSource,.6f,.7f, .8f,1f);
        CurrentHealth -= damage;
        OnHealthChanged?.Invoke(CurrentHealth,maxHealth);
        
        if (CurrentHealth <= 0)
        {
            Die();
            _animator.SetTrigger(Die1);
            StartCoroutine(WaitThenRespawn(3.5f));
        }
    }

    private IEnumerator WaitThenBecomeVulnerableAgain()
    {
        yield return new WaitForSeconds(invulnTimeOnHit);
        _invincible = false;
    }

    public void FallToDeath()
    {
        _animator.SetTrigger(Die1);
        _currentHealth = 0;
        OnHealthChanged.Invoke(_currentHealth,maxHealth);
        _dead = true;
        OnDie?.Invoke();
        DAudio.PlayClip(fallToDeathSound,_audioSource,0.75f);
        StartCoroutine(WaitThenRespawn(2.5f));
    }

    private IEnumerator WaitThenRespawn(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        RespawnPlayer();
        transform.position = checkPointManager.GetLastCheckPointThatWasPassed().transform.position;
        _dead = false;
        CurrentHealth = maxHealth;
        OnHealthChanged?.Invoke(CurrentHealth,maxHealth);
    }

    private void RespawnPlayer()
    {
        _animator.SetTrigger(Revive);
        _taterCharacterController.speed = 8;
        _inventory.MaxAmmoAllWeapons();
        OnRevive?.Invoke();
    }

    private void Die()
    {
        OnDie?.Invoke();
        _dead = true;
        _taterCharacterController.speed = 0;
    }
}