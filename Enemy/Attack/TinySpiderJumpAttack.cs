using UnityEngine;

public class TinySpiderJumpAttack : SpiderJumpAttack
{
    public override bool CanAttack()
    {
        Vector3 enemyPosition = enemy.transform.position;
        float distance = (enemyPosition - enemy.PlayerTransform.position).magnitude;

        return distance <= attackConfig.range;
    }
}
