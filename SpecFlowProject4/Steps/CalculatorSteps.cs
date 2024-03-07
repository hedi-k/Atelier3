using System;
using TechTalk.SpecFlow;
using MediaTekDocuments.view;
using NUnit.Framework;
using System.Windows.Forms;
using MediaTekDocuments.model;

namespace SpecFlowProject4.Steps
{
    [Binding]
    public class CalculatorSteps
    {
        private static Utilisateur user = new Utilisateur("01", "admin", "mdp", "01");
        private readonly FrmMediatek frmMediatek = new FrmMediatek(user);

        [Given(@"je saisie ""(.*)""")]
        public void GivenJeSaisie(string p0)
        {
            TabControl tabOngletsApplication = (TabControl)frmMediatek.Controls["tabControl"];
            frmMediatek.Visible = true;
            tabOngletsApplication.SelectedTab = (TabPage)tabOngletsApplication.Controls["tabLivres"];
            TextBox txbLivresTitreRecherche = (TextBox)frmMediatek.Controls["tabControl"].Controls["tabLivres"].Controls["grpLivresRecherche"].Controls["txbLivresTitreRecherche"];
            txbLivresTitreRecherche.Text = p0;
        }
        
        [Then(@"Il doit apparaître dans le titre des infos détaillé ""(.*)""")]
        public void ThenIlDoitApparaitreDansLeTitreDesInfosDetaille(string resultat)
        {
            TextBox txbLivresTitre = (TextBox)frmMediatek.Controls["tabControl"].Controls["tabLivres"].Controls["grpLivresInfos"].Controls["txbLivresTitre"];
            Assert.Equals(txbLivresTitre.Text, resultat);
        }
    }
}
