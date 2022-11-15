using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzDO
{

    public class QueryExecutor
    {


        private readonly Uri uri;
        private readonly string personalAccessToken;

        /// <summary>
        ///     Initializes a new instance of the <see cref="QueryExecutor" /> class.
        /// </summary>
        /// <param name="orgName">
        ///     An organization in Azure DevOps Services. If you don't have one, you can create one for free:
        ///     <see href="https://go.microsoft.com/fwlink/?LinkId=307137" />.
        /// </param>
        /// <param name="personalAccessToken">
        ///     A Personal Access Token, find out how to create one:
        ///     <see href="/azure/devops/organizations/accounts/use-personal-access-tokens-to-authenticate?view=azure-devops" />.
        /// </param>
        public QueryExecutor(string orgName, string personalAccessToken)
        {
            this.uri = new Uri("https://dev.azure.com/" + orgName);
            this.personalAccessToken = personalAccessToken;
        }


        /// <summary>
        ///     Execute a WIQL (Work Item Query Language) query to return a list of open bugs.
        /// </summary>
        /// <param name="project">The name of your project within your organization.</param>
        /// <returns>A list of <see cref="WorkItem"/> objects representing all the open bugs.</returns>
        public async Task<IList<WorkItem>> QueryOpenBugs(string project)
        {
            var credentials = new VssBasicCredential(string.Empty, this.personalAccessToken);

            // create a wiql object and build our query
            var wiql = new Wiql()
            {
                // NOTE: Even if other columns are specified, only the ID & URL are available in the WorkItemReference
                Query = "Select [Id] " +
                        "From WorkItems " +
                        "Where [Work Item Type] = 'Bug' " +
                        "And [System.TeamProject] = '" + project + "' " +
                        "And [System.State] <> 'Closed' " +
                        "Order By [State] Asc, [Changed Date] Desc",
            };
            try
            {
                //create instance of work item tracking http client
                using (WorkItemTrackingHttpClient workItemTrackingHttpClient = new WorkItemTrackingHttpClient(uri, credentials))
                {
                    //execute the query to get the list of work items in the results
                    WorkItemQueryResult workItemQueryResult = workItemTrackingHttpClient.QueryByWiqlAsync(wiql).Result;

                    //List<WorkItemField> fieldsQuery = await workItemTrackingHttpClient.GetFieldsAsync(project, null, null, System.Threading.CancellationToken.None);

                    //Console.WriteLine("========== Workitem fields =========");
                    //foreach (var field in fieldsQuery)
                    //{
                    //    Console.WriteLine(field.Name + "\t" + field.ReferenceName);
                    //}


                    //Splict the query result (the list of work item IDs) into groups of 200.
                    var QueryGroups = from i in Enumerable.Range(0, workItemQueryResult.WorkItems.Count())
                                      group workItemQueryResult.WorkItems.ToList()[i] by i / 200;

                    foreach (var QueryGroup in QueryGroups)
                    {
                        //some error handling                
                        if (QueryGroup.Count() != 0)
                        {
                            //need to get the list of our work item ids and put them into an array
                            List<int> list = new List<int>();
                            foreach (var item in QueryGroup.ToList())
                            {
                                list.Add(item.Id);
                            }
                            int[] arr = list.ToArray();

                            // build a list of the fields we want to see
                            string[] fields = new[] { "System.Id", "System.Title", "System.State", "System.AssignedTo" };


                            //get work items for the ids found in query
                            var workItems = workItemTrackingHttpClient.GetWorkItemsAsync(arr, fields, workItemQueryResult.AsOf).Result;

                            Console.WriteLine("\n\n----------------------------------------------------------------");
                            Console.WriteLine("\nQuery Results: {0} items found for Group {1}", workItems.Count, QueryGroup.Key);
                            Console.WriteLine("\n----------------------------------------------------------------");


                            //loop though work items and write to console
                            foreach (var workItem in workItems)
                            {
                                workItem.Fields.TryGetValue("System.AssignedTo", out Object identityOjbect);
                                if (identityOjbect == null)
                                {
                                    identityOjbect = new { DisplayName = "Unassigned" };
                                }
                                JObject identity = JObject.FromObject(identityOjbect);
                                Console.WriteLine("ID:{0} Title:{1}  State:{2}  AssignedTo: {3}", workItem.Id, workItem.Fields["System.Title"], workItem.Fields["System.State"], identity["DisplayName"]);
                            }

                            return workItems;

                        }
                    }
                }

            }
            catch (Exception ex)
            {

                Console.Write(ex.Message);
                return null;
            }
            return null;
        }

        /// <summary>
        ///     Execute a WIQL (Work Item Query Language) query to print a list of open bugs.
        /// </summary>
        /// <param name="project">The name of your project within your organization.</param>
        /// <returns>An async task.</returns>
        public async Task PrintOpenBugsAsync(string project)
        {
            try
            {
                var workItems = await this.QueryOpenBugs(project).ConfigureAwait(false);

                Console.WriteLine("Query Results: {0} items found", workItems.Count);

                // loop though work items and write to console
                foreach (var workItem in workItems)
                {
                    Console.WriteLine(
                        "{0}\t{1}\t{2}\t{3}",
                        workItem.Id,
                        workItem.Fields["System.Title"],
                        workItem.Fields["System.State"],
                        workItem.Fields["System.AssignedTo"]);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);

            }

        }
    }
}
