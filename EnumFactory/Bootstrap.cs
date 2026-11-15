using Systems.Items;
using Systems.Items.Behaviors;
using UnityEngine;

using AttackTypeFactory = EnumFactory<AttackTypeId, AttackType>;
using MovementTypeFactory = EnumFactory<MovementTypeId, MovementType>;


public static class Bootstrap
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        RegisterAttackTypes();
        RegisterMovementTypes();
    }

    private static void RegisterAttackTypes()
    {
        AttackTypeFactory.Register(
            AttackTypeId.Jump,
            typeof(JumpAttack));

        AttackTypeFactory.Register(
            AttackTypeId.SpiderJump,
            typeof(SpiderJumpAttack));

        AttackTypeFactory.Register(
            AttackTypeId.TinySpiderJump,
            typeof(TinySpiderJumpAttack));
    }

    private static void RegisterMovementTypes()
    {
        MovementTypeFactory.Register(
            MovementTypeId.FollowPlayer,
            typeof(FollowPlayer));

        MovementTypeFactory.Register(
            MovementTypeId.FollowPlayerRadius,
            typeof(FollowPlayerRadius));
    }
}
