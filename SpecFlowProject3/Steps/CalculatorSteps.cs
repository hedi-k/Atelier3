using System;
using TechTalk.SpecFlow;
using MediaTekDocuments.view;
using NUnit.Framewor;
using System.Windows.Forms;
using Intersoft.Crosslight.WinRT.Controls;

namespace SpecFlowProject3.Steps
{
    [Binding]
    public class CalculatorSteps
    {
        private readonly FrmMediatek frmMediatek = new FrmMediatek(null);

       [Given(@"je saisie ""(.*)""")]
        public void GivenJeSaisie(string valeur)
        {
            TabControl tabOngletsApplication = (TabControl)frmMediatek.Controls["tabControl"];
            frmMediatek.Visible = true;
            tabOngletsApplication.SelectedTab = (TabPage)tabOngletsApplication.Controls["tabLivres"];
            TextBox txbLivresTitreRecherche = (TextBox)frmMediatek.Controls["tabControl"].Controls["tabLivres"].Controls["grpLivresRecherche"].Controls["txbLivresTitreRecherche"];
            txbLivresTitreRecherche.Text = valeur;
        }
        
        [Then(@"Il doit apparaître dans le titre des infos détaillé ""(.*)""")]
        public void ThenIlDoitApparaitreDansLeTitreDesInfosDetaille(string p0)
        {
            TextBox txbLivresTitre = (TextBox)frmMediatek.Controls["tabControl"].Controls["tabLivres"].Controls["grpLivresInfos"].Controls["txbLivresTitre"];
            Assert.Equals(txbLivresTitre.Text, resultat);

        }
    }
}
