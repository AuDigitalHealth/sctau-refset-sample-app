DROP DATABASE IF EXISTS `{0}`;
CREATE DATABASE `{0}`;
USE `{0}`;

DROP TABLE IF EXISTS `concepts`;
CREATE TABLE IF NOT EXISTS `concepts` (
  `id` bigint(18) NOT NULL,
  `effectivetime` timestamp NOT NULL default CURRENT_TIMESTAMP on update CURRENT_TIMESTAMP,
  `active` int(1) NOT NULL,
  `moduleid` bigint(18) NOT NULL,
  `definitionstatusid` bigint(18) NOT NULL,
  PRIMARY KEY  (`id`,`effectivetime`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;


DROP TABLE IF EXISTS `descriptions`;
CREATE TABLE IF NOT EXISTS `descriptions` (
  `id` bigint(18) NOT NULL,
  `effectivetime` timestamp NOT NULL default CURRENT_TIMESTAMP on update CURRENT_TIMESTAMP,
  `active` int(1) NOT NULL,
  `moduleid` bigint(18) NOT NULL,
  `conceptid` bigint(18) NOT NULL,
  `languagecode` varchar(10) collate utf8_unicode_ci NOT NULL,
  `typeid` bigint(18) NOT NULL,
  `term` varchar(500) collate utf8_unicode_ci NOT NULL,
  `casesignificanceid` bigint(18) NOT NULL,
  PRIMARY KEY  (`id`,`effectivetime`),
  CONSTRAINT FOREIGN KEY (conceptid) REFERENCES concepts(id) ON DELETE CASCADE,
  CONSTRAINT FOREIGN KEY (moduleid) REFERENCES concepts(id) ON DELETE CASCADE,
  CONSTRAINT FOREIGN KEY (typeid) REFERENCES concepts(id) ON DELETE CASCADE
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;


DROP TABLE IF EXISTS `identifiers`;
CREATE TABLE IF NOT EXISTS `identifiers` (
  `identifierschemeid` bigint(18) NOT NULL,
  `alternativeidentifier` varchar(36) collate utf8_unicode_ci NOT NULL,
  `effectivetime` timestamp NOT NULL default CURRENT_TIMESTAMP on update CURRENT_TIMESTAMP,
  `active` int(1) NOT NULL,
  `moduleid` bigint(18) NOT NULL,
  `referencedcomponentid` bigint(18) NOT NULL,
  PRIMARY KEY  (`identifierschemeid`,`alternativeidentifier`,`effectivetime`),
  CONSTRAINT FOREIGN KEY (moduleid) REFERENCES concepts(id) ON DELETE CASCADE,
  CONSTRAINT FOREIGN KEY (identifierschemeid) REFERENCES concepts(id) ON DELETE CASCADE
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;


DROP TABLE IF EXISTS `relationships`;
CREATE TABLE IF NOT EXISTS `relationships` (
  `id` bigint(18) NOT NULL,
  `effectivetime` timestamp NOT NULL default CURRENT_TIMESTAMP on update CURRENT_TIMESTAMP,
  `active` int(1) NOT NULL,
  `moduleid` bigint(18) NOT NULL,
  `sourceid` bigint(18) NOT NULL,
  `destinationid` bigint(18) NOT NULL,
  `relationshipgroup` bigint(18) NOT NULL,
  `typeid` bigint(18) NOT NULL,
  `characteristictypeid` bigint(18) NOT NULL,
  `modifierid` bigint(18) NOT NULL,
  PRIMARY KEY  (`id`,`effectivetime`),
  CONSTRAINT FOREIGN KEY (moduleid) REFERENCES concepts(id) ON DELETE CASCADE,
  CONSTRAINT FOREIGN KEY (typeid) REFERENCES concepts(id) ON DELETE CASCADE,
  CONSTRAINT FOREIGN KEY (characteristictypeid) REFERENCES concepts(id) ON DELETE CASCADE,
  CONSTRAINT FOREIGN KEY (sourceid) REFERENCES concepts(id) ON DELETE CASCADE,
  CONSTRAINT FOREIGN KEY (destinationid) REFERENCES concepts(id) ON DELETE CASCADE
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;


DROP TABLE IF EXISTS `concept_refset`;
CREATE TABLE IF NOT EXISTS `concept_refset` (
  `id` varchar(36) collate utf8_unicode_ci NOT NULL,
  `effectivetime` timestamp NOT NULL default CURRENT_TIMESTAMP on update CURRENT_TIMESTAMP,
  `active` int(1) NOT NULL,
  `moduleid` bigint(18) NOT NULL,
  `refsetid` bigint(18) NOT NULL,
  `referencedconceptid` bigint(18) NOT NULL,
  PRIMARY KEY  (`id`,`effectivetime`),
  CONSTRAINT FOREIGN KEY (moduleid) REFERENCES concepts(id) ON DELETE CASCADE,
  CONSTRAINT FOREIGN KEY (refsetid) REFERENCES concepts(id) ON DELETE CASCADE,
  CONSTRAINT FOREIGN KEY (referencedconceptid) REFERENCES concepts(id) ON DELETE CASCADE
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;


DROP TABLE IF EXISTS `description_refset`;
CREATE TABLE IF NOT EXISTS `description_refset` (
  `id` varchar(36) collate utf8_unicode_ci NOT NULL,
  `effectivetime` timestamp NOT NULL default CURRENT_TIMESTAMP on update CURRENT_TIMESTAMP,
  `active` int(1) NOT NULL,
  `moduleid` bigint(18) NOT NULL,
  `refsetid` bigint(18) NOT NULL,
  `referenceddescriptionid` bigint(18) NOT NULL,
  `valueid` bigint(18) NOT NULL,
  PRIMARY KEY  (`id`,`effectivetime`),
  CONSTRAINT FOREIGN KEY (moduleid) REFERENCES concepts(id) ON DELETE CASCADE,
  CONSTRAINT FOREIGN KEY (refsetid) REFERENCES concepts(id) ON DELETE CASCADE,
  CONSTRAINT FOREIGN KEY (referenceddescriptionid) REFERENCES descriptions(id) ON DELETE CASCADE
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

