using System;
using UnityEngine;

public class Asteroid : IPoolable, IEnemy
{
    private const float FullCircleDegrees = 360f;
    public AsteroidSize Size { get; private set; }
    public PhysicsMovement Physics { get; private set; }
    public bool IsFragment { get; private set; }

    public event Action<Asteroid> OnDestroyed;
    public event Action OnSpawnedEvent;
    public event Action OnReturnedEvent;

    public EnemyType Type => Size switch
    {
        AsteroidSize.Large => EnemyType.AsteroidLarge,
        AsteroidSize.Medium => EnemyType.AsteroidMedium,
        AsteroidSize.Small => EnemyType.AsteroidSmall,
        _ => EnemyType.AsteroidLarge
    };

    public Asteroid(float raduis, AsteroidSize size)
    {
        Physics = new PhysicsMovement { Radius = raduis, DragCoefficient = 0 };
        Size = size;
    }

    public void OnDespawn()
    {
        OnReturnedEvent?.Invoke();
    }

    public void TakeHit()
    {
        OnDestroyed?.Invoke(this);
    }

    public void MarkAsFragment()
    {
        IsFragment = true;
    }

    public void Spawn(Vector2 position, float speed)
    {
        Physics.Position = position;
        float randomAngle = UnityEngine.Random.Range(0, FullCircleDegrees);
        Vector2 direction = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad));
        Physics.Velocity = direction * speed;
        OnSpawnedEvent?.Invoke();
    }
}