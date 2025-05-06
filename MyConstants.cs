using System.Security.Claims;

namespace Quorom
{
    public class MyConstants
    {
        public static class ChallengeTypeGuids
        {
            public const string Accountability = "89733f5b-c28a-43b2-9dc2-14466e5a37b5";
            public const string Ambiguity = "3992af8b-fffa-4461-ba9b-7bb6b0d93d32";
            public const string Budget = "109b6f41-e097-4b86-9fec-b50d5d8c41cc";
            public const string Communication = "94d94923-3a98-46f1-a4c2-cf04f4833dde";
            public const string Conflict = "7d19481f-6bcc-41ae-8345-ca2ce0aef87b";
            public const string Competency = "86821e87-4ff8-4f36-9d26-da6e5ea31db3";
            public const string Deadline = "584220b7-26e4-4a9e-8333-e86bfea55794";
            public const string Goal = "15946387-7783-480d-b7e6-c7e1729c4bd6";
            public const string Resource = "5877bd14-a5f3-42d3-957f-2f44661d48f4";
            public const string Risk = "89a2201f-5e96-4adb-b6d6-1b2a2091f6a5";
            public const string ScopeCreep = "be252507-8103-43df-b7d3-2f5ea1bc4d46";
            public const string Scheduling = "2701c191-a3f4-4d01-8382-2ab7820d67cf";
            public const string Stakeholder = "2ee65cee-fd07-45f1-83ef-347c64da6ef5";
            public const string Technology = "3adb434e-7cb6-447b-af2d-2834a69d0f74";
        }
        public static class ChallengeTypeNames
        {
            public const string Accountability = "Accountability";
            public const string Ambiguity = "Ambiguity";
            public const string Budget = "Budget";
            public const string Communication = "Communication";
            public const string Conflict = "Conflict";
            public const string Competency = "Competency";
            public const string Deadline = "Deadline";
            public const string Goal = "Goal";
            public const string Resource = "Resource";
            public const string Risk = "Risk";
            public const string ScopeCreep = "Scope Creep";
            public const string Scheduling = "Scheduling";
            public const string Stakeholder = "Stakeholder";
            public const string Technology = "Technology";
        }
        public static class ClaimStore
        {
            public static List<Claim> claimList =
                [
                new Claim("Create" , "Create"),
                new Claim("Update" , "Update"),
                new Claim("Delete" , "Delete"),
                ];
        }
        public static class Copyright
        {
            public const string CopyrightString = "Quorom 2025. An iDea of Enterprise. All rights reserved.";
        }
        public static class DbTables
        {
            public const string Challenge = "Challenge";
            public const string ChallengeType = "ChallengeTypes";
            public const string Initiative = "Initiatives";
            public const string InitiativeTask = "InitiativeTasks";
            public const string InitiativeType = "InitiativeTypes";
            public const string NotificationGroup = "NotificationGroups";
            public const string NotificationGroupQuoromite = "NotificationGroupQuoromites";
            public const string Quoromite = "Quoromites";
            public const string Task = "Tasks";
            public const string TaskChallenge = "TaskChallenges";
            public const string TaskType = "TaskTypes";
            public const string Login = "Login";
        }
        public static class InitiaiveTypeGuids
        {
            public const string Education = "3c1fbfa6-b888-41af-9527-c9a2a968911b";
            public const string Environment = "dbec15a2-001d-4134-85f8-8b8fa1f93615";
            public const string Financial = "e5789a21-bbec-4926-990c-2949ecc316ae";
            public const string Health = "e538bef6-b817-488c-b104-79873b006fcf";
            public const string Housing = "013d47cc-bc96-402a-8629-fb0edc1a9e5e";
            public const string Policy = "d2348d99-f8a7-41ef-a076-5521b0c64ddd";
            public const string Security = "4259e1ed-654c-4f68-add2-372dd5c0d9ca";
            public const string Strategic = "2d136539-d9e7-467a-9348-1eaf1e1ba625";
        }
        public static class InitiaiveTypeNames
        {
            public const string Education = "Education";
            public const string Environment = "Environment";
            public const string Financial = "Financial";
            public const string Health = "Health";
            public const string Housing = "Health";
            public const string Policy = "Policy";
            public const string Security = "Security";
            public const string Strategic = "Strategic";
        }
        public static class Messages
        {
            public const string Success = "Success";
            public const string Information = "Information";
            public const string Warning = "Warning";
            public const string Error = "Error";
        }
        public static class PolicyName
        {
            public const string Admin = "Admin";
            public const string AdminAndUser = "AdminAndUser";
            public const string AdminRole_CreateClaim = "AdminRole_CreateClaim";
            public const string AdminRole_CreateUpdateDeleteClaim = "AdminRole_CreateUpdateDeleteClaim";
        }
        public static class ProcessType
        {
            public const string AddRecord = "[A] Add Record [A]";
            public const string ReadRecord = "[R] Read Record [R]";
            public const string UpdateRecord = "[U] Update Record [U]";
            public const string DeleteRecordSoft = "[DS] User Record Deletion [DS]";
            public const string DeleteRecord = "[D] Permanent Record Deletion [D]";
            public const string Login = "[L] Successful Login [L]";
        }
        public static class QuoromRoleGuids
        {
            public const string Administrator = "e6fbee75-1155-4cb4-b02b-d4f88aadbdc9";
            public const string Contributer = "cadd478a-f9af-48a7-b330-cf2be2d8e09f";
            public const string Deleter = "8a900325-ec69-4329-bb61-77b5af5c9c1b";
            public const string Modifier = "0a4a7818-ea98-48fe-84cf-fca6a1e6a117";
            public const string SuperUser = "2129e86c-2306-46d5-9cfe-9eb91ba351be";
            public const string Viewer = "a3f97e34-b7cc-4e5f-8c97-1fb1e96738e7r";
        }
        public static class QuoromRoleNames
        {
            public const string Administrator = "Administrator";
            public const string Contributer = "Contributor";
            public const string Deleter = "Deleter";
            public const string Modifier = "Modifier";
            public const string SuperUser = "SuperUser";
            public const string Viewer = "Viewer";
        }
        public static class QuoromSuperAdmin
        {
            public const string Id = "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5";
            public const string Email = "nkosi.alexander@gov.tt";
            public const string Password = "|0<kTh3mD0wn";
        }
        public static class Statuses
        {
            public const string Opened = "Opened";
            public const string InProgress = "In Progress";
            public const string Completed = "Completed";
        }
        public static class SubStatuses
        {
            public const string EarlyStart = "EarlyStart";
            public const string NoActivity = "No Activity";
            public const string OnTime = "On Time";
            public const string LateStart = "Late Start";
            public const string Overdue = "Over Due";
            public const string Archive = "Archive";
        }
        public static class TaskTypeGuids
        {
            public const string Approval = "410b8c41-fea2-481d-b129-ae4fe6e04c08";
            public const string Coordinated = "e7642e73-5b67-4c8e-b1bb-aab4043375bb";
            public const string Dependency = "9baba539-467d-4668-b2bf-2d8bb9dacf40";
            public const string FollowUp = "8cbb73c4-e357-4593-a157-248dd0039f3a";
            public const string Incidental = "3843a8a3-5242-494b-97f6-e5f8bef8dac6";
            public const string Planned = "fb74d606-84f4-4812-aa8b-07a49d0a94fc";
        }
        public static class TaskTypeNames
        {
            public const string Approval = "Approval";
            public const string Coordinated = "Coordinated";
            public const string Dependency = "Dependency";
            public const string FollowUp = "Follow-Up";
            public const string Incidental = "Incidental";
            public const string Planned = "Planned";
        }
    }
}
