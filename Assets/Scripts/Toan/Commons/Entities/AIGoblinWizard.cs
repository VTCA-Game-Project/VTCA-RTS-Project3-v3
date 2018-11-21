using Common.Entity;
using InterfaceCollection;
using UnityEngine;

public class AIGoblinWizard : AIAgent
{
    public GameObject effect;
    public void Update()
    {
    }
    public override void Attack()
    {       
        base.Attack();
        if(TargetEntity != null)
        {
            Vector3 effectpos = new Vector3(TargetEntity.transform.position.x, TargetEntity.transform.position.y+5f, TargetEntity.transform.position.z);
                var ef = Instantiate(effect, effectpos, transform.rotation);
                if (ef != null)
                {
                    ef.gameObject.SetActive(true);                   
                }
           

            TargetEntity.TakeDamage(Damage);
        }
    }
}
