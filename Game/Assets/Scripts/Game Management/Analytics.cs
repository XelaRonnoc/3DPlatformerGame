using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class Analytics : MonoBehaviour
{

    // Large portions of code from iLearn https://github.com/COMP2160-22s2/comp2160-prac-week-07
    private string path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Logs");

    [SerializeField] private string newName;
    [SerializeField] private string nameFormat = "{0}/{1}-{2:yyyy-MM-dd-HH-mm-ss}.txt";
    [SerializeField] private string[] headers = {};

    private string lineFormat;
    private StreamWriter log = null;

    static private Analytics instance;
    static public Analytics Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("There is no Analystics in the scene.");
            }
            return instance;
        }
    }

    public void Awake()
    {

        if (instance != null)
        {
            // destroy duplicates
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }
    // Start is called before the first frame update
    void Start()
    {

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        string filename = string.Format(nameFormat, path, newName, System.DateTime.Now);

        log = new StreamWriter(filename);

        log.WriteLine(string.Join("\t", headers));
        log.Flush();

        lineFormat = "";
        for(int i = 0; i < headers.Length; i++)
        {
            if (i > 0)
            {
                lineFormat += "\t";
            }
            lineFormat += string.Format("{{{0}}}", i);
        }

    }

    private void OnDestroy()
    {

        if(log != null)
        {
            log.Close();
        }

    }

    public void WriteLine(params object[] args)
    {

        if (args.Length != headers.Length)
        {
            Debug.LogError("Unexpected number of arguments to LogFile.WriteLine");
        }
        log.WriteLine(lineFormat, args);
        log.Flush();

    }


}
