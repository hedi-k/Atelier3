using NUnit.Framework;
using System;

namespace TestMediaTekDocuments
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            DateTime dateCommande = new DateTime(2024, 01, 01);
            DateTime dateFinAbonnement = new DateTime(2025, 01, 01);
            DateTime dateParution = new DateTime(2024, 02, 02);

            bool resultat = ParutionEntreCmdEtAbonnement(dateCommande, dateFinAbonnement, dateParution);
            Assert.IsFalse(resultat);
            Assert.Pass("essai");
        }

        //Compare les dates
        private bool ParutionEntreCmdEtAbonnement(DateTime dateCmd, DateTime dateFinAbo, DateTime dateParu)
        {
            //retourne faux si une parution est en cours.
            if (dateCmd < dateParu && dateFinAbo > dateParu)
            {
                return false;
            }
            else
            { 
                return true; 
            }
        }
    }
}