﻿
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Public (public concerné par le document) hérite de Categorie
    /// </summary>
    public class Public : Categorie
    {
        /// <summary>
        /// Objet public
        /// </summary>
        /// <param name="id">id du public</param>
        /// <param name="libelle">nnom du public</param>
        public Public(string id, string libelle) : base(id, libelle)
        {
        }

    }
}
