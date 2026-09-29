using QuinyxRepo.QuinyxReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuinyxRepo.Entity
{

    public class Absence
    {

        #region Init


        public Absence()
        {

        }

        public Absence(ScheduleV2 a)
        {
            this.AbsenceId = $"S-{a.scheduleId}";
            this.FromDate = a.begDate;
            this.ToDate = a.endDate;
            this.Code = a.leaveExtCode;
            this.ABadgeNo = a.badgeNo;
        }

        public Absence(LeaveApplicationV2 a)
        {
            this.AbsenceId = $"A-{a.id}";
            this.FromDate = a.fromDate;
            this.ToDate = a.toDate;
            this.Code = a.extCode;
            this.ABadgeNo = a.fromBadgeNo;
            this.SickLevel = a.sickLevel.ToString();
            this.EstHours = a.estHours.ToString();
            this.FromPersonId = a.fromPersId;
            this.ToPersonId = a.toPersId ?? 0;
            this.SendDate = a.senddate;
            this.ts = a.ts;
            this.approvedByManager = a.approvedByManager;
            this.beenViewed = a.beenViewed;
        }

        #endregion



        #region Properties



        public string ABadgeNo { get; set; }

        public string Name { get; set; }

        public string AbsenceId { get; set; }

        public string Code { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime SendDate { get; set; }

        public string SickLevel { get; set; }

        public string EstHours { get; set; }

        public int FromPersonId { get; set; }

        public int ToPersonId { get; set; }

        //from emp
        public int costCentre { get; set; }
        public string costCentreExtCode { get; set; }

        public string UnitName { get; set; }

        public string UnitExtId { get; set; }

        public int FullEmploymentHrs { get; set; }

        public string EmploymentPercent { get; set; }

        public string sex { get; set; }

        public string TemplateName { get; set; }

        public string TemplateExtId { get; set; }


        public bool approvedByManager { get; set; }

        public int beenViewed { get; set; }

        public DateTime ts { get; set; }
        #endregion
    }
}
