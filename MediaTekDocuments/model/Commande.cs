using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    /// <summary>
    ///  Classe métier Commande : contient des propriétés spécifiques aux commandes
    /// </summary>
    public class Commande
    {
        /// <summary>
        /// Représente l'id d'une commande
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// Représente la date d'une commande
        /// </summary>
        public DateTime DateCommande { get; }
        /// <summary>
        /// Représente le montant d'une commande
        /// </summary>
        public double Montant { get; } //type double imposé par la BDD
        /// <summary>
        /// Objet commande
        /// </summary>
        /// <param name="id">id de la commande</param>
        /// <param name="dateCommande">date de la commande</param>
        /// <param name="montant">montant de la commande</param>
        public Commande(string id, DateTime dateCommande, double montant) 
        {
            Id = id;
            DateCommande = dateCommande;
            Montant = montant;

        }
    }
}
