using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.MetadirectoryServices;
using BambooRepo.Repo;

namespace FimSync_Ezma
{
    public class EzmaExtension :
            IMAExtensible2CallExport,
            IMAExtensible2CallImport,
            //IMAExtensible2FileImport,
            //IMAExtensible2FileExport,
            //IMAExtensible2GetHierarchy,
            IMAExtensible2GetSchema,
            IMAExtensible2GetCapabilities,
            IMAExtensible2GetParameters
    //IMAExtensible2GetPartitions
    {
        //
        // Constructor
        //
        public EzmaExtension()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #region EcmaStuff

        public int ExportDefaultPageSize { get; } = 100;
        public int ExportMaxPageSize { get; } = 1000;
        public int ImportDefaultPageSize { get; } = 3000;
        public int ImportMaxPageSize { get; } = 3000;

        public MACapabilities Capabilities
        {
            get
            {
                MACapabilities cap = new MACapabilities();
                cap.ConcurrentOperation = true;
                cap.DeleteAddAsReplace = true;
                cap.DeltaImport = true;
                cap.DistinguishedNameStyle = MADistinguishedNameStyle.None;
                cap.ExportType = MAExportType.AttributeUpdate; //
                cap.FullExport = true;
                cap.ObjectConfirmation = MAObjectConfirmation.Normal;
                cap.ObjectRename = false;

                cap.NoReferenceValuesInFirstExport = true;
                cap.Normalizations = MANormalizations.None;

                return cap;
            }
        }

        #endregion


        #region Properties



        public string ApiEndpoint;
        public System.Security.SecureString ApiKey;
        public string CompanyCode;
        public string ReportName;
        public string GetEmployees;

        #endregion



        #region Connectors

        public void OpenExportConnection(KeyedCollection<string, ConfigParameter> configParameters, Schema types, OpenExportConnectionRunStep exportRunStep)
        {
            this.ApiEndpoint = configParameters["ApiEndpoint"].Value;
            this.ReportName = configParameters["ReportName"].Value;
            this.CompanyCode = configParameters["CompanyCode"].Value;
            this.ApiKey = configParameters["ApiKey"].SecureValue;
            this.GetEmployees = configParameters["GetEmployees"].Value;
            SetEnvironment();
        }

        public void CloseExportConnection(CloseExportConnectionRunStep exportRunStep)
        {

        }

        public OpenImportConnectionResults OpenImportConnection(KeyedCollection<string, ConfigParameter> configParameters, Schema types, OpenImportConnectionRunStep importRunStep)
        {
            this.ApiEndpoint = configParameters["ApiEndpoint"].Value;
            this.ReportName = configParameters["ReportName"].Value;
            this.CompanyCode = configParameters["CompanyCode"].Value;
            this.ApiKey = configParameters["ApiKey"].SecureValue;
            this.GetEmployees = configParameters["GetEmployees"].Value;

            SetEnvironment();
            return new OpenImportConnectionResults();
        }

        public CloseImportConnectionResults CloseImportConnection(CloseImportConnectionRunStep importRunStep)
        {
            return new CloseImportConnectionResults();
        }

        #endregion


        #region Handlers

        public PutExportEntriesResults PutExportEntries(IList<CSEntryChange> csentries)
        {
            MIMRepo repo = new MIMRepo { CSEntries = csentries, ApiEndpoint = ApiEndpoint, ApiKey = ApiKey, CompanyCode = CompanyCode, ReportName = ReportName, GetEmployees = StringToBool(GetEmployees) };
            repo.Put();

            return repo.ExportEntriesResults;
        }


        public GetImportEntriesResults GetImportEntries(GetImportEntriesRunStep importRunStep)
        {
            GetImportEntriesResults importReturnInfo;
            List<CSEntryChange> csentries = new List<CSEntryChange>();

            MIMRepo repo = new MIMRepo { ApiEndpoint = ApiEndpoint, ApiKey = ApiKey, CompanyCode = CompanyCode, ReportName = ReportName, GetEmployees = StringToBool(GetEmployees) };
            repo.Get();

            var c = repo.CSEntries;

            importReturnInfo = new GetImportEntriesResults { MoreToImport = false, CSEntries = c };
            return importReturnInfo;
        }

        public Schema GetSchema(KeyedCollection<string, ConfigParameter> configParameters)
        {
            this.ApiEndpoint = configParameters["ApiEndpoint"].Value;
            this.ReportName = configParameters["ReportName"].Value;
            this.CompanyCode = configParameters["CompanyCode"].Value;
            this.ApiKey = configParameters["ApiKey"].SecureValue;
            this.GetEmployees = configParameters["GetEmployees"].Value;

            MIMRepo repo = new MIMRepo { ApiEndpoint = ApiEndpoint, ApiKey = ApiKey, CompanyCode = CompanyCode, ReportName = ReportName, GetEmployees = StringToBool(GetEmployees) };

            repo.MakeJSSchema();

            Schema schema = Schema.Create();
            foreach (var sch in repo.SchemaDefs)
            {
                schema.Types.Add(sch.Value);
            }

            return schema;
        }


        public IList<ConfigParameterDefinition> GetConfigParameters(KeyedCollection<string, ConfigParameter> configParameters, ConfigParameterPage page)
        {
            List<ConfigParameterDefinition> configParametersDefinitions = new List<ConfigParameterDefinition>();
            switch (page)
            {
                case
                    ConfigParameterPage.Connectivity:
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("ApiEndpoint", "", "https://api.bamboohr.com/api/gateway.php/bahras/v1/reports/custom?format=JSON&onlyCurrent=false"));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateEncryptedStringParameter("ApiKey", ""));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("CompanyCode", ""));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("ReportName", ""));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateCheckBoxParameter("GetEmployees", true));

                    break;
                    //case ConfigParameterPage.Global:
                    // break;
                    //case ConfigParameterPage.Partition:
                    // break;
                    //case ConfigParameterPage.RunStep:
                    // break;
            }
            return configParametersDefinitions;
        }

        public ParameterValidationResult ValidateConfigParameters(KeyedCollection<string, ConfigParameter> configParameters, ConfigParameterPage page)
        {
            ParameterValidationResult myResults = new ParameterValidationResult();
            return myResults;
        }

        #endregion


        #region Parameters




        #endregion


        #region Helpers


        private void SetEnvironment()
        {

        }

        bool StringToBool(string cb)
        {
            if (cb == "1")
            {
                return true;
            }

            return false;
        }


        #endregion




    };
}
