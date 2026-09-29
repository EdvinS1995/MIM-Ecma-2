using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.MetadirectoryServices;
using AzureRepo.Repo;

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

        public System.Security.SecureString JWTToken;

        //public string GetAbsence;
        public string GetEmployees;

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
            //this.ClientId = configParameters["ClientId"].Value;
            //this.ClientSecret = configParameters["ClientSecret"].SecureValue;
            //this.Username = configParameters["Username"].Value;
            //this.JWTToken = configParameters["JWTToken"].SecureValue;
            this.GetEmployees = configParameters["GetEmployees"].Value;
            SetEnvironment();
        }

        public void CloseExportConnection(CloseExportConnectionRunStep exportRunStep)
        {

        }

        public OpenImportConnectionResults OpenImportConnection(KeyedCollection<string, ConfigParameter> configParameters, Schema types, OpenImportConnectionRunStep importRunStep)
        {
            this.ApiEndpoint = configParameters["ApiEndpoint"].Value;
            //this.ClientId = configParameters["ClientId"].Value;
            //this.ClientSecret = configParameters["ClientSecret"].SecureValue;
            //this.Username = configParameters["Username"].Value;
            //this.JWTToken = configParameters["JWTToken"].SecureValue;
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
            MIMRepo repo = new MIMRepo { LoginEndpoint = LoginEndpoint, ApiEndpoint = ApiEndpoint,   ClientId=ClientId, ClientSecret=ClientSecret, GetEmployees = StringToBool(GetEmployees) };
            repo.Put();

            return repo.ExportEntriesResults;
        }


        public GetImportEntriesResults GetImportEntries(GetImportEntriesRunStep importRunStep)
        {
            GetImportEntriesResults importReturnInfo;
            List<CSEntryChange> csentries = new List<CSEntryChange>();

            MIMRepo repo = new MIMRepo { LoginEndpoint = LoginEndpoint, ApiEndpoint = ApiEndpoint, ClientId = ClientId, ClientSecret = ClientSecret, GetEmployees = StringToBool(GetEmployees) };
            repo.Get();

            var c = repo.CSEntries;

            importReturnInfo = new GetImportEntriesResults { MoreToImport = false, CSEntries = c };
            return importReturnInfo;
        }

        public Schema GetSchema(KeyedCollection<string, ConfigParameter> configParameters)
        {
            //this.LoginEndpoint = configParameters["LoginEndpoint"].Value;
            //this.ClientId = configParameters["ClientId"].Value;
            //this.ClientSecret = configParameters["ClientSecret"].SecureValue;
            this.ApiEndpoint = configParameters["ApiEndpoint"].Value;
            //this.Username = configParameters["Username"].Value;
            //this.JWTToken = configParameters["JWTToken"].SecureValue;
            this.GetEmployees = configParameters["GetEmployees"].Value;

            MIMRepo repo = new MIMRepo {LoginEndpoint = LoginEndpoint, ApiEndpoint = ApiEndpoint, ClientId = ClientId, ClientSecret = ClientSecret, GetEmployees = StringToBool(GetEmployees) };

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
                    //configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("LoginEndpoint", "", "https://test.salesforce.com/services/oauth2/token"));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("ApiEndpoint", "", "https://datahub-9c66ce90.sesam.cloud/api/publishers/ansatt-activedirectory-endpoint/entities"));
                    //configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("ClientId", ""));
                    //configParametersDefinitions.Add(ConfigParameterDefinition.CreateEncryptedStringParameter("ClientSecret", ""));
                    //configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("Username", ""));
                    //configParametersDefinitions.Add(ConfigParameterDefinition.CreateEncryptedStringParameter("JWTToken", ""));
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
