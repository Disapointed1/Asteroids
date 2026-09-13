using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Asteroid : IPoolable, IEnemy
{
    private const float FullCircleDegrees = 360f;

    public Asteroid(float radius, AsteroidSize size)
    {
        Physics = new PhysicsMovement { Radius = radius, DragCoefficient = 0 };
        Size = size;
    }

    public AsteroidSize Size { get; }
    public bool IsFragment { get; private set; }
    public PhysicsMovement Physics { get; }

    public EnemyType Type => Size switch
    {
        AsteroidSize.Large => EnemyType.AsteroidLarge,
        AsteroidSize.Medium => EnemyType.AsteroidMedium,
        AsteroidSize.Small => EnemyType.AsteroidSmall,
        _ => EnemyType.AsteroidLarge
    };

    public void TakeHit()
    {
        OnDestroyed?.Invoke(this);
    }

    public void OnDespawn()
    {
        OnReturnedEvent?.Invoke();
    }

    public event Action<Asteroid> OnDestroyed;
    public event Action OnSpawnedEvent;
    public event Action OnReturnedEvent;

    public void MarkAsFragment()
    {
        IsFragment = true;
    }

    public void Spawn(Vector2 position, float speed)
    {
        Physics.Position = position;
        var randomAngle = Random.Range(0, FullCircleDegrees);
        var direction = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad));
        Physics.Velocity = direction * speed;
        OnSpawnedEvent?.Invoke();
    }
}