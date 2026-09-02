using UnityEngine;

public class UfoView : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidBody;
    [SerializeField] private float _chaseSpeed = 3f;

    private Ufo _ufo;
    private IShipInfo _shipInfo;

    public void Initialize(Ufo ufo, IShipInfo shipInfo)
    {
        _ufo = ufo;
        _shipInfo = shipInfo;
    }

    private void FixedUpdate()
    {
        _ufo.Chase(_shipInfo.Position, _chaseSpeed, Time.fixedDeltaTime);
        _rigidBody.MovePosition(_ufo.Physics.Position);
    }

    public void SyncPosition()
    {
        transform.position = _ufo.Physics.Position;
    }


}
