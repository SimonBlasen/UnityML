using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Statics
{
    public static bool writeLogFile = true;

    public static void Log(string message)
    {
        if (writeLogFile)
        {
            File.AppendAllText(".\\log.txt", "\n" + message);
        }
    }
}
