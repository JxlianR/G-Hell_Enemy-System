using UnityEngine;
using UnityEngine.AI;

public class SpiderJumpAttack : AttackType
{
    private float chargingTimer;
    
    private Vector3 jumpTargetPosition;
    private Vector3 jumpStartPosition;
    
    private float elapsedTime;

    public override void Tick()
    {
        switch (attackState)
        {
            case AttackState.Idle:
                TickIdle();
                break;
            case AttackState.Charging:
                TickCharging();
                break;
            case AttackState.Attacking:
                TickAttack();
                break;
            case AttackState.Recharging:
                TickRecharging();
                break;
        }
    }

    public override bool CanAttack() => enemy.AttackEnabled;

    private void TickIdle()
    {
        cooldownTimer += Time.deltaTime;
        
        if (!(cooldownTimer >= attackConfig.cooldown))
            return;

        if (!CanAttack())
            return;
        
        enemy.SetMovementEnabled(false);
        attackState = AttackState.Charging;
    }

    private void TickCharging()
    {
        chargingTimer += Time.deltaTime;

        if (chargingTimer >= attackConfig.chargeTime)
        {
            Attack();
            attackState = AttackState.Attacking;
        }
    }
    
    private void TickAttack()
    {
        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / attackConfig.jumpDuration);

        Vector3 position = Vector3.Lerp(jumpStartPosition, jumpTargetPosition, t);
        position.y += Mathf.Sin(t * Mathf.PI) * attackConfig.jumpHeight;

        enemy.transform.position = position;
        
        if (t < 1f)
            return;
        
        TryDealDamage();
        
        attackState = AttackState.Recharging;
        Recharge();
    }

    private void TickRecharging()
    {
        rechargeTimer += Time.deltaTime;
            
        if (rechargeTimer >= attackConfig.rechargeTime)
            OnRechargeFinished();
    }
    
    public override void Attack()
    {
        animator.SetTrigger("Attack");
        
        jumpStartPosition = enemy.transform.position;
        jumpTargetPosition = enemy.PlayerTransform.position;
        
        NavMeshHit hit;

        if (NavMesh.SamplePosition(
            jumpTargetPosition,
            out hit,
            2f,
            NavMesh.AllAreas))
        {
            jumpTargetPosition = hit.position;
        }
        
        elapsedTime = 0f;
    }

    private void TryDealDamage()
    {
        float distance = Vector3.Distance(enemy.transform.position, enemy.PlayerTransform.position);
        if (distance <= attackConfig.damageRange)
            enemy.PlayerHealthComponent.TakeDamage(attackConfig.damage);
    }

    public override void Recharge()
    {
        animator.SetTrigger("Idle");
    }

    public override void OnRechargeFinished()
    {
        cooldownTimer = 0f;
        chargingTimer = 0f;
        rechargeTimer = 0f;
        elapsedTime = 0f;
        
        enemy.SetMovementEnabled(true);
        attackState = AttackState.Idle;
    }
}
