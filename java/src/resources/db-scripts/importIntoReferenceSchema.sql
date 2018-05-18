-- RF2_CONCEPTS
TRUNCATE TABLE concepts;
LOAD DATA LOCAL INFILE '${rf2.concepts.file}' 
    INTO TABLE concepts 
    CHARACTER SET 'utf8' 
    IGNORE 1 LINES (id,@effectivetime,active,moduleid,definitionstatusid) 
    set effectivetime = str_to_date(@effectivetime, '%Y%m%d');

-- RF2_DESCRIPTIONS
TRUNCATE TABLE descriptions;
LOAD DATA LOCAL INFILE '${rf2.descriptions.file}' 
    INTO TABLE descriptions 
    CHARACTER SET 'utf8' 
    IGNORE 1 LINES (id,@effectivetime,active,moduleid,conceptid,languagecode,typeid,term,casesignificanceid) 
    set effectivetime = str_to_date(@effectivetime, '%Y%m%d');

-- RF2_RELATIONSHIPS
TRUNCATE TABLE relationships;
LOAD DATA LOCAL INFILE '${rf2.relationships.file}' 
    INTO TABLE relationships 
    CHARACTER SET 'utf8' 
    IGNORE 1 LINES (id,@effectivetime,active,moduleid,sourceid,destinationid,relationshipgroup,typeid,characteristictypeid,modifierid) 
    set effectivetime = str_to_date(@effectivetime, '%Y%m%d');

-- RF2_IDS
TRUNCATE TABLE identifiers;
LOAD DATA LOCAL INFILE '${rf2.identifiers.file}' 
    INTO TABLE identifiers 
    CHARACTER SET 'utf8' 
    IGNORE 1 LINES (identifierschemeid,alternativeidentifier,@effectivetime,active,referencedcomponentid) 
    set effectivetime = str_to_date(@effectivetime, '%Y%m%d');

-- RF2_CLINICAL_REFSETS (Combined)
TRUNCATE TABLE concept_refset;
LOAD DATA LOCAL INFILE '${refsets.file}' 
    INTO TABLE concept_refset 
    CHARACTER SET 'utf8' (id,@effectivetime,active,moduleid,refsetid,referencedconceptid) 
    set effectivetime = str_to_date(@effectivetime, '%Y%m%d');

-- RF2_EN-AU_LANGUAGE_REFSET
TRUNCATE TABLE description_refset;
LOAD DATA LOCAL INFILE '${rf2.adrs.file}' 
    INTO TABLE description_refset 
    CHARACTER SET 'utf8' 
    IGNORE 1 LINES (id,@effectivetime,active,moduleid,refsetid,referenceddescriptionid,valueid) 
    set effectivetime = str_to_date(@effectivetime, '%Y%m%d');

