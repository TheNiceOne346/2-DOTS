using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using static UnityEngine.EventSystems.EventTrigger;

public partial struct BulletSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        EntityManager entityManager = state.EntityManager;
        NativeArray<Entity> allEntities = entityManager.GetAllEntities();

        foreach (Entity entity in allEntities)
        {
            if (entityManager.HasComponent<BulletComponent>(entity) && entityManager.HasComponent<SpawnTimeComponent>(entity))
            {
                // Move the bullet
                LocalTransform bulletTransform = entityManager.GetComponentData<LocalTransform>(entity);
                BulletComponent bulletComponent = entityManager.GetComponentData<BulletComponent>(entity);
                bulletTransform.Position += bulletComponent.Speed * SystemAPI.Time.DeltaTime * bulletTransform.Right();
                entityManager.SetComponentData(entity, bulletTransform);

                // Check for collision with enemies
                CheckCollisionWithEnemies(entity, ref state);

                // Remove bullet after 10 seconds
                float lifetime = 10f; // Bullet lifetime in seconds
                float spawnTime = entityManager.GetComponentData<SpawnTimeComponent>(entity).Time;
                if (SystemAPI.Time.ElapsedTime - spawnTime >= lifetime)
                {
                    entityManager.DestroyEntity(entity);
                }
            }
        }
    }

    private void CheckCollisionWithEnemies(Entity bulletEntity, ref SystemState state)
    {
        var bulletTransform = state.EntityManager.GetComponentData<LocalTransform>(bulletEntity);
        foreach (var (health, enemyTransform) in SystemAPI.Query<RefRW<HealthComponent>, RefRW<LocalTransform>>())
        {
            float distance = math.distance(bulletTransform.Position, enemyTransform.ValueRO.Position);
            if (distance < 1f) // Assume a collision threshold
            {
                // Deal damage
                health.ValueRW.CurrentHealth -= 20;
                if (health.ValueRW.CurrentHealth <= 0)
                {
                    // Destroy the enemy entity if health is 0
                    state.EntityManager.DestroyEntity(entity);
                }
                // Destroy the bullet after dealing damage
                state.EntityManager.DestroyEntity(bulletEntity);
                break; // Exit the loop after damage is dealt
            }
        }
    }
}

