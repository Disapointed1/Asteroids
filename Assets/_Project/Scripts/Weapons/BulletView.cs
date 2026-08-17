using UnityEngine;

public class BulletView : MonoBehaviour
{
   private Bullet _bullet;

   public void Initialize(Bullet bullet)
   {
      _bullet = bullet;
   }

   private void FixedUpdate()
   {
      if(_bullet ==  null) return;
      _bullet.PhysicsMovement.UpdatePosition(Time.fixedDeltaTime);
      transform.position = _bullet.PhysicsMovement.Position;
   }
   public void SyncPosition()
   {
      transform.position = _bullet.PhysicsMovement.Position;
      transform.rotation = Quaternion.Euler(0,0, _bullet.Rotation);
   }
}
