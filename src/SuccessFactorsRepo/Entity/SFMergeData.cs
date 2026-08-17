using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuccessFactorsRepo.Entity
{

    public class ExportData
    {
        public string Anchor { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class MUResult
    {
        public MetadataU __metadata { get; set; }
        public string username { get; set; }
    }



    public class MEResult
    {
        public Metadata __metadata { get; set; }
        //public string emailType { get; set; } = "1412";
        //public string personIdExternal { get; set; } = string.Empty;
        public string emailAddress { get; set; } = string.Empty;
        public bool isPrimary { get; set; } = true;
    }


    public class Metadata
    {
        public string uri { get; set; }
        public string type { get; set; } = "SFOData.PerEmail";
    }

    public class MetadataU
    {
        public string uri { get; set; }
 
    }



    public class Result
    {
        public __Metadata __metadata { get; set; }
        public string userId { get; set; }
        public DateTime lastModifiedWithTZ { get; set; }
        public string custom03 { get; set; }
        public string status { get; set; }
        public string lastName { get; set; }
        public string displayName { get; set; }
        public string email { get; set; }
        public string assignmentUUID { get; set; }
        public string defaultFullName { get; set; }
        public string totalTeamSize { get; set; }
        public DateTime lastModifiedDateTime { get; set; }
        public DateTime sciLastModified { get; set; }
        public string firstName { get; set; }
        public DateTime lastModified { get; set; }
        public string title { get; set; }
        public string department { get; set; }
        public bool isPrimaryAssignment { get; set; }
        public string username { get; set; }
        public Delegatorofautodelegateconfignav delegatorOfAutoDelegateConfigNav { get; set; }
        public Incumbentofpositionnav incumbentOfPositionNav { get; set; }
        public Externalcodeofcust_Care_Testnav externalCodeOfcust_Care_TestNav { get; set; }
        public Headofunitoffobusinessunitnav headOfUnitOfFOBusinessUnitNav { get; set; }
        public Cust_Userofcust_Care_Notenav cust_userOfcust_Care_NoteNav { get; set; }
        public Externalcodeofcust_Timesheetstatusnav externalCodeOfcust_timesheetStatusNav { get; set; }
        public Externalcodeofcust_Benefits_Parent_V2nav externalCodeOfcust_Benefits_Parent_v2Nav { get; set; }
        public Subjectuseridofachievementnav subjectUserIdOfAchievementNav { get; set; }
        public Useridoftimeaccountsnapshotnav userIdOfTimeAccountSnapshotNav { get; set; }
        public Useridofexternaltimerecordnav userIdOfExternalTimeRecordNav { get; set; }
        public Userssysidofemployeedatareplicationnotificationnav usersSysIdOfEmployeeDataReplicationNotificationNav { get; set; }
        public Useridoftemporarytimeinformationnav userIdOfTemporaryTimeInformationNav { get; set; }
        public Useridofcust_Profileupdatednav userIdOfcust_ProfileUpdatedNav { get; set; }
        public Participantuserid1ofonboardingmeetingeventnav participantUserId1OfOnboardingMeetingEventNav { get; set; }
        public Targetidoftimemanagementalertnav targetIdOfTimeManagementAlertNav { get; set; }
        public Userofcust_Securityclearancenav userOfcust_securityClearanceNav { get; set; }
        public Cust_Userofcust_Care_Periodnav cust_userOfcust_Care_PeriodNav { get; set; }
        public Useridofworkschedulenav userIdOfWorkScheduleNav { get; set; }
        public Participantuserid2ofonboardingmeetingeventnav participantUserId2OfOnboardingMeetingEventNav { get; set; }
        public Useridoftimecollectornav userIdOfTimeCollectorNav { get; set; }
        public Participantuserid3ofonboardingmeetingeventnav participantUserId3OfOnboardingMeetingEventNav { get; set; }
        public Personkeynav personKeyNav { get; set; }
        public Userssysidofemployeedatareplicationconfirmationnav usersSysIdOfEmployeeDataReplicationConfirmationNav { get; set; }
        public Useridofemployeetimegroupnav userIdOfEmployeeTimeGroupNav { get; set; }
        public Useridofemployeetimesheetnav userIdOfEmployeeTimeSheetNav { get; set; }
        public Useridofexternalallowancenav userIdOfExternalAllowanceNav { get; set; }
        public Processoridofonboardingcandidateinfonav processorIdOfOnboardingCandidateInfoNav { get; set; }
        public Useridofonboardingcandidateinfonav userIdOfOnboardingCandidateInfoNav { get; set; }
        public Usersysidofworkordernav userSysIdOfWorkOrderNav { get; set; }
        public Competencyratingnav competencyRatingNav { get; set; }
        public Concurrentuserofonboardingcandidateinfonav concurrentUserOfOnboardingCandidateInfoNav { get; set; }
        public Useridofaccrualcalculationbasenav userIdOfAccrualCalculationBaseNav { get; set; }
        public Useridofhrischangelogdatareplicationnav userIdOfHRISChangeLogDataReplicationNav { get; set; }
        public Useridofcust_Workingtimealertnav userIdOfcust_WorkingTimeAlertNav { get; set; }
        public Userpermissionsnav userPermissionsNav { get; set; }
        public Userofcust_Alert_Work_Ordernav userOfcust_Alert_Work_OrderNav { get; set; }
        public Assigneeuseridoftimemanagementalertnav assigneeUserIdOfTimeManagementAlertNav { get; set; }
        public Auditusersysidofonetimedeductionnav auditUserSysIdOfOneTimeDeductionNav { get; set; }
        public Useridofexternaltimedatanav userIdOfExternalTimeDataNav { get; set; }
        public Workorderowneridofworkordernav workOrderOwnerIdOfWorkOrderNav { get; set; }
        public Subjectuseridofactivitynav subjectUserIdOfActivityNav { get; set; }
        public Userssysidofempcostdistributionnav usersSysIdOfEmpCostDistributionNav { get; set; }
        public Costcentermanageroffocostcenternav costCenterManagerOfFOCostCenterNav { get; set; }
        public Useridofdatareplicationproxynav userIdOfDataReplicationProxyNav { get; set; }
        public Employmentidentityofdrtmpurgefreezenav employmentIdentityOfDRTMPurgeFreezeNav { get; set; }
        public Userofcust_Exit_Detailsnav userOfcust_exit_detailsNav { get; set; }
        public Usersysidofonetimedeductionnav userSysIdOfOneTimeDeductionNav { get; set; }
        public Usersysidofrecurringdeductionnav userSysIdOfRecurringDeductionNav { get; set; }
        public Workerofpaymentinformationv3nav workerOfPaymentInformationV3Nav { get; set; }
        public Useridofemployeetimenav userIdOfEmployeeTimeNav { get; set; }
        public Empinfo empInfo { get; set; }
        public Userssysidofsecondaryassignmentsitemnav usersSysIdOfSecondaryAssignmentsItemNav { get; set; }
        public Useridofemployeepayrollrunresultsnav userIdOfEmployeePayrollRunResultsNav { get; set; }
        public Cust_Userofcust_Care_Formnav cust_UserOfcust_Care_FormNav { get; set; }
        public Useridoftimeaccountnav userIdOfTimeAccountNav { get; set; }
        public Useridofbudgetgroupnav userIdOfBudgetGroupNav { get; set; }
        public Externalcodeofcust_Carefollowupnav externalCodeOfcust_careFollowUpNav { get; set; }
        public Manageridofonboardingcandidateinfonav managerIdOfOnboardingCandidateInfoNav { get; set; }
        public Cust_User_Writerofcust_Care_Notenav cust_user_writerOfcust_Care_NoteNav { get; set; }
        public Hrmanageridofonboardingcandidateinfonav hrManagerIdOfOnboardingCandidateInfoNav { get; set; }
        public Headofunitoffodepartmentnav headOfUnitOfFODepartmentNav { get; set; }
        public Userssysidofemployeedatareplicationelementnav usersSysIdOfEmployeeDataReplicationElementNav { get; set; }
        public Participantuserid4ofonboardingmeetingeventnav participantUserId4OfOnboardingMeetingEventNav { get; set; }
        public Headofunitoffodivisionnav headOfUnitOfFODivisionNav { get; set; }
        public Participantuserid5ofonboardingmeetingeventnav participantUserId5OfOnboardingMeetingEventNav { get; set; }
        public Assigneeuseridofdomaineventalertnav assigneeUserIdOfDomainEventAlertNav { get; set; }
        public Userofcust_Birthdaynav userOfcust_BirthDayNav { get; set; }
        public Useridofpositionrighttoreturnnav userIdOfPositionRightToReturnNav { get; set; }
        public Delegateeofautodelegatedetailnav delegateeOfAutoDelegateDetailNav { get; set; }
        public Userofcust_Anniversarynav userOfcust_AnniversaryNav { get; set; }
        public Userofcust_Taxadvisornav userOfcust_taxadvisorNav { get; set; }
        public Userssysidofhiredatechangenav usersSysIdOfHireDateChangeNav { get; set; }
    }

    public class __Metadata
    {
        public string uri { get; set; }
        public string type { get; set; }
    }

    public class Delegatorofautodelegateconfignav
    {
        public __Deferred __deferred { get; set; }
    }

    public class __Deferred
    {
        public string uri { get; set; }
    }

    public class Incumbentofpositionnav
    {
        public __Deferred1 __deferred { get; set; }
    }

    public class __Deferred1
    {
        public string uri { get; set; }
    }

    public class Externalcodeofcust_Care_Testnav
    {
        public __Deferred2 __deferred { get; set; }
    }

    public class __Deferred2
    {
        public string uri { get; set; }
    }

    public class Headofunitoffobusinessunitnav
    {
        public __Deferred3 __deferred { get; set; }
    }

    public class __Deferred3
    {
        public string uri { get; set; }
    }

    public class Cust_Userofcust_Care_Notenav
    {
        public __Deferred4 __deferred { get; set; }
    }

    public class __Deferred4
    {
        public string uri { get; set; }
    }

    public class Externalcodeofcust_Timesheetstatusnav
    {
        public __Deferred5 __deferred { get; set; }
    }

    public class __Deferred5
    {
        public string uri { get; set; }
    }

    public class Externalcodeofcust_Benefits_Parent_V2nav
    {
        public __Deferred6 __deferred { get; set; }
    }

    public class __Deferred6
    {
        public string uri { get; set; }
    }

    public class Subjectuseridofachievementnav
    {
        public __Deferred7 __deferred { get; set; }
    }

    public class __Deferred7
    {
        public string uri { get; set; }
    }

    public class Useridoftimeaccountsnapshotnav
    {
        public __Deferred8 __deferred { get; set; }
    }

    public class __Deferred8
    {
        public string uri { get; set; }
    }

    public class Useridofexternaltimerecordnav
    {
        public __Deferred9 __deferred { get; set; }
    }

    public class __Deferred9
    {
        public string uri { get; set; }
    }

    public class Userssysidofemployeedatareplicationnotificationnav
    {
        public __Deferred10 __deferred { get; set; }
    }

    public class __Deferred10
    {
        public string uri { get; set; }
    }

    public class Useridoftemporarytimeinformationnav
    {
        public __Deferred11 __deferred { get; set; }
    }

    public class __Deferred11
    {
        public string uri { get; set; }
    }

    public class Useridofcust_Profileupdatednav
    {
        public __Deferred12 __deferred { get; set; }
    }

    public class __Deferred12
    {
        public string uri { get; set; }
    }

    public class Participantuserid1ofonboardingmeetingeventnav
    {
        public __Deferred13 __deferred { get; set; }
    }

    public class __Deferred13
    {
        public string uri { get; set; }
    }

    public class Targetidoftimemanagementalertnav
    {
        public __Deferred14 __deferred { get; set; }
    }

    public class __Deferred14
    {
        public string uri { get; set; }
    }

    public class Userofcust_Securityclearancenav
    {
        public __Deferred15 __deferred { get; set; }
    }

    public class __Deferred15
    {
        public string uri { get; set; }
    }

    public class Cust_Userofcust_Care_Periodnav
    {
        public __Deferred16 __deferred { get; set; }
    }

    public class __Deferred16
    {
        public string uri { get; set; }
    }

    public class Useridofworkschedulenav
    {
        public __Deferred17 __deferred { get; set; }
    }

    public class __Deferred17
    {
        public string uri { get; set; }
    }

    public class Participantuserid2ofonboardingmeetingeventnav
    {
        public __Deferred18 __deferred { get; set; }
    }

    public class __Deferred18
    {
        public string uri { get; set; }
    }

    public class Useridoftimecollectornav
    {
        public __Deferred19 __deferred { get; set; }
    }

    public class __Deferred19
    {
        public string uri { get; set; }
    }

    public class Participantuserid3ofonboardingmeetingeventnav
    {
        public __Deferred20 __deferred { get; set; }
    }

    public class __Deferred20
    {
        public string uri { get; set; }
    }

    public class Personkeynav
    {
        public __Deferred21 __deferred { get; set; }
    }

    public class __Deferred21
    {
        public string uri { get; set; }
    }

    public class Userssysidofemployeedatareplicationconfirmationnav
    {
        public __Deferred22 __deferred { get; set; }
    }

    public class __Deferred22
    {
        public string uri { get; set; }
    }

    public class Useridofemployeetimegroupnav
    {
        public __Deferred23 __deferred { get; set; }
    }

    public class __Deferred23
    {
        public string uri { get; set; }
    }

    public class Useridofemployeetimesheetnav
    {
        public __Deferred24 __deferred { get; set; }
    }

    public class __Deferred24
    {
        public string uri { get; set; }
    }

    public class Useridofexternalallowancenav
    {
        public __Deferred25 __deferred { get; set; }
    }

    public class __Deferred25
    {
        public string uri { get; set; }
    }

    public class Processoridofonboardingcandidateinfonav
    {
        public __Deferred26 __deferred { get; set; }
    }

    public class __Deferred26
    {
        public string uri { get; set; }
    }

    public class Useridofonboardingcandidateinfonav
    {
        public __Deferred27 __deferred { get; set; }
    }

    public class __Deferred27
    {
        public string uri { get; set; }
    }

    public class Usersysidofworkordernav
    {
        public __Deferred28 __deferred { get; set; }
    }

    public class __Deferred28
    {
        public string uri { get; set; }
    }

    public class Competencyratingnav
    {
        public __Deferred29 __deferred { get; set; }
    }

    public class __Deferred29
    {
        public string uri { get; set; }
    }

    public class Concurrentuserofonboardingcandidateinfonav
    {
        public __Deferred30 __deferred { get; set; }
    }

    public class __Deferred30
    {
        public string uri { get; set; }
    }

    public class Useridofaccrualcalculationbasenav
    {
        public __Deferred31 __deferred { get; set; }
    }

    public class __Deferred31
    {
        public string uri { get; set; }
    }

    public class Useridofhrischangelogdatareplicationnav
    {
        public __Deferred32 __deferred { get; set; }
    }

    public class __Deferred32
    {
        public string uri { get; set; }
    }

    public class Useridofcust_Workingtimealertnav
    {
        public __Deferred33 __deferred { get; set; }
    }

    public class __Deferred33
    {
        public string uri { get; set; }
    }

    public class Userpermissionsnav
    {
        public __Deferred34 __deferred { get; set; }
    }

    public class __Deferred34
    {
        public string uri { get; set; }
    }

    public class Userofcust_Alert_Work_Ordernav
    {
        public __Deferred35 __deferred { get; set; }
    }

    public class __Deferred35
    {
        public string uri { get; set; }
    }

    public class Assigneeuseridoftimemanagementalertnav
    {
        public __Deferred36 __deferred { get; set; }
    }

    public class __Deferred36
    {
        public string uri { get; set; }
    }

    public class Auditusersysidofonetimedeductionnav
    {
        public __Deferred37 __deferred { get; set; }
    }

    public class __Deferred37
    {
        public string uri { get; set; }
    }

    public class Useridofexternaltimedatanav
    {
        public __Deferred38 __deferred { get; set; }
    }

    public class __Deferred38
    {
        public string uri { get; set; }
    }

    public class Workorderowneridofworkordernav
    {
        public __Deferred39 __deferred { get; set; }
    }

    public class __Deferred39
    {
        public string uri { get; set; }
    }

    public class Subjectuseridofactivitynav
    {
        public __Deferred40 __deferred { get; set; }
    }

    public class __Deferred40
    {
        public string uri { get; set; }
    }

    public class Userssysidofempcostdistributionnav
    {
        public __Deferred41 __deferred { get; set; }
    }

    public class __Deferred41
    {
        public string uri { get; set; }
    }

    public class Costcentermanageroffocostcenternav
    {
        public __Deferred42 __deferred { get; set; }
    }

    public class __Deferred42
    {
        public string uri { get; set; }
    }

    public class Useridofdatareplicationproxynav
    {
        public __Deferred43 __deferred { get; set; }
    }

    public class __Deferred43
    {
        public string uri { get; set; }
    }

    public class Employmentidentityofdrtmpurgefreezenav
    {
        public __Deferred44 __deferred { get; set; }
    }

    public class __Deferred44
    {
        public string uri { get; set; }
    }

    public class Userofcust_Exit_Detailsnav
    {
        public __Deferred45 __deferred { get; set; }
    }

    public class __Deferred45
    {
        public string uri { get; set; }
    }

    public class Usersysidofonetimedeductionnav
    {
        public __Deferred46 __deferred { get; set; }
    }

    public class __Deferred46
    {
        public string uri { get; set; }
    }

    public class Usersysidofrecurringdeductionnav
    {
        public __Deferred47 __deferred { get; set; }
    }

    public class __Deferred47
    {
        public string uri { get; set; }
    }

    public class Workerofpaymentinformationv3nav
    {
        public __Deferred48 __deferred { get; set; }
    }

    public class __Deferred48
    {
        public string uri { get; set; }
    }

    public class Useridofemployeetimenav
    {
        public __Deferred49 __deferred { get; set; }
    }

    public class __Deferred49
    {
        public string uri { get; set; }
    }

    public class Empinfo
    {
        public __Deferred50 __deferred { get; set; }
    }

    public class __Deferred50
    {
        public string uri { get; set; }
    }

    public class Userssysidofsecondaryassignmentsitemnav
    {
        public __Deferred51 __deferred { get; set; }
    }

    public class __Deferred51
    {
        public string uri { get; set; }
    }

    public class Useridofemployeepayrollrunresultsnav
    {
        public __Deferred52 __deferred { get; set; }
    }

    public class __Deferred52
    {
        public string uri { get; set; }
    }

    public class Cust_Userofcust_Care_Formnav
    {
        public __Deferred53 __deferred { get; set; }
    }

    public class __Deferred53
    {
        public string uri { get; set; }
    }

    public class Useridoftimeaccountnav
    {
        public __Deferred54 __deferred { get; set; }
    }

    public class __Deferred54
    {
        public string uri { get; set; }
    }

    public class Useridofbudgetgroupnav
    {
        public __Deferred55 __deferred { get; set; }
    }

    public class __Deferred55
    {
        public string uri { get; set; }
    }

    public class Externalcodeofcust_Carefollowupnav
    {
        public __Deferred56 __deferred { get; set; }
    }

    public class __Deferred56
    {
        public string uri { get; set; }
    }

    public class Manageridofonboardingcandidateinfonav
    {
        public __Deferred57 __deferred { get; set; }
    }

    public class __Deferred57
    {
        public string uri { get; set; }
    }

    public class Cust_User_Writerofcust_Care_Notenav
    {
        public __Deferred58 __deferred { get; set; }
    }

    public class __Deferred58
    {
        public string uri { get; set; }
    }

    public class Hrmanageridofonboardingcandidateinfonav
    {
        public __Deferred59 __deferred { get; set; }
    }

    public class __Deferred59
    {
        public string uri { get; set; }
    }

    public class Headofunitoffodepartmentnav
    {
        public __Deferred60 __deferred { get; set; }
    }

    public class __Deferred60
    {
        public string uri { get; set; }
    }

    public class Userssysidofemployeedatareplicationelementnav
    {
        public __Deferred61 __deferred { get; set; }
    }

    public class __Deferred61
    {
        public string uri { get; set; }
    }

    public class Participantuserid4ofonboardingmeetingeventnav
    {
        public __Deferred62 __deferred { get; set; }
    }

    public class __Deferred62
    {
        public string uri { get; set; }
    }

    public class Headofunitoffodivisionnav
    {
        public __Deferred63 __deferred { get; set; }
    }

    public class __Deferred63
    {
        public string uri { get; set; }
    }

    public class Participantuserid5ofonboardingmeetingeventnav
    {
        public __Deferred64 __deferred { get; set; }
    }

    public class __Deferred64
    {
        public string uri { get; set; }
    }

    public class Assigneeuseridofdomaineventalertnav
    {
        public __Deferred65 __deferred { get; set; }
    }

    public class __Deferred65
    {
        public string uri { get; set; }
    }

    public class Userofcust_Birthdaynav
    {
        public __Deferred66 __deferred { get; set; }
    }

    public class __Deferred66
    {
        public string uri { get; set; }
    }

    public class Useridofpositionrighttoreturnnav
    {
        public __Deferred67 __deferred { get; set; }
    }

    public class __Deferred67
    {
        public string uri { get; set; }
    }

    public class Delegateeofautodelegatedetailnav
    {
        public __Deferred68 __deferred { get; set; }
    }

    public class __Deferred68
    {
        public string uri { get; set; }
    }

    public class Userofcust_Anniversarynav
    {
        public __Deferred69 __deferred { get; set; }
    }

    public class __Deferred69
    {
        public string uri { get; set; }
    }

    public class Userofcust_Taxadvisornav
    {
        public __Deferred70 __deferred { get; set; }
    }

    public class __Deferred70
    {
        public string uri { get; set; }
    }

    public class Userssysidofhiredatechangenav
    {
        public __Deferred71 __deferred { get; set; }
    }

    public class __Deferred71
    {
        public string uri { get; set; }
    }


}
