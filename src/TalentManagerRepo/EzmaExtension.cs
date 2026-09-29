using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.MetadirectoryServices;
using TalentManagerRepo.Repo;

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

		#region EcmaStuff

		public int ExportDefaultPageSize { get; } = 100;
		public int ExportMaxPageSize { get; } = 1000;
		public int ImportDefaultPageSize { get; } = 3000;
		public int ImportMaxPageSize { get; } = 3000;

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


		#region Properties



		public string ApiEndpoint;
		public System.Security.SecureString ApiKey;
		public string CustomerAlias;
        public string Anchor;
        public string GetEmployees;
        public int DaysBeforeStart;
        public int DaysAfterTerminate;


        #endregion



        #region Connectors

        public void OpenExportConnection(KeyedCollection<string, ConfigParameter> configParameters, Schema types, OpenExportConnectionRunStep exportRunStep)
		{
			this.ApiEndpoint = configParameters["ApiEndpoint"].Value;
			this.CustomerAlias = configParameters["CustomerAlias"].Value;
			this.ApiKey = configParameters["ApiKey"].SecureValue;
            this.Anchor = configParameters["Anchor"].Value;
            this.GetEmployees = configParameters["GetEmployees"].Value;
            this.DaysBeforeStart = int.Parse(configParameters["DaysBeforeStart"].Value);
            this.DaysAfterTerminate = int.Parse(configParameters["DaysAfterTerminate"].Value);
            SetEnvironment();
		}

		public void CloseExportConnection(CloseExportConnectionRunStep exportRunStep)
		{

		}

		public OpenImportConnectionResults OpenImportConnection(KeyedCollection<string, ConfigParameter> configParameters, Schema types, OpenImportConnectionRunStep importRunStep)
		{
            this.ApiEndpoint = configParameters["ApiEndpoint"].Value;
            this.CustomerAlias = configParameters["CustomerAlias"].Value;
            this.ApiKey = configParameters["ApiKey"].SecureValue;
            this.Anchor = configParameters["Anchor"].Value;
            this.GetEmployees = configParameters["GetEmployees"].Value;
            this.DaysBeforeStart = int.Parse(configParameters["DaysBeforeStart"].Value);
            this.DaysAfterTerminate = int.Parse(configParameters["DaysAfterTerminate"].Value);
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
				ApiEndpoint = ApiEndpoint,
				CustomerAlias = CustomerAlias,
				ApiKey = ApiKey,
				GetEmployees = StringToBool(GetEmployees),
                Anchor = Anchor,
                CSEntries = csentries,
                DaysBeforeStart = DaysBeforeStart,
                DaysAfterTerminate = DaysAfterTerminate

            }; 
			
			repo.Put();

			return repo.ExportEntriesResults;
		}


		public GetImportEntriesResults GetImportEntries(GetImportEntriesRunStep importRunStep)
		{
			GetImportEntriesResults importReturnInfo;
			List<CSEntryChange> csentries = new List<CSEntryChange>();

			MIMRepo repo = new MIMRepo
			{
				ApiEndpoint = ApiEndpoint,
				CustomerAlias = CustomerAlias,
				ApiKey = ApiKey,
				Anchor = Anchor,
				GetEmployees = StringToBool(GetEmployees),
                DaysBeforeStart = DaysBeforeStart,
                DaysAfterTerminate = DaysAfterTerminate
            }; 
			
			repo.Get();

			var c = repo.CSEntries;

			importReturnInfo = new GetImportEntriesResults { MoreToImport = false, CSEntries = c };
			return importReturnInfo;
		}

		public Schema GetSchema(KeyedCollection<string, ConfigParameter> configParameters)
		{
			this.ApiEndpoint = configParameters["ApiEndpoint"].Value;
			this.CustomerAlias = configParameters["CustomerAlias"].Value;
            this.Anchor = configParameters["Anchor"].Value;
            this.ApiKey = configParameters["ApiKey"].SecureValue;
			this.GetEmployees = configParameters["GetEmployees"].Value;
            this.DaysBeforeStart = int.Parse(configParameters["DaysBeforeStart"].Value);
            this.DaysAfterTerminate = int.Parse(configParameters["DaysAfterTerminate"].Value);

            MIMRepo repo = new MIMRepo
			{
				ApiEndpoint = ApiEndpoint,
				CustomerAlias = CustomerAlias,
				ApiKey = ApiKey,
				Anchor = Anchor,
				GetEmployees = StringToBool(GetEmployees),
                DaysBeforeStart = DaysBeforeStart,
                DaysAfterTerminate = DaysAfterTerminate

            };

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
					configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("ApiEndpoint", "", "https://manager-api.hr-manager.net/restful.svc/"));
					configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("CustomerAlias", ""));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("Anchor", ""));
					configParametersDefinitions.Add(ConfigParameterDefinition.CreateEncryptedStringParameter("ApiKey", ""));
					configParametersDefinitions.Add(ConfigParameterDefinition.CreateCheckBoxParameter("GetEmployees", true));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("DaysBeforeStart", "", "60"));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("DaysAfterTerminate", "", "90"));
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




	}
}
