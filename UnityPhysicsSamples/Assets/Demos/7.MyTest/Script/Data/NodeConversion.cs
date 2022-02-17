using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


public struct DialogNode
{
    public BlobArray<int> Links;
    public FixedString128 Content;
}

public struct DialogTree
{
    public BlobArray<DialogNode> Nodes;
}

struct NodeGraphSpawner : IComponentData
{
    public BlobAssetReference<DialogTree> Graph;
}


public class NodeConversion : GameObjectConversionSystem
{
    BlobAssetReference<DialogTree> BuildNodeGraph(NodeAuthoring[] authoringNodes)
    {
        using (var builder = new BlobBuilder(Allocator.Temp))
        {
            ref var root = ref builder.ConstructRoot<DialogTree>();
            var nodeArray = builder.Allocate(ref root.Nodes, authoringNodes.Length);


            for (int i = 0; i < nodeArray.Length; i++)
            {
                nodeArray[i].Content = authoringNodes[i].Content;
                var links = builder.Allocate(ref nodeArray[i].Links, authoringNodes[i].links.Length);
                for (int j = 0; j < authoringNodes[i].links.Length; j++)
                {
                    links[j] = Array.IndexOf(authoringNodes, authoringNodes[i].links[j]);
                }
            }
            return builder.CreateBlobAssetReference<DialogTree>(Allocator.Persistent);
        }
    }

    private EntityQuery m_Query;

    protected override void OnCreate()
    {
        base.OnCreate();
        m_Query = GetEntityQuery(typeof(NodeAuthoring));
    }

    protected override void OnUpdate()
    {

        var authoringNodes = m_Query.ToComponentArray<NodeAuthoring>();
      //  var nodes = Array.FindAll(authoringNodes, node => node.links.Length > 0);

        var nodeGraph = BuildNodeGraph(authoringNodes);

        Entities.ForEach((GraphAuthoring npc) =>
        {
            DstEntityManager.AddComponentData(GetPrimaryEntity(npc), new NodeGraphSpawner
            {
                Graph = nodeGraph,
            });
        });
    }
}
