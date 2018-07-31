using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobGenerateTrackContin : ThreadedJob
{
    public long Seed { get; set; }
    public int OffsetJump { get; set; }
    public float Width { get; set; }
    public Terrain Terrain { get; set; }
    public TerrainModifier TerrainModifier { get; set; }

    public bool Stop { get; set; }

    public List<GeneratedTrack> succTracks = new List<GeneratedTrack>();

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
        Stop = false;

        if (Suppliers == null)
        {
            Suppliers = new GeneratedTrackSupplyer[0];
        }

        while (Stop == false)
        {
            track = DiscreteInt2Generator.GenerateTrack(10f, Width, Seed);
            track.Seed = Seed;


            if (GenRendererGUI.JumpFaultyTracks)
            {
                while (track.Faulty && Stop == false)
                {
                    //Seed = Utils.longRandom();
                    Seed += OffsetJump;
                    track = DiscreteInt2Generator.GenerateTrack(10f, Width, Seed);
                    track.Seed = Seed;
                }
            }


            if (Stop == false)
            {
                if (Terrain != null)
                {
                    //track.ModifyTerrain(Terrain);

                    track.SetTerrainModifier(TerrainModifier);
                }

                track.Analyze();

                track.GenerateBorder();

                /*for (int i = 0; i < Suppliers.Length; i++)
                {
                    Suppliers[i].Track = track;
                    Suppliers[i].TrackUpdated();
                }*/



                track.GenerateMesh();

                succTracks.Add(track);

                Seed += OffsetJump;
            }
        }



        /*Render(track);

        for (int i = 0; i < supplyers.Length; i++)
        {
            supplyers[i].Track = track;
            supplyers[i].TrackUpdated();
        }*/


    }
}
