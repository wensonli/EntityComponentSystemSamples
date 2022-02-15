using NpcSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class DisplayNpcDialog : MonoBehaviour
{

    public Text text;

    private Entity showDialogNpc;

    private NodeGraphSpawner spawner;

    private EntityManager entityManager;

    // Start is called before the first frame update
    void Start()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

    }

    // Update is called once per frame
    void Update()
    {

        if (!World.DefaultGameObjectInjectionWorld.IsCreated)
        {
            return;
        }


        var entites = entityManager.GetAllEntities() ;

        for (int i = 0; i < entites.Length; i++)
        {
            var hasDialog = entityManager.HasComponent<NpcShowDialogComponent>(entites[i]);

            if (hasDialog)
            {
                var system = World.DefaultGameObjectInjectionWorld.GetExistingSystem<SimulationSystemGroup>();
                if (system != null)
                {
                    var npcData = system.GetSingleton<NodeGraphSpawner>();

                    text.text = npcData.Graph.Value.Nodes[0].Content.ToString();
                    showDialogNpc = entites[i];

                    StartCoroutine(disapearDialog(showDialogNpc, 5f));

                    break;
                }
        
        
            }
        }

        entites.Dispose();


        IEnumerator disapearDialog(Entity entity, float duration)
        {
            yield return new WaitForSeconds(duration);

            World.DefaultGameObjectInjectionWorld.EntityManager.RemoveComponent<NpcShowDialogComponent>(entity);

            text.text = "";
        }

    }
}
