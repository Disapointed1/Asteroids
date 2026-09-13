using System;
using UnityEngine;

public class Ufo : IPoolable, IEnemy
{
    public Ufo(float radius, float mass)
    {
        Physics = new PhysicsMovement { Radius = radius, Mass = mass, DragCoefficient = 0 };
    }

    public PhysicsMovement Physics { get; }

    public EnemyType Type => EnemyType.Ufo;

    public void TakeHit()
    {
        OnDestroyed?.Invoke(this);
    }

    public void OnDespawn()
    {
        OnReturnedEvent?.Invoke();
    }

    public event Action<Ufo> OnDestroyed;
    public event Action OnSpawnedEvent;
    public event Action OnReturnedEvent;

    public void SetPosition(Vector2 position)
    {
        Physics.Position = position;
        OnSpawnedEvent?.Invoke();
    }

    public void Chase(Vector2 targetPosition, float speed, float deltaTime)
    {
        var direction = (targetPosition - Physics.Position).normalized;
        Physics.ApplyAcceleration(direction * speed, deltaTime);
    }
}