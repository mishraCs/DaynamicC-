using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;

public class Class1 : IPlugin
{
    public void Execute(IServiceProvider serviceProvider)
    {
        var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
        var factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
        var service = factory.CreateOrganizationService(context.UserId);
        var tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

        try
        {
            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity targetEntity)
            {
                tracingService.Trace("Updated name column start.");

                // Get PreImage (old value) if available
                string oldValue = string.Empty;
                if (context.PreEntityImages.Contains("PreImage"))
                {
                    Entity preImage = context.PreEntityImages["PreImage"];
                    oldValue = preImage.GetAttributeValue<string>("ge_studentid");
                    tracingService.Trace($"Old value (PreImage) of ge_studentid: {oldValue}");
                }

                // Get the updated value from Target
                if (targetEntity.Attributes.Contains("ge_studentid"))
                {
                    string updatedName = targetEntity.GetAttributeValue<string>("ge_studentid");
                    tracingService.Trace($"Updated value of ge_studentid: {updatedName}");

                    // Use the old value (from PreImage) to match in the query
                    QueryExpression query = new QueryExpression("ge_applicationm");
                    query.Criteria.AddCondition("ge_applicationid", ConditionOperator.Equal, oldValue);

                    EntityCollection results = service.RetrieveMultiple(query);

                    if (results.Entities.Count > 0)
                    {
                        Entity secondEntityRecord = results.Entities[0];
                        tracingService.Trace($"Updated name column in second entity{updatedName}.");
                        secondEntityRecord["ge_applicationid"] = updatedName;
                        // Uncomment the next line to update the second entity record
                        // service.Update(secondEntityRecord);
                        service.Update(secondEntityRecord);
                        tracingService.Trace("Updated name column in second entity.");
                    }
                    else
                    {
                        tracingService.Trace("No matching record found in the second entity.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            tracingService.Trace($"An error occurred: {ex.Message}");
            throw new InvalidPluginExecutionException($"An error occurred in the plugin: {ex.Message}");
        }
    }
}
