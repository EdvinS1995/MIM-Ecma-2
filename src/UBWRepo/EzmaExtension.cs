using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.MetadirectoryServices;
using UBWRepo.Repo;

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
        //test

        #region Properties

        public int ExportDefaultPageSize { get; } = 100;

        public int ExportMaxPageSize { get; } = 1000;

        public int ImportDefaultPageSize { get; } = 3000;

        public int ImportMaxPageSize { get; } = 3000;

        public string LoginEndpoint;
        public string ApiEndpoint;


        public string ClientId;
        public System.Security.SecureString ClientSecret;

        public string Username;
        public System.Security.SecureString Password;

        //public string GetAbsence;
        public string GetEmployees;

        public string DaysBeforeStart;

        public string DaysAfterDisable;

        public string CompanyCode;

        public MACapabilities Capabilities
        {
            get
            {
                MACapabilities cap = new MACapabilities();
                cap.ConcurrentOperation = true;
                cap.DeleteAddAsReplace = true;
                cap.DeltaImport = true;
                cap.DistinguishedNameStyle = MADistinguishedNameStyle.None;
                cap.ExportType = MAExportType.ObjectReplace; //AttributeUpdate; //
                cap.FullExport = true;
                cap.ObjectConfirmation = MAObjectConfirmation.Normal;
                cap.ObjectRename = false;

                cap.NoReferenceValuesInFirstExport = true;
                cap.Normalizations = MANormalizations.None;

                return cap;
            }
        }

        #endregion



        #region Connectors

        public void OpenExportConnection(KeyedCollection<string, ConfigParameter> configParameters, Schema types, OpenExportConnectionRunStep exportRunStep)
        {
            this.ApiEndpoint = configParameters["ApiEndpoint"].Value;
            this.Username = configParameters["Username"].Value;
            this.Password = configParameters["Password"].SecureValue;
            this.GetEmployees = configParameters["GetEmployees"].Value;
            this.DaysAfterDisable = configParameters["DaysAfterDisable"].Value;
            this.DaysBeforeStart = configParameters["DaysBeforeStart"].Value;
            this.CompanyCode = configParameters["CompanyCode"].Value;
            SetEnvironment();
        }

        public void CloseExportConnection(CloseExportConnectionRunStep exportRunStep)
        {

        }

        public OpenImportConnectionResults OpenImportConnection(KeyedCollection<string, ConfigParameter> configParameters, Schema types, OpenImportConnectionRunStep importRunStep)
        {
            this.ApiEndpoint = configParameters["ApiEndpoint"].Value;
            this.Username = configParameters["Username"].Value;
            this.Password = configParameters["Password"].SecureValue;
            this.GetEmployees = configParameters["GetEmployees"].Value;
            this.DaysAfterDisable = configParameters["DaysAfterDisable"].Value;
            this.DaysBeforeStart = configParameters["DaysBeforeStart"].Value;
            this.CompanyCode = configParameters["CompanyCode"].Value;
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
            MIMRepo repo = new MIMRepo { ApiEndpoint = ApiEndpoint, Username = Username, Password = Password, GetEmployees = StringToBool(GetEmployees), CompanyCode = CompanyCode, DaysBeforeStart = int.Parse(DaysBeforeStart), DaysAfterDisable = int.Parse(DaysAfterDisable), CSEntries = csentries };
            repo.Put();

            return repo.ExportEntriesResults;
        }


        public GetImportEntriesResults GetImportEntries(GetImportEntriesRunStep importRunStep)
        {
            GetImportEntriesResults importReturnInfo;
            List<CSEntryChange> csentries = new List<CSEntryChange>();

            MIMRepo repo = new MIMRepo { ApiEndpoint = ApiEndpoint, Username = Username, Password = Password, GetEmployees = StringToBool(GetEmployees), CompanyCode = CompanyCode, DaysBeforeStart = int.Parse(DaysBeforeStart), DaysAfterDisable = int.Parse(DaysAfterDisable) };

            repo.Get();

            var c = repo.CSEntries;

            importReturnInfo = new GetImportEntriesResults { MoreToImport = false, CSEntries = c };
            return importReturnInfo;
        }

        public Schema GetSchema(KeyedCollection<string, ConfigParameter> configParameters)
        {
            this.ApiEndpoint = configParameters["ApiEndpoint"].Value;
            this.Username = configParameters["Username"].Value;
            this.Password = configParameters["Password"].SecureValue;
            this.GetEmployees = configParameters["GetEmployees"].Value;
            this.DaysAfterDisable = configParameters["DaysAfterDisable"].Value;
            this.DaysBeforeStart = configParameters["DaysBeforeStart"].Value;
            this.CompanyCode = configParameters["CompanyCode"].Value;

            MIMRepo repo = new MIMRepo {ApiEndpoint = ApiEndpoint,  Username = Username, Password = Password, GetEmployees = StringToBool(GetEmployees), CompanyCode = CompanyCode, DaysBeforeStart = int.Parse(DaysBeforeStart), DaysAfterDisable = int.Parse(DaysAfterDisable) };


            repo.MakeSchema();

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
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("ApiEndpoint", "", ""));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("CompanyCode", ""));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("Username", ""));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateEncryptedStringParameter("Password", ""));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateCheckBoxParameter("GetEmployees", true));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("DaysBeforeStart", "", "30"));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("DaysAfterDisable", "", "30"));

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
