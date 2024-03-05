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
    public class GenreTests
    {
        private const string id = "01";
        private const string libelle = "Libelle genre";
        private static readonly Genre genre = new Genre (id, libelle);
        [TestMethod()]
        public void GenreTest()
        {
            Assert.AreEqual(id, genre.Id, "Devrait réussir => id valorisé");
            Assert.AreEqual(libelle, genre.Libelle, "Devrait réussir => libelle valorisé");
        }
    }
}