using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffData", menuName = "Scriptable Objects/Buff Data")]
public class Buffs : ScriptableObject
{
    public enum BuffsType { DAMAGE, MOVEMENT, STEAL, GOLD, FISHING, REVIVE }

    public BuffsType buffType;
    public int maxCounter;
    public int counter;

}
