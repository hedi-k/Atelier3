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

        //Cette méthode envoi à l acces livre, dvd et Revue.
        //En C#, T est un paramètre de type générique.
        //Ce qui implique quelle va traiter indiférement un objet Livre/Dvd/revue
        public bool EnvoiDocuments<T>(T unDocument)
        {
            return access.CreerDocument(unDocument);
        }
        //Modifie un livre, un dvd ou une revue
        //En C#, T est un paramètre de type générique.
        //Ce qui implique quelle va traiter indiférement un objet Livre/Dvd/revue
        public bool ModifierDocuments<T>(T unDocument)
        {
            return access.ModifierDocument(unDocument);
        }
        //Supprime un livre, dvd ou revue
        //En C#, T est un paramètre de type générique.
        //Ce qui implique quelle va traiter indiférement un objet Livre/Dvd/revue
        public bool SupprimerDocument<T>(T unDocument)
        {
            return access.SupprimerDocument(unDocument);
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
        //Retourne les états
        public List<Etat> GetAllEtats()
        {
            return access.GetAllEtats();
        }
 

        
    }
}
