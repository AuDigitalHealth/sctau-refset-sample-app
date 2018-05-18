package au.gov.digitalhealth.model;

/**
 * Defines SCT and SCT-AU Metadata (constants) utilised by the application.  
 */
public class Metadata {

    /**
     * Defines the possible attribute values (each being a concept SCTID) assignable to a member in the 
     * Australia Dialect Reference Set (ADRS).  
     */
    public enum LanguageAcceptability {
        
        NONE(0), 
        PREFERRED(900000000000548007L), 
        ACCEPTABLE(900000000000549004L);
        
        private long sctId;
        
        private LanguageAcceptability(long sctId) {
            this.sctId = sctId;
        }
        
        public long getSctId() {
            return this.sctId;
        }
        
        public static LanguageAcceptability forSctId(Long sctId) {
            if (sctId == null) {
                return NONE;
            }
            for (LanguageAcceptability value : values()) {
                if (value.getSctId() == sctId) {
                    return value;
                }
            }
            return NONE;
        }
    };
    
    /** 
     * SNOMED CT RF2 'active' value
     */
    public final static String ACTIVE_STATUS_VALUE = "1";
}
