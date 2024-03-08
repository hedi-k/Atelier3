using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MediaTekDocuments.controller;
using MediaTekDocuments.model;

namespace MediaTekDocuments.view
{
    /// <summary>
    /// Classe d'authentification
    /// </summary>
    public partial class FrmAuthentification : Form
    {
 
        private FrmAuthentificationController controller;
        /// <summary>
        /// Constructeur de la classe
        /// </summary>
        public FrmAuthentification()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Lien avec le contrôleur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmAuthentification_Load(object sender, EventArgs e)
        {
            controller = new FrmAuthentificationController();
        }

        /// <summary>
        /// Action du bouton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            String login = txtLogin.Text;
            String pwd = txtPwd.Text;
            
            Utilisateur utilisateur = new Utilisateur("", login, pwd, "");
            Utilisateur utilisateurConnecte = controller.Authentification(utilisateur);
            //Si le bon couple mot de passee utilisateur est entré il sera retourné et l'application va se lancer
            if (utilisateurConnecte != null)
            {
                this.Hide();
                FrmMediatek frm = new FrmMediatek(utilisateurConnecte);
                frm.ShowDialog();
            }
            else { MessageBox.Show("Authentification incorrecte !"); }

        }
        /// <summary>
        /// Pour masquer les lettres que l'on entre comme mot de passe.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPwd_TextChanged(object sender, EventArgs e)
        {
            txtPwd.PasswordChar = '*';
        }
    }
}
