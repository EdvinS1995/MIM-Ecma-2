namespace VismaHRMRepo.Entity
{

    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class personsXML
    {

        private personsXMLPerson[] personField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("person")]
        public personsXMLPerson[] person
        {
            get
            {
                return this.personField;
            }
            set
            {
                this.personField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPerson
    {

        private personsXMLPersonAuthentication authenticationField;

        private personsXMLPersonCareerInfo careerInfoField;

        private personsXMLPersonContactInfo contactInfoField;

        private System.DateTime dateOfBirthField;

        private personsXMLPersonDependent[] dependentsField;

        private personsXMLPersonEmployments employmentsField;

        private string familyNameField;

        private string genderCodeField;

        private string givenNameField;

        private personsXMLPersonHrmAuthentication[] hrmAuthenticationsField;

        private System.DateTime lastChangedDateField;

        private string maritalStatusField;

        private personsXMLPersonMunicipality municipalityField;

        private string nationalityCodeField;

        private personsXMLPersonPostalAddress postalAddressField;

        private personsXMLPersonSocialSecurityOffice socialSecurityOfficeField;

        private string ssnField;

        private bool ssnFieldSpecified;

        private string personIdHRMField;

        /// <remarks/>
        public personsXMLPersonAuthentication authentication
        {
            get
            {
                return this.authenticationField;
            }
            set
            {
                this.authenticationField = value;
            }
        }

        /// <remarks/>
        public personsXMLPersonCareerInfo careerInfo
        {
            get
            {
                return this.careerInfoField;
            }
            set
            {
                this.careerInfoField = value;
            }
        }

        /// <remarks/>
        public personsXMLPersonContactInfo contactInfo
        {
            get
            {
                return this.contactInfoField;
            }
            set
            {
                this.contactInfoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime dateOfBirth
        {
            get
            {
                return this.dateOfBirthField;
            }
            set
            {
                this.dateOfBirthField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("dependent", IsNullable = false)]
        public personsXMLPersonDependent[] dependents
        {
            get
            {
                return this.dependentsField;
            }
            set
            {
                this.dependentsField = value;
            }
        }

        /// <remarks/>
        public personsXMLPersonEmployments employments
        {
            get
            {
                return this.employmentsField;
            }
            set
            {
                this.employmentsField = value;
            }
        }

        /// <remarks/>
        public string familyName
        {
            get
            {
                return this.familyNameField;
            }
            set
            {
                this.familyNameField = value;
            }
        }

        /// <remarks/>
        public string genderCode
        {
            get
            {
                return this.genderCodeField;
            }
            set
            {
                this.genderCodeField = value;
            }
        }

        /// <remarks/>
        public string givenName
        {
            get
            {
                return this.givenNameField;
            }
            set
            {
                this.givenNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("hrmAuthentication", IsNullable = false)]
        public personsXMLPersonHrmAuthentication[] hrmAuthentications
        {
            get
            {
                return this.hrmAuthenticationsField;
            }
            set
            {
                this.hrmAuthenticationsField = value;
            }
        }

        /// <remarks/>
        public System.DateTime lastChangedDate
        {
            get
            {
                return this.lastChangedDateField;
            }
            set
            {
                this.lastChangedDateField = value;
            }
        }

        /// <remarks/>
        public string maritalStatus
        {
            get
            {
                return this.maritalStatusField;
            }
            set
            {
                this.maritalStatusField = value;
            }
        }

        /// <remarks/>
        public personsXMLPersonMunicipality municipality
        {
            get
            {
                return this.municipalityField;
            }
            set
            {
                this.municipalityField = value;
            }
        }

        /// <remarks/>
        public string nationalityCode
        {
            get
            {
                return this.nationalityCodeField;
            }
            set
            {
                this.nationalityCodeField = value;
            }
        }

        /// <remarks/>
        public personsXMLPersonPostalAddress postalAddress
        {
            get
            {
                return this.postalAddressField;
            }
            set
            {
                this.postalAddressField = value;
            }
        }

        /// <remarks/>
        public personsXMLPersonSocialSecurityOffice socialSecurityOffice
        {
            get
            {
                return this.socialSecurityOfficeField;
            }
            set
            {
                this.socialSecurityOfficeField = value;
            }
        }

        /// <remarks/>
        public string ssn
        {
            get
            {
                return this.ssnField;
            }
            set
            {
                this.ssnField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ssnSpecified
        {
            get
            {
                return this.ssnFieldSpecified;
            }
            set
            {
                this.ssnFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string personIdHRM
        {
            get
            {
                return this.personIdHRMField;
            }
            set
            {
                this.personIdHRMField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonAuthentication
    {

        private string initialsField;

        private string userIdField;

        private string usernameField;

        private string[] aliasField;

        /// <remarks/>
        public string initials
        {
            get
            {
                return this.initialsField;
            }
            set
            {
                this.initialsField = value;
            }
        }

        /// <remarks/>
        public string userId
        {
            get
            {
                return this.userIdField;
            }
            set
            {
                this.userIdField = value;
            }
        }

        /// <remarks/>
        public string username
        {
            get
            {
                return this.usernameField;
            }
            set
            {
                this.usernameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("alias")]
        public string[] alias
        {
            get
            {
                return this.aliasField;
            }
            set
            {
                this.aliasField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonCareerInfo
    {

        private string jobTitleField;

        private string workTasksField;

        /// <remarks/>
        public string jobTitle
        {
            get
            {
                return this.jobTitleField;
            }
            set
            {
                this.jobTitleField = value;
            }
        }

        /// <remarks/>
        public string workTasks
        {
            get
            {
                return this.workTasksField;
            }
            set
            {
                this.workTasksField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonContactInfo
    {

        private string emailField;

        private string mobilePhoneField;

        private string privatePhoneField;

        private string workPhoneField;

        private bool paycheckByEmailField;

        private string privateEmailField;

        private string privateMobilePhoneField;

        /// <remarks/>
        public string email
        {
            get
            {
                return this.emailField;
            }
            set
            {
                this.emailField = value;
            }
        }

        /// <remarks/>
        public string mobilePhone
        {
            get
            {
                return this.mobilePhoneField;
            }
            set
            {
                this.mobilePhoneField = value;
            }
        }

        /// <remarks/>
        public string privatePhone
        {
            get
            {
                return this.privatePhoneField;
            }
            set
            {
                this.privatePhoneField = value;
            }
        }

        /// <remarks/>
        public string workPhone
        {
            get
            {
                return this.workPhoneField;
            }
            set
            {
                this.workPhoneField = value;
            }
        }

        /// <remarks/>
        public bool paycheckByEmail
        {
            get
            {
                return this.paycheckByEmailField;
            }
            set
            {
                this.paycheckByEmailField = value;
            }
        }

        /// <remarks/>
        public string privateEmail
        {
            get
            {
                return this.privateEmailField;
            }
            set
            {
                this.privateEmailField = value;
            }
        }

        /// <remarks/>
        public string privateMobilePhone
        {
            get
            {
                return this.privateMobilePhoneField;
            }
            set
            {
                this.privateMobilePhoneField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonDependent
    {

        private bool aloneWithChildField;

        private System.DateTime birthYearField;

        private bool birthYearFieldSpecified;

        private bool chronicallyIllField;

        private personsXMLPersonDependentContactInfo contactInfoField;

        private string nameField;

        private personsXMLPersonDependentPostalAddress postalAddressField;

        private bool primaryField;

        private string relationField;

        /// <remarks/>
        public bool aloneWithChild
        {
            get
            {
                return this.aloneWithChildField;
            }
            set
            {
                this.aloneWithChildField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime birthYear
        {
            get
            {
                return this.birthYearField;
            }
            set
            {
                this.birthYearField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool birthYearSpecified
        {
            get
            {
                return this.birthYearFieldSpecified;
            }
            set
            {
                this.birthYearFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool chronicallyIll
        {
            get
            {
                return this.chronicallyIllField;
            }
            set
            {
                this.chronicallyIllField = value;
            }
        }

        /// <remarks/>
        public personsXMLPersonDependentContactInfo contactInfo
        {
            get
            {
                return this.contactInfoField;
            }
            set
            {
                this.contactInfoField = value;
            }
        }

        /// <remarks/>
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public personsXMLPersonDependentPostalAddress postalAddress
        {
            get
            {
                return this.postalAddressField;
            }
            set
            {
                this.postalAddressField = value;
            }
        }

        /// <remarks/>
        public bool primary
        {
            get
            {
                return this.primaryField;
            }
            set
            {
                this.primaryField = value;
            }
        }

        /// <remarks/>
        public string relation
        {
            get
            {
                return this.relationField;
            }
            set
            {
                this.relationField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonDependentContactInfo
    {

        private string emailField;

        private string mobilePhoneField;

        private string privatePhoneField;

        private string workPhoneField;

        /// <remarks/>
        public string email
        {
            get
            {
                return this.emailField;
            }
            set
            {
                this.emailField = value;
            }
        }

        /// <remarks/>
        public string mobilePhone
        {
            get
            {
                return this.mobilePhoneField;
            }
            set
            {
                this.mobilePhoneField = value;
            }
        }

        /// <remarks/>
        public string privatePhone
        {
            get
            {
                return this.privatePhoneField;
            }
            set
            {
                this.privatePhoneField = value;
            }
        }

        /// <remarks/>
        public string workPhone
        {
            get
            {
                return this.workPhoneField;
            }
            set
            {
                this.workPhoneField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonDependentPostalAddress
    {

        private string address1Field;

        private string address2Field;

        private string countryCodeField;

        private string postalAreaField;

        private string postalCodeField;

        /// <remarks/>
        public string address1
        {
            get
            {
                return this.address1Field;
            }
            set
            {
                this.address1Field = value;
            }
        }

        /// <remarks/>
        public string address2
        {
            get
            {
                return this.address2Field;
            }
            set
            {
                this.address2Field = value;
            }
        }

        /// <remarks/>
        public string countryCode
        {
            get
            {
                return this.countryCodeField;
            }
            set
            {
                this.countryCodeField = value;
            }
        }

        /// <remarks/>
        public string postalArea
        {
            get
            {
                return this.postalAreaField;
            }
            set
            {
                this.postalAreaField = value;
            }
        }

        /// <remarks/>
        public string postalCode
        {
            get
            {
                return this.postalCodeField;
            }
            set
            {
                this.postalCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonEmployments
    {

        private personsXMLPersonEmploymentsEmployment employmentField;

        /// <remarks/>
        public personsXMLPersonEmploymentsEmployment employment
        {
            get
            {
                return this.employmentField;
            }
            set
            {
                this.employmentField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonEmploymentsEmployment
    {

        private personsXMLPersonEmploymentsEmploymentBankDetails bankDetailsField;

        private personsXMLPersonEmploymentsEmploymentCategory categoryField;

        private personsXMLPersonEmploymentsEmploymentCompany companyField;

        private string employeeIdField;

        private decimal employmentPercentageField;

        private System.DateTime endDateField;

        private bool endDateFieldSpecified;

        private System.DateTime lastChangedDateField;

        private System.DateTime lastEmployeedField;

        private bool lastEmployeedFieldSpecified;

        private decimal paymentInAdvanceField;

        private personsXMLPersonEmploymentsEmploymentPension pensionField;

        private personsXMLPersonEmploymentsEmploymentPosition[] positionsField;

        private System.DateTime preferentialDateField;

        private bool preferentialDateFieldSpecified;

        private System.DateTime startDateField;

        private object statisticsField;

        private personsXMLPersonEmploymentsEmploymentTaxDetails taxDetailsField;

        private personsXMLPersonEmploymentsEmploymentUnion unionField;

        /// <remarks/>
        public personsXMLPersonEmploymentsEmploymentBankDetails bankDetails
        {
            get
            {
                return this.bankDetailsField;
            }
            set
            {
                this.bankDetailsField = value;
            }
        }

        /// <remarks/>
        public personsXMLPersonEmploymentsEmploymentCategory category
        {
            get
            {
                return this.categoryField;
            }
            set
            {
                this.categoryField = value;
            }
        }

        /// <remarks/>
        public personsXMLPersonEmploymentsEmploymentCompany company
        {
            get
            {
                return this.companyField;
            }
            set
            {
                this.companyField = value;
            }
        }

        /// <remarks/>
        public string employeeId
        {
            get
            {
                return this.employeeIdField;
            }
            set
            {
                this.employeeIdField = value;
            }
        }

        /// <remarks/>
        public decimal employmentPercentage
        {
            get
            {
                return this.employmentPercentageField;
            }
            set
            {
                this.employmentPercentageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime endDate
        {
            get
            {
                return this.endDateField;
            }
            set
            {
                this.endDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool endDateSpecified
        {
            get
            {
                return this.endDateFieldSpecified;
            }
            set
            {
                this.endDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public System.DateTime lastChangedDate
        {
            get
            {
                return this.lastChangedDateField;
            }
            set
            {
                this.lastChangedDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime lastEmployeed
        {
            get
            {
                return this.lastEmployeedField;
            }
            set
            {
                this.lastEmployeedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool lastEmployeedSpecified
        {
            get
            {
                return this.lastEmployeedFieldSpecified;
            }
            set
            {
                this.lastEmployeedFieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal paymentInAdvance
        {
            get
            {
                return this.paymentInAdvanceField;
            }
            set
            {
                this.paymentInAdvanceField = value;
            }
        }

        /// <remarks/>
        public personsXMLPersonEmploymentsEmploymentPension pension
        {
            get
            {
                return this.pensionField;
            }
            set
            {
                this.pensionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("position", IsNullable = false)]
        public personsXMLPersonEmploymentsEmploymentPosition[] positions
        {
            get
            {
                return this.positionsField;
            }
            set
            {
                this.positionsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime preferentialDate
        {
            get
            {
                return this.preferentialDateField;
            }
            set
            {
                this.preferentialDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool preferentialDateSpecified
        {
            get
            {
                return this.preferentialDateFieldSpecified;
            }
            set
            {
                this.preferentialDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime startDate
        {
            get
            {
                return this.startDateField;
            }
            set
            {
                this.startDateField = value;
            }
        }

        /// <remarks/>
        public object statistics
        {
            get
            {
                return this.statisticsField;
            }
            set
            {
                this.statisticsField = value;
            }
        }

        /// <remarks/>
        public personsXMLPersonEmploymentsEmploymentTaxDetails taxDetails
        {
            get
            {
                return this.taxDetailsField;
            }
            set
            {
                this.taxDetailsField = value;
            }
        }

        /// <remarks/>
        public personsXMLPersonEmploymentsEmploymentUnion union
        {
            get
            {
                return this.unionField;
            }
            set
            {
                this.unionField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonEmploymentsEmploymentBankDetails
    {

        private string accountNumberField;

        private string aliasField;

        private string bankCodeField;

        private string iBANField;

        private string nationalityCodeField;

        private string swiftField;

        /// <remarks/>
        public string accountNumber
        {
            get
            {
                return this.accountNumberField;
            }
            set
            {
                this.accountNumberField = value;
            }
        }

        /// <remarks/>
        public string alias
        {
            get
            {
                return this.aliasField;
            }
            set
            {
                this.aliasField = value;
            }
        }

        /// <remarks/>
        public string bankCode
        {
            get
            {
                return this.bankCodeField;
            }
            set
            {
                this.bankCodeField = value;
            }
        }

        /// <remarks/>
        public string IBAN
        {
            get
            {
                return this.iBANField;
            }
            set
            {
                this.iBANField = value;
            }
        }

        /// <remarks/>
        public string nationalityCode
        {
            get
            {
                return this.nationalityCodeField;
            }
            set
            {
                this.nationalityCodeField = value;
            }
        }

        /// <remarks/>
        public string swift
        {
            get
            {
                return this.swiftField;
            }
            set
            {
                this.swiftField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonEmploymentsEmploymentCategory
    {

        private string descriptionField;

        private string idField;

        /// <remarks/>
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonEmploymentsEmploymentCompany
    {

        private string companyIdField;

        private string companyNameField;

        private string organizationNumberField;

        /// <remarks/>
        public string companyId
        {
            get
            {
                return this.companyIdField;
            }
            set
            {
                this.companyIdField = value;
            }
        }

        /// <remarks/>
        public string companyName
        {
            get
            {
                return this.companyNameField;
            }
            set
            {
                this.companyNameField = value;
            }
        }

        /// <remarks/>
        public string organizationNumber
        {
            get
            {
                return this.organizationNumberField;
            }
            set
            {
                this.organizationNumberField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonEmploymentsEmploymentPension
    {

        private string nameField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonEmploymentsEmploymentPosition
    {

        private personsXMLPersonEmploymentsEmploymentPositionBenefit[] benefitsField;

        private personsXMLPersonEmploymentsEmploymentPositionChart chartField;

        private personsXMLPersonEmploymentsEmploymentPositionCostCentres costCentresField;

        private string customIdField;

        private bool customIdFieldSpecified;

        private decimal employmentPositionPercentageField;

        private personsXMLPersonEmploymentsEmploymentPositionFixedTransaction[] fixedTransactionsField;

        private System.DateTime lastChangedDateField;

        private personsXMLPersonEmploymentsEmploymentPositionLeave leaveField;

        private personsXMLPersonEmploymentsEmploymentPositionLocation locationField;

        private string positionCategoryNumberField;

        private System.DateTime positionEndDateField;

        private bool positionEndDateFieldSpecified;

        private personsXMLPersonEmploymentsEmploymentPositionPositionInfo positionInfoField;

        private decimal positionPercentageField;

        private System.DateTime positionStartDateField;

        private personsXMLPersonEmploymentsEmploymentPositionPositionStatistics positionStatisticsField;

        private personsXMLPersonEmploymentsEmploymentPositionSalaryInfo salaryInfoField;

        private System.DateTime seniorityDateField;

        private bool seniorityDateFieldSpecified;

        private bool shiftWorkField;

        private decimal weeklyHoursField;

        private bool weeklyHoursFieldSpecified;

        private personsXMLPersonEmploymentsEmploymentPositionWorkDaysInWeek workDaysInWeekField;

        private decimal yearlyHoursField;

        private bool yearlyHoursFieldSpecified;

        private bool isPrimaryPositionField;

        private System.DateTime validFromDateField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("benefit", IsNullable = false)]
        public personsXMLPersonEmploymentsEmploymentPositionBenefit[] benefits
        {
            get
            {
                return this.benefitsField;
            }
            set
            {
                this.benefitsField = value;
            }
        }

        /// <remarks/>
        public personsXMLPersonEmploymentsEmploymentPositionChart chart
        {
            get
            {
                return this.chartField;
            }
            set
            {
                this.chartField = value;
            }
        }

        /// <remarks/>
        public personsXMLPersonEmploymentsEmploymentPositionCostCentres costCentres
        {
            get
            {
                return this.costCentresField;
            }
            set
            {
                this.costCentresField = value;
            }
        }

        /// <remarks/>
        public string customId
        {
            get
            {
                return this.customIdField;
            }
            set
            {
                this.customIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool customIdSpecified
        {
            get
            {
                return this.customIdFieldSpecified;
            }
            set
            {
                this.customIdFieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal employmentPositionPercentage
        {
            get
            {
                return this.employmentPositionPercentageField;
            }
            set
            {
                this.employmentPositionPercentageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("fixedTransaction", IsNullable = false)]
        public personsXMLPersonEmploymentsEmploymentPositionFixedTransaction[] fixedTransactions
        {
            get
            {
                return this.fixedTransactionsField;
            }
            set
            {
                this.fixedTransactionsField = value;
            }
        }

        /// <remarks/>
        public System.DateTime lastChangedDate
        {
            get
            {
                return this.lastChangedDateField;
            }
            set
            {
                this.lastChangedDateField = value;
            }
        }

        /// <remarks/>
        public personsXMLPersonEmploymentsEmploymentPositionLeave leave
        {
            get
            {
                return this.leaveField;
            }
            set
            {
                this.leaveField = value;
            }
        }

        /// <remarks/>
        public personsXMLPersonEmploymentsEmploymentPositionLocation location
        {
            get
            {
                return this.locationField;
            }
            set
            {
                this.locationField = value;
            }
        }

        /// <remarks/>
        public string positionCategoryNumber
        {
            get
            {
                return this.positionCategoryNumberField;
            }
            set
            {
                this.positionCategoryNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime positionEndDate
        {
            get
            {
                return this.positionEndDateField;
            }
            set
            {
                this.positionEndDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool positionEndDateSpecified
        {
            get
            {
                return this.positionEndDateFieldSpecified;
            }
            set
            {
                this.positionEndDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public personsXMLPersonEmploymentsEmploymentPositionPositionInfo positionInfo
        {
            get
            {
                return this.positionInfoField;
            }
            set
            {
                this.positionInfoField = value;
            }
        }

        /// <remarks/>
        public decimal positionPercentage
        {
            get
            {
                return this.positionPercentageField;
            }
            set
            {
                this.positionPercentageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime positionStartDate
        {
            get
            {
                return this.positionStartDateField;
            }
            set
            {
                this.positionStartDateField = value;
            }
        }

        /// <remarks/>
        public personsXMLPersonEmploymentsEmploymentPositionPositionStatistics positionStatistics
        {
            get
            {
                return this.positionStatisticsField;
            }
            set
            {
                this.positionStatisticsField = value;
            }
        }

        /// <remarks/>
        public personsXMLPersonEmploymentsEmploymentPositionSalaryInfo salaryInfo
        {
            get
            {
                return this.salaryInfoField;
            }
            set
            {
                this.salaryInfoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime seniorityDate
        {
            get
            {
                return this.seniorityDateField;
            }
            set
            {
                this.seniorityDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool seniorityDateSpecified
        {
            get
            {
                return this.seniorityDateFieldSpecified;
            }
            set
            {
                this.seniorityDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool shiftWork
        {
            get
            {
                return this.shiftWorkField;
            }
            set
            {
                this.shiftWorkField = value;
            }
        }

        /// <remarks/>
        public decimal weeklyHours
        {
            get
            {
                return this.weeklyHoursField;
            }
            set
            {
                this.weeklyHoursField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool weeklyHoursSpecified
        {
            get
            {
                return this.weeklyHoursFieldSpecified;
            }
            set
            {
                this.weeklyHoursFieldSpecified = value;
            }
        }

        /// <remarks/>
        public personsXMLPersonEmploymentsEmploymentPositionWorkDaysInWeek workDaysInWeek
        {
            get
            {
                return this.workDaysInWeekField;
            }
            set
            {
                this.workDaysInWeekField = value;
            }
        }

        /// <remarks/>
        public decimal yearlyHours
        {
            get
            {
                return this.yearlyHoursField;
            }
            set
            {
                this.yearlyHoursField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool yearlyHoursSpecified
        {
            get
            {
                return this.yearlyHoursFieldSpecified;
            }
            set
            {
                this.yearlyHoursFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isPrimaryPosition
        {
            get
            {
                return this.isPrimaryPositionField;
            }
            set
            {
                this.isPrimaryPositionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
        public System.DateTime validFromDate
        {
            get
            {
                return this.validFromDateField;
            }
            set
            {
                this.validFromDateField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonEmploymentsEmploymentPositionBenefit
    {

        private string codeField;

        private System.DateTime fromDateField;

        private string noteField;

        private string numberField;

        private System.DateTime toDateField;

        private bool toDateFieldSpecified;

        private decimal valueField;

        private bool valueFieldSpecified;

        /// <remarks/>
        public string code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime fromDate
        {
            get
            {
                return this.fromDateField;
            }
            set
            {
                this.fromDateField = value;
            }
        }

        /// <remarks/>
        public string note
        {
            get
            {
                return this.noteField;
            }
            set
            {
                this.noteField = value;
            }
        }

        /// <remarks/>
        public string number
        {
            get
            {
                return this.numberField;
            }
            set
            {
                this.numberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime toDate
        {
            get
            {
                return this.toDateField;
            }
            set
            {
                this.toDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool toDateSpecified
        {
            get
            {
                return this.toDateFieldSpecified;
            }
            set
            {
                this.toDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool valueSpecified
        {
            get
            {
                return this.valueFieldSpecified;
            }
            set
            {
                this.valueFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonEmploymentsEmploymentPositionChart
    {

        private personsXMLPersonEmploymentsEmploymentPositionChartUnit unitField;

        private string idField;

        private string nameField;

        /// <remarks/>
        public personsXMLPersonEmploymentsEmploymentPositionChartUnit unit
        {
            get
            {
                return this.unitField;
            }
            set
            {
                this.unitField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonEmploymentsEmploymentPositionChartUnit
    {

        private personsXMLPersonEmploymentsEmploymentPositionChartUnitManager managerField;

        private personsXMLPersonEmploymentsEmploymentPositionChartUnitRoles[] rolesField;

        private string idField;

        private string nameField;

        /// <remarks/>
        public personsXMLPersonEmploymentsEmploymentPositionChartUnitManager manager
        {
            get
            {
                return this.managerField;
            }
            set
            {
                this.managerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("roles", IsNullable = false)]
        public personsXMLPersonEmploymentsEmploymentPositionChartUnitRoles[] roles
        {
            get
            {
                return this.rolesField;
            }
            set
            {
                this.rolesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonEmploymentsEmploymentPositionChartUnitManager
    {

        private string idField;

        private string nameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonEmploymentsEmploymentPositionChartUnitRoles
    {

        private string idField;

        private string nameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonEmploymentsEmploymentPositionCostCentres
    {

        private personsXMLPersonEmploymentsEmploymentPositionCostCentresDimension1 dimension1Field;

        private personsXMLPersonEmploymentsEmploymentPositionCostCentresDimension2 dimension2Field;

        private personsXMLPersonEmploymentsEmploymentPositionCostCentresDimension3 dimension3Field;

        private personsXMLPersonEmploymentsEmploymentPositionCostCentresDimension4 dimension4Field;

        private personsXMLPersonEmploymentsEmploymentPositionCostCentresDimension5 dimension5Field;

        /// <remarks/>
        public personsXMLPersonEmploymentsEmploymentPositionCostCentresDimension1 dimension1
        {
            get
            {
                return this.dimension1Field;
            }
            set
            {
                this.dimension1Field = value;
            }
        }

        /// <remarks/>
        public personsXMLPersonEmploymentsEmploymentPositionCostCentresDimension2 dimension2
        {
            get
            {
                return this.dimension2Field;
            }
            set
            {
                this.dimension2Field = value;
            }
        }

        /// <remarks/>
        public personsXMLPersonEmploymentsEmploymentPositionCostCentresDimension3 dimension3
        {
            get
            {
                return this.dimension3Field;
            }
            set
            {
                this.dimension3Field = value;
            }
        }

        /// <remarks/>
        public personsXMLPersonEmploymentsEmploymentPositionCostCentresDimension4 dimension4
        {
            get
            {
                return this.dimension4Field;
            }
            set
            {
                this.dimension4Field = value;
            }
        }

        /// <remarks/>
        public personsXMLPersonEmploymentsEmploymentPositionCostCentresDimension5 dimension5
        {
            get
            {
                return this.dimension5Field;
            }
            set
            {
                this.dimension5Field = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonEmploymentsEmploymentPositionCostCentresDimension1
    {

        private string nameField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonEmploymentsEmploymentPositionCostCentresDimension2
    {

        private string nameField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonEmploymentsEmploymentPositionCostCentresDimension3
    {

        private string nameField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonEmploymentsEmploymentPositionCostCentresDimension4
    {

        private string nameField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonEmploymentsEmploymentPositionCostCentresDimension5
    {

        private string nameField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonEmploymentsEmploymentPositionFixedTransaction
    {

        private decimal amountField;

        private personsXMLPersonEmploymentsEmploymentPositionFixedTransactionCostCentres costCentresField;

        private decimal numberField;

        private string payCodeField;

        private decimal rateField;

        private System.DateTime toDateField;

        private bool toDateFieldSpecified;

        private personsXMLPersonEmploymentsEmploymentPositionFixedTransactionType typeField;

        private string uniqueKeyField;

        /// <remarks/>
        public decimal amount
        {
            get
            {
                return this.amountField;
            }
            set
            {
                this.amountField = value;
            }
        }

        /// <remarks/>
        public personsXMLPersonEmploymentsEmploymentPositionFixedTransactionCostCentres costCentres
        {
            get
            {
                return this.costCentresField;
            }
            set
            {
                this.costCentresField = value;
            }
        }

        /// <remarks/>
        public decimal number
        {
            get
            {
                return this.numberField;
            }
            set
            {
                this.numberField = value;
            }
        }

        /// <remarks/>
        public string payCode
        {
            get
            {
                return this.payCodeField;
            }
            set
            {
                this.payCodeField = value;
            }
        }

        /// <remarks/>
        public decimal rate
        {
            get
            {
                return this.rateField;
            }
            set
            {
                this.rateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime toDate
        {
            get
            {
                return this.toDateField;
            }
            set
            {
                this.toDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool toDateSpecified
        {
            get
            {
                return this.toDateFieldSpecified;
            }
            set
            {
                this.toDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public personsXMLPersonEmploymentsEmploymentPositionFixedTransactionType type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string uniqueKey
        {
            get
            {
                return this.uniqueKeyField;
            }
            set
            {
                this.uniqueKeyField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonEmploymentsEmploymentPositionFixedTransactionCostCentres
    {

        private personsXMLPersonEmploymentsEmploymentPositionFixedTransactionCostCentresDimension1 dimension1Field;

        private personsXMLPersonEmploymentsEmploymentPositionFixedTransactionCostCentresDimension2 dimension2Field;

        private personsXMLPersonEmploymentsEmploymentPositionFixedTransactionCostCentresDimension5 dimension5Field;

        private personsXMLPersonEmploymentsEmploymentPositionFixedTransactionCostCentresDimension4 dimension4Field;

        /// <remarks/>
        public personsXMLPersonEmploymentsEmploymentPositionFixedTransactionCostCentresDimension1 dimension1
        {
            get
            {
                return this.dimension1Field;
            }
            set
            {
                this.dimension1Field = value;
            }
        }

        /// <remarks/>
        public personsXMLPersonEmploymentsEmploymentPositionFixedTransactionCostCentresDimension2 dimension2
        {
            get
            {
                return this.dimension2Field;
            }
            set
            {
                this.dimension2Field = value;
            }
        }

        /// <remarks/>
        public personsXMLPersonEmploymentsEmploymentPositionFixedTransactionCostCentresDimension5 dimension5
        {
            get
            {
                return this.dimension5Field;
            }
            set
            {
                this.dimension5Field = value;
            }
        }

        /// <remarks/>
        public personsXMLPersonEmploymentsEmploymentPositionFixedTransactionCostCentresDimension4 dimension4
        {
            get
            {
                return this.dimension4Field;
            }
            set
            {
                this.dimension4Field = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonEmploymentsEmploymentPositionFixedTransactionCostCentresDimension1
    {

        private string nameField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonEmploymentsEmploymentPositionFixedTransactionCostCentresDimension2
    {

        private string nameField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonEmploymentsEmploymentPositionFixedTransactionCostCentresDimension5
    {

        private string nameField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonEmploymentsEmploymentPositionFixedTransactionCostCentresDimension4
    {

        private string nameField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonEmploymentsEmploymentPositionFixedTransactionType
    {

        private string nameField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonEmploymentsEmploymentPositionLeave
    {

        private System.DateTime leaveFromDateField;

        private bool leaveFromDateFieldSpecified;

        private decimal leavePercentageField;

        private bool leavePercentageFieldSpecified;

        private personsXMLPersonEmploymentsEmploymentPositionLeaveLeaveReason leaveReasonField;

        private System.DateTime leaveToDateField;

        private bool leaveToDateFieldSpecified;

        private string leaveTypeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime leaveFromDate
        {
            get
            {
                return this.leaveFromDateField;
            }
            set
            {
                this.leaveFromDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool leaveFromDateSpecified
        {
            get
            {
                return this.leaveFromDateFieldSpecified;
            }
            set
            {
                this.leaveFromDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal leavePercentage
        {
            get
            {
                return this.leavePercentageField;
            }
            set
            {
                this.leavePercentageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool leavePercentageSpecified
        {
            get
            {
                return this.leavePercentageFieldSpecified;
            }
            set
            {
                this.leavePercentageFieldSpecified = value;
            }
        }

        /// <remarks/>
        public personsXMLPersonEmploymentsEmploymentPositionLeaveLeaveReason leaveReason
        {
            get
            {
                return this.leaveReasonField;
            }
            set
            {
                this.leaveReasonField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime leaveToDate
        {
            get
            {
                return this.leaveToDateField;
            }
            set
            {
                this.leaveToDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool leaveToDateSpecified
        {
            get
            {
                return this.leaveToDateFieldSpecified;
            }
            set
            {
                this.leaveToDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string leaveType
        {
            get
            {
                return this.leaveTypeField;
            }
            set
            {
                this.leaveTypeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonEmploymentsEmploymentPositionLeaveLeaveReason
    {

        private string valueField;

        private string nameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonEmploymentsEmploymentPositionLocation
    {

        private string nameField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonEmploymentsEmploymentPositionPositionInfo
    {

        private personsXMLPersonEmploymentsEmploymentPositionPositionInfoPositionCode positionCodeField;

        private personsXMLPersonEmploymentsEmploymentPositionPositionInfoPositionType positionTypeField;

        private personsXMLPersonEmploymentsEmploymentPositionPositionInfoPublicPositionCode publicPositionCodeField;

        /// <remarks/>
        public personsXMLPersonEmploymentsEmploymentPositionPositionInfoPositionCode positionCode
        {
            get
            {
                return this.positionCodeField;
            }
            set
            {
                this.positionCodeField = value;
            }
        }

        /// <remarks/>
        public personsXMLPersonEmploymentsEmploymentPositionPositionInfoPositionType positionType
        {
            get
            {
                return this.positionTypeField;
            }
            set
            {
                this.positionTypeField = value;
            }
        }

        /// <remarks/>
        public personsXMLPersonEmploymentsEmploymentPositionPositionInfoPublicPositionCode publicPositionCode
        {
            get
            {
                return this.publicPositionCodeField;
            }
            set
            {
                this.publicPositionCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonEmploymentsEmploymentPositionPositionInfoPositionCode
    {

        private string nameField;

        private string positionCodeField;

        private string positionIdField;

        private string tableNrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string positionCode
        {
            get
            {
                return this.positionCodeField;
            }
            set
            {
                this.positionCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string positionId
        {
            get
            {
                return this.positionIdField;
            }
            set
            {
                this.positionIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tableNr
        {
            get
            {
                return this.tableNrField;
            }
            set
            {
                this.tableNrField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonEmploymentsEmploymentPositionPositionInfoPositionType
    {

        private string nameField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonEmploymentsEmploymentPositionPositionInfoPublicPositionCode
    {

        private object setField;

        private object setTypeField;

        private string valueField;
        private string nameField;


        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        public object set
        {
            get
            {
                return this.setField;
            }
            set
            {
                this.setField = value;
            }
        }

        /// <remarks/>
        public object setType
        {
            get
            {
                return this.setTypeField;
            }
            set
            {
                this.setTypeField = value;
            }
        }


    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonEmploymentsEmploymentPositionPositionStatistics
    {

        private personsXMLPersonEmploymentsEmploymentPositionPositionStatisticsBusinessNumber businessNumberField;

        private personsXMLPersonEmploymentsEmploymentPositionPositionStatisticsCompanyNumber companyNumberField;

        private personsXMLPersonEmploymentsEmploymentPositionPositionStatisticsServicePlace servicePlaceField;

        private personsXMLPersonEmploymentsEmploymentPositionPositionStatisticsWorkClassification workClassificationField;

        private personsXMLPersonEmploymentsEmploymentPositionPositionStatisticsWorkMunicipality workMunicipalityField;

        private bool includeInAAField;

        private bool includeInPAIField;

        /// <remarks/>
        public personsXMLPersonEmploymentsEmploymentPositionPositionStatisticsBusinessNumber businessNumber
        {
            get
            {
                return this.businessNumberField;
            }
            set
            {
                this.businessNumberField = value;
            }
        }

        /// <remarks/>
        public personsXMLPersonEmploymentsEmploymentPositionPositionStatisticsCompanyNumber companyNumber
        {
            get
            {
                return this.companyNumberField;
            }
            set
            {
                this.companyNumberField = value;
            }
        }

        /// <remarks/>
        public personsXMLPersonEmploymentsEmploymentPositionPositionStatisticsServicePlace servicePlace
        {
            get
            {
                return this.servicePlaceField;
            }
            set
            {
                this.servicePlaceField = value;
            }
        }

        /// <remarks/>
        public personsXMLPersonEmploymentsEmploymentPositionPositionStatisticsWorkClassification workClassification
        {
            get
            {
                return this.workClassificationField;
            }
            set
            {
                this.workClassificationField = value;
            }
        }

        /// <remarks/>
        public personsXMLPersonEmploymentsEmploymentPositionPositionStatisticsWorkMunicipality workMunicipality
        {
            get
            {
                return this.workMunicipalityField;
            }
            set
            {
                this.workMunicipalityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool includeInAA
        {
            get
            {
                return this.includeInAAField;
            }
            set
            {
                this.includeInAAField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool includeInPAI
        {
            get
            {
                return this.includeInPAIField;
            }
            set
            {
                this.includeInPAIField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonEmploymentsEmploymentPositionPositionStatisticsBusinessNumber
    {

        private string nameField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonEmploymentsEmploymentPositionPositionStatisticsCompanyNumber
    {

        private string nameField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonEmploymentsEmploymentPositionPositionStatisticsServicePlace
    {

        private string nameField;

        private string idField;

        /// <remarks/>
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonEmploymentsEmploymentPositionPositionStatisticsWorkClassification
    {

        private string nameField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonEmploymentsEmploymentPositionPositionStatisticsWorkMunicipality
    {

        private string nameField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonEmploymentsEmploymentPositionSalaryInfo
    {

        private object[] itemsField;

        private ItemsChoiceType[] itemsElementNameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("additionalValues", typeof(personsXMLPersonEmploymentsEmploymentPositionSalaryInfoAdditionalValues))]
        [System.Xml.Serialization.XmlElementAttribute("basicSalary", typeof(decimal))]
        [System.Xml.Serialization.XmlElementAttribute("paymentLocation", typeof(personsXMLPersonEmploymentsEmploymentPositionSalaryInfoPaymentLocation))]
        [System.Xml.Serialization.XmlElementAttribute("salaryEndDate", typeof(System.DateTime), DataType = "date")]
        [System.Xml.Serialization.XmlElementAttribute("salaryStartDate", typeof(System.DateTime), DataType = "date")]
        [System.Xml.Serialization.XmlElementAttribute("salaryTable", typeof(personsXMLPersonEmploymentsEmploymentPositionSalaryInfoSalaryTable))]
        [System.Xml.Serialization.XmlElementAttribute("wageFrame", typeof(personsXMLPersonEmploymentsEmploymentPositionSalaryInfoWageFrame))]
        [System.Xml.Serialization.XmlElementAttribute("wageFrameAutoRiseCode", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("yearlySalary", typeof(decimal))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
        public object[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ItemsElementName")]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemsChoiceType[] ItemsElementName
        {
            get
            {
                return this.itemsElementNameField;
            }
            set
            {
                this.itemsElementNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonEmploymentsEmploymentPositionSalaryInfoAdditionalValues
    {

        private decimal value5Field;

        private bool value5FieldSpecified;

        private decimal value1Field;

        private bool value1FieldSpecified;

        private decimal value4Field;

        private bool value4FieldSpecified;

        private decimal value2Field;

        private bool value2FieldSpecified;

        private decimal value7Field;

        private bool value7FieldSpecified;

        private decimal value8Field;

        private bool value8FieldSpecified;

        /// <remarks/>
        public decimal value5
        {
            get
            {
                return this.value5Field;
            }
            set
            {
                this.value5Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool value5Specified
        {
            get
            {
                return this.value5FieldSpecified;
            }
            set
            {
                this.value5FieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal value1
        {
            get
            {
                return this.value1Field;
            }
            set
            {
                this.value1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool value1Specified
        {
            get
            {
                return this.value1FieldSpecified;
            }
            set
            {
                this.value1FieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal value4
        {
            get
            {
                return this.value4Field;
            }
            set
            {
                this.value4Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool value4Specified
        {
            get
            {
                return this.value4FieldSpecified;
            }
            set
            {
                this.value4FieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal value2
        {
            get
            {
                return this.value2Field;
            }
            set
            {
                this.value2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool value2Specified
        {
            get
            {
                return this.value2FieldSpecified;
            }
            set
            {
                this.value2FieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal value7
        {
            get
            {
                return this.value7Field;
            }
            set
            {
                this.value7Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool value7Specified
        {
            get
            {
                return this.value7FieldSpecified;
            }
            set
            {
                this.value7FieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal value8
        {
            get
            {
                return this.value8Field;
            }
            set
            {
                this.value8Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool value8Specified
        {
            get
            {
                return this.value8FieldSpecified;
            }
            set
            {
                this.value8FieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonEmploymentsEmploymentPositionSalaryInfoPaymentLocation
    {

        private string nameField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonEmploymentsEmploymentPositionSalaryInfoSalaryTable
    {

        private string nameField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonEmploymentsEmploymentPositionSalaryInfoWageFrame
    {

        private string descriptionField;

        private string codeField;

        /// <remarks/>
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(IncludeInSchema = false)]
    public enum ItemsChoiceType
    {

        /// <remarks/>
        additionalValues,

        /// <remarks/>
        basicSalary,

        /// <remarks/>
        paymentLocation,

        /// <remarks/>
        salaryEndDate,

        /// <remarks/>
        salaryStartDate,

        /// <remarks/>
        salaryTable,

        /// <remarks/>
        wageFrame,

        /// <remarks/>
        wageFrameAutoRiseCode,

        /// <remarks/>
        yearlySalary,
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonEmploymentsEmploymentPositionWorkDaysInWeek
    {

        private bool fridayField;

        private bool mondayField;

        private bool saturdayField;

        private bool sundayField;

        private bool thursdayField;

        private bool tuesdayField;

        private bool wednesdayField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool friday
        {
            get
            {
                return this.fridayField;
            }
            set
            {
                this.fridayField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool monday
        {
            get
            {
                return this.mondayField;
            }
            set
            {
                this.mondayField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool saturday
        {
            get
            {
                return this.saturdayField;
            }
            set
            {
                this.saturdayField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool sunday
        {
            get
            {
                return this.sundayField;
            }
            set
            {
                this.sundayField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool thursday
        {
            get
            {
                return this.thursdayField;
            }
            set
            {
                this.thursdayField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool tuesday
        {
            get
            {
                return this.tuesdayField;
            }
            set
            {
                this.tuesdayField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool wednesday
        {
            get
            {
                return this.wednesdayField;
            }
            set
            {
                this.wednesdayField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonEmploymentsEmploymentTaxDetails
    {

        private string cardYearField;

        private bool cardYearFieldSpecified;

        private bool deductionLimitField;

        private bool fullTaxField;

        private bool secondEmployeeField;

        private personsXMLPersonEmploymentsEmploymentTaxDetailsTaxDeductionCard taxDeductionCardField;

        private decimal taxExemptionCardField;

        private decimal taxPercentageField;

        /// <remarks/>
        public string cardYear
        {
            get
            {
                return this.cardYearField;
            }
            set
            {
                this.cardYearField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool cardYearSpecified
        {
            get
            {
                return this.cardYearFieldSpecified;
            }
            set
            {
                this.cardYearFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool deductionLimit
        {
            get
            {
                return this.deductionLimitField;
            }
            set
            {
                this.deductionLimitField = value;
            }
        }

        /// <remarks/>
        public bool fullTax
        {
            get
            {
                return this.fullTaxField;
            }
            set
            {
                this.fullTaxField = value;
            }
        }

        /// <remarks/>
        public bool secondEmployee
        {
            get
            {
                return this.secondEmployeeField;
            }
            set
            {
                this.secondEmployeeField = value;
            }
        }

        /// <remarks/>
        public personsXMLPersonEmploymentsEmploymentTaxDetailsTaxDeductionCard taxDeductionCard
        {
            get
            {
                return this.taxDeductionCardField;
            }
            set
            {
                this.taxDeductionCardField = value;
            }
        }

        /// <remarks/>
        public decimal taxExemptionCard
        {
            get
            {
                return this.taxExemptionCardField;
            }
            set
            {
                this.taxExemptionCardField = value;
            }
        }

        /// <remarks/>
        public decimal taxPercentage
        {
            get
            {
                return this.taxPercentageField;
            }
            set
            {
                this.taxPercentageField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonEmploymentsEmploymentTaxDetailsTaxDeductionCard
    {

        private string cardIdField;

        private string cardNameField;

        private string cardTypeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string cardId
        {
            get
            {
                return this.cardIdField;
            }
            set
            {
                this.cardIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string cardName
        {
            get
            {
                return this.cardNameField;
            }
            set
            {
                this.cardNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string cardType
        {
            get
            {
                return this.cardTypeField;
            }
            set
            {
                this.cardTypeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonEmploymentsEmploymentUnion
    {

        private string nameField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonHrmAuthentication
    {

        private string logonIdField;

        private personsXMLPersonHrmAuthenticationType typeField;

        /// <remarks/>
        public string logonId
        {
            get
            {
                return this.logonIdField;
            }
            set
            {
                this.logonIdField = value;
            }
        }

        /// <remarks/>
        public personsXMLPersonHrmAuthenticationType type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonHrmAuthenticationType
    {

        private string nameField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonMunicipality
    {

        private string nameField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonPostalAddress
    {

        private string address1Field;

        private string address2Field;

        private string countryCodeField;

        private string postalAreaField;

        private string postalCodeField;

        /// <remarks/>
        public string address1
        {
            get
            {
                return this.address1Field;
            }
            set
            {
                this.address1Field = value;
            }
        }

        /// <remarks/>
        public string address2
        {
            get
            {
                return this.address2Field;
            }
            set
            {
                this.address2Field = value;
            }
        }

        /// <remarks/>
        public string countryCode
        {
            get
            {
                return this.countryCodeField;
            }
            set
            {
                this.countryCodeField = value;
            }
        }

        /// <remarks/>
        public string postalArea
        {
            get
            {
                return this.postalAreaField;
            }
            set
            {
                this.postalAreaField = value;
            }
        }

        /// <remarks/>
        public string postalCode
        {
            get
            {
                return this.postalCodeField;
            }
            set
            {
                this.postalCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class personsXMLPersonSocialSecurityOffice
    {

        private string nameField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }


}
