using DelegateCollection;
using EnumCollection;
using Manager;
using Pattern;
using RTS_ScriptableObject;
using UnityEngine;

namespace Common
{
    public abstract class Construct : GameEntity
    {
        public ConstructOffset Offset;
        public Player Player;
        public GameObject destructEffect;
        public int Hp                       { get; protected set; }
        public Group Group                  { get; set; }
        public ConstructId Id               { get; protected set; }
        public ConstructId[] Owned          { get; protected set; }
        public GameAction AddConstruct      { protected get; set; }
        public GameAction RemoveConstruct   { protected get; set; }
        private HPBar hpimage;
        public BuildElement Cellinfo;
        public override Vector3 Position
        {
            get
            { return Vector3.ProjectOnPlane(transform.position, Vector3.up); }
        }
        public override Vector3 Heading
        {
            get
            { return Vector3.ProjectOnPlane(transform.forward, Vector3.up); }
        }
        public override Vector3 Velocity { get { return Vector3.zero; } }
        public override bool IsDead { get; protected set; }
       
        protected virtual void Awake()
        {
            if (Group == Group.NPC)
            {
                gameObject.layer = LayerMask.NameToLayer("NPC");
                Player = FindObjectOfType<NPCPlayer>();
            }
            else
            {
                gameObject.layer = LayerMask.NameToLayer("Construct");
                Player = FindObjectOfType<MainPlayer>();
            }
            Init();
        }
        protected virtual void Start()
        {
           
            AddConstruct        = Player.AddConstruct;
            RemoveConstruct     = Player.RemoveConstruct;

            hpimage = this.GetComponentInChildren<HPBar>();
            InitOffset();
        }
        protected virtual void Update() { }

        protected void Init()
        {
            switch (Id)
            {
                case ConstructId.Yard:
                    Owned = new ConstructId[]
                    {
                    ConstructId.Refinery,
                    };
                    break;
                case ConstructId.Refinery:
                    Owned = new ConstructId[]
                    {
                    ConstructId.Barrack,
                    };
                    break;
                case ConstructId.Barrack:
                    Owned = new ConstructId[]
                    {
                    ConstructId.Defender,
                    ConstructId.Radar,
                    };
                    break;
                default:
                    Owned = new ConstructId[0];
                    break;
            }
        }
        protected void UnlockConstruct()
        {
            if(AddConstruct == null) AddConstruct = Player.AddConstruct;
            AddConstruct(this);
        }

        protected virtual void InitOffset()
        {
            Hp = Offset.MaxHP;
        }

        // public method
        public override void TakeDamage(int damage)
        {
            Hp -= damage;

            hpimage.SetValue((float)Hp / Offset.MaxHP);
            if (Hp <= 0)
            {
                IsDead = true;
                Hp = 0;
                DestroyConstruct();
            }
        }
        public virtual void Build()
        {
            UnlockConstruct();
        }
        public virtual void DestroyConstruct()
        {
            var effect = Instantiate(destructEffect, transform.position, Quaternion.identity);
            RemoveConstruct(this);
            Destroy(transform.root.gameObject);
            Destroy(effect, 0.5f);
            if (Cellinfo != null)
            {
               
                Cellinfo.letOnDestroy(transform.position);
            }
        }

    }
}