using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RendererProfiler : RTProfiler
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
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

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
        instTrack.transform.localScale = new Vector3(20f, 1f, 20f);

        MeshFilter meshFilter = instTrack.GetComponent<MeshFilter>();

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        List<int> trianglesWhite = new List<int>();

        List<int> trianglesGrass = new List<int>();


        float xPosFirst = 0f;

        Vector3 middlePosFirst = racetrack.track.SplineAt(xPosFirst);
        middlePosFirst.y = racetrack.height.SplineAt(xPosFirst);

        Vector3[] tangentFirst = racetrack.track.TangentAt(xPosFirst);

        Vector3 forwardFirst = tangentFirst[1] - tangentFirst[0];
        forwardFirst.Normalize();
        Vector3 sidewardsFirst = Quaternion.Euler(0f, 90f, 0f) * forwardFirst;

        Vector3 upwardsFirst = Vector3.Cross(sidewardsFirst, forwardFirst) * -1f;
        upwardsFirst.Normalize();
        float distUpFirst = Mathf.Tan((racetrack.bend.SplineAt(xPosFirst) * Mathf.PI) / 180f);
        upwardsFirst *= distUpFirst;
        upwardsFirst *= 20f;

        sidewardsFirst.Normalize();

        Vector3 firstl_p = (sidewardsFirst + upwardsFirst) * -0.5f * racetrack.width.SplineAt(xPosFirst) + middlePosFirst;
        Vector3 firstr_p = (sidewardsFirst + upwardsFirst) * 0.5f * racetrack.width.SplineAt(xPosFirst) + middlePosFirst;

        Vector3 firstl_whiteBegin = firstl_p + (sidewardsFirst + upwardsFirst) * whiteStripOffset;
        Vector3 firstr_whiteBegin = firstr_p - (sidewardsFirst + upwardsFirst) * whiteStripOffset;

        Vector3 firstl_whiteInner = firstl_whiteBegin + (sidewardsFirst + upwardsFirst) * whiteStripWidth;
        Vector3 firstr_whiteInner = firstr_whiteBegin - (sidewardsFirst + upwardsFirst) * whiteStripWidth;

        Vector3 firstl_grassSameHeight = firstl_p - (sidewardsFirst + upwardsFirst) * grassWidth;
        Vector3 firstr_grassSameHeight = firstr_p + (sidewardsFirst + upwardsFirst) * grassWidth;

        Vector3 firstl_grassHill = firstl_grassSameHeight - (sidewardsFirst + upwardsFirst) * grassHillWidth + Vector3.Cross((sidewardsFirst), forwardFirst) * grassHillHeight;
        Vector3 firstr_grassHill = firstr_grassSameHeight + (sidewardsFirst + upwardsFirst) * grassHillWidth + Vector3.Cross((sidewardsFirst), forwardFirst) * grassHillHeight;

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

        for (int i = 1; i < resolution; i++)
        {
            float xPos = ((float)i) / ((float)resolution);

            Vector3 middlePos = racetrack.track.SplineAt(xPos);
            middlePos.y = racetrack.height.SplineAt(xPos);

            Vector3[] tangent = racetrack.track.TangentAt(xPos);

            Vector3 forward = tangent[1] - tangent[0];
            forward.Normalize();
            Vector3 sidewards = Quaternion.Euler(0f, 90f, 0f) * forward;

            Vector3 upwards = Vector3.Cross(sidewards, forward) * -1f;
            upwards.Normalize();
            float distUp = Mathf.Tan((racetrack.bend.SplineAt(xPos) * Mathf.PI) / 180f);
            upwards *= distUp;
            upwards *= 20f;

            //upwards = new Vector3(0f, 1f, 0f);

            Debug.Log("Distup: " + distUp);

            sidewards.Normalize();

            Vector3 l_p = (sidewards + upwards) * -0.5f * racetrack.width.SplineAt(xPos) + middlePos;
            Vector3 r_p = (sidewards + upwards) * 0.5f * racetrack.width.SplineAt(xPos) + middlePos;

            Vector3 l_whiteBegin = l_p + (sidewards + upwards) * whiteStripOffset;
            Vector3 r_whiteBegin = r_p - (sidewards + upwards) * whiteStripOffset;

            Vector3 l_whiteInner = l_whiteBegin + (sidewards + upwards) * whiteStripWidth;
            Vector3 r_whiteInner = r_whiteBegin - (sidewards + upwards) * whiteStripWidth;

            Vector3 l_grassSameHeight = l_p - (sidewards + upwards) * grassWidth;
            Vector3 r_grassSameHeight = r_p + (sidewards + upwards) * grassWidth;

            Vector3 l_grassHill = l_grassSameHeight - (sidewards + upwards) * grassHillWidth + Vector3.Cross((sidewards), forward) * grassHillHeight;
            Vector3 r_grassHill = r_grassSameHeight + (sidewards + upwards) * grassHillWidth + Vector3.Cross((sidewards), forward) * grassHillHeight;

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

        //Debug.Log("Submeshcount: " + meshFilter.mesh.subMeshCount);

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


public class RendTrackPart
{

}