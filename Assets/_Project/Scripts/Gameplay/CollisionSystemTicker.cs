
using UnityEngine;

public class CollisionSystemTicker : MonoBehaviour
{
   private CollisionSystem _collisionSystem;

   public void Initialize(CollisionSystem collisionSystem)
   {
      _collisionSystem = collisionSystem;
   }

   private void FixedUpdate()
   {
      if(_collisionSystem == null) return;
      _collisionSystem.CheckCollisions();
   }

}
