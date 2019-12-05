using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JobJournal : MonoBehaviour
{
    public GameObject jobTextPrefab = null;
    public GameObject jobTabWindow = null;
    public List<Job> jobs = new List<Job>();
    
    public void GenerateNewJob()
    {
        Job newJob = new Job();
        jobs.Add(newJob);
        GameObject jobTextObj = Instantiate(jobTextPrefab, jobTabWindow.transform);
        jobTextObj.GetComponent<TextMeshProUGUI>().SetText(newJob.Format());
    }

    public void ClearJobs()
    {
        jobs = new List<Job>();
        foreach (Transform child in jobTabWindow.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
