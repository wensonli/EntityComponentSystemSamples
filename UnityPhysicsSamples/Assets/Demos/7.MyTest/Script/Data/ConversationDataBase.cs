using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Conversation", menuName ="NPC System/Conversation")]
public class ConversationDataBase : ScriptableObject
{
    [Tooltip("conversation only ID")]
    public int ID;

    public string conversationName;


    public List<ConversationNode> conversations;


}


