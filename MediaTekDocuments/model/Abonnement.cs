using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Abonnement hérite de Commande : contient des propriétés spécifiques aux abonnements
    /// </summary>
    public class Abonnement : Commande
    {
        /// <summary>
        /// Représente la date de fin de l'abonnement
        /// </summary>
        public DateTime DateFinAbonnement { get; }
        /// <summary>
        /// Représente l'id de la revue
        /// </summary>
        public string IdRevue { get; }
        /// <summary>
        /// Objet Abonnement
        /// </summary>
        /// <param name="id">id de l'abonnement</param>
        /// <param name="dateCommande">date de commande de l abonnement</param>
        /// <param name="montant">montant de l'abonnement</param>
        /// <param name="dateFinAbonnement">date de fin de l abonnement</param>
        /// <param name="idRevue">id de la revue de l abonnement</param>
        public Abonnement(string id, DateTime dateCommande, double montant, DateTime dateFinAbonnement, string idRevue)
            : base(id, dateCommande, montant)
        {
            DateFinAbonnement = dateFinAbonnement;
            IdRevue = idRevue;
        }

    }
}
