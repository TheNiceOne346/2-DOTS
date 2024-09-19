using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class EnemyAuthoring : MonoBehaviour
{
    public float health;
    public float speed;

    public class enemybake: Baker<EnemyAuthoring> 
    {
        public override void Bake(EnemyAuthoring authoring)
        {
            Entity enemyEntity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(enemyEntity, new AIComponent
            {
                health = authoring.health,
                speed = authoring.speed
            });
        }

    }
}
