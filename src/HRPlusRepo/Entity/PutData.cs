using System;
using System.Diagnostics.SymbolStore;


namespace HRPlusRepo.Entity
{

    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "Bluegarden.HRPlus.Schemas")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "Bluegarden.HRPlus.Schemas", IsNullable = false)]
    public partial class UpdateEmployment
    {

        private UpdateEmploymentHeader headerField;

        private UpdateEmploymentIdentificationCodes identificationCodesField;

        private UpdateEmploymentChanges changesField;

        /// <remarks/>
        public UpdateEmploymentHeader Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        /// <remarks/>
        public UpdateEmploymentIdentificationCodes IdentificationCodes
        {
            get
            {
                return this.identificationCodesField;
            }
            set
            {
                this.identificationCodesField = value;
            }
        }

        /// <remarks/>
        public UpdateEmploymentChanges Changes
        {
            get
            {
                return this.changesField;
            }
            set
            {
                this.changesField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "Bluegarden.HRPlus.Schemas")]
    public partial class UpdateEmploymentHeader
    {

        private string sourceSystemField;

        private System.DateTime sourceSystemTimeStampField;

        private string messageIdField;

        private string correlationIdField;

        private string businessActionField;

        private byte schemaVersionField;

        /// <remarks/>
        public string SourceSystem
        {
            get
            {
                return this.sourceSystemField;
            }
            set
            {
                this.sourceSystemField = value;
            }
        }

        /// <remarks/>
        public System.DateTime SourceSystemTimeStamp
        {
            get
            {
                return this.sourceSystemTimeStampField;
            }
            set
            {
                this.sourceSystemTimeStampField = value;
            }
        }

        /// <remarks/>
        public string MessageId
        {
            get
            {
                return this.messageIdField;
            }
            set
            {
                this.messageIdField = value;
            }
        }

        /// <remarks/>
        public string CorrelationId
        {
            get
            {
                return this.correlationIdField;
            }
            set
            {
                this.correlationIdField = value;
            }
        }

        /// <remarks/>
        public string BusinessAction
        {
            get
            {
                return this.businessActionField;
            }
            set
            {
                this.businessActionField = value;
            }
        }

        /// <remarks/>
        public byte SchemaVersion
        {
            get
            {
                return this.schemaVersionField;
            }
            set
            {
                this.schemaVersionField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "Bluegarden.HRPlus.Schemas")]
    public partial class UpdateEmploymentIdentificationCodes
    {

        private string identificationTypeField;

        private int employerCodeField;

        private int workNumberField;

        private string employmentNumberField;

        private int positionCodeField;

        /// <remarks/>
        public string IdentificationType
        {
            get
            {
                return this.identificationTypeField;
            }
            set
            {
                this.identificationTypeField = value;
            }
        }

        /// <remarks/>
        public int EmployerCode
        {
            get
            {
                return this.employerCodeField;
            }
            set
            {
                this.employerCodeField = value;
            }
        }

        /// <remarks/>
        public int WorkNumber
        {
            get
            {
                return this.workNumberField;
            }
            set
            {
                this.workNumberField = value;
            }
        }

        /// <remarks/>
        public string EmploymentNumber
        {
            get
            {
                return this.employmentNumberField;
            }
            set
            {
                this.employmentNumberField = value;
            }
        }

        /// <remarks/>
        public int PositionCode
        {
            get
            {
                return this.positionCodeField;
            }
            set
            {
                this.positionCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "Bluegarden.HRPlus.Schemas")]
    public partial class UpdateEmploymentChanges
    {

        private UpdateEmploymentChangesChange changeField;

        /// <remarks/>
        public UpdateEmploymentChangesChange Change
        {
            get
            {
                return this.changeField;
            }
            set
            {
                this.changeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "Bluegarden.HRPlus.Schemas")]
    public partial class UpdateEmploymentChangesChange
    {

        private string emailWorkField;

        private System.DateTime affectingDateField;

        /// <remarks/>
        public string EmailWork
        {
            get
            {
                return this.emailWorkField;
            }
            set
            {
                this.emailWorkField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
        public System.DateTime AffectingDate
        {
            get
            {
                return this.affectingDateField;
            }
            set
            {
                this.affectingDateField = value;
            }
        }
    }




}
