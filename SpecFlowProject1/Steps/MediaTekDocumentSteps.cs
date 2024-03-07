using System;
using System.Windows.Forms;
using TechTalk.SpecFlow;
using MediaTekDocuments.view;
using MediaTekDocuments.model;
using NUnit.Framework;

namespace SpecFlowProject1.Features
{
    [Binding]
    public class MediaTekDocumentSteps
    {
        private static Utilisateur user = new Utilisateur("a", "a", "a", "01");
        private readonly FrmMediatek frmMediatek = new FrmMediatek(user);
       

        
        [Given(@"je saisie ""([^""]*)""")]
        public void GivenJeSaisieLeTitreLaPlaneteDesSingesDansTxbLivresTitreRecherche(string valeur)
        {
            TabControl tabOngletsApplication = (TabControl)frmMediatek.Controls["tabOngletsApplication"];
            frmMediatek.Visible = true;
            tabOngletsApplication.SelectedTab = (TabPage)tabOngletsApplication.Controls["tabLivres"];
            TextBox txbLivresTitreRecherche = (TextBox)frmMediatek.Controls["tabOngletsApplication"].Controls["tabLivres"].Controls["grpLivresRecherche"].Controls["txbLivresTitreRecherche"];
            txbLivresTitreRecherche.Text = valeur;
        }

        [Then(@"Il doit apparaître dans le titre des infos détaillé ""([^""]*)""")]
        public void ThenIlDoitApparaitreDansLeTitreDesInfosDetailleTxbLivresTitre(string resultat)
        {
            TextBox txbLivresTitre = (TextBox)frmMediatek.Controls["tabOngletsApplication"].Controls["tabLivres"].Controls["grpLivresInfos"].Controls["txbLivresTitre"];
            Assert.That(txbLivresTitre.Text, Is.EqualTo(resultat));
        }
        
        
    }
}
