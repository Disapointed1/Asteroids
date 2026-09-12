using UnityEngine;

public class BulletView : MonoBehaviour
{
   private Bullet _bullet;

   public void Initialize(Bullet bullet)
   {
      _bullet = bullet;
      _bullet.OnFired += HandleFired;
      _bullet.OnReturned += HandleReturned;
   }

   private void FixedUpdate()
   {
      if(_bullet ==  null) return;
      transform.position = _bullet.Physics.Position;
   }
   public void SyncPosition()
   {
      transform.position = _bullet.Physics.Position;
      transform.rotation = Quaternion.Euler(0,0, _bullet.Rotation);
   }

   private void HandleFired()
   {
      gameObject.SetActive(true);
      SyncPosition();
   }

   private void HandleReturned()
   {
      gameObject.SetActive(false);
   }
}
