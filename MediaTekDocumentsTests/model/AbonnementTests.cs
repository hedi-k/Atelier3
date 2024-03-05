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
    public class AbonnementTests
    {
        private const string id = "01";
        private static DateTime dateDebutAbo = new DateTime(2020, 01, 01);
        private const double montant = 2;
        private static DateTime dateFinAbo = new DateTime(2021, 01, 01);
        private const string idRevue = "02";
        private static readonly Abonnement abo = new Abonnement(id, dateDebutAbo, montant, dateFinAbo, idRevue);
        [TestMethod()]
        public void AbonnementTest()
        {
            Assert.AreEqual(id, abo.Id, "Devrait réussir => id valorisé");
            Assert.AreEqual(dateDebutAbo, abo.DateCommande, "Devrait réussir => date début valorisée");
            Assert.AreEqual(montant, abo.Montant, "Devrait réussir => montant valorisé");
            Assert.AreEqual(dateFinAbo, abo.DateFinAbonnement, "Devrait réussir => date de fin valorisée");
            Assert.AreEqual(idRevue, abo.IdRevue, "Devrait réussir => id revue valorisé");
        }
    }
}