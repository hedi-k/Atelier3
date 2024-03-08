using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Suivi hérite de Commande : contient des propriétés spécifiques aux suivis
    /// </summary>
    public class Suivi : Categorie
    {
        /// <summary>
        /// Objet suivi
        /// </summary>
        /// <param name="id">id de suivi</param>
        /// <param name="libelle">nom du suivi</param>
        public Suivi(string id, string libelle) : base(id, libelle)
        {
        }
    }
}
