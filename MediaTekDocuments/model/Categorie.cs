
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Categorie (réunit les informations des classes Public, Genre et Rayon)
    /// </summary>
    public class Categorie
    {
        /// <summary>
        /// Représente l'id d'une catégorie
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// Représente le libelle d'une catégorie
        /// </summary>
        public string Libelle { get; }
        /// <summary>
        /// Objet catégorie
        /// </summary>
        /// <param name="id">id de la catégorie</param>
        /// <param name="libelle">libelle de la catégorie</param>
        public Categorie(string id, string libelle)
        {
            this.Id = id;
            this.Libelle = libelle;
        }

        /// <summary>
        /// Récupération du libellé pour l'affichage dans les combos
        /// </summary>
        /// <returns>Libelle</returns>
        public override string ToString()
        {
            return this.Libelle;
        }

    }
}
