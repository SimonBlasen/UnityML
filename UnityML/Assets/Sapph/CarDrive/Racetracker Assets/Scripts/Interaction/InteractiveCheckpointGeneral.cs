using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveCheckpointGeneral : MonoBehaviour {


    public Color colorHovered;
    public Color colorUnhovered;
    public Color colorRecentlyHovered;
    private bool hovered = false;
    private bool recentlyHovered = false;

    public bool Hovered
    {
        get
        {
            return hovered;
        }
        set
        {
            if (value != hovered)
            {
                hovered = value;
                Material copy = new Material(GetComponent<MeshRenderer>().sharedMaterial);
                if (value)
                    copy.color = colorHovered;
                else
                    copy.color = colorUnhovered;
                GetComponent<MeshRenderer>().sharedMaterial = copy;
            }
        }
    }

    public bool RecentlyHovered
    {
        get
        {
            return recentlyHovered;
        }
        set
        {
            if (value != recentlyHovered || GetComponent<MeshRenderer>().sharedMaterial.color != (value ? colorRecentlyHovered : colorUnhovered))
            {
                if (!hovered)
                {
                    recentlyHovered = value;
                    Material copy = new Material(GetComponent<MeshRenderer>().sharedMaterial);
                    if (value)
                        copy.color = colorRecentlyHovered;
                    else
                        copy.color = colorUnhovered;
                    GetComponent<MeshRenderer>().sharedMaterial = copy;
                }
            }
        }
    }

}
