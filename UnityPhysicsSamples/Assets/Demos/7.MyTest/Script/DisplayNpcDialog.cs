using NpcSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class DisplayNpcDialog : MonoBehaviour
{

    public Text text1;

    public Text text2;

    public Button _btnContent1;

    public Button _btnContent2;

    private Entity showDialogNpc;

    private NodeGraphSpawner spawner;

    private EntityManager entityManager;

    private  int curDialogNodeIndex;

    // Start is called before the first frame update
    void Start()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        _btnContent1.onClick.AddListener(OnButtonClick);
        _btnContent2.onClick.AddListener(OnButtonClick);
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

                    text1.text = npcData.Graph.Value.Nodes[0].Content.ToString();
                    showDialogNpc = entites[i];

                    curDialogNodeIndex =  0;


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

         
        }

    }

    private void OnButtonClick()
    {
        var system = World.DefaultGameObjectInjectionWorld.GetExistingSystem<SimulationSystemGroup>();
        if (system != null)
        {
            Debug.LogError($"liwen onClick {curDialogNodeIndex}");


            var nodeGraphSpawner = system.GetSingleton<NodeGraphSpawner>();

            ref var dialogTree = ref nodeGraphSpawner.Graph.Value;


            ref var curDialogNode = ref  nodeGraphSpawner.Graph.Value.Nodes[curDialogNodeIndex];

            text1.text = "";
            text2.text = "";

            if (curDialogNode.Links.Length != 0)
            {
                if (curDialogNode.Links.Length == 2)
                {
                    Debug.LogError($"liwen 2 node {curDialogNode.Links[1]}");
                    text2.text = dialogTree.Nodes[curDialogNode.Links[1]].Content.ToString();
                }
                Debug.LogError($"liwen 1 node {curDialogNode.Links[0]}");
                text1.text = dialogTree.Nodes[curDialogNode.Links[0]].Content.ToString();

                curDialogNodeIndex = curDialogNode.Links[0];
            }
            else
            {
                text1.text = "";
                text2.text = "";

                curDialogNodeIndex = 0;

            }

        }

    }
}
