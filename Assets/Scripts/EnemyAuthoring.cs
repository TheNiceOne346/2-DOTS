using Unity.Entities;
using UnityEngine;

public class EnemyAuthoring : MonoBehaviour
{
    public float health;
    public float speed;

  
    public class EnemyBaker : Baker<EnemyAuthoring>
    {
        public override void Bake(EnemyAuthoring authoring)
        {
            // Get the entity associated with this GameObject
            Entity enemyEntity = GetEntity(TransformUsageFlags.Dynamic);

            
            AddComponent(enemyEntity, new AIComponent
            {
                health = authoring.health,
                speed = authoring.speed
            });

            // Add a tag to recognize this entity as an AI
            AddComponent<AITag>(enemyEntity);
        }
    }
}

