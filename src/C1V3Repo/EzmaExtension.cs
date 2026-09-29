using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.MetadirectoryServices;
using C1V3Repo.Repo;

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
		private int m_importDefaultPageSize = 3000;
		private int m_importMaxPageSize = 3000;
		///public int ExportDefaultPageSize { get; }
		///public int ExportMaxPageSize { get; }
		public int ImportDefaultPageSize => m_importDefaultPageSize;
		public int ImportMaxPageSize => m_importMaxPageSize;

		public string BaseUrl;
		public string ClientId;
		public System.Security.SecureString ClientSecret;

		public string GetAbsence;
		public string GetEmployees;
		public string IncludeDisabled;
		public string IncludeInactive;
		public string HidePropName;


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
				cap.ExportType = MAExportType.AttributeUpdate; //ObjectReplace; //
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
			this.BaseUrl = configParameters["BaseUrl"].Value;
			this.ClientId = configParameters["ClientId"].Value;
			this.ClientSecret = configParameters["ClientSecret"].SecureValue;
			this.GetAbsence = configParameters["GetAbsence"].Value;
			this.GetEmployees = configParameters["GetEmployees"].Value;
            this.HidePropName = configParameters["HidePropName"].Value;
            this.IncludeDisabled = configParameters["IncludeDisabled"].Value;
            this.IncludeInactive = configParameters["IncludeInactive"].Value;
            SetEnvironment();
		}

		public void CloseExportConnection(CloseExportConnectionRunStep exportRunStep)
		{
			var b = 0;
		}

		public OpenImportConnectionResults OpenImportConnection(KeyedCollection<string, ConfigParameter> configParameters, Schema types, OpenImportConnectionRunStep importRunStep)
		{
			this.BaseUrl = configParameters["BaseUrl"].Value;
			this.ClientId = configParameters["ClientId"].Value;
			this.ClientSecret = configParameters["ClientSecret"].SecureValue;
			this.GetAbsence = configParameters["GetAbsence"].Value;
			this.GetEmployees = configParameters["GetEmployees"].Value;
            this.HidePropName = configParameters["HidePropName"].Value;
            this.IncludeDisabled = configParameters["IncludeDisabled"].Value;
            this.IncludeInactive = configParameters["IncludeInactive"].Value;
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
			MIMRepo repo = new MIMRepo { BaseUrl = BaseUrl, ClientID = ClientId, ClientSecret = ClientSecret, GetAbsence = StringToBool(GetAbsence), GetEmployees = StringToBool(GetEmployees), IncludeDisabled = StringToBool(IncludeDisabled), IncludeInactive = StringToBool(IncludeInactive), HidePropName = StringToBool(HidePropName), CSEntries = csentries };
			repo.Put();

			return repo.ExportEntriesResults;
		}


		public GetImportEntriesResults GetImportEntries(GetImportEntriesRunStep importRunStep)
		{
			GetImportEntriesResults importReturnInfo;
			List<CSEntryChange> csentries = new List<CSEntryChange>();

			MIMRepo repo = new MIMRepo { BaseUrl = BaseUrl, ClientID = ClientId, ClientSecret = ClientSecret, GetAbsence = StringToBool(GetAbsence), GetEmployees = StringToBool(GetEmployees), IncludeDisabled = StringToBool(IncludeDisabled), IncludeInactive = StringToBool(IncludeInactive), HidePropName = StringToBool(HidePropName)};
			repo.Get();

			var c = repo.CSEntries;

			importReturnInfo = new GetImportEntriesResults { MoreToImport = false, CSEntries = c };
			return importReturnInfo;
		}

		public Schema GetSchema(KeyedCollection<string, ConfigParameter> configParameters)
		{
			this.BaseUrl = configParameters["BaseUrl"].Value;
			this.ClientId = configParameters["ClientId"].Value;
			this.ClientSecret = configParameters["ClientSecret"].SecureValue;
			this.GetAbsence = configParameters["GetAbsence"].Value;
			this.GetEmployees = configParameters["GetEmployees"].Value;
            this.HidePropName = configParameters["HidePropName"].Value;
            this.IncludeDisabled = configParameters["IncludeDisabled"].Value;
            this.IncludeInactive = configParameters["IncludeInactive"].Value;

            MIMRepo repo = new MIMRepo { BaseUrl = BaseUrl, ClientID = ClientId, ClientSecret = ClientSecret, GetAbsence = StringToBool(GetAbsence), GetEmployees = StringToBool(GetEmployees), IncludeDisabled = StringToBool(IncludeDisabled), IncludeInactive = StringToBool(IncludeInactive), HidePropName = StringToBool(HidePropName) };

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
					configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("BaseUrl", ""));
					configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("ClientId", ""));
					configParametersDefinitions.Add(ConfigParameterDefinition.CreateEncryptedStringParameter("ClientSecret", ""));
					configParametersDefinitions.Add(ConfigParameterDefinition.CreateCheckBoxParameter("GetAbsence"));
					configParametersDefinitions.Add(ConfigParameterDefinition.CreateCheckBoxParameter("GetEmployees"));
					configParametersDefinitions.Add(ConfigParameterDefinition.CreateCheckBoxParameter("IncludeDisabled"));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateCheckBoxParameter("IncludeInactive"));
                    configParametersDefinitions.Add(ConfigParameterDefinition.CreateCheckBoxParameter("HidePropName"));
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
