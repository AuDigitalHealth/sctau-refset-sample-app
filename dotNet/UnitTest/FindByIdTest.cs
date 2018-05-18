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
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace CTDemo.UnitTest
{
    /// <summary>
    /// Test searching for a concept by SCTID 
    /// </summary>
    [TestClass]
    public class FindByIdTest
    {
        private const long INVALID_CONCEPT_ID = 123L;
        
        // Concept: 'Access instrument' 
        private const long KNOWN_INACTIVE_CONCEPT_ID = 370127007L;
        
        // Concept: 'Fifth metatarsal structure' 
        private const long KNOWN_ACTIVE_CONCEPT_ID = 301000L;

        [TestMethod]
        public void TestKnownId()
        {
            Concept concept = ConceptFinder.FindById(KNOWN_ACTIVE_CONCEPT_ID);
            Assert.IsNotNull(concept, "Known concept not found");
            Assert.IsTrue(concept.sctId == KNOWN_ACTIVE_CONCEPT_ID, "Incorrect concept id");
            Assert.IsTrue("Fifth metatarsal structure".Equals(concept.GetPreferredTerm()),"Incorrect en-AU preferred term");
        }
    
        [TestMethod]
        public void TestUnknownId() {
            Concept concept = ConceptFinder.FindById(INVALID_CONCEPT_ID);
            Assert.IsNull(concept,"Unexpected concept found for invalid id");
        }

         [TestMethod]
        public void TestInactiveConcept() {
            Concept concept = ConceptFinder.FindById(KNOWN_INACTIVE_CONCEPT_ID);
            Assert.IsNull(concept,"Inactive concept not expected");
        }
    

    }
}
