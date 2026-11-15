using System.Collections;
using UnityEngine;

public abstract class AttackType
{
    protected Enemy enemy;
    protected AttackConfig attackConfig;
    protected Animator animator;

    protected enum AttackState
    {
        Idle,
        Charging,
        Attacking,
        Recharging
    }

    protected AttackState attackState = AttackState.Idle;

    protected float cooldownTimer;
    protected float rechargeTimer;

    public virtual void Initialize(
        Enemy entity,
        AttackConfig config,
        Animator animatorComponent)
    {
        enemy = entity;
        attackConfig = config;
        animator = animatorComponent;
    }

    public abstract void Tick();
    public abstract bool CanAttack();
    public abstract void Attack();
    public abstract void Recharge();
    public abstract void OnRechargeFinished();
}
