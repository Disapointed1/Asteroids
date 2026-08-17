using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Ship : IShipInfo
{
    public PhysicsMovement Physics { get;  private set ;}
    public int Health { get; private set; } = 3;
    public bool IsInvulnerable { get; private set; } = false ;
    public float Rotation { get; private set; } = 0;
    public int MaxLaserCharges { get; private set; } = 3;
    public int CurrentLaserCharges { get; private set; } = 3;
    public float LaserRechargeTime { get; private set; } = 3f;

    public Vector2 Position => Physics.Position;

    public event Action OnInvulnerabilityStarted;
    public event Action OnInvulnerabilityEnded;
    public event Action OnLaserChargesChanged;
    public event Action OnHealthChanged;
    public event Action OnDied;
    public event Action OnLaserFired;

    private async UniTaskVoid InvulnerabilityTimer()
    {
        IsInvulnerable = true;
        OnInvulnerabilityStarted?.Invoke();
        await UniTask.Delay(TimeSpan.FromSeconds(3));
        IsInvulnerable = false;
        OnInvulnerabilityEnded?.Invoke();
    }

    public Ship(float radius, float mass, float dragCoefficient)
    {
        Physics = new PhysicsMovement
        {
            Radius = radius,
            Mass = mass,
            DragCoefficient = dragCoefficient,
        };
        LaserChargeLoop().Forget();
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
            InvulnerabilityTimer().Forget();

    }
    public void ApplyRotation(float rotation)
    {
        Rotation += rotation;
    }

    public bool TryUseLaserCharge()
    {
        if(CurrentLaserCharges <= 0)
            return false;
        CurrentLaserCharges--;
        OnLaserChargesChanged?.Invoke();
        OnLaserFired?.Invoke();
        return true;
    }

    private async UniTaskVoid LaserChargeLoop()
    {
        while (true)
        {
            if (CurrentLaserCharges < MaxLaserCharges)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(LaserRechargeTime));
                CurrentLaserCharges++;
                OnLaserChargesChanged?.Invoke();
            }
            else
            {
                await UniTask.Yield();
            }
        }


    }

}
