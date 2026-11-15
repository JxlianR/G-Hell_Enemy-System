using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [SerializeField] public EnemyDefinition enemy;
    private AttackType attackType;
    private MovementType movementType;
    
    private HealthComponent healthComponent;
    
    public NavMeshAgent Agent { get; private set; }

    private Animator animator;

    private XPSystem xpSystem;
    private CoinSystem coinSystem;

    public int Damage { get; private set; }

    public bool AttackEnabled { get; private set; } = true;
    public bool MovementEnabled { get; private set; } = true;
    public bool IsDead { get; private set; }
    
    public Transform PlayerTransform { get; private set; }
    public HealthComponent PlayerHealthComponent { get; private set; }

    private EnemyPool pool;
    private bool initialized;
    
    // Test stuff for stop moving when Inventory is open
    private bool blocked;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        healthComponent = GetComponent<HealthComponent>();

        animator = GetComponentInChildren<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        xpSystem = ServiceLocator.Current.Get<XPSystem>();
        coinSystem = ServiceLocator.Current.Get<CoinSystem>();
        
        healthComponent.onDead.AddListener(OnDead);
        GameManagerTest.Instance.onToggleInventory.AddListener(SetBlocked);
        
        //Change to a different function later for the Pooling -> Should be Enable/Disable but GameManagerTest.Instance is not ready then
        GameManagerTest.Instance.OnGameplayFreeze += OnGameplayFreeze;
        GameManagerTest.Instance.OnGameplayResume += OnGameplayResume;
    }

    private void OnDestroy()
    {
        if (GameManagerTest.Instance == null) return;
        
        GameManagerTest.Instance.OnGameplayFreeze -= OnGameplayFreeze;
        GameManagerTest.Instance.OnGameplayResume -= OnGameplayResume;
    }

    public void Initialize(Transform player, int damage, int health)
    {
        Spawn(damage, health);
     
        if (initialized) return;
        
        InitializeAgent();
        
        PlayerTransform = player;
        PlayerHealthComponent = player.GetComponent<HealthComponent>();
        SetGoldAndXpValue(enemy.xpDrop, enemy.goldDrop);

        attackType = EnumFactory<AttackTypeId, AttackType>.Create(enemy.attack.attackType);
        movementType = EnumFactory<MovementTypeId, MovementType>.Create(enemy.movement.movementType);
        attackType.Initialize(this, enemy.attack, animator);
        movementType.Initialize(this, enemy.movement, animator);

        initialized = true;
    }
    
    private void InitializeAgent()
    {
        Agent.speed = enemy.movement.speed;
        if (!Agent.isOnNavMesh)
        {
            Debug.Log("Agent not on NavMesh");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (blocked || !initialized)
            return;

        attackType?.Tick();

        if (MovementEnabled)
            movementType?.Move();
    }

    private void Spawn(int damage, int health)
    {
        healthComponent.Initialize(health);
        Damage = damage;
    }

    private void OnGameplayFreeze()
    {
        blocked = true;
        SetMovementEnabled(false);

        if (animator == null) return;
        animator.SetTrigger("Idle");
    }

    private void OnGameplayResume()
    {
        SetMovementEnabled(true);
        blocked = false;
    }

    private void OnDead()
    {
        xpSystem.GainXP(enemy.xpDrop);
        SpawnCoins();
        
        IsDead = true;

        if (pool != null)
            pool.Release(this);
        else Destroy(gameObject);
    }

    public void Reset()
    {
        IsDead = false;
        SetMovementEnabled(true);
        SetAttackEnabled(true);
    }

    private void SpawnCoins()
    {
        for (int i = 0; i < enemy.goldDrop; i++)
        {
            Vector3 offset = Random.insideUnitSphere * 0.5f;
            Vector3 spawnPosition = transform.position + offset;

            if (NavMesh.SamplePosition(spawnPosition, out NavMeshHit hit, 3f, NavMesh.AllAreas))
            {
                spawnPosition = hit.position;
                spawnPosition.y += 0.3f;
            }
            
            coinSystem.SpawnCoin(spawnPosition, 1);
        }
    }

    public void SetPool(EnemyPool enemyPool) => pool = enemyPool;

    public void SetAttackEnabled(bool enabled) => AttackEnabled = enabled;

    public void SetMovementEnabled(bool enabled)
    {
        if (IsDead)
            return;
        
        if (enabled)
            movementType.Continue();
        else movementType.Stop();
    }

    private void SetBlocked(bool newState) => blocked = newState;

    public EnemyDefinition GetDefinition() => enemy;
}
