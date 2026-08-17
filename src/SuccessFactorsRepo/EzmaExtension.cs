using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.MetadirectoryServices;
using SuccessFactorsRepo.Repo;

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
        public EzmaExtension()
        {
        }

        #region Properties

        private int m_exportDefaultPageSize = 100;
        private int m_exportMaxPageSize = 1000;
        public int ExportDefaultPageSize => m_exportDefaultPageSize;
        public int ExportMaxPageSize => m_exportMaxPageSize;
        private int m_importDefaultPageSize = 3000;
        private int m_importMaxPageSize = 3000;
        public int ImportDefaultPageSize => m_importDefaultPageSize;
        public int ImportMaxPageSize => m_importMaxPageSize;

        public string CompanyCode;
        public string GetEmployees;
        public int DaysBeforeStart;
        public int DaysAfterEnd;

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

        public void OpenExportConnection(KeyedCollection<string, ConfigParameter> configParameters, Schema types, OpenExportConnectionRunStep exportRunStep)
        {
            this.CompanyCode = configParameters["CompanyCode"].Value;
            this.GetEmployees = configParameters["GetEmployees"].Value;
            this.DaysBeforeStart = int.Parse(configParameters["DaysBeforeStart"].Value);
            this.DaysAfterEnd = int.Parse(configParameters["DaysAfterEnd"].Value);
            SetEnvironment();
        }

        public void CloseExportConnection(CloseExportConnectionRunStep exportRunStep)
        {
        }

        public OpenImportConnectionResults OpenImportConnection(KeyedCollection<string, ConfigParameter> configParameters, Schema types, OpenImportConnectionRunStep importRunStep)
        {
            this.CompanyCode = configParameters["CompanyCode"].Value;
            this.GetEmployees = configParameters["GetEmployees"].Value;
            this.DaysBeforeStart = int.Parse(configParameters["DaysBeforeStart"].Value);
            this.DaysAfterEnd = int.Parse(configParameters["DaysAfterEnd"].Value);
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
            MIMRepo repo = new MIMRepo
            {
                CompanyCode = CompanyCode,
                GetEmployees = StringToBool(GetEmployees),
                DaysBeforeStart = DaysBeforeStart,
                DaysAfterEnd = DaysAfterEnd,
                CSEntries = csentries
            };
            repo.Put();
            return repo.ExportEntriesResults;
        }

        public GetImportEntriesResults GetImportEntries(GetImportEntriesRunStep importRunStep)
        {
            MIMRepo repo = new MIMRepo
            {
                CompanyCode = CompanyCode,
                GetEmployees = StringToBool(GetEmployees),
                DaysBeforeStart = DaysBeforeStart,
                DaysAfterEnd = DaysAfterEnd
            };
            repo.Get();

            return new GetImportEntriesResults { MoreToImport = false, CSEntries = repo.CSEntries };
        }

        public Schema GetSchema(KeyedCollection<string, ConfigParameter> configParameters)
        {
            this.CompanyCode = configParameters["CompanyCode"].Value;
            this.GetEmployees = configParameters["GetEmployees"].Value;
            this.DaysBeforeStart = int.Parse(configParameters["DaysBeforeStart"].Value);
            this.DaysAfterEnd = int.Parse(configParameters["DaysAfterEnd"].Value);

            MIMRepo repo = new MIMRepo
            {
                CompanyCode = CompanyCode,
                GetEmployees = StringToBool(GetEmployees),
                DaysBeforeStart = DaysBeforeStart,
                DaysAfterEnd = DaysAfterEnd
            };
            repo.MakeSchema();

            Schema schema = Schema.Create();
            schema.Types.Add(repo.SchemaDef);
            return schema;
        }

        public IList<ConfigParameterDefinition> GetConfigParameters(KeyedCollection<string, ConfigParameter> configParameters, ConfigParameterPage page)
        {
            List<ConfigParameterDefinition> configParametersDefinitions = new List<ConfigParameterDefinition>();
            switch (page)
            {
                case ConfigParameterPage.Connectivity:
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("CompanyCode", ""));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateCheckBoxParameter("GetEmployees", true));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("DaysBeforeStart", "", "30"));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("DaysAfterEnd", "", "30"));
                    break;
            }
            return configParametersDefinitions;
        }

        public ParameterValidationResult ValidateConfigParameters(KeyedCollection<string, ConfigParameter> configParameters, ConfigParameterPage page)
        {
            ParameterValidationResult myResults = new ParameterValidationResult();
            return myResults;
        }

        #endregion


        #region Helpers

        private void SetEnvironment()
        {
        }

        bool StringToBool(string cb)
        {
            return cb == "1";
        }

        #endregion
    };
}