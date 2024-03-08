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
    /// <summary>
    /// Classe  d'ajout / modification d'un livre ou dvd
    /// </summary>
    public partial class FrmAjout : Form
    {
        private FrmMediatekController controller;
        private string quelEnvoi;
        private List<Livre> listeLivre;
        /// <summary>
        /// besoin pour le test des id d'un livre.
        /// </summary>
        private Livre livreModif = null;
        /// <summary>
        /// besoin pour la modification d'un livre.
        /// </summary>
        private List<Dvd> listeDvd;
        /// <summary>
        /// besoin pour le test des id d'un dvd.
        /// </summary>
        private Dvd dvdModif = null;
        /// <summary>
        /// besoin pour la modification d'un dvd.
        /// </summary>
        private List<Revue> listeRevue = null;  
        /// <summary>
        /// besoin pour la modification d'une revue.
        /// </summary>
        private Revue revueModif = null;


        /// <summary>
        /// Constructeur de la classe
        /// </summary>
        /// <param name="bdgGenres2">binding source qui contient les genres</param>
        /// <param name="bdgPublics">binding source qui contient les publics</param>
        /// <param name="bdgRayons">binding source qui contient les rayons</param>
        /// <param name="affichage">booléen qui va influencer l'affichage</param>
        /// <param name="onglet">chaine de caractère qui va influencer les appels des controleurs</param>
        /// <param name="lesListes">Liste qui contient des dvd ou des livres</param>
        /// <param name="aModifier">contient l'objet a modifier en cas de modification</param>
        public FrmAjout(BindingSource bdgGenres2, BindingSource bdgPublics, BindingSource bdgRayons, bool affichage, string onglet,
            List<Object> lesListes = null, Object aModifier = null)
        {
            InitializeComponent();
            //Remplit les ComboBox qui contiennent les Genres, Publics et Rayons.
            RemplirCbx(bdgGenres2, cbxGenres);
            RemplirCbx(bdgPublics, cbxPublics);
            RemplirCbx(bdgRayons, cbxRayons);
            //Gestion des affichages des TextBox et label
            Affichage(onglet, affichage);
            //Convertit la liste d'objets reçue en liste de livres, de DVDs ou de revues.
            ChargeListe(onglet, lesListes);
            //Méthode pour la modification d'un livre ou un dvd
            ChargeObjet(onglet, aModifier);
            //Affecte le comportement pour l'appel du contrôleur
            quelEnvoi = onglet;
        }
        /// <summary>
        /// appel du controleur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            controller = new FrmMediatekController(); //Le controleur ne fonctionne pas sans!!
        }

        /// <summary>
        /// Convertit la liste d'objets reçue en liste de livres, de DVD ou de revues.
        /// </summary>
        /// <param name="onglet">présice si est avec un livre, dvd ou revue</param>
        /// <param name="uneListe">Liste qui contient des livres, dvd ou revue</param>
        private void ChargeListe(string onglet, List<Object> uneListe)
        {
            switch (onglet)
            {
                case "livre":
                    //Convertit la liste d'objets en livres
                    listeLivre = uneListe.ConvertAll(Object => (Livre)Object);
                    break;
                case "dvd":
                    listeDvd = uneListe.ConvertAll(Object => (Dvd)Object);
                    break;
                case "revue":
                    listeRevue = uneListe.ConvertAll(Object => (Revue)Object);
                    break;
            }
        }
        /// <summary>
        /// Remplit la comboBox reçue en paramètre en fonction de la bdg reçue en paramètre
        /// </summary>
        /// <param name="bdg">binding source en paramètre</param>
        /// <param name="cbx">combobox a remplir</param>
        private void RemplirCbx(BindingSource bdg, ComboBox cbx)
        {
            cbx.DataSource = bdg;
            if (cbx.Items.Count > 0)
            {
                cbx.SelectedIndex = -1;
            }
        }
        /// <summary>
        /// Gestion des affichages des TextBox et label
        /// </summary>
        /// <param name="onglet">présice si est avec un livre, dvd ou revue</param>
        /// <param name="affichage">modifie l'affichage des textbox et buttons</param>
        private void Affichage(string onglet, bool affichage)
        {
            //Adapte le comportement des boutons
            btnAjouter.Enabled = !affichage;
            txbNumero.Enabled = !affichage;
            btnModifier.Enabled = affichage;
            txbGenre.Enabled = false;
            txbPublic.Enabled = false;
            txbRayon.Enabled = false;
            //Adapte les label
            switch (onglet)
            {
                case "livre":
                    lblAuteurRealisateurPer.Text = "Auteur * :";
                    lblCollectionSynopsisDel.Text = "Collection * :";
                    lblIsbnDuree.Text = "ISBN * :";
                    txbIsbnDuree.Enabled = true;
                    break;
                case "dvd":
                    lblAuteurRealisateurPer.Text = "Realisateur * :";
                    lblCollectionSynopsisDel.Text = "Synopsi * :";
                    lblIsbnDuree.Text = "Durée * :";
                    txbIsbnDuree.Enabled = true;
                    break;
                case "revue":
                    lblAuteurRealisateurPer.Text = "Périodicité * :";
                    lblCollectionSynopsisDel.Text = "Délai mise à dispo * :";
                    lblIsbnDuree.Text = "";
                    txbIsbnDuree.Enabled = false; // ne pas afficher pour les revues
                    break;
            }
        }
        /// <summary>
        /// Méthode pour la modification d'un livre ou un dvd
        /// </summary>
        /// <param name="onglet">présice si est avec un livre, dvd ou revue</param>
        /// <param name="aModifier">précise si on est en modification ou création</param>
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
                case "revue":
                    revueModif = (Revue)aModifier;
                    if (aModifier != null)
                    {
                        txbNumero.Text = revueModif.Id;
                        txbTitre.Text = revueModif.Titre;
                        txbImage.Text = revueModif.Image;
                        txbAuteurRealisateurPer.Text = revueModif.Periodicite;
                        txbCollectionSynopsisDel.Text = revueModif.DelaiMiseADispo.ToString();
                        txbGenre.Text = revueModif.Genre;
                        txbPublic.Text = revueModif.Public;
                        txbRayon.Text = revueModif.Rayon;

                    }
                    break;
            }
        }
        /// <summary>
        /// Metohde pour vider les TextBox et ComboBox
        /// </summary>
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
        /// <summary>
        /// Retourne l'ID du genre selectionné dans la cbxGenre 
        /// </summary>
        /// <param name="unGenre">le genre</param>
        /// <returns>Retourne l'objet categorie</returns>
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
        /// <summary>
        /// Retourne l'ID du publix selectionné dans la cbx public
        /// </summary>
        /// <param name="unPublic">le public</param>
        /// <returns>Retourne l'objet categorie</returns>
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
        /// <summary>
        /// Retourne l'id du public selectionné dans la cbxPublic
        /// </summary>
        /// <param name="unRayon">le rayon</param>
        /// <returns>Retourne l'objet categorie</returns>
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
        /// <summary>
        /// Retourne l'id au format 5 digits 1 =>00001
        /// </summary>
        /// <param name="id">l'id a formater</param>
        /// <returns> Retourne l'id au format 5 digits</returns>
        private string FormaterId(string id)
        {
            int idFormate = int.Parse(id);
            return idFormate.ToString("D5");
        }
        /// <summary>
        /// Action du bouton Annuler. Ferme la fênêtre.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnnuler_Click(object sender, EventArgs e)
        {
            FrmMediatek frmMediatek = new FrmMediatek();
            frmMediatek.Show();
            this.Hide();
        }
        /// <summary>
        /// /Action du bouton Ajouter. Envoi un livre, un dvd ou une revue dans la bdd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAjouter_Click(object sender, EventArgs e)
        {
            switch (quelEnvoi)
            {
                case "livre":
                    Livre livre = SuperLivre(); 
                    if (livre != null)
                    {
                        if (controller.EnvoiDocuments(livre)) // l'api return true si l'ajout a fonctionné
                        {
                            MessageBox.Show("Livre ajouté");
                            Vide();
                        }
                    }
                    break;

                case "dvd":
                    Dvd dvd = SuperDvd();
                    if (dvd != null)
                    {
                        if (controller.EnvoiDocuments(dvd))
                        {
                            MessageBox.Show("DVD ajouté");
                            Vide();
                        }
                    }
                    break;

                case "revue":
                    Revue revue = SuperRevue();
                    if (revue != null)
                    {
                        if (controller.EnvoiDocuments(revue))
                        {
                            MessageBox.Show("Revue ajouté");
                            Vide();
                        }
                    }
                    break;
            }
        }
        /// <summary>
        /// Action du bouton Modifier
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifier_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Modifier ?", "Confirmer", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                switch (quelEnvoi)
                {
                    case "livre":
                        Livre livre = SuperLivre();
                        if (livre != null)
                        {
                            if (controller.ModifierDocuments(livre))
                            {
                                MessageBox.Show("Livre modifié");
                            }
                        }
                        break;
                    case "dvd":
                        Dvd dvd = SuperDvd();
                        if (dvd != null)
                        {
                            if (controller.ModifierDocuments(dvd))
                            {
                                MessageBox.Show("DVD modifié");
                            }
                        }
                        break;
                    case "revue":
                        Revue revue = SuperRevue();
                        if (revue != null)
                        {
                            if (controller.ModifierDocuments(revue))
                            {
                                MessageBox.Show("Revue modifié");
                            }
                        }
                        break;
                }
            }
        }
        /// <summary>
        /// Methode pour la création/modification d'un livre
        /// </summary>
        /// <returns>L'objet Livre</returns>
        private Livre ValoriseLivre()
        {
            try
            {
                //valorise tout les paramètres
                string id = FormaterId(txbNumero.Text);
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
            catch (Exception ex) { return null; }
        }
        /// <summary>
        /// Contrôle des valeurs entrées
        /// </summary>
        /// <returns>Retourne l'objet livre</returns>
        private Livre SuperLivre()
        {
            try
            {
                Livre livre = ValoriseLivre();
                //controle les ComboBox sauf si modification
                if (cbxGenres.Text != "" && cbxPublics.Text != "" && cbxRayons.Text != "" || livreModif != null)
                {
                    //controle les TextBox 
                    if (txbTitre.Text != "" && txbIsbnDuree.Text != "" && txbAuteurRealisateurPer.Text != "" && txbCollectionSynopsisDel.Text != "" && txbNumero.Text != "")
                    {
                        Livre testId = listeLivre.Find(x => x.Id.Equals(txbNumero.Text));
                        //controle que l'id soit disponible sauf si modification
                        if (testId == null || livreModif != null)
                        {
                            //controle la valeur de l'ID
                            if ((int.Parse(txbNumero.Text)) > 0 && (int.Parse(txbNumero.Text)) < 2000)
                            {
                                //contrôle le format de l'ISBN 
                                if (txbIsbnDuree.Text.Length == 13)
                                {
                                    return livre;
                                }
                                else { MessageBox.Show("ISBN incorrecte. (13 chiffres)"); return null; }
                            }
                            else { MessageBox.Show("Id incorrecte. (compris entre 00001 et 19999)"); return null; }
                        }
                        else { MessageBox.Show("Cet id est déja utilisé."); return null; }
                    }
                    else { MessageBox.Show("Entrez un id, un titre, un auteur, un ISBN et une collection."); return null; }
                }
                else { MessageBox.Show("Sélectionnez un genre, un public et un rayon."); return null; }
            }
            catch (Exception ex) { return null; }
        }
        /// <summary>
        /// Methode pour la création/modification d'un dvd
        /// </summary>
        /// <returns>Retourne l'objet dvd</returns>
        private Dvd ValoriseDvd()
        {
            try // nécessaire  si txbIsbnDuree est null, le Parse fait planter.
            {
                //valorise tout les paramètres
                string id = txbNumero.Text;
                string titre = txbTitre.Text;
                string image = txbImage.Text;
                int duree = int.Parse(txbIsbnDuree.Text);
                string realisateur = txbAuteurRealisateurPer.Text;
                string synopsis = txbCollectionSynopsisDel.Text;
                string idGenre = GetIdGenre(cbxGenres.Text);
                string genre = txbGenre.Text;
                string idPublic = GetIdPublic(cbxPublics.Text);
                string Public = txbPublic.Text;
                string idRayon = GetIdRayon(cbxRayons.Text);
                string rayon = txbRayon.Text;

                if (dvdModif != null) //cas d'une modification d'un dvd
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
            catch (Exception ex) { return null; }
        }
        /// <summary>
        /// Contrôle des valeurs entrées
        /// </summary>
        /// <returns>Retourne l'objet dvd</returns>
        private Dvd SuperDvd()
        {
            try
            {
                Dvd dvd = ValoriseDvd();
                //controle les ComboBox sauf si modification
                if (cbxGenres.Text != "" && cbxPublics.Text != "" && cbxRayons.Text != "" || dvdModif != null)
                {
                    //controle les TextBox avant appel du constructeur
                    if (txbTitre.Text != "" && txbIsbnDuree.Text != "" && txbAuteurRealisateurPer.Text != "" && txbCollectionSynopsisDel.Text != "" && txbNumero.Text != "")
                    {
                        //controle la valeur de l'ID
                        if ((int.Parse(txbNumero.Text)) > 19999 && (int.Parse(txbNumero.Text)) < 30000)
                        {
                            Dvd testId = listeDvd.Find(x => x.Id.Equals(txbNumero.Text));     
                            if (testId == null || dvdModif != null)
                            {
                                //controle la valeur de Durée
                                if (int.Parse(txbIsbnDuree.Text) > 1)
                                {
                                    return dvd;
                                }
                                else { MessageBox.Show("Valeur de durée incorrecte."); return null; }
                            }
                            else { MessageBox.Show("Cet Id est déja utilisé."); return null; }
                        }
                        else { MessageBox.Show("Id incorrecte. (compris entre 20000 et 30000)"); return null; }
                    }
                    else { MessageBox.Show("Entrez un id, un titre, un réalisateur, une duréer et une synopsis."); return null; }
                }
                else { MessageBox.Show("Sélectionnez un genre, un public et un rayon."); return null; }
            }
            catch (Exception ex) { return null; }
        }
        /// <summary>
        /// Methode pour la création/modification d'une revue
        /// </summary>
        /// <returns>Retourne l'objet revue</returns>
        private Revue ValoriseRevue()
        {
            try // nécessaire, si txbCollectionSynopsisDel est null, le Parse fait planter.
            {
                //valorise tout les paramètres
                string id = txbNumero.Text;
                string titre = txbTitre.Text;
                string image = txbImage.Text;
                string periodicite = txbAuteurRealisateurPer.Text;
                int delaiMiseADispo = int.Parse(txbCollectionSynopsisDel.Text);
                string idGenre = GetIdGenre(cbxGenres.Text);
                string genre = txbGenre.Text;
                string idPublic = GetIdPublic(cbxPublics.Text);
                string lePublic = txbPublic.Text;
                string idRayon = GetIdRayon(cbxRayons.Text);
                string rayon = txbRayon.Text;
                if (revueModif != null)
                {
                    //si le Genre n'est pas modifié
                    if (idGenre == null)
                    {
                        idGenre = GetIdGenre(txbGenre.Text);
                    }
                    //si le public n'est pas modifié
                    if (idPublic == null)
                    {
                        idPublic = GetIdPublic(txbPublic.Text);
                    }
                    //si le rayon n'est pas modifié
                    if (idRayon == null)
                    {
                        idRayon = GetIdRayon(txbRayon.Text);
                    }
                }
                Revue revueValorise = new Revue(id, titre, image, idGenre, genre = null, idPublic, lePublic = null, idRayon, rayon = null, periodicite, delaiMiseADispo);

                return revueValorise;
            }
            catch (Exception ex) { }
            return null;
        }
        /// <summary>
        /// Contrôle des valeurs entrées
        /// </summary>
        /// <returns>Retourne l'objet revue</returns>
        private Revue SuperRevue()
        {
            try
            {
                Revue revue = ValoriseRevue();
                //Controle les comboBox sauf si modification
                if (cbxGenres.Text != "" && cbxPublics.Text != "" && cbxRayons.Text != "" || revueModif != null)
                {
                    //Controle les txtBox avant appel du constructeur
                    if (txbTitre.Text != "" && txbAuteurRealisateurPer.Text != "" && txbCollectionSynopsisDel.Text != "" && txbNumero.Text != "")
                    {
                        if ((int.Parse(txbCollectionSynopsisDel.Text) > 1) && (int.Parse(txbCollectionSynopsisDel.Text) < 1000))
                        {
                            if ((int.Parse(txbNumero.Text)) > 10000 && (int.Parse(txbNumero.Text)) < 20000)
                            {
                                //Controle que l'id n'est pas déja utilisé avant appel du constructeur
                                Revue testId = listeRevue.Find(x => x.Id.Equals(txbNumero.Text));
                                if (testId == null || revueModif != null)
                                {

                                    return revue;
                                }
                                else { MessageBox.Show("Id déja utilisé"); return null; }
                            }
                            else { MessageBox.Show("Id incorrecte (entre 10 000 et 20 000)"); return null; }
                        }
                        else { MessageBox.Show("Delais incorrecte"); return null; }
                    }
                    else { MessageBox.Show("Entrez un id, un titre, une périodicité, un délai de mise à dispo"); return null; }
                }
                else { MessageBox.Show("Sélectionnez un genre, un public et un rayon."); return null; }
            }
            catch (Exception ex) { return null; }
        }
    }
}
