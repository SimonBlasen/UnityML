using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsTower : Damageable
{
    public override void Damage(float damage)
    {
        base.Damage(damage);

        if (currHealth <= 0f)
        {
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}
