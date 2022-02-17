using NpcSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;


/// <summary>
/// "Authoring" component for npcs. Part of the GameObject Conversion workflow.
/// Allows us to edit GameObjects in the Editor and convert those GameObjects to the optimized Entity representation
/// </summary>
[DisallowMultipleComponent]
public class NpcAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public int id;

    /// <summary>
    /// show nick name on npc head 
    /// </summary>
    public string nickName;

    /// <summary>
    /// show dialog conent when player near
    /// </summary>
    public string dialogContent;

    /// <summary>
    /// show dialog duration seconds
    /// </summary>
    public long showDialogDuration = 5;

    /// <summary>
    /// conversation id to database item
    /// </summary>
    public int conversationID;

    [SerializeField]
    private TextMesh txtNickName;

    private void Awake()
    {
        txtNickName = GetComponentInChildren<TextMesh>();

    }


    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponents(entity, new ComponentTypes(
            new ComponentType[]
            {
                typeof(NpcComponent),
                       
            }));


        dstManager.SetComponentData(entity, new NpcComponent
        {
            id = id,
            nickName = nickName,
            showDialogDuration = showDialogDuration,
            dialogContent = dialogContent,
            conversationID = conversationID

        });

        txtNickName.text = nickName;
    }
}
