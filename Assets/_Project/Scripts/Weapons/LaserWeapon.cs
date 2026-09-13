using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class LaserWeapon
{
    private readonly CancellationTokenSource _cts = new();
    private readonly float _laserRechargeTime;

    private bool _isDisposed;

    private float _lastChargeTime;

    public LaserWeapon(int maxLaserCharges, float laserRechargeTime)
    {
        MaxCharges = maxLaserCharges;
        _laserRechargeTime = laserRechargeTime;
        CurrentLaserCharges = maxLaserCharges;
        ChargeLoop(_cts.Token).Forget();
    }

    public int CurrentLaserCharges { get; private set; }
    public int MaxCharges { get; }

    public float TimeUntilNextCharges => CurrentLaserCharges >= MaxCharges
        ? 0f
        : Math.Max(0f, _laserRechargeTime - (Time.time - _lastChargeTime));

    public event Action OnChargesChanged;
    public event Action OnFired;

    public bool TryFire()
    {
        if (_isDisposed || CurrentLaserCharges <= 0)
            return false;
        CurrentLaserCharges--;
        OnChargesChanged?.Invoke();
        OnFired?.Invoke();
        return true;
    }

    public void Dispose()
    {
        _isDisposed = true;
        _cts.Cancel();
        _cts.Dispose();
    }

    private async UniTask ChargeLoop(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
            if (CurrentLaserCharges < MaxCharges)
            {
                _lastChargeTime = Time.time;
                await UniTask.Delay(TimeSpan.FromSeconds(_laserRechargeTime), cancellationToken: token);
                CurrentLaserCharges++;
                OnChargesChanged?.Invoke();
            }
            else
            {
                await UniTask.Yield(token);
            }
    }
}