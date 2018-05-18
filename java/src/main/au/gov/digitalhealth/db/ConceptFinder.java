package au.gov.digitalhealth.db;

import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.ArrayList;
import java.util.Collection;
import java.util.HashMap;
import java.util.HashSet;
import java.util.Map;

import au.gov.digitalhealth.model.Concept;
import au.gov.digitalhealth.model.Metadata;
import au.gov.digitalhealth.db.DataSource;

/**
 * Find concepts using the {@link DataSource} for given search parameters.
 */
public class ConceptFinder {

    /**
     * Underlying data store to retrieve terminology from.
     */
    private static DataSource dataSource = new DataSource();

    /**
     * Finds the matching active concept for the <code>conceptSctid</code>.
     *
     * @param conceptSctid Long
     * @return Concept matching the <code>conceptSctid</code>. <code>null</code> is returned if no concept found
     * @throws RuntimeException SQLException database errors.
     */
    public static Concept findById(long conceptSctid) throws RuntimeException {
        Concept concept = null;

        ResultSet resultSet = runSql("select concept.id"
                + " from concepts concept"
                + " where concept.id = " + conceptSctid
                + " and concept.active = " + Metadata.ACTIVE_STATUS_VALUE, dataSource.getMaxRows());
        try {
            if (resultSet != null && resultSet.next()) {
                concept = getConceptDetails(resultSet.getLong(1));
            }
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }

        return concept;
    }

    /**
     * Finds the matching active concepts with an active description/s that match the partial <code>term</code>.
     *
     * A maximum number of rows is returned as configured in @see {@link DataSource.getMaxRows()}
     *
     * @param term String full or partial concept term
     * @return Collection of Concept
     * @throws RuntimeException SQLException database errors.
     */
    public static Collection<Concept> findByTerm(String term) throws RuntimeException {
        Collection<Concept> concepts = new ArrayList<Concept>();

        ResultSet resultSet = runSql("select distinct concept.id"
            + " from concepts concept"
            + " join descriptions description on description.conceptid = concept.id"
            + " where description.term like '%" + term + "%'"
            + " and description.active = " + Metadata.ACTIVE_STATUS_VALUE
            + " and concept.active = " + Metadata.ACTIVE_STATUS_VALUE
            + " order by concept.id, description.effectivetime DESC", dataSource.getMaxRows());
        try {
            while (resultSet != null && resultSet.next()) {
                concepts.add(getConceptDetails(resultSet.getLong(1)));
            }
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }

        return concepts;
    }

    /**
     * Finds all the active members for a reference set <code>refsetSctId</code>
     *
     * @param refsetSctId long reference set id
     * @return Collection of Concept
     * @throws RuntimeException SQLException database errors.
     */
    public static Collection<Concept> findRefsetMembers(long refsetSctId) throws RuntimeException {
        Collection<Concept> concepts = new ArrayList<Concept>();

        ResultSet resultSet = runSql("select concept.id"
                + " from concepts concept"
                + " join concept_refset clinical on clinical.referencedconceptid = concept.id"
                + " where clinical.refsetid = " + refsetSctId
                + " order by concept.id", getResultLimit());

        try {
            while (resultSet != null && resultSet.next()) {
                concepts.add(getConceptDetails(resultSet.getLong(1)));
            }
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }

        return concepts;
    }

    /**
     * finds all the reference sets the concepts has an active membership with.
     *
     * @param conceptSctid long
     * @return Map of reference set value <code>Long</code> keyed by reference set id <code>Long</code>
     * @throws RuntimeException SQLException database errors.
     */
    private static HashSet<Long> findConceptRefsets(long conceptSctid) throws RuntimeException {
        HashSet<Long> refsetIds = new HashSet<Long>();
        ResultSet resultSet = runSql("select clinical.refsetid"
                + " from concepts concept"
                + " join concept_refset clinical on  clinical.referencedconceptid = concept.id"
                + " where concept.id = " + conceptSctid
                + " and concept.active = " + Metadata.ACTIVE_STATUS_VALUE
                + " order by concept.id DESC");

        try {
            if(resultSet != null){
                while (resultSet.next()) {
                    refsetIds.add(resultSet.getLong(1));
                }
            }
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }

        return refsetIds;
    }

    /**
     * Finds all the active descriptions for the <code>conceptSctid</code>.
     *
     * @param conceptSctid long
     * @return Map of ADRS language type <code>Long</code> keyed by the description term <code>String</code>
     * @throws RuntimeException SQLException database errors.
     */
    private static Map<String, Long> findConceptDescriptions(long conceptSctid) throws RuntimeException {
        Map<String, Long> refsetIds = new HashMap<String, Long>();
        ResultSet resultSet = runSql("select description.term, adrs.valueid"
                + " from concepts concept"
                + " join descriptions description on description.conceptid = concept.id"
                + " left join description_refset adrs on  adrs.referenceddescriptionid = description.id"
                + " where concept.id = " + conceptSctid
                + " and description.active = " + Metadata.ACTIVE_STATUS_VALUE
                + " and concept.active = " + Metadata.ACTIVE_STATUS_VALUE
                + " order by description.term, description.effectivetime DESC");

        try {
            if(resultSet != null){
                while (resultSet.next()) {
                    refsetIds.put(resultSet.getString(1), resultSet.getLong(2));
                }
            }
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }

        return refsetIds;
    }

    /**
     * Executes the <code>sql</code> with no limit on the number of returned rows
     *
     * @param sql String
     * @return ResultSet
     * @throws RuntimeException SQLException database errors.
     */
	private static ResultSet runSql(String sql) throws RuntimeException {
        return runSql(sql, 0);
    }

    /**
     * Executes the <code>sql</code> and limits the number of returned rows
     *
     * @param sql String
     * @param maxRows int
     * @return ResultSet
     * @throws RuntimeException SQLException database errors.
     */
    private static ResultSet runSql(String sql, int maxRows) throws RuntimeException {
        ResultSet resultSet = null;

        Statement statement;
        try {
            statement = dataSource.getConnection().createStatement();
            statement.setMaxRows(maxRows);
            if (statement.execute(sql)) {
                resultSet = statement.getResultSet();
            }
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }

        return resultSet;
    }

    /**
     * Creates a {@link Concept} object from the <code>conceptId</code> and finds the descriptions and reference sets
     *
     * @param conceptId long
     * @return Concept
     * @throws SQLException
     */
    private static Concept getConceptDetails(long conceptId) throws SQLException {
        Map<String, Long> descriptionAdrsMap;

        Concept concept = new Concept();
        concept.setSctId(conceptId);

        descriptionAdrsMap = findConceptDescriptions(concept.getSctId());
        for (String term : descriptionAdrsMap.keySet()) {
            concept.addDescription(term, descriptionAdrsMap.get(term));
        }

        HashSet<Long> refsetIds = findConceptRefsets(concept.getSctId());
        for (Long refsetId : refsetIds) {
            concept.addRefsetMembership(refsetId);
        }

        return concept;
    }

    /**
     * Get the maximum number of results that will be returned by a find operation.
     */
    public static int getResultLimit() {
        return dataSource.getMaxRows();
    }
}
