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
    public class RayonTests
    {
        private const string id = "01";
        private const string libelle = "Libelle rayon";
        private static readonly Public rayon = new Public(id, libelle);
        [TestMethod()]
        public void RayonTest()
        {
            Assert.AreEqual(id, rayon.Id, "Devrait réussir => id valorisé");
            Assert.AreEqual(libelle, rayon.Libelle, "Devrait réussir => libelle valorisé");
        }
    }
}