using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuccessFactorsRepo.Entity
{

    public class SFUsers
    {
        public SFUserD d { get; set; }
    }

    public class SFUserD
    {
        public SFUserResult[] results { get; set; }
        public string __next { get; set; }
    }

    public class SFUserResult
    {
        public DateTime? dateOfBirth { get; set; }
        public DateTime? hireDate { get; set; }
        public DateTime? lastModified { get; set; }
        public DateTime? lastModifiedDateTime { get; set; }

        public string businessPhone { get; set; }
        public string cellPhone { get; set; }

        public string email { get; set; }

        public string jobTitle { get; set; }


        public string addressLine1 { get; set; }
        public string city { get; set; }
        public string competency { get; set; }
        public string country { get; set; }
        public string custom01 { get; set; }
        public string custom02 { get; set; }
        public string custom03 { get; set; }
        public string custom04 { get; set; }
        public string custom05 { get; set; }
        public string custom06 { get; set; }
        public string custom07 { get; set; }
        public string custom08 { get; set; }
        public string custom09 { get; set; }
        public string custom10 { get; set; }
        public string defaultFullName { get; set; }
        public string defaultLocale { get; set; }
        public string department { get; set; }
        public string division { get; set; }
        public string empId { get; set; }
        public string firstName { get; set; }
        public string gender { get; set; }
        public string jobCode { get; set; }
        public string lastName { get; set; }
        public string location { get; set; }
        public string loginMethod { get; set; }
        public string nationality { get; set; }

        public string state { get; set; }
        public string status { get; set; }
        public string timeZone { get; set; }
        public string title { get; set; }
        public string userId { get; set; }

        public string totalTeamSize { get; set; }
        public string username { get; set; }
        public string zipCode { get; set; }
    }

    public class SFUserResultOrg
    {
        public DateTime? dateOfBirth { get; set; }
        public DateTime? hireDate { get; set; }
        public DateTime? lastModified { get; set; }
        public DateTime? lastModifiedDateTime { get; set; }
        public DateTime? lastModifiedWithTZ { get; set; }
        public int teamMembersSize { get; set; }
        public string addressLine2 { get; set; }
        public string addressLine3 { get; set; }
        public string benchStrength { get; set; }
        public string bonusBudgetAmount { get; set; }
        public string bonusTarget { get; set; }
        public string businessPhone { get; set; }
        public string businessSegment { get; set; }
        public string cellPhone { get; set; }
        public string citizenship { get; set; }
        public string compensationBonusEligible { get; set; }
        public string compensationEligible { get; set; }
        public string compensationReadOnly { get; set; }
        public string compensationSalaryEligible { get; set; }
        public string compensationSalaryRateType { get; set; }
        public string compensationSalaryRateUnits { get; set; }
        public string compensationStockEligible { get; set; }
        public string criticalTalentComments { get; set; }
        public string dateOfCurrentPosition { get; set; }
        public string dateOfPosition { get; set; }
        public string email { get; set; }
        public string employeeClass { get; set; }
        public string ethnicity { get; set; }
        public string fax { get; set; }
        public string finalJobCode { get; set; }
        public string finalJobFamily { get; set; }
        public string finalJobRole { get; set; }
        public string futureLeader { get; set; }
        public string homePhone { get; set; }
        public string impactOfLoss { get; set; }
        public string impactOfLossComments { get; set; }
        public string issueComments { get; set; }
        public string jobFamily { get; set; }
        public string jobLevel { get; set; }
        public string jobRole { get; set; }
        public string jobTitle { get; set; }
        public string keyPosition { get; set; }
        public string lastReviewDate { get; set; }
        public string level { get; set; }
        public string localCurrencyCode { get; set; }
        public string lumpsum2Target { get; set; }
        public string lumpsumTarget { get; set; }
        public string married { get; set; }
        public string matrix1Label { get; set; }
        public string matrix2Label { get; set; }
        public string matrixManaged { get; set; }
        public string meritEffectiveDate { get; set; }
        public string meritTarget { get; set; }
        public string mi { get; set; }
        public string minority { get; set; }
        public string newToPosition { get; set; }
        public string nickname { get; set; }
        public string onboardingId { get; set; }
        public string origHireDate { get; set; }
        public string payGrade { get; set; }
        public string promotionAmount { get; set; }
        public string raiseProrating { get; set; }
        public string reasonForLeaving { get; set; }
        public string reloComments { get; set; }
        public string reloLocation { get; set; }
        public string reloWilling { get; set; }
        public string reviewFreq { get; set; }
        public string riskOfLoss { get; set; }
        public string salary { get; set; }
        public string salaryBudgetExtra2Percentage { get; set; }
        public string salaryBudgetExtraPercentage { get; set; }
        public string salaryBudgetFinalSalaryPercentage { get; set; }
        public string salaryBudgetLumpsumPercentage { get; set; }
        public string salaryBudgetMeritPercentage { get; set; }
        public string salaryBudgetPromotionPercentage { get; set; }
        public string salaryBudgetTotalRaisePercentage { get; set; }
        public string salaryLocal { get; set; }
        public string salaryProrating { get; set; }
        public string salutation { get; set; }
        public string sciLastModified { get; set; }
        public string seatingChart { get; set; }
        public string serviceDate { get; set; }
        public string ssn { get; set; }
        public string stockBudgetOptionAmount { get; set; }
        public string stockBudgetOther1Amount { get; set; }
        public string stockBudgetOther2Amount { get; set; }
        public string stockBudgetOther3Amount { get; set; }
        public string stockBudgetStockAmount { get; set; }
        public string stockBudgetUnitAmount { get; set; }
        public string suffix { get; set; }
        public string sysCostOfSource { get; set; }
        public string sysSource { get; set; }
        public string sysStartingSalary { get; set; }
        public string talentPool { get; set; }
        public string veteranDisabled { get; set; }
        public string veteranMedal { get; set; }
        public string veteranProtected { get; set; }
        public string veteranSeparated { get; set; }
        public string addressLine1 { get; set; }
        public string city { get; set; }
        public string competency { get; set; }
        public string country { get; set; }
        public string custom01 { get; set; }
        public string custom02 { get; set; }
        public string custom03 { get; set; }
        public string custom04 { get; set; }
        public string custom05 { get; set; }
        public string custom06 { get; set; }
        public string custom07 { get; set; }
        public string custom08 { get; set; }
        public string custom09 { get; set; }
        public string custom10 { get; set; }
        public string defaultFullName { get; set; }
        public string defaultLocale { get; set; }
        public string department { get; set; }
        public string division { get; set; }
        public string empId { get; set; }
        public string firstName { get; set; }
        public string gender { get; set; }
        public string jobCode { get; set; }
        public string lastName { get; set; }
        public string location { get; set; }
        public string loginMethod { get; set; }
        public string nationality { get; set; }
        public string objective { get; set; }
        public string performance { get; set; }
        public string potential { get; set; }
        public string state { get; set; }
        public string status { get; set; }
        public string timeZone { get; set; }
        public string title { get; set; }
        public string totalTeamSize { get; set; }
        public string userId { get; set; }
        public string username { get; set; }
        public string zipCode { get; set; }
    }
}
