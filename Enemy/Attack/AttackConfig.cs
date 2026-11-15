using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class AttackConfig
{
    public AttackTypeId attackType;

    [Title("Charge Variables")]
    [ShowIf(
        "@attackType == AttackTypeId.SpiderJump || " +
        "attackType == AttackTypeId.TinySpiderJump")]
    public float chargeTime;

    [Title("Spider Jump Specific Variables")]
    [ShowIf(
        "@attackType == AttackTypeId.SpiderJump || " +
        "attackType == AttackTypeId.TinySpiderJump")]
    public float jumpHeight;

    [ShowIf(
        "@attackType == AttackTypeId.SpiderJump || " +
        "attackType == AttackTypeId.TinySpiderJump")]
    public float jumpDuration;
}