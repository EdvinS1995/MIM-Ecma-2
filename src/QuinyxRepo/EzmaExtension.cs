using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.MetadirectoryServices;
using QuinyxRepo.Repo;


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

        private int m_exportDefaultPageSize = 100;
        private int m_exportMaxPageSize = 1000;
        public int ExportDefaultPageSize => m_exportDefaultPageSize;
        public int ExportMaxPageSize => m_exportMaxPageSize;
        private int m_importDefaultPageSize = 9000;
        private int m_importMaxPageSize = 9000;
        private int m_importcount;
        private int m_totalcount;
        private int m_pagesize;
        ///public int ExportDefaultPageSize { get; }
        ///public int ExportMaxPageSize { get; }
        public int ImportDefaultPageSize => m_importDefaultPageSize;
        public int ImportMaxPageSize => m_importMaxPageSize;

        private IList<CSEntryChange> allCsentries = new List<CSEntryChange>();

        public string APIKey;
        public string ImportBaseUrl;

        public string GetAbsence;
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
            this.ImportBaseUrl = configParameters["ImportBaseUrl"].Value;
            this.APIKey = configParameters["APIKey"].Value;
            this.GetAbsence = configParameters["GetAbsence"].Value;
            this.GetEmployees = configParameters["GetEmployees"].Value;
            SetEnvironment();
        }

        public void CloseExportConnection(CloseExportConnectionRunStep exportRunStep)
        {
            var x = 0;
        }

        public OpenImportConnectionResults OpenImportConnection(KeyedCollection<string, ConfigParameter> configParameters, Schema types, OpenImportConnectionRunStep importRunStep)
        {
            this.ImportBaseUrl = configParameters["ImportBaseUrl"].Value;
            this.APIKey = configParameters["APIKey"].Value;
            this.GetAbsence = configParameters["GetAbsence"].Value;
            this.GetEmployees = configParameters["GetEmployees"].Value;
            MIMRepo repo = new MIMRepo { ImportBaseUrl = ImportBaseUrl, APIKey = APIKey, GetEmployees = StringToBool(GetEmployees), GetAbsence = StringToBool(GetAbsence) };

            repo.Get();
            allCsentries = repo.CSEntries;
            m_totalcount = repo.CSEntries.Count;
            m_importcount = 0;
            m_pagesize = importRunStep.PageSize;

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
            MIMRepo repo = new MIMRepo { ImportBaseUrl = ImportBaseUrl,  APIKey = APIKey, CSEntries = csentries, GetEmployees = StringToBool(GetEmployees), GetAbsence = StringToBool(GetAbsence) };
            repo.Put();

            return repo.ExportEntriesResults;
        }


        public GetImportEntriesResults GetImportEntries(GetImportEntriesRunStep importRunStep)
        {
            GetImportEntriesResults importReturnInfo = new GetImportEntriesResults();

            int skipRecords = m_importcount;
            int takeRecords = m_pagesize;
            if (skipRecords + takeRecords > m_totalcount)
            {
                takeRecords = m_totalcount - skipRecords;
            }

            var c = allCsentries.Skip(skipRecords).Take(takeRecords).ToList();
            m_importcount = c.Count;

            importReturnInfo = new GetImportEntriesResults { MoreToImport = m_importcount < m_totalcount, CSEntries = c };
            return importReturnInfo;
        }

        public Schema GetSchema(KeyedCollection<string, ConfigParameter> configParameters)
        {
            this.ImportBaseUrl = configParameters["ImportBaseUrl"].Value;
            this.APIKey = configParameters["APIKey"].Value;
            this.GetAbsence = configParameters["GetAbsence"].Value;
            this.GetEmployees = configParameters["GetEmployees"].Value;

            MIMRepo repo = new MIMRepo { ImportBaseUrl = ImportBaseUrl,  APIKey = APIKey, GetEmployees=StringToBool(GetEmployees), GetAbsence= StringToBool(GetAbsence) };


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
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("ImportBaseUrl", "", "https://app.quinyx.com/FlexForceWebServices.php?"));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("APIKey", ""));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateCheckBoxParameter("GetAbsence"));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateCheckBoxParameter("GetEmployees"));
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
