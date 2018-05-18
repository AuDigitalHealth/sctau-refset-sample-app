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
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CTDemo.UnitTest
{
    /// <summary>
    /// Check the reference set memberships obtained for a concept.
    /// </summary>
    [TestClass]
    public class FindRefsetMembershipTest
    {

    // Concept: 'Fifth metatarsal structure' 
    private const long KNOWN_ACTIVE_CONCEPT_ID = 301000L;

    [TestMethod]
    public void TestRefsetMembership() {
        Concept concept = ConceptFinder.FindById(KNOWN_ACTIVE_CONCEPT_ID);
        Assert.IsNotNull(concept,"Expecting an object reference, not null");
        
        IList<RefsetMember> memberships = concept.GetRefsetMemberships();
        Assert.IsNotNull(memberships,"Expecting an object reference, not null");
        Assert.IsFalse(memberships.Count == 0, "Refset should contain members");
        
        HashSet<String> memberKeys = new HashSet<String>();
        foreach (RefsetMember member in memberships) {

            Assert.IsNotNull(member.GetRefsetConcept(),"Refset membership missing refset concept");
            Assert.IsNotNull(member.GetReferencedConcept(),"Refset membership missing referenced concept");

          String key =
            member.GetRefsetConcept().sctId.ToString() +
            member.GetReferencedConcept().sctId.ToString();     
            if (memberKeys.Contains(key)) {
                Assert.Fail("Duplicate membership encounter");
            }
            memberKeys.Add(key);
        }
      }
    }
}
