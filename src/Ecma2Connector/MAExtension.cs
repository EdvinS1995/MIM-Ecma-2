using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security;
using Ecma2Connector.Repo;
using Microsoft.MetadirectoryServices;

namespace Ecma2Connector
{
    // Interface surface confirmed against a working connector in this org (EzmaExtension,
    // namespace FimSync_Ezma): implicit interface implementation (no IFoo.Method prefixes),
    // Capabilities as a plain property, OpenExportConnection/CloseExportConnection return void.
    public class MAExtension :
        IMAExtensible2CallExport,
        IMAExtensible2CallImport,
        IMAExtensible2GetSchema,
        IMAExtensible2GetCapabilities,
        IMAExtensible2GetParameters
    {
        #region Properties

        public int ExportDefaultPageSize { get; } = 100;
        public int ExportMaxPageSize { get; } = 1000;
        public int ImportDefaultPageSize { get; } = 5000;
        public int ImportMaxPageSize { get; } = 5000;

        public string ApiEndpoint = string.Empty;
        public string Username = string.Empty;
        public SecureString Password;
        public SecureString BearerToken;

        public string ObjectType = "Person";
        public string AnchorAttributeName = "id";
        public string ListEndpoint = string.Empty;
        public string JsonRoot = string.Empty;

        public MACapabilities Capabilities
        {
            get
            {
                var cap = new MACapabilities();
                cap.ConcurrentOperation = true;
                cap.DeleteAddAsReplace = true;
                cap.DeltaImport = true;
                cap.DistinguishedNameStyle = MADistinguishedNameStyle.None;
                cap.ExportType = MAExportType.ObjectReplace; // AttributeUpdate is the alternative
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
            LoadConfig(configParameters);
        }

        public void CloseExportConnection(CloseExportConnectionRunStep exportRunStep)
        {
        }

        public OpenImportConnectionResults OpenImportConnection(KeyedCollection<string, ConfigParameter> configParameters, Schema types, OpenImportConnectionRunStep importRunStep)
        {
            LoadConfig(configParameters);
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
            var repo = BuildRepo();
            repo.CSEntries = csentries;
            repo.Put();

            return repo.ExportEntriesResults;
        }

        public GetImportEntriesResults GetImportEntries(GetImportEntriesRunStep importRunStep)
        {
            var repo = BuildRepo();
            repo.Get(ListEndpoint);

            return new GetImportEntriesResults { MoreToImport = false, CSEntries = repo.CSEntries };
        }

        public Schema GetSchema(KeyedCollection<string, ConfigParameter> configParameters)
        {
            LoadConfig(configParameters);
            var repo = BuildRepo();

            var schema = Schema.Create();
            schema.Types.Add(repo.MakeSchema(ListEndpoint));
            return schema;
        }

        public IList<ConfigParameterDefinition> GetConfigParameters(KeyedCollection<string, ConfigParameter> configParameters, ConfigParameterPage page)
        {
            var configParametersDefinitions = new List<ConfigParameterDefinition>();

            switch (page)
            {
                case ConfigParameterPage.Connectivity:
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("ApiEndpoint", "", "https://example.com/api/entities"));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("Username", ""));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateEncryptedStringParameter("Password", ""));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateEncryptedStringParameter("BearerToken", ""));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("ObjectType", "Person"));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("AnchorAttribute", "id"));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("ListEndpoint", ""));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("JsonRoot", ""));
                    break;
            }

            return configParametersDefinitions;
        }

        public ParameterValidationResult ValidateConfigParameters(KeyedCollection<string, ConfigParameter> configParameters, ConfigParameterPage page)
        {
            return new ParameterValidationResult();
        }

        #endregion

        #region Helpers

        private void LoadConfig(KeyedCollection<string, ConfigParameter> configParameters)
        {
            ApiEndpoint = configParameters["ApiEndpoint"].Value;
            Username = configParameters["Username"].Value;
            Password = configParameters["Password"].SecureValue;
            BearerToken = configParameters["BearerToken"].SecureValue;
            ObjectType = configParameters["ObjectType"].Value;
            AnchorAttributeName = configParameters["AnchorAttribute"].Value;
            ListEndpoint = configParameters["ListEndpoint"].Value;
            JsonRoot = configParameters["JsonRoot"].Value;
        }

        private MIMRepo BuildRepo()
        {
            return new MIMRepo
            {
                ApiEndpoint = ApiEndpoint,
                Username = Username,
                Password = Password,
                BearerToken = BearerToken,
                ObjectType = ObjectType,
                AnchorAttributeName = AnchorAttributeName,
                JsonRoot = JsonRoot
            };
        }

        #endregion
    }
}
