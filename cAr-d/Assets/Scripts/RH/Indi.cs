using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Individual
public class Indi
{
    private GameObject[] segs = new GameObject[3];
    public List<GameObject> chromosome;//Object of tiles
    public List<string> chrs; //String of tiles
    private float fitness; //FITNESS
    public static int SEQUENCE_LENGTH=20;//LENGTH TRACK
    public static float mutation = 0.3f; //Mutation
    public int metric;
    


    public float FITNESS
    {
        get
        {
            return fitness;
        }
        set
        {
            fitness = value;
        }
    }


    public List<GameObject> CHROMOSOME
    {
        get
        {
            return chromosome;
        }
        set
        {
            chromosome = value;
        }
    }

    public List<string> CHRS
    {
        get
        {
            return chrs;
        }
        set
        {
            chrs = value;
        }
    }

    public int METRIC
    {
        get
        {
            return metric;
        }
        set
        {
            metric = value;
        }
    }

    public Indi(float ff)
    {
        fitness = My_fitness(ff);
        chromosome = new List<GameObject>();
        chrs = new List<string>();
        metric = 0;
        FindSegs();
    }
    
    public void FindSegs()
    {
        segs[0] = GameObject.FindGameObjectWithTag("straight");
        segs[1] = GameObject.FindGameObjectWithTag("turnR");
        segs[2] = GameObject.FindGameObjectWithTag("turnL");
    }




    public float My_fitness(float ff)
    {

        float fitness = ff;
        return fitness;
    }


    // Perform mating and produce new offspring 
    public string Crossover(Indi par2, int p)
    {
        // random probability  
        float pp = Random.Range(0.0f, 100.0f) / 100;

        // chromosome for offspring 
        string child_chromosome;

        // if prob is less than 0.3, insert gene 
        // from parent 1  
        if (pp < mutation)
        {
            child_chromosome = chrs[p];

        }
        // if prob is between 0.3 and 0.6 insert 
        // gene from parent 2 
        else if (pp < (mutation* 2))
        {
            child_chromosome = par2.chrs[p];
        }
        else
        {
            int rr = Random.Range(0, segs.Length);
            child_chromosome = segs[rr].tag;
        }
        return child_chromosome;

    }

    public static void ScrableShuffle(List<string> ts)
    {
        for (int i = 0; i < RH.pTrackLength / 10; i++)
        {

            float pp = Random.Range(0.0f, 100.0f) / 100;
            //Debug.Log("pm"+pp);
            if (pp < Indi.mutation)
            {
                //Debug.Log("Benw sto shuffle gia i"+i);
                int start = i * 10;
                int end = (i + 1) * 10;
                Shuffle(ts, start, end);
            }
        }

    }
    public static void Shuffle(List<string> ts, int start, int last)
    {

        //    if (start == 0)
        //  {
        //    start += 1;
        // }
        //Debug.Log("TS C " +ts.Count);
       //Debug.Log("start" + start + "last" + last);
        for (int i = start; i < last; i++)
        {

            int r = UnityEngine.Random.Range(i, last);
            string tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }

}
