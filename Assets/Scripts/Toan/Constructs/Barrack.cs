using Common.Entity;
using EnumCollection;
using InterfaceCollection;
using Manager;
using Pattern;
using UnityEngine;
using Utils;

namespace Common.Building
{
    public class Barrack : Construct,IProduce
    {
        protected override void Awake()
        {
            Id = ConstructId.Barrack;
            base.Awake();
        }
        protected override void Start()
        {
         
            SoilderElement[] buySoliderButtons = FindObjectsOfType<SoilderElement>();
            
            if(buySoliderButtons != null)
            {
                for (int i = 0; i < buySoliderButtons.Length; i++)
                {
                   
                    buySoliderButtons[i].setsomething(Produce);

                }
            }
                   
            base.Start();
        }

        public GameObject GetSoldier(System.Enum type)
        {
            if (type.GetType() == typeof(Soldier))
            {
                return AssetUtils.Instance.GetAsset(type.ToString()) as GameObject;
            }
            return null;
        }

        public void Produce(System.Enum type)
        {
          
            GameObject prefab = GetSoldier(type);
            if(prefab != null)
            {
                AIAgent agent = Instantiate(prefab, transform.position, Quaternion.identity).GetComponent<AIAgent>();
                agent.Owner = Player;
                agent.gameObject.SetActive(true);
                agent.SetTarget(TargetType.Place, Vector3.ProjectOnPlane(transform.position + transform.forward * 8,Vector3.up));
            }
        }
    }
}
