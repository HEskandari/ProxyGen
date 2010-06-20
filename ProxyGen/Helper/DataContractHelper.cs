using System;
using System.Collections.Generic;
using System.ServiceModel.Description;
using System.Linq;

namespace ProxyGen.Helper
{
    public static class DataContractHelper
    {
        public static bool HasWarnings(this ICollection<MetadataConversionError> errors)
        {
            return errors != null &&
                   errors.Count > 0 &&
                   errors.Any(x => x.IsWarning);
        }

        public static bool HasErrors(this ICollection<MetadataConversionError> errors)
        {
            return errors != null &&
                   errors.Count > 0 &&
                   errors.Any(x => x.IsWarning == false);
        }

        public static string GetWarnings(this ICollection<MetadataConversionError> errors)
        {
            return errors.Where(e => e.IsWarning).Select(e => e.Message).FirstOrDefault();
        }
    }
}