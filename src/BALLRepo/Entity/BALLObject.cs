using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BALLRepo.Entity
{
    public class BALLObject
    {
        #region BALL settings


        public string RequestedBy { get; set; }
        public string RequestLanguage { get; set; }

        /// <summary>
        /// The job will be performed on this date and time.
        /// Will be Now if not set
        /// </summary>
        public DateTime? Performdate { get; set; }


        /// <summary>
        /// CS Ticket id if known. If blank a new ticket will be created for operation
        /// </summary>
        public string CSTicketId { get; set; }

        /// <summary>
        /// Comment wil be shown to BALL Operator if set
        /// </summary>
        public string UserComment { get; set; }

        /// <summary>
        /// Set to True if user shall be enabled on create - deafult is True
        /// </summary>
        public bool Enable { get; set; } = true;

        /// <summary>
        /// Set to true to add Exchange email to user
        /// </summary>
        public bool EnableExchange { get; set; }

        /// <summary>
        /// Use rules to make email
        /// </summary>
        public bool MakeEmailAddress { get; set; }

        /// <summary>
        /// Set to true to remove Exchange email from user
        /// Own settings since it is critical
        /// </summary>
        public bool DisableExchange { get; set; }

        #endregion

         #region Company

        /// <summary>
        /// Company must be set - use companyGuid, CompanyId or CompanyName. Will be checked in this order to find company - failing if not found
        /// </summary>
        public string CompanyGuid { get; set; }

        /// <summary>
        /// Company must be set - use companyGuid, CompanyId or CompanyName. Will be checked in this order to find company - failing if not found
        /// </summary>
        public string CompanyId { get; set; }


        /// <summary>
        /// Name of company - case-sensitive correctly spelled to be found
        /// </summary>
        public string CompanyName { get; set; }


		/// <summary>
		/// Name of company - case-sensitive correctly spelled to be found
		/// </summary>
		public string CompanyCode { get; set; }

		/// <summary>
		/// AdCompany must be set if there are more than one - use adcompanyGuid, adCompanyId or adCompanyName. default AdCompany will be used if not set
		/// Important to set to get user in the right OU
		/// </summary>
		public string DirectoryGuid { get; set; }


        /// <summary>
        /// Name of Adcompany - case-sensitive correctly spelled to be found
        /// </summary>
        public string DirectoryName { get; set; }



        /// <summary>
        /// Location - use locationGuid, locationId or locationName. Will be checked in this order to find location - default location will be used if not set
        /// </summary>
        public string LocationGuid { get; set; }

        /// <summary>
        /// Name of location - case-sensitive correctly spelled to be found
        /// </summary>
        public string LocationName { get; set; }



		/// <summary>
		/// Name of location - case-sensitive correctly spelled to be found
		/// </summary>
		public string LocationCode { get; set; }

		/// <summary>
		/// Invoice - use invoiceGuid, invoiceId or invoiceName. Will be checked in this order to find invoice - default invoice will be used if not set
		/// </summary>
		public string SubscriptionGuid { get; set; }


        /// <summary>
        /// Name of invoice - case-sensitive correctly spelled to be found
        /// </summary>
        public string SubscriptionName { get; set; }

        /// <summary>
        /// Usertype Id - set to id from list. If not set Intility ID | User will be chosen.
        ///1	Intility ID | User
        ///2	Intility ID | User (Custom)
        ///4	Intility ID | Resource account
        ///5	Intility ID | Email user
        ///12	Driftskonto
        ///24	Intility ID | External user
        ///26	Shared calendar
        ///27	Shared mailbox
        /// 
        /// 
        /// </summary>
        public string UsertypeGuid { get; set; }


        public string UsertypeName { get; set; }

        /// <summary>
        /// State - can be set
        /// </summary>
        public string StateGuid { get; set; }

        /// <summary>
        /// Name of State - case-sensitive correctly spelled to be found
        /// </summary>
        public string StateName { get; set; }
        #endregion

        #region Fields




        /// <summary>
        /// UPN of user -  normally not set on create, must be set on Change or if manually username is needed
        /// </summary>
        public string userPrincipalName { get; set; }


        /// <summary>
        /// Username/samaccountname - normally not set on create, must be set on Change or if manually username is needed
        /// </summary>
        public string sAMAccountName { get; set; }

        /// <summary>
        /// Account GUID
        /// </summary>
        public string objectGUID { get; set; }

        /// <summary>
        /// Bean.User GUID
        /// </summary>
        public string BeanUserGuid { get; set; }

        /// <summary>
        /// Info field in AD
        /// </summary>
        public string info { get; set; }

        /// <summary>
        /// Comment field in AD
        /// </summary>
        public string comment { get; set; }

        /// <summary>
        /// If EnableExchange = true - prim will be set in Exchange - else it will be added to AD Mail-property
        /// </summary>
        public string mail { get; set; }

        /// <summary>
        /// Use for samaccountname if set
        /// </summary>
        public string mailNickname { get; set; }

        /// <summary>
        /// Display name can be set if wanted - will be first + last if not set
        /// </summary>
        public string displayName { get; set; }

        /// <summary>
        /// first or last name must be set or operation will fail
        /// </summary>
        public string givenName { get; set; }

        /// <summary>
        /// first or last name must be set or operation will fail
        /// </summary>
        public string sn { get; set; }


        /// <summary>
        /// Phone can be set
        /// </summary>
        public string telephoneNumber { get; set; }

        /// <summary>
        /// Fax to be set
        /// </summary>
        public string facsimileTelephoneNumber { get; set; }

        /// <summary>
        /// Cellphone can be set
        /// </summary>
        public string mobile { get; set; }

        /// <summary>
        /// ipPhone can be set
        /// </summary>
        public string ipPhone { get; set; }

        /// <summary>
        /// Address can be set
        /// </summary>
        public string streetAddress { get; set; }

        /// <summary>
        /// PostalCode can be set
        /// </summary>
        public string postalCode { get; set; }

        /// <summary>
        /// postOfficeBox can be set
        /// </summary>
        public string postOfficeBox { get; set; }


        /// <summary>
        /// Office can be set
        /// </summary>
        public string physicalDeliveryOfficeName { get; set; }

        /// <summary>
        /// Place can be set
        /// </summary>
        public string l { get; set; }

        /// <summary>
        /// Correct country code
        /// </summary>
        public string c { get; set; }

        /// <summary>
        /// Correct country code
        /// </summary>
        public string co { get; set; }


        /// <summary>
        /// Pref language with correct language code
        /// </summary>
        public string preferredLanguage { get; set; }

        /// <summary>
        /// Title can be set
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// employeeID can be set
        /// </summary>
        public string employeeID { get; set; }


        /// <summary>
        /// employeeID can be set
        /// </summary>
        public string employeeType { get; set; }


        /// <summary>
        /// Is initials
        /// </summary>
        public string initials { get; set; }

        /// <summary>
        /// Company in AD - will be set to ROT company name if not set
        /// </summary>
        public string company { get; set; }

        /// <summary>
        /// Department can be set
        /// </summary>
        public string department { get; set; }


        /// <summary>
        /// state can be set
        /// </summary>
        public string st { get; set; }


        /// <summary>
        /// Department can be set
        /// </summary>
        public string distinguishedName { get; set; }


        /// <summary>
        /// SamAccountname or DN of manager
        /// </summary>
        public string manager { get; set; }

        /// <summary>
        /// Pager
        /// </summary>
        public string pager { get; set; }

        /// <summary>
        /// Division
        /// </summary>
        public string division { get; set; }

        /// <summary>
        /// Home phone
        /// </summary>
        public string homePhone { get; set; }

        /// <summary>
        /// uidNumber
        /// </summary>
        public string uidNumber { get; set; }

        /// <summary>
        /// businessCategory
        /// </summary>
        public string businessCategory { get; set; }

        /// <summary>
        ///roomNumber
        /// </summary>
        public string roomNumber { get; set; }

        /// <summary>
        /// account expire date
        /// </summary>
        public string accountExpires { get; set; }

        /// <summary>
        /// Personal Id for use with BankId
        /// </summary>
        public string i2PersonalID { get; set; }

        /// <summary>
        /// Personnummer
        /// </summary>
        public string i2NationalId { get; set; }

        /// <summary>
        /// Birth date on format Generalized-Time - YYYYMMDDHHMMSS.0Z
        /// </summary>
        public string i2Birthdate { get; set; }

        /// <summary>
        /// Birth date on format Generalized-Time - YYYYMMDDHHMMSS.0Z
        /// </summary>
        public string i2AdditionalPhone { get; set; }

        /// <summary>
        /// Set password of user
        /// </summary>
        public string password { get; set; }

        /// <summary>
        /// List of proxy addresses can be set. Only valid if EnableExchange
        /// </summary>
        public List<string> proxyAddresses { get; set; }

        /// <summary>
        /// List of proxy addresses to be removed. Only valid if EnableExchange
        /// Used only on Patch
        /// </summary>
        public List<string> proxyAddressesOut { get; set; }

        /// <summary>
        ///  ExtensionAttribute10
        /// </summary>
        public string extensionAttribute1 { get; set; }

        /// <summary>
        ///  ExtensionAttribute10
        /// </summary>
        public string extensionAttribute2 { get; set; }

        /// <summary>
        ///  ExtensionAttribute10
        /// </summary>
        public string extensionAttribute3 { get; set; }
        /// <summary>
        ///  ExtensionAttribute10
        /// </summary>
        public string extensionAttribute4 { get; set; }
        /// <summary>
        ///  ExtensionAttribute10
        /// </summary>
        public string extensionAttribute5 { get; set; }
        /// <summary>
        ///  ExtensionAttribute10
        /// </summary>
        public string extensionAttribute6 { get; set; }
        /// <summary>
        ///  ExtensionAttribute10
        /// </summary>
        public string extensionAttribute7 { get; set; }
        /// <summary>
        ///  ExtensionAttribute10
        /// </summary>
        public string extensionAttribute8 { get; set; }
        /// <summary>
        ///  ExtensionAttribute10
        /// </summary>
        public string extensionAttribute9 { get; set; }
        /// <summary>
        ///  ExtensionAttribute10
        /// </summary>
        public string extensionAttribute10 { get; set; }
        /// <summary>
        ///  ExtensionAttribute10
        /// </summary>
        public string extensionAttribute11 { get; set; }
        /// <summary>
        ///  ExtensionAttribute10
        /// </summary>
        public string extensionAttribute12 { get; set; }
        /// <summary>
        ///  ExtensionAttribute10
        /// </summary>
        public string extensionAttribute13 { get; set; }
        /// <summary>
        ///  ExtensionAttribute10
        /// </summary>
        public string extensionAttribute14 { get; set; }
        /// <summary>
        ///  ExtensionAttribute10
        /// </summary>
        public string extensionAttribute15 { get; set; }

        /// <summary>
        ///  ExtensionAttribute10
        /// </summary>
        public string extensionName { get; set; }

        /// <summary>
        ///  employeeNumber
        /// </summary>
        public string employeeNumber { get; set; }

        #endregion

        #region Lists

        /// <summary>
        /// List of appgroups
        /// </summary>
        public string AppGroups { get; set; }

        /// <summary>
        /// List of groupnames to be added
        /// </summary>
        public string Groups { get; set; }

        /// <summary>
        /// List of groupnames to be added
        /// </summary>
        public string GroupsString { get; set; }


        /// <summary>
        /// List of groupnames to be removed
        /// Used only on Patch
        /// </summary>
        public List<string> GroupsOut { get; set; }

        /// <summary>
        /// List of groupnames to be removed
        /// Used only on Patch
        /// </summary>
        public string GroupsOutString { get; set; }

        /// <summary>
        /// List of Services to be added
        ///</summary>
        public string Options { get; set; }

        /// <summary>
        /// List of Services to be removed
        /// Used only on Patch
        /// </summary>
        public List<string> OptionsOut { get; set; }



        #endregion


        #region Settings

        /// <summary>
        /// List of extra settings to be added to the e-mail
        /// needs settingsname and settingsvalue
        /// </summary>
        public string ExtraSettings { get; set; }




        /// <summary>
        /// If CopyGroupsFromAccount is set to username of account it will be fetched and groups for the user will be added
        /// CopyGroupsFromAccount must be case sensitive correct for samaccountname of ordinary account or _templateUser
        /// </summary>
        public string TemplateAccount { get; set; }


        /// <summary>
        /// If set to True the user will be disabled.
        /// </summary>
        public bool DoDisableOperation { get; set; }

        /// <summary>
        /// Number of months OOF and/or VS should be on - default is 3;
        /// </summary>
        public int MonthsOOFVS { get; set; } = 3;

        /// <summary>
        /// Out of Office HTML Message
        /// </summary>
        public string OOFMessage { get; set; }


        /// <summary>
        /// If set - email will be forwarded to this address
        /// </summary>
        public string ForwardingAddress { get; set; }

        /// <summary>
        /// If true email will be backed up to users homearea
        /// </summary>
        public bool BackupEmail { get; set; }

        /// <summary>
        /// List of SamAccountnames to be given access to users Home Area
        /// </summary>
        public List<string> HomeAccessUsers { get; set; }

        #endregion

        #region Classes

        public class ExtraSetting
        {
            public string SettingName { get; set; }
            public string SettingValue { get; set; }
        }

        #endregion
    }
}