using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RendererBezProfiler : RTProfiler
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
        if (instTrack != null)
        {
            Destroy(instTrack);
            instTrack = null;
        }
        instTrack = Instantiate(renderedTrackPrefab, transform);
        instTrack.transform.localPosition = Vector3.zero;

        Vector3[] leftCps = new Vector3[racetrack.track.controlPoints.Length];
        Vector3[] rightCps = new Vector3[racetrack.track.controlPoints.Length];

        for (int i = 0; i < leftCps.Length; i++)
        {
            Vector3 cpPos = racetrack.track.controlPoints[i];
            Vector3[] tangentCp = racetrack.track.TangentAt(((float)i) / ((float)leftCps.Length));
            Vector3 forwardCp = tangentCp[1] - tangentCp[0];
            forwardCp.Normalize();
            Vector3 sidewardsCp = Quaternion.Euler(0f, 90f, 0f) * forwardCp;
            sidewardsCp.Normalize();
            rightCps[i] = cpPos + sidewardsCp * racetrack.width.controlPoints[i] * 0.5f;
            leftCps[i] = cpPos - sidewardsCp * racetrack.width.controlPoints[i] * 0.5f;
        }

        ClosedSpline<Vector3> leftTrack = new ClosedSpline<Vector3>(leftCps);
        ClosedSpline<Vector3> rightTrack = new ClosedSpline<Vector3>(rightCps);



        MeshFilter meshFilter = instTrack.GetComponent<MeshFilter>();

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        List<int> trianglesWhite = new List<int>();

        List<int> trianglesGrass = new List<int>();


        float xPosFirst = 0f;

        /*Vector3 middlePosFirst = racetrack.track.SplineAt(xPosFirst);

        Vector3[] tangentFirst = racetrack.track.TangentAt(xPosFirst);

        Vector3 forwardFirst = tangentFirst[1] - tangentFirst[0];
        forwardFirst.Normalize();
        Vector3 sidewardsFirst = Quaternion.Euler(0f, 90f, 0f) * forwardFirst;

        sidewardsFirst.Normalize();

        Vector3 firstl_p = sidewardsFirst * -0.5f * racetrack.width.SplineAt(xPosFirst) + middlePosFirst;
        Vector3 firstr_p = sidewardsFirst * 0.5f * racetrack.width.SplineAt(xPosFirst) + middlePosFirst;

        Vector3 firstl_whiteBegin = firstl_p + sidewardsFirst * whiteStripOffset;
        Vector3 firstr_whiteBegin = firstr_p - sidewardsFirst * whiteStripOffset;

        Vector3 firstl_whiteInner = firstl_whiteBegin + sidewardsFirst * whiteStripWidth;
        Vector3 firstr_whiteInner = firstr_whiteBegin - sidewardsFirst * whiteStripWidth;

        Vector3 firstl_grassSameHeight = firstl_p - sidewardsFirst * grassWidth;
        Vector3 firstr_grassSameHeight = firstr_p + sidewardsFirst * grassWidth;

        Vector3 firstl_grassHill = firstl_grassSameHeight - sidewardsFirst * grassHillWidth + Vector3.Cross(sidewardsFirst, forwardFirst) * grassHillHeight;
        Vector3 firstr_grassHill = firstr_grassSameHeight + sidewardsFirst * grassHillWidth + Vector3.Cross(sidewardsFirst, forwardFirst) * grassHillHeight;*/

        Vector3[] tangentFirstLeft = leftTrack.TangentAt(xPosFirst);
        Vector3 forwardFirstLeft = tangentFirstLeft[1] - tangentFirstLeft[0];
        forwardFirstLeft.Normalize();
        Vector3 sidewardsFirstLeft = Quaternion.Euler(0f, 90f, 0f) * forwardFirstLeft;
        sidewardsFirstLeft.Normalize();

        Vector3[] tangentFirstRight = rightTrack.TangentAt(xPosFirst);
        Vector3 forwardFirstRight = tangentFirstRight[1] - tangentFirstRight[0];
        forwardFirstRight.Normalize();
        Vector3 sidewardsFirstRight = Quaternion.Euler(0f, 90f, 0f) * forwardFirstRight;
        sidewardsFirstRight.Normalize();

        Vector3 firstl_p = leftTrack.SplineAt(xPosFirst);
        Vector3 firstr_p = rightTrack.SplineAt(xPosFirst);

        Vector3 firstl_whiteBegin = firstl_p + sidewardsFirstLeft * whiteStripOffset;
        Vector3 firstr_whiteBegin = firstr_p - sidewardsFirstRight * whiteStripOffset;

        Vector3 firstl_whiteInner = firstl_whiteBegin + sidewardsFirstLeft * whiteStripWidth;
        Vector3 firstr_whiteInner = firstr_whiteBegin - sidewardsFirstRight * whiteStripWidth;

        Vector3 firstl_grassSameHeight = firstl_p - sidewardsFirstLeft * grassWidth;
        Vector3 firstr_grassSameHeight = firstr_p + sidewardsFirstRight * grassWidth;

        Vector3 firstl_grassHill = firstl_grassSameHeight - sidewardsFirstLeft * grassHillWidth + Vector3.Cross(sidewardsFirstLeft, forwardFirstLeft) * grassHillHeight;
        Vector3 firstr_grassHill = firstr_grassSameHeight + sidewardsFirstRight * grassHillWidth + Vector3.Cross(sidewardsFirstRight, forwardFirstRight) * grassHillHeight;

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

        for (int i = 1; i < resolution; i++)
        {
            float xPos = ((float)i) / ((float)resolution);

            /*Vector3 middlePos = racetrack.track.SplineAt(xPos);

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
            Vector3 r_grassHill = r_grassSameHeight + sidewards * grassHillWidth + Vector3.Cross(sidewards, forward) * grassHillHeight;*/

            Vector3[] tangentLeft = leftTrack.TangentAt(xPos);
            Vector3 forwardLeft = tangentLeft[1] - tangentLeft[0];
            forwardLeft.Normalize();
            Vector3 sidewardsLeft = Quaternion.Euler(0f, 90f, 0f) * forwardLeft;
            sidewardsLeft.Normalize();

            Vector3[] tangentRight = rightTrack.TangentAt(xPos);
            Vector3 forwardRight = tangentRight[1] - tangentRight[0];
            forwardRight.Normalize();
            Vector3 sidewardsRight = Quaternion.Euler(0f, 90f, 0f) * forwardRight;
            sidewardsRight.Normalize();

            Vector3 l_p = leftTrack.SplineAt(xPos);
            Vector3 r_p = rightTrack.SplineAt(xPos);

            Vector3 l_whiteBegin = l_p + sidewardsLeft * whiteStripOffset;
            Vector3 r_whiteBegin = r_p - sidewardsRight * whiteStripOffset;

            Vector3 l_whiteInner = l_whiteBegin + sidewardsLeft * whiteStripWidth;
            Vector3 r_whiteInner = r_whiteBegin - sidewardsRight * whiteStripWidth;

            Vector3 l_grassSameHeight = l_p - sidewardsLeft * grassWidth;
            Vector3 r_grassSameHeight = r_p + sidewardsRight * grassWidth;

            Vector3 l_grassHill = l_grassSameHeight - sidewardsLeft * grassHillWidth + Vector3.Cross(sidewardsLeft, forwardLeft) * grassHillHeight;
            Vector3 r_grassHill = r_grassSameHeight + sidewardsRight * grassHillWidth + Vector3.Cross(sidewardsRight, forwardRight) * grassHillHeight;
            

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
}