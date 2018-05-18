package au.gov.digitalhealth.model;

import java.util.ArrayList;
import java.util.Collection;
import java.util.Collections;
import java.util.HashMap;
import java.util.Map;

import au.gov.digitalhealth.db.ConceptFinder;
import au.gov.digitalhealth.model.Metadata.LanguageAcceptability;
import au.gov.digitalhealth.model.Concept;
import au.gov.digitalhealth.model.Metadata;
import au.gov.digitalhealth.model.RefsetMember;

/**
 * A data object modelling the "Concept" component in SNOMED CT. 
 *
 * It is not comprehensive and contains just the elements utilised by this sample application.*
 *
 * It contains the descriptions for the concept and, for each description, it's acceptability as defined in the 
 * Australian Dialect Reference Set (ADRS). It also contains each reference set membership this concept participates in.
 * 
 */
public class Concept {
    
    /** The SCT ID of this concept */
    long sctId;
    
    /** Maps a description (concept term) to an en-AU acceptability */
    private Map<String, LanguageAcceptability> descriptions = new HashMap<String, Metadata.LanguageAcceptability>();

    /** Stores all the refset memberships that this concepts participates in */
    private Collection<RefsetMember> refsetMemberships = new ArrayList<RefsetMember>();

    
    public Concept() {}

    
    public long getSctId() {
        return sctId;
    }

    public void setSctId(long sctId) {
        this.sctId = sctId;
    }

    /**
     * Add a new description term for this concept.
     * This is not persisted in the database.
     * 
     * @param term The description text
     * @param acceptabilityConceptId 
     *          The SCTID of a language acceptability concept.
     *          This should be the attribute value assigned to each member of the 
     *          Australian Dialect Reference Set (ADRS). 
     *          Ref to {@link LanguageAcceptability} for values.
     */
    public void addDescription(String term, long acceptabilityConceptId) {
        descriptions.put(term, LanguageAcceptability.forSctId(acceptabilityConceptId));
    }
    
    /**
     * Add a new refset membership for this concept.
     * This is not persisted to the database.
     * 
     * @param refsetSctId The SCTID of the reference set this concept belongs to
     * @throws RuntimeException if the parameters are not valid (existing) concept ids (SCTIDs).
     */
    public void addRefsetMembership(long refsetSctId) {
        
        Concept refsetConcept = ConceptFinder.findById(refsetSctId);
        
        if (refsetConcept == null) {
            throw new RuntimeException(
                "Invalid refset membership. Unknown or inactive refset concept: " + refsetSctId);
        }
        
        RefsetMember member = new RefsetMember();
        member.setReferencedConcept(this);
        member.setRefsetConcept(refsetConcept);
        
        refsetMemberships.add(member);
    }
    
    /**
     * @return A unmodifiable collection containing each refset membership this concept participates in
     */
    public Collection<RefsetMember> getRefsetMemberships() {
        return Collections.unmodifiableCollection(this.refsetMemberships);
    }
    
    /**
     * @return The most preferable description for this concept. This is defined by the Australia Dialect Reference Set (ADRS).
     */
    public String getPreferredTerm() {
        for (String description : descriptions.keySet()) {
            LanguageAcceptability acceptability = descriptions.get(description);
            if (LanguageAcceptability.PREFERRED.equals(acceptability)) {
                return description;
            }
        }
        // If term with preferred acceptability not found then just return the first description
        // which by the ordering in the db query should give us the latest term
        // (alternatively we could have continued looking for an ACCEPTABLE term)
        return descriptions.keySet().iterator().next();
    }
    
    /**
     * Creates a formatted string containing all the details of the concept, namely:<ul>
     * <li>The SCT ID
     * <li>Each of the descriptions (should only be active)
     * <li>For each description, identifies the ADRS Preferred and Acceptable terms
     * <li>Each refset membership this concept participates in
     * <p/>
     */
    @Override
    public String toString() {
        StringBuffer result = new StringBuffer();
        result.append("SCT ID ").append(sctId).append("\n");
        
        for (String description : descriptions.keySet()) {
            result.append("\t").append(description);
            LanguageAcceptability acceptability = descriptions.get(description);
            if (!LanguageAcceptability.NONE.equals(acceptability)) {
                result.append(" [EN-AU ").append(acceptability).append(" TERM]");
            }
            result.append("\n");
        }
        
        for (RefsetMember membership : refsetMemberships) {
            result.append("\t\t Is member of refset '").append(membership.getRefsetConcept().getPreferredTerm());
            result.append("'\n");
        }
        
        result.append("\n");
        return result.toString();
    }    
    
}
