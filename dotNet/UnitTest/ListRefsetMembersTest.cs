using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CTDemo.UnitTest
{
    /// <summary>
    /// Test a valid and invalid RefsetId
    /// </summary>
    [TestClass]
    public class ListRefsetMembersTest
    {
     private const long KNOWN_REFSET = 32570331000036102L;
     private const long INVALID_REFSET = 32570031000036104L;

    [TestMethod]
    public void TestValidRefsetId() {
        List<Concept> members = ConceptFinder.FindRefsetMembers(KNOWN_REFSET);
        Assert.IsNotNull(members,"Expecting an object reference, not null");
        Assert.IsFalse(members.Count == 0, "Refset should contain members");
    }

    [TestMethod]
    public void TestInvalidRefsetId() {
        List<Concept> members = ConceptFinder.FindRefsetMembers(INVALID_REFSET);
        Assert.IsNotNull(members,"Expected an empty collection, not null reference");
        Assert.IsTrue( members.Count == 0,"No members should be returned for invalid refset");
    }

  }
}
