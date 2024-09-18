using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class SpawnerAuthoring : MonoBehaviour
{
    public GameObject prefab;
    public float spawnRate;

    class SpawnerBaker : Baker<SpawnerAuthoring>
    {
        public override void Bake(SpawnerAuthoring authoring)
        {
            Entity newEntity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(newEntity, new Spawner
            {
                prefab = GetEntity(authoring.prefab, TransformUsageFlags.Dynamic),
                spawnPosition = float2.zero,
                spawnRate = authoring.spawnRate
            });

        }
    }
}
