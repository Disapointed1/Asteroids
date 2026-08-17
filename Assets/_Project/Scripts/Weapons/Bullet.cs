using UnityEngine;

public class Bullet : IPoolable
{
    public GameObject LinkedView { get; private set; }
    public PhysicsMovement PhysicsMovement {get; private set;}
    public float Rotation { get; private set; }

    public Bullet (float radius)
    {
        PhysicsMovement = new PhysicsMovement{Radius = radius, DragCoefficient = 0};
    }
    public void OnSpawn()
    {
    }

    public void OnDespawn()
    {
        LinkedView.SetActive(false);
    }

    public void Fire(Vector2 position, Vector2 velocity, float rotation)
    {
       PhysicsMovement.Position = position;
       PhysicsMovement.Velocity = velocity;
       Rotation = rotation;
       LinkedView.SetActive(true);
       LinkedView.GetComponent<BulletView>().SyncPosition();
    }

    public void SetView(GameObject view)
    {
        LinkedView = view;
    }

}
