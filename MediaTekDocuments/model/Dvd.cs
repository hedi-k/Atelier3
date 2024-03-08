
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Dvd hérite de LivreDvd : contient des propriétés spécifiques aux dvd
    /// </summary>
    public class Dvd : LivreDvd
    {
        /// <summary>
        ///Représente la durée du dvd
        /// </summary>
        public int Duree { get; }
        /// <summary>
        /// Représente le nom du réalsateur du dvd
        /// </summary>
        public string Realisateur { get; }
        /// <summary>
        /// Représente le synopsis du dvd
        /// </summary>
        public string Synopsis { get; }
        /// <summary>
        /// L'objet dvd
        /// </summary>
        /// <param name="id">id du dvd</param>
        /// <param name="titre">titre du dvd</param>
        /// <param name="image">image du dvd</param>
        /// <param name="duree">duréer du dvd</param>
        /// <param name="realisateur">nom du réalisateur du dvd</param>
        /// <param name="synopsis">synopsis du dvd</param>
        /// <param name="idGenre">id du genre du dvd</param>
        /// <param name="genre">labelle du genre du dvd</param>
        /// <param name="idPublic">id du public du dvd</param>
        /// <param name="lePublic">labelle du public du dvd</param>
        /// <param name="idRayon">id du rayon du dvd</param>
        /// <param name="rayon">labelle du rayon du dvd</param>
        public Dvd(string id, string titre, string image, int duree, string realisateur, string synopsis,
            string idGenre, string genre, string idPublic, string lePublic, string idRayon, string rayon)
            : base(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon)
        {
            this.Duree = duree;
            this.Realisateur = realisateur;
            this.Synopsis = synopsis;
        }

    }
}
