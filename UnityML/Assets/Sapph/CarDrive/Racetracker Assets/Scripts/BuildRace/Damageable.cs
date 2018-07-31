using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour {

    [SerializeField]
    protected float maxHealth = 1f;

    [SerializeField]
    protected float currHealth;

	// Use this for initialization
	void Start () {
        currHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual void Damage(float damage)
    {
        currHealth -= damage;
        currHealth = currHealth < 0f ? 0f : currHealth;
    }

    public virtual void RepairComplete()
    {
        currHealth = maxHealth;
    }
}
