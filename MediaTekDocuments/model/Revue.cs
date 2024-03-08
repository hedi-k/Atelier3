
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Revue hérite de Document : contient des propriétés spécifiques aux revues
    /// </summary>
    public class Revue : Document
    {
        /// <summary>
        /// Représente la periodicite d'une revue
        /// </summary>
        public string Periodicite { get; set; }
        /// <summary>
        /// Représente le delai de mise a disposition d'une revue
        /// </summary>
        public int DelaiMiseADispo { get; set; }
        /// <summary>
        /// Objet revue
        /// </summary>
        /// <param name="id">id de la revue</param>
        /// <param name="titre">titre de la revue</param>
        /// <param name="image">chemin de l'image de la revue</param>
        /// <param name="idGenre">id du genre de la revue</param>
        /// <param name="genre">nom du genre de la revue</param>
        /// <param name="idPublic">id du public de la reuve</param>
        /// <param name="lePublic">nom du public de la revue</param>
        /// <param name="idRayon">id du rayon de la revue</param>
        /// <param name="rayon">nom du rayon de la revue</param>
        /// <param name="periodicite">périodicité de la revue</param>
        /// <param name="delaiMiseADispo">delais de mis a disposition de la revue</param>
        public Revue(string id, string titre, string image, string idGenre, string genre,
            string idPublic, string lePublic, string idRayon, string rayon,
            string periodicite, int delaiMiseADispo)
             : base(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon)
        {
            Periodicite = periodicite;
            DelaiMiseADispo = delaiMiseADispo;
        }

    }
}
