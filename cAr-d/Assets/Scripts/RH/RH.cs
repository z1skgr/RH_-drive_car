using System.Collections.Generic;
using UnityEngine;
using System.Collections;
public class RH : MonoBehaviour
{
    // Start is called before the first frame update
    public static string[] seg = { "straight", "turnR", "turnL" }; // H eutheia kai oi strofes se string

    //Static Variables
    public static bool[] startt = { };/*Population triggers */
    public static bool[] endt = { };

    public static float[,] timeLapsed;/*Population times & Fitnesses */
    public static float avgLapsed;
    public static float maxLapsed;
    public static float minLapsed;
    public static float cdf;

    //Gap between tracks
    public static float popGap;
    //Change line after number of tracks
    public static int trackGap;

    public static bool[] evolvestartt = { };/*Evolved Triggers & variables*/
    public static bool[] evolveendt = { };

    //Gap between evolved and population
    public static float firstevolvedGap;
    //Gap between evolved tracks
    public static float otherevolvedGap;

    public static bool destroyed = true;
    public static bool Simfinished;

    //PREFABS
    public GameObject[] segments; //3 tiles

    public GameObject driver; //CarAgent
    public GameObject[] SegmentTrigger; // Start and End trigger
    public GameObject StartLine; //Start Line

    //Generation
    private List<Indi> population; /*P_k*/
    private List<Indi> new_generation;  /*P_k+1*/
    private List<Indi> new_generation2; /*Evolved generation with 10 tiles by crossoving tiles until checkpoint */
    private List<Indi> new_generation3; /*Evolved Tracks */
    public static List<Indi> pool; //Pool for promoting offspings to new generations
    public static int evolutioncount; //Evolutions

    //Variables for construction tracks
    private float[] pValue;
    private float[,] pArray;
    private Vector3[] boxSize;
    private string[,] direction;
    private bool firstPop; //First population
    public static bool endofEvolution;


    private float[,] specArray;
    private string[] specDirection;

    private bool Collide; /*Collision between tiles in the same track*/


    //Additional variables for construction
    private GameObject startline;
    private GameObject starttrigger, endtrigger;
    private GameObject[] lines;
    private GameObject checktrigger;
    public GameObject check;

    //Counters for tracks,segments etc
    public static int POPULATION_SIZE;
    private int populationCount;
    private int segmentCount;
    private int count = 0;

    //Variables for construction  evolved tracks
    private int s;
    private float[] eValue;
    private float[,] eArray;
    public GameObject[] evolveTrigger;
    public GameObject evolveLine;

    public static float[] evolveLapsed;

    //Variables for execution the simulation
    private bool cpEvolved; //End Checkpoint evolution
    public static bool endEvolved;    // End evolution
    private bool eSimEnd;   //Evolved simulation end
    private bool eval = false; //Assign Fitnesss
    private bool eveval = false;//Assign Fitness to evolved tracks
    public static bool avgWritten = false; //Write fitnesses to file
    public static bool checkReached = false; // All Cars passed checkpoints
    public static bool promo = false;


    ///Temporary Variables
    private Indi chromo;
    private Indi parent1, parent2;
    private Indi newch;
    private Indi chromomm;

    int r;
    private bool sttime = false;
    ///


    //Player Track Variables
    public static List<string> nextTrack;

    public static float[] playerValue;
    public static float[,] playerArray;
    public static string[,] playerDirection;
    public static float[,] playerArray2;
    public static string[,] playerDirection2;

    public static int level = 0;

    private List<Indi> playerTrack;
    private int tracklength;
    public static bool build = false;

    //Player Track Construction
    public GameObject playerLine;
    public GameObject EvolveT;
    public GameObject[] player;
    public static int pTrackLength;
    public static bool playerfinish = false;


    private bool ftime = true;//Variable for creation of car agent in playertrack . True = Car creation. 





    /*Boundar Triggers & variables*/
    public GameObject[] boundaryTrigger;
    public static bool[] boundarystartt = { };
    public static bool[] boundaryendt = { };
    public static float[] boundaryLapsed;
    private int tracklength2 = 0;
    public GameObject bLine;

    /*Boundar Variables for writing files*/
    public static bool isBound = false;
    public static bool endBound = false;


    /*Tiles --->5 variables for predicting next tiles in tracks*/
    public static string nextTile;
    public static string nextbTile;//bound
    public static string nextcTile;//init pop
    public static string nextpTile;//player
    public static string nexteTile;//evolve
    public static string nextcsTile;//population
    public static int metrioc;


    private GameObject deleted;

    
    /* Channel Func ---> */
    public static float[] bounds0;
    public static float[] bounds60;
    
    private bool done = false;



    public static int eprepe = 0;
    public static int exw = 0;

    /*-----*?*/
    public static int curv = 0;
    void Awake()
    {
        Initialize();
        //Boundary Snake Tracks
        CreateNoStraight(5);
        CreateNoStraight(6);
        CreateNoStraight(7);
        CreateNoStraight(8);
        CreateNoStraight(9);
    }

    private void Start()
    {
        //Boundary Straight Tracks
        CreateStraight(0, false);
        CreateStraight(1, false);
        CreateStraight(2,false);
        CreateStraight(3,false);
        CreateStraight(4, true);
    }


    /*CreateStraight
    Create Boundary Track with straights only
    */

    /*
     Steps on creating tracks
    1)Trigger First
    2)Tiles
    3)CheckPoints
    4)Trigger End
     */
    private void CreateStraight(int pp, bool delete)
    {
        int zz = 0;

        while (zz < pTrackLength - 1)
        {
            zz = tracklength2;

            //Creating straight tiles
            string ss;
            if (zz == 0)
            {
                ss = "straight";
            }
            else
            {
                ss = nextbTile;
            }
            //Update next tiles
            nextbTile = "straight";
            nextTile = nextbTile; //Straight tracks only have straight tiles. Eazy to predict

            newch.chrs.Add(ss);
            CreateSegment(pp, tracklength2, newch, playerArray, playerDirection, pTrackLength, false, 0);

            //Update segment counter for collisions
            if (!Collide)
            {
                
                tracklength2++;
            }
            else
            {
                Debug.Log("PWS GENEN1");
                tracklength2--;
            }

            CreateTriggerFirst(pp, tracklength2, playerArray, boundaryTrigger[0], bLine, true, false);
            CreateTriggerEnd(pp, tracklength2, playerArray, boundaryTrigger[1], pTrackLength, playerDirection,newch);

        }

        nextTile = ""; //Restoring nextTile. No tiles after the end of the track
        tracklength2 = 0;
        if (delete)
        {
            newch = new Indi(0);
        }

    }


    /*CreateNoStraight
    Create Boundary Track with turns only
    */
    private void CreateNoStraight(int pp)
    {
        int zz = 0;

        while (zz < pTrackLength - 1)
        {
            zz = tracklength2;
            string ss = "";
            if (zz == 0)
            {
                ss = "straight";
                nextbTile = "turnL";
            }
            else
            {
                if (zz % 4 == 0)
                {
                    ss = "turnR";
                    nextbTile = "turnL";

                }
                else if (zz % 4 == 1)
                {
                    ss = "turnL";
                   
                }
                else if (zz % 4 == 2)
                {
                    ss = "turnL";
                    nextbTile = "turnR";

                }
                else if (zz % 4 == 3)
                {
                    ss = "turnR";
                }
            }
            //Update next tiles
            nextTile = nextbTile;
            newch.chrs.Add(ss);

            CreateSegment(pp, tracklength2, newch, playerArray2, playerDirection2, pTrackLength, false,0);
            if (!Collide)
            {

                tracklength2++;
            }
            else
            {
                Debug.Log("PWS GENE2");
                tracklength2--;
            }

            CreateTriggerFirst(pp, tracklength2, playerArray2, boundaryTrigger[0], bLine, true, false);
            CreateTriggerEnd(pp, tracklength2, playerArray2, boundaryTrigger[1], pTrackLength, playerDirection2,newch);

        }


        tracklength2 = 0;
        newch = new Indi(0);

    }


    /*DestroyPlayerTrack
     Destroy player track
    Is called when player finishes track. After that new tiles are created from RH algorithm.
     */
    private void DestroyPlayerTrack()
    {

        Debug.Log("PCount" + playerTrack.Count);

        int zz = playerTrack[0].CHROMOSOME.Count - 2;
        playerArray[5, 0] = playerTrack[0].CHROMOSOME[zz + 1].transform.position.x;
        playerArray[5, 1] = 0.0f;
        playerArray[5, 2] = playerTrack[0].CHROMOSOME[zz + 1].transform.position.z;

        playerDirection[0, 5] = playerDirection[pTrackLength - 1, 5];

        //Debug.Log(playerDirection[0, 5] + "::::");
        //Adjust player size so that new player tracks can go along
        if (playerTrack[0].CHROMOSOME[zz + 1].CompareTag("straight"))
        {
            if (playerDirection[0, 5] == "straight")
            {
                playerArray[5, 1] += boxSize[0].y;   //Auksanoume to segment distance tou plithismou
                playerArray[5, 2] += (6.55f + 0.001f);
            }
            else if (playerDirection[0, 5] == "back")
            {
                playerArray[5, 1] += boxSize[0].y;   //Auksanoume to segment distance tou plithismou
                playerArray[5, 2] -= (6.55f + 0.001f);
            }
            else if (playerDirection[0, 5] == "right")
            {
                playerArray[5, 1] += boxSize[0].y;   //Auksanoume to segment distance tou plithismou
                playerArray[5, 0] += (6.55f + 0.001f);
            }
            else if (playerDirection[0, 5] == "left")
            {
                playerArray[5, 1] += boxSize[0].y;   //Auksanoume to segment distance tou plithismou
                playerArray[5, 0] -= (6.55f + 0.001f);
            }
        }
        if (playerTrack[0].CHROMOSOME[zz + 1].CompareTag("turnL"))
        {
            if (playerDirection[0, 5] == "straight")
            {

                playerArray[5, 1] += boxSize[2].y;   //Auksanoume to segment distance tou plithismou
                playerArray[5, 2] += (8.35f + 0.001f);

            }
            else if (playerDirection[0, 5] == "back")
            {

                playerArray[5, 1] += boxSize[2].y;   //Auksanoume to segment distance tou plithismou
                playerArray[5, 2] -= 8.35f + 0.001f;

            }
            else if (playerDirection[0, 5] == "right")
            {

                playerArray[5, 1] += boxSize[2].y;   //Auksanoume to segment distance tou plithismou
                playerArray[5, 0] += 8.35f + 0.001f;
            }
            else if (playerDirection[0, 5] == "left")
            {

                playerArray[5, 1] += boxSize[2].y;   //Auksanoume to segment distance tou plithismou
                playerArray[5, 0] -= (8.35f + 0.001f);

            }
        }
        else if (playerTrack[0].CHROMOSOME[zz + 1].CompareTag("turnR"))
        {
            if (playerDirection[0, 5] == "straight")
            {
                playerArray[5, 1] += boxSize[1].y;   //Auksanoume to segment distance tou plithismou
                playerArray[5, 2] += (8.35f + 0.001f);


            }
            else if (playerDirection[0, 5] == "back")
            {

                playerArray[5, 1] += boxSize[1].y;   //Auksanoume to segment distance tou plithismou
                playerArray[5, 2] -= (8.35f + 0.001f);

            }
            else if (playerDirection[0, 5] == "right")
            {

                playerArray[5, 1] += boxSize[1].y;   //Auksanoume to segment distance tou plithismou
                playerArray[5, 0] += (8.35f + 0.001f);
            }
            else if (playerDirection[0, 5] == "left")
            {

                playerArray[5, 1] += boxSize[1].y;   //Auksanoume to segment distance tou plithismou
                playerArray[5, 0] -= (8.35f + 0.001f);

            }
        }

        playerTrack[0].CHROMOSOME[zz + 1].tag = "deleted";
        deleted = playerTrack[0].CHROMOSOME[zz + 1];

        GameObject[] pp = GameObject.FindGameObjectsWithTag("playerline");

        if (pp.Length>1)
        {
            Destroy(pp[1]);
        }
        
        //Delete player track tiles except the last one
        for (int i = zz; i > -1; i--)
        {
            Destroy(playerTrack[0].CHROMOSOME[i]);
        }
        playerTrack.RemoveAt(0);

        build = false;
        playerfinish = false;

    }


    /*PlayerTrack
     Track is created 5 tiles at a time
    Track is constisted of 60 tiles
     */

    private void PlayerTrack()
    {
        string ss;
        int zz = 0;

        while (zz % 5 != 4)
        {
            zz = tracklength;
            
            
            if (tracklength == 0)
            {
                ss = "straight";
            }
            else
            {

                ss = nextpTile;

            }

            
            //Update next tiles from mutation

            if(ftime == true)
            {
                nextpTile = Mutated_genes();
            }
            else
            {
                if (tracklength < pTrackLength - 1)
                {
                    nextpTile = nextTrack[tracklength + 1];
                }
                
            }

            nextTile = nextpTile;
            chromomm.chrs.Add(ss);

            CreateSegment(5, tracklength, chromomm, playerArray, playerDirection, pTrackLength, true,6);
            //
            //Update segment counter for collisions
            if (!Collide)
            {


                tracklength++;
                
            }
            else
            {

                tracklength--;
                //Debug.Log("HERE" + tracklength);
            }
            CreateTriggerFirst(5, tracklength, playerArray, null, playerLine, false, ftime);
            CreateTriggerEnd(5, tracklength, playerArray, EvolveT, pTrackLength, playerDirection,chromomm);

        }
        nextTile = "";
        if (tracklength == pTrackLength)
        {


            playerTrack.Add(chromomm);

            for (int pp = 0; pp < pTrackLength; pp++)
            {
                if (playerTrack[0].CHROMOSOME[pp].tag != "straight")
                {
                    playerTrack[0].METRIC++;
                }
            }
            metrioc = playerTrack[0].metric;
            exw = metrioc;
            Debug.Log("pm" + exw);


            tracklength = 0;
            chromomm = new Indi(0);
            build = true;
            ftime = false;
            if (ftime == false)
            {
                nextTrack = new List<string>();
            }
            
        }

    }




    void Update()
    {
        if (level == 20)
        {
            endofEvolution = true;
        }

        if (!endofEvolution)
        {
            if (!destroyed)
            {   /*Tracks not constructed*/

                Simfinished = true;
                /* Check if all cars end tracks*/

                if (GameObject.FindGameObjectsWithTag("Agent").Length > 0)
                {
                    Simfinished = false;

                }

                if (!Simfinished)
                { /* Check if all cars passed checkpoints*/
                    if (GameObject.FindGameObjectsWithTag("checkpoint").Length == 0)
                    {
                        checkReached = true;
                    }
                    else
                    {
                        checkReached = false;
                    }
                }

                //Debug.Log("checkReached" + checkReached + "count" + count);
                if (checkReached && count == 0)
                { /*Check if fitness has passed to the population for the 10 tiles and initialize variables for starting evolution of the tracks for the 10 tiles*/
                    new_generation2 = new List<Indi>();

                    for (int i = 0; i < population.Count; i++)
                    {
                        population[i].FITNESS = timeLapsed[i, 0];
                        //Debug.Log("Check:" + evolutioncount + "Ti" + i + ":" + population[i].FITNESS);
                    }
                    population.Sort(delegate (Indi x, Indi y) { return x.FITNESS.CompareTo(y.FITNESS); });
                    count++; //We want only one time to assign fitness.
                    chromo = new Indi(0);

                }
                if (checkReached && !cpEvolved)
                { /* Create first 10 tiles for every child*/
                    CheckpointEvolved();

                }

                if (Simfinished && cpEvolved && !eval)
                {
                    /*Car Agents have finished tracks--Time for fitness*/
                    //Debug.Log("Start Fitness" + evolutioncount);

                    for (int i = 0; i < POPULATION_SIZE; i++)
                    {
                        population[i].FITNESS = timeLapsed[i, 1];
                        //Debug.Log("EvolutionCOunt:" + evolutioncount + "Ti" + i + ":" + population[i].FITNESS);
                    }

                    population.Sort(delegate (Indi x, Indi y) { return x.FITNESS.CompareTo(y.FITNESS); });
                    eval = true;


                    //Prepare variables for constructing complete children
                    new_generation = new List<Indi>();
                    new_generation3 = new List<Indi>();

                    for (int i = 0; i < s; i++)
                    {
                        List<string> chromos = population[i].chrs;
                        Indi chro = new Indi(0);
                        chro.CHRS = chromos;
                        chro.FITNESS = population[i].FITNESS;
                        chro.METRIC = population[i].METRIC;

                        new_generation.Add(chro);

                    }
                    for(int i = 0; i < populationCount; i++)
                    {
                        Debug.Log("c" + i + ":" + population[i].METRIC + ":evolve-"+evolutioncount);
                    }
                    //Calculate fitness variables
                    avgLapsed /= POPULATION_SIZE;
                    maxLapsed = population[POPULATION_SIZE - 1].FITNESS;
                    minLapsed = population[0].FITNESS;
              //      Debug.Log("Avg" + avgLapsed);
               //     Debug.Log("Max" + maxLapsed);
                //    Debug.Log("Min" + minLapsed);
                    avgWritten = true;

                    PoolInsertion1(); //Insert population to the pool

                    DestroyPopulation();
                    //Debug.Log("P Count:" + pool.Count);
                    Debug.Log("Pop Destroyed");

                    chromo = new_generation2[0];
                }
                if (eval && !endEvolved)
                {
                    //Finish evolution for full tracks
                    EndEvolve();
                }
                if (endEvolved && !eveval)
                {
                    //Play evolved children to evaluate fitness to these tracks
                    eSimEnd = true;
                    if (GameObject.FindGameObjectsWithTag("Agent").Length > 0)
                    {
                        eSimEnd = false;
                    }
                    if (eSimEnd)
                    {
                        //Assign Fitness to evolved tracks
                        //Debug.Log("Start Fitness2" + evolutioncount);
                        for (int i = 0; i < POPULATION_SIZE; i++)
                        {
                            new_generation3[i].FITNESS = evolveLapsed[i];
                            //Debug.Log("EvolvedCount:" + evolutioncount + "Ti" + i + ":" + new_generation3[i].FITNESS);
                        }
                        eveval = true;
                        evolutioncount++;
                    }
                }
                if (eveval)
                {
                    /*Insert population and children to pool and clear field from these tracks*/
                    PoolInsertion2(); //Insert children to the pool
                    //Debug.Log("Pool C:" + pool.Count);
                    DestroyChildren();
                    Debug.Log("Children Destroyed");
                    ResetTimers(); //Reset Timers for next population
                    count = 0;
                }
            }
            else
            {
                /*Create Population from tracks*/
                CreatePop();
            }

            //Create Track for player
            if (!build)
            {
                PlayerTrack();

                

                if (build)
                {
                    DestroyPlayerTile(); //Destroy last tile of the last circuit
                                         //nextTrack = new List<string>();
                }
                if (!isBound)
                {
                    /*Create boundary tracks for time limits --- 3 straights/3 snakes with their car agents */
                    GameObject[] ss = GameObject.FindGameObjectsWithTag("boundline");
                    for (int i = 1; i < ss.Length; i++)
                    {
                        GameObject cars = Instantiate(player[(i - 1) % 5], new Vector3(ss[i].transform.position.x, ss[i].transform.position.y, ss[i].transform.position.z - 5.0f), player[(i - 1) % 5].transform.rotation);
                        cars.SetActive(true);
                        //Debug.Log("CNAME"+cars.name);
                        cars.tag = "BAgent";
                    }
                    isBound = true;
                }
                
            }//If player Track is created then check if it is finished so next tiles be created.
            if (build)
            {
                if (playerfinish)
                {
                    DestroyPlayerTrack();
                }

            }
            if (endBound && !done)
            {
                float min, max;
                min = boundaryLapsed[0];
                max = boundaryLapsed[0];
                for (int i = 1; i < 5; i++)
                {
                    min = Mathf.Min(min, boundaryLapsed[i]);
                    max = Mathf.Max(max, boundaryLapsed[i]);
                }
                bounds0[0] = min;
                bounds0[1] = max;
                Debug.Log("0:::::"+bounds0[0] + ":::::1:" + bounds0[1]);

                min = boundaryLapsed[5];
                max = boundaryLapsed[5];
                for (int i = 6; i < 10; i++)
                {
                    min = Mathf.Min(min, boundaryLapsed[i]);
                    max = Mathf.Max(max, boundaryLapsed[i]);
                }
                bounds60[0] = min;
                bounds60[1] = max;


                Debug.Log("0:::::"+bounds60[0] + ":::::1:" + bounds60[1]);
                done = true;

                //Debug.Log("flow:"+flow(0));
                //Debug.Log("flow:" + flow(30));
                //Debug.Log("flow:" + flow(60));
                float ff = Flow(0);
                //Debug.Log("ff" + 1+":"+ff);
               //     Debug.Log("flowpos" + flowpos(ff));



                 ff = Flow(30);
                //Debug.Log("ff" + 2 + ":" + ff);
                //Debug.Log("flowpos" + flowpos(ff));
                 ff = Flow(60);
                //Debug.Log("ff" + 3 + ":" + ff);
                //Debug.Log("flowpos" + flowpos(ff));
                
            }
        }

    }
    public static float Flow(int x)
    {
        float ff = ((bounds60[1] - bounds0[0])/pTrackLength)*x + bounds0[0];
        return ff;
    }

    public static int Flowpos(float x)
    {
        int ff = Mathf.RoundToInt((x - bounds0[0])/ ((bounds60[1] - bounds0[0]) / pTrackLength));
        if (ff > 60)
        {
            return 60;
        }
        if (ff < 0)
        {
            return 0;
        }
        return ff;
    }


    /*DestroyPlayerTile
     Destroy the last tile of the last player circuit
    It is used to keep car sensors updated -- Else Car not respondedmUTA
     */
    private void DestroyPlayerTile()
    {
        GameObject[] zz = GameObject.FindGameObjectsWithTag("deleted");
        Debug.Log("zzz" + zz.Length);
        if (zz.Length > 0)
        {
            for (int i = 0; i < zz.Length - 1; i++)
            {
                Destroy(zz[i]);
            }
        }
    }



    /*Rank Selection
     Genetic Operator for parent selection*/
    private int RWS()
    {
        int selection = -1;
        cdf = 0;
        /*Number is selected from the first half of population
         SUM(i) from 0 to n=n(n+1)/2
        where i = population size/2
        to create the SUM OF X from i to n
         */
        int formula = ((population.Count / 2) * ((population.Count / 2) + 1)) / 2; 
        int r = Random.Range(0, (formula + 1));

        //Choose a number from space[(0 population_size/2]

        int j = population.Count / 2;

        /*Find the space that correspond to the number
         Higher priority --> Bigger space
        Priority from 0 to population size/2
        exmpl
        0 number -> 10 priority
        0 number -> [0 1 2 3 4 5 6 7 8 9] space
        for population size 10
         */
        

        while (j > 0)
        {
            cdf += j;

            if (r <= cdf)
            {
                // Debug.Log("RSW"+ (population.Count / 2 - j));
                selection = (population.Count / 2 - j);
                break;
            }
            j--;
        }
        return selection;
    }

    /*PoolInsertion
     Is separated to
    PoolInsertion 1 & 2
     */

    /*PoolInsertion2
     Children tracks are added to the pool*/
    private void PoolInsertion2()
    {

        for (int i = 0; i < new_generation3.Count; i++)
        {
            List<string> chromos = new_generation3[i].chrs;
            Indi chro = new Indi(0);
            chro.CHRS = chromos;
            chro.FITNESS = new_generation3[i].FITNESS;
            chro.METRIC= new_generation3[i].METRIC;
            pool.Add(chro);
        }
        //Sort after insertion of population and evolves
        pool.Sort(delegate (Indi x, Indi y) { return x.FITNESS.CompareTo(y.FITNESS); });

    }

    /*PoolInsertion1
    Parent tracks are added to the pool*/
    private void PoolInsertion1()
    {
        /* Pool is built from (s*population && (POPULATION_SIZE-s)*children)*/
        for (int i = 0; i < population.Count; i++)
        {
            List<string> chromos = population[i].chrs;
            Indi chro = new Indi(0);
            chro.CHRS = chromos;
            chro.FITNESS = population[i].FITNESS;
            chro.METRIC = population[i].METRIC;
            pool.Add(chro);
            
            
        }
    }


    public static int Select(int m,int ppp)
    {
        int k = m + 1;
        int[] kk = new int[k]; //k tournament select
                               //
                               ////on
        int[] selection = new int[k];
        int formula = POPULATION_SIZE * 2;
        int minn;
        int min = POPULATION_SIZE * 2 + 1;
        int minind = 0;
        int mini = 0;

        if(ppp< pTrackLength / 2)
        {
            for (int i = 0; i < k; i++)
            {
                //Debug.Log("Min Tour" + i);
                minn = pTrackLength + 1;
                minind = pTrackLength + 1;
                for (int j = 0; j < kk.Length; j++)
                {
                    kk[j] = Random.Range(0, (formula));
                    // Debug.Log("kk"+kk[j]+":"+pool[kk[j]].METRIC);
                    if (minn > pool[kk[j]].METRIC)
                    {
                        minn = pool[kk[j]].METRIC;

                        //if (kk[j] < minind)
                        //{
                        minind = kk[j];
                        //}

                    }
                    if (minn == pool[kk[j]].METRIC)
                    {
                        if (kk[j] < minind)
                        {
                            minind = kk[j];
                        }
                    }



                    //Debug.Log(i + ":" + "kk" + j + ":" + kk[j]);

                }


                //Debug.Log("minn" + minn);
                selection[i] = minn;
                if (min > selection[i])
                {
                    min = selection[i];
                    mini = minind;

                }

                //min = Mathf.Min(selection[i], min);
                //Debug.Log("ind" + mini + " : vv" + min);
            }


        }
        else if(ppp > pTrackLength / 2)
        {
            for (int i = 0; i < k; i++)
            {
                Debug.Log("Max Tour" + i);
                minn=0;
                minind = 0;
                for (int j = 0; j < kk.Length; j++)
                {
                    kk[j] = Random.Range(0, (formula));
                    Debug.Log("kk"+kk[j]+":"+pool[kk[j]].METRIC);
                    if (minn < pool[kk[j]].METRIC)
                    {
                        minn = pool[kk[j]].METRIC;

                        //if (kk[j] < minind)
                        //{
                        minind = kk[j];
                        //}

                    }
                    if (minn == pool[kk[j]].METRIC)
                    {
                        if (kk[j] < minind)
                        {
                            minind = kk[j];
                        }
                    }



                    //Debug.Log(i + ":" + "kk" + j + ":" + kk[j]);

                }


                //Debug.Log("minn" + minn);
                selection[i] = minn;
                if (min < selection[i])
                {
                    min = selection[i];
                    mini = minind;

                }

                //min = Mathf.Min(selection[i], min);
                Debug.Log("ind" + mini + " : vv" + min);
            }
        }
      

        return mini;
    }



    public static void ChoosePool()
    {


        int zz = 0; //count
        int i = 0;//index
        int m = pTrackLength / Indi.SEQUENCE_LENGTH;

        int min = pTrackLength + 1;
        bool isExit=false;
        //Debug.Log("Start Choose Pool");
        if (endBound && (pool.Count > POPULATION_SIZE))
        {

            Debug.Log("Apo pool"); 
            //Debug.Log("i" + i);
            Debug.Log("Exw" + exw);
            //Debug.Log("Prohgoumeno Lap prevlap"+ LapComplete.prevLap);
            Debug.Log("Prohgoumeno Lap ttimer" + LapComplete.ttimer);

            eprepe = Flowpos(LapComplete.prevLap);
            Debug.Log("Eprepe" + eprepe);
            //Debug.Log("Difference" + (exw - eprepe));
            exw = 0;


            do
            {
                //min = MTournamentSelect(m);
                if (!isExit)
                {

                    min = Select(m, eprepe);
                    Debug.Log("Apo select"+min + "me"+ pool[min].METRIC);
                }
                else
                {
                    min = 0;
                    Debug.Log("Oxi Apo select" + min);
                }

                
                //Debug.Log("::::::"+min+ "::::::"+pool[min].metric + "isExit"+ isExit);
                //ccc += pool[min].metric;
                for (int z = 0; z <  Indi.SEQUENCE_LENGTH; z++)
                {
                 
                    if (exw <= eprepe + 3)
                    {
                        nextTrack.Add(pool[min].CHRS[z]);
                    }
                    else
                    {
                        nextTrack.Add("straight");
                    }
                    
                    zz++;

                    if (nextTrack[nextTrack.Count-1] != "straight")
                    {   
                        exw++;
                    }

                    if (exw > eprepe + 1)
                    {
                        isExit = true;
                    }
                       //break;
                }

                

                i++;


                //Debug.Log("min:" + min);
                //Debug.Log("zz" + zz);
            } while (zz != pTrackLength);

          
            //if (pp < Indi.mutation)
           // {
                Indi.ScrableShuffle(nextTrack);
            //}
            

            Debug.Log("New tr c" + nextTrack.Count);
        }
        else
        {


            Debug.Log("Αpo Mut");


            for (int z = 0; z < pTrackLength; z++)
            {
                nextTrack.Add(Mutated_genes());
                    
            }


        }
        //      Debug.Log("cccc"+ cccc);

        //pool = new List<Indi>();


        Debug.Log("New Track:" + exw + " \tLEVEL:" + level);
    }



    /*Evolution for the 11-20 tiles based on Finish Time of the track*/
    private void EndEvolve()
    {
        /*Steps
         1)Construct the first 10 tiles for every children
         2)Choose parents
         3)Based on parents fitness construct tiles through crossover
         4)After constructing full track then add to the evolved population (new_generation3)*/

        //Choose parents for crossover - We wont want to choose every time parents . We only care to find them once


        //Construct the first 10 tiles and additional components(triggers)

        //Create full children through crossover

        if (segmentCount < Indi.SEQUENCE_LENGTH)
        {
            //Debug.Log("pop C" + populationCount);
            if (sttime)
            {
                int index = RWS();
                //len = population.Count;
                r = index;
                //Debug.Log("P1:" + r);
                //Random.Range(0, len / 2);
                parent1 = population[r];
                index = RWS();
                r = index;
                //Random.Range(0, len / 2);
                parent2 = population[r];
                eArray[populationCount, 0] = specArray[populationCount, 0];
                eArray[populationCount, 1] = specArray[populationCount, 1];
                eArray[populationCount, 2] = specArray[populationCount, 2];
                direction[segmentCount - 1, populationCount] = specDirection[populationCount];
                //Debug.Log("pop" + populationCount + "e10.0" + eArray[populationCount, 0] + "e10.1" + eArray[populationCount, 1] + "e10.2" + eArray[populationCount, 2] + "direction10:" + direction[segmentCount - 1, populationCount]);
                sttime = false;




            }

            //Update next tiles from mating
            string ss;
            if (segmentCount == (Indi.SEQUENCE_LENGTH / 2))
            {
                ss = parent1.Crossover(parent1, segmentCount);
            }
            else
            {
                ss = nextcsTile;
            }

            if (segmentCount < (Indi.SEQUENCE_LENGTH - 1))
            {
                nextcsTile = parent1.Crossover(parent1, segmentCount + 1);
                nextTile = nextcsTile;
            }






            chromo.chrs.Add(ss);
            CreateSegment(populationCount, segmentCount, chromo, eArray, direction, Indi.SEQUENCE_LENGTH, false,5);
            
            //Update segment counter for collisions
            if (!Collide)
            {

                segmentCount++;
            }
            else
            {

                //if (segmentCount == Indi.SEQUENCE_LENGTH / 2) { Debug.Log("Collide" + populationCount + ":::" + segmentCount); }
                
                //Debug.Log("STO EVOLVE LATHOS");
                segmentCount--;
            }
            CreateTriggerEnd(populationCount, segmentCount, eArray, evolveTrigger[1], Indi.SEQUENCE_LENGTH, direction, chromo);

        }
        else
        {
            /*Complete all tracks means that tracks can be evaluated so CarAgents are provided to the tracks*/
            //Debug.Log("CHROMO L"+chromo.CHROMOSOME.Count+"i"+populationCount);

            for (int kk = 0; kk < segmentCount; kk++)
            {
                if (chromo.CHROMOSOME[kk].tag != "straight")
                {
                    chromo.metric++;
                }
            }

            //Debug.Log("c" + populationCount+ ":" + chromo.METRIC + ":evolve-" + evolutioncount + "newpop");
            


            new_generation3.Add(chromo);
            nextTile = "";
            segmentCount = Indi.SEQUENCE_LENGTH / 2;
            populationCount++;
            if (populationCount != POPULATION_SIZE)
            {
                chromo = new_generation2[populationCount];
            }
            sttime = true;


            if (populationCount == POPULATION_SIZE)
            {
                //Debug.Log("Tha teleiwsei to evolve edw");
                endEvolved = true;
                segmentCount = 0;
                populationCount = 0;
                CarAgent.canMoved = false;
                GameObject[] startlines = GameObject.FindGameObjectsWithTag("evolveline");
                int count = 0;
                foreach (GameObject _go in startlines)
                {
                    if (count == 0)
                    {
                        count++;
                        continue;
                    }

                    GameObject cars = Instantiate(driver, new Vector3(_go.transform.position.x, _go.transform.position.y, _go.transform.position.z - 2.0f), driver.transform.rotation);
                    cars.SetActive(true);
                }
                CarAgent.canMoved = true;

            }

        }
    }


    

    /*NewPop
     Create next generations for tracks*/

    private void NewPop()
    {
        /*Steps
         1)Elitism
         2)Complete new population from pool
         3)Complete Constructions*/

        string ss;
        int zz;
        do
        {
            zz = segmentCount;
            if (segmentCount == 0)
            {
                ss = "straight";
            }
            else
            {
                ss = nexteTile;
            }
            //Update next tile from evolution from last population and pool

            if (populationCount < s)
            {//Step 1
                if (segmentCount < (Indi.SEQUENCE_LENGTH - 1))
                {
                    nexteTile = new_generation[populationCount].chrs[segmentCount + 1];
                }
            }
            else
            {//Step 2
                int z = populationCount - s;
                //Debug.Log("z" + z + "evolutioncount" + evolutioncount + "segmentcount"+segmentCount);
                if (segmentCount < (Indi.SEQUENCE_LENGTH - 1))
                {
                    nexteTile = pool[z].chrs[segmentCount + 1];
                }

            }

            nextTile = nexteTile;
            chromo.chrs.Add(ss);

            CreateSegment(populationCount, segmentCount, chromo, pArray, direction, Indi.SEQUENCE_LENGTH, false,0);
            //Update segment counter for collisions
            if (!Collide)
            {
                segmentCount++;
            }
            else
            {
                /*Dont expect collision because tracks have previously been created in last population*/
                Debug.Log("EDW DEN PREPEI");
            }

            CreateTriggerFirst(populationCount, segmentCount, pArray, SegmentTrigger[0], StartLine, true, false);
            CreateTriggerEnd(populationCount, segmentCount, pArray, SegmentTrigger[1], Indi.SEQUENCE_LENGTH, direction, chromo);

        } while (zz % 5 == 0);

        nextTile = "";

        if (segmentCount == Indi.SEQUENCE_LENGTH)
        {
            
            //Debug.Log(populationCount+":---"+chromo.metric);
            CreateCheck(chromo, populationCount);
            for (int kk = 0; kk < segmentCount; kk++)
            {
                if (chromo.CHROMOSOME[kk].tag != "straight")
                {
                    chromo.metric++;
                }
            }

            //Debug.Log("chromo" + populationCount + ": metric-" + chromo.metric+"poop");
            population.Add(chromo);

            segmentCount = 0;
            chromo = new Indi(0);
            populationCount++;
        }
        if (populationCount == POPULATION_SIZE)
        {
            destroyed = false;
            CarAgent.canMoved = false;

            GameObject[] startlines = GameObject.FindGameObjectsWithTag("startline");
            int countz = 0;
            foreach (GameObject _go in startlines)
            {
                if (countz == 0)
                {
                    countz++;
                    continue;
                }

                GameObject cars = Instantiate(driver, new Vector3(_go.transform.position.x, _go.transform.position.y, _go.transform.position.z - 2.0f), driver.transform.rotation);
                cars.SetActive(true);
            }
            CarAgent.canMoved = true;
            populationCount = 0;
            segmentCount = 0;

        }

    }


    /*CreatePop
     Create population of tracks*/
    private void CreatePop()
    {
        /*First population is selected randomly
         Create track by constructing tiles one by one. And track after track*/
        if (!firstPop)
        {
            int zz;
            string ss;
            do
            {
                zz = segmentCount;
                if (segmentCount == 0)
                {
                    ss = "straight";
                    
                }
                else
                {
                    ss = nextcTile;
                }
                //Update next tile from mutation
                nextcTile = Mutated_genes();
                nextTile = nextcTile;

                chromo.chrs.Add(ss);

                CreateSegment(populationCount, segmentCount, chromo, pArray, direction, Indi.SEQUENCE_LENGTH, false,0);
                //Update segment counter for collisions
                if (!Collide)
                {

                    segmentCount++;
                }
                else
                {
                    segmentCount--;
                }

                CreateTriggerFirst(populationCount, segmentCount, pArray, SegmentTrigger[0], StartLine, true, false);
                CreateTriggerEnd(populationCount, segmentCount, pArray, SegmentTrigger[1], Indi.SEQUENCE_LENGTH, direction, chromo);

            } while (zz % 5 == 0);

            nextTile = "";

            if (segmentCount == Indi.SEQUENCE_LENGTH)
            {
                for(int kk = 0; kk < segmentCount; kk++)
                {
                    if (!chromo.CHROMOSOME[kk].CompareTag("straight"))
                    {
                        chromo.metric++;
                    }
                }
                CreateCheck(chromo, populationCount);
                population.Add(chromo);
                segmentCount = 0;
                chromo = new Indi(0);
                populationCount++;
            }
            if (populationCount == POPULATION_SIZE)
            {
                destroyed = false;
                CarAgent.canMoved = false;
                GameObject[] startlines = GameObject.FindGameObjectsWithTag("startline");
                int count = 0;
                foreach (GameObject _go in startlines)
                {
                    if (count == 0)
                    {
                        count++;
                        continue;
                    }

                    GameObject cars = Instantiate(driver, new Vector3(_go.transform.position.x, _go.transform.position.y, _go.transform.position.z - 2.0f), driver.transform.rotation);
                    cars.SetActive(true);
                }
                CarAgent.canMoved = true;
                firstPop = true;
                populationCount = 0;
                segmentCount = 0;
                
            }
        }
        else
        {
            NewPop();
        }
    }

    /*CheckpointEvolved
     Evolve until checkpoints*/

    private void CheckpointEvolved()
    {
        /*Steps
         1)Crossover 10 first segments through parent selection
         2)Construct them so that we can know if they collide one another
         3)Destroy tracks after crossover finish and initialize variables
         4)Repeat the first three steps for all tracks*/

        if (segmentCount == 0)
        {
            int index = RWS();
            //len = population.Count;
            r = index;
            //Random.Range(0, len / 2);
            //Debug.Log("P1:" + r);

            parent1 = population[r];
            index = RWS();
            r = index;
            //Random.Range(0, len / 2);
            parent2 = population[r];

        }


        //Step 1 and 2
        if (segmentCount < Indi.SEQUENCE_LENGTH / 2)
        {
            string ss;
            if (segmentCount == 0)
            {
                ss = "straight";
            }
            else
            {
                ss = nextcsTile;
            }
            //Update next tile from mating

            if (segmentCount < (Indi.SEQUENCE_LENGTH / 2) - 1)
            {
                nextcsTile = parent1.Crossover(parent1, segmentCount + 1);
                nextTile = nextcsTile;
            }

            chromo.chrs.Add(ss);
            CreateSegment(populationCount, segmentCount, chromo, eArray, direction, Indi.SEQUENCE_LENGTH, false,5);
            //Update segment counter for collisions
            if (!Collide)
            {
                segmentCount++;
            }
            else
            {
                segmentCount--;
            }


            CreateTriggerFirst(populationCount, segmentCount, eArray, evolveTrigger[0], evolveLine, true, false);
            CreateTriggerEnd(populationCount, segmentCount, eArray, evolveTrigger[1], Indi.SEQUENCE_LENGTH, direction, chromo);
        }
        //Step 3
        if (segmentCount == Indi.SEQUENCE_LENGTH / 2)
        {
            nextTile = "";
            specArray[populationCount, 0] = eArray[populationCount, 0];
            specArray[populationCount, 1] = eArray[populationCount, 1];
            specArray[populationCount, 2] = eArray[populationCount, 2];
            specDirection[populationCount] = direction[segmentCount - 1, populationCount];
            //Debug.Log(specDirection[populationCount] + "::::" + populationCount);
            //Debug.Log("pop" + populationCount + "spec0" + specArray[populationCount, 0] + "spec1" + specArray[populationCount, 1] + "spec2" + specArray[populationCount, 2]+"sDir:"+specDirection[populationCount]);
            populationCount++;
            segmentCount = 0;
            new_generation2.Add(chromo);


            eArray[populationCount - 1, 1] = eValue[1];

            if (populationCount - 1 == 0)
            {
                eArray[populationCount - 1, 0] = eValue[0];
                eArray[populationCount - 1, 2] = firstevolvedGap;
            }
            else if ((populationCount - 1) % trackGap == 0)
            {
                eArray[populationCount - 1, 0] = 0.0f;
                eArray[populationCount - 1, 2] = eArray[populationCount - 2, 2] + otherevolvedGap;
            }
            else
            {
                eArray[populationCount - 1, 0] = eArray[populationCount - 2, 0] + popGap;
                eArray[populationCount - 1, 2] = eArray[populationCount - 2, 2];
            }
            chromo = new Indi(0);
        }

        //10 tiles have been created for every track
        if (populationCount == POPULATION_SIZE)
        {
            cpEvolved = true;
            //Debug.Log("Evolve1 End");
            //Debug.Log("pop" + populationCount + "-" + evolutioncount);
            populationCount = 0;
            segmentCount = Indi.SEQUENCE_LENGTH / 2;
        }
    }

    /*Initialize
     Initialize value variables
    */

    private void Initialize()
    {
        //Initialize directios, dimensions,sizes,simulation variables,timers etc.
        POPULATION_SIZE = 25;
        trackGap = 25;

        boxSize = new Vector3[segments.Length];

        for (int i = 0; i < segments.Length; i++)
        {
            boxSize[i] = segments[i].GetComponent<Collider>().bounds.size;
        }
        pValue = new float[3];

        eValue = new float[3];

        pValue[0] = 0.0f;
        pValue[1] = 0.0f;
        pValue[2] = 0.0f;
        eValue[0] = 0.0f;
        eValue[1] = 0.0f;
        eValue[2] = 0.0f;
        pArray = new float[POPULATION_SIZE, 3];
        eArray = new float[POPULATION_SIZE, 3];
        specArray = new float[POPULATION_SIZE, 3];


        firstevolvedGap = -1000.0f;
        popGap = 2 * Indi.SEQUENCE_LENGTH * 10.0f;
        otherevolvedGap = -popGap;

        for (int k = 0; k < POPULATION_SIZE; k++)
        {
            eArray[k, 1] = eValue[1];
            pArray[k, 1] = pValue[1];

            if (k == 0)
            {
                pArray[k, 0] = pValue[0];
                pArray[k, 2] = pValue[2];

                eArray[k, 0] = eValue[0];
                eArray[k, 2] = firstevolvedGap;
            }
            else if (k % trackGap == 0)
            {
                pArray[k, 2] = pArray[k - 1, 2] + popGap;
                pArray[k, 0] = 0.0f;

                eArray[k, 0] = 0.0f;
                eArray[k, 2] = eArray[k - 1, 2] + otherevolvedGap;
            }
            else
            {
                pArray[k, 0] = pArray[k - 1, 0] + popGap;
                pArray[k, 2] = pArray[k - 1, 2];

                eArray[k, 0] = eArray[k - 1, 0] + popGap;
                eArray[k, 2] = eArray[k - 1, 2];
            }


        }

        direction = new string[Indi.SEQUENCE_LENGTH, POPULATION_SIZE];
        specDirection = new string[POPULATION_SIZE];

        startt = new bool[POPULATION_SIZE];
        endt = new bool[POPULATION_SIZE];
        timeLapsed = new float[POPULATION_SIZE, 2];
        population = new List<Indi>(POPULATION_SIZE);

        evolveendt = new bool[POPULATION_SIZE];
        evolvestartt = new bool[POPULATION_SIZE];
        evolveLapsed = new float[POPULATION_SIZE];


        Collide = false;
        endofEvolution = false;
        evolutioncount = 0;
        firstPop = false;

        cpEvolved = false;
        endEvolved = false;
        eSimEnd = true;

        for (int i = 0; i < POPULATION_SIZE; i++)
        {
            startt[i] = false;
            endt[i] = false;
            evolvestartt[i] = false;
            evolveendt[i] = false;
            timeLapsed[i, 0] = 0.0f;
            timeLapsed[i, 1] = 0.0f;
            evolveLapsed[i] = 0.0f;
        }
        avgLapsed = 0.0f;

        s = (20 * POPULATION_SIZE) / 100;

        populationCount = 0;
        segmentCount = 0;

        chromo = new Indi(0);
        pool = new List<Indi>();


        pTrackLength = 3*Indi.SEQUENCE_LENGTH;
        playerDirection = new string[pTrackLength, 6];
        playerDirection2 = new string[pTrackLength, 10];
        playerValue = new float[3];

        playerValue[0] = 0.0f;
        playerValue[1] = 0.0f;
        playerValue[2] = -6000.0f;


        playerArray = new float[6, 3];
        playerArray[5, 0] = -15000.0f;
        playerArray[5, 1] = 0.0f;
        playerArray[5, 2] = 0.0f;

        playerArray2 = new float[10, 3];
        playerDirection[0, 5] = "straight";

        bounds0 = new float[2];
        bounds60 = new float[2];
        //Boundary Tracks order
        for (int i = 0; i < 5; i++)
        {
            playerArray[i, 1] = playerValue[1];
            playerArray[i, 2] = playerValue[2];
            playerDirection[0, i] = "straight";
            if (i == 0)
            {

                playerArray[i, 0] = playerValue[0];
            }
            else
            {
                playerArray[i, 0] = playerArray[i - 1, 0] + 500.0f;
            }
        }


        //Boundary Tracks order
        for (int i = 0; i < 10; i++)
        {
            playerArray2[i, 1] = playerValue[1];
            playerArray2[i, 2] = playerValue[2];
            playerDirection2[0, i] = "straight";
            if (i == 0)
            {

                playerArray2[i, 0] = playerValue[0];
            }
            else
            {

                    playerArray2[i, 0] = playerArray2[i - 1, 0] + 500.0f;
                
               
            }

        }


        chromomm = new Indi(0);
        newch = new Indi(0);

        playerTrack = new List<Indi>();
        nextTrack = new List<string>();

        boundarystartt = new bool[10];
        boundaryendt = new bool[10];
        boundaryLapsed = new float[10];
    }


    /*Mutated_genes
     Mutation Operator - Random Resetting (an extension of the bit flip)
     Ask for a random tile
    
     */

    public static string Mutated_genes()
    {
        return seg[Random.Range(0, seg.Length)];
    }

    /*Reset Timers
     Reset timers and track signs for every track*/
    private void ResetTimers()
    {
        timeLapsed = new float[POPULATION_SIZE, 2];
        evolveLapsed = new float[POPULATION_SIZE];
        startt = new bool[POPULATION_SIZE];
        endt = new bool[POPULATION_SIZE];
        evolveendt = new bool[POPULATION_SIZE];
        evolvestartt = new bool[POPULATION_SIZE];
        avgLapsed = 0.0f;
    }

    /*DestroyPopulation
     Clear the field from the tracks(population and evolved children, as well as the additional components of every track
    Initialize variables for the next generation*/

    private void DestroyChildren()
    {
        /*Steps
         1)Lines
         2)Tiles
         */
        //Destroy lines
        lines = GameObject.FindGameObjectsWithTag("evolveline");
        for (int i = 1; i < lines.Length; i++)
        {
            Destroy(lines[i]);
        }

        //Destroy Children tiles
        if (new_generation3.Count > 0)
        {
            for (int k = 0; k < POPULATION_SIZE; k++)
            {
                for (int p = new_generation3[k].CHROMOSOME.Count - 1; p > -1; p--)
                {
                    Destroy(new_generation3[k].CHROMOSOME[p]);
                }
                eArray[k, 1] = eValue[1];
                if (k == 0)
                {
                    eArray[k, 0] = eValue[0];
                    eArray[k, 2] = firstevolvedGap;
                }
                else if (k % trackGap == 0)
                {
                    eArray[k, 0] = 0.0f;
                    eArray[k, 2] = eArray[k - 1, 2] + otherevolvedGap;
                }
                else
                {
                    eArray[k, 0] = eArray[k - 1, 0] + popGap;
                    eArray[k, 2] = eArray[k - 1, 2];
                }
            }
        }

        direction = new string[Indi.SEQUENCE_LENGTH, POPULATION_SIZE];
        destroyed = true;

        endEvolved = false;
        cpEvolved = false;
        checkReached = false;
        endEvolved = false;
        eveval = false;
        eval = false;
        avgWritten = false;
        promo = false;

        population = new List<Indi>();
        chromo = new Indi(0);
    }

    private void DestroyPopulation()
    {
        /*Steps
         1)Destroy tracks and their components*/

        lines = GameObject.FindGameObjectsWithTag("startline");
        for (int i = 1; i < lines.Length; i++)
        {
            Destroy(lines[i]);

        }

        if (population.Count > 0)
        {
            for (int k = 0; k < POPULATION_SIZE; k++)
            {
                //Destroy only objects. Generation will be initialized again (variables next_generation,next_generation2,next_generation3)

                for (int p = population[k].CHROMOSOME.Count - 1; p > -1; p--)
                {
                    Destroy(population[k].CHROMOSOME[p]);

                }

                //Re initialize variables for construction - New tracks / New dimensions
                pArray[k, 1] = pValue[1];

                if (k == 0)
                {
                    pArray[k, 0] = pValue[0];
                    pArray[k, 2] = pValue[2];
                }
                else if (k % trackGap == 0)
                {
                    pArray[k, 2] = pArray[k - 1, 2] + popGap;
                    pArray[k, 0] = 0.0f;
                }
                else
                {
                    pArray[k, 0] = pArray[k - 1, 0] + popGap;
                    pArray[k, 2] = pArray[k - 1, 2];
                }
            }

        }

    }


    /*CollisionCheck
     Check collision between the last tile inserted to the track with the rest tiles of the track
     */
    private void CollisionCheck(List<GameObject> chromo, int l)
    {
        //Intersection between tile size means that two tiles are blocking the road. So the track is unplayable
        Collide = false;

        for (int i = 0; i < l; i++)
        {
            if (chromo[l].GetComponent<Collider>().bounds.Intersects(chromo[i].GetComponent<Collider>().bounds))
            {
                Collide = true;
            }

            //We want to check if "deleted" tile is collided with playertrack
            if (deleted != null)
            {
                if (chromo[l].GetComponent<Collider>().bounds.Intersects(deleted.GetComponent<Collider>().bounds))
                {
                    Collide = true;
                }
            }

            if (Collide)
            {
                break;
            }

        }

    }


    

    /*CreateTriggers
     Construct the necessery components of the track
    1)Triggers
    2)StartLines*/

    private void CreateTriggerFirst(int i, int l, float[,] pArray, GameObject S1, GameObject line, bool ag, bool pdriver)
    {
        //The components are created through the constuction of the track (in contrast of checkpoint's creation)
        //Components are created with the proper rotation so that they apply to the track
        /*First, create the startline and start trigger*/
        //pdriver means that we create car agents.Usefull for playerTrack
        if (l == 1)
        {
           
            if (ag == true)
            {
                //Rounded so its centered after first straight tile
                float zinc = Mathf.Round((pArray[i, 2] + 3f) / 10f) * 10f;
                //Debug.Log("zinc" + zinc);

                startline = Instantiate(line, new Vector3(pArray[i, 0], pArray[i, 1] + 0.1f, zinc - 6f), line.transform.rotation);
                starttrigger = Instantiate(S1, new Vector3(startline.transform.position.x, startline.transform.position.y, startline.transform.position.z), S1.transform.rotation);
                starttrigger.SetActive(true);
            }
            else
            {
                if (pdriver)
                {
                    //Rounded so its centered after first straight tile
                    float zinc = Mathf.Round((pArray[i, 2] + 3f) / 10f) * 10f; 
                    //Debug.Log("zinc" + zinc);
                    startline = Instantiate(line, new Vector3(pArray[i, 0], pArray[i, 1] + 0.1f, zinc - 6f), line.transform.rotation);

                    GameObject cars = Instantiate(player[5], new Vector3(startline.transform.position.x, startline.transform.position.y, startline.transform.position.z - 2.2f), player[5].transform.rotation);
                    cars.SetActive(true);
                }
            }
        }
    }


    private void CreateTriggerEnd(int i, int l, float[,] pArray, GameObject S2, int trackLength, string[,] direction,Indi chromo)
    {
        //The components are created through the constuction of the track (in contrast of checkpoint's creation)
        //Components are created with the proper rotation so that they apply to the track
        /*First, create the startline and start trigger*/
        //pdriver means that we create car agents.Usefull for playerTrack

        /*Last, create the end trigger*/
        if (l == trackLength)
        {
            /*Adjust end trigger based on last tile
             Straight tile is smaller
             */
            endtrigger = null;
            if (direction[l - 1, i] == "right")
            {
                if (chromo.CHROMOSOME[l - 1].CompareTag("straight"))
                {
                    endtrigger = Instantiate(S2, new Vector3(pArray[i, 0] + boxSize[1].x / 2 - 2.4f, pArray[i, 1], pArray[i, 2]), S2.transform.rotation);
                }
                else
                {
                    endtrigger = Instantiate(S2, new Vector3(pArray[i, 0] + boxSize[1].x / 2 - 0.4f, pArray[i, 1], pArray[i, 2]), S2.transform.rotation);
                }
                
                endtrigger.transform.Rotate(0f, 0f, 90.0f);
            }
            else if (direction[l - 1, i] == "straight")
            {
                if (chromo.CHROMOSOME[l - 1].CompareTag("straight"))
                {
                    endtrigger = Instantiate(S2, new Vector3(pArray[i, 0], pArray[i, 1], pArray[i, 2] + boxSize[1].z / 2 - 2.4f), S2.transform.rotation);
                }
                else
                {
                    endtrigger = Instantiate(S2, new Vector3(pArray[i, 0], pArray[i, 1], pArray[i, 2] + boxSize[1].z / 2 - 0.4f), S2.transform.rotation);
                }
                
                endtrigger.transform.Rotate(0f, 0f, 0.0f);

            }
            else if (direction[l - 1, i] == "back")
            {
                if (chromo.CHROMOSOME[l - 1].CompareTag("straight"))
                {
                    endtrigger = Instantiate(S2, new Vector3(pArray[i, 0], pArray[i, 1], pArray[i, 2] - boxSize[1].z / 2 + 2.4f), S2.transform.rotation);
                }
                else
                {
                    endtrigger = Instantiate(S2, new Vector3(pArray[i, 0], pArray[i, 1], pArray[i, 2] - boxSize[1].z / 2 + 0.4f), S2.transform.rotation);
                }
                
                endtrigger.transform.Rotate(0f, 0f, 180.0f);
            }
            else if (direction[l - 1, i] == "left")
            {
                if (chromo.CHROMOSOME[l - 1].CompareTag("straight"))
                {
                    endtrigger = Instantiate(S2, new Vector3(pArray[i, 0] - boxSize[1].x / 2 + 2.4f, pArray[i, 1], pArray[i, 2]), S2.transform.rotation);
                }
                else
                {
                    endtrigger = Instantiate(S2, new Vector3(pArray[i, 0] - boxSize[1].x / 2 + 0.4f, pArray[i, 1], pArray[i, 2]), S2.transform.rotation);
                }
                
                endtrigger.transform.Rotate(0f, 0f, 270f);
            }

            endtrigger.SetActive(true);
        }
    }


    /*CreateCheck
     Create Checkpoint in the middle of the circuit. The circuit is fully completed and the last thing that is inserted is the checkpoint*/
    private void CreateCheck(Indi chromo, int i)
    {
        /*Find the middle of the circuit and rotate it throught the direction of the track*/
        GameObject gg = chromo.CHROMOSOME[(Indi.SEQUENCE_LENGTH / 2) - 2];

        //Find the direction of the track so the trigger is applied to the track smoothly
        if (direction[(Indi.SEQUENCE_LENGTH / 2) - 2, i] == "straight")
        {
            checktrigger = Instantiate(check, new Vector3(gg.transform.position.x, gg.transform.position.y, gg.transform.position.z + boxSize[0].z / 2), check.transform.rotation);
            checktrigger.transform.Rotate(0f, 0f, 0.0f);
        }
        else if (direction[(Indi.SEQUENCE_LENGTH / 2) - 2, i] == "right")
        {
            checktrigger = Instantiate(check, new Vector3(gg.transform.position.x + boxSize[0].x / 2, gg.transform.position.y, gg.transform.position.z), check.transform.rotation);
            checktrigger.transform.Rotate(0f, 0f, 90.0f);
        }
        else if (direction[(Indi.SEQUENCE_LENGTH / 2) - 2, i] == "left")
        {
            checktrigger = Instantiate(check, new Vector3(gg.transform.position.x - boxSize[0].x / 2, gg.transform.position.y, gg.transform.position.z), check.transform.rotation);
            checktrigger.transform.Rotate(0f, 0f, 270f);
        }
        else if (direction[(Indi.SEQUENCE_LENGTH / 2) - 2, i] == "back")
        {
            checktrigger = Instantiate(check, new Vector3(gg.transform.position.x, gg.transform.position.y, gg.transform.position.z - boxSize[0].z / 2), check.transform.rotation);
            checktrigger.transform.Rotate(0f, 0f, 180.0f);
        }
        checktrigger.SetActive(true);
    }


    /*CreateSegment*
     Create tile for every track
     */
    private void CreateSegment(int i, int l, Indi chromo, float[,] pArray, string[,] direction, int tLength, bool pr,int num)
    {
        /*Steps
                 1)Create tile based on tag
                 2)Check if creating the tile, the rest track has collision problems
                 3)Collision is faced by destroying the 3 previous tiles and constructing one
                 4)Calculate tile space so the next tile can be created in the remaining space
                 */

        //Step 1
        if (chromo.chrs[l] == seg[0])
        {
            if (l != 0)
            {
                //Adjust straight tile rotation based on its direction. So dont collide when instantiate
                if (direction[l - 1, i] == "right")
                {
                    chromo.CHROMOSOME.Add(Instantiate(segments[0], new Vector3(pArray[i, 0], pArray[i, 1] + 0.1f, pArray[i, 2]), Quaternion.Euler(0, 90, 0)));
                }
                else if (direction[l - 1, i] == "left")
                {
                    chromo.CHROMOSOME.Add(Instantiate(segments[0], new Vector3(pArray[i, 0], pArray[i, 1] + 0.1f, pArray[i, 2]), Quaternion.Euler(0, 270, 0)));
                }
                else
                {
                    chromo.CHROMOSOME.Add(Instantiate(segments[0], new Vector3(pArray[i, 0], pArray[i, 1] + 0.1f, pArray[i, 2]), segments[0].transform.rotation));
                }
            }

            else
            {//Adjust straight tile rotation based on its direction. So dont collide when instantiate -- for player track
                if (direction[0, i] == "right")
                {
                    chromo.CHROMOSOME.Add(Instantiate(segments[0], new Vector3(pArray[i, 0], pArray[i, 1] + 0.1f, pArray[i, 2]), Quaternion.Euler(0, 90, 0)));
                }
                else if (direction[0, i] == "left")
                {
                    chromo.CHROMOSOME.Add(Instantiate(segments[0], new Vector3(pArray[i, 0], pArray[i, 1] + 0.1f, pArray[i, 2]), Quaternion.Euler(0, 270, 0)));
                }
                else
                {
                    chromo.CHROMOSOME.Add(Instantiate(segments[0], new Vector3(pArray[i, 0], pArray[i, 1] + 0.1f, pArray[i, 2]), segments[0].transform.rotation));
                }
            }
                
        }
        //No need to adjust rotationo turn tiles
        else if (chromo.chrs[l] == seg[1])
        {
            chromo.CHROMOSOME.Add(Instantiate(segments[1], new Vector3(pArray[i, 0], pArray[i, 1] + 0.1f, pArray[i, 2]), segments[1].transform.rotation));
        }
        else if (chromo.chrs[l] == seg[2])
        {
            chromo.CHROMOSOME.Add(Instantiate(segments[2], new Vector3(pArray[i, 0], pArray[i, 1] + 0.1f, pArray[i, 2]), segments[2].transform.rotation));
        }


        //Step 2
        CollisionCheck(chromo.CHROMOSOME, l);
        if (Collide)
        {
            //Step 3
            int pos = chromo.CHROMOSOME.Count - 1;
            GameObject gg = chromo.CHROMOSOME[pos - 2];

            //Find the position of the last track taking into account the deletion of the 4 tiles
            pArray[i, 0] = chromo.CHROMOSOME[pos - 2].transform.position.x;
            pArray[i, 1] = chromo.CHROMOSOME[pos - 2].transform.position.y - 0.1f;
            pArray[i, 2] = chromo.CHROMOSOME[pos - 2].transform.position.z;

            //Delete 1
            Destroy(chromo.CHROMOSOME[pos]);
            chromo.CHROMOSOME.RemoveAt(pos);
            chromo.chrs.RemoveAt(pos);
            direction[pos, i] = "";

            pos = chromo.CHROMOSOME.Count - 1;
            ////Delete 2
            Destroy(chromo.CHROMOSOME[pos]);
            chromo.CHROMOSOME.RemoveAt(pos);
            chromo.chrs.RemoveAt(pos);
            direction[pos, i] = "";

            pos = chromo.CHROMOSOME.Count - 1;
            //Delete 3
            Destroy(chromo.CHROMOSOME[pos]);
            chromo.CHROMOSOME.RemoveAt(pos);
            chromo.chrs.RemoveAt(pos);
            direction[pos, i] = "";


            float rr = Random.Range(0.0f, 100.0f) / 100.0f;
            /*Check the tile after deletion and consider the available tiles that can be created*/
            /*turns =true means mutation for agent
             turns=false means mutation for boundarys . Need only tiles of turns to create boundary tracks*/
            if (chromo.CHROMOSOME.Count == 0)
            {
                Debug.Log("I am here");
                chromo.CHROMOSOME.Add(Instantiate(segments[0], new Vector3(pArray[i, 0], pArray[i, 1] + 0.1f, pArray[i, 2]), segments[0].transform.rotation));
                Destroy(startline);
                Destroy(starttrigger);

            }
            else
            {
                //Adjust tiles after collision
                if (gg.CompareTag("straight"))
                {
                    
                    Transform poss = chromo.CHROMOSOME[pos - 1].transform;
                    if (chromo.CHROMOSOME[pos - 1].CompareTag("straight"))
                    {                        
                        if (direction[pos - 1, i] == "right")
                        {
                            pArray[i, 0] = poss.position.x + 8.35f;
                        }
                        else if (direction[pos - 1, i] == "left")
                        {
                            pArray[i, 0] = poss.position.x - 8.35f;
                        }
                        else if (direction[pos - 1, i] == "straight")
                        {
                            pArray[i, 2] = poss.position.z + 8.35f;
                        }
                        else if (direction[pos - 1, i] == "back")
                        {
                            pArray[i, 2] = poss.position.z - 8.35f;
                        }
                    }
                    else
                    {

                        if (direction[pos - 1, i] == "right")
                        {
                            pArray[i, 0] = poss.position.x + 10f;
                        }
                        else if (direction[pos - 1, i] == "left")
                        {
                            pArray[i, 0] = poss.position.x - 10f;
                        }
                        else if (direction[pos - 1, i] == "straight")
                        {
                            pArray[i, 2] = poss.position.z + 10f;
                        }
                        else if (direction[pos - 1, i] == "back")
                        {
                            pArray[i, 2] = poss.position.z - 10f;
                        }
                    }

                    if (rr < 0.5f)
                    {

                        //Create left tile
                        chromo.CHROMOSOME.Add(Instantiate(segments[1], new Vector3(pArray[i, 0], pArray[i, 1] + 0.1f, pArray[i, 2]), segments[1].transform.rotation));
                    }
                    else
                    {
                        //Create right tile
                        chromo.CHROMOSOME.Add(Instantiate(segments[2], new Vector3(pArray[i, 0], pArray[i, 1] + 0.1f, pArray[i, 2]), segments[2].transform.rotation));
                    }
                }
                else if (gg.CompareTag("turnR"))
                {

                    if (rr < 0.5f)
                    {
                        Transform poss = chromo.CHROMOSOME[pos - 1].transform;
                        if (chromo.CHROMOSOME[pos - 1].CompareTag("straight"))
                        {
                            
                            if (direction[pos - 1, i] == "right")
                            {
                                pArray[i, 0] = poss.position.x + 8.35f;
                            }
                            else if (direction[pos - 1, i] == "left")
                            {
                                pArray[i, 0] = poss.position.x - 8.35f;
                            }
                            else if (direction[pos - 1, i] == "straight")
                            {
                                pArray[i, 2] = poss.position.z + 8.35f;
                            }
                            else if (direction[pos - 1, i] == "back")
                            {
                                pArray[i, 2] = poss.position.z - 8.35f;
                            }
                        }
                        else
                        {
                            if (direction[pos - 1, i] == "right")
                            {
                                pArray[i, 0] = poss.position.x + 10f;
                            }
                            else if (direction[pos - 1, i] == "left")
                            {
                                pArray[i, 0] = poss.position.x - 10f;
                            }
                            else if (direction[pos - 1, i] == "straight")
                            {
                                pArray[i, 2] = poss.position.z + 10f;
                            }
                            else if (direction[pos - 1, i] == "back")
                            {
                                pArray[i, 2] = poss.position.z - 10f;
                            }
                        }

                        //Create left tile
                        chromo.CHROMOSOME.Add(Instantiate(segments[2], new Vector3(pArray[i, 0], pArray[i, 1] + 0.1f, pArray[i, 2]), segments[2].transform.rotation));
                    }
                    else
                    {
                        Transform poss = chromo.CHROMOSOME[pos - 1].transform;
                        if (!chromo.CHROMOSOME[pos - 1].CompareTag("straight"))
                        {
                            
                            if (direction[pos - 1, i] == "right")
                            {
                                pArray[i, 0] = poss.position.x + 8.35f;
                            }
                            else if (direction[pos - 1, i] == "left")
                            {
                                pArray[i, 0] = poss.position.x - 8.35f;
                            }
                            else if (direction[pos - 1, i] == "straight")
                            {
                                pArray[i, 2] = poss.position.z + 8.35f;
                            }
                            else if (direction[pos - 1, i] == "back")
                            {
                                pArray[i, 2] = poss.position.z - 8.35f;
                            }
                        }
                        else
                        {
                            if (direction[pos - 1, i] == "right")
                            {
                                pArray[i, 0] = poss.position.x + 6.55f;
                            }
                            else if (direction[pos - 1, i] == "left")
                            {
                                pArray[i, 0] = poss.position.x - 6.55f;
                            }
                            else if (direction[pos - 1, i] == "straight")
                            {
                                pArray[i, 2] = poss.position.z + 6.55f;
                            }
                            else if (direction[pos - 1, i] == "back")
                            {
                                pArray[i, 2] = poss.position.z - 6.55f;
                            }
                        }


                        //Create straight tile and adjust rotation
                        if (direction[pos - 1, i] == "right")
                        {
                            chromo.CHROMOSOME.Add(Instantiate(segments[0], new Vector3(pArray[i, 0], pArray[i, 1] + 0.1f, pArray[i, 2]), Quaternion.Euler(0, 90, 0)));
                        }
                        else if (direction[pos - 1, i] == "left")
                        {
                            chromo.CHROMOSOME.Add(Instantiate(segments[0], new Vector3(pArray[i, 0], pArray[i, 1] + 0.1f, pArray[i, 2]), Quaternion.Euler(0, 270, 0)));
                        }
                        else
                        {
                            chromo.CHROMOSOME.Add(Instantiate(segments[0], new Vector3(pArray[i, 0], pArray[i, 1] + 0.1f, pArray[i, 2]), segments[0].transform.rotation));
                        }
                    }
                }
                else if (gg.CompareTag("turnL"))
                {
                    if (rr < 0.5f)
                    {
                        Transform poss = chromo.CHROMOSOME[pos - 1].transform;
                        //Create right tile
                        if (chromo.CHROMOSOME[pos - 1].CompareTag("straight"))
                        {

                            if (direction[pos - 1, i] == "right")
                            {
                                pArray[i, 0] = poss.position.x + 8.35f;
                            }
                            else if (direction[pos - 1, i] == "left")
                            {
                                pArray[i, 0] = poss.position.x - 8.35f;
                            }
                            else if (direction[pos - 1, i] == "straight")
                            {
                                pArray[i, 2] = poss.position.z + 8.35f;
                            }
                            else if (direction[pos - 1, i] == "back")
                            {
                                pArray[i, 2] = poss.position.z - 8.35f;
                            }
                        }
                        else
                        {
                            if (direction[pos - 1, i] == "right")
                            {
                                pArray[i, 0] = poss.position.x + 10f;
                            }
                            else if (direction[pos - 1, i] == "left")
                            {
                                pArray[i, 0] = poss.position.x - 10f;
                            }
                            else if (direction[pos - 1, i] == "straight")
                            {
                                pArray[i, 2] = poss.position.z + 10f;
                            }
                            else if (direction[pos - 1, i] == "back")
                            {
                                pArray[i, 2] = poss.position.z - 10f;
                            }
                        }

                        chromo.CHROMOSOME.Add(Instantiate(segments[1], new Vector3(pArray[i, 0], pArray[i, 1] + 0.1f, pArray[i, 2]), segments[1].transform.rotation));
                    }
                    else
                    {
                        Transform poss = chromo.CHROMOSOME[pos - 1].transform;
                        if (!chromo.CHROMOSOME[pos - 1].CompareTag("straight"))
                        {
                            if (direction[pos - 1, i] == "right")
                            {
                                pArray[i, 0] = poss.position.x + 8.35f;
                            }
                            else if (direction[pos - 1, i] == "left")
                            {
                                pArray[i, 0] = poss.position.x - 8.35f;
                            }
                            else if (direction[pos - 1, i] == "straight")
                            {
                                pArray[i, 2] = poss.position.z + 8.35f;
                            }
                            else if (direction[pos - 1, i] == "back")
                            {
                                pArray[i, 2] = poss.position.z - 8.35f;
                            }
                        }
                        else
                        {
                            if (direction[pos - 1, i] == "right")
                            {
                                pArray[i, 0] = poss.position.x + 6.55f;
                            }
                            else if (direction[pos - 1, i] == "left")
                            {
                                pArray[i, 0] = poss.position.x - 6.55f;
                            }
                            else if (direction[pos - 1, i] == "straight")
                            {
                                pArray[i, 2] = poss.position.z + 6.55f;
                            }
                            else if (direction[pos - 1, i] == "back")
                            {
                                pArray[i, 2] = poss.position.z - 6.55f;
                            }
                        }

                        //Create straight tile and adjust rotation
                        if (direction[pos - 1, i] == "right")
                        {
                            chromo.CHROMOSOME.Add(Instantiate(segments[0], new Vector3(pArray[i, 0], pArray[i, 1] + 0.1f, pArray[i, 2]), Quaternion.Euler(0, 90, 0)));
                        }
                        else if (direction[pos - 1, i] == "left")
                        {
                            chromo.CHROMOSOME.Add(Instantiate(segments[0], new Vector3(pArray[i, 0], pArray[i, 1] + 0.1f, pArray[i, 2]), Quaternion.Euler(0, 270, 0)));
                        }
                        else
                        {
                            chromo.CHROMOSOME.Add(Instantiate(segments[0], new Vector3(pArray[i, 0], pArray[i, 1] + 0.1f, pArray[i, 2]), segments[0].transform.rotation));
                        }
                    }
                }

            }

            if (num == 5)
            {
                //Adjust next tile when evolve
                nextcsTile = parent1.Crossover(parent1, segmentCount);
                nextTile = nextcsTile;
                //Debug.Log("EDW");
            }
            if (num == 6)
            {
                //Adjust next tile when evolve
                if (!ftime)
                {
                    //Debug.Log("GT EDW"+tracklength);
                    
                    nextpTile = nextTrack[tracklength];
                    nextTile = nextpTile;
                }
                    

            }

            chromo.chrs.Add(chromo.CHROMOSOME[pos].tag);
            CalculateDistance(pos, i, chromo, pArray, direction, tLength, pr);
        }
        else
        {
            CalculateDistance(l, i, chromo, pArray, direction, tLength, pr);
        }


    }


    /*CalculateDistance
     Calculates the size that a tile takes, and adapts the direction of the track after a tile's construction to the track
     */
    private void CalculateDistance(int l, int i, Indi chromo, float[,] pArray, string[,] direction, int trackLength, bool pp)
    {
        /*4 Direction
         Straight, Right,Left,Back*/
        //First tile is always straight so the direction is also straight

        /*
         Notations on
        straight-straight
        straight-turnR/L
        turnR-straight
        tunrL-straight
         */
        if (l == 0)
        {
            //pp is usefull for remembering direction of playerTrack.
            if (!pp)
            {
                direction[l, i] = "straight";
                pArray[i, 1] += boxSize[0].y;   //Increase segment distance of track
                if (nextTile == "straight")
                {
                    pArray[i, 2] += (6.55f + 0.001f);
                }
                else
                {
                    pArray[i, 2] += (8.35f + 0.001f);
                }
            }
            else
            {
                //Debug.Log("GEIA S");

                if (direction[l, i] == "right")
                {
                    pArray[i, 1] += boxSize[0].y;   //Increase segment distance of track

                    if (nextTile == "straight")
                    {
                        pArray[i, 0] += (6.55f + 0.001f);
                    }
                    else
                    {
                        pArray[i, 0] += (8.35f + 0.001f);

                    }

                }
                else if (direction[l, i] == "straight")
                {
                    chromo.CHROMOSOME[l].transform.Rotate(0f, 0f, 0f);

                    pArray[i, 1] += boxSize[0].y;  //Increase segment distance of track
                    if (nextTile == "straight")
                    {
                        pArray[i, 2] += (6.55f + 0.001f);
                    }
                    else
                    {
                        pArray[i, 2] += (8.35f + 0.001f);
                    }

                }
                else if (direction[l, i] == "back")
                {
                    chromo.CHROMOSOME[l].transform.Rotate(0f, 180.0f, 0f);
                    pArray[i, 1] += boxSize[0].y;   //Increase segment distance of track
                    if (nextTile == "straight")
                    {
                        pArray[i, 2] -= (6.55f + 0.001f);
                    }
                    else
                    {
                        pArray[i, 2] -= (8.35f + 0.001f);

                    }

                }
                else if (direction[l, i] == "left")
                {
                    pArray[i, 1] += boxSize[0].y;   //Increase segment distance of track
                    if (nextTile == "straight")
                    {
                        pArray[i, 0] -= (6.55f + 0.001f);
                    }
                    else
                    {
                        pArray[i, 0] -= (8.35f + 0.001f);
                    }
                }
            }
        }
        else
        {
            direction[l, i] = direction[l - 1, i];

            if (chromo.CHROMOSOME[l].CompareTag("straight"))
            {
                if (direction[l, i] == "right")
                {
                    if (l <= trackLength - 2)
                    {
                        pArray[i, 1] += boxSize[0].y;    //Increase segment distance of tracks

                        if (nextTile == "straight")
                        {
                            pArray[i, 0] += (6.55f + 0.001f);
                        }
                        else
                        {
                            pArray[i, 0] += (8.35f + 0.001f);
                        }
                    }
                }
                else if (direction[l, i] == "straight")
                {
                    chromo.CHROMOSOME[l].transform.Rotate(0f, 0f, 0f);

                    if (l <= trackLength - 2)
                    {
                        pArray[i, 1] += boxSize[0].y;    //Increase segment distance of track

                        if (nextTile == "straight")
                        {
                            pArray[i, 2] += (6.55f + 0.001f);

                        }
                        else
                        {
                            pArray[i, 2] += (8.35f + 0.001f);
                        }
                    }
                }
                else if (direction[l, i] == "back")
                {
                    chromo.CHROMOSOME[l].transform.Rotate(0f, 180.0f, 0f);

                    if (l <= trackLength - 2)
                    {
                        pArray[i, 1] += boxSize[0].y;   //Increase segment distance of track
                        if (nextTile == "straight")
                        {
                            pArray[i, 2] -= (6.55f + 0.001f);
                        }
                        else
                        {
                            pArray[i, 2] -= (8.35f + 0.001f);
                        }
                    }
                }
                else if (direction[l, i] == "left")
                {
                    if (l <= trackLength - 2)
                    {
                        pArray[i, 1] += boxSize[0].y;   //Increase segment distance of track

                        if (nextTile == "straight")
                        {
                            pArray[i, 0] -= (6.55f + 0.001f);
                        }
                        else
                        {
                            pArray[i, 0] -= (8.35f + 0.001f);
                        }
                    }

                }
            }
            else if (chromo.CHROMOSOME[l].CompareTag("turnR"))
            {
                if (direction[l, i] == "right")
                {
                    chromo.CHROMOSOME[l].transform.Rotate(0f, 180.0f, 0f);

                    if (l <= trackLength - 2)
                    {
                        pArray[i, 1] += boxSize[1].y;   //Increase segment distance of track

                        if (nextTile == "straight")
                        {
                            pArray[i, 2] -= 8.35f + 0.001f;
                        }
                        else
                        {
                            pArray[i, 2] -= 10f + 0.001f;
                        }
                    }
                    direction[l, i] = "back";
                }
                else if (direction[l, i] == "straight")
                {
                    chromo.CHROMOSOME[l].transform.Rotate(0f, 90f, 0f);


                    if (l <= trackLength - 2)
                    {
                        pArray[i, 1] += boxSize[1].y;   //Increase segment distance of track

                        if (nextTile == "straight")
                        {
                            pArray[i, 0] += (8.35f + 0.001f);
                        }
                        else
                        {
                            pArray[i, 0] += (10f + 0.001f);
                        }
                    }
                    direction[l, i] = "right";
                }
                else if (direction[l, i] == "back")
                {
                    chromo.CHROMOSOME[l].transform.Rotate(0f, 270.0f, 0f);

                    if (l <= trackLength - 2)
                    {
                        pArray[i, 1] += boxSize[1].y;  //Increase segment distance of track

                        if (nextTile == "straight")
                        {
                            pArray[i, 0] -= (8.35f + 0.001f);
                        }
                        else
                        {
                            pArray[i, 0] -= (10f + 0.001f);
                        }
                    }
                    direction[l, i] = "left";
                }
                else if (direction[l, i] == "left")
                {
                    chromo.CHROMOSOME[l].transform.Rotate(0f, 0f, 0f);

                    if (l <= trackLength - 2)
                    {
                        pArray[i, 1] += boxSize[1].y;   //Increase segment distance of track
                        if (nextTile == "straight")
                        {
                            pArray[i, 2] += 8.35f + 0.001f;
                        }
                        else
                        {
                            pArray[i, 2] += 10f + 0.001f;
                        }
                    }
                    direction[l, i] = "straight";
                }
            }
            else if (chromo.CHROMOSOME[l].CompareTag("turnL"))
            {
                if (direction[l, i] == "right")
                {
                    chromo.CHROMOSOME[l].transform.Rotate(0f, 0.0f, 0f);

                    if (l <= trackLength - 2)
                    {
                        pArray[i, 1] += boxSize[2].y;   //Increase segment distance of track
                        if (nextTile == "straight")
                        {
                            pArray[i, 2] += 8.35f + 0.001f;
                        }
                        else
                        {
                            pArray[i, 2] += 10f + 0.001f;
                        }
                        
                    }
                    direction[l, i] = "straight";
                }
                else if (direction[l, i] == "straight")
                {
                    chromo.CHROMOSOME[l].transform.Rotate(0f, 270.0f, 0f);

                    if (l <= trackLength - 2)
                    {
                        pArray[i, 1] += boxSize[2].y;   //Increase segment distance of track
                        if (nextTile == "straight")
                        {
                            pArray[i, 0] -= (8.35f + 0.001f);
                        }
                        else
                        {
                            pArray[i, 0] -= (10f + 0.001f);
                        }
                        
                    }
                    direction[l, i] = "left";
                }
                else if (direction[l, i] == "back")
                {
                    chromo.CHROMOSOME[l].transform.Rotate(0f, 90.0f, 0f);

                    if (l <= trackLength - 2)
                    {
                        pArray[i, 1] += boxSize[2].y;  //Increase segment distance of track
                        if (nextTile == "straight")
                        {
                            pArray[i, 0] += 8.35f + 0.001f;
                        }
                        else
                        {
                            pArray[i, 0] += 10f + 0.001f;
                        }
                    }
                    direction[l, i] = "right";
                }
                else if (direction[l, i] == "left")
                {
                    chromo.CHROMOSOME[l].transform.Rotate(0f, 180f, 0f);

                    if (l <= trackLength - 2)
                    {
                        pArray[i, 1] += boxSize[2].y;   //Increase segment distance of track
                        if (nextTile == "straight")
                        {
                            pArray[i, 2] -= 8.35f + 0.001f;
                        }
                        else
                        {
                            pArray[i, 2] -= 10f + 0.001f;
                        }
                    }
                    direction[l, i] = "back";
                }
            }
        }
    }



}
