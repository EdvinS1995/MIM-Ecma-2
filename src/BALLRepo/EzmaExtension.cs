
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml;
using System.Text;
using System.Collections.Specialized;
using System.Reflection;
using Microsoft.MetadirectoryServices;
using BALLRepo.Repo;
using System.Diagnostics;

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
        private int m_importDefaultPageSize = 15000;
        private int m_importMaxPageSize = 15000;
        ///public int ExportDefaultPageSize { get; }
        ///public int ExportMaxPageSize { get; }
        public int ImportDefaultPageSize => m_importDefaultPageSize;
        public int ImportMaxPageSize => m_importMaxPageSize;

        public string BaseUrl;
        public string CompanyCode;
        public string RequestedBy;
        public string RequestLanguage;
        public string AnchorName;
        public string AdHocCST;
        public string GetAppGroups;
        public string GetGroups;
        public string GetOptions;

        

        protected OperationType exportType;
        protected int exportBatchSize;



        public MACapabilities Capabilities
        {
            get
            {
                MACapabilities cap = new MACapabilities();
                cap.ConcurrentOperation = true;
                cap.DeleteAddAsReplace = true;
                cap.DeltaImport = true;
                cap.DistinguishedNameStyle = MADistinguishedNameStyle.None;
                cap.ExportType = MAExportType.AttributeUpdate; //AttributeUpdate; //
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
            BaseUrl = configParameters["BaseUrl"].Value;
            RequestedBy = configParameters["RequestedBy"].Value;
            RequestLanguage = configParameters["RequestLanguage"].Value;
            CompanyCode = configParameters["CompanyCode"].Value;
            AnchorName = configParameters["AnchorName"].Value;
            AdHocCST = configParameters["AdHocCST"].Value;
            GetAppGroups = configParameters["GetAppGroups"].Value;
            GetGroups = configParameters["GetGroups"].Value;
            GetOptions = configParameters["GetOptions"].Value;

            if (string.IsNullOrEmpty(AnchorName))
            {
                AnchorName = "employeeID";
            }

            exportType = exportRunStep.ExportType;
            exportBatchSize = exportRunStep.BatchSize;
            try
            {

            }
            catch (Exception ex)
            {
                throw new TerminateRunException(ex.Message);
            }
        }

        public void CloseExportConnection(CloseExportConnectionRunStep exportRunStep)
        {
            
        }

        public OpenImportConnectionResults OpenImportConnection(KeyedCollection<string, ConfigParameter> configParameters, Schema types, OpenImportConnectionRunStep importRunStep)
        {
            BaseUrl = configParameters["BaseUrl"].Value;
            CompanyCode = configParameters["CompanyCode"].Value;
            RequestedBy = configParameters["RequestedBy"].Value;
            RequestLanguage = configParameters["RequestLanguage"].Value;
            AnchorName = configParameters["AnchorName"].Value;
            AdHocCST = configParameters["AdHocCST"].Value;
            GetAppGroups = configParameters["GetAppGroups"].Value;
            GetGroups = configParameters["GetGroups"].Value;
            GetOptions = configParameters["GetOptions"].Value;

            if (string.IsNullOrEmpty(AnchorName))
            {
                AnchorName = "employeeID";
            }

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
            BRepo repo = new BRepo { BaseUrl = BaseUrl, CompanyCode = CompanyCode, CSEntries = csentries, RequestLanguage = RequestLanguage, RequestedBy = RequestedBy, AnchorName = AnchorName, AdHocCST = AdHocCST };

            repo.Put();

            return repo.ExportEntriesResults;

        }


        public GetImportEntriesResults GetImportEntries(GetImportEntriesRunStep importRunStep)
        {
            List<CSEntryChange> csentries = new List<CSEntryChange>();

            BRepo repo = new BRepo { BaseUrl = BaseUrl, CompanyCode = CompanyCode, RequestLanguage = RequestLanguage, RequestedBy = RequestedBy, AnchorName = AnchorName, GetOptions = GetOptions, GetGroups = GetGroups, GetAppGroups=GetAppGroups };
            repo.Get();

            var c = repo.CSEntries;

            GetImportEntriesResults importReturnInfo = new GetImportEntriesResults { MoreToImport = false, CSEntries = c };
            return importReturnInfo;
        }

        public Schema GetSchema(KeyedCollection<string, ConfigParameter> configParameters)
        {
            BaseUrl = configParameters["BaseUrl"].Value;
            RequestedBy = configParameters["RequestedBy"].Value;
            RequestLanguage = configParameters["RequestLanguage"].Value;
            CompanyCode = configParameters["CompanyCode"].Value;
            AnchorName = configParameters["AnchorName"].Value;
            AdHocCST = configParameters["AdHocCST"].Value;
            GetAppGroups = configParameters["GetAppGroups"].Value;
            GetGroups = configParameters["GetGroups"].Value;
            GetOptions = configParameters["GetOptions"].Value;

            if (string.IsNullOrEmpty(AnchorName))
            {
                AnchorName = "employeeID";
            }

            BRepo repo = new BRepo { BaseUrl = BaseUrl, CompanyCode = CompanyCode, AnchorName = AnchorName };
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
                case
                    ConfigParameterPage.Connectivity:
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("BaseUrl", "", "https://rot.intility.com/Ballapi/api/mimuser/"));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("CompanyCode", ""));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("RequestedBy", ""));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("RequestLanguage", "","no"));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("AnchorName", "","employeeID"));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("AdHocCST", "", ""));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateCheckBoxParameter("GetAppGroups"));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateCheckBoxParameter("GetGroups"));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateCheckBoxParameter("GetOptions"));
                    break;
                    //case ConfigParameterPage.Global:
                    //configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("GetBaseUrl", ""));
                    //configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("CompanyCode", ""));
                    //break;
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



        #endregion




    };
}
