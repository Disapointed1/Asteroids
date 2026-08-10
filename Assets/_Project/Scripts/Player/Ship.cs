using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship
{
    public PhysicsMovement Physics { get;  private set ;}
    public int Health { get; private set; } = 3;
    public bool IsInvulnerable { get; private set; } = false ;
    public float Rotation { get; private set; } = 0;


    public event Action OnHealthChanged;
    public event Action OnDied;

    public Ship(float radius, float mass, float dragCoefficient)
    {
        Physics = new PhysicsMovement
        {
            Radius = radius,
            Mass = mass,
            DragCoefficient = dragCoefficient,
        };
    }

    public void TakeDamage(int damage)
    {
        if (IsInvulnerable)
            return;
        Health -= damage;
        OnHealthChanged?.Invoke();
        if (Health <= 0)
            OnDied?.Invoke();

    }
    public void ApplyRotation(float rotation)
    {
        Rotation += rotation;
    }
}
