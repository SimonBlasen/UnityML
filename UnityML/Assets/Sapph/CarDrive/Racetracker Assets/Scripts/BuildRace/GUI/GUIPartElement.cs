using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIPartElement : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Text amountText;
    [SerializeField]
    private Image image;
    [SerializeField]
    private Image imageHovered;
    [SerializeField]
    private Image imageAmountZero;

    [Header("Textures")]
    [SerializeField]
    private Sprite part1x1x3;
    [SerializeField]
    private Sprite part1x1x5;
    [SerializeField]
    private Sprite part1x1x7;
    [SerializeField]
    private Sprite part1x1x9;
    [SerializeField]
    private Sprite part1x1x11;
    [SerializeField]
    private Sprite part1x1x13;
    [SerializeField]
    private Sprite part1x1x15;
    [SerializeField]
    private Sprite partCorner2x4;
    [SerializeField]
    private Sprite partCorner3x5;
    [SerializeField]
    private Sprite partPinRound2x1;
    [SerializeField]
    private Sprite partPinRound3x1;
    [SerializeField]
    private Sprite partTurn1x1x2;
    [SerializeField]
    private Sprite partAxis1x2;
    [SerializeField]
    private Sprite partAxis1x3;
    [SerializeField]
    private Sprite partAxis1x4;
    [SerializeField]
    private Sprite partAxis1x5;
    [SerializeField]
    private Sprite partAxis1x6;
    [SerializeField]
    private Sprite partAxis1x7;
    [SerializeField]
    private Sprite partAxis1x8;
    [SerializeField]
    private Sprite partCrossExtend;
    [SerializeField]
    private Sprite partGear24;
    [SerializeField]
    private Sprite partGear8;
    [SerializeField]
    private Sprite partGearCorner4;
    [SerializeField]
    private Sprite partMotorElectro1;
    [SerializeField]
    private Sprite partMotorElectro2;
    [SerializeField]
    private Sprite partMotorElectro3;
    [SerializeField]
    private Sprite partSeat;
    [SerializeField]
    private Sprite partSteer1Left;
    [SerializeField]
    private Sprite partTurn1x3x3;
    [SerializeField]
    private Sprite partWheelStreet1;
    [SerializeField]
    private Sprite partWheelStreet2;
    [SerializeField]
    private Sprite partWheelStreet3;
    [SerializeField]
    private Sprite partWheelStreet4;

    public GUIElementManager guiElementManager = null;

    private PartType type;
    private int amount = 0;

    // Use this for initialization
    void Start ()
    {
        if (amount <= 0)
        {
            imageAmountZero.enabled = true;
        }
        else
        {
            imageAmountZero.enabled = false;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public int Amount
    {
        get
        {
            return amount;
        }
        set
        {
            amount = value;
            amountText.text = "x" + amount;
            if (amount <= 0)
            {
                imageAmountZero.enabled = true;
            }
            else
            {
                imageAmountZero.enabled = false;
            }
        }
    }

    public PartType PartType
    {
        get
        {
            return type;
        }
        set
        {
            type = value;
            // TODO image setzen

            switch (type)
            {
                case PartType.Part1x1x3:
                    image.sprite = part1x1x3;
                    break;
                case PartType.Part1x1x5:
                    image.sprite = part1x1x5;
                    break;
                case PartType.Part1x1x7:
                    image.sprite = part1x1x7;
                    break;
                case PartType.Part1x1x9:
                    image.sprite = part1x1x9;
                    break;
                case PartType.Part1x1x11:
                    image.sprite = part1x1x11;
                    break;
                case PartType.Part1x1x13:
                    image.sprite = part1x1x13;
                    break;
                case PartType.Part1x1x15:
                    image.sprite = part1x1x15;
                    break;
                case PartType.PartCorner2x4:
                    image.sprite = partCorner2x4;
                    break;
                case PartType.PartCorner3x5:
                    image.sprite = partCorner3x5;
                    break;
                case PartType.PartPinRound2x1:
                    image.sprite = partPinRound2x1;
                    break;
                case PartType.PartPinRound3x1:
                    image.sprite = partPinRound3x1;
                    break;
                case PartType.PartTurn1x1x2:
                    image.sprite = partTurn1x1x2;
                    break;
                case PartType.PartAxis1x2:
                    image.sprite = partAxis1x2;
                    break;
                case PartType.PartAxis1x3:
                    image.sprite = partAxis1x3;
                    break;
                case PartType.PartAxis1x4:
                    image.sprite = partAxis1x4;
                    break;
                case PartType.PartAxis1x5:
                    image.sprite = partAxis1x5;
                    break;
                case PartType.PartAxis1x6:
                    image.sprite = partAxis1x6;
                    break;
                case PartType.PartAxis1x7:
                    image.sprite = partAxis1x7;
                    break;
                case PartType.PartAxis1x8:
                    image.sprite = partAxis1x8;
                    break;
                case PartType.PartCrossExtend:
                    image.sprite = partCrossExtend;
                    break;
                case PartType.PartGear24:
                    image.sprite = partGear24;
                    break;
                case PartType.PartGear8:
                    image.sprite = partGear8;
                    break;
                case PartType.PartGearCorner4:
                    image.sprite = partGearCorner4;
                    break;
                case PartType.PartMotorElectro1:
                    image.sprite = partMotorElectro1;
                    break;
                case PartType.PartMotorElectro2:
                    image.sprite = partMotorElectro2;
                    break;
                case PartType.PartMotorElectro3:
                    image.sprite = partMotorElectro3;
                    break;
                case PartType.PartSeat:
                    image.sprite = partSeat;
                    break;
                case PartType.PartSteer1Left:
                    image.sprite = partSteer1Left;
                    break;
                case PartType.PartTurn1x3x3:
                    image.sprite = partTurn1x3x3;
                    break;
                case PartType.PartWheelStreet1:
                    image.sprite = partWheelStreet1;
                    break;
                case PartType.PartWheelStreet2:
                    image.sprite = partWheelStreet2;
                    break;
                case PartType.PartWheelStreet3:
                    image.sprite = partWheelStreet3;
                    break;
                case PartType.PartWheelStreet4:
                    image.sprite = partWheelStreet4;
                    break;
            }
        }
    }

    public void HoverEnter()
    {
        imageHovered.enabled = true;
    }

    public void HoverExit()
    {
        imageHovered.enabled = false;
    }

    public void Click()
    {
        if (guiElementManager != null)
        {
            guiElementManager.PartSelectedClicked(type);
        }
    }
}
