using Unity.Entities;
using Unity.Mathematics;

public struct Spawner : IComponentData
{
    public Entity prefab;
    public float2 spawnPosition;
    public float2 destroyPos;
    public float nextSpawnTime;
    public float spawnRate;
    

}
