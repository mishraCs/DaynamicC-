using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;

namespace Update_logic
{
    public class Class1 : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var service = factory.CreateOrganizationService(context.UserId);
            var tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            tracingService.Trace("Backup student Plugin start");

            List<object> oldAppColoumn = new List<object>();
            if (context.PreEntityImages.Contains("PreImage"))
            {
                Entity preImage = context.PreEntityImages["PreImage"];
                tracingService.Trace("PreImage retrieved successfully");

                // Retrieve each attribute from PreImage
                string oldAppName = preImage.Contains("ge_applicationid") ? preImage.GetAttributeValue<string>("ge_applicationid") : null;
                oldAppColoumn.Add(oldAppName);

                int? oldAppClass = preImage.Contains("ge_noclass") ? preImage.GetAttributeValue<int?>("ge_noclass") : null;
                oldAppColoumn.Add(oldAppClass);

                string oldAppPhone = preImage.Contains("ge_phone") ? preImage.GetAttributeValue<string>("ge_phone") : null;
                oldAppColoumn.Add(oldAppPhone);

                string oldAppEmail = preImage.Contains("ge_email") ? preImage.GetAttributeValue<string>("ge_email") : null;
                oldAppColoumn.Add(oldAppEmail);

            }
            else
            {
                tracingService.Trace("PreImage is not available.");
            }

            // Retrieve target entity values
            tracingService.Trace("Processing Target entity values...");
            Entity entity = (Entity)context.InputParameters["Target"];
            string AppName = entity.GetAttributeValue<string>("ge_applicationid");
            int? AppClass = entity.GetAttributeValue<int?>("ge_noclass");
            string AppPhone = entity.GetAttributeValue<string>("ge_phone");
            string AppEmail = entity.GetAttributeValue<string>("ge_email");
            string AppId = entity.GetAttributeValue<string>("ge_applicationidd");

            List<object> checkUpdAppCol = new List<object>
            {
                AppName,
                AppClass,
                AppPhone,
                AppEmail,
            };
            List<object> logicalName = new List<object>
            {
                "ge_applicationid",
                "ge_noclass",
                "ge_phone",
                "ge_email",
                "ge_applicationidd"
            };

            // Compare oldAppColoumn and checkUpdAppCol values
            tracingService.Trace("Comparing PreImage values with Target values...");
            for (int i = 0; i < oldAppColoumn.Count; i++)
            {
                if (i < checkUpdAppCol.Count)
                {
                    var oldValue = oldAppColoumn[i];
                    var newValue = checkUpdAppCol[i];
                    var InslogicaName = logicalName[i];

                    if (oldValue == null && newValue == null)
                    {
                        tracingService.Trace($"Index {i}: Both old and new values are null.");
                    }
                    else if (oldValue != null && newValue != null && oldValue.Equals(newValue))
                    {
                        tracingService.Trace($"Index {i}: Values match. Old value: {oldValue}, New value: {newValue}");
                    }
                    else
                    {
                        tracingService.Trace($"Index {i}: Values differ. Old value: {oldValue}, New value: {newValue}");

                        // Create backup entity for differing values
                        Entity BackupEntity = new Entity("ge_backupstudent");
                        BackupEntity["ge_backupstudentno"] = AppId; // Unique identifier for the backup

                        // Assign specific field value to ge_name
                        //BackupEntity["ge_name"] = entity.Contains("ge_fieldname") ? entity["ge_fieldname"].ToString() : null;
                        BackupEntity["ge_name"] = InslogicaName;
                        BackupEntity["ge_oldvalue"] = oldValue?.ToString(); // Convert old value to string
                        BackupEntity["ge_newvalue"] = newValue?.ToString(); // Convert new value to string

                        // Create the backup entity
                        service.Create(BackupEntity);
                        tracingService.Trace($"Backup created for index {i}: Old value - {oldValue}, New value - {newValue}");
                    }
                }
                else
                {
                    tracingService.Trace($"Index {i}: No corresponding Target value to compare.");
                }
            }
        }
    }
}
