using System;
using UnityEngine;

public class Asteroid : IPoolable
{
    public AsteroidSize Size { get; private set; }
    public GameObject LinkedView { get; private set; }
    public PhysicsMovement Physics { get; private set; }

    public event Action <Asteroid> OnDestroyed;

    public Asteroid(float raduis, AsteroidSize size)
    {
        Physics = new PhysicsMovement {Radius =  raduis, DragCoefficient = 0};
        Size = size;
    }

    public void SetView(GameObject view)
    {
        LinkedView = view;
    }


    public void OnSpawn()
    {

    }

    public void OnDespawn()
    {

    }

    public void TakeHit()
    {
        OnDestroyed?.Invoke(this);
    }

    public void Spawn(Vector2 position, float speed)
    {
        Physics.Position = position;
        float randomAngle = UnityEngine.Random.Range(0, 360);
        Vector2 direction = new Vector2(Mathf.Cos(randomAngle *  Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad));
        Physics.Velocity = direction * speed;
        LinkedView.SetActive(true);
    }

}
