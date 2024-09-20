using System.Linq;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

partial struct SpawnerSystem : ISystem
{
    private const int MaxEnemies = 10;


    public void OnUpdate(ref SystemState state)
    {
        Camera mainCamera = Camera.main;

        foreach (RefRW<Spawner> spawner in SystemAPI.Query<RefRW<Spawner>>())
        {
            if (spawner.ValueRO.nextSpawnTime < SystemAPI.Time.ElapsedTime)
            {
                // Check the current number of active enemies

                // int activeEnemiesCount = SystemAPI.Query<RefRW<HealthComponent>>().Count();

                //   if (activeEnemiesCount >= MaxEnemies) return; // Do not spawn if max is reached


                Entity newEntity = state.EntityManager.Instantiate(spawner.ValueRO.prefab);

                // Random spawn logic
                float randomX = Random.Range(0, Screen.width);
                float randomY = Random.Range(0, Screen.height);
                Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(randomX, randomY, mainCamera.nearClipPlane));
                float3 pos = new float3(worldPos.x, worldPos.y, 0f);

                state.EntityManager.SetComponentData(newEntity, new LocalTransform
                {
                    Position = pos,
                    Scale = 1f
                });






                spawner.ValueRW.nextSpawnTime = (float)SystemAPI.Time.ElapsedTime + spawner.ValueRO.spawnRate;
                // Add components to the new enemy entity
               // state.EntityManager.AddComponentData(newEntity, new AITag());
               // state.EntityManager.AddComponentData(newEntity, new HealthComponent { CurrentHealth = 40 });
            }
        }
    }

}



