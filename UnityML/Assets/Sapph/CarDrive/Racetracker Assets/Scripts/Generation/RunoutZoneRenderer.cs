using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunoutZoneRenderer : MonoBehaviour
{
    [SerializeField]
    private float trianglesSize = 1f;

    private TerrainModifier terrainModifier = null;

    private Vector3[] rightPoints = null;
    private Vector3[] leftPoints = null;

    private RegularTriangleMesh regTriaMesh;

    // Use this for initialization
    void Start ()
    {
        /*Vector2[] leftPoints = new Vector2[12];
        leftPoints[0] = new Vector2(10f, 20f);
        leftPoints[1] = new Vector2(10f, 30f);
        leftPoints[2] = new Vector2(15f, 40f);
        leftPoints[3] = new Vector2(15f, 50f);
        leftPoints[4] = new Vector2(20f, 60f);
        leftPoints[5] = new Vector2(30f, 65f);
        leftPoints[6] = new Vector2(40f, 60f);
        leftPoints[7] = new Vector2(40f, 50f);
        leftPoints[8] = new Vector2(35f, 40f);
        leftPoints[9] = new Vector2(35f, 30f);
        leftPoints[10] = new Vector2(40f, 20f);
        leftPoints[11] = new Vector2(35f, 10f);
        regTriaMesh = new RegularTriangleMesh(leftPoints, new Vector2[] { });
        regTriaMesh.FillMiddle(new Vector2(20f, 30f));
        

        Vector3[] vertices3D = new Vector3[regTriaMesh.Vertices.Length];
        for (int i = 0; i < vertices3D.Length; i++)
        {
            vertices3D[i] = new Vector3(regTriaMesh.Vertices[i].x, 0f, regTriaMesh.Vertices[i].y);
        }
        
        GetComponent<MeshFilter>().mesh.Clear();
        GetComponent<MeshFilter>().mesh.vertices = vertices3D;

        GetComponent<MeshFilter>().mesh.subMeshCount = 1;

        GetComponent<MeshFilter>().mesh.SetTriangles(regTriaMesh.Triangles, 0);

        GetComponent<MeshFilter>().mesh.RecalculateNormals();

        GetComponent<MeshCollider>().sharedMesh = GetComponent<MeshFilter>().mesh;*/
    }
	
	// Update is called once per frame
	void Update ()
    {

    }

    public void Render()
    {
        if (terrainModifier != null && leftPoints != null && rightPoints != null)
        {

        }
    }

    public TerrainModifier TerrainModifier
    {
        get
        {
            return terrainModifier;
        }
        set
        {
            terrainModifier = value;
        }
    }

    public Vector3[] RightPoints
    {
        get
        {
            return rightPoints;
        }
        set
        {
            rightPoints = value;
        }
    }

    public Vector3[] LeftPoints
    {
        get
        {
            return leftPoints;
        }
        set
        {
            leftPoints = value;
        }
    }
}


public class RegularTriangleMesh
{
    private Vector2[] rightPoints = null;
    private Vector2[] leftPoints = null;

    private List<TwoTriaMeshVertex> verticesInt;
    private List<Vector2> vertices;
    private List<int> triangles;

    private bool[,] activeTrianglesLeft = null;
    private bool[,] activeTrianglesRight = null;
    private bool[,] activeTrianglesMiddle = null;

    private int translateX = 128;
    private int translateY = 128;

    private float sqrt3 = Mathf.Sqrt(3f);

    public RegularTriangleMesh(Vector2[] leftPoints, Vector2[] rightPoints)
    {
        this.leftPoints = leftPoints;
        this.rightPoints = rightPoints;
        activeTrianglesLeft = new bool[256, 256];
        activeTrianglesRight = new bool[256, 256];
        activeTrianglesMiddle = new bool[256, 256];

        for (int i = 0; i < activeTrianglesMiddle.GetLength(0); i++)
        {
            for (int j = 0; j < activeTrianglesMiddle.GetLength(1); j++)
            {
                activeTrianglesLeft[i, j] = false;
                activeTrianglesRight[i, j] = false;
                activeTrianglesMiddle[i, j] = false;
            }
        }

        verticesInt = new List<TwoTriaMeshVertex>();
        triangles = new List<int>();
        vertices = new List<Vector2>();
    }

    public RegularTriangleMesh(Vector3[] leftPoints, Vector3[] rightPoints)
    {
        this.leftPoints = new Vector2[leftPoints.Length];
        this.rightPoints = new Vector2[rightPoints.Length];
        for (int i = 0; i < this.leftPoints.Length; i++)
        {
            this.leftPoints[i] = new Vector2(leftPoints[i].x, leftPoints[i].z);
        }
        for (int i = 0; i < this.rightPoints.Length; i++)
        {
            this.rightPoints[i] = new Vector2(rightPoints[i].x, rightPoints[i].z);
        }
        activeTrianglesLeft = new bool[256, 256];
        activeTrianglesRight = new bool[256, 256];
        activeTrianglesMiddle = new bool[256, 256];

        for (int i = 0; i < activeTrianglesMiddle.GetLength(0); i++)
        {
            for (int j = 0; j < activeTrianglesMiddle.GetLength(1); j++)
            {
                activeTrianglesLeft[i, j] = false;
                activeTrianglesRight[i, j] = false;
                activeTrianglesMiddle[i, j] = false;
            }
        }

        verticesInt = new List<TwoTriaMeshVertex>();
        triangles = new List<int>();
        vertices = new List<Vector2>();
    }

    public Vector2[] Vertices
    {
        get
        {
            return vertices.ToArray();
        }
    }

    public int[] Triangles
    {
        get
        {
            return triangles.ToArray();
        }
    }

    public void FillMiddle(Vector2 startPoint)
    {
        fillRecMiddle((int)startPoint.x, (int)startPoint.y);

        for (int i = 0; i < activeTrianglesMiddle.GetLength(0); i++)
        {
            for (int j = 0; j < activeTrianglesMiddle.GetLength(1); j++)
            {
                if (activeTrianglesMiddle[i, j])
                {
                    int absX = i - translateX;
                    int absY = j - translateY;

                    TwoTriaMeshVertex vertex1;
                    TwoTriaMeshVertex vertex2;
                    TwoTriaMeshVertex vertex3;

                    // Upside down
                    if ((absX + absY) % 2 == 0)
                    {
                        vertex1 = new TwoTriaMeshVertex(absX, absY);
                        vertex2 = new TwoTriaMeshVertex(absX - 1, absY + 1);
                        vertex3 = new TwoTriaMeshVertex(absX + 1, absY + 1);
                    }
                    // Upright
                    else
                    {
                        vertex1 = new TwoTriaMeshVertex(absX - 1, absY);
                        vertex2 = new TwoTriaMeshVertex(absX + 1, absY);
                        vertex3 = new TwoTriaMeshVertex(absX, absY + 1);
                    }
                    vertex1.deg = 1;
                    vertex2.deg = 1;
                    vertex3.deg = 1;

                    bool v1Found = false;
                    for (int k = verticesInt.Count - 1; k >= 0; k--)
                    {
                        if (verticesInt[k] == vertex1)
                        {
                            verticesInt[k].deg++;
                            triangles.Add(k);
                            v1Found = true;
                            break;
                        }
                    }
                    if (!v1Found)
                    {
                        verticesInt.Add(vertex1);
                        triangles.Add(verticesInt.Count - 1);
                    }


                    bool v2Found = false;
                    for (int k = verticesInt.Count - 1; k >= 0; k--)
                    {
                        if (verticesInt[k] == vertex2)
                        {
                            verticesInt[k].deg++;
                            triangles.Add(k);
                            v2Found = true;
                            break;
                        }
                    }
                    if (!v2Found)
                    {
                        verticesInt.Add(vertex2);
                        triangles.Add(verticesInt.Count - 1);
                    }


                    bool v3Found = false;
                    for (int k = verticesInt.Count - 1; k >= 0; k--)
                    {
                        if (verticesInt[k] == vertex3)
                        {
                            verticesInt[k].deg++;
                            triangles.Add(k);
                            v3Found = true;
                            break;
                        }
                    }
                    if (!v3Found)
                    {
                        verticesInt.Add(vertex3);
                        triangles.Add(verticesInt.Count - 1);
                    }


                    verticesInt[triangles[triangles.Count - 1]].AddEdge(verticesInt[triangles[triangles.Count - 2]]);
                    verticesInt[triangles[triangles.Count - 1]].AddEdge(verticesInt[triangles[triangles.Count - 3]]);
                    verticesInt[triangles[triangles.Count - 2]].AddEdge(verticesInt[triangles[triangles.Count - 3]]);
                    verticesInt[triangles[triangles.Count - 2]].AddEdge(verticesInt[triangles[triangles.Count - 1]]);
                    verticesInt[triangles[triangles.Count - 3]].AddEdge(verticesInt[triangles[triangles.Count - 2]]);
                    verticesInt[triangles[triangles.Count - 3]].AddEdge(verticesInt[triangles[triangles.Count - 1]]);
                }
            }
        }

        List<int> outerFace = new List<int>();
        List<int> outerFaceUnsorted = new List<int>();

        for (int i = 0; i < verticesInt.Count; i++)
        {
            if (verticesInt[i].deg < 6)
            {
                outerFaceUnsorted.Add(i);
            }
            vertices.Add(new Vector2(verticesInt[i].x, verticesInt[i].y - 0.5f));
        }

        outerFace.Add(outerFaceUnsorted[0]);
        for (int i = 1; i < outerFaceUnsorted.Count; i++)
        {
            for (int j = 0; j < outerFaceUnsorted.Count; j++)
            {
                if (outerFace.Contains(outerFaceUnsorted[j]) == false && verticesInt[outerFace[outerFace.Count - 1]].edges.Contains(verticesInt[outerFaceUnsorted[j]]))
                {
                    outerFace.Add(outerFaceUnsorted[j]);
                    break;
                }
            }
        }

        int leftPointsIndex = vertices.Count;
        for (int i = 0; i < leftPoints.Length; i++)
        {
            vertices.Add(leftPoints[i]);
        }
        int rightPointsIndex = vertices.Count;
        for (int i = 0; i < rightPoints.Length; i++)
        {
            vertices.Add(rightPoints[i]);
        }

        int iPoints = 0;
        int iFace = 0;
        float minDistance = float.MaxValue;
        int faceDirection = 1;
        for (int i = 0; i < outerFace.Count; i++)
        {
            if (Vector2.Distance(vertices[outerFace[i]], leftPoints[iPoints]) < minDistance)
            {
                iFace = i;
            }
        }
        int tempNextI = 1;
        if (Vector2.Distance(vertices[outerFace[(iFace + 1) % outerFace.Count]], leftPoints[tempNextI]) > Vector2.Distance(vertices[outerFace[(iFace - 1 < 0 ? outerFace.Count - 1 : iFace - 1)]], leftPoints[tempNextI]))
        {
            faceDirection = -1;
        }

        while (iPoints < leftPoints.Length)
        {
            int nextI = (iPoints + 1) % leftPoints.Length;
            int nextFace = ((iFace + faceDirection) < 0 ? outerFace.Count - 1 : (iFace + faceDirection)) % outerFace.Count;

            float distanceFaceNext = Vector2.Distance(vertices[iPoints + leftPointsIndex], vertices[outerFace[nextFace]]);
            float distanceLeftNext = Vector2.Distance(vertices[nextI + leftPointsIndex], vertices[outerFace[iFace]]);

            if (distanceLeftNext < distanceFaceNext)
            {
                triangles.Add(iPoints + leftPointsIndex);
                triangles.Add(iFace);
                triangles.Add(nextI + leftPointsIndex);
                iPoints++;
            }
            else
            {
                triangles.Add(iFace);
                triangles.Add(iPoints + leftPointsIndex);
                triangles.Add(nextFace);
                iFace = ((iFace + faceDirection) < 0 ? outerFace.Count - 1 : (iFace + faceDirection)) % outerFace.Count;
            }
        }
    }

    private void fillRecMiddle(int x, int y)
    {
        activeTrianglesMiddle[x, y] = true;

        if (x - 1 >= 0 && activeTrianglesMiddle[x - 1, y] == false && activeTrianglesLeft[x - 1, y] == false && activeTrianglesRight[x - 1, y] == false)
        {
            fillRecMiddle(x - 1, y);
        }
        if (y - 1 >= 0 && activeTrianglesMiddle[x, y - 1] == false && activeTrianglesLeft[x, y - 1] == false && activeTrianglesRight[x, y - 1] == false)
        {
            fillRecMiddle(x, y - 1);
        }
        if (x + 1 < activeTrianglesLeft.GetLength(0) && activeTrianglesMiddle[x + 1, y] == false && activeTrianglesLeft[x + 1, y] == false && activeTrianglesRight[x + 1, y] == false)
        {
            fillRecMiddle(x + 1, y);
        }
        if (y + 1 < activeTrianglesLeft.GetLength(1) && activeTrianglesMiddle[x, y + 1] == false && activeTrianglesLeft[x, y + 1] == false && activeTrianglesRight[x, y + 1] == false)
        {
            fillRecMiddle(x, y + 1);
        }
    }

    public void SetActive(Line line, bool left)
    {
        Line trLine = new Line(new Vector2(line.p1.x, line.p1.y / sqrt3), new Vector2(line.p2.x, line.p2.y / sqrt3));

        int xMin = (int)Mathf.Min(trLine.p1.x, trLine.p2.x);
        int xMax = ((int)Mathf.Max(trLine.p1.x, trLine.p2.x)) + 1;
        int yMin = (int)Mathf.Min(trLine.p1.y + 0.5f, trLine.p2.y + 0.5f);
        int yMax = (int)Mathf.Max(trLine.p1.y + 0.5f, trLine.p2.y + 0.5f);

        List<TwoTrianglesLine> twoTriasLines = new List<TwoTrianglesLine>();

        for (int x = xMin; x <= xMax; x++)
        {
            for (int y = yMin; y <= yMax; y++)
            {
                // Upside down
                if ((x + y) % 2 == 0)
                {
                    if (x == xMin)
                    {
                        twoTriasLines.Add(new TwoTrianglesLine(x, y, x - 1, y, new Line(new Vector2(x - 1f, y + 0.5f), new Vector2(x, y - 0.5f))));
                    }
                    twoTriasLines.Add(new TwoTrianglesLine(x, y, x, y + 1, new Line(new Vector2(x - 1f, y + 0.5f), new Vector2(x + 1f, y + 0.5f))));
                    twoTriasLines.Add(new TwoTrianglesLine(x, y, x + 1, y, new Line(new Vector2(x, y - 0.5f), new Vector2(x + 1f, y + 0.5f))));
                }
                //Upright
                else
                {
                    if (x == xMin)
                    {
                        twoTriasLines.Add(new TwoTrianglesLine(x, y, x - 1, y, new Line(new Vector2(x - 1f, y - 0.5f), new Vector2(x, y + 0.5f))));
                    }
                    if (y == yMin)
                    {
                        twoTriasLines.Add(new TwoTrianglesLine(x, y, x, y - 1, new Line(new Vector2(x - 1f, y - 0.5f), new Vector2(x + 1f, y - 0.5f))));
                    }
                    twoTriasLines.Add(new TwoTrianglesLine(x, y, x + 1, y, new Line(new Vector2(x, y + 0.5f), new Vector2(x + 1f, y - 0.5f))));
                }
            }
        }

        for (int i = 0; i < twoTriasLines.Count; i++)
        {
            Vector2 point;
            if (twoTriasLines[i].line.Intersects(trLine, out point))
            {
                setValue(twoTriasLines[i].x1, twoTriasLines[i].y1, true, left);
                setValue(twoTriasLines[i].x2, twoTriasLines[i].y2, true, left);
            }
        }
    }

    private bool getValueLeft(int x, int y)
    {
        int transX = x + translateX;
        int transY = y + translateY;

        if (transX >= 0 && transY >= 0 && transX < activeTrianglesLeft.GetLength(0) && transY < activeTrianglesLeft.GetLength(1))
        {
            return activeTrianglesLeft[transX, transY];
        }
        else
        {
            return false;
        }
    }

    private bool getValueRight(int x, int y)
    {
        int transX = x + translateX;
        int transY = y + translateY;

        if (transX >= 0 && transY >= 0 && transX < activeTrianglesRight.GetLength(0) && transY < activeTrianglesRight.GetLength(1))
        {
            return activeTrianglesRight[transX, transY];
        }
        else
        {
            return false;
        }
    }

    private bool getValueMiddle(int x, int y)
    {
        int transX = x + translateX;
        int transY = y + translateY;

        if (transX >= 0 && transY >= 0 && transX < activeTrianglesMiddle.GetLength(0) && transY < activeTrianglesMiddle.GetLength(1))
        {
            return activeTrianglesMiddle[transX, transY];
        }
        else
        {
            return false;
        }
    }

    private void setValue(int x, int y, bool val, bool left)
    {
        int transX = x + translateX;
        int transY = y + translateY;

        bool[,] translatedArrayLeft = null;
        bool[,] translatedArrayRight = null;
        bool[,] translatedArrayMiddle = null;
        int diffX = 0;
        int diffY = 0;
        if (transX < 0)
        {
            int oldTranslateX = translateX;
            translateX = -x;
            diffX = translateX - oldTranslateX;
        }
        if (transY < 0)
        {
            int oldTranslateY = translateY;
            translateY = -x;
            diffY = translateY - oldTranslateY;
        }

        if (diffX != 0 || diffY != 0)
        {
            translatedArrayLeft = new bool[activeTrianglesLeft.GetLength(0) + diffX, activeTrianglesLeft.GetLength(1) + diffY];
            translatedArrayRight = new bool[activeTrianglesLeft.GetLength(0) + diffX, activeTrianglesLeft.GetLength(1) + diffY];
            translatedArrayMiddle = new bool[activeTrianglesLeft.GetLength(0) + diffX, activeTrianglesLeft.GetLength(1) + diffY];
        }



        if (translatedArrayLeft != null)
        {
            for (int i = 0; i < translatedArrayLeft.GetLength(0); i++)
            {
                for (int j = 0; j < translatedArrayLeft.GetLength(1); j++)
                {
                    if (i - diffX >= 0 && j - diffY >= 0)
                    {
                        translatedArrayLeft[i, j] = activeTrianglesLeft[i - diffX, j - diffY];
                        translatedArrayRight[i, j] = activeTrianglesRight[i - diffX, j - diffY];
                        translatedArrayMiddle[i, j] = activeTrianglesMiddle[i - diffX, j - diffY];
                    }
                    else
                    {
                        translatedArrayLeft[i, j] = false;
                        translatedArrayRight[i, j] = false;
                        translatedArrayMiddle[i, j] = false;
                    }
                }
            }

            transX = x + translateX;
            transY = y + translateY;
        }



        bool[,] expandedArrayLeft = null;
        bool[,] expandedArrayRight = null;
        bool[,] expandedArrayMiddle = null;

        if (transX >= activeTrianglesLeft.GetLength(0))
        {
            expandedArrayLeft = new bool[transX + 1, activeTrianglesLeft.GetLength(1)];
            expandedArrayRight = new bool[transX + 1, activeTrianglesLeft.GetLength(1)];
            expandedArrayMiddle = new bool[transX + 1, activeTrianglesLeft.GetLength(1)];
        }
        else if (transY >= activeTrianglesLeft.GetLength(1))
        {
            expandedArrayLeft = new bool[activeTrianglesLeft.GetLength(0), transY + 1];
            expandedArrayRight = new bool[activeTrianglesLeft.GetLength(0), transY + 1];
            expandedArrayMiddle = new bool[activeTrianglesLeft.GetLength(0), transY + 1];
        }

        if (expandedArrayLeft != null)
        {
            for (int i = 0; i < expandedArrayLeft.GetLength(0); i++)
            {
                for (int j = 0; j < expandedArrayLeft.GetLength(1); j++)
                {
                    if (i < activeTrianglesLeft.GetLength(0) && j < activeTrianglesLeft.GetLength(1))
                    {
                        expandedArrayLeft[i, j] = activeTrianglesLeft[i, j];
                        expandedArrayRight[i, j] = activeTrianglesRight[i, j];
                        expandedArrayMiddle[i, j] = activeTrianglesMiddle[i, j];
                    }
                    else
                    {
                        expandedArrayLeft[i, j] = false;
                        expandedArrayRight[i, j] = false;
                        expandedArrayMiddle[i, j] = false;
                    }
                }
            }

            setValue(x, y, val, left);
        }
        else
        {
            if (left)
            {
                activeTrianglesLeft[transX, transY] = val;
            }
            else
            {
                activeTrianglesRight[transX, transY] = val;
            }
        }
    }
}


public class TwoTrianglesLine
{
    public int x1;
    public int y1;
    public int x2;
    public int y2;

    public Line line;

    public TwoTrianglesLine(int x1, int y1, int x2, int y2, Line line)
    {
        this.x1 = x1;
        this.x2 = x2;
        this.y1 = y1;
        this.y2 = y2;

        this.line = line;
    }
}

public class TwoTriaMeshVertex
{
    public int x;
    public int y;
    public int deg = 0;

    public List<TwoTriaMeshVertex> edges;

    public TwoTriaMeshVertex(int x, int y)
    {
        this.x = x;
        this.y = y;

        edges = new List<TwoTriaMeshVertex>();
    }

    public TwoTriaMeshVertex() : this(0, 0)
    {

    }


    public override bool Equals(object obj)
    {
        if (obj.GetType() == typeof(TwoTriaMeshVertex))
        {
            TwoTriaMeshVertex other = (TwoTriaMeshVertex)obj;
            return other.x == x && other.y == y;
        }

        return false;
    }

    public static bool operator ==(TwoTriaMeshVertex t1, TwoTriaMeshVertex t2)
    {
        return t1.x == t2.x && t1.y == t2.y;
    }

    public static bool operator !=(TwoTriaMeshVertex t1, TwoTriaMeshVertex t2)
    {
        return t1.x != t2.x || t1.y != t2.y;
    }

    public void AddEdge(TwoTriaMeshVertex other)
    {
        if (edges.Contains(other) == false)
        {
            edges.Add(other);
        }
    }
}