package au.gov.digitalhealth;

import static org.junit.Assert.assertNotNull;
import static org.junit.Assert.assertNull;
import static org.junit.Assert.assertTrue;

import org.junit.Test;

import au.gov.digitalhealth.db.ConceptFinder;
import au.gov.digitalhealth.model.Concept;

/**
 * Test searching for a concept by SCTID 
 */
public class FindByIdTest {

    private static final long INVALID_CONCEPT_ID = 123L;
    
    /** Concept: 'Access instrument' */
    private static final long KNOWN_INACTIVE_CONCEPT_ID = 370127007L;
    
    /** Concept: 'Fifth metatarsal structure' */
    private static final long KNOWN_ACTIVE_CONCEPT_ID = 301000L;

    
    @Test
    public void testKnownId() {
        Concept concept = ConceptFinder.findById(KNOWN_ACTIVE_CONCEPT_ID);
        assertNotNull("Known concept not found", concept);
        
        assertTrue("Incorrect concept id", concept.getSctId() == KNOWN_ACTIVE_CONCEPT_ID);
        assertTrue("Incorrect en-AU preferred term", "Fifth metatarsal structure".equals(concept.getPreferredTerm()));
        
    }
    
    @Test
    public void testUnknownId() {
        Concept concept = ConceptFinder.findById(INVALID_CONCEPT_ID);
        assertNull("Unexpected concept found for invalid id", concept);
    }
    
    @Test
    public void testInactiveConcept() {
        Concept concept = ConceptFinder.findById(KNOWN_INACTIVE_CONCEPT_ID);
        assertNull("Inactive concept not expected", concept);
    }
    
}
