using NpcSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class DisplayNpcDialog : MonoBehaviour, IReceiveEntity
{

    public Text text;

    private Entity m_NpcEntity;

    public void SetReceivedEntity(Entity entity)
    {
        m_NpcEntity = entity;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!World.DefaultGameObjectInjectionWorld.IsCreated ||
            !World.DefaultGameObjectInjectionWorld.EntityManager.Exists(m_NpcEntity))
        {
            return;
        }

        var npcData = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<NpcComponent>(m_NpcEntity);

        if (npcData.showDialog)
        {
            text.text = $"Hello player, I am NPC {npcData.id}";

        }

       
    }
}
