using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class NPCPlayer : Player
    {
        protected override void Start()
        {
            Group = EnumCollection.Group.NPC;
            base.Start();
        }
    }
}