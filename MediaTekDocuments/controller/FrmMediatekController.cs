using System.Collections.Generic;
using MediaTekDocuments.model;
using MediaTekDocuments.dal;

namespace MediaTekDocuments.controller
{
    /// <summary>
    /// Contrôleur lié à FrmMediatek
    /// </summary>
    class FrmMediatekController
    {
        /// <summary>
        /// Objet d'accès aux données
        /// </summary>
        private readonly Access access;

        /// <summary>
        /// Récupération de l'instance unique d'accès aux données
        /// </summary>
        public FrmMediatekController()
        {
            access = Access.GetInstance();
        }

        /// <summary>
        /// getter sur la liste des genres
        /// </summary>
        /// <returns>Liste d'objets Genre</returns>
        public List<Categorie> GetAllGenres()
        {
            return access.GetAllGenres();
        }

        /// <summary>
        /// getter sur la liste des livres
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public List<Livre> GetAllLivres()
        {
            return access.GetAllLivres();
        }

        /// <summary>
        /// getter sur la liste des Dvd
        /// </summary>
        /// <returns>Liste d'objets dvd</returns>
        public List<Dvd> GetAllDvd()
        {
            return access.GetAllDvd();
        }

        /// <summary>
        /// getter sur la liste des revues
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public List<Revue> GetAllRevues()
        {
            return access.GetAllRevues();
        }

        /// <summary>
        /// getter sur les rayons
        /// </summary>
        /// <returns>Liste d'objets Rayon</returns>
        public List<Categorie> GetAllRayons()
        {
            return access.GetAllRayons();
        }

        /// <summary>
        /// getter sur les publics
        /// </summary>
        /// <returns>Liste d'objets Public</returns>
        public List<Categorie> GetAllPublics()
        {
            return access.GetAllPublics();
        }


        /// <summary>
        /// récupère les exemplaires d'une revue
        /// </summary>
        /// <param name="idDocuement">id de la revue concernée</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesRevue(string idDocuement)
        {
            return access.GetExemplairesRevue(idDocuement);
        }

        /// <summary>
        /// Crée un exemplaire d'une revue dans la bdd
        /// </summary>
        /// <param name="exemplaire">L'objet Exemplaire concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerExemplaire(Exemplaire exemplaire)
        {
            return access.CreerExemplaire(exemplaire);
        }

        //Méthode qui envoi un livre 
        public bool CreerLivre(Livre unLivre)
        {
            return access.CreerLivre(unLivre);
        }
        //Méthode qui supprime un livre
        public bool SupprimerLivre(Livre unLivre)
        {
            return access.SupprimerLivre(unLivre);
        }
        //Méthode qui modifie un livre
        public bool ModifierLivre(Livre unLivre)
        {
            return access.ModifierLivre(unLivre);
        }
        //Méthode qui envoi un dvd
        public bool CreerDvd(Dvd unDvd)
        {
            return access.CreerDvd(unDvd);
        }
        //Méthode qui supprimer un dvd
        public bool SupprimerDvd(Dvd unDvd)
        {
            return access.SupprimerDvd(unDvd);
        }
        //Méthode qui modifie un dvd
        public bool ModifierDvd(Dvd unDvd)
        {
            return access.ModifierDvd(unDvd);
        }
        //Méthode qui envoi une revue
        public bool CreerRevue(Revue uneRevue)
        {
            return access.CreerRevue(uneRevue);
        }
        //Méthode qui supprime une revue
        public bool SupprimerRevue(Revue uneRevue)
        {
            return access.SupprimerRevue(uneRevue);
        }
        //Méthode qui modifie une revue
        public bool ModifierRevue(Revue uneRevue)
        {
            return access.ModifierRevue(uneRevue);
        }
        //Charge la liste de suivi
        public List<Categorie> GetSuivi()
        {
            return access.GetSuivi();
        }
        //retourne les commandes de livre
        public List<CommandeDocument> GetAllCommandeLivres()
        {
            return access.GetAllCommandeLivre();
        }
        //Ajoute une commande de livre ou dvd
        public bool CreerCmd(CommandeDocument uneCommande)
        {
            return access.CreerCmd(uneCommande);
        }
        //Suppression d'une commande de livre
        public bool SupprimerCmd(CommandeDocument uneCommande)
        {
            return access.SupprimerCmd(uneCommande);
        }
        //Modifie une commande de livre ou Dvd
        public bool ModifierCmd(CommandeDocument uneCommande)
        {
            return access.ModifierCmd(uneCommande);
        }
        //Retourne les commandes de dvd
        public List<CommandeDocument> GetAllCommandeDvds()
        {
            return access.GetAllCommandeDvds();
        }
        //Retourne les commandes de revues
        public List<Abonnement> GetAllCommandeRevues()
        {
            return access.GetAllCommandeRevues();
        }
        //Ajoute une commande de revue
        public bool CreerCmdRevue(Abonnement unAbonnement)
        {
            return access.CreerCmdRevue(unAbonnement);
        }
        //Supprime une commande de revue
        public bool SupprimerCmdRevue(Abonnement unAbonnement)
        {
            return access.SupprimerCmdRevue(unAbonnement);
        }
        //GetAllEtat
        public List<Etat> GetAllEtats()
        {
            return access.GetAllEtats();
        }
        public bool ModifierExemplaire(Exemplaire unExemplaire)
        {
            return access.ModifierExemplaire(unExemplaire);
        }
        public bool SupprimerExemplaire(Exemplaire unExemplaire)
        {
            return access.SupprimerExemplaire(unExemplaire);
        }
    }
}
