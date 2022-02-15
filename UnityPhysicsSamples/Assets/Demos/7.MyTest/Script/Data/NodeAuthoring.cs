﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeAuthoring : MonoBehaviour
{
    public NodeAuthoring[] links;

    public string Content;

    void OnDrawGizmos()
    {
        var position = transform.position;

        foreach (var link in links)
            Gizmos.DrawLine(position, link.transform.position);

        Gizmos.DrawWireCube(position, Vector3.one);
    }
}
