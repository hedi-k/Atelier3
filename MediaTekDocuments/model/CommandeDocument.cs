using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    public class CommandeDocument : Commande 
    {
        public int NbExemplaire { get; }
        public string IdSuivi { get; }
        public string Suivi { get; }
        public string IdLivreDvd { get; }

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
