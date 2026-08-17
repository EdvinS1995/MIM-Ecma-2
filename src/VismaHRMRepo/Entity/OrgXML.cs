using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VismaHRMRepo.Entity
{



    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class charts
    {

        private chartsChart chartField;

        /// <remarks/>
        public chartsChart chart
        {
            get
            {
                return this.chartField;
            }
            set
            {
                this.chartField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class chartsChart
    {

        private chartsChartUnit[] unitsField;

        private byte idField;

        private string nameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("unit", IsNullable = false)]
        public chartsChartUnit[] units
        {
            get
            {
                return this.unitsField;
            }
            set
            {
                this.unitsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class chartsChartUnit
    {

        private chartsChartUnitContactInfo contactInfoField;

        private chartsChartUnitManager managerField;

        private chartsChartUnitTask[] tasksField;

        private chartsChartUnitRole[] rolesField;

        private ushort kodeField;

        private string nameField;

        private byte companyField;

        private string idField;

        private ushort parentidField;

        private string unitypeField;

        /// <remarks/>
        public chartsChartUnitContactInfo contactInfo
        {
            get
            {
                return this.contactInfoField;
            }
            set
            {
                this.contactInfoField = value;
            }
        }

        /// <remarks/>
        public chartsChartUnitManager manager
        {
            get
            {
                return this.managerField;
            }
            set
            {
                this.managerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("task", IsNullable = false)]
        public chartsChartUnitTask[] tasks
        {
            get
            {
                return this.tasksField;
            }
            set
            {
                this.tasksField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("role", IsNullable = false)]
        public chartsChartUnitRole[] roles
        {
            get
            {
                return this.rolesField;
            }
            set
            {
                this.rolesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort kode
        {
            get
            {
                return this.kodeField;
            }
            set
            {
                this.kodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte company
        {
            get
            {
                return this.companyField;
            }
            set
            {
                this.companyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort parentid
        {
            get
            {
                return this.parentidField;
            }
            set
            {
                this.parentidField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string unitype
        {
            get
            {
                return this.unitypeField;
            }
            set
            {
                this.unitypeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class chartsChartUnitContactInfo
    {

        private object phoneField;

        private object faxField;

        private object postaladdress1Field;

        private object postaladdress2Field;

        private object postalcodeField;

        private object postalareaField;

        private object visitaddress1Field;

        private object visitaddress2Field;

        private object visitpostalcodeField;

        private object visitpostalareaField;

        private object emailField;

        /// <remarks/>
        public object phone
        {
            get
            {
                return this.phoneField;
            }
            set
            {
                this.phoneField = value;
            }
        }

        /// <remarks/>
        public object fax
        {
            get
            {
                return this.faxField;
            }
            set
            {
                this.faxField = value;
            }
        }

        /// <remarks/>
        public object postaladdress1
        {
            get
            {
                return this.postaladdress1Field;
            }
            set
            {
                this.postaladdress1Field = value;
            }
        }

        /// <remarks/>
        public object postaladdress2
        {
            get
            {
                return this.postaladdress2Field;
            }
            set
            {
                this.postaladdress2Field = value;
            }
        }

        /// <remarks/>
        public object postalcode
        {
            get
            {
                return this.postalcodeField;
            }
            set
            {
                this.postalcodeField = value;
            }
        }

        /// <remarks/>
        public object postalarea
        {
            get
            {
                return this.postalareaField;
            }
            set
            {
                this.postalareaField = value;
            }
        }

        /// <remarks/>
        public object visitaddress1
        {
            get
            {
                return this.visitaddress1Field;
            }
            set
            {
                this.visitaddress1Field = value;
            }
        }

        /// <remarks/>
        public object visitaddress2
        {
            get
            {
                return this.visitaddress2Field;
            }
            set
            {
                this.visitaddress2Field = value;
            }
        }

        /// <remarks/>
        public object visitpostalcode
        {
            get
            {
                return this.visitpostalcodeField;
            }
            set
            {
                this.visitpostalcodeField = value;
            }
        }

        /// <remarks/>
        public object visitpostalarea
        {
            get
            {
                return this.visitpostalareaField;
            }
            set
            {
                this.visitpostalareaField = value;
            }
        }

        /// <remarks/>
        public object email
        {
            get
            {
                return this.emailField;
            }
            set
            {
                this.emailField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class chartsChartUnitManager
    {

        private string proprietorTypeField;

        private ushort idField;

        private string nameField;

        /// <remarks/>
        public string proprietorType
        {
            get
            {
                return this.proprietorTypeField;
            }
            set
            {
                this.proprietorTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class chartsChartUnitTask
    {

        private chartsChartUnitTaskEmployee[] employeesField;

        private byte idField;

        private string descriptionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("employee", IsNullable = false)]
        public chartsChartUnitTaskEmployee[] employees
        {
            get
            {
                return this.employeesField;
            }
            set
            {
                this.employeesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class chartsChartUnitTaskEmployee
    {

        private string proprietorTypeField;

        private ushort idField;

        private string nameField;

        /// <remarks/>
        public string proprietorType
        {
            get
            {
                return this.proprietorTypeField;
            }
            set
            {
                this.proprietorTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class chartsChartUnitRole
    {

        private chartsChartUnitRoleEmployee[] employeesField;

        private byte idField;

        private string descriptionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("employee", IsNullable = false)]
        public chartsChartUnitRoleEmployee[] employees
        {
            get
            {
                return this.employeesField;
            }
            set
            {
                this.employeesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class chartsChartUnitRoleEmployee
    {

        private string proprietorTypeField;

        private ushort idField;

        private string nameField;

        /// <remarks/>
        public string proprietorType
        {
            get
            {
                return this.proprietorTypeField;
            }
            set
            {
                this.proprietorTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }


}
