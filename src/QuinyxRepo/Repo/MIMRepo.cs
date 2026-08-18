using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using Microsoft.MetadirectoryServices;
using QuinyxRepo.Entity;
using QuinyxRepo.QuinyxReference;

namespace QuinyxRepo.Repo
{
    public class MIMRepo
    {
        #region Properties


        public string ImportBaseUrl { get; set; }

        public string APIKey { get; set; }

        public bool GetAbsence { get; set; }

        public bool GetEmployees { get; set; }


        /// <summary>
        /// all entries
        /// </summary>
        public IList<CSEntryChange> CSEntries { get; set; }

        /// <summary>
        /// definition of schemna
        /// </summary>
        public Dictionary<string, SchemaType> SchemaDefs { get; set; }


        /// <summary>
        /// Result of export
        /// </summary>
        public PutExportEntriesResults ExportEntriesResults { get; set; }


        public List<EmployeeExt> QuinyxObjects { get; set; }

        public string JsonData { get; set; }


        #endregion


        #region Handlers

        public void Get()
        {
            var csen = new List<CSEntryChange>();
            if (GetEmployees)
            {
                csen.AddRange(GetEmps());
            }
            if (GetAbsence)
            {
                csen.AddRange(GetAbs());
            }

            CSEntries = csen;
        }

        private List<CSEntryChange> GetEmps()
        {
            var emps = FetchEmployees();
            QuinyxObjects = emps;
            string tempj = Newtonsoft.Json.JsonConvert.SerializeObject(emps);
            var ents = new List<CSEntryChange>();
            foreach (var emp in emps)
            {
                ents.Add(FillCSEntry(emp, "badgeNo", "Person"));
            }

            return ents;

        }

        private List<CSEntryChange> GetAbs()
        {
            var abss = FetchAbsence();
            var ents = new List<CSEntryChange>();
            foreach (var emp in abss)
            {
                ents.Add(FillCSEntry(emp, "AbsenceId", "absence"));
            }

            return ents;

        }


        public void Put()
        {
            ExportEntriesResults = new PutExportEntriesResults();
            var remoteAddress = new System.ServiceModel.EndpointAddress(ImportBaseUrl);
            var babi = new BasicHttpsBinding();
            babi.MaxReceivedMessageSize = 111131072;
            FlexForcePortTypeClient cli = new FlexForcePortTypeClient(babi, remoteAddress);

            var qKeys = cli.wsdlGetUnitsAPIKey(APIKey).ToList();

            var tem = new GetAgreementTemplatesV2();
            tem.extTemplateId = "";
            var agtempls = cli.wsdlGetAgreementTemplatesV2(APIKey, tem);
            var templs = agtempls.agreementTemplates.ToList();

            var agr = new GetAgreementsV2();
            var allAgrs = new List<AgreementV2>();
            foreach (var qKey in qKeys)
            {
                var agrs = cli.wsdlGetAgreementsV2(qKey.API_key, agr);
                var aglist = agrs.agreements.ToList();
                allAgrs.AddRange(aglist);
            }

            //var Jsonagrs = Newtonsoft.Json.JsonConvert.SerializeObject(allAgrs);

            //var prt = UpdateQObject(boob, "Update", cli, qKeys, templs, allAgrs, out string em);

            foreach (CSEntryChange csentryChange in CSEntries)
            {
                IList<AttributeChange> attributeChanges = new List<AttributeChange>();

                var status = Util.ErrValidate("BadRequest");
                string errorName = "";

                if (csentryChange.ObjectType == "Person")
                {
                    //try
                    //{
                    switch (csentryChange.ObjectModificationType)
                    {
                        case ObjectModificationType.Add:
                            {
                                var boo = FillQObject(csentryChange);

                                if (string.IsNullOrEmpty(boo.UnitExtId))
                                {
                                    ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, MAExportError.ExportErrorInvalidDN, "Quinyx error", "No Unit"));
                                    break;
                                }

                                //User Creation Code
                                var errorMessage = "";
                                var poststatus = UpdateQObject(boo, "Add", cli, qKeys, templs, allAgrs, out errorMessage);
                                if (!string.IsNullOrEmpty(errorMessage))
                                {
                                    errorName = "Quinyx error";
                                }

                                status = Util.ErrValidate(poststatus);
                                //attributeChanges.Add(AttributeChange.CreateAttributeAdd("employeeID", boo.employeeID));
                                ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, status, errorName, errorMessage));
                                break;
                            }
                        case ObjectModificationType.Delete:
                            {

                                //User Deletion Code
                                var poststatus = "BadRequest";
                                var errorMessage = "Not implemented";
                                if (!string.IsNullOrEmpty(errorMessage))
                                {
                                    errorName = "Quinyx error";
                                }

                                status = Util.ErrValidate(poststatus);
                                //attributeChanges.Add(AttributeChange.CreateAttributeDelete("employeeID"));
                                ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, status, errorName, errorMessage));

                                break;
                            }

                        case ObjectModificationType.Replace:
                            {
                                var boo = FillQObject(csentryChange);


                                if (string.IsNullOrEmpty(boo.badgeNo))
                                {
                                    ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, MAExportError.ExportErrorInvalidDN, "Quinyx error", "No badgeNo"));
                                    break;
                                }
                                var errorMessage = "";
                                var patchstatus = UpdateQObject(boo, "Update", cli, qKeys, templs, allAgrs, out errorMessage);
                                if (!string.IsNullOrEmpty(errorMessage))
                                {
                                    errorName = "Quinyx error";
                                }

                                status = Util.ErrValidate(patchstatus);
                                //attributeChanges.Add(AttributeChange.CreateAttributeReplace("employeeID", boo.employeeID));
                                ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, status, errorName, errorMessage));

                                break;
                            }

                        case ObjectModificationType.Update:
                            {
                                var boo = FillQObject(csentryChange);
                                if (string.IsNullOrEmpty(boo.badgeNo))
                                {
                                    ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, MAExportError.ExportErrorInvalidDN, "Quinyx error", "No badgeNo"));
                                    break;
                                }
                                var errorMessage = "";
                                var patchstatus = UpdateQObject(boo, "Update", cli, qKeys, templs, allAgrs, out errorMessage);
                                if (!string.IsNullOrEmpty(errorMessage))
                                {
                                    errorName = "Quinyx error";
                                }

                                status = Util.ErrValidate(patchstatus);
                                //attributeChanges.Add(AttributeChange.CreateAttributeReplace("employeeID", boo.employeeID));
                                ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, status, errorName, errorMessage));

                                break;
                            }
                        default:
                            break;
                    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(csentryChange.Identifier, attributeChanges, status, "Workday repo Error", ex.Message));
                    //}
                }

            }
        }

        #endregion



        #region Ecma

        public bool MakeSchema()
        {
            SchemaDefs = new Dictionary<string, SchemaType>();
            if (GetEmployees)
            {
                SchemaDefs.Add("Person", CreateSchema<EmployeeExt>("badegNo", "Person"));
            }
            if (GetAbsence)
            {
                SchemaDefs.Add("absence", CreateSchema<Absence>("AbsenceId", "absence"));
            }

            return true;
        }


        public SchemaType CreateSchema<T>(string anchor, string objectType)
        {
            var schemaDef = Microsoft.MetadirectoryServices.SchemaType.Create(objectType, false);

            schemaDef.Attributes.Add(SchemaAttribute.CreateAnchorAttribute(anchor, AttributeType.String));


            List<string> specProps = new List<string>();
            specProps.Add(anchor);

            var wdProps = typeof(T).GetProperties();

            foreach (PropertyInfo p in wdProps)
            {
                if (specProps.Contains(p.Name)) continue;
                Type type = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
                string typeName = type.Name;
                AttributeOperation op = new AttributeOperation();

                schemaDef.Attributes.Add(SchemaAttribute.CreateSingleValuedAttribute(p.Name, Util.GetType(typeName)));
            }

            return schemaDef;
        }


        private EmployeeExt FillQObject(CSEntryChange cse)
        {
            EmployeeExt bol = new EmployeeExt();

            var wvProps = typeof(EmployeeExt).GetProperties();

            foreach (PropertyInfo p in wvProps)
            {


                string propName = p.Name;

                Type typex = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
                string typeName = typex.Name;

                if (cse.AnchorAttributes.Contains(propName))
                {
                    var atch = cse.AnchorAttributes[propName];
                    if (atch != null)
                    {
                        var val = atch.Value;
                        p.SetValue(bol, val);
                    }
                }

                if (cse.AttributeChanges.Contains(propName))
                {
                    var atch = cse.AttributeChanges[propName];
                    var val = atch.ValueChanges.FirstOrDefault(x => x.ModificationType == ValueModificationType.Add);
                    if (val != null)
                    {
                        switch (typeName)
                        {
                            case "String":
                                {
                                    p.SetValue(bol, val.Value.ToString());
                                }
                                break;
                            case "DateTime":
                                {
                                    var dd = DateTime.Parse(val.Value.ToString());
                                    p.SetValue(bol, dd);
                                }
                                break;
                            case "Boolean":
                                {
                                    var dd = bool.Parse(val.Value.ToString());
                                    p.SetValue(bol, dd);
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }


            string dn = cse.DN;
            if (dn.Contains("/"))
            {
                string[] dnSplit = dn.Split('/');
                bol.badgeNo = dnSplit[0].Trim();
                bol.UnitExtId = dnSplit[1].Trim();
            }


            return bol;
        }

        private CSEntryChange FillCSEntry<T>(T bol, string anchor, string objectType)
        {
            CSEntryChange csentry1 = CSEntryChange.Create();
            if (bol == null)
            {
                return csentry1;
            }

            //csentry1.DN = $"{bol.badgeNo}/{bol.UnitExtId}";
            csentry1.ObjectModificationType = ObjectModificationType.Add;
            csentry1.ObjectType = objectType;

            var wdProps = typeof(T).GetProperties();

            foreach (PropertyInfo p in wdProps)
            {
                var pValue = p.GetValue(bol);
                var tp = p.PropertyType.Name;

                if (pValue != null)
                {
                    var ob = tp == "Boolean" ? pValue : pValue.ToString();
                    if (tp == "DateTime")
                    {
                        var tx = DateTime.Parse(pValue.ToString());
                        ob = tx.ToShortDateString();
                    }

                    if (p.Name == anchor)
                    {
                        csentry1.AnchorAttributes.Add(AnchorAttribute.Create(p.Name, ob.ToString()));
                    }
                    else
                    {
                        csentry1.AttributeChanges.Add(AttributeChange.CreateAttributeAdd(p.Name, ob));
                    }
                }
            }
            return csentry1;
        }


        #endregion




        #region Soap

        public List<Absence> FetchAbsence()
        {
            var remoteAddress = new System.ServiceModel.EndpointAddress(ImportBaseUrl);
            var babi = new BasicHttpsBinding();
            babi.MaxReceivedMessageSize = 111131072;
            FlexForcePortTypeClient cli = new FlexForcePortTypeClient(babi, remoteAddress);

            var abs = new List<Absence>();

            List<string> codes = new List<string>
            {
                "149",
                "129",
                "130",
                "110",
                "10",
                "189",
                "194"
            };

            var qKeys = cli.wsdlGetUnitsAPIKey(APIKey).ToList();
            var allLevs = new List<LeaveApplicationV2>();
            foreach (var qKey in qKeys)
            {
                var leavreq = new getLeaveApplicationsV2Request { };
                var leav = cli.wsdlGetLeaveApplicationsV2(qKey.API_key, new getLeaveApplicationsV2Request());
                var lev = leav.leaveApplications;
                allLevs.AddRange(lev.ToList());


                //var lax = lev.Where(x => x. && codes.Contains(x.extCode) && x.beenViewed == 1).Select(x => new Absence(x)).ToList();
                var la = lev.Select(x => new Absence(x)).ToList();
                abs.AddRange(la);
            }

            var JsonLev = Newtonsoft.Json.JsonConvert.SerializeObject(allLevs.Where(x => x.fromBadgeNo == "kh342"));
            var absLev = Newtonsoft.Json.JsonConvert.SerializeObject(abs.Where(x => x.ABadgeNo == "kh342"));

            if (QuinyxObjects?.Count > 0)
            {
                foreach (var ab in abs)
                {
                    ab.costCentre = QuinyxObjects.FirstOrDefault(x => x.badgeNo == ab.ABadgeNo)?.costCentre ?? 0;
                    ab.costCentreExtCode = QuinyxObjects.FirstOrDefault(x => x.badgeNo == ab.ABadgeNo)?.costCentreExtCode;
                    ab.UnitExtId = QuinyxObjects.FirstOrDefault(x => x.badgeNo == ab.ABadgeNo)?.UnitExtId;
                    ab.UnitName = QuinyxObjects.FirstOrDefault(x => x.badgeNo == ab.ABadgeNo)?.UnitName;
                    ab.FullEmploymentHrs = QuinyxObjects.FirstOrDefault(x => x.badgeNo == ab.ABadgeNo)?.FullEmploymentHrs ?? 0;
                    ab.EmploymentPercent = QuinyxObjects.FirstOrDefault(x => x.badgeNo == ab.ABadgeNo)?.EmploymentPercent;
                    ab.sex = QuinyxObjects.FirstOrDefault(x => x.badgeNo == ab.ABadgeNo)?.sex;
                    ab.TemplateExtId = QuinyxObjects.FirstOrDefault(x => x.badgeNo == ab.ABadgeNo)?.TemplateExtId;
                    ab.TemplateName = QuinyxObjects.FirstOrDefault(x => x.badgeNo == ab.ABadgeNo)?.TemplateName;
                    ab.Name = $"{QuinyxObjects.FirstOrDefault(x => x.badgeNo == ab.ABadgeNo)?.givenName} {QuinyxObjects.FirstOrDefault(x => x.badgeNo == ab.ABadgeNo)?.familyName}";
                }
            }

            //JsonData = Newtonsoft.Json.JsonConvert.SerializeObject(abs);
            return abs;
        }


        private List<EmployeeExt> FetchEmployees()
        {
            var remoteAddress = new System.ServiceModel.EndpointAddress(ImportBaseUrl);
            var babi = new BasicHttpsBinding();
            babi.MaxReceivedMessageSize = 111131072;
            FlexForcePortTypeClient cli = new FlexForcePortTypeClient(babi, remoteAddress);

            var tem = new GetAgreementTemplatesV2();
            tem.extTemplateId = "";
            var agtempls = cli.wsdlGetAgreementTemplatesV2(APIKey, tem);
            var templs = agtempls.agreementTemplates.ToList();



            //var templs = new List<AgreementTemplateV2>();


            //var templstring = templs.Select(x => x.id + " " + x.templateName);
            //string tempj = Newtonsoft.Json.JsonConvert.SerializeObject(templs);

            var allEmps = new List<EmployeeExt>();
            var allAgrs = new List<AgreementV2>();
            var allTracks = new List<ttBalanceItem>();

            var qKeys = cli.wsdlGetUnitsAPIKey(APIKey).ToList();

            //string tempUnits = Newtonsoft.Json.JsonConvert.SerializeObject(qKeys);

            var agr = new GetAgreementsV2();
            agr.extAgreementId = "";

            int[] neoid = new int[0];
            string[] neorol = new string[0];

            var allqemps = new List<Employee>();



            foreach (var qKey in qKeys)
            {


                var qeemps = cli.wsdlGetEmployeesV2(qKey.API_key, 0, 0, "", "", false, neoid, neorol).ToList();


                var emps = qeemps.Select(y => new EmployeeExt(y)).ToList(); //.Where(x => x.active == 1)
                if (emps.Any())
                {
                    allqemps.AddRange(qeemps);
                    allEmps.AddRange(emps);
                    var agrs = cli.wsdlGetAgreementsV2(qKey.API_key, agr);
                    var aglist = agrs.agreements.ToList();
                    allAgrs.AddRange(aglist);


                    //string jtrack = Newtonsoft.Json.JsonConvert.SerializeObject(trackList);

                    //var xxxx = trackList;
                }

            }

            foreach (var emp1 in allEmps) //.Where(x => x.badgeNo == "kh1948").ToList())
            {
                var agr1 = allAgrs.FirstOrDefault(x => x.badgeNo == emp1.badgeNo && x.isMainAgreement);
                if (agr1 != null)
                {
                    emp1.agreementId = agr1.id.ToString();
                    emp1.templateId = agr1.templateId.ToString();
                    emp1.TemplateExtId = agr1.extTemplateId;
                    emp1.FullEmploymentHrs = (int)agr1.fullEmploymentHrs;
                    var getTempl = templs.FirstOrDefault(x => x.id == agr1.templateId);
                    if (getTempl != null)
                    {
                        emp1.TemplateName = getTempl.templateName;
                    }
                    var emprate = agr1.employmentRates.FirstOrDefault();
                    if (emprate != null)
                    {
                        emp1.rate = (decimal)emprate.rate;
                        int decRate = (int)(emp1.rate * 100);
                        emp1.EmploymentPercent = $"{decRate}%";
                    }
                }
                var unit1 = qKeys.FirstOrDefault(x => x.id == int.Parse(emp1.restId));
                if (unit1 != null)
                {
                    emp1.UnitExtId = unit1.extUnitId;
                    emp1.UnitName = unit1.name; //userId = emp1.id

                    string balDate = $"{DateTime.Now.Year}-12-31";

                    var trackers = cli.wsdlGetTimeTrackerList(unit1.API_key, emp1.badgeNo, 1).ToList();

                    //var jsontrackers = Newtonsoft.Json.JsonConvert.SerializeObject(trackers);


                    List<QuinyxReference.ttBalanceItem> ttbs = new List<ttBalanceItem>
                    {
                        new ttBalanceItem {badgeNo = emp1.badgeNo,  timeTrackerId = 4667,date = balDate},
                        new ttBalanceItem {badgeNo = emp1.badgeNo,  timeTrackerId = 4668,date = balDate},
                        new ttBalanceItem {badgeNo = emp1.badgeNo,  timeTrackerId = 5586,date = balDate},
                        new ttBalanceItem {badgeNo = emp1.badgeNo,  timeTrackerId = 9275,date = balDate}
                    };



                    var tracks = cli.wsdlGetTimeTrackersBalance(unit1.API_key, ttbs.ToArray());
                    var trackList = tracks.ToList();
                    //var trajson = Newtonsoft.Json.JsonConvert.SerializeObject(trackList);
                    emp1.SaldoSyktBarn = trackList.FirstOrDefault(x => x.timeTrackerId == 9275)?.balance ?? 0;
                    emp1.SaldoFerie = trackList.FirstOrDefault(x => x.timeTrackerId == 4667)?.balance ?? 0;
                    emp1.SaldoTidbank = trackList.FirstOrDefault(x => x.timeTrackerId == 4668)?.balance ?? 0;
                    emp1.SaldoJubileumsuke = trackList.FirstOrDefault(x => x.timeTrackerId == 5586)?.balance ?? 0;


                    var startDateFirstJanuary = new DateTime(DateTime.Now.Year, 1, 1);


                    getTimePunchesRequest timPunc = new getTimePunchesRequest
                    {
                        badgeNo = emp1.badgeNo,
                        fromDate = startDateFirstJanuary,
                        toDate = DateTime.Now,
                        fromDateSpecified = true,
                        toDateSpecified = true,
                        shiftTaskInfo = true,
                        shiftTaskInfoSpecified = true

                    };
                    var timPunches100 = cli.wsdlGetTimePunches(unit1.API_key, timPunc);

                    List<string> overTimeTypes = new List<string>
                    {
                        "100",
                        "200",
                        "300",
                        "400",
                        };

                    var overTime = timPunches100.timepunches.Where(x => !string.IsNullOrEmpty(x.extShiftCategoryId) && overTimeTypes.Contains(x.extShiftCategoryId)).ToList();
                    var sumHours = overTime.Sum(x => x.hours);
                    emp1.SumOvertid = sumHours;


                    //var timpunJson = Newtonsoft.Json.JsonConvert.SerializeObject(timPunches100);

                    //var xxxx = timpunJson;
                }
            }

            return allEmps;
        }

        public string UpdateQObject(EmployeeExt emp, string Operation, FlexForcePortTypeClient cli, List<UnitKey> QKeys, List<AgreementTemplateV2> AgreementTemplates, List<AgreementV2> allAgrs, out string errorMessage)
        {
            errorMessage = "";
            SetUpdateClass sup = new SetUpdateClass();
            sup.Templs = AgreementTemplates;

            var unit1 = QKeys.FirstOrDefault(x => x.extUnitId == emp.UnitExtId);
            if (unit1 != null && emp.UnitExtId != null)
            {
                var empList = new List<UpdateEmployee>();
                var sem = sup.SetEmp(emp);

                if (sem != null)
                {
                    empList.Add(sem);
                }


                if (empList.Any())
                {
                    var rese = cli.wsdlUpdateEmployees(unit1.API_key, empList.ToArray());
                    var x = rese;
                }

                var agrs = new List<UpdateAgreementV2>();

                var sag = sup.SetAgr(emp, Operation, allAgrs);
                if (sag != null)
                {
                    agrs.Add(sag);
                }


                if (agrs.Any())
                {
                    var resa = cli.wsdlUpdateAgreementsV2(unit1.API_key, agrs.ToArray());
                    var x = resa;
                }
                return "OK";
            }

            return "NotFound";
        }

        public string UpdateQObjects(List<EmployeeExt> emps, out string errorMessage)
        {
            errorMessage = "";
            string code = "OK";
            var remoteAddress = new System.ServiceModel.EndpointAddress(ImportBaseUrl);
            var babi = new BasicHttpsBinding();
            babi.MaxReceivedMessageSize = 111131072;
            FlexForcePortTypeClient cli = new FlexForcePortTypeClient(babi, remoteAddress);

            var qKeys = cli.wsdlGetUnitsAPIKey(APIKey).ToList();

            var tem = new GetAgreementTemplatesV2();
            tem.extTemplateId = "";
            var agtempls = cli.wsdlGetAgreementTemplatesV2(APIKey, tem);
            var templs = agtempls.agreementTemplates.ToList();

            SetUpdateClass sup = new SetUpdateClass();
            sup.Templs = templs;

            foreach (var emp in emps)
            {
                var unit1 = qKeys.FirstOrDefault(x => x.extUnitId == emp.UnitExtId);
                if (unit1 != null)
                {
                    var empList = new List<UpdateEmployee>();

                    var sem = sup.SetEmp(emp);

                    //var jsem = Newtonsoft.Json.JsonConvert.SerializeObject(sem);

                    if (sem != null)
                    {
                        empList.Add(sem);
                    }
                    if (empList.Any())
                    {
                        var rese = cli.wsdlUpdateEmployees(unit1.API_key, empList.ToArray());
                        var x = rese;
                        //var jsemres = Newtonsoft.Json.JsonConvert.SerializeObject(rese);
                    }

                    var agrs = new List<UpdateAgreementV2>();
                    agrs.Add(sup.SetAgr(emp, "Add", new List<AgreementV2>()));
                    if (agrs.Any())
                    {
                        var resa = cli.wsdlUpdateAgreementsV2(unit1.API_key, agrs.ToArray());
                        var x = resa;
                    }
                }
            }

            return code;
        }


        private void GetEmpXML()
        {




        }

        #endregion

        #region Old

        //public void PutMany()
        //{
        //    ExportEntriesResults = new PutExportEntriesResults();

        //    string errorName = "";
        //    string errorMessage = "";
        //    var status = Util.ErrValidate("BadRequest");

        //    var boo = FillQObjects(CSEntries, out errorMessage);

        //    if (boo != null)
        //    {

        //        string res = UpdateQObjects(boo, out errorMessage);

        //        status = Util.ErrValidate(res);
        //    }

        //    if (!string.IsNullOrEmpty(errorMessage))
        //    {
        //        errorName = "Quinyx Repo error";
        //    }


        //    ExportEntriesResults.CSEntryChangeResults.Add(CSEntryChangeResult.Create(Guid.NewGuid(), new List<AttributeChange>(), status, errorName, errorMessage));
        //}


        #endregion

    }
}
