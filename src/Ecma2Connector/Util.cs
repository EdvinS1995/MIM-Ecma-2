using Microsoft.MetadirectoryServices;

namespace Ecma2Connector
{
    public static class Util
    {
        /// <summary>
        /// Maps an HTTP-style status word returned by the connected system into the
        /// MAExportError the Sync Service expects on a CSEntryChangeResult.
        /// </summary>
        public static MAExportError ErrValidate(string status)
        {
            switch (status)
            {
                case "OK":
                case "Success":
                case "Created":
                case "Accepted":
                case "NoContent":
                    return MAExportError.Success;

                case "NotFound":
                case "Gone":
                    return MAExportError.ExportErrorInvalidDN;

                case "Unauthorized":
                case "Forbidden":
                    return MAExportError.ExportErrorConnectionFailure;

                case "BadRequest":
                case "UnprocessableEntity":
                default:
                    return MAExportError.ExportErrorCustomContinueRun;
            }
        }
    }
}
