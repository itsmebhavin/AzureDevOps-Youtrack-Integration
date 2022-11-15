using AzDO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {

        // Get values from the config file. PAT = Board access personal access token from azure devops
        string OrgName = ConfigurationManager.AppSettings["OrgName"].ToString();
        string PAT = ConfigurationManager.AppSettings["PAT"].ToString();
        string ProjectName = ConfigurationManager.AppSettings["ProjectName"].ToString();

        // Azure DevOps Workitems 
        //QueryExecutor qe = new QueryExecutor(OrgName, PAT);
        //var bugs = qe.QueryOpenBugs(ProjectName);
        //Task workItems = qe.PrintOpenBugsAsync(ProjectName);

        // Youtrack 
        //youtrack yt = new youtrack();
        //await yt.youtrackAsync();



        //Azure DevOps REST API way
        AzureDevOpsRestApi restway = new AzureDevOpsRestApi();
       // AzureDevOpsRestApi.AzDevOpsREST();
        AzureDevOpsRestApi.GetProjects();
    }
}

