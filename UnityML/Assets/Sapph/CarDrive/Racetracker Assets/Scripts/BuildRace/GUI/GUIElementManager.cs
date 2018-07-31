using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIElementManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Canvas canvasGUI;
    [SerializeField]
    private ManagerBuilding manager;

    [Header("Settings")]
    [SerializeField]
    private int gapBetweenElements = 150;
    [SerializeField]
    private Vector2 leftTopPosition;
    [SerializeField]
    private Vector2 bottomRightPosition;

    [Header("Prefabs")]
    [SerializeField]
    private GameObject partElementPrefab;

    private List<GameObject> instObjs = new List<GameObject>();

    // Use this for initialization
    void Start ()
    {
        

        //int amountOfParts = 500;

        //AddElement(PartType.Part1x1x3, amountOfParts);
        //AddElement(PartType.Part1x1x5, amountOfParts);
        //AddElement(PartType.Part1x1x7, amountOfParts);
        //AddElement(PartType.Part1x1x9, amountOfParts);
        //AddElement(PartType.Part1x1x11, amountOfParts);
        //AddElement(PartType.Part1x1x13, amountOfParts);
        //AddElement(PartType.Part1x1x15, amountOfParts);
        //AddElement(PartType.PartPinRound2x1, amountOfParts);
        //AddElement(PartType.PartPinRound3x1, amountOfParts);
        //AddElement(PartType.PartTurn1x1x2, amountOfParts);
    }
	
	// Update is called once per frame
	void Update ()
    {
        
    }

    public void LoadFromLevel1()
    {
        if (Level1.fileWithModel != null && Level1.fileWithModel.Length > 0 && Level1.elements != null)
        {
            for (int i = 0; i < Level1.elements.Length; i++)
            {
                AddElement(Level1.elements[i].partType, Level1.elements[i].amount);
            }
        }
    }

    public void ClearElements()
    {
        for (int i = 0; i < instObjs.Count; i++)
        {
            Destroy(instObjs[i]);
        }

        instObjs.Clear();
    }

    public void AddElement(PartType type)
    {
        AddElement(type, 0);
    }

    public void AddElement(PartType type, int amount)
    {
        GameObject inst = (GameObject)Instantiate(partElementPrefab);
        inst.transform.parent = transform;
        inst.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
        inst.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
        inst.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        //inst.transform.localPosition = leftTopPosition;
        inst.GetComponent<RectTransform>().anchoredPosition = getNextElementPosition();
        // TODO rest machen, getNextElementPosition aufrufen und implementieren

        inst.GetComponent<GUIPartElement>().PartType = type;
        inst.GetComponent<GUIPartElement>().Amount = amount;
        inst.GetComponent<GUIPartElement>().guiElementManager = this;

        instObjs.Add(inst);
    }

    public GUIPartElement GetElement(PartType type)
    {
        for (int i = 0; i < instObjs.Count; i++)
        {
            if (instObjs[i].GetComponent<GUIPartElement>().PartType == type)
            {
                return instObjs[i].GetComponent<GUIPartElement>();
            }
        }

        return null;
    }

    public bool GUIShown
    {
        get
        {
            return canvasGUI.enabled;
        }
        set
        {
            canvasGUI.enabled = value;
        }
    }

    public void PartSelectedClicked(PartType type)
    {
        if (GetElement(type).Amount > 0)
        {
            manager.PartSelectedClicked(type);
        }
    }

    private Vector2 getNextElementPosition()
    {
        int elementsPerRow = (int)((bottomRightPosition.x - leftTopPosition.x) / gapBetweenElements);
        int xPos = instObjs.Count % elementsPerRow;
        xPos = xPos * gapBetweenElements;
        int yPos = instObjs.Count / elementsPerRow;
        yPos = yPos * gapBetweenElements * -1;

        xPos += (int)leftTopPosition.x;
        yPos += (int)leftTopPosition.y;

        return new Vector2(xPos, yPos);
    }
}
