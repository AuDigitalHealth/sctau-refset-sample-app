using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace CTDemo
{
    /// <summary>
    ///  A data object modelling the "Concept" component in SNOMED CT. 
    ///  
    ///  It is not comprehensive and contains just the elements utilised by this sample application. *
    ///  
    ///  It contains the descriptions for the concept and, for each description, it's acceptability as defined in the 
    ///  Australian Dialect Reference Set (ADRS). It also contains each reference set membership this concept participates in.
    /// </summary>
    public class Concept
    {
        /** The SCT ID of this concept */
        public long sctId { get; set; }

        /** Maps a description (concept term) to an en-AU acceptability */
        private Dictionary<string, Metadata.LanguageAcceptability> descriptions = new Dictionary<string, Metadata.LanguageAcceptability>();

        /** Stores all the refset memberships that this concepts participates in */
        private List<RefsetMember> refsetMemberships = new List<RefsetMember>();

        /// <summary>
        /// Add a new description term for this concept.
        /// This is not persisted in the database.
        ///<param name="term">The description text</param>
        ///<param name="acceptabilityConceptId">
        ///       The SCTID of a language acceptability concept.
        ///       This should be the attribute value assigned to each member of the 
        ///       Australian Dialect Reference Set (ADRS). 
        ///       Ref to {@link LanguageAcceptability} for values.
        /// </param>
        ///</summary>
        public void AddDescription(String term, long acceptabilityConceptId)
        {
            descriptions[term] = Metadata.GetLanguageAcceptability(acceptabilityConceptId);
        }

        /// <summary>
        /// Add a new refset membership for this concept.
        /// This is not persisted to the database.
        ///<param name="refsetSctId">The SCTID of the reference set this concept belongs to</param>
        /// <throws>RuntimeException if the parameters are not valid (existing) concept ids (SCTIDs).</throws>
        /// </summary>
        public void AddRefsetMembership(long refsetSctId)
        {
            Concept refsetConcept = ConceptFinder.FindById(refsetSctId);

            if (refsetConcept == null)
            {
                throw new Exception(
                    "Invalid refset membership. Unknown or inactive refset concept: " + refsetSctId);
            }

            RefsetMember member = new RefsetMember();
            member.SetReferencedConcept(this);
            member.SetRefsetConcept(refsetConcept);
            refsetMemberships.Add(member);
        }

        /// <summary>
        /// <returns>A unmodifiable collection containing each refset membership this concept participates in</returns> 
        /// </summary>
        public IList<RefsetMember> GetRefsetMemberships()
        {
            return this.refsetMemberships.AsReadOnly();
        }

        /// <summary>
        /// <returns>The most preferable description for this concept. This is defined by the Australia Dialect Reference Set (ADRS).</returns> 
        /// </summary>
        public string GetPreferredTerm() {

            foreach (KeyValuePair<string, Metadata.LanguageAcceptability> kvp in descriptions)
            {
                Metadata.LanguageAcceptability acceptability = descriptions[kvp.Key];
                if (acceptability.Equals(Metadata.LanguageAcceptability.PREFERRED))
                {
                    return kvp.Key;
                }
            }
            // If term with preferred acceptability not found then just return the first description
            // which by the ordering in the db query should give us the latest term
            // (alternatively we could have continued looking for an ACCEPTABLE term)
            return descriptions.First().Key;
        }

        /// <summary>
        /// <ul>Creates a formatted string containing all the details of the concept, namely:</ul>
        /// <li>The SCT ID</li>
        /// <li>Each of the descriptions (should only be active)</li>
        /// <li>For each description, identifies the ADRS Preferred and Acceptable terms</li>
        /// <li>Each refset membership this concept participates in </li>
        /// </summary>
        public override string ToString() {
            var result = new StringBuilder();
            result.Append("SCT ID ").Append(sctId).Append("\n");
            
            foreach (KeyValuePair<string, Metadata.LanguageAcceptability> kvp in descriptions) {
                result.Append("\t").Append(kvp.Key);
                Metadata.LanguageAcceptability acceptability = descriptions[kvp.Key];
                if (!acceptability.Equals(Metadata.LanguageAcceptability.NONE)) {
                    result.Append(" [EN-AU ").Append(acceptability).Append(" TERM]");
                }
                result.Append("\n");
            }

            foreach (RefsetMember membership in refsetMemberships)
            {
                result.Append("\t\t Is member of refset '").Append(membership.GetRefsetConcept().GetPreferredTerm());
                result.Append("'\n");
            }
            result.Append("\n");
            return result.ToString();
        }    

    }
}
