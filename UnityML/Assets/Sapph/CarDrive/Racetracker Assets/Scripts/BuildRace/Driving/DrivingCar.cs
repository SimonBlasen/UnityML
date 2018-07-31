using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DrivingCar : Damageable
{
    protected bool[] shootingWeapon = new bool[1];

    public byte ID { get; set; }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public abstract float CurrentAvgRPM { get; }

    public abstract float CurrentSteerAngle { get; }

    public abstract float MaxSteerAngle { get; }

    public void SetShooting(int weapon, bool on)
    {
        if (weapon >= 0 && weapon < shootingWeapon.Length)
        {
            shootingWeapon[weapon] = on;
        }
    }

    public bool[] ShootingWeapon
    {
        get
        {
            return shootingWeapon;
        }
    }
}
