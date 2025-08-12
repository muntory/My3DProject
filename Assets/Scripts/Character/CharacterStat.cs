using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public event Action<float> onHealthChange;


    protected virtual void Awake()
    {
        InitializeStat();
    }


    public virtual void InitializeStat()
    {
        MaxHealth = 100f;
        Health = MaxHealth;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Health -= 10f;
        }
    }

}
