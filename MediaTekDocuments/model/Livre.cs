
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Livre hérite de LivreDvd : contient des propriétés spécifiques aux livres
    /// </summary>
    public class Livre : LivreDvd
    {
        /// <summary>
        /// Représente l'idbn d'un livre
        /// </summary>
        public string Isbn { get; }
        /// <summary>
        /// Représente l'autheur d'un livre
        /// </summary>
        public string Auteur { get; }
        /// <summary>
        /// Représente la collection d'un livre
        /// </summary>
        public string Collection { get; }
        /// <summary>
        /// Objet livre
        /// </summary>
        /// <param name="id">id du livre</param>
        /// <param name="titre">titre du livre</param>
        /// <param name="image">image du livre (son chemin)</param>
        /// <param name="isbn">isbn du livre</param>
        /// <param name="auteur">autheur du livre</param>
        /// <param name="collection">collection du livre</param>
        /// <param name="idGenre">id du genre du livre</param>
        /// <param name="genre">nom du genre du livre</param>
        /// <param name="idPublic">id du public du livre</param>
        /// <param name="lePublic">nom du public du livre</param>
        /// <param name="idRayon">id du rayon du livre</param>
        /// <param name="rayon">nom du rayon du livre</param>
        public Livre(string id, string titre, string image, string isbn, string auteur, string collection,
            string idGenre, string genre, string idPublic, string lePublic, string idRayon, string rayon)
            : base(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon)
        {
            this.Isbn = isbn;
            this.Auteur = auteur;
            this.Collection = collection;
        }



    }
}
