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
    public class DvdTests
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
        private const int duree = 05;
        private const string realisateur = "nom réalisateur";
        private const string synopsis = "le synopsis";
        private static readonly Dvd dvd = new Dvd(id, titre, image, duree, realisateur, synopsis, idGenre, genre, idPublic, labellePublic, idRaron, rayon);

        [TestMethod()]
        public void DvdTest()
        {
            Assert.AreEqual(id, dvd.Id, "Devrait réussir => id valorisé");
            Assert.AreEqual(titre, dvd.Titre, "Devrait réussir => titre valorisé");
            Assert.AreEqual(image, dvd.Image, "Devrait réussir => image valorisée");
            Assert.AreEqual(duree, dvd.Duree, "Devrait réussir => durée valorisée");
            Assert.AreEqual(realisateur, dvd.Realisateur, "Devrait réussir => realisateur valorisé");
            Assert.AreEqual(synopsis, dvd.Synopsis, "Devrait réussir => synopsis valorisée");
            Assert.AreEqual(idGenre, dvd.IdGenre, "Devrait réussir => idGenre valorisé");
            Assert.AreEqual(genre, dvd.Genre, "Devrait réussir => genre valorisé");
            Assert.AreEqual(idPublic, dvd.IdPublic, "Devrait réussir => idPublic valorisé");
            Assert.AreEqual(labellePublic, dvd.Public, "Devrait réussir => public valorisé");
            Assert.AreEqual(idRaron, dvd.IdRayon, "Devrait réussir => idRayon valorisé");
            Assert.AreEqual(rayon, dvd.Rayon, "Devrait réussir => rayon valorisé");
        }
    }
}