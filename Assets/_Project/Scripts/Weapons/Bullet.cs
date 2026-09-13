using System;
using UnityEngine;

public class Bullet : IPoolable
{
    public Bullet(float radius)
    {
        Physics = new PhysicsMovement { Radius = radius, DragCoefficient = 0 };
    }

    public PhysicsMovement Physics { get; }
    public float Rotation { get; private set; }

    public void OnDespawn()
    {
        OnReturned?.Invoke();
    }

    public event Action OnFired;
    public event Action OnReturned;

    public void Fire(Vector2 position, Vector2 velocity, float rotation)
    {
        Physics.Position = position;
        Physics.Velocity = velocity;
        Rotation = rotation;
        OnFired?.Invoke();
    }
}