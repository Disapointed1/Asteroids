using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Ship : IShipInfo
{
    private readonly CancellationTokenSource _cts = new();

    private readonly float _invulnerabilityDuration;

    private bool _isDisposed;

    public Ship(float radius, float mass, float dragCoefficient, float maxSpeed, int maxHealth,
        float invulnerabilityDuration)
    {
        Health = maxHealth;
        _invulnerabilityDuration = invulnerabilityDuration;
        Physics = new PhysicsMovement
        {
            Radius = radius,
            Mass = mass,
            DragCoefficient = dragCoefficient,
            MaxSpeed = maxSpeed
        };
    }

    public PhysicsMovement Physics { get; }
    public int Health { get; private set; }
    public bool IsInvulnerable { get; private set; }
    public float Rotation { get; private set; }


    public Vector2 Position => Physics.Position;

    public event Action OnInvulnerabilityStarted;
    public event Action OnInvulnerabilityEnded;
    public event Action OnDied;
    public event Action OnHealthChanged;

    public void TakeDamage(int damage)
    {
        if (_isDisposed || IsInvulnerable)
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
        _isDisposed = true;
        _cts.Cancel();
        _cts.Dispose();
    }


    private async UniTask InvulnerabilityTimer(CancellationToken token)
    {
        IsInvulnerable = true;
        OnInvulnerabilityStarted?.Invoke();
        await UniTask.Delay(TimeSpan.FromSeconds(_invulnerabilityDuration), cancellationToken: token);
        IsInvulnerable = false;
        OnInvulnerabilityEnded?.Invoke();
    }
}