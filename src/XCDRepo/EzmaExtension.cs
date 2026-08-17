using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.MetadirectoryServices;
using XCDRepo.Repo;

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

		public string LoginEndpoint;
		public string ApiEndpoint;

		public string ClientId;
		public System.Security.SecureString ClientSecret;

		public string Username;
		public System.Security.SecureString Password;
        public System.Security.SecureString SecToken { get; set; }


        //public string GetAbsence;
        public string GetEmployees;

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
			this.LoginEndpoint = configParameters["LoginEndpoint"].Value;
			this.ApiEndpoint = configParameters["ApiEndpoint"].Value;
			this.ClientId = configParameters["ClientId"].Value;
			this.ClientSecret = configParameters["ClientSecret"].SecureValue;
			this.Username = configParameters["Username"].Value;
			this.Password = configParameters["Password"].SecureValue;
			this.SecToken = configParameters["SecurityToken"].SecureValue;
			this.GetEmployees = configParameters["GetEmployees"].Value;
			SetEnvironment();
		}

		public void CloseExportConnection(CloseExportConnectionRunStep exportRunStep)
		{

		}

		public OpenImportConnectionResults OpenImportConnection(KeyedCollection<string, ConfigParameter> configParameters, Schema types, OpenImportConnectionRunStep importRunStep)
		{
			this.LoginEndpoint = configParameters["LoginEndpoint"].Value;
			this.ApiEndpoint = configParameters["ApiEndpoint"].Value;
			this.ClientId = configParameters["ClientId"].Value;
			this.ClientSecret = configParameters["ClientSecret"].SecureValue;
			this.Username = configParameters["Username"].Value;
			this.Password = configParameters["Password"].SecureValue;
			this.SecToken = configParameters["SecurityToken"].SecureValue;
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
			MIMRepo repo = new MIMRepo { LoginEndpoint = LoginEndpoint, ApiEndpoint = ApiEndpoint, ClientID = ClientId, ClientSecret = ClientSecret, Username = Username, Password = Password, SecToken = SecToken, GetEmployees = StringToBool(GetEmployees), CSEntries = csentries };
			repo.Put();

			return repo.ExportEntriesResults;
		}


		public GetImportEntriesResults GetImportEntries(GetImportEntriesRunStep importRunStep)
		{
			GetImportEntriesResults importReturnInfo;
			List<CSEntryChange> csentries = new List<CSEntryChange>();

			MIMRepo repo = new MIMRepo { LoginEndpoint = LoginEndpoint, ApiEndpoint = ApiEndpoint, ClientID = ClientId, ClientSecret = ClientSecret, Username=Username, Password=Password, SecToken = SecToken,  GetEmployees = StringToBool(GetEmployees) };
			repo.Get();

			var c = repo.CSEntries;

			importReturnInfo = new GetImportEntriesResults { MoreToImport = false, CSEntries = c };
			return importReturnInfo;
		}

		public Schema GetSchema(KeyedCollection<string, ConfigParameter> configParameters)
		{
			this.LoginEndpoint = configParameters["LoginEndpoint"].Value;
			this.ApiEndpoint = configParameters["ApiEndpoint"].Value;
			this.ClientId = configParameters["ClientId"].Value;
			this.ClientSecret = configParameters["ClientSecret"].SecureValue;
			this.Username = configParameters["Username"].Value;
			this.Password = configParameters["Password"].SecureValue;
			this.SecToken = configParameters["SecurityToken"].SecureValue;
			this.GetEmployees = configParameters["GetEmployees"].Value;

			MIMRepo repo = new MIMRepo { LoginEndpoint = LoginEndpoint, ApiEndpoint = ApiEndpoint, ClientID = ClientId, ClientSecret = ClientSecret, Username = Username, Password = Password, SecToken = SecToken, GetEmployees = StringToBool(GetEmployees) };

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
					configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("LoginEndpoint", "", "https://test.salesforce.com/services/oauth2/token"));
					configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("ApiEndpoint", "", "/services/data/v49.0"));
					configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("ClientId", ""));
					configParametersDefinitions.Add(ConfigParameterDefinition.CreateEncryptedStringParameter("ClientSecret", ""));
					configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter("Username", ""));
					configParametersDefinitions.Add(ConfigParameterDefinition.CreateEncryptedStringParameter("Password", ""));
					configParametersDefinitions.Add(ConfigParameterDefinition.CreateEncryptedStringParameter("SecurityToken", ""));
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
