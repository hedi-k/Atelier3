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
    public class SuiviTests
    {
        private const string id = "01";
        private const string libelle = "Libelle suivi";
        private static readonly Suivi suivi = new Suivi(id, libelle);

        [TestMethod()]
        public void SuiviTest()
        {
            Assert.AreEqual(id, suivi.Id, "Devrait réussir => id valorisé");
            Assert.AreEqual(libelle, suivi.Libelle, "Devrait réussir => libelle valorisé");
        }
    }
}