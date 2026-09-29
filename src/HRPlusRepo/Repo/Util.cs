using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.MetadirectoryServices;

namespace HRPlusRepo.Repo
{
    public static class Util
    {
        #region Helpers


        public static AttributeType GetType(string propname)
        {
            AttributeType typ = AttributeType.String;
            switch (propname)
            {
                case "String":
                    {
                        typ = AttributeType.String;
                        break;
                    }
                case "Boolean":
                    {
                        typ = AttributeType.Boolean;
                        break;
                    }
                case "DateTime":
                    {
                        typ = AttributeType.String;
                        break;
                    }
                default:
                    break;
            }
            return typ;
        }

        public static MAExportError ErrValidate(string statusCode)
        {
            MAExportError er = new MAExportError();

            switch (statusCode)
            {
                case "OK":
                    {
                        er = MAExportError.Success;
                        break;
                    }
                case "Accepted":
                    {
                        er = MAExportError.Success;
                        break;
                    }
                case "BadRequest":
                    {
                        er = MAExportError.ExportErrorCustomStopRun;
                        break;
                    }
                case "NotFound":
                    {
                        er = MAExportError.ExportErrorInvalidDN;
                        break;
                    }
                case "NotAcceptable":
                    {
                        er = MAExportError.ExportErrorInvalidAttributeValue;
                        break;
                    }
                case "NotModified":
                    {
                        er = MAExportError.ExportErrorAmbiguousUpdate;
                        break;
                    }
                default:
                    break;
            }

            return er;
        }

        #endregion
    }
}
