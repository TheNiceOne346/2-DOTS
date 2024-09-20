using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct BulletSystem : ISystem
{
    private void OnUpdate(ref SystemState state)
    {
        EntityManager entityManager = state.EntityManager;
        NativeArray<Entity> AllEntities = entityManager.GetAllEntities();

        foreach (Entity entity in AllEntities)
        {
            if (entityManager.HasComponent<BulletComponent>(entity)) 
            {
                LocalTransform bulletTransform = entityManager.GetComponentData<LocalTransform>(entity);
                BulletComponent bulletComponent = entityManager.GetComponentData<BulletComponent>(entity);
                bulletTransform.Position += bulletComponent.Speed * SystemAPI.Time.DeltaTime * bulletTransform.Right();
                entityManager.SetComponentData(entity, bulletTransform);

                NativeArray<Entity>EnemyEntity = entityManager.GetAllEntities(Allocator.Temp);
                foreach(Entity entityEnemy in EnemyEntity) 
                {
                    if (entityManager.HasComponent<AIComponent>(entityEnemy))
                    {
                        LocalTransform enemyTransform = entityManager.GetComponentData<LocalTransform>(entityEnemy);
                        AIComponent aIComponent = entityManager.GetComponentData<AIComponent>(entityEnemy);
                        float3 enemyPos = enemyTransform.Position;
                        if (math.distance(bulletTransform.Position, enemyPos ) < .5f)
                        {
                            entityManager.DestroyEntity(entity);  
                            aIComponent.health -= 25;
                            entityManager.SetComponentData(entityEnemy, aIComponent);
                            break;
                        }
                    }
                }
                EnemyEntity.Dispose();

            }
        }
        AllEntities.Dispose();
    }

}
