
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier (abstraite) LivreDvd hérite de Document
    /// </summary>
    public abstract class LivreDvd : Document
    {
        /// <summary>
        /// Objet livreDVD
        /// </summary>
        /// <param name="id">id du document</param>
        /// <param name="titre">titre du document</param>
        /// <param name="image">chemin de l image du document</param>
        /// <param name="idGenre">id du genre du document</param>
        /// <param name="genre">nom du genre du document</param>
        /// <param name="idPublic">id du pblic du document</param>
        /// <param name="lePublic">nom du public du document</param>
        /// <param name="idRayon">id du rayon du document</param>
        /// <param name="rayon">nom du rayon du document</param>
        protected LivreDvd(string id, string titre, string image, string idGenre, string genre,
            string idPublic, string lePublic, string idRayon, string rayon)
            : base(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon)
        {
        }

    }
}
