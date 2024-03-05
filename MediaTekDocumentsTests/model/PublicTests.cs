using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaTekDocuments.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model.Tests
{
    [TestClass()]
    public class PublicTests
    {
        private const string id = "01";
        private const string libelle = "Libelle public";
        private static readonly Public unpublic = new Public(id, libelle);

        [TestMethod()]
        public void PublicTest()
        {
            Assert.AreEqual(id, unpublic.Id, "Devrait réussir => id valorisé");
            Assert.AreEqual(libelle, unpublic.Libelle, "Devrait réussir => libelle valorisé");
        }
    }
}