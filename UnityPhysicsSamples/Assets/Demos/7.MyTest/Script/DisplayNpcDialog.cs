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

    private EntityManager entityManager;

    private  int curDialogNodeIndex;

    /// <summary>
    /// 后续改成自动导入数据
    /// </summary>
    public List<ConversationDataBase> conversationDB;

    private ConversationDataBase curConversationData;

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

                var showDialogComponent = entityManager.GetComponentData<NpcShowDialogComponent>(entites[i]);

                var conversationID = showDialogComponent.conversationID;

                curConversationData = GetConversationData(conversationID);

                if (curConversationData.conversations.Count > 0)
                {
                    text1.text = curConversationData.conversations[0].DialogText;

                    showDialogNpc = entites[i];

                    curDialogNodeIndex = 0;

                    StartCoroutine(disapearDialog(showDialogNpc, 5f));
                    break;
                }

       

/*                var system = World.DefaultGameObjectInjectionWorld.GetExistingSystem<SimulationSystemGroup>();
                if (system != null)
                {
                    var npcData = system.GetSingleton<NodeGraphSpawner>();

                    text1.text = npcData.Graph.Value.Nodes[0].Content.ToString();
                    showDialogNpc = entites[i];

                    curDialogNodeIndex =  0;


                    StartCoroutine(disapearDialog(showDialogNpc, 5f));

                    break;
                }*/
        
        
            }
        }

        entites.Dispose();


        IEnumerator disapearDialog(Entity entity, float duration)
        {
            yield return new WaitForSeconds(duration);

            World.DefaultGameObjectInjectionWorld.EntityManager.RemoveComponent<NpcShowDialogComponent>(entity);

         
        }

    }

    private ConversationDataBase GetConversationData(int id)
    {
        for (int i = 0; i < conversationDB.Count; i++)
        {
            if (id == conversationDB[i].ID)
            {
                return conversationDB[i];
            }
        }

        return default;
    }

    private void OnButtonClick()
    {

        if (curDialogNodeIndex >= 0 && curDialogNodeIndex < curConversationData.conversations.Count)
        {
            var curDialogNode = curConversationData.conversations[curDialogNodeIndex];
            text1.text = "";
            text2.text = "";

            if (curDialogNode.Links.Count != 0)
            {
                if (curDialogNode.Links.Count == 2)
                {
                    text2.text = curConversationData.conversations[curDialogNode.Links[1]].DialogText.ToString();
                }
                text1.text = curConversationData.conversations[curDialogNode.Links[0]].DialogText.ToString();

                curDialogNodeIndex = curDialogNode.Links[0];
            }
            else
            {
                text1.text = "";
                text2.text = "";

                curDialogNodeIndex = 0;

            }
        }



/*
        var system = World.DefaultGameObjectInjectionWorld.GetExistingSystem<SimulationSystemGroup>();
        if (system != null)
        {
            var nodeGraphSpawner = system.GetSingleton<NodeGraphSpawner>();

            ref var dialogTree = ref nodeGraphSpawner.Graph.Value;


            ref var curDialogNode = ref  nodeGraphSpawner.Graph.Value.Nodes[curDialogNodeIndex];

            text1.text = "";
            text2.text = "";

            if (curDialogNode.Links.Length != 0)
            {
                if (curDialogNode.Links.Length == 2)
                {
                    text2.text = dialogTree.Nodes[curDialogNode.Links[1]].Content.ToString();
                }
                text1.text = dialogTree.Nodes[curDialogNode.Links[0]].Content.ToString();

                curDialogNodeIndex = curDialogNode.Links[0];
            }
            else
            {
                text1.text = "";
                text2.text = "";

                curDialogNodeIndex = 0;

            }

        }*/

    }
}
