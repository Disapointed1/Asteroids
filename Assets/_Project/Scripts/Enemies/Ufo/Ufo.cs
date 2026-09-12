using System;
using UnityEngine;

public class Ufo : IPoolable, IEnemy
{
    public PhysicsMovement Physics { get; private set; }

    public event Action<Ufo> OnDestroyed;
    public event Action OnSpawnedEvent;
    public event Action OnReturnedEvent;

    public EnemyType Type => EnemyType.Ufo;

    public Ufo(float radius, float mass)
    {
        Physics = new PhysicsMovement { Radius = radius, Mass = mass, DragCoefficient = 0 };
    }

    public void OnDespawn()
    {
        OnReturnedEvent?.Invoke();
    }

    public void TakeHit()
    {
        OnDestroyed?.Invoke(this);
    }

    public void SetPosition(Vector2 position)
    {
        Physics.Position = position;
        OnSpawnedEvent?.Invoke();
    }

    public void Chase(Vector2 targetPosition, float speed, float deltaTime)
    {
        Vector2 direction = (targetPosition - Physics.Position).normalized;
        Physics.ApplyAcceleration(direction * speed, deltaTime);
    }
}