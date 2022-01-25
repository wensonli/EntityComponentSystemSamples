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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (!World.DefaultGameObjectInjectionWorld.IsCreated)
        {
            return;
        }


        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var entites = entityManager.GetAllEntities() ;

        for (int i = 0; i < entites.Length; i++)
        {
            var hasDialog = entityManager.HasComponent<NpcShowDialogComponent>(entites[i]);

            if (hasDialog)
            {
                var npcData = entityManager.GetComponentData<NpcComponent>(entites[i]);

                text.text = $"{npcData.dialogContent}";
                showDialogNpc = entites[i];

                StartCoroutine(disapearDialog(showDialogNpc, npcData.showDialogDuration));
                break;
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
