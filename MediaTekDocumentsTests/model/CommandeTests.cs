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
    public class CommandeTests
    {
        private const string id = "01";
        private static DateTime dateCmd = new DateTime(2023, 01, 01);
        private const double montant = 4;
        private static readonly Commande cmd = new Commande(id, dateCmd, montant);
        [TestMethod()]
        public void CommandeTest()
        {
            Assert.AreEqual(id, cmd.Id, "Devrait réussir => id valorisé");
            Assert.AreEqual(dateCmd, cmd.DateCommande, "Devrait réussir => date valorisée");
            Assert.AreEqual(montant, cmd.Montant, "Devrait réussir => montant valorisé");
        }
    }
}