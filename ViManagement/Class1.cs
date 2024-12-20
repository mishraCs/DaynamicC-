using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;

namespace ViManagement
{
    public class Class1 : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var service = factory.CreateOrganizationService(context.UserId);
            var tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            tracingService.Trace("Application creat plugin started");
            try
            {
                Entity entity = (Entity)context.InputParameters["Target"];
                ProcessEntity(entity, service, tracingService, context);
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException($"An error occurred in the plugin: {ex.Message}");
            }
        }

        private void ProcessEntity(Entity entity, IOrganizationService service, ITracingService tracingService, IPluginExecutionContext context)
        {
            string name = entity.GetAttributeValue<string>("ge_applicationid");
            EntityReference geclass = entity.GetAttributeValue<EntityReference>("ge_class");
            string email = entity.GetAttributeValue<string>("ge_email");
            string phone = entity.GetAttributeValue<string>("ge_phone");
            decimal? percentage = entity.GetAttributeValue<decimal?>("ge_percentage");
            DateTime? dob = entity.GetAttributeValue<DateTime?>("ge_dob");

            int age = CalculateAge(dob);

            if (IsValidApplicant(percentage, age))
            {
                entity["ge_status"] = new OptionSetValue(122700000);
                CreateStudentRecord(service, tracingService, name, geclass, email, phone, context);
            }
            else
            {
                entity["ge_status"] = new OptionSetValue(122700001);
            }
        }

        private int CalculateAge(DateTime? dob)
        {
            if (!dob.HasValue) return 0;

            int age = DateTime.Now.Year - dob.Value.Year;
            if (DateTime.Now < dob.Value.AddYears(age)) age--;
            return age;
        }

        private bool IsValidApplicant(decimal? percentage, int age)
        {
            return percentage.HasValue && percentage.Value >= 50 && age >= 12;
        }

        private void CreateStudentRecord(
            IOrganizationService service,
            ITracingService tracingService,
            string name,
            EntityReference geclass,
            string email,
            string phone,
            IPluginExecutionContext context)
        {
            Entity studentEntity = new Entity("ge_students");

            if (!string.IsNullOrEmpty(name))
            {
                studentEntity["ge_studentid"] = name;
            }

            if (geclass != null)
            {
                studentEntity["ge_class"] = geclass;

                EntityReference teacherReference = RetrieveTeacherReference(service, tracingService, geclass, context);
                if (teacherReference != null)
                {
                    studentEntity["ge_teacher"] = teacherReference;
                }
            }

            if (!string.IsNullOrEmpty(email))
            {
                studentEntity["ge_email"] = email;
            }

            if (!string.IsNullOrEmpty(phone))
            {
                studentEntity["ge_phone"] = phone;
            }

            service.Create(studentEntity);
        }

        private EntityReference RetrieveTeacherReference(
            IOrganizationService service,
            ITracingService tracingService,
            EntityReference geclass,
            IPluginExecutionContext context)
        {
            if (geclass != null)
            {
                var classdetails = service.Retrieve("ge_classm", geclass.Id, new ColumnSet(true));
                var teacher = classdetails.GetAttributeValue<EntityReference>("ge_teacher").Id;
                EntityReference teacherLookup = new EntityReference("ge_teachers", teacher);
                tracingService.Trace($"The lookup id of teacher {teacherLookup}");
                SendEmail(teacherLookup, tracingService, service, context);
                return teacherLookup;
            }
            else
            {
                tracingService.Trace("Unable to Getting data using geclass to teacher.");
                return null;
            }
        }
        private void SendEmail(EntityReference teacherLookup, ITracingService tracingService, IOrganizationService service, IPluginExecutionContext context)
        {
            if (teacherLookup == null)
            {
                tracingService.Trace("Teacher lookup is null.");
                throw new InvalidPluginExecutionException("Teacher reference is missing.");
            }
            tracingService.Trace($"Fetching email for teacher with ID: {teacherLookup.Id}");
            Entity teacherEntity = service.Retrieve(teacherLookup.LogicalName, teacherLookup.Id, new ColumnSet("ge_email"));
            if (!teacherEntity.Contains("ge_email"))
            {
                tracingService.Trace("Teacher does not have an email address.");
                throw new InvalidPluginExecutionException("Teacher email address is missing.");
            }
            Entity targetEntity = service.Retrieve(teacherEntity.LogicalName, teacherEntity.Id, new ColumnSet(true));
            var teacherEmail = targetEntity.GetAttributeValue<string>("ge_email");
            //EntityReference ownerRef = new EntityReference("ge_teachers", ownerId);
            Entity toParty = new Entity("activityparty");
            toParty["partyid"] = new EntityReference(teacherEntity.LogicalName, teacherLookup.Id);
            Entity email = new Entity("email");
            email["subject"] = $"Hello akash ";
            email["description"] = "hello";
            email["directioncode"] = true;
            email["from"] = new Entity[] { new Entity("activityparty") { ["addressused"] = teacherEmail } };
            email["to"] = new Entity[] { new Entity("activityparty") { ["addressused"] = "ownerRef" } };
            Guid emailId = service.Create(email);
            tracingService.Trace($" Guid {emailId}");
        }
    }
}