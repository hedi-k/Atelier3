
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Rayon (rayon de classement du document) hérite de Categorie
    /// </summary>
    public class Rayon : Categorie
    {
        /// <summary>
        /// objet rayon
        /// </summary>
        /// <param name="id">id du rayon</param>
        /// <param name="libelle">nom du rayon</param>
        public Rayon(string id, string libelle) : base(id, libelle)
        {
        }

    }
}
