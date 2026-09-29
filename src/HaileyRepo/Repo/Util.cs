using Microsoft.MetadirectoryServices;

namespace HaileyRepo.Repo
{
    public static class Util
    {
        /// <summary>
        /// Maps a source-field type hint to a MIM AttributeType. HaileyHR's API is
        /// string-heavy (dates included) so almost everything defaults to String, matching
        /// this codebase's convention of avoiding MIM date-parsing edge cases.
        /// </summary>
        public static AttributeType GetType(string sourceTypeName)
        {
            switch (sourceTypeName)
            {
                case "Boolean":
                    return AttributeType.Boolean;
                case "Int32":
                case "Int64":
                    return AttributeType.Integer;
                default:
                    return AttributeType.String;
            }
        }

        /// <summary>
        /// Maps an HTTP status code (as returned by HttpStatusCode.ToString()) to MAExportError.
        /// HaileyHR doesn't document a bulk/per-item error vocabulary for these endpoints —
        /// every write here is a single-resource POST/PUT, so plain HTTP status is authoritative.
        /// </summary>
        public static MAExportError ErrValidate(string statusCode)
        {
            switch (statusCode)
            {
                case "OK":
                case "Created":
                case "Accepted":
                case "NoContent":
                    return MAExportError.Success;
                case "NotFound":
                    return MAExportError.ExportErrorInvalidDN;
                case "BadRequest":
                case "UnprocessableEntity":
                    return MAExportError.ExportErrorInvalidAttributeValue;
                case "Conflict":
                    return MAExportError.ExportErrorAmbiguousUpdate;
                case "Unauthorized":
                case "Forbidden":
                    return MAExportError.ExportErrorCustomStopRun;
                case "TooManyRequests":
                    return MAExportError.ExportErrorCustomContinueRun;
                default:
                    return MAExportError.ExportErrorCustomStopRun;
            }
        }
    }
}
