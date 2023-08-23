using System;
using UnityEngine;

public enum UnlockRequirementType{CompleteTier, GainTotalGold}

[Serializable][CreateAssetMenu(fileName = "UnlockRequirementData", menuName = "New UnlockRequirement")]
public class UnlockRequirementData : ScriptableObject
{
    [SerializeField] public UnlockRequirementType type;
    public int amount;
}

[Serializable]
public class UnlockRequirement
{
    public UnlockRequirementType type;
    public int value;
}