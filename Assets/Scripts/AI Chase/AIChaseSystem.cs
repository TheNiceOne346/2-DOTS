using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;

public partial struct AIChaseSystem : ISystem
{
    public float speed;

    private AIComponent aIComponent;

   public void OnUpdate(ref SystemState state)
    {

        // Get the player entity
        Entity playerEntity = SystemAPI.GetSingletonEntity<PlayerComponent>();
        var entityManager = state.EntityManager;

        // Get the player's transform
        LocalTransform playerTransform = entityManager.GetComponentData<LocalTransform>(playerEntity);

        // Query all AI entities with AITag
        foreach (var (aiTransform, aiTag) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<AITag>>())
        {
           

            float distance = math.distance(aiTransform.ValueRO.Position, playerTransform.Position);
            if (distance < 40f) // Chase distance
            {
                // Move AI towards the player
                float3 direction = math.normalize(playerTransform.Position - aiTransform.ValueRO.Position);
                aiTransform.ValueRW.Position += direction * aIComponent.speed * SystemAPI.Time.DeltaTime; // Speed
                Debug.Log("direction" + direction);
                aiTransform.ValueRW.Rotation = quaternion.LookRotationSafe(direction, math.up());
            }
            if(aIComponent.health <= 0)
            {
                Debug.Log("dead");
            }
                

        }
        
    }

}




