using Unity.Entities;

public struct AITag : IComponentData { }
public struct AIComponent : IComponentData 
{
    public float health;
    public float speed;
}

