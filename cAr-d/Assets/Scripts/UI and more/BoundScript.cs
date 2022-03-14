using UnityEngine;
using System.IO;
using System;

public class BoundScript : MonoBehaviour
{
    private GameObject[] trig;

    private string path;
    private string content;
    private int count = 0;

    private void Start()
    {
        CreateText();
    }

    void Update()
    {
        if (RH.isBound)
        {
            trig = GameObject.FindGameObjectsWithTag("BAgent");
            if (trig.Length > 0)
            {
                RH.endBound = false;
            }
            else
            {
                RH.endBound = true;
            }
        }


    }
    void FixedUpdate()
    {
        if (RH.endBound && count==0)
        {
            CreateBounds();
        }
        //Debug.Log("END BOUND" + RH.endBound);
    }

    void CreateText()
    {
        DateTime dt = DateTime.UtcNow;

        path = Application.dataPath + "/" + RH.POPULATION_SIZE + "_POPULATION_SIZE_" + (1 - 2 * Indi.mutation) + "_mutation_" + "_s" + Indi.SEQUENCE_LENGTH + "_" + dt.ToString("HH_mm dd MMMM, yyyy") + "_" + "2.txt";
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "Login log \n\n");
            File.AppendAllText(path, "Mutation:" + (1 - 2 * Indi.mutation) + "\n");
            File.AppendAllText(path, "Population size:" + RH.POPULATION_SIZE + "\n");
            File.AppendAllText(path, "Sequence length:" + Indi.SEQUENCE_LENGTH + "\n\n");
        }
        else
        {
            File.Delete(path);
            File.WriteAllText(path, "Login log \n\n");
            File.AppendAllText(path, "Mutation:" + (1 - 2 * Indi.mutation) + "\n");
            File.AppendAllText(path, "Population size:" + RH.POPULATION_SIZE + "\n");
            File.AppendAllText(path, "Sequence length:" + Indi.SEQUENCE_LENGTH + "\n\n");

        }
        content = "Login date: " + DateTime.Now + "\n\n";

        File.AppendAllText(path, content);

        content = "Bound Agent Timers: " + "\n\n";

        File.AppendAllText(path, content);
    }

    void CreateBounds()
    {
        content ="Straight\t"+ 0 + "\t" + 1 + "\t" + 2 + "\t" + 3 + "\t" + 4 + "\n";
        File.AppendAllText(path, content);
        content ="\t"+ RH.boundaryLapsed[0] + "\t" + RH.boundaryLapsed[1] + "\t" + RH.boundaryLapsed[2] + "\t" + RH.boundaryLapsed[3] + "\t" + RH.boundaryLapsed[4] + "\n";
        File.AppendAllText(path, content);
        content = "Turns\t" + 0 + "\t" + 1 + "\t" + 2 + "\t" + 3 + "\t" + 4 +  "\n";
        File.AppendAllText(path, content);
        content = "\t" + RH.boundaryLapsed[5] + "\t" + RH.boundaryLapsed[6] + "\t" + RH.boundaryLapsed[7] + "\t" + RH.boundaryLapsed[8] + "\t" + RH.boundaryLapsed[9] +"\n";
        File.AppendAllText(path, content);
        count++;
    }
}
