using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.MetadirectoryServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StaticLib;
using SuccessFactorsRepo.Entity;

namespace SuccessFactorsRepo.Repo
{
    public class MIMRepo
    {
        #region Properties

        public string CompanyCode { get; set; }
        private string _token = string.Empty;
        private string _serviceUrl = string.Empty;
        private string _apiPoint = string.Empty;
        public bool GetEmployees { get; set; }
        public int DaysBeforeStart { get; set; }
        public int DaysAfterEnd { get; set; }
        public IList<CSEntryChange> CSEntries { get; set; }
        public string JsonC1Root { get; set; }
        public List<Dictionary<string, string>> EmpDic { get; set; }
        public SchemaType SchemaDef { get; set; }
        public PutExportEntriesResults ExportEntriesResults { get; set; }
        private HttpClient _hClient;
        private string _logPath = $@"C:\temp\SFMIMLog_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
        private readonly StringBuilder _logBuffer = new StringBuilder();

        #endregion

        #region Logging

        private void Log(string message)
        {
            _logBuffer.AppendLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {message}");
        }

        private void FlushLog()
        {
            try
            {
                Directory.CreateDirectory(@"C:\temp");
                File.WriteAllText(_logPath, _logBuffer.ToString());
            }
            catch { }
        }

        private void LogSeparator()
        {
            Log("==========================================================================");
        }

        private void LogUserJobs(string userId, List<SFEmpJobResult> jobs)
        {
            Log($"  --- EmpJob records for userId={userId} (count={jobs.Count}) ---");
            foreach (var j in jobs)
            {
                Log($"    Job: startDate={j.startDate:yyyy-MM-dd} endDate={j.endDate:yyyy-MM-dd}");
                Log($"         businessUnit={j.businessUnit} division={j.division} department={j.department}");
                Log($"         company={j.company} countryOfCompany={j.countryOfCompany}");
                Log($"         location={j.location} managerId={j.managerId} position={j.position}");
                Log($"         jobTitle={j.jobTitle} costCenter={j.costCenter} costCenterName={j.costCenterNav?.name}");
                Log($"         customString2={j.customString2} customString7={j.customString7} customString27={j.customString27}");
                Log($"         contractEndDate={j.contractEndDate:yyyy-MM-dd} customDate1={j.customDate1:yyyy-MM-dd}");
            }
        }

        private void LogUserEmployments(string userId, List<SFEmpEmploymentResult> emps)
        {
            Log($"  --- EmpEmployment records for userId={userId} (count={emps.Count}) ---");
            foreach (var e in emps)
            {
                Log($"    Employment: personIdExternal={e.personIdExternal} startDate={e.startDate:yyyy-MM-dd} endDate={e.endDate:yyyy-MM-dd} lastDateWorked={e.lastDateWorked:yyyy-MM-dd}");
                Log($"                isContingentWorker={e.isContingentWorker}");
                Log($"                vendor={e.workOrderNav?.results?.FirstOrDefault()?.vendorNav?.vendorName}");
                if (e.jobInfoNav?.results != null)
                    foreach (var jn in e.jobInfoNav.results.OrderByDescending(x => x.startDate))
                        Log($"                jobInfoNav: startDate={jn.startDate:yyyy-MM-dd} employeeClass={jn.employeeClassNav?.externalCode} employmentType={jn.employmentTypeNav?.externalCode}");
            }
        }

        private void LogFinalMIMObject(SFMIMObject o)
        {
            Log($"  --- Final MIMObject for userId={o.userId} personIdExternal={o.personIdExternal} ---");
            Log($"      firstName={o.firstName} lastName={o.lastName} employeeNumber={o.employeeNumber}");
            Log($"      status={o.status} emailAddress={o.emailAddress}");
            Log($"      originalStartDate={o.originalStartDate:yyyy-MM-dd} endDate={o.endDate:yyyy-MM-dd} lastDateWorked={o.lastDateWorked:yyyy-MM-dd}");
            Log($"      originalStartDateJob={o.originalStartDateJob:yyyy-MM-dd} endDateJob={o.endDateJob:yyyy-MM-dd}");
            Log($"      businessUnit={o.businessUnit} businessUnitNoName={o.businessUnitNoName} businessUnitEnName={o.businessUnitEnName}");
            Log($"      division={o.division} divisionNoName={o.divisionNoName} divisionEnName={o.divisionEnName}");
            Log($"      department={o.department} departmentNoName={o.departmentNoName} departmentEnName={o.departmentEnName}");
            Log($"      jobCustomString2={o.jobCustomString2} customString2(nb_NO)={o.customString2}");
            Log($"      customString2EnName={o.customString2EnName} customString2DefaultName={o.customString2DefaultName}");
            Log($"      jobTitle={o.jobTitle} titleNo={o.titleNo} titleEn={o.titleEn} titleDefault={o.titleDefault}");
            Log($"      company={o.company} countryOfCompany={o.countryOfCompany} costCenter={o.costCenter}");
            Log($"      location={o.location} manager={o.manager}");
            Log($"      isContingentWorker={o.isContingentWorker} vendor={o.Vendor}");
            Log($"      employeeType={o.employeeType} employeeTypeText={o.employeeTypeText} employeeTypeId={o.employeeTypeId}");
            Log($"      employeeClassExternalCode={o.employeeClassExternalCode} employeeClassEnName={o.employeeClassEnName} employeeClassNoName={o.employeeClassNoName}");
            Log($"      employmentTypeExternalCode={o.employmentTypeExternalCode} employmentTypeEnName={o.employmentTypeEnName} employmentTypeNoName={o.employmentTypeNoName}");
            Log($"      BusinessCategory={o.BusinessCategory}");
            Log($"      UnitCode={o.UnitCode}");
            Log($"      BusinessUnitNoPath={o.BusinessUnitNoPath}");
            Log($"      BusinessUnitEnPath={o.BusinessUnitEnPath}");
            Log($"      ShowIntranetCode={o.ShowIntranetCode} ShowIntranetEnName={o.ShowIntranetEnName} ShowIntranetNoName={o.ShowIntranetNoName}");
            Log($"      SystemAccessCode={o.SystemAccessCode} SystemAccessEnName={o.SystemAccessEnName} SystemAccessNoName={o.SystemAccessNoName}");
            Log($"      mobile={o.mobile} businessPhone={o.businessPhone} homePhone={o.homePhone}");
            Log($"      address1={o.address1} address2={o.address2} zipCode={o.zipCode} office={o.office} country={o.country}");
            Log($"      customDate={o.customDate:yyyy-MM-dd} jobCustomString27={o.jobCustomString27}");
            Log($"      terminationAgreement={o.terminationAgreement} personCustomString1={o.personCustomString1}");
        }

        private string SafeGet(string url, CancellationToken ct)
        {
            try
            {
                var resp = _hClient.GetAsync(url, ct).ConfigureAwait(false).GetAwaiter().GetResult();
                if (!resp.IsSuccessStatusCode)
                {
                    Log($"WARNING SafeGet {url}: HTTP {resp.StatusCode} {resp.ReasonPhrase}");
                    return string.Empty;
                }
                return resp.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Log($"ERROR SafeGet {url}: {ex.Message}");
                return string.Empty;
            }
        }

        #endregion

        #region Auth

        public MIMRepo() { }

        private bool DecideAuth(string companyCode)
        {
            switch (companyCode)
            {
                case "WMQAA": { var c = Get2Creds(companyCode); AuthAPost(c); break; }
                case "WMPRODA": { var c = Get2Creds(companyCode); AuthAPost(c); break; }
                case "GP": { var c = Get1Creds(companyCode); AuthHClient(c); break; }
                case "VR": { var c = Get1Creds(companyCode); AuthHClient(c); break; }
                default: break;
            }
            return true;
        }

        private Suc2FCreds Get2Creds(string companyCode)
        {
            Suc2FCreds creds = new Suc2FCreds();
            switch (companyCode)
            {
                case "WMQAA":
                    creds = new Suc2FCreds
                    {
                        LoginEndPoint = "https://login.microsoftonline.com/norgesbank.no/",
                        ApiEndPoint = "https://api2.successfactors.eu/",
                        Grant_type = "password",
                        SFGrant_type = "urn:ietf:params:oauth:grant-type:saml2-bearer",
                        Password = SecretRef.WMQAA_Password,
                        Scope = "api://c48a4569-18e9-4e10-b82f-41d1814f0f98/api_access",
                        TokenType = "urn:ietf:params:oauth:token-type:saml2",
                        Company_id = "norgesbankD",
                        Client_id = "c48a4569-18e9-4e10-b82f-41d1814f0f98",
                        Client_Secret = SecretRef.WMQAA_ClientSecret,
                        ApiKey = SecretRef.WMQAA_APIKey,
                        User_id = "ServiceAccountSFToken@NorgesBank.onmicrosoft.com",
                    };
                    _apiPoint = creds.ApiEndPoint;
                    break;
                case "WMPRODA":
                    creds = new Suc2FCreds
                    {
                        LoginEndPoint = "https://login.microsoftonline.com/norgesbank.no/",
                        ApiEndPoint = "https://api2.successfactors.eu/",
                        Grant_type = "password",
                        SFGrant_type = "urn:ietf:params:oauth:grant-type:saml2-bearer",
                        Password = SecretRef.WMPRODA_Password,
                        Scope = "api://c48a4569-18e9-4e10-b82f-41d1814f0f98/api_access",
                        TokenType = "urn:ietf:params:oauth:token-type:saml2",
                        Company_id = "norgesbank",
                        Client_id = "c48a4569-18e9-4e10-b82f-41d1814f0f98",
                        Client_Secret = SecretRef.WMPRODA_ClientSecret,
                        ApiKey = SecretRef.WMPRODA_APIKey,
                        User_id = "ServiceAccountSFToken@NorgesBank.onmicrosoft.com",
                    };
                    _apiPoint = creds.ApiEndPoint;
                    break;
            }
            return creds;
        }

        private Suc1FCreds Get1Creds(string companyCode)
        {
            Suc1FCreds creds = new Suc1FCreds();
            switch (companyCode)
            {
                case "GP":
                    creds = new Suc1FCreds { ApiEndPoint = "https://api55.sapsf.eu/", Username = "SFAPI@EntraProd", Password = SecretRef.GP_Password };
                    _apiPoint = creds.ApiEndPoint;
                    break;
                case "VR":
                    creds = new Suc1FCreds { ApiEndPoint = "https://api55.sapsf.eu/", Username = "API_intility@AllenteProd", Password = SecretRef.VR_Password };
                    _apiPoint = creds.ApiEndPoint;
                    break;
            }
            return creds;
        }

        public void AuthHClient(Suc1FCreds creds)
        {
            _hClient = new HttpClient();
            var aut = Encoding.UTF8.GetBytes($"{creds.Username}:{creds.Password}");
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            _hClient.DefaultRequestHeaders.Add("Authorization", $"Basic {Convert.ToBase64String(aut)}");
            _hClient.BaseAddress = new Uri(creds.ApiEndPoint);
            _hClient.DefaultRequestHeaders.Accept.Clear();
            _hClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void AuthAPost(Suc2FCreds creds)
        {
            _token = null;
            var tClient = new HttpClient();
            var tokenEndPoint = new Uri(new Uri(creds.ApiEndPoint), "oauth/token");
            var tokenrUri = new Uri(new Uri(creds.LoginEndPoint), "oauth2/v2.0/token");

            HttpContent content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"grant_type", creds.Grant_type}, {"username", creds.User_id}, {"password", creds.Password},
                {"scope", creds.Scope}, {"client_id", creds.Client_id}, {"client_secret", creds.Client_Secret},
                {"requested_token_type", creds.TokenType},
            });

            var bsaml = tClient.PostAsync(tokenrUri, content).Result.Content.ReadAsStringAsync().Result;
            var azuretok = (string)JObject.Parse(bsaml)["access_token"];

            content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"company_id", creds.Company_id}, {"client_id", creds.ApiKey},
                {"grant_type", creds.SFGrant_type}, {"assertion", azuretok}
            });

            var jsonContent = tClient.PostAsync(tokenEndPoint, content).Result.Content.ReadAsStringAsync().Result;
            var obj = JObject.Parse(jsonContent);
            _token = (string)obj["access_token"];
            _serviceUrl = (string)obj["instance_url"];

            _hClient = new HttpClient();
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            _hClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");
            _hClient.BaseAddress = new Uri(creds.ApiEndPoint);
            _hClient.DefaultRequestHeaders.Accept.Clear();
            _hClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void AuthPost(Suc2FCreds creds)
        {
            _token = null;
            var tClient = new HttpClient();
            var tokenEndPoint = new Uri(new Uri(creds.ApiEndPoint), "oauth/idp");
            var tokenUrl = $"{creds.ApiEndPoint}/oauth/token";
            var tokenrUri = new Uri(new Uri(creds.ApiEndPoint), "oauth/token");

            HttpContent content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"client_id", creds.Client_id}, {"user_id", creds.User_id},
                {"token_url", tokenUrl}, {"private_key", creds.Private_key}
            });
            var bsaml = tClient.PostAsync(tokenEndPoint, content).Result.Content.ReadAsStringAsync().Result;

            content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"company_id", creds.Company_id}, {"client_id", creds.Client_id},
                {"user_id", creds.User_id}, {"grant_type", "urn:ietf:params:oauth:grant-type:saml2-bearer"},
                {"assertion", bsaml}
            });

            var jsonContent = tClient.PostAsync(tokenrUri, content).Result.Content.ReadAsStringAsync().Result;
            var obj = JObject.Parse(jsonContent);
            _token = (string)obj["access_token"];
            _serviceUrl = (string)obj["instance_url"];

            _hClient = new HttpClient();
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            _hClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");
            _hClient.BaseAddress = new Uri(creds.ApiEndPoint);
            _hClient.DefaultRequestHeaders.Accept.Clear();
            _hClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        #endregion


        #region Handlers

        public void Get()
        {
            DecideAuth(CompanyCode);
            CSEntries = new List<CSEntryChange>();
            if (GetEmployees)
            {
                var fo = GetApi();
                foreach (var ffo in fo)
                    CSEntries.Add(FillCSEntry(ffo, "personIdExternal", "Person"));
            }
        }

        public void Put()
        {
            DecideAuth(CompanyCode);
            ExportEntriesResults = new PutExportEntriesResults();

            foreach (CSEntryChange csentryChange in CSEntries)
            {
                IList<AttributeChange> attributeChanges = new List<AttributeChange>();
                var status = Util.ErrValidate("BadRequest");
                string errorName = "";

                try
                {
                    switch (csentryChange.ObjectModificationType)
                    {
                        case ObjectModificationType.Add:
                        case ObjectModificationType.Delete:
                            {
                                var poststatus = "BadRequest";
                                var errorMessage = "Not implemented";
                                errorName = "SucFac error";
                                status = Util.ErrValidate(poststatus);
                                ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, status, errorName, errorMessage));
                                break;
                            }
                        case ObjectModificationType.Replace:
                        case ObjectModificationType.Update:
                            {
                                var errorMessage = "";
                                var boo = FillRootObject(csentryChange);
                                var anchor = boo.Anchor;
                                if (string.IsNullOrEmpty(anchor))
                                {
                                    ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, MAExportError.ExportErrorInvalidDN, "Quinyx error", "No badgeNo"));
                                    break;
                                }
                                if (UpdateRecord(boo, out errorMessage)) status = MAExportError.Success;
                                ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, status, errorName, errorMessage));
                                break;
                            }
                        default: break;
                    }
                }
                catch (Exception ex)
                {
                    ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, status, "SF repo Error", ex.Message));
                }
            }
        }

        private ExportData FillRootObject(CSEntryChange cse)
        {
            var ep = new ExportData { Anchor = "" };
            foreach (var a in cse.AnchorAttributes)
                ep.Anchor = a.Value.ToString();
            foreach (var p in cse.AttributeChanges)
            {
                var val = p.ValueChanges.FirstOrDefault(x => x.ModificationType == ValueModificationType.Add);
                if (val != null)
                {
                    if (p.Name == "emailAddress") ep.Email = val.Value.ToString();
                    if (p.Name == "username") ep.Username = val.Value.ToString();
                }
            }
            return ep;
        }

        #endregion


        #region API

        private List<SFMIMObject> GetApi()
        {
            var baseURL = $"{_apiPoint}odata/v2/";
            var ct = new CancellationToken();

            Log($"START GetApi - CompanyCode={CompanyCode} DaysBeforeStart={DaysBeforeStart} DaysAfterEnd={DaysAfterEnd}");
            LogSeparator();

            // PerPerson
            var sfPerPersonList = new List<SFPerPersonResult>();
            try
            {
                var json = SafeGet($"{baseURL}PerPerson?$select=personIdExternal,createdDateTime", ct);
                var res = JsonConvert.DeserializeObject<SFPerPersons>(json);
                if (res?.d?.results != null) sfPerPersonList.AddRange(res.d.results);
                while (res?.d?.__next != null)
                {
                    json = SafeGet(res.d.__next, ct);
                    res = JsonConvert.DeserializeObject<SFPerPersons>(json);
                    if (res?.d?.results != null) sfPerPersonList.AddRange(res.d.results);
                }
            }
            catch (Exception ex) { Log($"ERROR PerPerson: {ex.Message}"); throw; }
            Log($"PerPerson fetched: {sfPerPersonList.Count} records");

            // User
            var sfUserList = new List<SFUserResult>();
            try
            {
                var json = SafeGet($"{baseURL}User?$filter=status in 'f','t','F','T','e','d'", ct);
                var res = JsonConvert.DeserializeObject<SFUsers>(json);
                if (res?.d?.results != null) sfUserList.AddRange(res.d.results);
                while (res?.d?.__next != null)
                {
                    json = SafeGet(res.d.__next, ct);
                    res = JsonConvert.DeserializeObject<SFUsers>(json);
                    if (res?.d?.results != null) sfUserList.AddRange(res.d.results);
                }
            }
            catch (Exception ex) { Log($"ERROR User: {ex.Message}"); throw; }
            Log($"User fetched: {sfUserList.Count} records");

            var duserList = sfUserList.ToList();
            if (!CompanyCode.StartsWith("WM"))
                duserList = sfUserList.GroupBy(x => x.empId, (_, g) => g.OrderByDescending(c => c.hireDate).First()).ToList();
            Log($"User after dedup: {duserList.Count} records");

            string sDate = DateTime.Now.AddDays(-DaysBeforeStart).Date.ToString("yyyy-MM-dd");
            string eDate = DateTime.Now.AddDays(DaysAfterEnd).Date.ToString("yyyy-MM-dd");
            Log($"Date range: sDate={sDate} eDate={eDate}");

            // EmpEmployment
            var sfEmploymentList = new List<SFEmpEmploymentResult>();
            try
            {
                string empUrl = $"{baseURL}EmpEmployment?fromDate={sDate}&toDate={eDate}";
                if (CompanyCode.StartsWith("WM"))
                    empUrl = $"{baseURL}EmpEmployment?fromDate={sDate}&toDate={eDate}" +
                             $"&$select=isContingentWorker,endDate,startDate,lastDateWorked," +
                             $"jobInfoNav/startDate,jobInfoNav/employeeClassNav/externalCode," +
                             $"jobInfoNav/employeeClassNav/picklistLabels/label,jobInfoNav/employeeClassNav/picklistLabels/locale," +
                             $"jobInfoNav/employmentType,jobInfoNav/employmentTypeNav/externalCode," +
                             $"jobInfoNav/employmentTypeNav/picklistLabels/label,workOrderNav/vendorNav/vendorName," +
                             $"jobInfoNav/employmentTypeNav/picklistLabels/locale,personIdExternal,userId" +
                             $"&$expand=jobInfoNav/employeeClassNav/picklistLabels,jobInfoNav/employmentTypeNav/picklistLabels,workOrderNav/vendorNav";

                var json = SafeGet(empUrl, ct);
                var res = JsonConvert.DeserializeObject<SFEmpEmployments>(json);
                if (res?.d?.results != null) sfEmploymentList.AddRange(res.d.results);
                while (res?.d?.__next != null)
                {
                    json = SafeGet(res.d.__next, ct);
                    res = JsonConvert.DeserializeObject<SFEmpEmployments>(json);
                    if (res?.d?.results != null) sfEmploymentList.AddRange(res.d.results);
                }
            }
            catch (Exception ex) { Log($"ERROR EmpEmployment: {ex.Message}"); }
            Log($"EmpEmployment fetched: {sfEmploymentList.Count} records");

            // -------------------------------------------------------
            // EmpJob Call 1 — fromDate/toDate
            // Gets all job rows including future users (DaysAfterEnd ahead).
            // customString2Nav included as fallback for future users not
            // yet returned by the asOfDate call.
            // -------------------------------------------------------
            var sfEmpJobList = new List<SFEmpJobResult>();
            try
            {
                string jobUrl = $"{baseURL}EmpJob?fromDate={sDate}&toDate={eDate}";
                if (CompanyCode.StartsWith("WM"))
                    jobUrl = $"{baseURL}EmpJob?fromDate={sDate}&toDate={eDate}" +
                             $"&$select=endDate,startDate,contractEndDate,businessUnit,costCenter,company,countryOfCompany," +
                             $"customString2,customString7,customString27,department,division,jobTitle,location,managerId,position,userId," +
                             $"costCenterNav/name," +
                             $"customString2Nav/externalName_nb_NO," +
                             $"customString2Nav/externalName_en_GB," +
                             $"customString2Nav/externalName_defaultValue" +
                             $"&$expand=costCenterNav,customString2Nav";

                var json = SafeGet(jobUrl, ct);
                var res = JsonConvert.DeserializeObject<SFEmpJobs>(json);
                if (res?.d?.results != null) sfEmpJobList.AddRange(res.d.results);
                while (res?.d?.__next != null)
                {
                    json = SafeGet(res.d.__next, ct);
                    res = JsonConvert.DeserializeObject<SFEmpJobs>(json);
                    if (res?.d?.results != null) sfEmpJobList.AddRange(res.d.results);
                }
            }
            catch (Exception ex) { Log($"ERROR EmpJob: {ex.Message}"); }
            Log($"EmpJob fetched: {sfEmpJobList.Count} records");

            // -------------------------------------------------------
            // EmpJob Call 2 — asOfDate=today
            // SF cannot combine asOfDate with fromDate/toDate.
            // This call returns currently active jobs with customString2Nav
            // resolved to today's effective group name — bypassing the
            // frozen snapshot from Call 1. Stored as dictionary on userId.
            // -------------------------------------------------------
            string asOfDate = DateTime.Now.Date.ToString("yyyy-MM-dd");
            var sfEmpJobCurrentNav = new Dictionary<string, Customstring2nav>();
            try
            {
                if (CompanyCode.StartsWith("WM"))
                {
                    string navUrl = $"{baseURL}EmpJob?asOfDate={asOfDate}" +
                                    $"&$select=userId," +
                                    $"customString2Nav/externalName_nb_NO," +
                                    $"customString2Nav/externalName_en_GB," +
                                    $"customString2Nav/externalName_defaultValue" +
                                    $"&$expand=customString2Nav";

                    var json = SafeGet(navUrl, ct);
                    var res = JsonConvert.DeserializeObject<SFEmpJobs>(json);
                    if (res?.d?.results != null)
                        foreach (var r in res.d.results)
                            if (!string.IsNullOrEmpty(r.userId) && r.customString2Nav != null)
                                sfEmpJobCurrentNav[r.userId] = r.customString2Nav;

                    while (res?.d?.__next != null)
                    {
                        json = SafeGet(res.d.__next, ct);
                        res = JsonConvert.DeserializeObject<SFEmpJobs>(json);
                        if (res?.d?.results != null)
                            foreach (var r in res.d.results)
                                if (!string.IsNullOrEmpty(r.userId) && r.customString2Nav != null)
                                    sfEmpJobCurrentNav[r.userId] = r.customString2Nav;
                    }
                }
            }
            catch (Exception ex) { Log($"ERROR EmpJob asOfDate: {ex.Message}"); }
            Log($"EmpJob asOfDate nav fetched: {sfEmpJobCurrentNav.Count} records");

            if (sfEmpJobCurrentNav.TryGetValue("3565", out var nav3565))
                Log($"EmpJob asOfDate sample userId=3565: nb_NO={nav3565.externalName_nb_NO} en_GB={nav3565.externalName_en_GB}");

            // PerEmail
            var sfPerEmailList = new List<SFPerEmailResult>();
            try
            {
                var json = SafeGet($"{baseURL}PerEmail", ct);
                var res = JsonConvert.DeserializeObject<SFPerEmails>(json);
                if (res?.d?.results != null) sfPerEmailList.AddRange(res.d.results);
                while (res?.d?.__next != null)
                {
                    json = SafeGet(res.d.__next, ct);
                    res = JsonConvert.DeserializeObject<SFPerEmails>(json);
                    if (res?.d?.results != null) sfPerEmailList.AddRange(res.d.results);
                }
            }
            catch (Exception ex) { Log($"ERROR PerEmail: {ex.Message}"); }
            Log($"PerEmail fetched: {sfPerEmailList.Count} records");

            // PerPhone
            var sfPerPhoneList = new List<SFPerPhoneResult>();
            try
            {
                string phoneUrl = CompanyCode.StartsWith("WM")
                    ? $"{baseURL}PerPhone?$select=personIdExternal,phoneType,phoneNumber,isPrimary,extension,phoneTypeNav/externalCode&$expand=phoneTypeNav"
                    : $"{baseURL}PerPhone?$select=personIdExternal,phoneType,phoneNumber,countryCode,isPrimary,phoneTypeNav/externalCode&$expand=phoneTypeNav";
                var json = SafeGet(phoneUrl, ct);
                var res = JsonConvert.DeserializeObject<SFPerPhones>(json);
                if (res?.d?.results != null) sfPerPhoneList.AddRange(res.d.results);
                while (res?.d?.__next != null)
                {
                    json = SafeGet(res.d.__next, ct);
                    res = JsonConvert.DeserializeObject<SFPerPhones>(json);
                    if (res?.d?.results != null) sfPerPhoneList.AddRange(res.d.results);
                }
            }
            catch (Exception ex) { Log($"ERROR PerPhone: {ex.Message}"); }
            Log($"PerPhone fetched: {sfPerPhoneList.Count} records");

            // PerAddressDEFLT
            var sfPerAddressList = new List<SFPerAddressDEFLTResult>();
            try
            {
                var json = SafeGet($"{baseURL}PerAddressDEFLT", ct);
                var res = JsonConvert.DeserializeObject<SFPerAddressDEFLTs>(json);
                if (res?.d?.results != null) sfPerAddressList.AddRange(res.d.results);
            }
            catch (Exception ex) { Log($"ERROR PerAddressDEFLT: {ex.Message}"); }
            Log($"PerAddressDEFLT fetched: {sfPerAddressList.Count} records");

            // PerPersonal
            var sfPerPersonalList = new List<SFPerPersonalResult>();
            try
            {
                string personalUrl = CompanyCode.StartsWith("WM")
                    ? $"{baseURL}PerPersonal?$select=firstName,lastName,customString8,customString20,customString20Nav/externalCode,customString20Nav/picklistLabels/label,customString20Nav/picklistLabels/locale,customString23,customString23Nav/externalCode,customString23Nav/picklistLabels/label,customString23Nav/picklistLabels/locale,personIdExternal,startDate&$expand=customString20Nav/picklistLabels,customString23Nav/picklistLabels"
                    : $"{baseURL}PerPersonal";
                var json = SafeGet(personalUrl, ct);
                var res = JsonConvert.DeserializeObject<SFPerPersonals>(json);
                if (res?.d?.results != null) sfPerPersonalList.AddRange(res.d.results);
                while (res?.d?.__next != null)
                {
                    json = SafeGet(res.d.__next, ct);
                    res = JsonConvert.DeserializeObject<SFPerPersonals>(json);
                    if (res?.d?.results != null) sfPerPersonalList.AddRange(res.d.results);
                }
            }
            catch (Exception ex) { Log($"ERROR PerPersonal: {ex.Message}"); }
            Log($"PerPersonal fetched: {sfPerPersonalList.Count} records");

            // FOCompany
            var sfCompanyList = new List<SFFOCompanyResult>();
            try
            {
                var json = SafeGet($"{baseURL}FOCompany", ct);
                var res = JsonConvert.DeserializeObject<SFFOCompanys>(json);
                if (res?.d?.results != null) sfCompanyList.AddRange(res.d.results);
            }
            catch (Exception ex) { Log($"ERROR FOCompany: {ex.Message}"); }
            Log($"FOCompany fetched: {sfCompanyList.Count} records");

            // FODepartment
            var sfDepartmentList = new List<SFFODepartmentResult>();
            try
            {
                var json = SafeGet($"{baseURL}FODepartment", ct);
                var res = JsonConvert.DeserializeObject<SFFODepartments>(json);
                if (res?.d?.results != null) sfDepartmentList.AddRange(res.d.results);
            }
            catch (Exception ex) { Log($"ERROR FODepartment: {ex.Message}"); }
            Log($"FODepartment fetched: {sfDepartmentList.Count} records");

            // FOBusinessUnit
            var sfBusinessList = new List<SFFOBusinessUnitResult>();
            try
            {
                var json = SafeGet($"{baseURL}FOBusinessUnit", ct);
                var res = JsonConvert.DeserializeObject<SFFOBusinessUnits>(json);
                if (res?.d?.results != null) sfBusinessList.AddRange(res.d.results);
            }
            catch (Exception ex) { Log($"ERROR FOBusinessUnit: {ex.Message}"); }
            Log($"FOBusinessUnit fetched: {sfBusinessList.Count} records");

            // FODivision
            var sfDivisionList = new List<SFFODivisionResult>();
            try
            {
                var json = SafeGet($"{baseURL}FODivision", ct);
                var res = JsonConvert.DeserializeObject<SFFODivisions>(json);
                if (res?.d?.results != null) sfDivisionList.AddRange(res.d.results);
            }
            catch (Exception ex) { Log($"ERROR FODivision: {ex.Message}"); }
            Log($"FODivision fetched: {sfDivisionList.Count} records");

            // FOLocation
            var sfLocationList = new List<SFFOLocationResult>();
            try
            {
                var json = SafeGet($"{baseURL}FOLocation", ct);
                var res = JsonConvert.DeserializeObject<SFFOLocations>(json);
                if (res?.d?.results != null) sfLocationList.AddRange(res.d.results);
            }
            catch (Exception ex) { Log($"ERROR FOLocation: {ex.Message}"); }
            Log($"FOLocation fetched: {sfLocationList.Count} records");

            // Position
            var sfPositionList = new List<SFPositionResult>();
            try
            {
                var json = SafeGet($"{baseURL}Position", ct);
                var res = JsonConvert.DeserializeObject<SFPositions>(json);
                if (res?.d?.results != null) sfPositionList.AddRange(res.d.results);
            }
            catch (Exception ex) { Log($"ERROR Position: {ex.Message}"); }
            Log($"Position fetched: {sfPositionList.Count} records");

            try { SafeGet($"{baseURL}Custom", ct); } catch { }

            var mimOs = duserList.Select(x => new SFMIMObject(x)).ToList();

            LogSeparator();
            Log($"BEGIN per-user processing ({mimOs.Count} users)");
            LogSeparator();

            foreach (var o in mimOs.OrderBy(x => x.userId))
            {
                try
                {
                    LogSeparator();
                    Log($"USER: userId={o.userId} username={o.username} status={o.status}");

                    var allUserJobs = sfEmpJobList.Where(x => x.userId == o.userId).OrderByDescending(y => y.startDate).ToList();
                    LogUserJobs(o.userId, allUserJobs);

                    var allUserEmps = sfEmploymentList.Where(x => x.userId == o.userId).OrderByDescending(y => y.startDate).ToList();
                    LogUserEmployments(o.userId, allUserEmps);

                    // Block 1: jobs up to DaysAfterEnd days in the future
                    var job = sfEmpJobList
                        .Where(x => x.userId == o.userId && x.startDate.Date != x.endDate.Date && x.startDate.Date < DateTime.Now.AddDays(DaysAfterEnd))
                        .OrderByDescending(y => y.startDate).FirstOrDefault();
                    Log($"  Block1 (future<={DaysAfterEnd}d): matched job = {(job == null ? "NONE" : $"startDate={job.startDate:yyyy-MM-dd} endDate={job.endDate:yyyy-MM-dd}")}");
                    if (job != null)
                        FillFromJob(o, job, sfEmpJobCurrentNav, sfEmploymentList, sfPerPersonalList, sfCompanyList,
                            sfBusinessList, sfDivisionList, sfDepartmentList, sfLocationList, sfPositionList, "Block1");

                    // Block 2: current active jobs (now)
                    job = sfEmpJobList
                        .Where(x => x.userId == o.userId && x.startDate.Date != x.endDate.Date && x.startDate.Date < DateTime.Now && x.endDate > DateTime.Now)
                        .OrderByDescending(y => y.startDate).FirstOrDefault();
                    Log($"  Block2 (active now):  matched job = {(job == null ? "NONE" : $"startDate={job.startDate:yyyy-MM-dd} endDate={job.endDate:yyyy-MM-dd}")}");
                    if (job != null)
                        FillFromJob(o, job, sfEmpJobCurrentNav, sfEmploymentList, sfPerPersonalList, sfCompanyList,
                            sfBusinessList, sfDivisionList, sfDepartmentList, sfLocationList, sfPositionList, "Block2");

                    var pers = sfPerPersonalList.FirstOrDefault(x => x.personIdExternal == o.personIdExternal);
                    if (pers != null) { o.firstName = pers.firstName; o.lastName = pers.lastName; o.employeeNumber = pers.customString8; }

                    var person = sfPerPersonList.FirstOrDefault(x => x.personIdExternal == o.personIdExternal);
                    if (person != null) o.personCustomString1 = person.customString1;

                    var empPhones = sfPerPhoneList.Where(x => x.personIdExternal == o.personIdExternal).ToList();
                    var mob = empPhones.FirstOrDefault(x => x.phoneType == "1061");
                    if (mob != null) o.mobile = mob.phoneNumber?.Trim();
                    var bpn = empPhones.FirstOrDefault(x => x.phoneType == "1060");
                    if (bpn != null) o.businessPhone = bpn.phoneNumber?.Trim();
                    var hom = empPhones.FirstOrDefault(x => x.phoneType == "859");
                    if (hom != null) o.homePhone = hom.phoneNumber?.Trim();
                    var em = sfPerEmailList.FirstOrDefault(x => x.personIdExternal == o.personIdExternal && x.emailType == "1412");
                    if (em != null) o.emailAddress = em.emailAddress;

                    if (!CompanyCode.StartsWith("WM"))
                    {
                        mob = empPhones.FirstOrDefault(x => x.phoneType == "858");
                        if (mob != null) o.mobile = $"{mob.countryCode}{mob.phoneNumber}".Trim();
                        bpn = empPhones.FirstOrDefault(x => x.phoneType == "484");
                        if (bpn != null) o.businessPhone = $"{bpn.countryCode}{bpn.phoneNumber}".Trim();
                        hom = empPhones.FirstOrDefault(x => x.phoneType == "859");
                        if (hom != null) o.homePhone = $"{hom.countryCode}{hom.phoneNumber}".Trim();
                        em = sfPerEmailList.FirstOrDefault(x => x.personIdExternal == o.personIdExternal && x.emailType == "850");
                        if (em != null) o.emailAddress = em.emailAddress;
                    }

                    var adr = sfPerAddressList.FirstOrDefault(x => x.personIdExternal == o.personIdExternal);
                    if (adr != null) { o.address2 = adr.address2; o.address1 = adr.address1; o.zipCode = adr.zipCode; o.office = adr.city; o.country = adr.country; }

                    LogFinalMIMObject(o);
                }
                catch (Exception ex)
                {
                    Log($"ERROR processing userId={o.userId}: {ex.Message}");
                    Log($"  StackTrace: {ex.StackTrace}");
                }
            }

            var retMims = mimOs.ToList();
            if (CompanyCode.StartsWith("WM"))
                retMims = mimOs.Where(x => x.businessUnit != null).ToList();

            var lDistRetMims = retMims
                .GroupBy(x => x.personIdExternal, (_, g) => g.OrderBy(c => c.endDate).First())
                .ToList();

            LogSeparator();
            Log($"END GetApi - final MIMObject count returned: {lDistRetMims.Count}");
            LogSeparator();

            FlushLog();
            return lDistRetMims;
        }

        private void FillFromJob(
            SFMIMObject o,
            SFEmpJobResult job,
            Dictionary<string, Customstring2nav> sfEmpJobCurrentNav,
            List<SFEmpEmploymentResult> sfEmploymentList,
            List<SFPerPersonalResult> sfPerPersonalList,
            List<SFFOCompanyResult> sfCompanyList,
            List<SFFOBusinessUnitResult> sfBusinessList,
            List<SFFODivisionResult> sfDivisionList,
            List<SFFODepartmentResult> sfDepartmentList,
            List<SFFOLocationResult> sfLocationList,
            List<SFPositionResult> sfPositionList,
            string blockName)
        {
            var emps = sfEmploymentList.Where(x => x.userId == o.userId).OrderByDescending(y => y.startDate).ToList();
            var emp = emps.FirstOrDefault();

            o.originalStartDate = emp?.startDate;
            o.endDate = emp?.endDate;
            o.lastDateWorked = emp?.lastDateWorked;
            o.personIdExternal = emp?.personIdExternal;
            o.endDateJob = job.endDate;
            o.originalStartDateJob = job.startDate;

            var bu = job.businessUnit;

            if (emp?.jobInfoNav?.results != null)
            {
                var jobNavs = emp.jobInfoNav.results.OrderByDescending(y => y.startDate).ToList();
                var employeeClass = jobNavs.FirstOrDefault()?.employeeClassNav?.externalCode;
                var employmentType = jobNavs.FirstOrDefault()?.employmentTypeNav?.externalCode;
                o.employeeClassNoName = jobNavs.FirstOrDefault()?.employeeClassNav?.picklistLabels?.results?.FirstOrDefault(x => x.locale.Contains("NO"))?.label;
                o.employeeClassEnName = jobNavs.FirstOrDefault()?.employeeClassNav?.picklistLabels?.results?.FirstOrDefault(x => x.locale.Contains("en"))?.label;
                o.employmentTypeEnName = jobNavs.FirstOrDefault()?.employmentTypeNav?.picklistLabels?.results?.FirstOrDefault(x => x.locale.Contains("en"))?.label;
                o.employmentTypeNoName = jobNavs.FirstOrDefault()?.employmentTypeNav?.picklistLabels?.results?.FirstOrDefault(x => x.locale.Contains("NO"))?.label;
                o.employeeClassExternalCode = employeeClass;
                o.employmentTypeExternalCode = employmentType;
                o.BusinessCategory = $"{bu}\\{employeeClass}\\{employmentType}\\{o.isContingentWorker}";
            }

            o.isContingentWorker = emp?.isContingentWorker.ToString();
            o.Vendor = emp?.workOrderNav?.results?.FirstOrDefault()?.vendorNav?.vendorName;

            var pers = sfPerPersonalList.FirstOrDefault(x => x.personIdExternal == o.personIdExternal);
            o.ShowIntranetCode = pers?.customString23;
            o.ShowIntranetEnName = pers?.customString23Nav?.picklistLabels?.results?.FirstOrDefault(x => x.locale.Contains("en"))?.label;
            o.ShowIntranetNoName = pers?.customString23Nav?.picklistLabels?.results?.FirstOrDefault(x => x.locale.Contains("NO"))?.label;
            o.SystemAccessCode = pers?.customString20;
            o.SystemAccessEnName = pers?.customString20Nav?.picklistLabels?.results?.FirstOrDefault(x => x.locale.Contains("en"))?.label;
            o.SystemAccessNoName = pers?.customString20Nav?.picklistLabels?.results?.FirstOrDefault(x => x.locale.Contains("NO"))?.label;

            o.company = sfCompanyList.FirstOrDefault(x => x.externalCode == job.company)?.name_defaultValue;
            o.countryOfCompany = job.countryOfCompany;
            o.costCenter = $"{job.costCenter} {job.costCenterNav?.name}";

            if (CompanyCode.StartsWith("WM")) { o.customDate = job.customDate1; }
            if (!CompanyCode.StartsWith("WM")) { o.employeeTypeId = job.customString48; o.terminationAgreement = job.customString49; }

            o.employeeTypeText = job.customString7;
            o.employeeType = job.jobTitle;
            o.businessUnit = bu;
            o.businessUnitEnName = sfBusinessList.FirstOrDefault(x => x.externalCode == bu)?.name_en_GB;
            o.businessUnitNoName = sfBusinessList.FirstOrDefault(x => x.externalCode == bu)?.name_nb_NO;
            o.businessUnitDefaultName = sfBusinessList.FirstOrDefault(x => x.externalCode == bu)?.name_defaultValue;

            var divi = job.division;
            o.division = divi;
            o.divisionEnName = sfDivisionList.FirstOrDefault(x => x.externalCode == divi)?.name_en_GB;
            o.divisionNoName = sfDivisionList.FirstOrDefault(x => x.externalCode == divi)?.name_nb_NO;
            o.divisionDefaultName = sfDivisionList.FirstOrDefault(x => x.externalCode == divi)?.name_defaultValue;

            var de = job.department;
            o.department = de;
            o.departmentEnName = sfDepartmentList.FirstOrDefault(x => x.externalCode == de)?.name_en_GB;
            o.departmentNoName = sfDepartmentList.FirstOrDefault(x => x.externalCode == de)?.name_nb_NO;
            o.departmentDefaultName = sfDepartmentList.FirstOrDefault(x => x.externalCode == de)?.name_defaultValue;

            o.jobCustomString2 = job.customString2;
            o.jobCustomString27 = job.customString27;
            o.location = sfLocationList.FirstOrDefault(x => x.externalCode == job.location)?.name;
            o.manager = job.managerId;
            o.jobTitle = job.jobTitle;
            o.titleNo = sfPositionList.FirstOrDefault(x => x.code == job.position)?.externalName_nb_NO;
            o.titleEn = sfPositionList.FirstOrDefault(x => x.code == job.position)?.externalName_en_GB;
            o.titleDefault = sfPositionList.FirstOrDefault(x => x.code == job.position)?.externalName_defaultValue;

            // -------------------------------------------------------
            // Group name — prefer asOfDate nav (current name).
            // Fall back to snapshot nav from Call 1 for future users
            // not yet active today (e.g. startDate in 30 days).
            // -------------------------------------------------------
            sfEmpJobCurrentNav.TryGetValue(o.userId, out var currentNav);
            var nav = currentNav ?? job.customString2Nav;
            string navSource = currentNav != null ? "asOfDate" : "snapshot";

            o.customString2 = nav?.externalName_nb_NO;
            o.customString2EnName = nav?.externalName_en_GB;
            o.customString2DefaultName = nav?.externalName_defaultValue;
            Log($"  {blockName} customString2Nav: code={job.customString2} source={navSource} nb_NO={nav?.externalName_nb_NO} en_GB={nav?.externalName_en_GB}");

            o.BusinessUnitNoPath = $"{bu} {o.businessUnitNoName}\\{divi} {o.divisionNoName}\\{de} {o.departmentNoName}\\{job.customString2} {nav?.externalName_nb_NO}";
            o.BusinessUnitEnPath = $"{bu} {o.businessUnitEnName}\\{divi} {o.divisionEnName}\\{de} {o.departmentEnName}\\{job.customString2} {nav?.externalName_en_GB}";
            o.UnitCode = $"{bu}.{divi}.{de}.{job.customString2}";
        }

        #endregion

        #region Export

        private bool SendPostRequest(string url, string data, string doMethod, out string errorMessage)
        {
            errorMessage = "";
            bool retResult = false;
            try
            {
                _hClient.DefaultRequestHeaders.Accept.Clear();
                _hClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var requestContent = new StringContent(data, Encoding.UTF8, "application/json");
                var response = _hClient.PostAsync(url, requestContent).GetAwaiter().GetResult();
                retResult = response.IsSuccessStatusCode;
                errorMessage = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }
            catch { errorMessage = "BadRequest"; }
            return retResult;
        }

        public bool UpdateRecord(ExportData ed, out string error)
        {
            DecideAuth(CompanyCode);
            error = "";
            bool result = false;
            var baseURL = $"{_apiPoint}odata/v2/";

            if (!string.IsNullOrEmpty(ed.Email))
            {
                var s2 = new MEResult { __metadata = new Metadata { uri = $"PerEmail(emailType='1412',personIdExternal='{ed.Anchor}')" }, emailAddress = ed.Email };
                result = SendPostRequest($"{baseURL}upsert", JsonConvert.SerializeObject(s2), "POST", out error);
            }
            if (!string.IsNullOrEmpty(ed.Username))
            {
                var s2 = new MUResult { __metadata = new MetadataU { uri = $"User('{ed.Anchor}')" }, username = ed.Username };
                result = SendPostRequest($"{baseURL}upsert?processInactiveEmployees=true", JsonConvert.SerializeObject(new[] { s2 }), "POST", out error);
            }
            return result;
        }

        #endregion

        #region Ecma

        public bool MakeSchema()
        {
            if (GetEmployees)
                SchemaDef = CreateSchema("personIdExternal", "Person");
            return true;
        }

        private SchemaType CreateSchema(string anchor, string objectType)
        {
            var schemaDef = Microsoft.MetadirectoryServices.SchemaType.Create(objectType, false);
            schemaDef.Attributes.Add(SchemaAttribute.CreateAnchorAttribute(anchor, AttributeType.String));
            var specProps = new List<string> { anchor };

            foreach (PropertyInfo p in typeof(SFMIMObject).GetProperties())
            {
                if (!specProps.Contains(p.Name))
                {
                    var typeName = (Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType).Name;
                    schemaDef.Attributes.Add(typeName == "List`1"
                        ? SchemaAttribute.CreateMultiValuedAttribute(p.Name, AttributeType.String)
                        : SchemaAttribute.CreateSingleValuedAttribute(p.Name, GetAttrType(typeName)));
                }
            }
            return schemaDef;
        }

        private CSEntryChange FillCSEntry(SFMIMObject bol, string anchor, string objectType)
        {
            var csentry1 = CSEntryChange.Create();
            if (bol == null) return csentry1;

            csentry1.ObjectModificationType = ObjectModificationType.Add;
            csentry1.ObjectType = objectType;
            var specProps = new List<string> { anchor };

            foreach (PropertyInfo p in typeof(SFMIMObject).GetProperties())
            {
                var pValue = p.GetValue(bol);
                if (pValue == null) continue;
                var ob = p.PropertyType.Name == "Boolean" ? pValue : pValue.ToString();
                if (p.Name == anchor)
                    csentry1.AnchorAttributes.Add(AnchorAttribute.Create(p.Name, ob.ToString()));
                else if (!specProps.Contains(p.Name))
                    csentry1.AttributeChanges.Add(AttributeChange.CreateAttributeAdd(p.Name, ob));
            }
            return csentry1;
        }

        private void FillCSEntries(string jsonRoot, string anchor, string objectType)
        {
            EmpDic = new List<Dictionary<string, string>>();
            var emfi = new Dictionary<string, string>();
            var csentry = CSEntryChange.Create();
            csentry.ObjectModificationType = ObjectModificationType.Add;
            csentry.ObjectType = objectType;
            foreach (var empItem in emfi) { CSEntries.Add(csentry); EmpDic.Add(emfi); }
        }

        #endregion

        #region Helpers

        private AttributeType GetAttrType(string typeName)
        {
            switch (typeName)
            {
                case "Boolean": return AttributeType.Boolean;
                default: return AttributeType.String;
            }
        }

        private string SecureStringToString(SecureString value)
        {
            IntPtr bstr = Marshal.SecureStringToBSTR(value);
            try { return Marshal.PtrToStringBSTR(bstr); }
            finally { Marshal.FreeBSTR(bstr); }
        }

        #endregion
    }
}