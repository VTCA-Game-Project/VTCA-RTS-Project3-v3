using System;
using EnumCollection;
using InterfaceCollection;
using Manager;
using UnityEngine;

namespace Common.Building
{
    public class Refinery : Construct, IProduce
    {
        public float RefreshTime;
        public int MaxRetainGold { get; protected set; }
        public int RemainingGold { get; protected set; }
        public bool IsMax
        {
            get { return counter == RefreshTime; }
        }


        private float counter;
        protected override void Awake()
        {
            MaxRetainGold = 300;
            counter = 0.0f;

            Id = ConstructId.Refinery;
            base.Awake();
        }
        protected override void Start()
        {
            base.Start();
        }
        protected override void Update()
        {
            counter += Time.deltaTime;
            if (counter > RefreshTime)
            {
                counter = RefreshTime;
            }
            RemainingGold = (int)(MaxRetainGold * (counter / RefreshTime));
        }

        protected void OnMouseDown()
        {
            if (Player.Group == Group.Player)
            {
                Produce(null);
                SoundManager.instanece.PlayEffect(0);
            }
        }

        public void Produce(Enum type = null)
        {
            if (RemainingGold > 0)
            {
                Player.TakeGold(RemainingGold);
                counter = 0.0f;
                RemainingGold = 0;
                if (Player.Group == Group.NPC)
                    Debug.Log(Player.GetGold());
            }
        }
    }
}