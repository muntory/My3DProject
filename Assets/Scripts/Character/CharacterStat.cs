using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    Health,
    WalkSpeed,

}

[System.Serializable]
public class StatEffect
{
    public StatType statType;
    public float value;
    public bool isPercent;
    public bool hasDuration;
    public float duration;
}

public class CharacterStat : MonoBehaviour
{
    private float health;
    public float Health
    { 
        get { return health; }
        set 
        { 
            health = Mathf.Clamp(value, 0f, MaxHealth);
            onHealthChange?.Invoke(health);
        }
    }
    public float MaxHealth;

    public float MaxWalkSpeed;
    private float walkSpeed;
    public float WalkSpeed
    {
        get { return walkSpeed; }
        set
        {
            walkSpeed = Mathf.Clamp(value, 0f, MaxWalkSpeed);
        }
    }

    public event Action<float> onHealthChange;


    protected virtual void Awake()
    {
        // InitializeStat();
    }

    private void Start()
    {
        InitializeStat();

    }


    public virtual void InitializeStat()
    {
        Health = MaxHealth / 2f;
        WalkSpeed = MaxWalkSpeed / 2f;
    }


    public float? GetStat(StatType type)
    {
        if (type == StatType.Health)
        {
            return Health;
        }
        if (type == StatType.WalkSpeed)
        {
            return WalkSpeed;
        }

        return null;
    }

    public void SetStat(StatType type, float value)
    {
        if (type == StatType.Health)
        {
            Health = value;
        }
        if (type == StatType.WalkSpeed)
        {
            walkSpeed = value;
        }
    }

    

}
