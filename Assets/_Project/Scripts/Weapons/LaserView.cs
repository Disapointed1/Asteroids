using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class LaserView : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private float _laserLength = 20f;
    [SerializeField] private float _visibleDuration = 0.1f;

    private IShipInfo _ship;

    public void Initialize(IShipInfo ship)
    {
        _ship = ship;
        _ship.OnLaserFired += HandleLaserFired;
        _lineRenderer.enabled = false;
    }

    private void HandleLaserFired()
    {
        ShowLaser().Forget();
    }

    private async UniTaskVoid ShowLaser()
    {
        Vector2 direction = new Vector2(-Mathf.Sin(_ship.Rotation * Mathf.Deg2Rad),
            Mathf.Cos(_ship.Rotation * Mathf.Deg2Rad));
            Vector2 start = _ship.Position;
            Vector2 end = start + direction * _laserLength;

            _lineRenderer.SetPosition(0, start);
            _lineRenderer.SetPosition(1, end);
            _lineRenderer.enabled = true;

            await UniTask.Delay(TimeSpan.FromSeconds(_visibleDuration));

            _lineRenderer.enabled = false;

    }

}
