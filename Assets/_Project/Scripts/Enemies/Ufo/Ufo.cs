using System;
using UnityEngine;

public class Ufo : IPoolable, IRewardable
{
  public PhysicsMovement Physics{get; private set;}
  public GameObject LinkedView {get; private set;}

  public event Action<Ufo> OnDestroyed;
  public EnemyType Type => EnemyType.Ufo;

  public Ufo(float radius, float mass)
  {
    Physics =  new PhysicsMovement { Radius = radius, Mass = mass, DragCoefficient = 0};
  }

  public void SetView(GameObject view)
  {
    LinkedView = view;
  }

  public void OnSpawn()
  {
    LinkedView.SetActive(true);
  }

  public void OnDespawn()
  {
    LinkedView.SetActive(false);
  }

  public void TakeHit()
  {
    OnDestroyed?.Invoke(this);
  }

  public void SetPosition(Vector2 position)
  {
    Physics.Position = position;
    LinkedView.GetComponent<UfoView>().SyncPosition();
  }

  public void Chase(Vector2 targetPosition, float speed, float deltaTime)
  {
    Vector2 direction = (targetPosition - Physics.Position).normalized;
    Physics.ApplyAcceleration(direction * speed, deltaTime);
    Physics.UpdatePosition(deltaTime);
  }

}
