using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class UfoSpawner
{
   private readonly IShipInfo _shipInfo;
   private readonly ObjectPool<Ufo> _ufoPool;
   private readonly WorldBoundary _boundary;
   private readonly float _ufoSpeed;
   private readonly UfoFactory _ufoFactory;
   private readonly EnemyCounterTracker _counterTracker;

   public ObjectPool<Ufo> UfoPool => _ufoPool;

   public UfoSpawner(UfoFactory ufoFactory, WorldBoundary boundary, float ufoSpeed, IShipInfo shipInfo, EnemyCounterTracker  counterTracker)
   {
      _ufoPool = new ObjectPool<Ufo>((() => ufoFactory.CreateUfo(_shipInfo)));
      _boundary = boundary;
      _ufoSpeed = ufoSpeed;
      _ufoFactory = ufoFactory;
      _shipInfo = shipInfo;
      _counterTracker = counterTracker;
   }

   public void StartSpawning()
   {
      SpawnLoop().Forget();
   }
   private async UniTaskVoid SpawnLoop()
   {
      while (true)
      {
         float spawnRate = Random.Range(1f, 5f);
         await UniTask.Delay(TimeSpan.FromSeconds(spawnRate));

         if(!_counterTracker.CanSpawn())
            continue;

         Ufo ufo =  _ufoPool.Get();
         ufo.OnDestroyed += HandleUfoDestroyed;
         Vector2 position = GetRandomSpawnPosition();
         ufo.SetPosition(position);
         _counterTracker.RegisterSpawned();
      }
   }

   private Vector2 GetRandomSpawnPosition()
   {
      int side = Random.Range(0, 4);
      float x, y;

      switch (side)
      {
         case 0:
            x  = Random.Range(-_boundary.HalfWidth, _boundary.HalfWidth);
            y = _boundary.HalfHeight + 1f;
            break;
         case 1:
            x = Random.Range(-_boundary.HalfWidth, _boundary.HalfWidth);
            y = -_boundary.HalfHeight - 1f;
            break;
         case 2:
            x = -_boundary.HalfWidth - 1f;
            y = Random.Range(-_boundary.HalfHeight, _boundary.HalfHeight);
            break;
         default:
            x = _boundary.HalfWidth + 1f;
            y = Random.Range(-_boundary.HalfHeight, _boundary.HalfHeight);
            break;
      }
      return new Vector2(x, y);
   }

   private void HandleUfoDestroyed(Ufo ufo)
   {
      ufo.OnDestroyed -= HandleUfoDestroyed;
      _ufoPool.Return(ufo);
      _counterTracker.RegisterDestroyed();
   }

}
