using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.MetadirectoryServices;
using SympaRepo.Repo;

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

        public int ImportDefaultPageSize { get; } = 5000;

        public int ImportMaxPageSize { get; } = 5000;

        public string LoginEndpoint = string.Empty;
        public string ApiEndpoint = string.Empty;


        public System.Security.SecureString ClientSecret;

        public string ClientId = string.Empty;
        public string GetEmployees = string.Empty;

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
            this.LoginEndpoint = configParameters["LoginEndpoint"].Value;
            this.ClientId = configParameters["ClientId"].Value;
            this.ClientSecret = configParameters["ClientSecret"].SecureValue;
            this.GetEmployees = configParameters["GetEmployees"].Value;
            SetEnvironment();
        }

        public void CloseExportConnection(CloseExportConnectionRunStep exportRunStep)
        {

        }

        public OpenImportConnectionResults OpenImportConnection(KeyedCollection<string, ConfigParameter> configParameters, Schema types, OpenImportConnectionRunStep importRunStep)
        {
            this.ApiEndpoint = configParameters["ApiEndpoint"].Value;
            this.LoginEndpoint = configParameters["LoginEndpoint"].Value;
            this.ClientId = configParameters["ClientId"].Value;
            this.ClientSecret = configParameters["ClientSecret"].SecureValue;
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
            MIMRepo repo = new MIMRepo { LoginEndpoint = LoginEndpoint, ClientId = ClientId, ApiEndpoint = ApiEndpoint, ClientSecret = ClientSecret, GetEmployees = StringToBool(GetEmployees), CSEntries = csentries };
            repo.Put();

            return repo.ExportEntriesResults;
        }


        public GetImportEntriesResults GetImportEntries(GetImportEntriesRunStep importRunStep)
        {
            GetImportEntriesResults importReturnInfo;
            List<CSEntryChange> csentries = new List<CSEntryChange>();

            MIMRepo repo = new MIMRepo { LoginEndpoint = LoginEndpoint, ClientId = ClientId, ApiEndpoint = ApiEndpoint, ClientSecret = ClientSecret, GetEmployees = StringToBool(GetEmployees) };
            repo.Get();

            var c = repo.CSEntries;

            importReturnInfo = new GetImportEntriesResults { MoreToImport = false, CSEntries = c };
            return importReturnInfo;
        }

        public Schema GetSchema(KeyedCollection<string, ConfigParameter> configParameters)
        {
            this.ApiEndpoint = configParameters["ApiEndpoint"].Value;
            this.LoginEndpoint = configParameters["LoginEndpoint"].Value;
            this.ClientId = configParameters["ClientId"].Value;
            this.ClientSecret = configParameters["ClientSecret"].SecureValue;
            this.GetEmployees = configParameters["GetEmployees"].Value;

            MIMRepo repo = new MIMRepo { LoginEndpoint = LoginEndpoint, ClientId = ClientId, ApiEndpoint = ApiEndpoint, ClientSecret = ClientSecret, GetEmployees = StringToBool(GetEmployees) };

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
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("LoginEndpoint", "", " https://elvia-uat.ifs.cloud/auth/realms/elviuat1/protocol/openid-connect/token"));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("ApiEndpoint", "", "https://elvia-uat.ifs.cloud/main/ifsapplications/projection/v1/"));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("ClientId", ""));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateEncryptedStringParameter("ClientSecret", ""));
                    //configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("Username", ""));
                    //configParametersDefinitions.Add(ConfigParameterDefinition.CreateEncryptedStringParameter("APIKey", ""));
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
