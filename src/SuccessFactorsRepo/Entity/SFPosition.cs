using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuccessFactorsRepo.Entity
{
    class SFPosition
    {
    }



    public class SFPositions
    {
        public SFPositionD d { get; set; }
    }

    public class SFPositionD
    {
        public SFPositionResult[] results { get; set; }
    }

    public class SFPositionResult
    {
        public string code { get; set; }
        public DateTime effectiveStartDate { get; set; }
        public string businessUnit { get; set; }
        public DateTime lastModifiedDateTime { get; set; }
        public string jobTitle { get; set; }
        public object criticality { get; set; }
        public DateTime createdDateTime { get; set; }
        public string jobCode { get; set; }
        public string type { get; set; }
        public object incumbent { get; set; }
        public string division { get; set; }
        public string cust_employeeGroup { get; set; }
        public object payRange { get; set; }
        public string regularTemporary { get; set; }
        public string costCenter { get; set; }
        public string standardHours { get; set; }
        public object legacyPositionId { get; set; }
        public string externalName_localized { get; set; }
        public string mdfSystemRecordStatus { get; set; }
        public bool vacant { get; set; }
        public string effectiveStatus { get; set; }
        public object technicalParameters { get; set; }
        public string externalName_en_GB { get; set; }
        public DateTime effectiveEndDate { get; set; }
        public object positionCriticality { get; set; }
        public object positionTitle { get; set; }
        public string cust_JobFunction { get; set; }
        public object description { get; set; }
        public string externalName_defaultValue { get; set; }
        public bool positionControlled { get; set; }
        public object payGrade { get; set; }
        public string company { get; set; }
        public string department { get; set; }
        public object employeeClass { get; set; }
        public object creationSource { get; set; }
        public object changeReason { get; set; }
        public string targetFTE { get; set; }
        public string lastModifiedBy { get; set; }
        public object jobLevel { get; set; }
        public string transactionSequence { get; set; }
        public string createdBy { get; set; }
        public string mdfSystemOptimisticLockUUID { get; set; }
        public object comment { get; set; }
        public string location { get; set; }
        public bool multipleIncumbentsAllowed { get; set; }
        public string externalName_en_US { get; set; }

        public string externalName_nb_NO { get; set; }

    }





}
