using System;
using TechTalk.SpecFlow;
using System.Windows.Forms;
using TechTalk.SpecFlow;
using MediaTekDocuments.view;
using MediaTekDocuments.model;
using NUnit.Framework;

namespace SpecFlowProject1.Steps
{
    [Binding]
    public class Test_sur_Id
    {
        private static Utilisateur user = new Utilisateur("a", "a", "a", "01");
        private readonly FrmMediatek frmMediatek = new FrmMediatek(user);

        [Given(@"je saisie l'id ""(.*)""")]
        public void GivenJeSaisieLId(string valeur)
        {
            TabControl tabOngletsApplication = (TabControl)frmMediatek.Controls["tabOngletsApplication"];
            frmMediatek.Visible = true;
            tabOngletsApplication.SelectedTab = (TabPage)tabOngletsApplication.Controls["tabLivres"];
            TextBox txbLivresNumRecherche = (TextBox)frmMediatek.Controls["tabOngletsApplication"].Controls["tabLivres"].Controls["grpLivresRecherche"].Controls["txbLivresNumRecherche"];
            txbLivresNumRecherche.Text = valeur;
        }
        
        [When(@"je clic sur le bouton recherche")]
        public void WhenJeClicSurLeBoutonRecherche()
        {
            Button btnLivresNumRecherche = (Button)frmMediatek.Controls["tabOngletsApplication"].Controls["tabLivres"].Controls["grpLivresRecherche"].Controls["btnLivresNumRecherche"];
            btnLivresNumRecherche.PerformClick();
        }
        
        [Then(@"il doit me trouver le titre ""(.*)""")]
        public void ThenIlDoitMeTrouverLeTitre(string resultat)
        {
            TextBox txbLivresTitre = (TextBox)frmMediatek.Controls["tabOngletsApplication"].Controls["tabLivres"].Controls["grpLivresInfos"].Controls["txbLivresTitre"];
            Assert.That(txbLivresTitre.Text, Is.EqualTo(resultat));
        }
    }
}
