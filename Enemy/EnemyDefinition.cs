using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/EnemyDefinition")]
public class EnemyDefinition : ScriptableObject
{
    [Tooltip("Prefab that is used for the enemy")]
    public GameObject prefab;

    [Title("Base Stats")]
    public int health;

    [Title("Attack")]
    public AttackConfig attack;

    [Title("Movement")]
    public MovementConfig movement;
    
    [Title("Amount of Rewards")]
    public int xpDrop;
    public int goldDrop;

    [Title("Rarity")]
    public Rarity rarity;

    public enum Rarity
    {
        Common,
        Rare,
        Legendary
    };
}
