namespace CTDemo
{

    /// <summary>
    ///  A simple Concept Attribute Value reference set member. 
    ///  This class only captures the following elements of the membership: 
    ///  <li>The reference set</li>
    ///  <li>The referenced concept - the concept belong to the reference set</li>
    ///  <li>The attribute value - a concept value attached to the membership </li>
    /// </summary>
    public class RefsetMember
    {
        private Concept refsetConcept;
        private Concept referencedConcept;


        public Concept GetRefsetConcept()
        {
            return refsetConcept;
        }

        public void SetRefsetConcept(Concept refsetConcept)
        {
            this.refsetConcept = refsetConcept;
        }

        public Concept GetReferencedConcept()
        {
            return referencedConcept;
        }

        public void SetReferencedConcept(Concept referencedConcept)
        {
            this.referencedConcept = referencedConcept;
        }
    }
}
