using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuccessFactorsRepo.Entity
{
    class SFEmpEmployment
    {
    }




    public class SFEmpEmployments
    {
        public SFEmpEmploymentD d { get; set; }
    }

    public class SFEmpEmploymentD
    {
        public SFEmpEmploymentResult[] results { get; set; }
        public string __next { get; set; }
    }

    public class SFEmpEmploymentResult
    {
        public string personIdExternal { get; set; }
        public string userId { get; set; }
        public bool hiringNotCompleted { get; set; }
        public bool isECRecord { get; set; }
        public DateTime lastModifiedDateTime { get; set; }
        public DateTime? endDate { get; set; }
        public DateTime createdDateTime { get; set; }
        public bool isContingentWorker { get; set; }
        public DateTime createdOn { get; set; }
        public DateTime? lastDateWorked { get; set; }
        public DateTime? originalStartDate { get; set; }
        public object customDate1 { get; set; }
        public string assignmentClass { get; set; }
        public string lastModifiedBy { get; set; }
        public string assignmentIdExternal { get; set; }
        public DateTime lastModifiedOn { get; set; }
        public string createdBy { get; set; }
        public DateTime startDate { get; set; }

        public Jobinfonav jobInfoNav { get; set; }

        public WorkOrderNav workOrderNav { get; set; }

    }






    public class Jobinfonav
    {
        public JonInfoResult1[] results { get; set; }
    }

    public class JonInfoResult1
    {
        public DateTime startDate { get; set; }

        public string employmentType { get; set; }
        public Employmenttypenav employmentTypeNav { get; set; }
        public Employeeclassnav employeeClassNav { get; set; }
    }

    public class Employmenttypenav
    {
        public string externalCode { get; set; }
        public Picklistlabels picklistLabels { get; set; }
    }

    public class Picklistlabels
    {
        public PickResult[] results { get; set; }
    }

    public class PickResult
    {
        public string locale { get; set; }
        public string label { get; set; }
    }



    public class Employeeclassnav
    {
        public string externalCode { get; set; }
        public Picklistlabels picklistLabels { get; set; }
    }



    public class WorkOrderNav
    {
        public WorkOrderResult[] results { get; set; }
    }

    public class WorkOrderResult
    {
        public Vendornav vendorNav { get; set; }
    }

    public class Vendornav
    {
        public string vendorName { get; set; }
    }

 

}
