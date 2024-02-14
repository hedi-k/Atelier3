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
        private List<Livre> listeLivre = null; //besoin pour le test de l'id
        private Livre livreModif;
        private List<Dvd> listeDvd = null;
        private string quelEnvoi;
        //livreModif = null rend le paramètre optionnel

        public FrmAjout(BindingSource bdgGenres2, BindingSource bdgPublics, BindingSource bdgRayons, bool affichage, string onglet, List<Object> lesListes = null, Livre livreModif = null)
        {
            InitializeComponent();
            RemplirCbx(bdgGenres2, cbxGenres);
            RemplirCbx(bdgPublics, cbxPublics);
            RemplirCbx(bdgRayons, cbxRayons);
            Affichage(affichage, onglet);
            ChargeListe(onglet, lesListes);
            quelEnvoi = onglet;

            //listeLivre = lesListes.ConvertAll(Object => (Livre)Object);

            this.livreModif = livreModif;
            ModifierLivre(livreModif);
            //lesDvd = lesDvds;
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
            txbLivresGenre.Enabled = false;
            txbLivresPublic.Enabled = false;
            txbLivresRayon.Enabled = false;


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
                Livre livre = ValoriseLivre();
                controller.ModifLivre(livre);
                btnAnnuler_Click(null, null);
            }
        }
        //Metohde pour vider les txtBox et combo
        private void Vide()
        {
            txbNumero.Text = "";
            txbLivresTitre.Text = "";
            txbLivresImage.Text = "";
            txbIsbnDuree.Text = "";
            txbAuteurRealisateurPer.Text = "";
            txbCollectionSynopsisDel.Text = "";
            cbxGenres.Text = "";
            txbLivresGenre.Text = "";
            cbxPublics.Text = "";
            txbLivresPublic.Text = "";
            cbxRayons.Text = "";
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
        //Methode pour la création/modification d'un livre
        private Livre ValoriseLivre()
        {
            //valorise tout les paramètres
            string id = txbNumero.Text;
            string titre = txbLivresTitre.Text;
            string image = txbLivresImage.Text;
            string isbn = txbIsbnDuree.Text;
            string auteur = txbAuteurRealisateurPer.Text;
            string collection = txbCollectionSynopsisDel.Text;
            string idGenre = GetIdGenre(cbxGenres.Text);
            string genre = txbLivresGenre.Text; //pas utilise pour créer un livre car selectionné dans la combo
            string idPublic = GetIdPublic(cbxPublics.Text);
            string Public = txbLivresPublic.Text;//pas utilise pour créer un livre car selectionné dans la combo
            string idRayon = GetIdRayon(cbxRayons.Text);
            string rayon = txbLivresRayon.Text;//pas utilise pour créer un livre car selectionné dans la combo
            //cas d'une modification de livre
            // les id pas valorisés ne sont pas des chaines vides mais null (rappel)
            if (livreModif != null)
            {
                //si le Genre n'est pas modifier
                if (idGenre == null)
                {
                    idGenre = GetIdGenre(txbLivresGenre.Text);
                }
                //si le public n'est pas modifier
                if (idPublic == null)
                {
                    GetIdPublic(txbLivresTitre.Text);
                }
                //si le rayon n'est pas modifier
                if (idRayon == null)
                {
                    GetIdRayon(txbLivresRayon.Text);
                }
            }

            //création de l'objet livre
            //1er version
            //Livre livreValorise = new Livre(id, titre, image, isbn, auteur, collection, idGenre, genre, idPublic, Public, idRayon, rayon);
            //genre public rayon doivent être a null car ils sont envoyés à l'api  qui ne les attend pas.
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
                if (txbLivresTitre.Text != "" && txbIsbnDuree.Text != "" && txbAuteurRealisateurPer.Text != "" && txbCollectionSynopsisDel.Text != "" && txbNumero.Text != "")
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
        //methode pour la modification d'un livre
        private void ModifierLivre(Livre livreModif)
        {
            if (livreModif != null)
            {
                txbNumero.Text = livreModif.Id;
                txbLivresTitre.Text = livreModif.Titre;
                txbLivresImage.Text = livreModif.Image;
                txbIsbnDuree.Text = livreModif.Isbn;
                txbAuteurRealisateurPer.Text = livreModif.Auteur;
                txbCollectionSynopsisDel.Text = livreModif.Collection;
                txbLivresGenre.Text = livreModif.Genre;
                txbLivresPublic.Text = livreModif.Public;
                txbLivresRayon.Text = livreModif.Rayon;
            }
        }
        //Methode pour la création/modification d'un dvd
        private Dvd ValoriseDvd()
        {
            try
            {
                //valorise tout les paramètres
                string id = txbNumero.Text;
                string titre = txbLivresTitre.Text;
                string image = txbLivresImage.Text;
                int duree = int.Parse(txbIsbnDuree.Text);
                string realisateur = txbAuteurRealisateurPer.Text;
                string synopsis = txbCollectionSynopsisDel.Text;
                string idGenre = GetIdGenre(cbxGenres.Text);
                string genre = txbLivresGenre.Text; //pas utilise pour créer un livre car selectionné dans la combo
                string idPublic = GetIdPublic(cbxPublics.Text);
                string Public = txbLivresPublic.Text;//pas utilise pour créer un livre car selectionné dans la combo
                string idRayon = GetIdRayon(cbxRayons.Text);
                string rayon = txbLivresRayon.Text;//pas utilise pour créer un livre car selectionné dans la combo

                Dvd dvdValorise = new Dvd(id, titre, image, duree, realisateur, synopsis, idGenre, genre, idPublic, Public, idRayon, rayon);
                return dvdValorise;
            }
            catch { }
            return null;

        }
        //methode pour l'envoi d'un dvd
        private void SuperDvd()
        {
            Dvd dvd = ValoriseDvd();
            //controle les comboBox avant appel du constructeur
            if (cbxGenres.Text != "" && cbxPublics.Text != "" && cbxRayons.Text != "")
            {
                //controle les txtBox avant appel du constructeur
                if (txbLivresTitre.Text != "" && txbIsbnDuree.Text != "" && txbAuteurRealisateurPer.Text != "" && txbCollectionSynopsisDel.Text != "" && txbNumero.Text != "")
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
