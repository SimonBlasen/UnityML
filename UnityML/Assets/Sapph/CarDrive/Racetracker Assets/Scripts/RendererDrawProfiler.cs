using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RendererDrawProfiler : RTProfiler
{
    public int resolution = 100;
    public float whiteStripWidth = 0.5f;
    public float whiteStripOffset = 1f;
    public float grassWidth = 2f;
    public float grassHillWidth = 5f;
    public float grassHillHeight = 2f;

    public GameObject prefabPoint;

    public GameObject renderedTrackPrefab;

    private GameObject instTrack = null;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override Racetrack ApplyRacetrack(Racetrack racetrack)
    {
        List<Line> lines = new List<Line>();

        if (instTrack != null)
        {
            Destroy(instTrack);
            instTrack = null;
        }
        instTrack = Instantiate(renderedTrackPrefab, transform);
        instTrack.transform.localPosition = Vector3.zero;

        MeshFilter meshFilter = instTrack.GetComponent<MeshFilter>();

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        List<int> trianglesWhite = new List<int>();

        List<int> trianglesGrass = new List<int>();


        float xPosFirst = 0f;

        Vector3 middlePosFirst = racetrack.track.SplineAt(xPosFirst);

        Vector3[] tangentFirst = racetrack.track.TangentAt(xPosFirst);

        Vector3 forwardFirst = tangentFirst[1] - tangentFirst[0];
        forwardFirst.Normalize();
        Vector3 sidewardsFirst = Quaternion.Euler(0f, 90f, 0f) * forwardFirst;

        sidewardsFirst.Normalize();

        Vector3 firstl_p = sidewardsFirst * -0.5f * racetrack.width.SplineAt(xPosFirst) + middlePosFirst;
        Vector3 firstr_p = sidewardsFirst * 0.5f * racetrack.width.SplineAt(xPosFirst) + middlePosFirst;

        Vector3 middlePos_lOFirst = middlePosFirst - sidewardsFirst * 0.01f;
        Vector3 middlePos_rOFirst = middlePosFirst + sidewardsFirst * 0.01f;

        lines.Add(new Line(new Vector2(middlePos_lOFirst.x, middlePos_lOFirst.z), new Vector2(firstl_p.x, firstl_p.z)));
        lines.Add(new Line(new Vector2(middlePos_rOFirst.x, middlePos_rOFirst.z), new Vector2(firstr_p.x, firstr_p.z)));

        Vector3 firstl_whiteBegin = firstl_p + sidewardsFirst * whiteStripOffset;
        Vector3 firstr_whiteBegin = firstr_p - sidewardsFirst * whiteStripOffset;

        Vector3 firstl_whiteInner = firstl_whiteBegin + sidewardsFirst * whiteStripWidth;
        Vector3 firstr_whiteInner = firstr_whiteBegin - sidewardsFirst * whiteStripWidth;

        Vector3 firstl_grassSameHeight = firstl_p - sidewardsFirst * grassWidth;
        Vector3 firstr_grassSameHeight = firstr_p + sidewardsFirst * grassWidth;

        Vector3 firstl_grassHill = firstl_grassSameHeight - sidewardsFirst * grassHillWidth + Vector3.Cross(sidewardsFirst, forwardFirst) * grassHillHeight;
        Vector3 firstr_grassHill = firstr_grassSameHeight + sidewardsFirst * grassHillWidth + Vector3.Cross(sidewardsFirst, forwardFirst) * grassHillHeight;

        //Vector3 firstl_np = sidewardsFirst * -0.5f * racetrack.width.SplineAt(((float)i + 1) / ((float)resolution));
        //Vector3 firstr_np = sidewardsFirst * 0.5f * racetrack.width.SplineAt(((float)i + 1) / ((float)resolution));

        //vertices.Add(firstl_p);
        //vertices.Add(firstr_p);

        vertices.Add(firstl_grassHill);
        vertices.Add(firstl_grassSameHeight);
        vertices.Add(firstl_p);
        vertices.Add(firstl_whiteBegin);
        vertices.Add(firstl_whiteInner);
        vertices.Add(firstr_whiteInner);
        vertices.Add(firstr_whiteBegin);
        vertices.Add(firstr_p);
        vertices.Add(firstr_grassSameHeight);
        vertices.Add(firstr_grassHill);

        uvs.Add(new Vector2(firstl_grassHill.x, firstl_grassHill.z));
        uvs.Add(new Vector2(firstl_grassSameHeight.x, firstl_grassSameHeight.z));
        uvs.Add(new Vector2(firstl_p.x, firstl_p.z));
        uvs.Add(new Vector2(firstl_whiteBegin.x, firstl_whiteBegin.z));
        uvs.Add(new Vector2(firstl_whiteInner.x, firstl_whiteInner.z));
        uvs.Add(new Vector2(firstr_whiteInner.x, firstr_whiteInner.z));
        uvs.Add(new Vector2(firstr_whiteBegin.x, firstr_whiteBegin.z));
        uvs.Add(new Vector2(firstr_p.x, firstr_p.z));
        uvs.Add(new Vector2(firstr_grassSameHeight.x, firstr_grassSameHeight.z));
        uvs.Add(new Vector2(firstr_grassHill.x, firstr_grassHill.z));

        Vector3 old_l_grassHill = firstl_grassHill;
        Vector3 old_l_grassSameHeight = firstl_grassSameHeight;
        Vector3 old_l_p = firstl_p;
        Vector3 old_l_whiteBegin = firstl_whiteBegin;
        Vector3 old_l_whiteInner = firstl_whiteInner;
        Vector3 old_r_whiteInner = firstr_whiteInner;
        Vector3 old_r_whiteBegin = firstr_whiteBegin;
        Vector3 old_r_p = firstr_p;
        Vector3 old_r_grassSameHeight = firstr_grassSameHeight;
        Vector3 old_r_grassHill = firstr_grassHill;

        for (int i = 1; i < resolution; i++)
        {
            float xPos = ((float)i) / ((float)resolution);

            Vector3 middlePos = racetrack.track.SplineAt(xPos);

            Vector3[] tangent = racetrack.track.TangentAt(xPos);

            Vector3 forward = tangent[1] - tangent[0];
            forward.Normalize();
            Vector3 sidewards = Quaternion.Euler(0f, 90f, 0f) * forward;

            sidewards.Normalize();

            Vector3 l_p = sidewards * -0.5f * racetrack.width.SplineAt(xPos) + middlePos;
            Vector3 r_p = sidewards * 0.5f * racetrack.width.SplineAt(xPos) + middlePos;
            
            Vector3 l_whiteBegin = l_p + sidewards * whiteStripOffset;
            Vector3 r_whiteBegin = r_p - sidewards * whiteStripOffset;

            Vector3 l_whiteInner = l_whiteBegin + sidewards * whiteStripWidth;
            Vector3 r_whiteInner = r_whiteBegin - sidewards * whiteStripWidth;

            Vector3 l_grassSameHeight = l_p - sidewards * grassWidth;
            Vector3 r_grassSameHeight = r_p + sidewards * grassWidth;

            Vector3 l_grassHill = l_grassSameHeight - sidewards * grassHillWidth + Vector3.Cross(sidewards, forward) * grassHillHeight;
            Vector3 r_grassHill = r_grassSameHeight + sidewards * grassHillWidth + Vector3.Cross(sidewards, forward) * grassHillHeight;

            Vector3 middlePos_lO = middlePos - sidewards * 0.01f;
            Vector3 middlePos_rO = middlePos + sidewards * 0.01f;


            Line leftLine = new Line(new Vector2(middlePos_lO.x, middlePos_lO.z), new Vector2(l_p.x, l_p.z));
            if (checkLines(leftLine, lines) == false)
            {

                old_l_grassHill = l_grassHill;
                old_l_grassSameHeight = l_grassSameHeight;
                old_l_p = l_p;
                old_l_whiteBegin = l_whiteBegin;
                old_l_whiteInner = l_whiteInner;
            }
            lines.Add(new Line(new Vector2(middlePos_lO.x, middlePos_lO.z), new Vector2(l_p.x, l_p.z)));

            Line rightLine = new Line(new Vector2(middlePos_rO.x, middlePos_rO.z), new Vector2(r_p.x, r_p.z));
            if (checkLines(rightLine, lines) == false)
            {

                old_r_grassHill = r_grassHill;
                old_r_grassSameHeight = r_grassSameHeight;
                old_r_p = r_p;
                old_r_whiteBegin = r_whiteBegin;
                old_r_whiteInner = r_whiteInner;
            }
            lines.Add(new Line(new Vector2(middlePos_rO.x, middlePos_rO.z), new Vector2(r_p.x, r_p.z)));



            //Vector3 l_np = sidewards * -0.5f * racetrack.width.SplineAt(((float)i + 1) / ((float)resolution));
            //Vector3 r_np = sidewards * 0.5f * racetrack.width.SplineAt(((float)i + 1) / ((float)resolution));

            //vertices.Add(l_p);
            //vertices.Add(r_p);

            vertices.Add(old_l_grassHill);
            vertices.Add(old_l_grassSameHeight);
            vertices.Add(old_l_p);
            vertices.Add(old_l_whiteBegin);
            vertices.Add(old_l_whiteInner);
            vertices.Add(old_r_whiteInner);
            vertices.Add(old_r_whiteBegin);
            vertices.Add(old_r_p);
            vertices.Add(old_r_grassSameHeight);
            vertices.Add(old_r_grassHill);

            uvs.Add(new Vector2(l_grassHill.x, l_grassHill.z));
            uvs.Add(new Vector2(l_grassSameHeight.x, l_grassSameHeight.z));
            uvs.Add(new Vector2(l_p.x, l_p.z));
            uvs.Add(new Vector2(l_whiteBegin.x, l_whiteBegin.z));
            uvs.Add(new Vector2(l_whiteInner.x, l_whiteInner.z));
            uvs.Add(new Vector2(r_whiteInner.x, r_whiteInner.z));
            uvs.Add(new Vector2(r_whiteBegin.x, r_whiteBegin.z));
            uvs.Add(new Vector2(r_p.x, r_p.z));
            uvs.Add(new Vector2(r_grassSameHeight.x, r_grassSameHeight.z));
            uvs.Add(new Vector2(r_grassHill.x, r_grassHill.z));

            triangles.Add((i - 1) * 10 + 2);
            triangles.Add((i - 0) * 10 + 2);
            triangles.Add((i - 1) * 10 + 3);
            triangles.Add((i - 1) * 10 + 3);
            triangles.Add((i - 0) * 10 + 2);
            triangles.Add((i - 0) * 10 + 3);

            triangles.Add((i - 1) * 10 + 4);
            triangles.Add((i - 0) * 10 + 4);
            triangles.Add((i - 1) * 10 + 5);
            triangles.Add((i - 1) * 10 + 5);
            triangles.Add((i - 0) * 10 + 4);
            triangles.Add((i - 0) * 10 + 5);

            triangles.Add((i - 1) * 10 + 6);
            triangles.Add((i - 0) * 10 + 6);
            triangles.Add((i - 1) * 10 + 7);
            triangles.Add((i - 1) * 10 + 7);
            triangles.Add((i - 0) * 10 + 6);
            triangles.Add((i - 0) * 10 + 7);



            trianglesWhite.Add((i - 1) * 10 + 3);
            trianglesWhite.Add((i - 0) * 10 + 3);
            trianglesWhite.Add((i - 1) * 10 + 4);
            trianglesWhite.Add((i - 1) * 10 + 4);
            trianglesWhite.Add((i - 0) * 10 + 3);
            trianglesWhite.Add((i - 0) * 10 + 4);

            trianglesWhite.Add((i - 1) * 10 + 5);
            trianglesWhite.Add((i - 0) * 10 + 5);
            trianglesWhite.Add((i - 1) * 10 + 6);
            trianglesWhite.Add((i - 1) * 10 + 6);
            trianglesWhite.Add((i - 0) * 10 + 5);
            trianglesWhite.Add((i - 0) * 10 + 6);



            trianglesGrass.Add((i - 1) * 10 + 0);
            trianglesGrass.Add((i - 0) * 10 + 0);
            trianglesGrass.Add((i - 1) * 10 + 1);
            trianglesGrass.Add((i - 1) * 10 + 1);
            trianglesGrass.Add((i - 0) * 10 + 0);
            trianglesGrass.Add((i - 0) * 10 + 1);

            trianglesGrass.Add((i - 1) * 10 + 1);
            trianglesGrass.Add((i - 0) * 10 + 1);
            trianglesGrass.Add((i - 1) * 10 + 2);
            trianglesGrass.Add((i - 1) * 10 + 2);
            trianglesGrass.Add((i - 0) * 10 + 1);
            trianglesGrass.Add((i - 0) * 10 + 2);

            trianglesGrass.Add((i - 1) * 10 + 7);
            trianglesGrass.Add((i - 0) * 10 + 7);
            trianglesGrass.Add((i - 1) * 10 + 8);
            trianglesGrass.Add((i - 1) * 10 + 8);
            trianglesGrass.Add((i - 0) * 10 + 7);
            trianglesGrass.Add((i - 0) * 10 + 8);

            trianglesGrass.Add((i - 1) * 10 + 8);
            trianglesGrass.Add((i - 0) * 10 + 8);
            trianglesGrass.Add((i - 1) * 10 + 9);
            trianglesGrass.Add((i - 1) * 10 + 9);
            trianglesGrass.Add((i - 0) * 10 + 8);
            trianglesGrass.Add((i - 0) * 10 + 9);
        }

        if (true)
        {
            #region Last One
            /*float xPos = 0f;

            Vector3 middlePos = racetrack.track.SplineAt(xPos);

            Vector3[] tangent = racetrack.track.TangentAt(xPos);

            Vector3 forward = tangent[1] - tangent[0];
            forward.Normalize();
            Vector3 sidewards = Quaternion.Euler(0f, 90f, 0f) * forward;

            sidewards.Normalize();

            Vector3 l_p = sidewards * -0.5f * racetrack.width.SplineAt(xPos) + middlePos;
            Vector3 r_p = sidewards * 0.5f * racetrack.width.SplineAt(xPos) + middlePos;

            Vector3 l_whiteBegin = l_p + sidewards * whiteStripOffset;
            Vector3 r_whiteBegin = r_p - sidewards * whiteStripOffset;

            Vector3 l_whiteInner = l_whiteBegin + sidewards * whiteStripWidth;
            Vector3 r_whiteInner = r_whiteBegin - sidewards * whiteStripWidth;

            Vector3 l_grassSameHeight = l_p - sidewards * grassWidth;
            Vector3 r_grassSameHeight = r_p + sidewards * grassWidth;

            Vector3 l_grassHill = l_grassSameHeight - sidewards * grassHillWidth + Vector3.Cross(sidewards, forward) * grassHillHeight;
            Vector3 r_grassHill = r_grassSameHeight + sidewards * grassHillWidth + Vector3.Cross(sidewards, forward) * grassHillHeight;

            //Vector3 l_np = sidewards * -0.5f * racetrack.width.SplineAt(((float)i + 1) / ((float)resolution));
            //Vector3 r_np = sidewards * 0.5f * racetrack.width.SplineAt(((float)i + 1) / ((float)resolution));

            //vertices.Add(l_p);
            //vertices.Add(r_p);

            vertices.Add(l_grassHill);
            vertices.Add(l_grassSameHeight);
            vertices.Add(l_p);
            vertices.Add(l_whiteBegin);
            vertices.Add(l_whiteInner);
            vertices.Add(r_whiteInner);
            vertices.Add(r_whiteBegin);
            vertices.Add(r_p);
            vertices.Add(r_grassSameHeight);
            vertices.Add(r_grassHill);

            uvs.Add(new Vector2(l_grassHill.x, l_grassHill.z));
            uvs.Add(new Vector2(l_grassSameHeight.x, l_grassSameHeight.z));
            uvs.Add(new Vector2(l_p.x, l_p.z));
            uvs.Add(new Vector2(l_whiteBegin.x, l_whiteBegin.z));
            uvs.Add(new Vector2(l_whiteInner.x, l_whiteInner.z));
            uvs.Add(new Vector2(r_whiteInner.x, r_whiteInner.z));
            uvs.Add(new Vector2(r_whiteBegin.x, r_whiteBegin.z));
            uvs.Add(new Vector2(r_p.x, r_p.z));
            uvs.Add(new Vector2(r_grassSameHeight.x, r_grassSameHeight.z));
            uvs.Add(new Vector2(r_grassHill.x, r_grassHill.z));*/

            triangles.Add((resolution - 1) * 10 + 2);
            triangles.Add((0) * 10 + 2);
            triangles.Add((resolution - 1) * 10 + 3);
            triangles.Add((resolution - 1) * 10 + 3);
            triangles.Add((0) * 10 + 2);
            triangles.Add((0) * 10 + 3);

            triangles.Add((resolution - 1) * 10 + 4);
            triangles.Add((0) * 10 + 4);
            triangles.Add((resolution - 1) * 10 + 5);
            triangles.Add((resolution - 1) * 10 + 5);
            triangles.Add((0) * 10 + 4);
            triangles.Add((0) * 10 + 5);

            triangles.Add((resolution - 1) * 10 + 6);
            triangles.Add((0) * 10 + 6);
            triangles.Add((resolution - 1) * 10 + 7);
            triangles.Add((resolution - 1) * 10 + 7);
            triangles.Add((0) * 10 + 6);
            triangles.Add((0) * 10 + 7);



            trianglesWhite.Add((resolution - 1) * 10 + 3);
            trianglesWhite.Add((0) * 10 + 3);
            trianglesWhite.Add((resolution - 1) * 10 + 4);
            trianglesWhite.Add((resolution - 1) * 10 + 4);
            trianglesWhite.Add((0) * 10 + 3);
            trianglesWhite.Add((0) * 10 + 4);

            trianglesWhite.Add((resolution - 1) * 10 + 5);
            trianglesWhite.Add((0) * 10 + 5);
            trianglesWhite.Add((resolution - 1) * 10 + 6);
            trianglesWhite.Add((resolution - 1) * 10 + 6);
            trianglesWhite.Add((0) * 10 + 5);
            trianglesWhite.Add((0) * 10 + 6);



            trianglesGrass.Add((resolution - 1) * 10 + 0);
            trianglesGrass.Add((0) * 10 + 0);
            trianglesGrass.Add((resolution - 1) * 10 + 1);
            trianglesGrass.Add((resolution - 1) * 10 + 1);
            trianglesGrass.Add((0) * 10 + 0);
            trianglesGrass.Add((0) * 10 + 1);

            trianglesGrass.Add((resolution - 1) * 10 + 1);
            trianglesGrass.Add((0) * 10 + 1);
            trianglesGrass.Add((resolution - 1) * 10 + 2);
            trianglesGrass.Add((resolution - 1) * 10 + 2);
            trianglesGrass.Add((0) * 10 + 1);
            trianglesGrass.Add((0) * 10 + 2);

            trianglesGrass.Add((resolution - 1) * 10 + 7);
            trianglesGrass.Add((0) * 10 + 7);
            trianglesGrass.Add((resolution - 1) * 10 + 8);
            trianglesGrass.Add((resolution - 1) * 10 + 8);
            trianglesGrass.Add((0) * 10 + 7);
            trianglesGrass.Add((0) * 10 + 8);

            trianglesGrass.Add((resolution - 1) * 10 + 8);
            trianglesGrass.Add((0) * 10 + 8);
            trianglesGrass.Add((resolution - 1) * 10 + 9);
            trianglesGrass.Add((resolution - 1) * 10 + 9);
            trianglesGrass.Add((0) * 10 + 8);
            trianglesGrass.Add((0) * 10 + 9);
            #endregion
        }






        meshFilter.mesh.Clear();
        meshFilter.mesh.vertices = vertices.ToArray();
        meshFilter.mesh.uv = uvs.ToArray();

        meshFilter.mesh.subMeshCount = 3;

        Debug.Log("Submeshcount: " + meshFilter.mesh.subMeshCount);

        meshFilter.mesh.SetTriangles(triangles.ToArray(), 0);
        meshFilter.mesh.SetTriangles(trianglesWhite.ToArray(), 1);
        meshFilter.mesh.SetTriangles(trianglesGrass.ToArray(), 2);

        meshFilter.mesh.RecalculateNormals();

        instTrack.GetComponent<MeshCollider>().sharedMesh = meshFilter.mesh;








        /*for (int i = 0; i < resolution; i++)
        {
            float xPos = ((float)i) / ((float)resolution);

            GameObject inst = (GameObject)Instantiate(prefabPoint);
            inst.transform.position = racetrack.track.SplineAt(xPos);
        }*/

        return racetrack;
    }

    private bool checkLines(Line newLine, List<Line> oldLines)
    {
        bool intersects = false;

        for (int i = 0; i < oldLines.Count; i++)
        {
            Vector2 iSecPt;
            if (oldLines[i].Intersects(newLine, out iSecPt))
            {
                intersects = true;
                break;
            }
        }

        return intersects;
    }
}


public class Line
{
    public Vector2 p1;
    public Vector2 p2;

    public Line()
    {
        p1 = Vector2.zero;
        p2 = Vector2.zero;
    }

    public Line(Vector2 p1, Vector2 p2)
    {
        this.p1 = p1;
        this.p2 = p2;
    }

    public bool Intersects(Line other, out Vector2 point)
    {
        Vector2 s = p1;
        Vector2 d = p2 - p1;
        Vector2 s2 = other.p1;
        Vector2 d2 = other.p2 - other.p1;

        point = Vector2.zero;

        if ((d2.x * d.y - d2.y * d.x) != 0)
        {
            float v = (((s2.y / d.y) - (s.y / d.y) + (s.x / d.x) - (s2.x / d.x)) * d.y * d.x) / (d2.x * d.y - d2.y * d.x);
            float u = (s2.x + v * d2.x - s.x) / d.x;

            point = s + u * d;

            if (v >= 0f && v <= 1f && u >= 0f && u <= 1f)
            {
                return true;
            }
        }

        return false;
    }

    public float Length
    {
        get
        {
            return Vector2.Distance(p1, p2);
        }
    }

    public bool IntersectsOffset(Line other, float tolerance, out Vector2 point)
    {
        Vector2 p3 = other.p1;
        Vector2 p4 = other.p2;

        point = Vector2.zero;

        if ((p1.x - p2.x) * (p3.y - p4.y) - (p1.y - p2.y) * (p3.x - p4.x) != 0)
        {
            point.x = ((p1.x * p2.y - p1.y * p2.x) * (p3.x - p4.x) - (p1.x - p2.x) * (p3.x * p4.y - p3.y * p4.x)) / ((p1.x - p2.x) * (p3.y - p4.y) - (p1.y - p2.y) * (p3.x - p4.x));
            point.y = ((p1.x * p2.y - p1.y * p2.x) * (p3.y - p4.y) - (p1.y - p2.y) * (p3.x * p4.y - p3.y * p4.x)) / ((p1.x - p2.x) * (p3.y - p4.y) - (p1.y - p2.y) * (p3.x - p4.x));

            bool isIntersectPoint = false;

            if ((point.x >= p1.x && point.x <= p2.x && point.y >= p1.y && point.y <= p2.y)
                || (point.x <= p1.x && point.x >= p2.x && point.y >= p1.y && point.y <= p2.y)
                || (point.x >= p1.x && point.x <= p2.x && point.y <= p1.y && point.y >= p2.y)
                || (point.x <= p1.x && point.x >= p2.x && point.y <= p1.y && point.y >= p2.y))
            {
                if ((point.x >= p3.x && point.x <= p4.x && point.y >= p3.y && point.y <= p4.y)
                || (point.x <= p3.x && point.x >= p4.x && point.y >= p3.y && point.y <= p4.y)
                || (point.x >= p3.x && point.x <= p4.x && point.y <= p3.y && point.y >= p4.y)
                || (point.x <= p3.x && point.x >= p4.x && point.y <= p3.y && point.y >= p4.y))
                {
                    isIntersectPoint = true;
                }
            }

            float smallestLength = Mathf.Min(Length, other.Length);

            if (isIntersectPoint
                && Vector2.Distance(point, p1) >= smallestLength * tolerance
                && Vector2.Distance(point, p2) >= smallestLength * tolerance
                && Vector2.Distance(point, p3) >= smallestLength * tolerance
                && Vector2.Distance(point, p4) >= smallestLength * tolerance)
            {
                return true;
            }
        }

        return false;
    }
}