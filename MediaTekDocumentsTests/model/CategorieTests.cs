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
    public class CategorieTests
    {
        private const string id = "01";
        private const string libelle = "titreLibelle";
        private static readonly Categorie cat = new Categorie(id, libelle);
        [TestMethod()]
        public void CategorieTest()
        {
            Assert.AreEqual(id, cat.Id, "Devrait réussir => id valorisé");
            Assert.AreEqual(libelle, cat.Libelle, "Devrait réussir => libelle valorisé");
        }

        [TestMethod()]
        public void ToStringTest()
        {
            Assert.AreEqual(libelle, cat.Libelle, "Devrait réussir => libelle pour les combos");
        }
    }
}