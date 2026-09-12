using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class LaserWeapon
{
    private const int MaxLaserCharges = 3;
    private const float LaserRechargeTime = 3f;

    private readonly CancellationTokenSource _cts = new CancellationTokenSource();

    private float _lastChargeTime;

    public int CurrentLaserCharges { get; private set; } = MaxLaserCharges;
    public int MaxCharges => MaxLaserCharges;
    public float TimeUntilNextCharges => CurrentLaserCharges >= MaxLaserCharges
        ? 0f
        : Math.Max(0f, LaserRechargeTime - (Time.time - _lastChargeTime));

    public event Action OnChargesChanged;
    public event Action OnFired;

    public LaserWeapon()
    {
        ChargeLoop(_cts.Token).Forget();
    }

    public bool TryFire()
    {
        if (CurrentLaserCharges <= 0)
            return false;
        CurrentLaserCharges--;
        OnChargesChanged?.Invoke();
        OnFired?.Invoke();
        return true;
    }

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
    }

    private async UniTask ChargeLoop(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            if (CurrentLaserCharges < MaxLaserCharges)
            {
                _lastChargeTime = Time.time;
                await UniTask.Delay(TimeSpan.FromSeconds(LaserRechargeTime), cancellationToken: token);
                CurrentLaserCharges++;
                OnChargesChanged?.Invoke();
            }
            else
            {
                await UniTask.Yield(cancellationToken: token);
            }
        }
    }
}