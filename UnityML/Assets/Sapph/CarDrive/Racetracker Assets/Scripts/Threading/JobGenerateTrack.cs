using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobGenerateTrack : ThreadedJob
{
    public long Seed { get; set; }
    public float Width { get; set; }
    public Terrain Terrain { get; set; }
    public TerrainModifier TerrainModifier { get; set; }

    public GeneratedTrackSupplyer[] Suppliers { get; set; }

    private GeneratedTrack track;
    public GeneratedTrack Track
    {
        get
        {
            return track;
        }
        set
        {
            track = value;
        }
    }

    private long tookSeed;
    public long TookSeed
    {
        get
        {
            return tookSeed;
        }
    }

    protected override void ThreadFunction()
    {
        if (Suppliers == null)
        {
            Suppliers = new GeneratedTrackSupplyer[0];
        }

        track = DiscreteInt2Generator.GenerateTrack(10f, Width, Seed);
        track.Seed = Seed;

        if (GenRendererGUI.JumpFaultyTracks)
        {
            while (track.Faulty)
            {
                //Seed = Utils.longRandom();
                Seed++;
                track = DiscreteInt2Generator.GenerateTrack(10f, Width, Seed);
                track.Seed = Seed;
            }
        }


        if (Terrain != null)
        {
            //track.ModifyTerrain(Terrain);

            track.SetTerrainModifier(TerrainModifier);
        }

        track.Analyze();

        track.GenerateBorder();

        for (int i = 0; i < Suppliers.Length; i++)
        {
            Suppliers[i].Track = track;
            Suppliers[i].TrackUpdated();
        }



        track.GenerateMesh();

        //track = track.Copy();


        /*Render(track);

        for (int i = 0; i < supplyers.Length; i++)
        {
            supplyers[i].Track = track;
            supplyers[i].TrackUpdated();
        }*/


    }
}
