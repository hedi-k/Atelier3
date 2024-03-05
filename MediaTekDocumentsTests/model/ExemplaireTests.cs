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
    public class ExemplaireTests
    {
        private const int numero = 01;
        private const string photo = "chemin photo";
        private static DateTime dateAchat = new DateTime(2020, 01, 01);
        private const string idEtat = "02";
        private const string id = "03";
        private static readonly Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, id);

       [TestMethod()]
        public void ExemplaireTest()
        {
            Assert.AreEqual(numero, exemplaire.Numero, "Devrait réussir => numero valorisé");
            Assert.AreEqual(photo, exemplaire.Photo, "Devrait réussir => photo valorisée");
            Assert.AreEqual(dateAchat, exemplaire.DateAchat, "Devrait réussir => date début valorisée");
            Assert.AreEqual(idEtat, exemplaire.IdEtat, "Devrait réussir => idEtat valorisé");
            Assert.AreEqual(id, exemplaire.Id, "Devrait réussir => id valorisé");
        }
    }
}