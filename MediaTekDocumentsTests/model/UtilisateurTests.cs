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
    public class UtilisateurTests
    {
        private const string id = "01";
        private const string nom = "user";
        private const string pwd = "mdp";
        private const string idService = "02";
        private static readonly Utilisateur user = new Utilisateur(id, nom, pwd, idService);


    [TestMethod()]
        public void UtilisateurTest()
        {
            Assert.AreEqual(id, user.Id, "Devrait réussir => id valorisé");
            Assert.AreEqual(nom, user.Nom, "Devrait réussir => nom valorisé");
            Assert.AreEqual(pwd, user.Pwd, "Devrait réussir => mot de passe valorisé");
            Assert.AreEqual(idService, user.IdService, "Devrait réussir => id servuce valorisé");

        }
    }
}