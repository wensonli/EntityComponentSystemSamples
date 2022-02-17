using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConversationNode
{
    public ActorType Actor;

    public ActorType Conversant;

    public string DialogText;

    public List<int> Links;
}


[System.Serializable]
public enum ActorType
{
    None,
    Player,
    NormalNpc
}
