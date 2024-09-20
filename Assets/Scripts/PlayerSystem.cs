using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Transforms;
using UnityEngine.Scripting.APIUpdating;
using System;

public partial struct PlayerSystem : ISystem
{

    private Entity playerEntity;
    private Entity inputEnity;
    private EntityManager entityManager;
    private PlayerComponent playerComponent;
    private InputComponent inputComponent;
    public void OnUpdate(ref SystemState state)
    {
        entityManager = state.EntityManager;
        playerEntity = SystemAPI.GetSingletonEntity<PlayerComponent>();
        inputEnity = SystemAPI.GetSingletonEntity<InputComponent>();

        playerComponent = entityManager.GetComponentData<PlayerComponent>(playerEntity);
        inputComponent = entityManager.GetComponentData<InputComponent>(inputEnity);    

        Move(ref state);
        Shoot(ref state);
        
    }

  

    // Update is called once per frame
    private void Move(ref SystemState state)
    {
        LocalTransform playerTransform = entityManager.GetComponentData<LocalTransform>(playerEntity);
        playerTransform.Position += new float3(inputComponent.movement * playerComponent.MoveSpeed * SystemAPI.Time.DeltaTime, 0);
        Vector2 dir= (Vector2)inputComponent.mousePos - (Vector2)Camera.main.WorldToScreenPoint(playerTransform.Position);
        float angle = math.degrees(math.atan2(dir.y,dir.x));
        playerTransform.Rotation = Quaternion.AngleAxis(angle, Vector3.forward);


        entityManager.SetComponentData(playerEntity, playerTransform);

  
    }

    private float nextShootTime;
   
    private void Shoot(ref SystemState state)
    {
        if (inputComponent.pressingLMB && nextShootTime < SystemAPI.Time.ElapsedTime)
        {
            EntityCommandBuffer ECB = new EntityCommandBuffer(Allocator.Temp);
            Entity bulletEntity = entityManager.Instantiate(playerComponent.BulletPrefab);

            ECB.AddComponent(bulletEntity, new BulletComponent { Speed = 10 });

            LocalTransform bulletTransform = entityManager.GetComponentData<LocalTransform>(bulletEntity);
            bulletTransform.Rotation = entityManager.GetComponentData<LocalTransform>(playerEntity).Rotation;

            LocalTransform playerTransform = entityManager.GetComponentData<LocalTransform>(playerEntity);
            bulletTransform.Position = playerTransform.Position + playerTransform.Right() + playerTransform.Up() * -0.3f;
            ECB.SetComponent(bulletEntity, bulletTransform);

            ECB.Playback(entityManager);

            nextShootTime = (float)SystemAPI.Time.ElapsedTime + playerComponent.ShootCooldown;
        }
    }



}
