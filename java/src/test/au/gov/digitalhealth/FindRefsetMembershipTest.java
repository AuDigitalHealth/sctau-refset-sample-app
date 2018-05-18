package au.gov.digitalhealth;

import static org.junit.Assert.assertFalse;
import static org.junit.Assert.assertNotNull;
import static org.junit.Assert.fail;

import java.util.Collection;
import java.util.HashSet;

import org.junit.Test;

import au.gov.digitalhealth.db.ConceptFinder;
import au.gov.digitalhealth.model.Concept;
import au.gov.digitalhealth.model.RefsetMember;

/**
 * Check the reference set memberships obtained for a concept.
 */
public class FindRefsetMembershipTest {

    /** Concept: 'Fifth metatarsal structure' */
    private static final long KNOWN_ACTIVE_CONCEPT_ID = 301000L;

    @Test
    public void testRefsetMembership() {
        Concept concept = ConceptFinder.findById(KNOWN_ACTIVE_CONCEPT_ID);
        assertNotNull("Expecting an object reference, not null",concept);
        
        Collection<RefsetMember> memberships = concept.getRefsetMemberships();
        assertNotNull("Expecting an object reference, not null", memberships);
        assertFalse("Refset should contain members", memberships.size() == 0);
        
        HashSet<String> memberKeys = new HashSet<String>();
        for (RefsetMember member : memberships) {
            
            assertNotNull("Refset membership missing refset concept", member.getRefsetConcept());
            assertNotNull("Refset membership missing referenced concept", member.getReferencedConcept());
            
            String key = 
                String.valueOf(member.getRefsetConcept().getSctId()) + 
                String.valueOf(member.getReferencedConcept().getSctId());          
            if (memberKeys.contains(key)) {
                fail("Duplicate membership encounter");
            }
            memberKeys.add(key);
        }
        
    }
    
}
