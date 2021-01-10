using System;
using System.Collections;
using PathologicalGames;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(ClipLengthDictionary))]
public class ChainBlade : MonoBehaviour, ITargetable
{
    [SerializeField] Transform targetPoint;
    [SerializeField] Transform resetPoint;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform kickPoint;
    [SerializeField] private Transform verticalSlashPoint;
    [SerializeField] private Transform swordSlashEffectPoint;
    [SerializeField] private float slashTimer = 1f;

    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private int maxHealth;
    [SerializeField] private Collider swordCollider;
    [SerializeField] private ParticleSystem swordTrail;
    [SerializeField] private Collider shieldCollider;
    [SerializeField] private GameObject verticalSlashProjectile;
    [SerializeField] private GameObject slashEffect;
    [SerializeField] private AggroChecker aggroChecker;

    [Header("MMFeedbackEvent")] [SerializeField]
    private UnityEvent OnSwordHitsGround;
    [SerializeField] private UnityEvent soundOnSwordSwung;

    [Header("UI")] [SerializeField] private GameObject chainbladeHPBarUI;
    [SerializeField] private TextMeshProUGUI demoCompleteField;

    public GameObject ChainbladeHpBarUi => chainbladeHPBarUI;

    public Transform TargetPoint => targetPoint;
    public event Action Die;
    public event Action<float, float> OnHealthChanged;
    
    
    public bool DoneAttacking { get; set; }
    public Ratchet ratchet { get; private set; }

    public Collider ShieldCollider => shieldCollider;

    public NavMeshAgent NavMeshAgent => _navMeshAgent;

    public AggroChecker AggroChecker => aggroChecker;

    public TextMeshProUGUI DemoCompleteField => demoCompleteField;

    public int MaxHealth => maxHealth;

    public float CurrentHealth
    {
        get => _currentHealth;
        set => _currentHealth = Mathf.Max(0f, value);
    }

    public Transform PlayerTransform => playerTransform;

    private Animator _animator;
    private ClipLengthDictionary _clipLengthDictionary;
    private NavMeshAgent _navMeshAgent;
    private Collider _collider;
    private float _currentHealth;
    private static readonly int Chase = Animator.StringToHash("Chase");
    private bool _initialized;
    public bool Aggroed { get; private set; }
    public Coroutine VerticalSlashSpawner { get; private set; }


    private void Awake()
    {
        _currentHealth = maxHealth;
        _collider = GetComponent<Collider>();
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _clipLengthDictionary = GetComponent<ClipLengthDictionary>();
        ratchet = playerTransform.GetComponent<Ratchet>();
    }


    public void GetShot(float damage, Vector3 positionOfDamager)
    {
        TakeDamage(damage);
    }

    private void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
    }


    public IEnumerator StartAttack(string clipName)
    {
        // _animator.applyRootMotion = true;
        _animator.SetTrigger(clipName);
        VerticalSlashSpawner = StartCoroutine(SpawnVerticalSlashOnDelay(clipName));
        yield return new WaitForSeconds(_clipLengthDictionary.GetClipLength(clipName));
        // _animator.applyRootMotion = false;
        DoneAttacking = true;
    }

    private IEnumerator SpawnVerticalSlashOnDelay(string clipName)
    {
        VerticalSlashSpawner = null;
        yield return new WaitForSeconds(_clipLengthDictionary.GetClipLength(clipName) - slashTimer);
        SpawnVerticalSlash();
        OnSwordHitsGround?.Invoke();
    }

    public IEnumerator StartKick(string clipName)
    {
        // _animator.applyRootMotion = true;
        _animator.SetTrigger(clipName);
        float timer = 0f;
        while (timer < _clipLengthDictionary.GetClipLength(clipName))
        {
            var directionToPlayer = Quaternion.LookRotation(playerTransform.position - transform.position, Vector3.up);
            timer += Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, directionToPlayer, 9f);
            yield return null;
        }

        // yield return new WaitForSeconds(_clipLengthDictionary.GetClipLength(clipName));
        // _animator.applyRootMotion = false;
        DoneAttacking = true;
    }

    //called by animation event in attack and kick
    public void FacePlayer()
    {
        transform.LookAt(new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z));
    }

    public void KickPlayer()
    {
        var hits = Physics.OverlapSphere(kickPoint.position, 2f, playerLayer);
        if (hits.Length < 1)
            return;
        var ratchet = hits[0].GetComponent<Ratchet>();
        if (ratchet)
        {
            ratchet.TakeDamage(10f);
            var rb = ratchet.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 6000f);
        }
    }

    public void EnableSword()
    {
        swordCollider.enabled = true;
        soundOnSwordSwung?.Invoke();
        swordTrail.Play();
        var slashEffectTransform = PoolManager.Pools["Effects"].Spawn(slashEffect.transform,
            swordSlashEffectPoint.position,
            Quaternion.LookRotation(swordSlashEffectPoint.forward, swordSlashEffectPoint.right));
        slashEffectTransform.localScale *= 3;
        PoolManager.Pools["Effects"].Despawn(slashEffectTransform, 2f);
    }

    public void DisableSword()
    {
        swordTrail.Stop();
        swordCollider.enabled = false;
    }

    public IEnumerator WaitThenDisableSword()
    {
        yield return new WaitForSeconds( 0.5f);
        DisableSword();
    }

    public void SpawnVerticalSlash()
    {
        var directionToPlayer = playerTransform.position - verticalSlashPoint.position;
        PoolManager.Pools["EnemyProj"].Spawn(verticalSlashProjectile.transform, verticalSlashPoint.position,
            Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z), Vector3.up));
    }

    public IEnumerator StartBlocking(string clipName)
    {
        _animator.SetBool(clipName, true);
        while (true)
        {
            var directionToPlayer = Quaternion.LookRotation(playerTransform.position - transform.position, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, directionToPlayer, 7f);
            yield return null;
        }
    }

    public void StopBlocking(string clipName)
    {
        _animator.SetBool(clipName, false);
    }

    public IEnumerator StartChasing(string clipName)
    {
        _animator.SetTrigger(clipName);
        while (true)
        {
            _navMeshAgent.SetDestination(playerTransform.position);
            yield return new WaitForSeconds(0.25f);
        }
    }

    public void StartDeath(string clipName)
    {
        _animator.SetTrigger(clipName);
        Die?.Invoke();
        _collider.enabled = false;
    }

    public void ResetBoss()
    {
        StartCoroutine(WaitThenResetBoss());
    }

    private IEnumerator WaitThenResetBoss()
    {
        yield return new WaitForSeconds(3.65f);
        // if (_initialized)
        // {
        _animator.ResetTrigger(Chase);
        _animator.SetTrigger("Idle");
        // }
        CurrentHealth = maxHealth;
        OnHealthChanged?.Invoke(_currentHealth, maxHealth);
        chainbladeHPBarUI.SetActive(false);
        transform.position = resetPoint.position;
        _initialized = true;
    }
}