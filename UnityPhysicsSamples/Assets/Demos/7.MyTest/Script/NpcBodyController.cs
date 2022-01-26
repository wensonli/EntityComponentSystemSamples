using NpcSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class NpcBodyController : MonoBehaviour, IReceiveEntity
{

    public Animator animatior;

    private Entity m_DisplayEntity;

    public void SetReceivedEntity(Entity entity)
    {
        this.m_DisplayEntity = entity;
    }

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        if (!World.DefaultGameObjectInjectionWorld.IsCreated ||
            !World.DefaultGameObjectInjectionWorld.EntityManager.Exists(m_DisplayEntity))
        {
            return;
        }


        var npcComponent = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<NpcComponent>(m_DisplayEntity);

        if (npcComponent.animalState == State.BOUNCE)
        {
            animatior.Play("Bounce");
        }
        else
        {
            animatior.Play("Idle");
        }

    }
}
