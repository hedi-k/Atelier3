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
    public class LivreTests
    {
        private const string id = "01";
        private const string titre = "Le Titre";
        private const string image = "chemin image";
        private const string ibsn = "000000";
        private const string auteur = "nom auteur";
        private const string collection = "nom collection";
        private const string idGenre = "02";
        private const string genre = "genre";
        private const string idPublic = "03";
        private const string labellePublic = "public";
        private const string idRaron = "04";
        private const string rayon = "rayon";
        private static readonly Livre livre = new Livre(id, titre, image, ibsn, auteur, collection, idGenre, genre, idPublic, labellePublic, idRaron, rayon);

        [TestMethod()]
        public void LivreTest()
        {
            Assert.AreEqual(id, livre.Id, "Devrait réussir => id valorisé");
            Assert.AreEqual(titre, livre.Titre, "Devrait réussir => titre valorisé");
            Assert.AreEqual(image, livre.Image, "Devrait réussir => image valorisée");
            Assert.AreEqual(ibsn, livre.Isbn, "Devrait réussir => isbn valorisée");
            Assert.AreEqual(auteur, livre.Auteur, "Devrait réussir => auteur valorisé");
            Assert.AreEqual(collection, livre.Collection, "Devrait réussir => collection valorisée");
            Assert.AreEqual(idGenre, livre.IdGenre, "Devrait réussir => idGenre valorisé");
            Assert.AreEqual(genre, livre.Genre, "Devrait réussir => genre valorisé");
            Assert.AreEqual(idPublic, livre.IdPublic, "Devrait réussir => idPublic valorisé");
            Assert.AreEqual(labellePublic, livre.Public, "Devrait réussir => public valorisé");
            Assert.AreEqual(idRaron, livre.IdRayon, "Devrait réussir => idRayon valorisé");
            Assert.AreEqual(rayon, livre.Rayon, "Devrait réussir => rayon valorisé");
        }
    }
}