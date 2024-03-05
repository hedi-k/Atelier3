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
    public class RevueTests
    {
        private const string id = "01";
        private const string titre = "Le Titre";
        private const string image = "chemin image";
        private const string idGenre = "02";
        private const string genre = "genre";
        private const string idPublic = "03";
        private const string labellePublic = "public";
        private const string idRaron = "04";
        private const string rayon = "rayon";
        private const string periodicite = "MS";
        private const int delai = 10;
        private static readonly Revue revue = new Revue(id, titre, image, idGenre, genre, idPublic, labellePublic, idRaron, rayon, periodicite, delai);

        [TestMethod()]
        public void RevueTest()
        {
            Assert.AreEqual(id, revue.Id, "Devrait réussir => id valorisé");
            Assert.AreEqual(titre, revue.Titre, "Devrait réussir => titre valorisé");
            Assert.AreEqual(image, revue.Image, "Devrait réussir => image valorisée");
            Assert.AreEqual(idGenre, revue.IdGenre, "Devrait réussir => idGenre valorisé");
            Assert.AreEqual(genre, revue.Genre, "Devrait réussir => genre valorisé");
            Assert.AreEqual(idPublic, revue.IdPublic, "Devrait réussir => idPublic valorisé");
            Assert.AreEqual(labellePublic, revue.Public, "Devrait réussir => public valorisé");
            Assert.AreEqual(idRaron, revue.IdRayon, "Devrait réussir => idRayon valorisé");
            Assert.AreEqual(rayon, revue.Rayon, "Devrait réussir => rayon valorisé");
            Assert.AreEqual(periodicite, revue.Periodicite, "Devrait réussir => périodicité valorisée");
            Assert.AreEqual(delai, revue.DelaiMiseADispo, "Devrait réussir => delai  valirusé");
        }
    }
}