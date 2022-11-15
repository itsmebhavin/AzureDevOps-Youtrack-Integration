# Azure DevOps to-from Youtrack replication or syncing issues between

## Using DevOps CLI and Youtrack REST API
- Using Azure DevOps C# nuget package
- Using YoutrackSharp C# lib from JetBrains

### Assumed app.config values
```
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
     <appSettings>
        <add key="PAT" value="adjlasjdakljsczm,nxcmziiauqw,zcz;clzkc" />
        <add key="OrgName" value="{YOUR ORG NAME FOR Azure DevOps}"/>
        <add key="ProjectName" value="{PROJECT NAME IN AzDO}" />
		 <add key="YoutrackProijectName" value="{Youtrack Project ID}"/>
		 <add key="YoutrackToken" value="perm:YnB {Youtrack Token}"/>
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
