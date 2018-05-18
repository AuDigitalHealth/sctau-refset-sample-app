
-- Create indexes to support queries executed by the application

CREATE INDEX concepts_id_active_idx ON concepts(id, active);

CREATE INDEX descriptions_id_active_idx ON descriptions(id, active);

CREATE INDEX descriptions_concept_id_idx ON descriptions(conceptid, active);

CREATE INDEX concept_refset_referenced_concept_id_idx ON concept_refset(referencedconceptid);

CREATE INDEX description_refset_referenced_description_id_idx ON description_refset(referenceddescriptionid);

