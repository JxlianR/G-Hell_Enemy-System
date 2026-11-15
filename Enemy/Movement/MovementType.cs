using UnityEngine;
using UnityEngine.AI;

public abstract class MovementType : MonoBehaviour
{
    protected Enemy enemy;
    protected MovementConfig movementConfig;
    protected NavMeshAgent agent;
    protected Animator animator;
    
    public virtual void Initialize(Enemy entity, MovementConfig config, Animator animatorComponent)
    {
        enemy = entity;
        movementConfig = config;
        agent = entity.Agent;
        animator = animatorComponent;
    }
    
    public abstract void Move();

    public void Stop()
    {
        if (agent == null || animator == null) return;
        
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        animator.SetTrigger("Idle");
    }

    public void Continue()
    {
        if (agent == null) return;
        agent.isStopped = false;
    }

}
