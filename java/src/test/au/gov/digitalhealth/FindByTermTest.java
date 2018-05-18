package au.gov.digitalhealth;

import static org.junit.Assert.assertFalse;
import static org.junit.Assert.assertNotNull;
import static org.junit.Assert.assertTrue;

import java.util.Collection;
import java.util.HashSet;

import org.junit.Test;

import au.gov.digitalhealth.db.ConceptFinder;
import au.gov.digitalhealth.model.Concept;

/**
 * Test searching for concepts by textual term (description) 
 */
public class FindByTermTest {

    /** Concept: Punch drunk */
    private static final long KNOWN_INACTIVE_CONCEPT_ID = 51996004L;
    
    /** Concept: Punch drunk */
    private static final long KNOWN_ACTIVE_CONCEPT_ID = 230283005L;

    /**
     * Search for the term "drunk". This has two concepts with the same description (a duplication) however
     * one is inactive - so the test is that the search result should contain the active concept but not the
     * inactive concept.
     */
    @Test
    public void testKnownTerm() {
        Collection<Concept> results = ConceptFinder.findByTerm("drunk");
        
        assertNotNull("Expected an empty collection, not null reference", results);
        assertFalse("Known term not found", results.size() == 0);
        assertTrue("Expected more than one concept found for known term", results.size() > 1);
        
        boolean foundActive = false;
        boolean foundInactive = false;
        
        for (Concept concept : results) {
            foundActive |= (concept.getSctId() == KNOWN_ACTIVE_CONCEPT_ID);
            foundInactive |= (concept.getSctId() == KNOWN_INACTIVE_CONCEPT_ID);            
        }
        
        assertTrue("Expected active concept was not present in results", foundActive);
        assertFalse("Inactive concept was present in the result, should only contain active concepts", foundInactive);
    }
    
    @Test
    public void testUnknownTerm() {
        Collection<Concept> results = ConceptFinder.findByTerm("wakawaka");
        
        assertNotNull("Expected an empty collection, not null reference", results);
        assertTrue("Expected no concepts for known invalid term", results.size() == 0);
    }
    
    @Test
    public void testNoDuplicates() {
        HashSet<Long> uniqueIds = new HashSet<Long>();
        Collection<Concept> results = ConceptFinder.findByTerm("heart");
        
        for (Concept c : results) {
            Long sctId = c.getSctId();
            assertFalse("Duplicate concept with SCTID " + sctId + " returned in result", uniqueIds.contains(sctId));
            uniqueIds.add(sctId);
        }
    }
    
    @Test
    public void testMaxRows() {
        int resultLimit = ConceptFinder.getResultLimit();
        Collection<Concept> results = ConceptFinder.findByTerm("heart");
        assertFalse("Number of results returned exceeds the maximum row count specified", results.size() > resultLimit);
    }
}
