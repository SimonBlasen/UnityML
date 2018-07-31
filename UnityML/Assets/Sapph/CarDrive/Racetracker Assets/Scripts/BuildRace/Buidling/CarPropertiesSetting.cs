using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPropertiesSetting
{
    private float damperHeightLF = 0.0f;
    private float damperHeightRF = 0.0f;
    private float damperHeightLR = 0.0f;
    private float damperHeightRR = 0.0f;
    private float damperStrengthLF = 0.0f;
    private float damperStrengthRF = 0.0f;
    private float damperStrengthLR = 0.0f;
    private float damperStrengthRR = 0.0f;
    public float SpringStrengthLF { get; set; }
    public float SpringStrengthRF { get; set; }
    public float SpringStrengthLR { get; set; }
    public float SpringStrengthRR { get; set; }
    private bool testBool = false;
    private int testInt = 0;
    private string testString = "";

    public CarPropertiesSetting()
    {
        SpringStrengthLF = 33786.9f;
        SpringStrengthRF = 33786.9f;
        SpringStrengthLR = 33786.9f;
        SpringStrengthRR = 33786.9f;

        DamperStrengthLF = 4344.03f;
        DamperStrengthRF = 4344.03f;
        DamperStrengthLR = 4344.03f;
        DamperStrengthRR = 4344.03f;
    }

    public bool TestBool
    {
        get
        {
            return testBool;
        }
        set
        {
            testBool = value;
        }
    }

    public int TestInt
    {
        get
        {
            return testInt;
        }
        set
        {
            testInt = value;
        }
    }

    public string TestString
    {
        get
        {
            return testString;
        }
        set
        {
            testString = value;
        }
    }

    public float DamperHeightLF
    {
        get
        {
            return damperHeightLF;
        }
        set
        {
            damperHeightLF = value;
        }
    }

    public float DamperHeightRF
    {
        get
        {
            return damperHeightRF;
        }
        set
        {
            damperHeightRF = value;
        }
    }

    public float DamperHeightLR
    {
        get
        {
            return damperHeightLR;
        }
        set
        {
            damperHeightLR = value;
        }
    }

    public float DamperHeightRR
    {
        get
        {
            return damperHeightRR;
        }
        set
        {
            damperHeightRR = value;
        }
    }

    public float DamperStrengthLF
    {
        get
        {
            return damperStrengthLF;
        }
        set
        {
            damperStrengthLF = value;
        }
    }

    public float DamperStrengthRF
    {
        get
        {
            return damperStrengthRF;
        }
        set
        {
            damperStrengthRF = value;
        }
    }

    public float DamperStrengthLR
    {
        get
        {
            return damperStrengthLR;
        }
        set
        {
            damperStrengthLR = value;
        }
    }

    public float DamperStrengthRR
    {
        get
        {
            return damperStrengthRR;
        }
        set
        {
            damperStrengthRR = value;
        }
    }

}
