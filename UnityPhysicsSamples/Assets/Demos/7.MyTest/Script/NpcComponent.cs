using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;


namespace NpcSystem
{

    public struct NpcComponent : IComponentData
    {
        public int id;
        public long showDialogDuration;
        public FixedString64 nickName;
        public FixedString128 dialogContent;
    }



}


