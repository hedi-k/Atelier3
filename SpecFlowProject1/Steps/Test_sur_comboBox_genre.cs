using System;
using System.Windows.Forms;
using TechTalk.SpecFlow;
using MediaTekDocuments.view;
using MediaTekDocuments.model;
using NUnit.Framework;

namespace SpecFlowProject1.Steps
{
    [Binding]
    public class Test_sur_comboBox_genre
    {
        private static Utilisateur user = new Utilisateur("a", "a", "a", "01");
        private readonly FrmMediatek frmMediatek = new FrmMediatek(user);

        [When(@"je saisie un genre ""(.*)""")]
        public void WhenJeSaisieUnGenre(string valeur)
        {
            TabControl tabOngletsApplication = (TabControl)frmMediatek.Controls["tabOngletsApplication"];
            frmMediatek.Visible = true;
            tabOngletsApplication.SelectedTab = (TabPage)tabOngletsApplication.Controls["tabLivres"];
            ComboBox cbxLivresGenres = (ComboBox)frmMediatek.Controls["tabOngletsApplication"].Controls["tabLivres"].Controls["grpLivresRecherche"].Controls["cbxLivresGenres"];
            int indiceLigne = cbxLivresGenres.FindStringExact(valeur);
            cbxLivresGenres.SelectedIndex = indiceLigne;
        }
        
        [Then(@"Le premier titre trouvé sur ce Genre est ""(.*)""")]
        public void ThenLePremierTitreTrouveSurCeGenreEst(string resultat)
        {
            TextBox txbLivresTitre = (TextBox)frmMediatek.Controls["tabOngletsApplication"].Controls["tabLivres"].Controls["grpLivresInfos"].Controls["txbLivresTitre"];
            Assert.That(txbLivresTitre.Text, Is.EqualTo(resultat)); ;
        }
    }
}
