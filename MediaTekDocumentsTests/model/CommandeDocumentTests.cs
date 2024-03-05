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
    public class CommandeDocumentTests
    {
        private const string id = "01";
        private static DateTime dateCmd = new DateTime(2020, 01, 01);
        private const double montant = 2;
        private const int nbExemplaire = 3;
        private const string idSuivi = "04";
        private const string Suivi = "relance";
        private const string idLivre = "05";
        private static readonly CommandeDocument cmdDoc = new CommandeDocument(id, dateCmd, montant,  nbExemplaire, idSuivi,Suivi, idLivre);
        [TestMethod()]
        public void CommandeDocumentTest()
        {
            Assert.AreEqual(id, cmdDoc.Id, "Devrait réussir => id valorisé");
            Assert.AreEqual(dateCmd, cmdDoc.DateCommande, "Devrait réussir => date commande valorisée");
            Assert.AreEqual(montant, cmdDoc.Montant, "Devrait réussir => montant valorisé");
            Assert.AreEqual(nbExemplaire, cmdDoc.NbExemplaire, "Devrait réussir => nombre exemplaire valorisé");
            Assert.AreEqual(idSuivi, cmdDoc.IdSuivi, "Devrait réussir => id suivi valorisé");
            Assert.AreEqual(Suivi, cmdDoc.Suivi, "Devrait réussir => Suivi valorisé");
            Assert.AreEqual(idSuivi, cmdDoc.IdSuivi, "Devrait réussir => id Suivi valorisé");
        }
    }
}