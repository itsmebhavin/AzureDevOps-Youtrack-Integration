# Azure DevOps to-from Youtrack replication or syncing issues between

## Using DevOps CLI and Youtrack REST API
- Using Azure DevOps C# nuget package
- Using YoutrackSharp C# lib from JetBrains

### Assumed app.config values
```
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
     <appSettings>
        <add key="PAT" value="{Azure PAT with Board scope}" /> <!-- e.g. asdad9lkjsdfj02lsdfknma -->
        <add key="OrgName" value="{YOUR ORG NAME FOR Azure DevOps}"/>
        <add key="ProjectName" value="{PROJECT NAME IN AzDO}" />
		 <add key="YoutrackProjectName" value="{Youtrack Project ID}"/>
		 <add key="YoutrackToken" value="{Youtrack Token}"/> <!-- e.g. perm:YnB.... -->
		 <add key="YoutrackURL" value="{Youtrack URL for your ORG}"/>
		 <add key="YoutrackAssignee" value="{Sample Youtrack Ticket Assignee Username}"/>
  </appSettings> 
</configuration>
```

### References for Azure DevOps
-  https://learn.microsoft.com/en-us/azure/devops/boards/queries/wiql-syntax?view=azure-devops
-  https://learn.microsoft.com/en-us/azure/devops/integrate/quickstarts/work-item-quickstart?view=azure-devops#prerequisites
-  https://learn.microsoft.com/en-us/azure/devops/boards/queries/query-by-date-or-current-iteration?view=azure-devops
-  https://learn.microsoft.com/en-us/azure/devops/integrate/concepts/dotnet-client-libraries?view=azure-devops&viewFallbackFrom=vsts
-  nuget:Microsoft.TeamFoundationServer.Client

### References for Youtrack 
- https://blog.jetbrains.com/youtrack/2017/07/youtracksharp-3-0-beta-a-net-standard-client-for-youtrack/
- https://blog.jetbrains.com/youtrack/2011/06/youtracksharp-a-net-client-for-youtrack/
- https://www.conradakunga.com/blog/querying-and-extracting-data-from-youtrack/


## Azure DevOps REST Api with Postman

Set authentication = basic {base64encoded token}

### Queries

- Get workitem by id ```GET https://dev.azure.com/{OrgName}/_apis/wit/workitems?ids=157&api-version=5.1```
```GET https://dev.azure.com/{OrgName}/{ProjectName/ID}/_apis/wit/workItems/94```
- Get projects list ``` GET https://dev.azure.com/{OrgName}/_apis/projects```
- Get all workitem with type=Task ```GET https://dev.azure.com/{OrgName}/{ProjectName/ID}/_apis/wit/workitems/$Task?api-version=6.0```
- Get results based on wiql (workitem query language) ```POST https://dev.azure.com/{OrgName}/{ProjectName/ID}/_apis/wit/wiql?api-version=6.0```
```
BODY Type = Json
{
   "query": "select [System.Id], [System.WorkItemType], [System.Title], [System.AssignedTo], [System.State], [System.Tags] from WorkItems where [System.TeamProject] = @project and [System.WorkItemType] <> '' and [Microsoft.VSTS.Common.StateChangeDate] = @today"
}
```
- Get comments for workitem {id} ```GET https://dev.azure.com/{OrgName}/{ProjectName/ID}/_apis/wit/workItems/94/comments```
- Get all updates for workitem {id} ```GET https://dev.azure.com/{OrgName}/{ProjectName/ID}/_apis/wit/workItems/94/updates```
- Get all fields for project ```GET https://dev.azure.com/{OrgName}/{ProjectName/ID}/_apis/wit/workItems/94/updates```
