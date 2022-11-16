using Microsoft.TeamFoundation.TestManagement.WebApi;
using System.Configuration;
using YouTrackSharp;
using YouTrackSharp.Issues;

namespace AzDO
{
    public class youtrack
    {
        public async Task youtrackAsync()
        {
            var youtrackUri     = ConfigurationManager.AppSettings["YoutrackURL"];
            var youtrackToken   = ConfigurationManager.AppSettings["YoutrackToken"];
            var youtrackProject = ConfigurationManager.AppSettings["YoutrackProjectName"];
            var youtrackAssignee    = ConfigurationManager.AppSettings["YoutrackAssignee"];

            try
            {
                var connection = new BearerTokenConnection(youtrackUri, youtrackToken);
                #region create issues
                var issuesService = connection.CreateIssuesService();
                var newIssue = new Issue
                {
                    Summary = "Test issue",
                    Description = "Test issue description."
                };

                newIssue.SetField("Assignee", youtrackAssignee);
                newIssue.SetField("Type", "Bug"); // non default value

                //Note: Commenting out to avoid keep creating new tickets everytime while development
                var result = issuesService.CreateIssue("USP", newIssue).Result;
                Console.WriteLine("New Issue creted - ", result);// result will be like projectid-{issueid}. e.g. USP-230 , which you can log back in AzDO
                #endregion

                #region get issues
                var issues = await issuesService.GetIssuesInProject(youtrackProject, take: 250);
                foreach (var issue in issues)
                {
                    Console.WriteLine("{0} {1} - State: {2}",
                        issue.Id,
                        issue.Summary,
                        issue.GetField("State")?.AsString());
                }
                #endregion

                #region project service
                var projectsService = connection.CreateProjectsService();
                var projectsForCurrentUser = await projectsService.GetAccessibleProjects();

                foreach (var project in projectsForCurrentUser)
                {
                    object value = $"{project.Name} - {project.ShortName}";
                    Console.WriteLine(value);
                }
                #endregion

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }
    }
}
