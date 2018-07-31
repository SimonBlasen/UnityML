using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Level1
{
    public static string fileWithModel = "";
    public static InventoryElement[] elements = null;
    public static int garageIndex = -1;
}

public class InventoryElement
{
    public PartType partType;
    public int amount;
}

public static class Racing
{
    public static CalculatedCarD calculatedCar = null;
    public static CalculatedCar calculatedCarNew = null;
}