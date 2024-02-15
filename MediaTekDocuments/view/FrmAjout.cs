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
        private List<Livre> listeLivre = null;  //besoin pour le test des id d'un livre
        private Livre livreModif = null;        //besoin pour la modification d'un livre
        private List<Dvd> listeDvd = null;      //besoin pour le test des id d'un dvd
        private Dvd dvdModif = null;            //besoin pour la modification d'un dvd
        private string quelEnvoi;


        public FrmAjout(BindingSource bdgGenres2, BindingSource bdgPublics, BindingSource bdgRayons, bool affichage, string onglet, List<Object> lesListes = null, Object aModifier = null)
        {
            InitializeComponent();
            RemplirCbx(bdgGenres2, cbxGenres);
            RemplirCbx(bdgPublics, cbxPublics);
            RemplirCbx(bdgRayons, cbxRayons);
            Affichage(affichage, onglet);
            ChargeListe(onglet, lesListes);
            ChargeObjet(onglet, aModifier);
            quelEnvoi = onglet;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            controller = new FrmMediatekController(); //Le controleur ne fonctionne pas sans!!
        }

        //Converti la liste d'objet reçut en list de livre ou dvd
        private void ChargeListe(string onglet, List<Object> uneListe)
        {
            switch (onglet)
            {
                case "livre":
                    listeLivre = uneListe.ConvertAll(Object => (Livre)Object);
                    break;
                case "dvd":
                    listeDvd = uneListe.ConvertAll(Object => (Dvd)Object);
                    break;
            }
        }
        //remplit la comboBox reçut en paramètre en fonction de la bdg reçut en paramètre
        private void RemplirCbx(BindingSource bdg, ComboBox cbx)
        {
            cbx.DataSource = bdg;
            if (cbx.Items.Count > 0)
            {
                cbx.SelectedIndex = -1;
            }
        }
        //Gestion de l'affichage des txtBox genre public rayon
        private void Affichage(bool affichage, string onglet)
        {
            btnAjouter.Enabled = !affichage;
            txbNumero.Enabled = !affichage;
            btnModifier.Enabled = affichage;
            txbGenre.Enabled = false;
            txbPublic.Enabled = false;
            txbRayon.Enabled = false;


            switch (onglet)
            {
                case "livre":
                    Console.WriteLine("Affichage cas livre");
                    lblAuteurRealisateurPer.Text = "Auteur * :";
                    lblCollectionSynopsisDel.Text = "Collection * :";
                    lblIsbnDuree.Text = "ISBN * :";
                    txbIsbnDuree.Enabled = true;
                    break;
                case "dvd":
                    Console.WriteLine("Affichage cas dvd");
                    lblAuteurRealisateurPer.Text = "Realisateur * :";
                    lblCollectionSynopsisDel.Text = "Synopsi * :";
                    lblIsbnDuree.Text = "Durée * :";
                    txbIsbnDuree.Enabled = true;
                    break;
                case "revu":
                    break;
            }
        }
        //methode pour la modification d'un livre ou un dvd
        private void ChargeObjet(string onglet, Object aModifier)
        {
            switch (onglet)
            {
                case "livre":
                    livreModif = (Livre)aModifier;
                    if (livreModif != null)
                    {
                        txbNumero.Text = livreModif.Id;
                        txbTitre.Text = livreModif.Titre;
                        txbImage.Text = livreModif.Image;
                        txbIsbnDuree.Text = livreModif.Isbn;
                        txbAuteurRealisateurPer.Text = livreModif.Auteur;
                        txbCollectionSynopsisDel.Text = livreModif.Collection;
                        txbGenre.Text = livreModif.Genre;
                        txbPublic.Text = livreModif.Public;
                        txbRayon.Text = livreModif.Rayon;
                    }
                    break;
                case "dvd":
                    dvdModif = (Dvd)aModifier;
                    if (aModifier != null)
                    {
                        txbNumero.Text = dvdModif.Id;
                        txbTitre.Text = dvdModif.Titre;
                        txbImage.Text = dvdModif.Image;
                        txbIsbnDuree.Text = dvdModif.Duree.ToString();
                        txbAuteurRealisateurPer.Text = dvdModif.Realisateur;
                        txbCollectionSynopsisDel.Text = dvdModif.Realisateur;
                        txbGenre.Text = dvdModif.Genre;
                        txbPublic.Text = dvdModif.Public;
                        txbRayon.Text = dvdModif.Rayon;
                    }
                    break;
            }
        }
        //Metohde pour vider les txtBox et combo
        private void Vide()
        {
            txbNumero.Text = "";
            txbTitre.Text = "";
            txbImage.Text = "";
            txbIsbnDuree.Text = "";
            txbAuteurRealisateurPer.Text = "";
            txbCollectionSynopsisDel.Text = "";
            cbxGenres.Text = "";
            txbGenre.Text = "";
            cbxPublics.Text = "";
            txbPublic.Text = "";
            cbxRayons.Text = "";
            txbRayon.Text = "";
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
        //action du bouton annuler.
        //ferme la fenêtre et retour à la précédente
        private void btnAnnuler_Click(object sender, EventArgs e)
        {
            FrmMediatek frmMediatek = new FrmMediatek();
            frmMediatek.Show();
            this.Hide();
        }
        //action du btn Ajouter
        //ajout d'un livre ou dvd dans la bdd
        private void btnAjouter_Click(object sender, EventArgs e)
        {
            switch (quelEnvoi)
            {
                case "livre":
                    SuperLivre();
                    break;
                case "dvd":
                    SuperDvd();
                    break;
            }
        }
        //Action du btn modifier 
        private void btnModifier_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Modifier ?", "Confirmer", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                switch (quelEnvoi)
                {
                    case "livre":
                        Livre livre = ValoriseLivre();
                        controller.ModifLivre(livre);
                        btnAnnuler_Click(null, null);
                        break;
                    case "dvd":
                        Dvd dvd = ValoriseDvd();
                        controller.ModifierDvd(dvd);
                        btnAnnuler_Click(null, null);
                        break;
                } 
            }
        }
        //Methode pour la création/modification d'un livre
        private Livre ValoriseLivre()
        {
            //valorise tout les paramètres
            string id = txbNumero.Text;
            string titre = txbTitre.Text;
            string image = txbImage.Text;
            string isbn = txbIsbnDuree.Text;
            string auteur = txbAuteurRealisateurPer.Text;
            string collection = txbCollectionSynopsisDel.Text;
            string idGenre = GetIdGenre(cbxGenres.Text);
            string genre = txbGenre.Text; 
            string idPublic = GetIdPublic(cbxPublics.Text);
            string Public = txbPublic.Text;
            string idRayon = GetIdRayon(cbxRayons.Text);
            string rayon = txbRayon.Text;
            //cas d'une modification de livre
            // les id pas valorisés ne sont pas des chaines vides mais null (rappel)
            if (livreModif != null)
            {
                //si le Genre n'est pas modifier
                if (idGenre == null)
                {
                    idGenre = GetIdGenre(txbGenre.Text);
                }
                //si le public n'est pas modifier
                if (idPublic == null)
                {
                    idPublic = GetIdPublic(txbPublic.Text);
                }
                //si le rayon n'est pas modifier
                if (idRayon == null)
                {
                    idRayon = GetIdRayon(txbRayon.Text);
                }
            }
            Livre livreValorise = new Livre(id, titre, image, isbn, auteur, collection, idGenre, genre = null, idPublic, Public = null, idRayon, rayon = null);
            return livreValorise;
        }
        //methode pour l'envoi d'un livre
        private void SuperLivre()
        {
            Livre livre = ValoriseLivre();
            //controle les comboBox avant appel du constructeur
            if (cbxGenres.Text != "" && cbxPublics.Text != "" && cbxRayons.Text != "")
            {
                //controle les txtBox avant appel du constructeur
                if (txbTitre.Text != "" && txbIsbnDuree.Text != "" && txbAuteurRealisateurPer.Text != "" && txbCollectionSynopsisDel.Text != "" && txbNumero.Text != "")
                {
                    //controle que l'id n'est pas déja utilisé avant appel du constructeur
                    Livre testId = listeLivre.Find(x => x.Id.Equals(txbNumero.Text));
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
                else { MessageBox.Show("Entrez un id, un titre, un auteur, un ISBN et une collection."); }
            }
            else { MessageBox.Show("Sélectionnez un genre, un public et un rayon."); }
        }
        //Methode pour la création/modification d'un dvd
        private Dvd ValoriseDvd()
        {
                //valorise tout les paramètres
                string id = txbNumero.Text;
                string titre = txbTitre.Text;
                string image = txbImage.Text;
                int duree = int.Parse(txbIsbnDuree.Text);
                string realisateur = txbAuteurRealisateurPer.Text;
                string synopsis = txbCollectionSynopsisDel.Text;
                string idGenre = GetIdGenre(cbxGenres.Text);
                string genre = txbGenre.Text; //pas utilise pour créer un livre car selectionné dans la combo
                string idPublic = GetIdPublic(cbxPublics.Text);
                string Public = txbPublic.Text;//pas utilise pour créer un livre car selectionné dans la combo
                string idRayon = GetIdRayon(cbxRayons.Text);
                string rayon = txbRayon.Text;//pas utilise pour créer un livre car selectionné dans la combo
                //cas d'une modification d'un dvd
                if (dvdModif != null)
                {
                    //si le Genre n'est pas modifier
                    if (idGenre == null)
                    {
                        idGenre = GetIdGenre(txbGenre.Text);
                    }
                    //si le public n'est pas modifier
                    if (idPublic == null)
                    {
                        idPublic = GetIdPublic(txbPublic.Text);
                    }
                    //si le rayon n'est pas modifier
                    if (idRayon == null)
                    {
                        idRayon = GetIdRayon(txbRayon.Text);
                    }
                }
                Dvd dvdValorise = new Dvd(id, titre, image, duree, realisateur, synopsis, idGenre, genre = null, idPublic, Public = null, idRayon, rayon = null);
                return dvdValorise;
        }
        //methode pour l'envoi d'un dvd
        private void SuperDvd()
        {
            Dvd dvd = ValoriseDvd();
            //controle les comboBox avant appel du constructeur
            if (cbxGenres.Text != "" && cbxPublics.Text != "" && cbxRayons.Text != "")
            {
                //controle les txtBox avant appel du constructeur
                if (txbTitre.Text != "" && txbIsbnDuree.Text != "" && txbAuteurRealisateurPer.Text != "" && txbCollectionSynopsisDel.Text != "" && txbNumero.Text != "")
                {
                    //controle que l'id n'est pas déja utilisé avant appel du constructeur
                    Dvd testId = listeDvd.Find(x => x.Id.Equals(txbNumero.Text));
                    if (testId != null)
                    {
                        MessageBox.Show("Cet id est déja utilisé.");
                    }
                    else
                    {
                        controller.EnvoiDvd(dvd);
                        Vide();
                    }
                }
                else { MessageBox.Show("Entrez un id, un titre, un réalisateur, une duréer et une synopsis."); }
            }
            else { MessageBox.Show("Sélectionnez un genre, un public et un rayon."); }
        }
    }
}
