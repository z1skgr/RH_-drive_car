using UnityEngine;
using System.IO;
using System;

public class WriteScript : MonoBehaviour
{
    // Start is called before the first frame update
    private string path;
    private string content;
    private int count = 0;
    //Write file for timers and levels and evolutions
    void Start()
    {
        CreateText();

    }

    // Update is called once per frame
    void Update()
    {
        if (RH.avgWritten)
        {
            CreateTimes();
        }
        
    }

    void CreateText()
    {
        DateTime dt = DateTime.UtcNow;
        
        path = Application.dataPath +"/"+ RH.POPULATION_SIZE + "_POPULATION_SIZE_"+ (1- 2*Indi.mutation)+"_mutation_"+ "_s"+ Indi.SEQUENCE_LENGTH + "_" +  dt.ToString("HH_mm dd MMMM, yyyy") + "_"+ "1.txt";
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

        File.AppendAllText(path,content);
        content = "AvgTimes  \tMaxTimes \tMinTimes \tLevel\n";
        File.AppendAllText(path, content);
    }

    void CreateTimes()
    {
        content =  RH.avgLapsed + "\t" + RH.maxLapsed +  "\t" + RH.minLapsed + "\t" + RH.level+"\n";
        File.AppendAllText(path, content);
        count++;
        RH.avgWritten = false;
    }

}
