using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System;

public class  PlayerWriteScript : MonoBehaviour
{
    public static string path;
    public static string content;
    public static int count = 0;
    public static float tt;
    // Start is called before the first frame update

    private void Start()
    {
        CreateText();
    }

    void CreateText()
    {
        DateTime dt = DateTime.UtcNow;

        path = Application.dataPath + "/" + RH.POPULATION_SIZE + "_POPULATION_SIZE_" + (1 - 2 * Indi.mutation) + "_mutation_" + "_s" + Indi.SEQUENCE_LENGTH + "_" + dt.ToString("HH_mm dd MMMM, yyyy") + "_" + "3.txt";
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

        content = "level" + "\t" +  "Timer" + "\t" + "Metric" + "\n";
        File.AppendAllText(path, content);

        
    }

    public static void CreateFlow()
    {

        content = RH.level + "\t" + tt + "\t" + RH.exw+ "\n";
        File.AppendAllText(path, content);

    }

    public void Update()
    {
        if (count == 0 && RH.endofEvolution)
        {
            content = "\n"+RH.bounds0[0] + "\t" + RH.bounds0[1]+"\n\n";
            File.AppendAllText(path, content);
            content = RH.bounds60[0] + "\t" + RH.bounds60[1] + "\n\n";
            File.AppendAllText(path, content);
            count++;
        }
    }
}
