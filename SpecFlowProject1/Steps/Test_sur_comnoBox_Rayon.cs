using System;
using System.Windows.Forms;
using TechTalk.SpecFlow;
using MediaTekDocuments.view;
using MediaTekDocuments.model;
using NUnit.Framework;

namespace SpecFlowProject1.Steps
{
    [Binding]
    public class Test_sur_comnoBox_Rayon
    {
        private static Utilisateur user = new Utilisateur("a", "a", "a", "01");
        private readonly FrmMediatek frmMediatek = new FrmMediatek(user);

        [When(@"je saisie un rayon ""(.*)""")]
        public void WhenJeSaisieUnRayon(string valeur)
        {
            TabControl tabOngletsApplication = (TabControl)frmMediatek.Controls["tabOngletsApplication"];
            frmMediatek.Visible = true;
            tabOngletsApplication.SelectedTab = (TabPage)tabOngletsApplication.Controls["tabLivres"];
            ComboBox cbxLivresRayons = (ComboBox)frmMediatek.Controls["tabOngletsApplication"].Controls["tabLivres"].Controls["grpLivresRecherche"].Controls["cbxLivresRayons"];
            int indiceLigne = cbxLivresRayons.FindStringExact(valeur);
            cbxLivresRayons.SelectedIndex = indiceLigne;
        }
        
        [Then(@"Le premier titre trouvé sur ce rayon ""(.*)""")]
        public void ThenLePremierTitreTrouveSurCeRayon(string resultat)
        {
            TextBox txbLivresTitre = (TextBox)frmMediatek.Controls["tabOngletsApplication"].Controls["tabLivres"].Controls["grpLivresInfos"].Controls["txbLivresTitre"];
            Assert.That(txbLivresTitre.Text, Is.EqualTo(resultat)); ;
        }
    
    }
}
