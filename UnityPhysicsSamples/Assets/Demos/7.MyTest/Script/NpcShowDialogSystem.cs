using NpcSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics.Stateful;
using UnityEngine;

/// <summary>
/// when player collider with npc, send message to UI show dialog
/// </summary>
public partial class NpcShowDialogSystem : SystemBase
{
    private TriggerEventConversionSystem m_TriggerSystem;

    private EntityQueryMask m_NonTriggerMask;

    private EntityQuery m_Npc;

    protected override void OnCreate()
    {
        m_TriggerSystem = World.GetOrCreateSystem<TriggerEventConversionSystem>();

        m_NonTriggerMask = EntityManager.GetEntityQueryMask(
            GetEntityQuery(new EntityQueryDesc
            {
                None = new ComponentType[]
                {
                    typeof(StatefulCollisionEvent)
                }
            })
        );

        m_Npc = GetEntityQuery(ComponentType.ReadWrite<NpcComponent>());
        //We wait to update until we have our converted entities
        RequireForUpdate(m_Npc);

        Debug.LogError("liwen NpcShowDialogSystem OnCreate ");
    }
    protected override void OnUpdate()
    {

        // Need this extra variable here so that it can
        // be captured by Entities.ForEach loop below
        var nonTriggerMask = m_NonTriggerMask;

        //We grab all the player scores because we don't know who will need to be assigned points
        var npcEntities = m_Npc.ToEntityArray(Allocator.TempJob);
        var npcComponent = GetComponentDataFromEntity<NpcComponent>();

        //We need to dispose our entities
        Entities
        .WithDisposeOnCompletion(npcEntities)
        .WithName("ShowNpcDialogOnTriggerEnter")
        .ForEach((Entity e, ref DynamicBuffer<StatefulCollisionEvent> triggerEventBuffer) =>
        {

            for (int i = 0; i < triggerEventBuffer.Length; i++)
            {
                //Here we grab our bullet entity and the other entity it collided with
                var triggerEvent = triggerEventBuffer[i];
                var otherEntity = triggerEvent.GetOtherEntity(e);

                var isPlayTag = HasComponent<PlayerTag>(otherEntity);

                Debug.LogError("liwen other is player  " + isPlayTag);
                // exclude other triggers and processed events
                if (triggerEvent.CollidingState == EventCollidingState.Stay || !isPlayTag)
                {
                    continue;
                }

                //We want our code to run on the first intersection of Bullet and other entity
                else if (triggerEvent.CollidingState == EventCollidingState.Enter)
                {
                    for (int j = 0; j < npcEntities.Length; j++)
                    {
                        var currentPlayScoreComponent = npcComponent[npcEntities[j]];

                        //We create a new component with updated values
                        var newPlayerScore = new NpcComponent
                        {
                            id = currentPlayScoreComponent.id,
                            name = currentPlayScoreComponent.name,
                            showDialog = true
                        };

                        Debug.LogError("liwen set npc showDialog true");
                        npcComponent[npcEntities[j]] = newPlayerScore;
                    }
  
                }
                else
                {
                    continue;
                }
            }
        }).Schedule();


    }

 
}
