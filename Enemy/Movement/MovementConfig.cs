using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class MovementConfig
{
    public MovementTypeId movementType;

    [Title("Base Movement Stats")]
    public float speed;
    
    [ShowIf("@movementType == MovementTypeId.FollowPlayerRadius || movementType == MovementTypeId.FollowPlayer")]
    [Title("Follow specific variables")]
    [Tooltip("Is the distance between agent destination and player position than this value, no new path is getting calculated")]
    public float pathUpdateThreshold = 0.5f;

    [Title("Follow Player Radius specific variables")]
    [ShowIf("@movementType == MovementTypeId.FollowPlayerRadius")]
    public float followRadius;

    [ShowIf("@movementType == MovementTypeId.FollowPlayerRadius")]
    [Tooltip("How long the enemy should stop moving when it reached the radius")]
    [Range(0.1f, 5f)]
    public float radiusReachedTimer = 1f;

    [ShowIf("@movementType == MovementTypeId.FollowPlayerRadius")]
    public float rotationSpeed = 10f;

    [Title("Keep Distance specific variables")]
    [ShowIf("@movementType == MovementTypeId.KeepDistance")]
    [Tooltip("The Elder approaches when farther away than this distance.")]
    public float maximumDistance = 10f;

    [ShowIf("@movementType == MovementTypeId.KeepDistance")]
    [Tooltip("The Elder retreats when the player gets closer than this distance.")]
    public float minimumDistance = 6f;

    [ShowIf("@movementType == MovementTypeId.KeepDistance")]
    [Tooltip("How quickly the Elder rotates toward the player.")]
    public float keepDistanceRotationSpeed = 5f;

    [ShowIf("@movementType == MovementTypeId.KeepDistance")]
    [Tooltip("How far the Elder attempts to retreat during each reposition.")]
    public float retreatDistance = 3f;
}