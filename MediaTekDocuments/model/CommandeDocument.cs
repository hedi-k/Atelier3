using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    /// <summary>
    ///  Classe métier CommandeDocument hérite de Commande : contient des propriétés spécifiques aux commandes de documents
    /// </summary>
    public class CommandeDocument : Commande 
    {
        /// <summary>
        /// Représente le nombre d'exemplaire
        /// </summary>
        public int NbExemplaire { get; }
        /// <summary>
        /// Représente d'id suivi d'une commande
        /// </summary>
        public string IdSuivi { get; }
        /// <summary>
        /// Représente le labelle de suivi d'une commande
        /// </summary>
        public string Suivi { get; }
        /// <summary>
        /// représente l'id du document
        /// </summary>
        public string IdLivreDvd { get; }
        /// <summary>
        /// Objet commande document
        /// </summary>
        /// <param name="id">id du document</param>
        /// <param name="dateCommande">date de la commande du document</param>
        /// <param name="montant">montant de la commande du document</param>
        /// <param name="nbExemplaire">nombre d exemplaire de la commande</param>
        /// <param name="idSuivi">id de suivi de la commande</param>
        /// <param name="suivi">labelle du suivi de la commande</param>
        /// <param name="idLivreDvd">id du document</param>
        public CommandeDocument(string id, DateTime dateCommande, double montant, int nbExemplaire, string idSuivi, string suivi, string idLivreDvd )
            : base(id, dateCommande, montant)
        {
            NbExemplaire = nbExemplaire;
            IdSuivi = idSuivi;
            Suivi = suivi;
            IdLivreDvd = idLivreDvd;
        }
    }
}
