using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Utilisateur, contient tout les paramètres liés aux utilisateurs
    /// </summary>
    public class Utilisateur
    {
        /// <summary>
        /// Représente l'id d'un utilisateur
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// Représente le nom d'un utilisateur
        /// </summary>
        public string Nom { get; }
        /// <summary>
        /// Représente le mot de passe du comtpe utilisateur
        /// </summary>
        public string Pwd { get; }
        /// <summary>
        /// Représente l'id du service de l'utilisateur
        /// </summary>
        public string IdService { get; }
        /// <summary>
        /// Objet utilsiteur
        /// </summary>
        /// <param name="id">id de l utilisateur</param>
        /// <param name="nom">nom de l utilisateur</param>
        /// <param name="pwd">mot de passe de l utilisateur</param>
        /// <param name="idService">id du service de l utilisateur</param>
        public Utilisateur (string id, string nom, string pwd, string idService)
        {
            Id = id;
            Nom = nom;
            Pwd = pwd;
            IdService = idService;
        }
    }
}
