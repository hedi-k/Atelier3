
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Genre : hérite de Categorie
    /// </summary>
    public class Genre : Categorie
    {
        /// <summary>
        /// Objet genre
        /// </summary>
        /// <param name="id">id du genre</param>
        /// <param name="libelle">nom du genre</param>
        public Genre(string id, string libelle) : base(id, libelle)
        {
        }

    }
}
