using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class FollowPlayerRadius : MovementType
{
    private float followRadius;
    private float radiusReachedTimer;
    private float rotationSpeed;
    
    private float timer;
    
    public override void Initialize(Enemy entity, MovementConfig config, Animator animatorComponent)
    {
        base.Initialize(entity, config, animatorComponent);
        followRadius = entity.GetDefinition().movement.followRadius;
        radiusReachedTimer = entity.GetDefinition().movement.radiusReachedTimer;
        rotationSpeed = enemy.GetDefinition().movement.rotationSpeed;
        
        enemy.Agent.SetDestination(enemy.PlayerTransform.position);
        agent.updateRotation = false;
    }
    
    public override void Move()
    {
        if (enemy.PlayerTransform == null) return;
        
        LookAtPlayer();
        
        if (agent.remainingDistance < followRadius)
        {
            ReachedRadius();
            return;
        }
        
        Vector3 targetPosition = enemy.PlayerTransform.position;
        
        if (!NavMesh.SamplePosition(
            targetPosition,
            out NavMeshHit hit,
            3f,
            NavMesh.AllAreas))
        {
            Debug.LogWarning($"{enemy.name}: Player has no NavMesh nearby");
            return;
        }

        if (Vector3.Distance(agent.destination, hit.position) < movementConfig.pathUpdateThreshold)
            return;
        
        NavMeshPath path = new NavMeshPath();

        bool result = NavMesh.CalculatePath(
        agent.transform.position,
        hit.position,
        NavMesh.AllAreas,
        path
        );

        if (result && path.status == NavMeshPathStatus.PathComplete)
        {
            agent.SetPath(path);
        }
        
        animator.SetTrigger("Walking");
    }

    private void LookAtPlayer()
    {
        Vector3 direction = enemy.PlayerTransform.position - enemy.transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return;
        
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void ReachedRadius()
    {
        Stop();
        enemy.SetAttackEnabled(true);
        
        timer += Time.deltaTime;
        if (timer >= radiusReachedTimer)
            RestartMovement();
    }

    private void RestartMovement()
    {
        Continue();
        timer = 0f;
        agent.SetDestination(enemy.PlayerTransform.position);
        enemy.SetAttackEnabled(false);
    }
}
