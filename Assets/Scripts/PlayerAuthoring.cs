using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class PlayerAuthoring : MonoBehaviour
{
    public float MoveSpeed = 5;
    public float ShootCooldown = 1;
    public GameObject Bulletprefab;

    public class PlayerBaker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            Entity playerEntity = GetEntity(TransformUsageFlags.None);

            AddComponent(playerEntity, new PlayerComponent
            {
                MoveSpeed = authoring.MoveSpeed,
                ShootCooldown = authoring.ShootCooldown,
                BulletPrefab = GetEntity(authoring.Bulletprefab, TransformUsageFlags.None)
            });

        }
    }
}
