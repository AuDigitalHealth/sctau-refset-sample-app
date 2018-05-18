package au.gov.digitalhealth;

import static org.junit.Assert.assertFalse;
import static org.junit.Assert.assertNotNull;
import static org.junit.Assert.assertTrue;

import java.util.Collection;

import org.junit.Test;

import au.gov.digitalhealth.db.ConceptFinder;
import au.gov.digitalhealth.model.Concept;

/**
 * Test the function to list all concepts in a reference set.
 */
public class ListRefsetMembersTest {

    @Test
    public void testValidRefsetId() {
        Collection<Concept> members = ConceptFinder.findRefsetMembers(32570331000036102L);
        assertNotNull("Expecting an object reference, not null", members);
        assertFalse("Refset should contain members", members.size() == 0);
    }

    @Test
    public void testInvalidRefsetId() {
        Collection<Concept> members = ConceptFinder.findRefsetMembers(32570031000036104L);
        assertNotNull("Expected an empty collection, not null reference", members);
        assertTrue("No members should be returned for invalid refset", members.size() == 0);
    }
    
    
}
