package au.gov.digitalhealth.model;

import au.gov.digitalhealth.model.Concept;

/**
 * A simple Concept Attribute Value reference set member. 
 *
 * This class only captures the following elements of the membership:<ul>
 * <li>The reference set
 * <li>The referenced concept - the concept belong to the reference set
 * <li>The attribute value - a concept value attached to the membership 
 */
public class RefsetMember {

    private Concept refsetConcept;
    
    private Concept referencedConcept;
    
    public Concept getRefsetConcept() {
        return refsetConcept;
    }

    public void setRefsetConcept(Concept refsetConcept) {
        this.refsetConcept = refsetConcept;
    }

    public Concept getReferencedConcept() {
        return referencedConcept;
    }

    public void setReferencedConcept(Concept referencedConcept) {
        this.referencedConcept = referencedConcept;
    }
   
}
