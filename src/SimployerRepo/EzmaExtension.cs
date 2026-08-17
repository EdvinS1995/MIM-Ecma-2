using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.MetadirectoryServices;
using SimployerRepo.Repo;

namespace FimSync_Ezma
{
    public class EzmaExtension :
            IMAExtensible2CallExport,
            IMAExtensible2CallImport,
            IMAExtensible2GetSchema,
            IMAExtensible2GetCapabilities,
            IMAExtensible2GetParameters
    {
        public EzmaExtension()
        {
        }

        #region Properties

        public int ExportDefaultPageSize { get; } = 100;
        public int ExportMaxPageSize { get; } = 1000;
        public int ImportDefaultPageSize { get; } = 9000;
        public int ImportMaxPageSize { get; } = 9000;

        // ── SIMPLOYER HRCONNECT ──────────────────────────────────────────────
        public string ApiType = string.Empty;
        public string ClientId = string.Empty;
        public string ClientSecret = string.Empty;
        public string BaseUrl = string.Empty;
        public string AuthUrl = string.Empty;

        // ── GAMMELT API ──────────────────────────────────────────────────────
        public string ApiEndpoint = string.Empty;
        public string Username = string.Empty;
        public string Password = string.Empty;
        public string Anchor = string.Empty;
        public string PublicCustomerId = string.Empty;
        public string ApiKey = string.Empty;
        public string SubscriptionKey = string.Empty;
        public string ImportUnitIds = string.Empty;
        public string ExportUnitId = string.Empty;
        public string IncludeSV = string.Empty;
        public string UnitIdType = string.Empty;
        public string UpdateUsername = string.Empty;

        // ── FELLES ──────────────────────────────────────────────────────────
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
                cap.ExportType = MAExportType.ObjectReplace;
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

        public void OpenExportConnection(
            KeyedCollection<string, ConfigParameter> configParameters,
            Schema types,
            OpenExportConnectionRunStep exportRunStep)
        {
            LoadConfig(configParameters);
            SetEnvironment();
        }

        public void CloseExportConnection(CloseExportConnectionRunStep exportRunStep)
        {
        }

        public OpenImportConnectionResults OpenImportConnection(
            KeyedCollection<string, ConfigParameter> configParameters,
            Schema types,
            OpenImportConnectionRunStep importRunStep)
        {
            LoadConfig(configParameters);
            SetEnvironment();
            return new OpenImportConnectionResults();
        }

        public CloseImportConnectionResults CloseImportConnection(
            CloseImportConnectionRunStep importRunStep)
        {
            return new CloseImportConnectionResults();
        }

        #endregion

        #region Handlers

        public PutExportEntriesResults PutExportEntries(IList<CSEntryChange> csentries)
        {
            MIMRepo repo = BuildRepo();
            repo.CSEntries = csentries;
            repo.Put();
            return repo.ExportEntriesResults;
        }

        public GetImportEntriesResults GetImportEntries(GetImportEntriesRunStep importRunStep)
        {
            MIMRepo repo = BuildRepo();
            repo.Get();

            return new GetImportEntriesResults
            {
                MoreToImport = false,
                CSEntries = repo.CSEntries
            };
        }

        public Schema GetSchema(KeyedCollection<string, ConfigParameter> configParameters)
        {
            LoadConfig(configParameters);

            MIMRepo repo = BuildRepo();
            repo.MakeSchema();

            Schema schema = Schema.Create();
            foreach (var sch in repo.SchemaDefs)
                schema.Types.Add(sch.Value);

            return schema;
        }

        public IList<ConfigParameterDefinition> GetConfigParameters(
            KeyedCollection<string, ConfigParameter> configParameters,
            ConfigParameterPage page)
        {
            var configParametersDefinitions = new List<ConfigParameterDefinition>();

            switch (page)
            {
                case ConfigParameterPage.Connectivity:

                    // ── VELG API-TYPE ────────────────────────────────────────
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateDropDownParameter(
                        "ApiType",
                        new string[] { "Simployer HRConnect", "Gammelt API" },
                        false,
                        "Simployer HRConnect"));

                    // ── SIMPLOYER HRCONNECT API ──────────────────────────────
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateDividerParameter());
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateLabelParameter("--- Simployer HRConnect API ---"));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("ClientId", "", ""));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("ClientSecret", "", ""));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("BaseUrl", "", "https://hrconnect.simployer.com"));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("AuthUrl", "", "https://simplauth.simployer.com/oauth/token"));

                    // ── GAMMELT API ──────────────────────────────────────────
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateDividerParameter());
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateLabelParameter("--- Gammelt API ---"));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("ApiEndpoint", "", "https://simployermim.azure-api.net/"));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("Username", ""));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateEncryptedStringParameter("Password", ""));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("Anchor", "", "PersonId"));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateEncryptedStringParameter("PublicCustomerId", ""));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateEncryptedStringParameter("ApiKey", ""));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("SubscriptionKey", "", "7785bd56f7b2450990774304e6aed77f"));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("ImportUnitIds", ""));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("ExportUnitId", ""));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("IncludeSV", "", "Room number:RoomNumber"));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("UnitIdType", "", "InternalId"));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateCheckBoxParameter("UpdateUsername", false));

                    // ── FELLES ───────────────────────────────────────────────
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateDividerParameter());
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateCheckBoxParameter("GetEmployees", true));

                    break;
            }

            return configParametersDefinitions;
        }

        public ParameterValidationResult ValidateConfigParameters(
            KeyedCollection<string, ConfigParameter> configParameters,
            ConfigParameterPage page)
        {
            return new ParameterValidationResult();
        }

        #endregion

        #region Helpers

        private void LoadConfig(KeyedCollection<string, ConfigParameter> configParameters)
        {
            // Felles
            ApiType = GetConfigValue(configParameters, "ApiType", "Simployer HRConnect");
            GetEmployees = GetConfigValue(configParameters, "GetEmployees", "1");

            // Simployer HRConnect
            ClientId = GetConfigValue(configParameters, "ClientId");
            ClientSecret = GetConfigValue(configParameters, "ClientSecret");
            BaseUrl = GetConfigValue(configParameters, "BaseUrl", "https://hrconnect.simployer.com");
            AuthUrl = GetConfigValue(configParameters, "AuthUrl", "https://simplauth.simployer.com/oauth/token");

            // Gammelt API
            ApiEndpoint = GetConfigValue(configParameters, "ApiEndpoint");
            Username = GetConfigValue(configParameters, "Username");
            Password = GetConfigValue(configParameters, "Password");
            Anchor = GetConfigValue(configParameters, "Anchor", "PersonId");
            PublicCustomerId = GetConfigValue(configParameters, "PublicCustomerId");
            ApiKey = GetConfigValue(configParameters, "ApiKey");
            SubscriptionKey = GetConfigValue(configParameters, "SubscriptionKey");
            ImportUnitIds = GetConfigValue(configParameters, "ImportUnitIds");
            ExportUnitId = GetConfigValue(configParameters, "ExportUnitId");
            IncludeSV = GetConfigValue(configParameters, "IncludeSV");
            UnitIdType = GetConfigValue(configParameters, "UnitIdType");
            UpdateUsername = GetConfigValue(configParameters, "UpdateUsername", "0");
        }

        private MIMRepo BuildRepo()
        {
            var repo = new MIMRepo
            {
                GetEmployees = StringToBool(GetEmployees)
            };

            if (ApiType == "Simployer HRConnect" || !string.IsNullOrEmpty(ClientId))
            {
                // ── Simployer HRConnect API ──────────────────────────────────
                repo.UseSimployer = true;
                repo.ClientId = ClientId;
                repo.ClientSecret = ClientSecret;
                repo.BaseUrl = BaseUrl;
                repo.AuthUrl = AuthUrl;
            }
            else
            {
                // ── Gammelt API ──────────────────────────────────────────────
                repo.UseSimployer = false;
                repo.ApiEndpoint = ApiEndpoint;
                repo.Username = Username;
                repo.Password = Password;
                repo.Anchor = Anchor;
                repo.PublicCustomerId = PublicCustomerId;
                repo.ApiKey = ApiKey;
                repo.SubscriptionKey = SubscriptionKey;
                repo.ImportUnitIds = ImportUnitIds;
                repo.ExportUnitId = ExportUnitId;
                repo.IncludeSV = IncludeSV;
                repo.UnitIdType = UnitIdType;
                repo.UpdateUsername = StringToBool(UpdateUsername);
            }

            return repo;
        }

        private string GetConfigValue(
            KeyedCollection<string, ConfigParameter> configParameters,
            string key,
            string defaultValue = "")
        {
            try
            {
                if (configParameters.Contains(key))
                    return configParameters[key].Value ?? defaultValue;
                return defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        private void SetEnvironment()
        {
        }

        private bool StringToBool(string cb)
        {
            return cb == "1";
        }

        #endregion
    }
}