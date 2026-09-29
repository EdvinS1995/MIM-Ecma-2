using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.MetadirectoryServices;
using HaileyRepo.Repo;

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

        public int ExportDefaultPageSize => 100;
        public int ExportMaxPageSize => 1000;
        public int ImportDefaultPageSize => 3000;
        public int ImportMaxPageSize => 3000;

        public string BaseUrl;
        public System.Security.SecureString AccessToken;

        public MACapabilities Capabilities
        {
            get
            {
                MACapabilities cap = new MACapabilities();
                cap.ConcurrentOperation = true;
                cap.DeleteAddAsReplace = true;
                // HaileyHR's GET /Employees has no date filter or pagination - every import run
                // is inherently a full snapshot, so MIM must do the delta-detection itself.
                cap.DeltaImport = false;
                cap.DistinguishedNameStyle = MADistinguishedNameStyle.None;
                // HaileyHR only exposes narrow, field-specific PUTs (CompanyEmail, custom fields),
                // never a full-object replace, so exports are partial attribute updates.
                cap.ExportType = MAExportType.AttributeUpdate;
                cap.FullExport = false;
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
            ReadConfigParameters(configParameters);
        }

        public void CloseExportConnection(CloseExportConnectionRunStep exportRunStep)
        {
        }

        public OpenImportConnectionResults OpenImportConnection(KeyedCollection<string, ConfigParameter> configParameters, Schema types, OpenImportConnectionRunStep importRunStep)
        {
            ReadConfigParameters(configParameters);
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
            MIMRepo repo = NewRepo();
            repo.CSEntries = csentries;
            repo.Put();
            return repo.ExportEntriesResults;
        }

        public GetImportEntriesResults GetImportEntries(GetImportEntriesRunStep importRunStep)
        {
            MIMRepo repo = NewRepo();
            repo.Get();
            return new GetImportEntriesResults { MoreToImport = false, CSEntries = repo.CSEntries };
        }

        public Schema GetSchema(KeyedCollection<string, ConfigParameter> configParameters)
        {
            ReadConfigParameters(configParameters);
            MIMRepo repo = NewRepo();
            repo.MakeSchema();

            Schema schema = Schema.Create();
            schema.Types.Add(repo.SchemaDef);
            return schema;
        }

        public IList<ConfigParameterDefinition> GetConfigParameters(KeyedCollection<string, ConfigParameter> configParameters, ConfigParameterPage page)
        {
            List<ConfigParameterDefinition> defs = new List<ConfigParameterDefinition>();
            switch (page)
            {
                case ConfigParameterPage.Connectivity:
                    defs.Add(ConfigParameterDefinition.CreateStringParameter("BaseUrl", "", "https://api.haileyhr.app"));
                    defs.Add(ConfigParameterDefinition.CreateEncryptedStringParameter("AccessToken", ""));
                    break;
            }
            return defs;
        }

        public ParameterValidationResult ValidateConfigParameters(KeyedCollection<string, ConfigParameter> configParameters, ConfigParameterPage page)
        {
            if (page == ConfigParameterPage.Connectivity)
            {
                if (string.IsNullOrWhiteSpace(configParameters["BaseUrl"].Value))
                    return new ParameterValidationResult(ParameterValidationResultCode.Failure, "BaseUrl is required", "BaseUrl");
            }
            return new ParameterValidationResult();
        }

        #endregion

        #region Helpers

        private void ReadConfigParameters(KeyedCollection<string, ConfigParameter> configParameters)
        {
            this.BaseUrl = configParameters["BaseUrl"].Value;
            this.AccessToken = configParameters["AccessToken"].SecureValue;
        }

        private MIMRepo NewRepo()
        {
            return new MIMRepo
            {
                BaseUrl = BaseUrl,
                AccessToken = AccessToken
            };
        }

        #endregion
    };
}
