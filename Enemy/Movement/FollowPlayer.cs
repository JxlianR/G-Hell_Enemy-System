using UnityEngine;
using UnityEngine.AI;

public class FollowPlayer : MovementType
{
    public override void Move()
    {
        if (enemy.PlayerTransform == null) return;

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
            animator.SetTrigger("Walking");
        }
    }
}
