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
    public class EtatTests
    {
        private const string id = "01";
        private const string libelle = "Libelle de l etat";
        private static readonly Etat etat = new Etat(id, libelle);
        [TestMethod()]
        public void EtatTest()
        {
            Assert.AreEqual(id, etat.Id, "Devrait réussir => id valorisé");
            Assert.AreEqual(libelle, etat.Libelle, "Devrait réussir => libelle valorisé");
        }
    }
}