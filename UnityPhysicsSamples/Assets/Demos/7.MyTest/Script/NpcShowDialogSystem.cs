﻿using NpcSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics.Stateful;
using Unity.Physics.Systems;
using UnityEngine;


/// <summary>
/// show dialog
/// </summary>
public struct NpcShowDialogComponent : IComponentData
{
    public int conversationID;
}

/// <summary>
/// when player collider with npc, send event to UI to show dialog
/// </summary>
/// 
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(TriggerEventConversionSystem))]
public partial class NpcShowDialogSystem : SystemBase
{
    private EndFixedStepSimulationEntityCommandBufferSystem m_CommandBufferSystem;
    private TriggerEventConversionSystem m_TriggerSystem;
    private EntityQueryMask m_NonTriggerMask;

    protected override void OnCreate()
    {
        m_CommandBufferSystem = World.GetOrCreateSystem<EndFixedStepSimulationEntityCommandBufferSystem>();
        m_TriggerSystem = World.GetOrCreateSystem<TriggerEventConversionSystem>();
        m_NonTriggerMask = EntityManager.GetEntityQueryMask(
            GetEntityQuery(new EntityQueryDesc
            {
                None = new ComponentType[]
                {
                    typeof(StatefulTriggerEvent)
                }
            })
        );


        RequireForUpdate(GetEntityQuery(new EntityQueryDesc
        {
            All = new ComponentType[]
            {
                typeof(StatefulTriggerEvent)
            }
        }));

    }
    protected override void OnUpdate()
    {

        Dependency = JobHandle.CombineDependencies(m_TriggerSystem.OutDependency, Dependency);

        var commandBuffer = m_CommandBufferSystem.CreateCommandBuffer();

        // Need this extra variable here so that it can
        // be captured by Entities.ForEach loop below
        var nonTriggerMask = m_NonTriggerMask;

        //We need to dispose our entities
        Entities
        .WithName("ShowNpcDialogOnTriggerEnter")
        .WithBurst()
        .ForEach((Entity e, ref DynamicBuffer<StatefulTriggerEvent> triggerEventBuffer) =>
        {

            for (int i = 0; i < triggerEventBuffer.Length; i++)
            {
                var triggerEvent = triggerEventBuffer[i];
                var otherEntity = triggerEvent.GetOtherEntity(e);

              

                // exclude other triggers and processed events
                if (triggerEvent.State == EventOverlapState.Stay)
                {
                    continue;
                }

                if (triggerEvent.State == EventOverlapState.Enter)
                {

                    if (HasComponent<NpcComponent>(triggerEvent.EntityB))
                    {
                        var showDialogComp = new NpcShowDialogComponent() ;
                        var npcComponentData = GetComponent<NpcComponent>(triggerEvent.EntityB);

                        showDialogComp.conversationID = npcComponentData.conversationID;

                        commandBuffer.AddComponent<NpcShowDialogComponent>(triggerEvent.EntityB, showDialogComp);

                  
                        npcComponentData.animalState = State.BOUNCE;
                        commandBuffer.AddComponent<NpcComponent>(triggerEvent.EntityB, npcComponentData);
                    }


                }
                else
                {
                    // State == PhysicsEventState.Exit
                    if (HasComponent<NpcComponent>(triggerEvent.EntityB))
                    {
                        commandBuffer.RemoveComponent<NpcShowDialogComponent>(triggerEvent.EntityB);

                        var npcComponentData = GetComponent<NpcComponent>(triggerEvent.EntityB);
                        npcComponentData.animalState = State.IDLE;
                        commandBuffer.AddComponent<NpcComponent>(triggerEvent.EntityB, npcComponentData);
                    }
                }
            }

            
        }).Schedule();

        m_CommandBufferSystem.AddJobHandleForProducer(Dependency);
    }

 
}
