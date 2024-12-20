using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViManagement
{
    public class CreateOf : IPlugin
    {

        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var service = factory.CreateOrganizationService(context.UserId);
            var tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            Entity entity = (Entity)context.InputParameters["Target"];

            string name = entity.GetAttributeValue<string>("ge_applicationid");
            string email = entity.GetAttributeValue<string>("ge_email");
            string phone = entity.GetAttributeValue<string>("ge_phone");
            decimal? percentage = entity.GetAttributeValue<decimal?>("ge_percentage");
            tracingService.Trace($"percentage - {percentage}");
            EntityReference geclass = entity.GetAttributeValue<EntityReference>("ge_class");

            var classdetails = service.Retrieve("ge_classm", geclass.Id, new ColumnSet(true));
            var teacher = classdetails.GetAttributeValue<EntityReference>("ge_teacher");
            DateTime? dob = entity.GetAttributeValue<DateTime?>("ge_dob");
            tracingService.Trace($"dob - {dob}");
            int? age = null;
            if (dob.HasValue)
            {
                DateTime currentDate = DateTime.Today;
                 age = currentDate.Year - dob.Value.Year;

                if (currentDate < dob.Value.AddYears(age.Value))
                {
                    age--;
                }

                tracingService.Trace($"Calculated age - {age}");
                tracingService.Trace($"Calculated age value - {age.Value}");

            }

            if (percentage.Value >= 50 && age.Value >= 12)
            {
                entity["ge_status"] = new OptionSetValue(122700000);
                service.Update(entity);
                Entity stu = new Entity("ge_students");
                stu["ge_studentid"] = name;
                stu["ge_email"] = email;
                stu["ge_phone"] = phone;
                stu["ge_teacher"] = new EntityReference("ge_teachers", teacher.Id);
                service.Create(stu);

            }
            else
            {
                entity["ge_status"] = new OptionSetValue(122700001);
                service.Update(entity);
            }
        }
    }
}
