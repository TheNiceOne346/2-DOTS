using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;


public partial struct AIChaseSystem : ISystem
{
    private EntityCommandBuffer ecb;

    public void OnUpdate(ref SystemState state)
    {
        var ecbsystem = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        ecb = ecbsystem.CreateCommandBuffer(state.WorldUnmanaged);
        
        Entity playerEntity = SystemAPI.GetSingletonEntity<PlayerComponent>();
        var entityManager = state.EntityManager;

        // Get the player's LocalTransform (position)
        LocalTransform playerTransform = entityManager.GetComponentData<LocalTransform>(playerEntity);

        // Loop through all AI entities with AITag and AIComponent
        foreach (var (aiTransform, aiComponent) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<AIComponent>>())
        {
            float3 aiPosition = aiTransform.ValueRO.Position;
            float3 playerPosition = playerTransform.Position;

            // Calculate distance between AI and Player
            float distance = math.distance(aiPosition, playerPosition);

            // Chase if within range
            if (distance < 40f)
            {
                float3 direction = math.normalize(playerPosition - aiPosition);
                aiTransform.ValueRW.Position += direction * aiComponent.ValueRO.speed * SystemAPI.Time.DeltaTime;

              
            }
          
            var query = entityManager.CreateEntityQuery(typeof(AIComponent), typeof(LocalTransform));
            var enemyEntity = query.ToEntityArray(Allocator.Temp);
            foreach (Entity enemy in enemyEntity)
            {
                if (entityManager.HasComponent<AIComponent>(enemy)&& entityManager.HasComponent<LocalTransform>(enemy)) 
                {
                    var aicomponent = entityManager.GetComponentData<AIComponent>(enemy);   
                    var enemytransform = entityManager.GetComponentData<LocalTransform>(enemy);
                    if (aicomponent.health <= 0)
                    {
                        ecb.DestroyEntity(enemy);
                        continue;
                    }
                }
            }

            // Check if AI is dead
            if (aiComponent.ValueRO.health <= 0)
            {
                Debug.Log("AI is dead");
               
            }
        }
    }
}





