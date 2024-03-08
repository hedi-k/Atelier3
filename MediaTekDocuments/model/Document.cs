
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Document (réunit les infomations communes à tous les documents : Livre, Revue, Dvd)
    /// </summary>
    public class Document
    {
        /// <summary>
        /// Représente l'id du document
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// Représente le titre du document
        /// </summary>
        public string Titre { get; }
        /// <summary>
        /// Représente l'image du document (son chemin)
        /// </summary>
        public string Image { get; }
        /// <summary>
        /// Représente l'id du genre du document
        /// </summary>
        public string IdGenre { get; }
        /// <summary>
        /// Représente le labelle du genre du document
        /// </summary>
        public string Genre { get; }
        /// <summary>
        /// Représente l'id public du document
        /// </summary>
        public string IdPublic { get; }
        /// <summary>
        /// Représente le labelle du public du document
        /// </summary>
        public string Public { get; }
        /// <summary>
        /// Représente l'id du rayon du document
        /// </summary>
        public string IdRayon { get; }
        /// <summary>
        /// Représente le labelle du rayon du document
        /// </summary>
        public string Rayon { get; }
        /// <summary>
        /// Objet document
        /// </summary>
        /// <param name="id">id du document</param>
        /// <param name="titre">titre du document</param>
        /// <param name="image">image du document</param>
        /// <param name="idGenre">id du genre du document</param>
        /// <param name="genre">labelle du genre du document</param>
        /// <param name="idPublic">id du public du document</param>
        /// <param name="lePublic">labelle du public du document</param>
        /// <param name="idRayon">id du rayon du document</param>
        /// <param name="rayon">rayon du document</param>
        public Document(string id, string titre, string image, string idGenre, string genre, string idPublic, string lePublic, string idRayon, string rayon)
        {
            Id = id;
            Titre = titre;
            Image = image;
            IdGenre = idGenre;
            Genre = genre;
            IdPublic = idPublic;
            Public = lePublic;
            IdRayon = idRayon;
            Rayon = rayon;
        }
    }
}
