using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Ship : IShipInfo
{
    private CancellationTokenSource _cts = new CancellationTokenSource();

    private const float InvulnerabilityDuration = 3f;

    public PhysicsMovement Physics { get; private set; }
    public int Health { get; private set; }
    public bool IsInvulnerable { get; private set; } = false;
    public float Rotation { get; private set; } = 0;


    public Vector2 Position => Physics.Position;

    public event Action OnInvulnerabilityStarted;
    public event Action OnInvulnerabilityEnded;
    public event Action OnDied;
    public event Action OnHealthChanged;

    public Ship(float radius, float mass, float dragCoefficient, float maxSpeed, int maxHealth)
    {
        Health = maxHealth;
        Physics = new PhysicsMovement
        {
            Radius = radius,
            Mass = mass,
            DragCoefficient = dragCoefficient,
            MaxSpeed = maxSpeed
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
        else
            InvulnerabilityTimer(_cts.Token).Forget();
    }

    public void ApplyRotation(float rotation)
    {
        Rotation += rotation;
    }


    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
    }


    private async UniTask InvulnerabilityTimer(CancellationToken token)
    {
        IsInvulnerable = true;
        OnInvulnerabilityStarted?.Invoke();
        await UniTask.Delay(TimeSpan.FromSeconds(InvulnerabilityDuration), cancellationToken: token);
        IsInvulnerable = false;
        OnInvulnerabilityEnded?.Invoke();
    }
}