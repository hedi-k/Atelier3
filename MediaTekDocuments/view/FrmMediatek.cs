using System;
using System.Windows.Forms;
using MediaTekDocuments.model;
using MediaTekDocuments.controller;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;
using MediaTekDocuments.view;

namespace MediaTekDocuments.view

{
    /// <summary>
    /// Classe d'affichage
    /// </summary>
    public partial class FrmMediatek : Form
    {
        #region Commun
        private readonly FrmMediatekController controller;
        private readonly BindingSource bdgGenres = new BindingSource();
        private readonly BindingSource bdgPublics = new BindingSource();
        private readonly BindingSource bdgRayons = new BindingSource();
        //BidingSource ajouté pour la gestion des suivis
        private readonly BindingSource bdgSuivi = new BindingSource();
        //BidingSource ajouté pour la gestion des états
        private readonly BindingSource bdgEtats = new BindingSource();
        private List<Etat> lesEtats = new List<Etat>();

        //Remplace FrmMediatek mais necessaire pour récupérer l'utilisateur
        public FrmMediatek(Utilisateur utilisateur)
        {
            InitializeComponent();
            this.controller = new FrmMediatekController();
            Permission(utilisateur);

        }
        /// <summary>
        /// Constructeur : création du contrôleur lié à ce formulaire
        /// </summary>
        internal FrmMediatek()
        {
            InitializeComponent();
            this.controller = new FrmMediatekController();
            
        }

        /// <summary>
        /// Rempli un des 3 combo (genre, public, rayon)
        /// </summary>
        /// <param name="lesCategories">liste des objets de type Genre ou Public ou Rayon</param>
        /// <param name="bdg">bindingsource contenant les informations</param>
        /// <param name="cbx">combobox à remplir</param>
        public void RemplirComboCategorie(List<Categorie> lesCategories, BindingSource bdg, ComboBox cbx)
        {
            bdg.DataSource = lesCategories;
            cbx.DataSource = bdg;
            if (cbx.Items.Count > 0)
            {
                cbx.SelectedIndex = -1;
            }
        }
        //Commun aux à tout les documents
        public void RemplirComboEtat(List<Etat> lesEtats, BindingSource bdg, ComboBox cbx)
        {
            bdg.DataSource = lesEtats;
            cbx.DataSource = bdg;
            cbx.DisplayMember = "Libelle"; //Pour forcer l affichage des libelles sans modifier le model Etat
            if (cbx.Items.Count > 0)
            {
                cbx.SelectedIndex = -1;
            }
        }
        //Retourne l'ID de l etat selectionné dans les cbxEtat 
        private string GetIdEtat(string unEtat)
        {
            List<Etat> uneListe = controller.GetAllEtats();
            foreach (Etat Etat in uneListe)
            {
                if (Etat.Libelle == unEtat)
                {
                    return Etat.Id;
                }
            }
            return null;
        }
        #endregion

        #region Onglet Livres
        private readonly BindingSource bdgLivresListe = new BindingSource();
        private List<Livre> lesLivres = new List<Livre>();

        /// <summary>
        /// Ouverture de l'onglet Livres : 
        /// appel des méthodes pour remplir le datagrid des livres et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabLivres_Enter(object sender, EventArgs e)
        {
            lesLivres = controller.GetAllLivres();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxLivresGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxLivresPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxLivresRayons);
            RemplirLivresListeComplete();
            lesEtats = controller.GetAllEtats();
            PresChargeDGVExemplaireLivre();
            listCmdLivres = controller.GetAllCommandeLivres();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="livres">liste de livres</param>
        private void RemplirLivresListe(List<Livre> livres)
        {
            bdgLivresListe.DataSource = livres;
            dgvLivresListe.DataSource = bdgLivresListe;
            dgvLivresListe.Columns["isbn"].Visible = false;
            dgvLivresListe.Columns["idRayon"].Visible = false;
            dgvLivresListe.Columns["idGenre"].Visible = false;
            dgvLivresListe.Columns["idPublic"].Visible = false;
            dgvLivresListe.Columns["image"].Visible = false;
            dgvLivresListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvLivresListe.Columns["id"].DisplayIndex = 0;
            dgvLivresListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du livre dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbLivresNumRecherche.Text.Equals(""))
            {
                txbLivresTitreRecherche.Text = "";
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                Livre livre = lesLivres.Find(x => x.Id.Equals(txbLivresNumRecherche.Text));
                if (livre != null)
                {
                    List<Livre> livres = new List<Livre>() { livre };
                    RemplirLivresListe(livres);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirLivresListeComplete();
                }
            }
            else
            {
                RemplirLivresListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des livres dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbLivresTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbLivresTitreRecherche.Text.Equals(""))
            {
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                txbLivresNumRecherche.Text = "";
                List<Livre> lesLivresParTitre;
                lesLivresParTitre = lesLivres.FindAll(x => x.Titre.ToLower().Contains(txbLivresTitreRecherche.Text.ToLower()));
                RemplirLivresListe(lesLivresParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxLivresGenres.SelectedIndex < 0 && cbxLivresPublics.SelectedIndex < 0 && cbxLivresRayons.SelectedIndex < 0
                    && txbLivresNumRecherche.Text.Equals(""))
                {
                    RemplirLivresListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du livre sélectionné
        /// </summary>
        /// <param name="livre">le livre</param>
        private void AfficheLivresInfos(Livre livre)
        {
            txbLivresAuteur.Text = livre.Auteur;
            txbLivresCollection.Text = livre.Collection;
            txbLivresImage.Text = livre.Image;
            txbLivresIsbn.Text = livre.Isbn;
            txbLivresNumero.Text = livre.Id;
            txbLivresGenre.Text = livre.Genre;
            txbLivresPublic.Text = livre.Public;
            txbLivresRayon.Text = livre.Rayon;
            txbLivresTitre.Text = livre.Titre;
            string image = livre.Image;
            try
            {
                pcbLivresImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbLivresImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du livre
        /// </summary>
        private void VideLivresInfos()
        {
            txbLivresAuteur.Text = "";
            txbLivresCollection.Text = "";
            txbLivresImage.Text = "";
            txbLivresIsbn.Text = "";
            txbLivresNumero.Text = "";
            txbLivresGenre.Text = "";
            txbLivresPublic.Text = "";
            txbLivresRayon.Text = "";
            txbLivresTitre.Text = "";
            pcbLivresImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresGenres.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Genre genre = (Genre)cbxLivresGenres.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresPublics.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Public lePublic = (Public)cbxLivresPublics.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresRayons.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxLivresRayons.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirLivresListe(livres);
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_SelectionChanged(object sender, EventArgs e)
        {

            if (dgvLivresListe.CurrentCell != null)
            {
                try
                {
                    Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];
                    AfficheLivresInfos(livre);
                }
                catch
                {
                    VideLivresZones();
                }
            }
            else
            {
                VideLivresInfos();
            }
            PresChargeDGVExemplaireLivre();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des livres
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirLivresListeComplete()
        {
            RemplirLivresListe(lesLivres);
            VideLivresZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideLivresZones()
        {
            cbxLivresGenres.SelectedIndex = -1;
            cbxLivresRayons.SelectedIndex = -1;
            cbxLivresPublics.SelectedIndex = -1;
            txbLivresNumRecherche.Text = "";
            txbLivresTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideLivresZones();
            string titreColonne = dgvLivresListe.Columns[e.ColumnIndex].HeaderText;
            List<Livre> sortedList = new List<Livre>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesLivres.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesLivres.OrderBy(o => o.Titre).ToList();
                    break;
                case "Collection":
                    sortedList = lesLivres.OrderBy(o => o.Collection).ToList();
                    break;
                case "Auteur":
                    sortedList = lesLivres.OrderBy(o => o.Auteur).ToList();
                    break;
                case "Genre":
                    sortedList = lesLivres.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesLivres.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesLivres.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirLivresListe(sortedList);
        }
        //Rempli la combo charge la liste des exmplaire et appel pour la dgv.
        private void PresChargeDGVExemplaireLivre()
        {
            if (txbLivresNumero.Text != null)
            {
                lesExemplaires = controller.GetExemplairesRevue(txbLivresNumero.Text);
                RemplirComboEtat(lesEtats, bdgEtats, cbxLivreEtat);
                ChargeDGVExemplaireLIvre(lesExemplaires, lesEtats);
            }
        }

        //Rempli la dgv des exemplaire du livre selectionné dans l onglet livre
        private void ChargeDGVExemplaireLIvre(List<Exemplaire> exemplaires, List<Etat> lesEtats)
        {
            if (exemplaires != null)
            {
                foreach (var exemplaire in exemplaires)
                {
                    // Rechercher le libellé correspondant à l'idEtat de l'exemplaire
                    var etat = lesEtats.FirstOrDefault(e => e.Id == exemplaire.IdEtat);
                    // Si un état correspondant est trouvé, remplacer l'idEtat par le libellé
                    if (etat != null)
                    {
                        exemplaire.IdEtat = etat.Libelle;
                    }
                }
                bdgExemplairesListe.DataSource = exemplaires;
                dgvLivreExemplaire.DataSource = bdgExemplairesListe;
                dgvLivreExemplaire.Columns["photo"].Visible = false;
                dgvLivreExemplaire.Columns["id"].Visible = false;
                dgvLivreExemplaire.Columns[0].HeaderText = "n° Exemplaire";
                dgvLivreExemplaire.Columns[3].HeaderText = "Etat";
            }
        }
        //trie sur la DGVExemplaire
        private void dgvLivreExemplaire_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            List<Exemplaire> sortedList = new List<Exemplaire>();
            string titreColonne = dgvLivreExemplaire.Columns[e.ColumnIndex].HeaderText;
            switch (titreColonne)
            {
                case "n° Exemplaire":
                    sortedList = lesExemplaires.OrderBy(o => o.Id).ToList();
                    break;
                case "DateAchat":
                    sortedList = lesExemplaires.OrderBy(o => o.DateAchat).ToList();
                    break;
                case "Etat":
                    sortedList = lesExemplaires.OrderBy(o => o.IdEtat).ToList();
                    break;
            }
            ChargeDGVExemplaireLIvre(sortedList, lesEtats);
        }

        //Action du btn modifier pour l'etat d'un exemplaire
        private void btnLivreExemplaireModifier_Click(object sender, EventArgs e)
        {
            if (dgvLivreExemplaire.CurrentCell != null && MessageBox.Show("Modifier ?", "Confirmer", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                //Récupère dans la variable les données de l'objet sélectionné dans la dgv
                DataGridViewRow row = dgvLivreExemplaire.SelectedRows[0];
                //DataBoundItem permet l'accès aux données de l'objet
                Exemplaire exemplaire = row.DataBoundItem as Exemplaire;
                exemplaire.IdEtat = GetIdEtat(cbxLivreEtat.Text);
                if (exemplaire.IdEtat != null)
                {
                    if (controller.ModifierDocuments(exemplaire))
                    {
                        PresChargeDGVExemplaireLivre();
                    }
                }
                else { MessageBox.Show("Selectionner un Etat pour le modifier."); }
            }
        }
        //Action du btn supprimer pour un exemplaire
        private void btnLivreExemplaireSupprimer_Click(object sender, EventArgs e)
        {
            if (dgvLivreExemplaire.CurrentCell != null && MessageBox.Show("Supprimer ?", "Confirmer", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                //Récupère dans la variable les données de l'objet sélectionné dans la dgv
                DataGridViewRow row = dgvLivreExemplaire.SelectedRows[0];
                //DataBoundItem permet l'accès aux données de l'objet
                Exemplaire exemplaire = row.DataBoundItem as Exemplaire;
                if (exemplaire.Numero > -1)
                {
                    if (controller.SupprimerDocument(exemplaire))
                    {
                        PresChargeDGVExemplaireLivre();
                    }
                }
                else { MessageBox.Show("Selectionner un Exemplaire pour le supprimer."); }


            }
        }

        #endregion

        #region Onglet Dvd
        private readonly BindingSource bdgDvdListe = new BindingSource();
        private List<Dvd> lesDvd = new List<Dvd>();

        /// <summary>
        /// Ouverture de l'onglet Dvds : 
        /// appel des méthodes pour remplir le datagrid des dvd et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabDvd_Enter(object sender, EventArgs e)
        {
            lesDvd = controller.GetAllDvd();
            listCmdDvds = controller.GetAllCommandeDvds();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxDvdGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxDvdPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxDvdRayons);
            RemplirDvdListeComplete();
            PresChargeDGVExemplaireDvd();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="Dvds">liste de dvd</param>
        private void RemplirDvdListe(List<Dvd> Dvds)
        {
            bdgDvdListe.DataSource = Dvds;
            dgvDvdListe.DataSource = bdgDvdListe;
            dgvDvdListe.Columns["idRayon"].Visible = false;
            dgvDvdListe.Columns["idGenre"].Visible = false;
            dgvDvdListe.Columns["idPublic"].Visible = false;
            dgvDvdListe.Columns["image"].Visible = false;
            dgvDvdListe.Columns["synopsis"].Visible = false;
            dgvDvdListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvDvdListe.Columns["id"].DisplayIndex = 0;
            dgvDvdListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du Dvd dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbDvdNumRecherche.Text.Equals(""))
            {
                txbDvdTitreRecherche.Text = "";
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                Dvd dvd = lesDvd.Find(x => x.Id.Equals(txbDvdNumRecherche.Text));
                if (dvd != null)
                {
                    List<Dvd> Dvd = new List<Dvd>() { dvd };
                    RemplirDvdListe(Dvd);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirDvdListeComplete();
                }
            }
            else
            {
                RemplirDvdListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des Dvd dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbDvdTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbDvdTitreRecherche.Text.Equals(""))
            {
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                txbDvdNumRecherche.Text = "";
                List<Dvd> lesDvdParTitre;
                lesDvdParTitre = lesDvd.FindAll(x => x.Titre.ToLower().Contains(txbDvdTitreRecherche.Text.ToLower()));
                RemplirDvdListe(lesDvdParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxDvdGenres.SelectedIndex < 0 && cbxDvdPublics.SelectedIndex < 0 && cbxDvdRayons.SelectedIndex < 0
                    && txbDvdNumRecherche.Text.Equals(""))
                {
                    RemplirDvdListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du dvd sélectionné
        /// </summary>
        /// <param name="dvd">le dvd</param>
        private void AfficheDvdInfos(Dvd dvd)
        {
            txbDvdRealisateur.Text = dvd.Realisateur;
            txbDvdSynopsis.Text = dvd.Synopsis;
            txbDvdImage.Text = dvd.Image;
            txbDvdDuree.Text = dvd.Duree.ToString();
            txbDvdNumero.Text = dvd.Id;
            txbDvdGenre.Text = dvd.Genre;
            txbDvdPublic.Text = dvd.Public;
            txbDvdRayon.Text = dvd.Rayon;
            txbDvdTitre.Text = dvd.Titre;
            string image = dvd.Image;
            try
            {
                pcbDvdImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbDvdImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du dvd
        /// </summary>
        private void VideDvdInfos()
        {
            txbDvdRealisateur.Text = "";
            txbDvdSynopsis.Text = "";
            txbDvdImage.Text = "";
            txbDvdDuree.Text = "";
            txbDvdNumero.Text = "";
            txbDvdGenre.Text = "";
            txbDvdPublic.Text = "";
            txbDvdRayon.Text = "";
            txbDvdTitre.Text = "";
            pcbDvdImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdGenres.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Genre genre = (Genre)cbxDvdGenres.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdPublics.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Public lePublic = (Public)cbxDvdPublics.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdRayons.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxDvdRayons.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDvdListe.CurrentCell != null)
            {
                try
                {
                    Dvd dvd = (Dvd)bdgDvdListe.List[bdgDvdListe.Position];
                    AfficheDvdInfos(dvd);
                }
                catch
                {
                    VideDvdZones();
                }
            }
            else
            {
                VideDvdInfos();
            }
            PresChargeDGVExemplaireDvd();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des Dvd
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirDvdListeComplete()
        {
            RemplirDvdListe(lesDvd);
            VideDvdZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideDvdZones()
        {
            cbxDvdGenres.SelectedIndex = -1;
            cbxDvdRayons.SelectedIndex = -1;
            cbxDvdPublics.SelectedIndex = -1;
            txbDvdNumRecherche.Text = "";
            txbDvdTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideDvdZones();
            string titreColonne = dgvDvdListe.Columns[e.ColumnIndex].HeaderText;
            List<Dvd> sortedList = new List<Dvd>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesDvd.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesDvd.OrderBy(o => o.Titre).ToList();
                    break;
                case "Duree":
                    sortedList = lesDvd.OrderBy(o => o.Duree).ToList();
                    break;
                case "Realisateur":
                    sortedList = lesDvd.OrderBy(o => o.Realisateur).ToList();
                    break;
                case "Genre":
                    sortedList = lesDvd.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesDvd.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesDvd.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirDvdListe(sortedList);
        }

        private void PresChargeDGVExemplaireDvd()
        {
            if (txbDvdNumero.Text != null)
            {
                lesExemplaires = controller.GetExemplairesRevue(txbDvdNumero.Text);
                RemplirComboEtat(lesEtats, bdgEtats, cbxDvdEtat);
                ChargeDGVExemplaireDvd(lesExemplaires, lesEtats);
            }
        }

        //Rempli la dgv des exemplaire du dvd selectionné dans l onglet dvd
        private void ChargeDGVExemplaireDvd(List<Exemplaire> exemplaires, List<Etat> lesEtats)
        {
            if (exemplaires != null)
            {
                foreach (var exemplaire in exemplaires)
                {
                    // Rechercher le libellé correspondant à l'idEtat de l'exemplaire
                    var etat = lesEtats.FirstOrDefault(e => e.Id == exemplaire.IdEtat);
                    // Si un état correspondant est trouvé, remplacer l'idEtat par le libellé
                    if (etat != null)
                    {
                        exemplaire.IdEtat = etat.Libelle;
                    }
                }
                bdgExemplairesListe.DataSource = exemplaires;
                dgvDvdExemplaire.DataSource = bdgExemplairesListe;
                dgvDvdExemplaire.Columns["photo"].Visible = false;
                dgvDvdExemplaire.Columns["id"].Visible = false;
                dgvDvdExemplaire.Columns[0].HeaderText = "n° Exemplaire";
                dgvDvdExemplaire.Columns[3].HeaderText = "Etat";
            }
        }
        //trie sur la dgv exemplaire
        private void dgvDvdExemplaire_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            List<Exemplaire> sortedList = new List<Exemplaire>();
            string titreColonne = dgvLivreExemplaire.Columns[e.ColumnIndex].HeaderText;
            switch (titreColonne)
            {
                case "n° Exemplaire":
                    sortedList = lesExemplaires.OrderBy(o => o.Id).ToList();
                    break;
                case "DateAchat":
                    sortedList = lesExemplaires.OrderBy(o => o.DateAchat).ToList();
                    break;
                case "Etat":
                    sortedList = lesExemplaires.OrderBy(o => o.IdEtat).ToList();
                    break;
            }
            ChargeDGVExemplaireDvd(sortedList, lesEtats);
        }
        //Modifie l état d'un exemplaire
        private void btnDvdExemplaireModifier_Click(object sender, EventArgs e)
        {
            if (dgvDvdExemplaire.CurrentCell != null && MessageBox.Show("Modifier ?", "Confirmer", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DataGridViewRow row = dgvDvdExemplaire.SelectedRows[0];
                Exemplaire exemplaire = row.DataBoundItem as Exemplaire;
                exemplaire.IdEtat = GetIdEtat(cbxLivreEtat.Text);
                if (exemplaire.IdEtat != null)
                {
                    if (controller.ModifierDocuments(exemplaire))
                    {
                        PresChargeDGVExemplaireDvd();
                    }
                }
                else { MessageBox.Show("Selectionner un Etat pour le modifier."); }
            }
        }
        //Action du btn supprimer pour un exemplaire de dvd
        private void btnDvdExemplaireSupprimer_Click(object sender, EventArgs e)
        {
            if (dgvDvdExemplaire.CurrentCell != null)
            {
                if (MessageBox.Show("Supprimer ?", "Confirmer", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    DataGridViewRow row = dgvDvdExemplaire.SelectedRows[0];
                    Exemplaire exemplaire = row.DataBoundItem as Exemplaire;
                    if (exemplaire.Numero > -1)
                    {
                        if (controller.SupprimerDocument(exemplaire))
                        {
                            PresChargeDGVExemplaireDvd();
                        }
                    }
                    else { MessageBox.Show("Selectionner un Exemplaire pour le supprimer."); }
                }
            }
        }

        #endregion

        #region Onglet Revues
        private readonly BindingSource bdgRevuesListe = new BindingSource();
        private List<Revue> lesRevues = new List<Revue>();

        /// <summary>
        /// Ouverture de l'onglet Revues : 
        /// appel des méthodes pour remplir le datagrid des revues et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabRevues_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
            //nécessaire pour savoir si un abonnement est en cour sur une revue avant de l effacer.
            listCmdRevues = controller.GetAllCommandeRevues();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxRevuesGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxRevuesPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxRevuesRayons);
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="revues"></param>
        private void RemplirRevuesListe(List<Revue> revues)
        {
            bdgRevuesListe.DataSource = revues;
            dgvRevuesListe.DataSource = bdgRevuesListe;
            dgvRevuesListe.Columns["idRayon"].Visible = false;
            dgvRevuesListe.Columns["idGenre"].Visible = false;
            dgvRevuesListe.Columns["idPublic"].Visible = false;
            dgvRevuesListe.Columns["image"].Visible = false;
            dgvRevuesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvRevuesListe.Columns["id"].DisplayIndex = 0;
            dgvRevuesListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage de la revue dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbRevuesNumRecherche.Text.Equals(""))
            {
                txbRevuesTitreRecherche.Text = "";
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbRevuesNumRecherche.Text));
                if (revue != null)
                {
                    List<Revue> revues = new List<Revue>() { revue };
                    RemplirRevuesListe(revues);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirRevuesListeComplete();
                }
            }
            else
            {
                RemplirRevuesListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des revues dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbRevuesTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbRevuesTitreRecherche.Text.Equals(""))
            {
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                txbRevuesNumRecherche.Text = "";
                List<Revue> lesRevuesParTitre;
                lesRevuesParTitre = lesRevues.FindAll(x => x.Titre.ToLower().Contains(txbRevuesTitreRecherche.Text.ToLower()));
                RemplirRevuesListe(lesRevuesParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxRevuesGenres.SelectedIndex < 0 && cbxRevuesPublics.SelectedIndex < 0 && cbxRevuesRayons.SelectedIndex < 0
                    && txbRevuesNumRecherche.Text.Equals(""))
                {
                    RemplirRevuesListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionné
        /// </summary>
        /// <param name="revue">la revue</param>
        private void AfficheRevuesInfos(Revue revue)
        {
            txbRevuesPeriodicite.Text = revue.Periodicite;
            txbRevuesImage.Text = revue.Image;
            txbRevuesDateMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbRevuesNumero.Text = revue.Id;
            txbRevuesGenre.Text = revue.Genre;
            txbRevuesPublic.Text = revue.Public;
            txbRevuesRayon.Text = revue.Rayon;
            txbRevuesTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbRevuesImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbRevuesImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations de la reuve
        /// </summary>
        private void VideRevuesInfos()
        {
            txbRevuesPeriodicite.Text = "";
            txbRevuesImage.Text = "";
            txbRevuesDateMiseADispo.Text = "";
            txbRevuesNumero.Text = "";
            txbRevuesGenre.Text = "";
            txbRevuesPublic.Text = "";
            txbRevuesRayon.Text = "";
            txbRevuesTitre.Text = "";
            pcbRevuesImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesGenres.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Genre genre = (Genre)cbxRevuesGenres.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesPublics.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Public lePublic = (Public)cbxRevuesPublics.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesRayons.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxRevuesRayons.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations de la revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvRevuesListe.CurrentCell != null)
            {
                try
                {
                    Revue revue = (Revue)bdgRevuesListe.List[bdgRevuesListe.Position];
                    AfficheRevuesInfos(revue);
                }
                catch
                {
                    VideRevuesZones();
                }
            }
            else
            {
                VideRevuesInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des revues
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirRevuesListeComplete()
        {
            RemplirRevuesListe(lesRevues);
            VideRevuesZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideRevuesZones()
        {
            cbxRevuesGenres.SelectedIndex = -1;
            cbxRevuesRayons.SelectedIndex = -1;
            cbxRevuesPublics.SelectedIndex = -1;
            txbRevuesNumRecherche.Text = "";
            txbRevuesTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideRevuesZones();
            string titreColonne = dgvRevuesListe.Columns[e.ColumnIndex].HeaderText;
            List<Revue> sortedList = new List<Revue>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesRevues.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesRevues.OrderBy(o => o.Titre).ToList();
                    break;
                case "Periodicite":
                    sortedList = lesRevues.OrderBy(o => o.Periodicite).ToList();
                    break;
                case "DelaiMiseADispo":
                    sortedList = lesRevues.OrderBy(o => o.DelaiMiseADispo).ToList();
                    break;
                case "Genre":
                    sortedList = lesRevues.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesRevues.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesRevues.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirRevuesListe(sortedList);
        }

        //Action du bouton modifier un exemplaire pour une revue
        private void btnRevueExemplaireModifier_Click(object sender, EventArgs e)
        {
            if (dgvReceptionExemplairesListe.CurrentCell != null && MessageBox.Show("Modifier ?", "Confirmer", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DataGridViewRow row = dgvReceptionExemplairesListe.SelectedRows[0];
                Exemplaire exemplaire = row.DataBoundItem as Exemplaire;
                exemplaire.IdEtat = GetIdEtat(cbxRevuEtat.Text);
                if (exemplaire.IdEtat != null)
                {
                    if (controller.ModifierDocuments(exemplaire))
                    {
                        AfficheReceptionExemplairesRevue();
                    }
                }
                else { MessageBox.Show("Selectionner un Etat pour le modifier."); }
            }
        }
        //Action du btn supprimer pour un exemplaire pour une revue
        private void btnRevueExemplaireSupprimer_Click(object sender, EventArgs e)
        {
            if (dgvReceptionExemplairesListe.CurrentCell != null && MessageBox.Show("Supprimer ?", "Confirmer", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DataGridViewRow row = dgvReceptionExemplairesListe.SelectedRows[0];
                Exemplaire exemplaire = row.DataBoundItem as Exemplaire;
                if (exemplaire.Numero > -1)
                {
                    if (controller.SupprimerDocument(exemplaire))
                    {
                        AfficheReceptionExemplairesRevue();
                    }
                }
                else { MessageBox.Show("Selectionner un Exemplaire pour le supprimer."); }
            }
        }
        #endregion

        #region Onglet Paarutions
        private readonly BindingSource bdgExemplairesListe = new BindingSource();
        private List<Exemplaire> lesExemplaires = new List<Exemplaire>();
        const string ETATNEUF = "00001";

        /// <summary>
        /// Ouverture de l'onglet : récupère le revues et vide tous les champs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabReceptionRevue_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
            txbReceptionRevueNumero.Text = "";
            RemplirComboEtat(lesEtats, bdgEtats, cbxRevuEtat); // charge la combobox des exemplaire pour revue
        }

        /// <summary>
        /// Remplit le dategrid des exemplaires avec la liste reçue en paramètre
        /// </summary>
        /// <param name="exemplaires">liste d'exemplaires</param>
        private void RemplirReceptionExemplairesListe(List<Exemplaire> exemplaires, List<Etat> lesEtats)
        {
            if (exemplaires != null)
            {
                //Modification
                foreach (var exemplaire in exemplaires)
                {
                    // Rechercher le libellé correspondant à l'idEtat de l'exemplaire
                    var etat = lesEtats.FirstOrDefault(e => e.Id == exemplaire.IdEtat);
                    // Si un état correspondant est trouvé, remplacer l'idEtat par le libellé
                    if (etat != null)
                    {
                        exemplaire.IdEtat = etat.Libelle;
                    }
                }


                bdgExemplairesListe.DataSource = exemplaires;
                dgvReceptionExemplairesListe.DataSource = bdgExemplairesListe;
                dgvReceptionExemplairesListe.Columns["photo"].Visible = false;
                dgvReceptionExemplairesListe.Columns["id"].Visible = false;
                dgvReceptionExemplairesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvReceptionExemplairesListe.Columns["numero"].DisplayIndex = 0;
                dgvReceptionExemplairesListe.Columns["dateAchat"].DisplayIndex = 1;
                dgvReceptionExemplairesListe.Columns[3].HeaderText = "Etat";
            }
            else
            {
                bdgExemplairesListe.DataSource = null;
            }
        }

        /// <summary>
        /// Recherche d'un numéro de revue et affiche ses informations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionRechercher_Click(object sender, EventArgs e)
        {
            if (!txbReceptionRevueNumero.Text.Equals(""))
            {
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbReceptionRevueNumero.Text));
                if (revue != null)
                {
                    AfficheReceptionRevueInfos(revue);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                }
            }
        }

        /// <summary>
        /// Si le numéro de revue est modifié, la zone de l'exemplaire est vidée et inactive
        /// les informations de la revue son aussi effacées
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbReceptionRevueNumero_TextChanged(object sender, EventArgs e)
        {
            txbReceptionRevuePeriodicite.Text = "";
            txbReceptionRevueImage.Text = "";
            txbReceptionRevueDelaiMiseADispo.Text = "";
            txbReceptionRevueGenre.Text = "";
            txbReceptionRevuePublic.Text = "";
            txbReceptionRevueRayon.Text = "";
            txbReceptionRevueTitre.Text = "";
            pcbReceptionRevueImage.Image = null;
            RemplirReceptionExemplairesListe(null, null);
            AccesReceptionExemplaireGroupBox(false);
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionnée et les exemplaires
        /// </summary>
        /// <param name="revue">la revue</param>
        private void AfficheReceptionRevueInfos(Revue revue)
        {
            // informations sur la revue
            txbReceptionRevuePeriodicite.Text = revue.Periodicite;
            txbReceptionRevueImage.Text = revue.Image;
            txbReceptionRevueDelaiMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbReceptionRevueNumero.Text = revue.Id;
            txbReceptionRevueGenre.Text = revue.Genre;
            txbReceptionRevuePublic.Text = revue.Public;
            txbReceptionRevueRayon.Text = revue.Rayon;
            txbReceptionRevueTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbReceptionRevueImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbReceptionRevueImage.Image = null;
            }
            // affiche la liste des exemplaires de la revue
            AfficheReceptionExemplairesRevue();
        }

        /// <summary>
        /// Récupère et affiche les exemplaires d'une revue
        /// </summary>
        private void AfficheReceptionExemplairesRevue()
        {
            string idDocuement = txbReceptionRevueNumero.Text;
            lesExemplaires = controller.GetExemplairesRevue(idDocuement);
            RemplirReceptionExemplairesListe(lesExemplaires, lesEtats);
            AccesReceptionExemplaireGroupBox(true);
        }

        /// <summary>
        /// Permet ou interdit l'accès à la gestion de la réception d'un exemplaire
        /// et vide les objets graphiques
        /// </summary>
        /// <param name="acces">true ou false</param>
        private void AccesReceptionExemplaireGroupBox(bool acces)
        {
            grpReceptionExemplaire.Enabled = acces;
            txbReceptionExemplaireImage.Text = "";
            txbReceptionExemplaireNumero.Text = "";
            pcbReceptionExemplaireImage.Image = null;
            dtpReceptionExemplaireDate.Value = DateTime.Now;
        }

        /// <summary>
        /// Recherche image sur disque (pour l'exemplaire à insérer)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionExemplaireImage_Click(object sender, EventArgs e)
        {
            string filePath = "";
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                // positionnement à la racine du disque où se trouve le dossier actuel
                InitialDirectory = Path.GetPathRoot(Environment.CurrentDirectory),
                Filter = "Files|*.jpg;*.bmp;*.jpeg;*.png;*.gif"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
            }
            txbReceptionExemplaireImage.Text = filePath;
            try
            {
                pcbReceptionExemplaireImage.Image = Image.FromFile(filePath);
            }
            catch
            {
                pcbReceptionExemplaireImage.Image = null;
            }
        }

        /// <summary>
        /// Enregistrement du nouvel exemplaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionExemplaireValider_Click(object sender, EventArgs e)
        {
            if (!txbReceptionExemplaireNumero.Text.Equals(""))
            {
                try
                {
                    int numero = int.Parse(txbReceptionExemplaireNumero.Text);
                    DateTime dateAchat = dtpReceptionExemplaireDate.Value;
                    string photo = txbReceptionExemplaireImage.Text;
                    string idEtat = ETATNEUF;
                    string idDocument = txbReceptionRevueNumero.Text;
                    Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, idDocument);
                    if (controller.CreerExemplaire(exemplaire))
                    {
                        AfficheReceptionExemplairesRevue();
                    }
                    else
                    {
                        MessageBox.Show("numéro de publication déjà existant", "Erreur");
                    }
                }
                catch
                {
                    MessageBox.Show("le numéro de parution doit être numérique", "Information");
                    txbReceptionExemplaireNumero.Text = "";
                    txbReceptionExemplaireNumero.Focus();
                }
            }
            else
            {
                MessageBox.Show("numéro de parution obligatoire", "Information");
            }
        }

        /// <summary>
        /// Tri sur une colonne
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvExemplairesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvReceptionExemplairesListe.Columns[e.ColumnIndex].HeaderText;
            List<Exemplaire> sortedList = new List<Exemplaire>();
            switch (titreColonne)
            {
                case "Numero":
                    sortedList = lesExemplaires.OrderBy(o => o.Numero).Reverse().ToList();
                    break;
                case "DateAchat":
                    sortedList = lesExemplaires.OrderBy(o => o.DateAchat).Reverse().ToList();
                    break;
                case "Etat":
                    sortedList = lesExemplaires.OrderBy(o => o.IdEtat).ToList();
                    break;
            }
            RemplirReceptionExemplairesListe(sortedList, lesEtats);
        }

        /// <summary>
        /// affichage de l'image de l'exemplaire suite à la sélection d'un exemplaire dans la liste
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvReceptionExemplairesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvReceptionExemplairesListe.CurrentCell != null)
            {
                Exemplaire exemplaire = (Exemplaire)bdgExemplairesListe.List[bdgExemplairesListe.Position];
                string image = exemplaire.Photo;
                try
                {
                    pcbReceptionExemplaireRevueImage.Image = Image.FromFile(image);
                }
                catch
                {
                    pcbReceptionExemplaireRevueImage.Image = null;
                }
            }
            else
            {
                pcbReceptionExemplaireRevueImage.Image = null;
            }
        }
        #endregion


        #region AJOUT/MODIF/SUPP LIVRE
        //Action du bouton Ajouter (livre)
        //Le boolean est pour l'affichage des TextBoxs genre public rayon et Bouton modifier.
        private void btnAjout_Click(object sender, EventArgs e)
        {
            //Chaîne de carctère qui va influencer les structures conditionnelles de FrmAjout
            string ongletLivre = "livre";
            //Converti la liste de livres en liste d'objets
            List<Object> listeObjLivre = lesLivres.ConvertAll(Livre => (Object)Livre);
            //Lance frmAjout avec les paramètres nécessaires à l'ajout d'un livre
            FrmAjout frmAjout = new FrmAjout(bdgGenres, bdgPublics, bdgRayons, false, ongletLivre, listeObjLivre);
            frmAjout.Text = "Ajout d'un LIVRE ";
            frmAjout.Show();
            this.Hide();
        }
        //Action du bouton supprimer (livre)
        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            if (dgvLivresListe.CurrentCell != null)     //vérification de selection d'un livre
            {
                if (dgvLivreExemplaire.CurrentCell == null)//Vérification qu'auncun exemplaire est rattaché
                {
                    Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];
                    CommandeDocument cmdLivre = listCmdLivres.Find(x => x.IdLivreDvd.Equals(livre.Id));
                    if (cmdLivre == null) //Vérification qu'aucune commande n'est en cours.
                    {
                        if (MessageBox.Show("Supprimer ?", "Confirmer", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            controller.SupprimerDocument(livre);
                            TabLivres_Enter(null, null);
                        }
                    }
                    else { MessageBox.Show("Une commande est en cours sur ce livre, il ne peut pas être effacé !"); }
                }
                else { MessageBox.Show("Les livres qui possèdent un exemplaire ne peuvent pas être supprimer !"); }
            }
            else { MessageBox.Show("selectionner un livre !"); }

        }
        //Action du bouton modifier (livre)
        //Le boolean est pour l'affichage des TextBoxs genre public rayon et Bouton modifier.
        private void btnModifier_Click(object sender, EventArgs e)
        {
            //Contrôle qu'un livre est sélectionné.
            if (dgvLivresListe.CurrentCell != null)
            {
                //Chaîne de caractère qui va influencer les structures conditionnelles de FrmAjout
                string ongletLivre = "livre";
                List<Object> listeObjLivre = lesLivres.ConvertAll(Livre => (Object)Livre); //Convertit la liste de livre en objet
                Object aModifier = (Object)bdgLivresListe.List[bdgLivresListe.Position];   //convertit le livre sélectionné en objet
                //Lance frmAjout avec les paramètres nécessaires à la modification d'un livre
                FrmAjout frmAjout = new FrmAjout(bdgGenres, bdgPublics, bdgRayons, true, ongletLivre, listeObjLivre, aModifier);
                frmAjout.Text = "Modifification d'un LIVRE";
                frmAjout.Show();
                this.Hide();
            }
            else { MessageBox.Show("selectionnez un livre !"); }
        }
        #endregion

        #region AJOUT/MODIF/SUPP DVD

        //Action du btn AjoutDvd lance la forme Ajout pour un dvd
        private void btnAjoutDvd_Click(object sender, EventArgs e)
        {
            //Chaîne de carctère qui va influencer les structures conditionnelles de FrmAjout
            string ongletDvd = "dvd";
            //Converti la liste de livres en liste d'objets
            List<Object> listeObjDvd = lesDvd.ConvertAll(Dvd => (Object)Dvd);
            //Lance frmAjout avec les paramètres nécessaires à l'ajout d'un livre
            FrmAjout frmAjout = new FrmAjout(bdgGenres, bdgPublics, bdgRayons, false, ongletDvd, listeObjDvd);
            frmAjout.Text = "Ajout d'un DVD";
            frmAjout.Show();
            this.Hide();
        }
        //Action du btn supprimer (DVD)
        private void btnSupprimerDvd_Click(object sender, EventArgs e)
        {
            //contrôle la selection
            if (dgvDvdListe.CurrentCell != null)
            {
                //contrôle un exemplaire
                if (dgvDvdExemplaire.CurrentCell == null)
                {
                    Dvd dvd = (Dvd)bdgDvdListe.List[bdgDvdListe.Position];
                    CommandeDocument cmdDvd = listCmdDvds.Find(x => x.IdLivreDvd.Equals(dvd.Id));
                    //contrôle une commande
                    if (cmdDvd == null)
                    {
                        if (MessageBox.Show("Supprimer ?", "Confirmer", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            controller.SupprimerDocument(dvd);
                            tabDvd_Enter(null, null);
                        }
                    }
                    else { MessageBox.Show("Une commande est en cours sur ce DVD, il ne peut pas être effacé !"); }
                }
                else { MessageBox.Show("Les DVD qui possèdent un exemplaire ne peuvent pas être supprimer !!"); }
            }
            else { MessageBox.Show("selectionner un DVD !"); }
        }
        //Action du btn modifier (DVD)
        private void btnModifDvd_Click(object sender, EventArgs e)
        {
            if (dgvDvdListe.CurrentCell != null)
            {
                string ongletDvd = "dvd";
                List<Object> listeObjDvd = lesDvd.ConvertAll(Dvd => (Object)Dvd);
                Object aModifier = (Object)bdgDvdListe.List[bdgDvdListe.Position];
                FrmAjout frmAjout = new FrmAjout(bdgGenres, bdgPublics, bdgRayons, true, ongletDvd, listeObjDvd, aModifier);
                frmAjout.Text = "Modification d'un DVD";
                frmAjout.Show();
                this.Hide();
            }
            else { MessageBox.Show("Selectionnez un Dvd !"); }
        }
        #endregion

        #region AJOUT/MODIF/SUPP REVUE

        //Action, du bouton Ajouter (Revue)
        private void btnAjoutRevue_Click(object sender, EventArgs e) // List<Revue> lesRevues
        {
            //Chaîne de carctère qui va influencer FrmAjout
            string ongletRevue = "revue";
            //Converti la liste de revues en liste d'objets
            List<Object> listeObjRevue = lesRevues.ConvertAll(Revue => (Object)Revue);
            //Lance frmAjout avec les paramètres nécessaires à l'ajout d'un livre
            FrmAjout frmAjout = new FrmAjout(bdgGenres, bdgPublics, bdgRayons, false, ongletRevue, listeObjRevue);
            frmAjout.Text = "Ajout d'une REVUE";
            frmAjout.Show();
            this.Hide();
        }
        //Action du bouton supprimer (Revue)
        private void btnSupprimerRevue_Click(object sender, EventArgs e)
        {
            //vérification de la selection
            if (dgvRevuesListe.CurrentCell != null)
            {
                //controle si un abonnement (donc une commande) est en cours
                Revue revue = (Revue)bdgRevuesListe.List[bdgRevuesListe.Position];
                Abonnement testId = listCmdRevues.Find(x => x.IdRevue.Equals(revue.Id));
                if (testId == null)
                {
                    //controle si des exmplaires sont rattaché à la revue.
                    lesExemplaires = controller.GetExemplairesRevue(revue.Id);
                    Exemplaire exemplaire = lesExemplaires.Find(x => x.Id.Equals(revue.Id));
                    if (exemplaire == null)
                    {
                        if (MessageBox.Show("Supprimer ?", "Confirmer", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            controller.SupprimerDocument(revue);
                            tabRevues_Enter(null, null);
                        }
                    }
                    else { MessageBox.Show("Impossible de supprimer une revue avec des exemplaires !"); }
                }
                else { MessageBox.Show("Impossible de supprimer une revue avec un abonnement en cours !"); }
            }
            else { MessageBox.Show("selectionner une Revue !"); }
        }
        //Action du bouton modifier (Revue)
        private void btnModifierRevue_Click(object sender, EventArgs e)
        {
            //Controle une selection de revue
            if (dgvRevuesListe.CurrentCell != null)
            {
                //Chaîne de carctère qui va influencer FrmAjout
                string ongletRevue = "revue";
                //Converti la liste de revues en liste d'objets
                List<Object> listeObjRevue = lesRevues.ConvertAll(Revue => (Object)Revue);
                //Lance frmAjout avec les paramètres nécessaires à l'ajout d'une revue
                Object aModifier = (Object)bdgRevuesListe.List[bdgRevuesListe.Position];
                FrmAjout frmAjout = new FrmAjout(bdgGenres, bdgPublics, bdgRayons, true, ongletRevue, listeObjRevue, aModifier);
                frmAjout.Text = "Modification d'une REVUE";
                frmAjout.Show();
                this.Hide();
            }
        }
        #endregion

        #region onglet Commande livre


        //Nécessaire pour remplir la DataGrideView
        private readonly BindingSource bdgCommandeLivresListe = new BindingSource();
        private List<CommandeDocument> listCmdLivres = new List<CommandeDocument>();
        //Nécessaire pour la modification (livre et dvd)
        private bool cmdLivreOuDvdModif = false;
        //Nécessaire pour controler l'état des suivis (livre et dvd)
        private int indiceSuivi;
        //retourne l'id de suivi selectionné
        private string GetIdSuivi(string unSuivi)
        {
            List<Categorie> uneListe = controller.GetSuivi();
            foreach (Categorie uneCategorie in uneListe)
            {
                if (uneCategorie.Libelle == unSuivi)
                {
                    return uneCategorie.Id;
                }
            }
            return null;
        }
        //retourne l'id au format 4 digits 1 =>0001
        private string FormaterId(string id)
        {
            int idFormate = int.Parse(id);
            return idFormate.ToString("D4");
        }
        //Action du l'onglet Commande de livres
        private void tabOngletCommandeLivre_Enter(object sender, EventArgs e)
        {
            //Charge la liste de livre commandé
            listCmdLivres = controller.GetAllCommandeLivres();
            //Remplit la DGV
            ChargeDgvCmdLivres(listCmdLivres);
            //Charge la cbx suivi
            RemplirComboCategorie(controller.GetSuivi(), bdgSuivi, cbxSuivi);
            grbCmdLivre2.Enabled = false;
            grbCmdLivre2.Enabled = false;
            indiceSuivi = -2;
            cmdLivreOuDvdModif = false;
        }
        //Action du bouton recherche d'un livre sur un ID
        private void btnRechCmdLivre_Click(object sender, EventArgs e)
        {
            //Crée un livre si il a l'id qui est entré dans la recherche
            Livre livre = lesLivres.Find(x => x.Id.Equals(txbCmdLivreNum.Text));
            if (livre != null)
            {
                AfficheCommandeLivreInfos(livre);
            }
            else
            {
                MessageBox.Show("numéro introuvable");
            }
        }
        //Affiche les informations du livre dans commande livre.
        private void AfficheCommandeLivreInfos(Livre livre)
        {
            txbCmdLivreTitre.Text = livre.Titre;
            txbCmdLivreAuteur.Text = livre.Auteur;
            txbCmdLivreCollection.Text = livre.Collection;
            txbCmdLivreGenre.Text = livre.Genre;
            txbCmdLivrePublic.Text = livre.Public;
            txbCmdLivreRayon.Text = livre.Rayon;
            txbCmdLivreISBN.Text = livre.Isbn;
            txbCmdLivreId.Text = livre.Id;
            string image = livre.Image;
            try
            {
                pcbCmdLivreImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbCmdLivreImage.Image = null;
            }

        }
        //Action du btn ajouter une commande de livre
        private void btnAjouterCmdLivre_Click(object sender, EventArgs e)
        {
            //Teste sur le titre, un livre à toujours un titre.
            if (txbCmdLivreTitre.Text != "")
            {
                grbCmdLivre2.Enabled = true;
                txbCmdLivreNumCmd.Enabled = true;
                //lors de l'enregistrement la commande doit être en cours
                cbxSuivi.SelectedIndex = 0;
                cbxSuivi.Enabled = false;
            }
        }

        //Vide les txb de commandelivre
        private void VideCmdLivre()
        {
            grbCmdLivre2.Enabled = false;
            txbCmdLivreTitre.Text = "";
            txbCmdLivreAuteur.Text = "";
            txbCmdLivreCollection.Text = "";
            txbCmdLivrePublic.Text = "";
            txbCmdLivreRayon.Text = "";
            txbCmdLivreISBN.Text = "";
            txbCmdLivreId.Text = "";
            txbCmdLivreNumCmd.Text = "";
            txbCmdLivreMontantCmd.Text = "";
            txbCmdLivrenbExCmd.Text = "";
            cbxSuivi.SelectedIndex = -1;
            dtpCmdLivreDateCmd.Value = DateTime.Today;
        }

        //Action du btn valider, il envoi la cmd de livre à la bdd
        private void btnCmdLivreValider_Click(object sender, EventArgs e)
        {
            CommandeDocument cmdLivre = SuperCmdLivre();
            //Si la commande est valorisé ou que l'on ne modifie pas une commande
            if (cmdLivre != null && cmdLivreOuDvdModif == false)
            {
                //Envoyer la commande au controleur.
                if (controller.EnvoiDocuments(cmdLivre))
                {
                    MessageBox.Show("Livre commandé.");
                    VideCmdLivre();
                    tabOngletCommandeLivre_Enter(null, null);
                }
            }
            if (cmdLivre != null && cmdLivreOuDvdModif == true)
            {
                if (GestionSuivi(cmdLivre, indiceSuivi))
                {
                    if (controller.ModifierDocuments(cmdLivre))
                    {
                        MessageBox.Show("Commande modifié.");
                        cmdLivreOuDvdModif = false;
                        VideCmdLivre();
                        tabOngletCommandeLivre_Enter(null, null);
                    }
                }
            }
        }
        //Valorise une commande
        private CommandeDocument ValoriseCommandeLivre()
        {
            try
            {
                string id = FormaterId(txbCmdLivreNumCmd.Text);
                DateTime dateCommande = dtpCmdLivreDateCmd.Value;
                double montant = double.Parse(txbCmdLivreMontantCmd.Text);
                int nbExemplaire = int.Parse(txbCmdLivrenbExCmd.Text);
                string idSuivi = GetIdSuivi(cbxSuivi.Text);
                string suivi = null;
                String idLivreDvd = txbCmdLivreId.Text;
                ;
                CommandeDocument commandeValorise = new CommandeDocument(id, dateCommande, montant, nbExemplaire, idSuivi, suivi, idLivreDvd);
                return commandeValorise;
            }
            catch (Exception ex) { return null; }
        }
        //Contrôle des valeurs entrées
        private CommandeDocument SuperCmdLivre()
        {
            try
            {
                //Valorise la commande.
                CommandeDocument cmdValorise = ValoriseCommandeLivre();
                //Contrôle le numéro de la commande.
                if (int.Parse(txbCmdLivreNumCmd.Text) > 0 && int.Parse(txbCmdLivreNumCmd.Text) < 1000)
                {
                    //Contrôle le montant de la commande.
                    if (double.Parse(txbCmdLivreMontantCmd.Text) > 0)
                    {
                        //Contrôle le nombre d'exmplaire de la commande.
                        if (int.Parse(txbCmdLivrenbExCmd.Text) > 0)
                        {
                            if (cmdValorise.IdSuivi != null)
                            {
                                //Contrôle que l'id de la commande soit disponible.
                                CommandeDocument testId = listCmdLivres.Find(x => x.Id.Equals(cmdValorise.Id));
                                if (testId == null || cmdLivreOuDvdModif == true)
                                {
                                    return cmdValorise;
                                }
                                else { MessageBox.Show("Numéro de commande déja utilisé"); return null; }
                            }
                            else { MessageBox.Show("Selectionnez un suivi"); return null; }
                        }
                        else { MessageBox.Show("Entrez un nombre d'exemplaire valide"); return null; }
                    }
                    else { MessageBox.Show("Entrez un montant valide"); return null; }
                }
                else { MessageBox.Show("Entrez un numéro de commande compris entre 1 et 1000"); return null; }
            }
            catch (Exception ex) { return null; }
        }
        //Methode pour remplir la liste des commandes de livre
        private void ChargeDgvCmdLivres(List<CommandeDocument> cmdLivre)
        {
            //transfère de la liste à la bidingsource
            bdgCommandeLivresListe.DataSource = cmdLivre;
            //transfère de la bindingsource à la data grid view
            dgvCmdLivre.DataSource = bdgCommandeLivresListe;
            //gestions des tailles de colonnes
            dgvCmdLivre.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            //Choix des colonnes affichés et de leur noms.
            dgvCmdLivre.Columns["IdSuivi"].Visible = false;
            dgvCmdLivre.Columns["DateCommande"].DisplayIndex = 0;
            dgvCmdLivre.Columns[3].HeaderText = "n° de document";
            dgvCmdLivre.Columns[4].HeaderText = "n° de commande";
        }
        //Action qui supprime une commande
        private void btnSupprimer_Click_1(object sender, EventArgs e)
        {
            //vérification selection
            if (dgvCmdLivre.CurrentCell != null)
            {
                if (MessageBox.Show("Supprimer ?", "Confirmer", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    CommandeDocument uneCommande = (CommandeDocument)bdgCommandeLivresListe.List[bdgCommandeLivresListe.Position];
                    //Contrôle sur le numéro d'id, 3 = livré 4 = réglé.
                    if (int.Parse(uneCommande.IdSuivi) < 3)
                    {
                        if (controller.SupprimerDocument(uneCommande))
                        {
                            MessageBox.Show("Commande supprimé.");
                            tabOngletCommandeLivre_Enter(null, null);
                        }
                    }
                    else { MessageBox.Show("Impossible de supprimer une commande déja livrée !"); }
                }
            }
            else { MessageBox.Show("selectionner une Commande !"); }
        }
        //Action du btn modifier dans commande de livre
        private void btnModiferCmdLivre_Click(object sender, EventArgs e)
        {
            //contrôle si une ligne est sélectionnée.
            if (dgvCmdLivre.CurrentCell != null)
            {
                try
                {
                    //booléen qui va influencer le contrôleur choisi.
                    cmdLivreOuDvdModif = true;
                    CommandeDocument cmd = (CommandeDocument)bdgCommandeLivresListe.List[bdgCommandeLivresListe.Position];
                    //Utilise cet indice pour valoriser le suivi d'une commande
                    indiceSuivi = int.Parse(cmd.IdSuivi);
                    //Gestion de l'affichage
                    grbCmdLivre2.Enabled = cmdLivreOuDvdModif;
                    cbxSuivi.Enabled = cmdLivreOuDvdModif;
                    txbCmdLivreNumCmd.Enabled = !cmdLivreOuDvdModif;
                    txbCmdLivreNumCmd.Text = cmd.Id;
                    txbCmdLivreMontantCmd.Text = cmd.Montant.ToString();
                    txbCmdLivrenbExCmd.Text = cmd.NbExemplaire.ToString();
                    dtpCmdLivreDateCmd.Value = cmd.DateCommande;
                }
                catch
                {
                }
            }
            else { MessageBox.Show("Selectionner une commande"); }
        }
        //Affiche les informations détaillés sur la selection de la data gride view
        private void dtgCmdLivre_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCmdLivre.CurrentCell != null)
            {
                try
                {
                    CommandeDocument cmd = (CommandeDocument)bdgCommandeLivresListe.List[bdgCommandeLivresListe.Position];
                    Livre livre = lesLivres.Find(x => x.Id.Equals(cmd.IdLivreDvd));
                    AfficheCommandeLivreInfos(livre);
                }
                catch
                {
                }
            }

        }
        //Gestion des suivis des commandes
        private bool GestionSuivi(CommandeDocument cmd, int ancienSuiv)
        {
            //Contrôle que le suivit de commande est cohérent encours (1)  => relance (2) => livré (3) => réglé (5)
            //livré = 3 moins réglé = 5 résultat = 2 si autre que livré résultat supérieur a 2
            if (int.Parse(cmd.IdSuivi) >= ancienSuiv && (int.Parse(cmd.IdSuivi) - ancienSuiv < 3))
            {
                return true;
            }
            else
            {
                MessageBox.Show("erreur du suivi sélectionné");
                return false;
            }
        }
        //Trie de la gride view
        private void dtgCmdLivre_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            List<CommandeDocument> sortedList = new List<CommandeDocument>();
            string titreColonne = dgvCmdLivre.Columns[e.ColumnIndex].HeaderText;
            switch (titreColonne)
            {
                case "DateCommande":
                    sortedList = listCmdLivres.OrderBy(o => o.DateCommande).ToList();
                    break;
                case "NbExemplaire":
                    sortedList = listCmdLivres.OrderBy(o => o.NbExemplaire).ToList();
                    break;
                case "Suivi":
                    sortedList = listCmdLivres.OrderBy(o => o.Suivi).ToList();
                    break;
                case "n° de document":
                    sortedList = listCmdLivres.OrderBy(o => o.IdLivreDvd).ToList();
                    break;
                case "n° de commande":
                    sortedList = listCmdLivres.OrderBy(o => o.Id).ToList();
                    break;
                case "Montant":
                    sortedList = listCmdLivres.OrderBy(o => o.Montant).ToList();
                    break;
            }
            ChargeDgvCmdLivres(sortedList);

        }
        //Action du bouton Annuler
        private void btnCmdLivreAnnuler_Click(object sender, EventArgs e)
        {
            tabOngletCommandeLivre_Enter(null, null);
        }
        #endregion

        #region onglet Commande DVD
        //Nécessaire pour remplir la DataGrideView
        private readonly BindingSource bdgCommandeDvdsListe = new BindingSource();
        private List<CommandeDocument> listCmdDvds = new List<CommandeDocument>();
        //Action à l'ouverture de l'onglet DVd
        private void tabOngletCommandeDvd_Enter(object sender, EventArgs e)
        {
            //Charger la liste de dvd car elle ne se rempplit pas au démmarage comme pour livre
            lesDvd = controller.GetAllDvd();
            //Remplit la dgv 
            listCmdDvds = controller.GetAllCommandeDvds();
            ChargeDgvCmdDvds(listCmdDvds);
            RemplirComboCategorie(controller.GetSuivi(), bdgSuivi, cbxSuiviDvd);
            grbCmdDvd.Enabled = false;
            grbCmdDvd2.Enabled = false;
            indiceSuivi = -2;
            cmdLivreOuDvdModif = false;

        }
        //Méthode pour remplir la liste des commandes de dvd
        private void ChargeDgvCmdDvds(List<CommandeDocument> cmdDvd)
        {
            bdgCommandeDvdsListe.DataSource = cmdDvd;
            dgvCmdDvd.DataSource = bdgCommandeDvdsListe;
            dgvCmdDvd.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvCmdDvd.Columns["IdSuivi"].Visible = false;
            dgvCmdDvd.Columns["DateCommande"].DisplayIndex = 0;
            dgvCmdDvd.Columns[3].HeaderText = "n° de document";
            dgvCmdDvd.Columns[4].HeaderText = "n° de commande";
        }

        //Action du bouton recherche d'un dvd sur l'ID
        private void btnRechCmdDvd_Click(object sender, EventArgs e)
        {
            Dvd dvd = lesDvd.Find(x => x.Id.Equals(txbCmdDvdNum.Text));
            if (dvd != null)
            {
                AfficheCommandeDvdInfos(dvd);
            }
            else { MessageBox.Show("numéro introuvable"); }
        }
        //Affiche les informations du dvd dans commande dvd
        private void AfficheCommandeDvdInfos(Dvd dvd)
        {
            txbCmdDvdTitre.Text = dvd.Titre;
            txbCmdDvdRealisateur.Text = dvd.Realisateur;
            txbCmdDvdSynopsis.Text = dvd.Synopsis;
            txbCmdDvdGenre.Text = dvd.Genre;
            txbCmdDvdPublic.Text = dvd.Public;
            txbCmdDvdRayon.Text = dvd.Rayon;
            txbCmdDvdDuree.Text = dvd.Duree.ToString();
            txbCmdDvdId.Text = dvd.Id;
            string image = dvd.Image;
            try
            {
                pcbCmdDvdImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbCmdDvdImage.Image = null;
            }
        }
        //Action du bouton AJouter une commande de dvd
        private void btnAjouterCmdDvd_Click(object sender, EventArgs e)
        {
            if (txbCmdDvdTitre.Text != "")
            {
                grbCmdDvd2.Enabled = true;
                txbCmdDvdNumCmd.Enabled = true;
                cbxSuiviDvd.SelectedIndex = 0;
                cbxSuiviDvd.Enabled = false;
            }
        }
        //Action du bouton Valider une commande de dvd
        private void btnCmdDvdValider_Click(object sender, EventArgs e)
        {
            CommandeDocument cmdDvd = SuperCmdDvd();
            if (cmdDvd != null && cmdLivreOuDvdModif == false)
                if (controller.EnvoiDocuments(cmdDvd))
                {
                    MessageBox.Show("Dvd commandé");
                    VideCmdDvd();
                    tabOngletCommandeDvd_Enter(null, null);
                }
            if (cmdDvd != null && cmdLivreOuDvdModif == true)
            {
                if (GestionSuivi(cmdDvd, indiceSuivi))
                {
                    if (controller.ModifierDocuments(cmdDvd))
                    {
                        MessageBox.Show("Commande modifiée.");
                        VideCmdDvd();
                        tabOngletCommandeDvd_Enter(null, null);
                    }
                }
            }
        }
        //Vide les txb de commande dvd
        private void VideCmdDvd()
        {
            grbCmdDvd2.Enabled = false;
            txbCmdDvdNumCmd.Text = "";
            dtpCmdDvdDateCmd.Value = DateTime.Today;
            txbCmdDvdMontantCmd.Text = "";
            txbCmdDvdnbExCmd.Text = "";
            cbxSuiviDvd.SelectedIndex = -1;
            txbCmdDvdId.Text = "";
            txbCmdDvdDuree.Text = "";
            txbCmdDvdTitre.Text = "";
            txbCmdDvdRealisateur.Text = "";
            txbCmdDvdSynopsis.Text = "";
            txbCmdDvdGenre.Text = "";
            txbCmdDvdPublic.Text = "";
            txbCmdDvdRayon.Text = "";
        }
        //Contrôle les valeurs entées
        private CommandeDocument SuperCmdDvd()
        {
            try
            {
                CommandeDocument cmdValorise = ValoriseCommandeDvd();
                if (int.Parse(txbCmdDvdNumCmd.Text) > 999 && int.Parse(txbCmdDvdNumCmd.Text) < 2000)
                {
                    if (double.Parse(txbCmdDvdMontantCmd.Text) > 0)
                    {
                        if (int.Parse(txbCmdDvdnbExCmd.Text) > 0)
                        {
                            if (cmdValorise.IdSuivi != null)
                            {
                                CommandeDocument testId = listCmdDvds.Find(x => x.Id.Equals(cmdValorise.Id));
                                if (testId == null || cmdLivreOuDvdModif == true)
                                {
                                    return cmdValorise;
                                }
                                else { MessageBox.Show("Numéro de commande déja utilisé"); return null; }
                            }
                            else { MessageBox.Show("Selectionnez un suivi"); return null; }
                        }
                        else { MessageBox.Show("Entrez un nombre d'exemplaire valide"); return null; }
                    }
                    else { MessageBox.Show("Entrez un montant valide"); return null; }
                }
                else { MessageBox.Show("Entrez un numéro de commande compris entre 1000 et 2000"); return null; }
            }
            catch (Exception ex) { return null; }
        }
        //Valorise une commande de dvd
        private CommandeDocument ValoriseCommandeDvd()
        {
            try
            {
                string id = FormaterId(txbCmdDvdNumCmd.Text);
                DateTime dateCommande = dtpCmdDvdDateCmd.Value;
                double montant = double.Parse(txbCmdDvdMontantCmd.Text);
                int nbExemplaire = int.Parse(txbCmdDvdnbExCmd.Text);
                string idSuivi = GetIdSuivi(cbxSuiviDvd.Text);
                string suivi = null;
                String idLivreDvd = txbCmdDvdId.Text;

                CommandeDocument commandeValorise = new CommandeDocument(id, dateCommande, montant, nbExemplaire, idSuivi, suivi, idLivreDvd);
                return commandeValorise;
            }
            catch (Exception ex) { return null; }
        }
        //Action du bouton modifier dans commande de dvd
        private void btnModiferCmdDvd_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvCmdDvd.CurrentCell != null)
                {
                    cmdLivreOuDvdModif = true;
                    CommandeDocument cmd = (CommandeDocument)bdgCommandeDvdsListe.List[bdgCommandeDvdsListe.Position];
                    indiceSuivi = int.Parse(cmd.IdSuivi);
                    grbCmdDvd2.Enabled = cmdLivreOuDvdModif;
                    cbxSuiviDvd.Enabled = cmdLivreOuDvdModif;
                    txbCmdDvdNumCmd.Enabled = !cmdLivreOuDvdModif;
                    txbCmdDvdNumCmd.Text = cmd.Id;
                    txbCmdDvdMontantCmd.Text = cmd.Montant.ToString();
                    txbCmdDvdnbExCmd.Text = cmd.NbExemplaire.ToString();
                    dtpCmdDvdDateCmd.Value = cmd.DateCommande;
                }
            }
            catch (Exception ex) { }
        }
        //Affiche les informations détaillées sur la sélection de la DGV
        private void dgvCmdDvd_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCmdDvd.CurrentCell != null)
            {
                CommandeDocument cmd = (CommandeDocument)bdgCommandeDvdsListe.List[bdgCommandeDvdsListe.Position];
                Dvd dvd = lesDvd.Find(x => x.Id.Equals(cmd.IdLivreDvd));
                AfficheCommandeDvdInfos(dvd);
            }
        }
        //Trie de la DGV
        private void dgvCmdDvd_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            List<CommandeDocument> sortedList = new List<CommandeDocument>();
            string titreColonne = dgvCmdDvd.Columns[e.ColumnIndex].HeaderText;
            switch (titreColonne)
            {
                case "DateCommande":
                    sortedList = listCmdDvds.OrderBy(o => o.DateCommande).ToList();
                    break;
                case "NbExemplaire":
                    sortedList = listCmdDvds.OrderBy(o => o.NbExemplaire).ToList();
                    break;
                case "Suivi":
                    sortedList = listCmdDvds.OrderBy(o => o.Suivi).ToList();
                    break;
                case "n° de document":
                    sortedList = listCmdDvds.OrderBy(o => o.IdLivreDvd).ToList();
                    break;
                case "n° de commande":
                    sortedList = listCmdDvds.OrderBy(o => o.Id).ToList();
                    break;
                case "Montant":
                    sortedList = listCmdDvds.OrderBy(o => o.Montant).ToList();
                    break;
            }
            ChargeDgvCmdDvds(sortedList);
        }
        //Action du bouton supprimer pour une commande de dvd
        private void btnSupprimerCmdDvd_Click(object sender, EventArgs e)
        {
            if (dgvCmdDvd.CurrentCell != null)     //vérification selection
            {
                if (MessageBox.Show("Supprimer ?", "Confirmer", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    //CommandeDocument uneCommande = (CommandeDocument)bdgCommandeDvdsListe.List[bdgDvdListe.Position];
                    CommandeDocument uneCommande = (CommandeDocument)bdgCommandeDvdsListe.List[bdgCommandeDvdsListe.Position];
                    //Contrôle sur le numéro d'id, 3 = livré 4 = réglé.
                    if (int.Parse(uneCommande.IdSuivi) < 3)
                    {
                        if (controller.SupprimerDocument(uneCommande))
                        {
                            MessageBox.Show("Commande supprimé.");
                            tabOngletCommandeDvd_Enter(null, null);
                        }
                    }
                    else
                    { MessageBox.Show("Impossible de supprimer une commande déja livrée !"); }
                }
            }
            else
            {
                MessageBox.Show("selectionner une Commande !");
            }
        }
        //Action du bouton annuler
        private void btnCmdDvdAnnuler_Click(object sender, EventArgs e)
        {
            tabOngletCommandeDvd_Enter(null, null);
        }
        #endregion

        #region Commande Revu / d'abonnement
        private readonly BindingSource bdgCommandeRevueListe = new BindingSource();
        private List<Abonnement> listCmdRevues = new List<Abonnement>();
        //Action de l'onglet Commande de Revues
        private void tabOngletCommandeRevue_Enter(object sender, EventArgs e)
        {
            //Il faut forcer la liste des Revues et d exemplaire
            lesRevues = controller.GetAllRevues(); ;
            listCmdRevues = controller.GetAllCommandeRevues();
            ChargeDGVCmdRevue(listCmdRevues);
            grpCmdRevue.Enabled = false;
            grpCmdRevue2.Enabled = false;
        }
        //Méthode qui remplie la DGV
        private void ChargeDGVCmdRevue(List<Abonnement> cmdRevue)
        {
            bdgCommandeRevueListe.DataSource = cmdRevue;
            dgvCmdRevue.DataSource = bdgCommandeRevueListe;
            dgvCmdRevue.Columns["DateCommande"].DisplayIndex = 0;
            dgvCmdRevue.Columns[1].HeaderText = "n° de document";
            dgvCmdRevue.Columns[2].HeaderText = "n° de commande";
        }
        //Action du bouton Recherche dans commande Revue
        private void btnRechCmdRevue_Click(object sender, EventArgs e)
        {
            Revue revue = lesRevues.Find(x => x.Id.Equals(txbCmdRevueNum.Text));
            if (revue != null)
            {
                AfficheCommandeRevueInfos(revue);
            }
            else { MessageBox.Show("numéro introuvable"); }
        }
        //Affiche les informations de la revue dans commande reuve
        private void AfficheCommandeRevueInfos(Revue revue)
        {
            txbCmdRevueTitre.Text = revue.Titre;
            txbCmdRevuePeriodicite.Text = revue.Periodicite;
            txbCmdRevueDelai.Text = revue.DelaiMiseADispo.ToString();
            txbCmdRevueGenre.Text = revue.Genre;
            txbCmdRevuePublic.Text = revue.Public;
            txbCmdRevueRayon.Text = revue.Rayon;
            txbCmdRevueId.Text = revue.Id;
            string image = revue.Image;
            try
            {
                pcbCmdRevueImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbCmdRevueImage.Image = null;
            }
        }
        //Action du bouton Ajouter une commande de revue
        private void btnAjouterCmdRevue_Click(object sender, EventArgs e)
        {
            if (txbCmdRevueTitre.Text != null)
            {
                grpCmdRevue2.Enabled = true;
            }
        }
        //Action du bouton valider une revue
        private void btnCmdRevueValider_Click(object sender, EventArgs e)
        {
            Abonnement cmdRevue = SuperCmdRevue();
            if (cmdRevue != null && controller.EnvoiDocuments(cmdRevue))
            {
                MessageBox.Show("Abonnement pris");
                VideCmdRevue();
                tabOngletCommandeRevue_Enter(null, null);
            }
        }
        //Vide les txb de commande revue
        private void VideCmdRevue()
        {
            grpCmdRevue2.Enabled = false;
            txbCmdRevueNumCmd.Text = "";
            txbCmdRevueMontantCmd.Text = "";
            dtpCmdRevueDateCmd.Value = DateTime.Today;
            dtpCmdRevueDateAbo.Value = DateTime.Today;
            txbCmdRevueId.Text = "";
            txbCmdRevueTitre.Text = "";
            txbCmdRevuePeriodicite.Text = "";
            txbCmdRevueDelai.Text = "";
            txbCmdRevueGenre.Text = "";
            txbCmdRevuePublic.Text = "";
            txbCmdRevueRayon.Text = "";
        }
        //Contrôle les valeurs
        private Abonnement SuperCmdRevue()
        {
            try
            {
                Abonnement cmdValorise = ValoriseCommandeRevue();
                if (int.Parse(txbCmdRevueNumCmd.Text) > 2000 && (int.Parse(txbCmdRevueNumCmd.Text) < 3000))
                {
                    if (int.Parse(txbCmdRevueMontantCmd.Text) > 0)
                    {
                        Abonnement testId = listCmdRevues.Find(x => x.Id.Equals(cmdValorise.Id));
                        if (testId == null)
                        {
                            return cmdValorise;
                        }
                        else { MessageBox.Show("Numéro de commande déja utilisé."); return null; }
                    }
                    else { MessageBox.Show("Entrez un montant valide"); return null; }
                }
                else { MessageBox.Show("Entrez un numéro de commande compris entre 2000 et 3000"); return null; }
            }
            catch (Exception ex) { return null; }
        }
        //Valorise une commande de revue
        private Abonnement ValoriseCommandeRevue()
        {
            try
            {
                string id = FormaterId(txbCmdRevueNumCmd.Text);
                DateTime dateCommande = dtpCmdRevueDateCmd.Value;
                double montant = double.Parse(txbCmdRevueMontantCmd.Text);
                DateTime dateAbo = dtpCmdRevueDateAbo.Value;
                string idRevue = txbCmdRevueId.Text;

                Abonnement cmdValorise = new Abonnement(id, dateCommande, montant, dateAbo, idRevue);
                return cmdValorise;
            }
            catch (Exception ex) { return null; }
        }
        //Affiche les informations détaillées sur la sélection dans la DGV
        private void dgvCmdRevue_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCmdRevue != null)
            {
                Abonnement cmd = (Abonnement)bdgCommandeRevueListe.List[bdgCommandeRevueListe.Position];
                Revue revue = lesRevues.Find(x => x.Id.Equals(cmd.IdRevue));
                AfficheCommandeRevueInfos(revue);
            }
        }
        //Trie sur la DGV
        private void dgvCmdRevue_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            List<Abonnement> sortedList = new List<Abonnement>();
            string titreColonne = dgvCmdRevue.Columns[e.ColumnIndex].HeaderText;
            switch (titreColonne)
            {
                case "DateCommande":
                    sortedList = listCmdRevues.OrderBy(o => o.DateCommande).ToList();
                    break;
                case "DateFinAbonnement":
                    sortedList = listCmdRevues.OrderBy(o => o.DateFinAbonnement).ToList();
                    break;
                case "n° de document":
                    sortedList = listCmdRevues.OrderBy(o => o.IdRevue).ToList();
                    break;
                case "n° de commande":
                    sortedList = listCmdRevues.OrderBy(o => o.Id).ToList();
                    break;
                case "Montant":
                    sortedList = listCmdRevues.OrderBy(o => o.Montant).ToList();
                    break;
            }
            ChargeDGVCmdRevue(sortedList);
        }
        //Action du bouton Supprimer pour une commande de Revue
        private void btnSupprimerCmdRevue_Click(object sender, EventArgs e)
        {
            if (dgvCmdRevue.CurrentCell != null)     //vérification selection
            {
                if (MessageBox.Show("Supprimer ?", "Confirmer", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Abonnement uneCmd = (Abonnement)bdgCommandeRevueListe.List[bdgCommandeRevueListe.Position];
                    //Ou parcourExemplaire est nulle alors supprime
                    if (ParcourExemplaire(uneCmd.DateCommande, uneCmd.DateFinAbonnement, uneCmd.IdRevue))
                    {
                        controller.SupprimerDocument(uneCmd);
                        MessageBox.Show("Commande supprimé.");
                        tabOngletCommandeRevue_Enter(null, null);
                    }
                    else { MessageBox.Show("Une parution est en cours."); }
                }
            }
            else { MessageBox.Show("selectionner une Commande !"); }
        }
        //Récupère la liste des exemplaire pour un id donné. Si trouve l exemplaire parcour la liste.
        private bool ParcourExemplaire(DateTime dateCmd, DateTime dateFInAbo, string idDocument)
        {
            lesExemplaires = controller.GetExemplairesRevue(idDocument);
            foreach (Exemplaire exemplaire in lesExemplaires)
            {
                if (ParutionEntreCmdEtAbonnement(dateCmd, dateFInAbo, exemplaire.DateAchat))
                {
                    return false;
                }
            }
            return true;

        }
        //Compare les dates
        private bool ParutionEntreCmdEtAbonnement(DateTime dateCmd, DateTime dateFinAbo, DateTime dateParu)
        {
            //retourne faux si une parution est en cours.
            if (dateCmd < dateParu && dateFinAbo > dateParu)
            {
                return false;
            }
            else
            { return true; }
        }
        //Contrôle abonnement - 30jours
        private void AbonnementsSurLaFIn()
        {
            string a = "Les abonnements qui se finissent sous moins de 30 jours sont : \n";
            lesRevues = controller.GetAllRevues(); 
            listCmdRevues = controller.GetAllCommandeRevues();
            foreach (Abonnement aboRevue in listCmdRevues)
            {
                DateTime dateMoins30Jours = aboRevue.DateFinAbonnement.AddDays(-30);
                if (dateMoins30Jours < DateTime.Now)
                {
                    Revue revue = lesRevues.Find(x => x.Id.Equals(aboRevue.IdRevue));
                    a = $"{a}titre : {revue.Titre} date de fin : {aboRevue.DateFinAbonnement.ToString()}\n";
                }
            }
            MessageBox.Show(a);
        }
        //Action du bouton Annuler pour une commande de Revue
        private void btnAnnulerCmdRevue_Click(object sender, EventArgs e)
        {
            tabOngletCommandeRevue_Enter(null, null);

        }





        #endregion

        //Contrôle les accès des utilisateur
        private void Permission(Utilisateur utilisateur)
        {
            int service = int.Parse(utilisateur.IdService);
            switch (service)
            {
                case 4: //service culture
                    MessageBox.Show(" les droits ne sont pas suffisants pour accéder à cette application");
                    Application.Exit();
                    break;
                case 3://service prêt
                    tabOngletCommandeLivre.Enabled = false;
                    tabOngletCommandeDvd.Enabled = false;
                    tabOngletCommandeRevue.Enabled = false;
                    
                    btnAjoutLivre.Enabled = false;
                    btnAjoutDvd.Enabled = false;
                    btnAjoutRevue.Enabled = false;
                    btnSupprimerLivre.Enabled = false;
                    btnSupprimerDvd.Enabled = false;
                    btnSupprimerRevue.Enabled = false;
                    btnModifier.Enabled = false;
                    btnModifDvd.Enabled = false;
                    btnModifierRevue.Enabled = false;

                    groupBox1.Enabled = false;
                    groupBox2.Enabled = false;
                    grpReceptionExemplaire.Enabled = false;

                    dgvCmdLivre.Visible = false;
                    dgvCmdDvd.Visible = false;
                    dgvCmdRevue.Visible = false;
                    break;
                case 2://service administratif
                    AbonnementsSurLaFIn();
                    break;
                case 1://ADMIN
                    
                    break;
            }


        }
    }
}
