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
    public class DocumentTests
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
        private static readonly Document doc = new Document(id, titre, image, idGenre, genre, idPublic, labellePublic, idRaron, rayon);
        [TestMethod()]
        public void DocumentTest()
        {
            Assert.AreEqual(id, doc.Id, "Devrait réussir => id valorisé");
            Assert.AreEqual(titre, doc.Titre, "Devrait réussir => titre valorisé");
            Assert.AreEqual(image, doc.Image, "Devrait réussir => image valorisée");
            Assert.AreEqual(idGenre, doc.IdGenre, "Devrait réussir => idGenre valorisé");
            Assert.AreEqual(genre, doc.Genre, "Devrait réussir => genre valorisé");
            Assert.AreEqual(idPublic, doc.IdPublic, "Devrait réussir => idPublic valorisé");
            Assert.AreEqual(labellePublic, doc.Public, "Devrait réussir => public valorisé");
            Assert.AreEqual(idRaron, doc.IdRayon, "Devrait réussir => idRayon valorisé");
            Assert.AreEqual(rayon, doc.Rayon, "Devrait réussir => rayon valorisé");
        }
    }
}