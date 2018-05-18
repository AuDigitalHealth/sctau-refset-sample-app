using System;
using System.Collections.Generic;
using System.Data;

namespace CTDemo
{
    /// <summary>
    /// Find concepts using the dataSource for given search parameters.
    /// <summary>
    public static class ConceptFinder
    {
        ///<summary>
        /// Finds the matching active concept for the <code>conceptSctid</code>.
        ///<param name="conceptSctid">long conceptSctid</param>
        ///<returns>Concept matching the <code>conceptSctid</code>. <code>null</code> is returned if no concept found</returns> 
        ///<throws> RuntimeException SQLException database errors.</throws>
        ///<summary>
        public static Concept FindById(long conceptSctid)
        {
            Concept concept = null;
            DataTable resultSet = DataSource.RunSQLQuery("select concept.id"
                    + " from concepts concept"
                    + " where concept.id = " + conceptSctid
                    + " and concept.active = " + Metadata.ACTIVE_STATUS_VALUE,
                    true);

            try
            {
                foreach (DataRow dr in resultSet.Rows)
                    concept = GetConceptDetails((long)dr["id"]);
                
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return concept;
        }

        ///<summary>
        /// Finds the matching active concepts with an active description/s that match the partial <code>term</code>.
        /// Specifies rows are Limitied
        /// 
        ///<param name="term">String Full or partial concept term</param>
        ///<returns>List of Concepts</returns> 
        ///<throws> RuntimeException SQLException database errors.</throws>
        ///<summary>
        public static List<Concept> FindByTerm(String term)
        {
            List<Concept> concepts = new List<Concept>();

            DataTable resultSet = DataSource.RunSQLQuery("select distinct concept.id"
                + " from concepts concept"
                + " join descriptions description on description.conceptid = concept.id"
                + " where description.term like '%" + term + "%'"
                + " and description.active = " + Metadata.ACTIVE_STATUS_VALUE
                + " and concept.active = " + Metadata.ACTIVE_STATUS_VALUE
                + " order by concept.id DESC",
                true);

            try
            {
                foreach (DataRow dr in resultSet.Rows)
                    concepts.Add(GetConceptDetails((long)dr["id"]));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return concepts;
        }

        ///<summary>
        /// Finds all the active members for a reference set <code>refsetSctId</code>
        ///<param name="refsetSctId">long reference set id</param>
        ///<returns>List of Concepts</returns> 
        ///<throws>RuntimeException SQLException database errors</throws>
        ///<summary>
        public static List<Concept> FindRefsetMembers(long refsetSctId)
        {
            List<Concept> concepts = new List<Concept>();

            DataTable resultSet = DataSource.RunSQLQuery(
                      "select concept.id"
                    + " from concepts concept"
                    + " join concept_refset clinical on clinical.referencedconceptid = concept.id"
                    + " where clinical.refsetid = " + refsetSctId
                    + " order by concept.id",true);

            try
            {
                foreach (DataRow dr in resultSet.Rows)
                    concepts.Add(GetConceptDetails((long)dr["id"]));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return concepts;
        }

        ///<summary>
        /// Finds all the reference sets the concepts has an active membership with.
        ///<param name="conceptSctid">long conceptSctid</param>
        ///<returns>A Set of <long>refsetid</long></returns> 
        ///<throws>RuntimeException SQLException database errors</throws>
        ///</summary>
        private static List<long> FindConceptRefsets(long conceptSctid)
        {
            var refsetIds = new List<long>();
            DataTable resultSet = DataSource.RunSQLQuery("select clinical.refsetid"
                  + " from concepts concept"
                  + " join concept_refset clinical on  clinical.referencedconceptid = concept.id"
                  + " where concept.id = " + conceptSctid
                  + " and concept.active = " + Metadata.ACTIVE_STATUS_VALUE
                  + " order by concept.id DESC");

            try
            {
                foreach (DataRow dr in resultSet.Rows)
                {
                    if (!refsetIds.Contains((long)dr["refsetid"]))
                        refsetIds.Add((long)dr["refsetid"]);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return refsetIds;
        }


      ///<summary>
      /// Finds all the active descriptions for the <code>conceptSctid</code>.
      ///<param name="conceptSctid">long conceptSctid </param>
      ///<returns>Dictionary of ADRS language type <code>Long</code> keyed by the description term <code>String</code>
      ///<throws>RuntimeException SQLException database errors</throws>
      ///</returns>
      ///</summary>
      private static Dictionary<string, long> FindConceptDescriptions(long conceptSctid)
      {
          Dictionary<string, long> refsetIds = new Dictionary<string, long>();

          DataTable resultSet = DataSource.RunSQLQuery("select description.term, adrs.valueid"
                  + " from concepts concept"
                  + " join descriptions description on description.conceptid = concept.id"
                  + " left join description_refset adrs on  adrs.referenceddescriptionid = description.id"
                  + " where concept.id = " + conceptSctid
                  + " and description.active = " + Metadata.ACTIVE_STATUS_VALUE
                  + " and concept.active = " + Metadata.ACTIVE_STATUS_VALUE
                  + " order by description.term, description.effectivetime DESC");

          try
          {
              foreach (DataRow dr in resultSet.Rows)
              {
                  // Set valueid to 0 if DBNull
                  long valueid = (dr["valueid"] != System.DBNull.Value ? long.Parse(dr["valueid"].ToString()) : 0);
                  if (!refsetIds.ContainsKey((string)dr["term"]))
                      refsetIds.Add((string)dr["term"], valueid);
              }
          }
          catch (Exception e)
          {
              throw new Exception(e.Message);
          }
          return refsetIds;
      }

      ///<summary>
      /// Creates a {@link Concept} object from the <code>conceptId</code> and finds the descriptions and reference sets
      ///<param name="conceptId">long conceptId</param>
      ///<returns>Concept</returns>returns>
      ///<throws>RuntimeException SQLException database errors</throws>
      ///</summary>
      private static Concept GetConceptDetails(long conceptId)
      {
          Dictionary<string, long> descriptionAdrsMap;
          List<long> refsetIds;

          Concept concept = new Concept();
          concept.sctId = conceptId;

          descriptionAdrsMap = FindConceptDescriptions(concept.sctId);
          foreach (KeyValuePair<string, long> kvp in descriptionAdrsMap)
              concept.AddDescription(kvp.Key, descriptionAdrsMap[kvp.Key]);

          refsetIds = FindConceptRefsets(concept.sctId);
          foreach (var refsetId in refsetIds)
            concept.AddRefsetMembership(refsetId);

          return concept;
      }
    }
}
