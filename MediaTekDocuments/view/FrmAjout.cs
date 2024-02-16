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
        private string quelEnvoi;
        private List<Livre> listeLivre;  //besoin pour le test des id d'un livre
        private Livre livreModif = null;        //besoin pour la modification d'un livre
        private List<Dvd> listeDvd;      //besoin pour le test des id d'un dvd
        private Dvd dvdModif = null;            //besoin pour la modification d'un dvd
        private List<Revue> listeRevue = null;  //besoin pour le test des id d'une revue
        private Revue revueModif = null;        //besoin pour la modification d'une revue



        public FrmAjout(BindingSource bdgGenres2, BindingSource bdgPublics, BindingSource bdgRayons, bool affichage, string onglet,
            List<Object> lesListes = null, Object aModifier = null)
        {
            InitializeComponent();
            RemplirCbx(bdgGenres2, cbxGenres);
            RemplirCbx(bdgPublics, cbxPublics);
            RemplirCbx(bdgRayons, cbxRayons);
            Affichage(onglet, affichage);
            ChargeListe(onglet, lesListes);
            ChargeObjet(onglet, aModifier);
            quelEnvoi = onglet;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            controller = new FrmMediatekController(); //Le controleur ne fonctionne pas sans!!
        }

        //Converti la liste d'objet reçut en list de livre ou dvd ou revue
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
                case "revue":
                    listeRevue = uneListe.ConvertAll(Object => (Revue)Object);
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
        //Gestion de l'affichage des txtBox et labelles
        private void Affichage(string onglet, bool affichage)
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
        //retourne l'id au format 5 digits 1 =>00001
        private string FormaterId(string id)
        {
            int idFormate = int.Parse(id);
            return idFormate.ToString("D5");
        }
        //action du bouton annuler.
        //ferme la fenêtre et retour à la précédente
        private void btnAnnuler_Click(object sender, EventArgs e)
        {
            FrmMediatek frmMediatek = new FrmMediatek();
            frmMediatek.Show();
            this.Hide();
        }
        //action du btn Ajouter, envoi un livre, un dvd ou une revue dans la bdd
        private void btnAjouter_Click(object sender, EventArgs e)
        {
            switch (quelEnvoi)
            {
                case "livre":
                    Livre livre = SuperLivre();
                    if (livre != null)
                    {
                        if (controller.EnvoiLivre(livre)) // l'api return true si l'ajout a fonctionné
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
                        if (controller.EnvoiDvd(dvd))// l'api return true si l'ajout a fonctionné
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
                        if (controller.EnvoiRevue(revue))// l'api return true si l'ajout a fonctionné
                        {
                            MessageBox.Show("Revue ajouté");
                            Vide();
                        }
                    }
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
                        Livre livre = SuperLivre();
                        if (livre != null)
                        {
                            if (controller.ModifLivre(livre))// l'api return true si l'ajout a fonctionné
                            {
                                MessageBox.Show("Livre modifié");
                            }
                        }
                        break;
                    case "dvd":
                        Dvd dvd = SuperDvd();
                        if (dvd != null)
                        {
                            if (controller.ModifierDvd(dvd))
                            {
                                MessageBox.Show("DVD modifié");
                            }
                        }
                        break;
                    case "revue":
                        Revue revue = SuperRevue();
                        if (revue != null)
                        {
                            if (controller.ModifierRevue(revue))
                            {
                                MessageBox.Show("Revue modifié");
                            }
                        }
                        break;
                }
            }
        }
        //Methode pour la création/modification d'un livre
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
        //methode pour l'envoi d'un livre après contrôle des valeurs entrées
        private Livre SuperLivre()
        {
            try
            {
                Livre livre = ValoriseLivre();
                //controle les combox sauf si modification
                if (cbxGenres.Text != "" && cbxPublics.Text != "" && cbxRayons.Text != "" || livreModif != null)
                {
                    //controle les txtBox 
                    if (txbTitre.Text != "" && txbIsbnDuree.Text != "" && txbAuteurRealisateurPer.Text != "" && txbCollectionSynopsisDel.Text != "" && txbNumero.Text != "")
                    {
                        Livre testId = listeLivre.Find(x => x.Id.Equals(txbNumero.Text));
                        //controle que l'id soit disponible saufe si modification
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
            catch { return null; }
        }
        //Methode pour la création/modification d'un dvd
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
                string genre = txbGenre.Text; //pas utilise pour créer un livre car selectionné dans la combo
                string idPublic = GetIdPublic(cbxPublics.Text);
                string Public = txbPublic.Text;//pas utilise pour créer un livre car selectionné dans la combo
                string idRayon = GetIdRayon(cbxRayons.Text);
                string rayon = txbRayon.Text;//pas utilise pour créer un livre car selectionné dans la combo

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
        //methode pour l'envoi d'un dvd
        private Dvd SuperDvd()
        {
            try
            {
                Dvd dvd = ValoriseDvd();
                //controle les comboBox sauf si modification
                if (cbxGenres.Text != "" && cbxPublics.Text != "" && cbxRayons.Text != "" || dvdModif != null)
                {
                    //controle les txtBox avant appel du constructeur
                    if (txbTitre.Text != "" && txbIsbnDuree.Text != "" && txbAuteurRealisateurPer.Text != "" && txbCollectionSynopsisDel.Text != "" && txbNumero.Text != "")
                    {
                        //controle la valeur de l'ID
                        if ((int.Parse(txbNumero.Text)) > 19999 && (int.Parse(txbNumero.Text)) < 30000)
                        {
                            Dvd testId = listeDvd.Find(x => x.Id.Equals(txbNumero.Text)); //bug ici a chercher
                                                                                          //controle que l'id soit disponible sauf si modification
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
        //Methode pour la création/modification d'une revue
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
                Revue revueValorise = new Revue(id, titre, image, idGenre, genre = null, idPublic, lePublic = null, idRayon, rayon = null, periodicite, delaiMiseADispo);

                return revueValorise;
            }
            catch (Exception ex) { }
            return null;
        }
        //Methode pour l'envoi d'une revue
        private Revue SuperRevue()
        {
            try
            {
                Revue revue = ValoriseRevue();
                //controle les comboBox sauf si modification
                if (cbxGenres.Text != "" && cbxPublics.Text != "" && cbxRayons.Text != "" || revueModif != null)
                {
                    //controle les txtBox avant appel du constructeur
                    if (txbTitre.Text != "" && txbAuteurRealisateurPer.Text != "" && txbCollectionSynopsisDel.Text != "" && txbNumero.Text != "")
                    {
                        if ((int.Parse(txbCollectionSynopsisDel.Text) > 1) && (int.Parse(txbCollectionSynopsisDel.Text) < 1000))
                        {
                            if ((int.Parse(txbNumero.Text)) > 10000 && (int.Parse(txbNumero.Text)) < 20000)
                            {
                                //controle que l'id n'est pas déja utilisé avant appel du constructeur
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
