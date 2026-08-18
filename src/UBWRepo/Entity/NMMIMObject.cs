using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UBWRepo.Entity
{
    public class NMMIMObject
    {
        #region Init


        public NMMIMObject() { }

        public NMMIMObject(UBWEntity u, List<UBWAttribute> attrs)
        {
            this.EmployeeId = u.personId;
            this.GivenName = u.nameInformation.firstName;
            this.SurName = u.nameInformation.surname;
            this.BirthDate = u.birthDate.ToString("yyyy MM dd");
            this.EmployeeType = u.employments.FirstOrDefault(x => x.employmentStartDate < DateTime.Now && x.employmentEndDate > DateTime.Now)?.employmentType;
            this.HireDate = u.dateFrom.ToString("yyyy MM dd");
            this.EndDate = u.dateTo.ToString("yyyyMMdd");
            this.Department = GetAttr(u.relatedValues.FirstOrDefault(x => x.dateFrom < DateTime.Now && x.dateTo > DateTime.Now && x.relationName == "AVDELING"), attrs);
            this.Company = GetAttr(u.relatedValues.FirstOrDefault(x => x.dateFrom < DateTime.Now && x.dateTo > DateTime.Now && x.relationName == "ARBSTED"), attrs);
            this.Title = u.employments.FirstOrDefault(x => x.employmentStartDate < DateTime.Now && x.employmentEndDate > DateTime.Now)?.employmentDescription;
            this.Mobile = FixPhone(u.contactPoints.FirstOrDefault()?.phoneNumbers.telephone4);
            this.Telephone = FixPhone(u.contactPoints.FirstOrDefault()?.phoneNumbers.telephone1);
            this.PostalCode = u.contactPoints.FirstOrDefault()?.address.postcode;
            this.PostalPlace = u.contactPoints.FirstOrDefault()?.address.place;
            this.StreetAddress = u.contactPoints.FirstOrDefault()?.address.streetAddress;
            this.Email = u.contactPoints.FirstOrDefault()?.additionalContactInfo.eMail;
        }

        public NMMIMObject(UBWUser u, List<UBWEntity> employees)
        {
            var mainEmp = u.employments.FirstOrDefault(x => x.startDate < DateTime.Now && x.endDate > DateTime.Now && x.isMainPosition);
            this.EmployeeId = u.personId;
            this.GivenName = u.nameInformation.firstName;
            this.DisplayName = u.name;
            this.SurName = u.nameInformation.surname;
            this.BirthDate = u.birthDate.ToString("yyyyMMdd");
            this.EmployeeType = u.relatedValues.FirstOrDefault(x => x.dateFrom < DateTime.Now && x.dateTo > DateTime.Now && x.relationName == "RESSTYPE")?.description;
            this.HireDate = u.dateFrom.ToString("yyyyMMdd");
            this.EndDate = u.dateTo.ToString("yyyyMMdd");
            this.Department = DesValue(u.relatedValues.FirstOrDefault(x => x.dateFrom < DateTime.Now && x.dateTo > DateTime.Now && x.relationName == "AVDELING"));
            this.Company = u.relatedValues.FirstOrDefault(x => x.dateFrom < DateTime.Now && x.dateTo > DateTime.Now && x.relationName == "ARBSTED")?.description;
            this.Title = u.relatedValues.FirstOrDefault(x => x.dateFrom < DateTime.Now && x.dateTo > DateTime.Now && x.relationName == "STILLNR")?.description;
            this.Mobile = FixPhone(u.contactPoints.FirstOrDefault()?.phoneNumbers.telephone4);
            this.Telephone = FixPhone(u.contactPoints.FirstOrDefault()?.phoneNumbers.telephone1);
            this.PostalCode = u.contactPoints.FirstOrDefault()?.address.postcode;
            this.PostalPlace = u.contactPoints.FirstOrDefault()?.address.place;
            this.StreetAddress = u.contactPoints.FirstOrDefault()?.address.streetAddress;
            var manId = mainEmp?.relatedValues.FirstOrDefault(x => x.dateFrom < DateTime.Now && x.dateTo > DateTime.Now && x.relationName == "ORGNIVA4")?.relatedValue;
            if (manId != null)
            {
                this.Manager = manId;
                this.ManagerName = employees.FirstOrDefault(x => x.personId == manId)?.personName;
            }
            this.Email = u.contactPoints.FirstOrDefault()?.additionalContactInfo.eMail;
        }

        private string DesValue(UBaseRelatedvalue re)
        {
            if (re != null)
            {
                return $"{re.description} ({re.relatedValue})";
            }
            return "";
        }

        private string GetAttr(Relatedvalue rel, List<UBWAttribute> attrs)
        {
            if (rel != null)
            {
                var val = attrs.FirstOrDefault(x => x.attributeId == rel.relationId && x.attributeValue == rel.relatedValue);
                if (val != null)
                {
                    return val.description;
                }
                return "";
            }
            return "";
        }

        private string GetUAttr(UBaseRelatedvalue rel, List<UBWAttribute> attrs)
        {
            if (rel != null)
            {
                var val = attrs.FirstOrDefault(x => x.attributeId == rel.relationId && x.attributeValue == rel.relatedValue);
                if (val != null)
                {
                    return val.description;
                }
                return "";
            }
            return "";
        }

        private string FixPhone(string phone)
        {
            var retPhone = phone;
            if (phone.Length == 10)
            {
                retPhone = $"+{phone}";
                return retPhone;
            }
            if (phone.Length == 8)
            {
                retPhone = $"+47{phone}";
                return retPhone;
            }

            return retPhone;
        }

        #endregion

        #region Properties

        public string BirthDate { get; set; }
        public string Company { get; set; }
        public string Department { get; set; }
        public string DisplayName { get; set; }

        public string EmployeeId { get; set; }
        public string EmployeeType { get; set; }
        public string GivenName { get; set; }
        public string SurName { get; set; }
        public string HireDate { get; set; }

        public string EndDate { get; set; }

        public string Manager { get; set; }
        public string ManagerName { get; set; }
        public string Mobile { get; set; }
        public string PostalCode { get; set; }
        public string PostalPlace { get; set; }
        public string StreetAddress { get; set; }
        public string Telephone { get; set; }
        public string Title { get; set; }

        public string Email { get; set; }
        #endregion
    }
}
