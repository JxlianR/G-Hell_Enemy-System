using UnityEngine;
using UnityEngine.Events;
using FMODUnity;

public class HealthComponent : MonoBehaviour
{
    public int MaxHealth { get; private set; } = 40;
    public int Health { get; private set; } = 40;

    [HideInInspector]
    public UnityEvent<int> onDamageTaken;
    [HideInInspector] 
    public UnityEvent receivedHit;
    [HideInInspector]
    public UnityEvent<int> onHealed;
    [HideInInspector]
    public UnityEvent onDead;

    [Header("Audio")]
    [SerializeField]
    private EventReference enemyHitSfx;

    public void Initialize(int healthStat)
    {
        MaxHealth = healthStat;
        Health = MaxHealth;
    }
    
    public void TakeDamage(int damage)
    {
        if (Health <= 0)
        {
            return;
        }

        Debug.Log("Damage dealt: " + damage);

        Health -= damage;
        Health = Mathf.Clamp(Health, 0, MaxHealth);

        onDamageTaken?.Invoke(damage);
        receivedHit?.Invoke();

        Debug.Log(gameObject + " Health: " + Health);

        if (Health <= 0)
        {
            Health = 0;
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (Health == MaxHealth) return;

        Health = Mathf.Min(Health + amount, MaxHealth);
        onHealed?.Invoke(amount);
    }

    private void Die()
    {
        Debug.Log("Someone Died bruh");
        onDead?.Invoke();
    }
}