using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;


public struct OneShotAnimationComponentData : IComponentData
{ }

public class OneShotAnimationComponent : MonoBehaviour, IConvertGameObjectToEntity
{
    public Animator animator;

    public int count;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        conversionSystem.AddHybridComponent(animator);
        dstManager.AddComponent(entity, typeof(OneShotAnimationComponentData));
    }
}


/// <summary>
/// 
/// </summary>
public class OneShotAnimationSystem : SystemBase
{

    static bool flag;

    protected override void OnUpdate()
    {


        Entities
           .WithName("ShowBouncingAnimation")
           .WithoutBurst()
           .WithAll<OneShotAnimationComponentData>()
           .ForEach((Animator animator) =>
           {
              // var animator = EntityManager.GetComponentObject<Animator>(e);

               if (flag == false)
               {
                   flag = true;
                   animator.Play("Bounce");
                   Debug.LogError("liwen play animal ");

               }

               //    EntityManager.RemoveComponent<OneShotAnimationComponentData>(e);


           })
           .Run();
    }
}
