using UnityEngine;

public class JumpAttack : AttackType
{
    public override void Tick()
    {
        switch (attackState)
        {
            case AttackState.Idle:
                TickIdle();
                break;
            case AttackState.Recharging:
                TickRecharging();
                break;
        }
    }

    private void TickIdle()
    {
        cooldownTimer += Time.deltaTime;
        if (cooldownTimer >= attackConfig.cooldown)
        {
            if (CanAttack())
            {
                enemy.SetMovementEnabled(false);
                attackState = AttackState.Attacking;
                Attack();
            }
        }
    }
    
    public override bool CanAttack()
    {
        Vector3 enemyPosition = enemy.transform.position;
        float distance = (enemyPosition - enemy.PlayerTransform.position).magnitude;

        return distance <= attackConfig.range;
    }

    public override void Attack()
    {
        animator.SetTrigger("Attack");
        float distance = Vector3.Distance(enemy.transform.position, enemy.PlayerTransform.position);
        
        if (distance <= attackConfig.damageRange)
            enemy.PlayerHealthComponent.TakeDamage(enemy.Damage);
        
        Recharge();
        attackState = AttackState.Recharging;
    }
    
    private void TickRecharging()
    {
        rechargeTimer += Time.deltaTime;
            
        if (rechargeTimer >= attackConfig.rechargeTime)
            OnRechargeFinished();
    }
    
    public override void Recharge()
    {
        animator.SetTrigger("Idle");
        enemy.SetMovementEnabled(false);
    }

    public override void OnRechargeFinished()
    {
        cooldownTimer = 0f;
        rechargeTimer = 0f;
        enemy.SetMovementEnabled(true);
        attackState = AttackState.Idle;
    }
}
