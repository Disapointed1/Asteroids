using System;
using UnityEngine;

public class Bullet : IPoolable
{
    public PhysicsMovement Physics { get; private set; }
    public float Rotation { get; private set; }

    public event Action OnFired;
    public event Action OnReturned;

    public Bullet(float radius)
    {
        Physics = new PhysicsMovement { Radius = radius, DragCoefficient = 0 };
    }

    public void OnDespawn()
    {
        OnReturned?.Invoke();
    }

    public void Fire(Vector2 position, Vector2 velocity, float rotation)
    {
        Physics.Position = position;
        Physics.Velocity = velocity;
        Rotation = rotation;
        OnFired?.Invoke();
    }
}