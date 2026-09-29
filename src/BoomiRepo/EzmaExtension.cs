
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using BoomiRepo.Repo;
using Microsoft.MetadirectoryServices;



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
        private int m_importDefaultPageSize = 5000;
        private int m_importMaxPageSize = 5000;

        public int ImportDefaultPageSize => m_importDefaultPageSize;
        public int ImportMaxPageSize => m_importMaxPageSize;

        public string ImportBaseUrl;
        public string ImportClientId;
        public string ImportClientSecret;
        public string ExportBaseUrl;
        public string ExportClientId;
        public string ExportClientSecret;

        public MACapabilities Capabilities
        {
            get
            {
				MACapabilities cap = new MACapabilities();
                //            myCapabilities.ConcurrentOperation = true;
                //            myCapabilities.ObjectRename = false;
                //            myCapabilities.DeleteAddAsReplace = true;
                //            myCapabilities.DeltaImport = true;
                //            myCapabilities.DistinguishedNameStyle = MADistinguishedNameStyle.None;
                //            myCapabilities.ExportType = MAExportType.AttributeReplace;
                //myCapabilities.NoReferenceValuesInFirstExport = false;
                //            myCapabilities.Normalizations = MANormalizations.None;
                cap.ConcurrentOperation = true;
                cap.DeleteAddAsReplace = true;
                cap.DeltaImport = true;
                cap.DistinguishedNameStyle = MADistinguishedNameStyle.None;
                cap.ExportType = MAExportType.AttributeReplace; //AttributeUpdate; //
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
            this.ExportBaseUrl = configParameters["ExportBaseUrl"].Value;
            this.ExportClientId = configParameters["ExportClientId"].Value;
            this.ExportClientSecret = configParameters["ExportClientSecret"].Value;
            SetEnvironment();
        }

        public void CloseExportConnection(CloseExportConnectionRunStep exportRunStep)
        {
            var d = 12;
        }

        public OpenImportConnectionResults OpenImportConnection(KeyedCollection<string, ConfigParameter> configParameters, Schema types, OpenImportConnectionRunStep importRunStep)
        {
            this.ImportBaseUrl = configParameters["ImportBaseUrl"].Value;
            this.ImportClientId = configParameters["ImportClientId"].Value;
            this.ImportClientSecret = configParameters["ImportClientSecret"].Value;

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

            BoRepo repo = new BoRepo { ExportBaseUrl = ExportBaseUrl, ExportClientId = ExportClientId, ExportClientSecret = ExportClientSecret,  CSEntries = csentries};
            repo.Put();

            return repo.ExportEntriesResults;
        }


        public GetImportEntriesResults GetImportEntries(GetImportEntriesRunStep importRunStep)
        {
            List<CSEntryChange> csentries = new List<CSEntryChange>();

            BoRepo repo = new BoRepo { ImportBaseUrl = ImportBaseUrl, ImportClientId = ImportClientId, ImportClientSecret = ImportClientSecret};
            repo.Get();

            var c = repo.CSEntries;

            GetImportEntriesResults importReturnInfo = new GetImportEntriesResults { MoreToImport = false, CSEntries = c };
            return importReturnInfo;
        }

        public Schema GetSchema(KeyedCollection<string, ConfigParameter> configParameters)
        {
            //this.ImportBaseUrl = configParameters["ImportBaseUrl"].Value;
            //this.ImportClientId = configParameters["ImportClientId"].Value;
            //this.ImportClientSecret = configParameters["ImportClientSecret"].Value;

            BoRepo repo = new BoRepo { ImportBaseUrl = ImportBaseUrl, ImportClientId = ImportClientId, ImportClientSecret = ImportClientSecret};
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
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("ImportBaseUrl", ""));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("ImportClientId", ""));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("ImportClientSecret", ""));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("ExportBaseUrl", ""));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("ExportClientId", ""));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("ExportClientSecret", ""));
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



        #endregion




    };
}
