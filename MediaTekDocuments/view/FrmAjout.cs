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
using MediaTekDocuments.view;

namespace MediaTekDocuments.view
{
    public partial class FrmAjout : Form
    {
        private FrmMediatekController controller;
        private List<Livre> listeLivre;

        public FrmAjout(BindingSource bdgGenres2, BindingSource bdgPublics, BindingSource bdgRayons, bool txtBoxGPA, List<Livre> lesLivres)
        {
            InitializeComponent();
            RemplirCbx(bdgGenres2, cbxLivresGenres);
            RemplirCbx(bdgPublics, cbxLivresPublics);
            RemplirCbx(bdgRayons, cbxLivresRayons);
            AffichagetxtBoxGPA(txtBoxGPA);
            listeLivre = lesLivres;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            controller = new FrmMediatekController(); //Le controleur ne fonctionne pas sans!!

        }

        //remplit la comboBox reçut en paramètre en fonction de la bdg reçut  en paramètre
        private void RemplirCbx(BindingSource bdg, ComboBox cbx)
        {
            cbx.DataSource = bdg;
            if (cbx.Items.Count > 0)
            {
                cbx.SelectedIndex = -1;
            }
        }

        //Gestion de l'affichage des txtBox genre public rayon
        private void AffichagetxtBoxGPA(bool txtBoxGPA)
        {
            txbLivresGenre.Enabled = txtBoxGPA;
            txbLivresPublic.Enabled = txtBoxGPA;
            txbLivresRayon.Enabled = txtBoxGPA;
        }

        //action du bouton annuler.
        //ferme la fenêtre et retour à la précédente
        private void btnAnnuler_Click(object sender, EventArgs e)
        {
            FrmMediatek frmMediatek = new FrmMediatek();
            frmMediatek.Show();
            this.Hide();
        }

        //action du btn Ajouter
        //ajout d'un livre dans la bdd
        private void btnAjouter_Click(object sender, EventArgs e)
        {
            //valorise tout les paramètres
            string id = txbLivresNumero.Text;
            string titre = txbLivresTitre.Text;
            string image = txbLivresImage.Text;
            string isbn = txbLivresIsbn.Text;
            string auteur = txbLivresAuteur.Text;
            string collection = txbLivresCollection.Text;
            string idGenre = GetIdGenre(cbxLivresGenres.Text);
            string genre = txbLivresGenre.Text; //pas utilise pour créer un livre car selectionné dans la combo
            string idPublic = GetIdPublic(cbxLivresPublics.Text);
            string Public = txbLivresPublic.Text;//pas utilise pour créer un livre car selectionné dans la combo
            string idRayon = GetIdRayon(cbxLivresRayons.Text);
            string rayon = txbLivresRayon.Text;//pas utilise pour créer un livre car selectionné dans la combo
            //création de l'objet livre
            Livre livre = new Livre(id, titre, image, isbn, auteur, collection, idGenre, genre, idPublic, Public, idRayon, rayon);
            //controle les comboBox avant appel du constructeur
            if (idGenre != null && idPublic != null && idRayon != null)
            {
                //controle les txtBox avant appel du constructeur
                if (titre != "" && isbn != "" && auteur != "" && collection != "" && id != "")
                {
                    //controle que l'id n'est pas déja utilisé avant appel du constructeur
                    Livre testId = listeLivre.Find(x => x.Id.Equals(txbLivresNumero.Text));
                    if (testId != null)
                    {
                        MessageBox.Show("Cet id est déja utilisé.");
                    }
                    else
                    {
                        controller.EnvoiLivre(livre);
                        Vide();
                    }
                }
                else { MessageBox.Show("Sélectionnez un id, un titre, un auteur, un ISBN et une collection."); }
            }
            else { MessageBox.Show("Sélectionnez un genre, un public et un rayon."); }
        }

        private void Vide()
        {
            txbLivresNumero.Text = "";
            txbLivresTitre.Text = "";
            txbLivresImage.Text = "";
            txbLivresIsbn.Text = "";
            txbLivresAuteur.Text = "";
            txbLivresCollection.Text = "";
            cbxLivresGenres.Text = "";
            txbLivresGenre.Text = "";
            cbxLivresPublics.Text = "";
            txbLivresPublic.Text = "";
            cbxLivresRayons.Text = "";
            txbLivresRayon.Text = "";
        }
        //retourne l'ID du genre selectionné dans la cbxGenre 
        private string GetIdGenre(string unGenre)
        {
            List<Categorie> uneListe = controller.GetAllGenres();
            foreach (Categorie uneCategorie in uneListe)
            {
                if (uneCategorie.Libelle == unGenre)
                {
                    return uneCategorie.Id;
                }
            }
            return null;
        }

        //retourne l'id du public selectionné dans la cbxPublic
        private string GetIdPublic(string unPublic)
        {
            List<Categorie> uneListe = controller.GetAllPublics();
            foreach (Categorie uneCategorie in uneListe)
            {
                if (uneCategorie.Libelle == unPublic)
                {
                    return uneCategorie.Id;
                }
            }
            return null;
        }

        //retourne l'id du public selectionné dans la cbxPublic
        private string GetIdRayon(string unRayon)
        {
            List<Categorie> uneListe = controller.GetAllRayons();
            foreach (Categorie uneCategorie in uneListe)
            {
                if (uneCategorie.Libelle == unRayon)
                {
                    return uneCategorie.Id;
                }
            }
            return null;
        }
    }
}
