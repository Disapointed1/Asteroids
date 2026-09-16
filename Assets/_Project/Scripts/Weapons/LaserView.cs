    using System;
    using System.Threading;
    using Cysharp.Threading.Tasks;
    using UnityEngine;

    public class LaserView : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private float _laserLength = 20f;
        [SerializeField] private float _visibleDuration = 0.1f;

        private readonly CancellationTokenSource _cts =  new CancellationTokenSource();

        private IShipInfo _ship;
        private LaserWeapon _laserWeapon;

        public void Initialize(IShipInfo ship, LaserWeapon laserWeapon)
        {
            _ship = ship;
            _laserWeapon = laserWeapon;
            laserWeapon.OnFired += HandleLaserFired;
            _lineRenderer.enabled = false;
        }

        private void HandleLaserFired()
        {
            ShowLaser(_cts.Token).Forget();
        }

        private async UniTask ShowLaser(CancellationToken cancellationToken)
        {
            var direction = DirectionMath.FromAngle(_ship.Rotation);
            var start = _ship.Position;
            var end = start + direction * _laserLength;

            _lineRenderer.SetPosition(0, start);
            _lineRenderer.SetPosition(1, end);
            _lineRenderer.enabled = true;

            await UniTask.Delay(TimeSpan.FromSeconds(_visibleDuration) , cancellationToken: cancellationToken);

            _lineRenderer.enabled = false;
        }

        private void OnDestroy()
        {
            _laserWeapon.OnFired -= HandleLaserFired;
            _cts.Cancel();
        }
    }