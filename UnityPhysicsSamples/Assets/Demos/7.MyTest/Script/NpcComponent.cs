using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;


namespace NpcSystem
{
    [GenerateAuthoringComponent]
    public struct NpcComponent : IComponentData
    {
        public int id;
        public FixedString64 name;
        public bool showDialog;
    }



}


