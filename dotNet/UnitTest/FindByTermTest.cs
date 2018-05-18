/** Copyright (c) 2018 Australian Digital Health Agency
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CTDemo.UnitTest
{
    /// <summary>
    /// Test searching for concepts by textual term (description) 
    /// </summary>
    [TestClass]
    public class FindByTermTest
    {

    // Concept: Punch drunk 
    private const long KNOWN_INACTIVE_CONCEPT_ID = 51996004L;
    
    // Concept: Punch drunk 
    private const long KNOWN_ACTIVE_CONCEPT_ID = 230283005L;
 
     /// <summary>
     /// Search for the term "drunk". This has two concepts with the same description (a duplication) however
     /// one is inactive - so the test is that the search result should contain the active concept but not the
     /// inactive concept.
     /// <summary>
     [TestMethod]
    public void TestKnownTerm() {
        List<Concept> results = ConceptFinder.FindByTerm("drunk");
        
        Assert.IsNotNull(results,"Expected an empty collection, not null reference");
        Assert.IsFalse(results.Count == 0, "Known term not found");
        Assert.IsTrue(results.Count > 1,"Expected more than one concept found for known term");
        
        bool foundActive = false;
        bool foundInactive = false;

        foreach (Concept concept in results)
        {
            foundActive |= (concept.sctId == KNOWN_ACTIVE_CONCEPT_ID);
            foundInactive |= (concept.sctId == KNOWN_INACTIVE_CONCEPT_ID);            
        }

        Assert.IsTrue(foundActive, "Expected active concept was not present in results");
        Assert.IsFalse(foundInactive, "Inactive concept was present in the result, should only contain active concepts");
    }
    

     [TestMethod]
    public void TestUnknownTerm() {
        List<Concept> results = ConceptFinder.FindByTerm("wakawaka");
        
        Assert.IsNotNull(results,"Expected an empty collection, not null reference");
        Assert.IsTrue(results.Count == 0,"Expected no concepts for known invalid term");
    }
    
     [TestMethod]
    public void TestNoDuplicates() {
        HashSet<long> uniqueIds = new HashSet<long>();

        List<Concept> results = ConceptFinder.FindByTerm("heart");
        
        foreach (Concept c in results) {
            long sctId = c.sctId;
            Assert.IsFalse(uniqueIds.Contains(sctId),"Duplicate concept with SCTID " + sctId.ToString() + " returned in result");
            uniqueIds.Add(sctId);
        }
    }

     [TestMethod]
    public void TestMaxRows() {
        int resultLimit = DataSource.GetMaxRows();
        List<Concept> results = ConceptFinder.FindByTerm("heart");
        Assert.IsFalse(results.Count > resultLimit,"Number of results returned exceeds the maximum row count specified");
    }


    }
}
