// <summary>
// Defines SCT and SCT-AU Metadata (constants) utilised by the application. 
// </summary>
namespace CTDemo
{
    /// <summary>
    /// Defines the possible attribute values (each being a concept SCTID) assignable to a member in the 
    /// Australia Dialect Reference Set (ADRS).  
    /// </summary>
    
    public class Metadata
    {
        /** 
         * SNOMED CT RF2 'active' value
         */
        public const string ACTIVE_STATUS_VALUE = "1";

        public enum LanguageAcceptability : long
        {
            NONE = 0,
            PREFERRED = 900000000000548007L,
            ACCEPTABLE = 900000000000549004L
        }

        public static LanguageAcceptability GetLanguageAcceptability(long sctId){
            switch (sctId)
            {
                case (long)LanguageAcceptability.ACCEPTABLE:
                    return LanguageAcceptability.ACCEPTABLE;
                case (long)LanguageAcceptability.NONE:
                    return LanguageAcceptability.NONE;
                case (long)LanguageAcceptability.PREFERRED:
                    return LanguageAcceptability.PREFERRED;
                default:
                    return LanguageAcceptability.NONE;
            }
        }
    };

 }

