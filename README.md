*NOTE: It is not recommended that the terminology services be accessed through SQL. The following methods are suggested as an alternative:*
* *FHIR*: https://www.healthterminologies.gov.au/specs/v2/conformant-server-apps/fhir-api
* *Syndication API*: https://www.healthterminologies.gov.au/specs/v2/conformant-server-apps/syndication-api 

# SNOMED CT-AU Refset Sample Application

## Purpose
The purpose of this project is to provide developers with a basic introduction to SNOMED CT-AU (RF2) files using a sample application. *The code is provided to aid understanding of the RF2 format, not as a basis for implementation of SNOMED CT-AU.*

The techniques and code used in this repository ***are NOT*** recommended as a method of implementing SNOMED CT-AU, a terminology server and/or a bespoke data structure for the particular requirements should be used instead. Detail knowledge of the release format described by these scripts and the implementation guide are required to build, but *not* when using an appropriate terminology server, which will abstract these details away and typically provide much more relevant search functionality and algorithms.

The National Clinical Terminology Service provides an HL7 FHIR terminology service endpoint containing up to date terminologies for Australia, the [National Terminology Server](https://www.healthterminologies.gov.au/tools?content=nts). The NCTS also makes available the terminology server software available at this endpoint free for use in Australian health care which can synchronise in content from the NCTS and be augmented with additional FHIR terminology resources as required, [more details on this refer to the NCTS website](https://www.healthterminologies.gov.au/tools?content=onto).

Vendors and implementers in Australia are encouraged to use these services where possible to avoid the duplicated effort of implementing the details described by these scripts and insulate themselves from possible future change to them.

## Getting Started
This repository contains project files for both Java and .NET projects. README files to get started can be found in the following locations:
* [Java](java/README.md)
* [.NET](dotNet/README.md)

Both versions of the application demonstrate one example of:
1. Loading the SNOMED CT-AU content from the Terminology Bundle (SNOMED CT RF2) into a relational database.
2. Building and executing either a Java or .NET application to perform some simple query operations to obtain information from the SNOMED CT-AU content.

## Documentation
Detailed documentation of the application design and purpose can be found in the [OVERVIEW](docs/OVERVIEW.md) file.