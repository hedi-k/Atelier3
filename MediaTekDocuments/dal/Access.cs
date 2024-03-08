using System;
using System.Collections.Generic;
using MediaTekDocuments.model;
using MediaTekDocuments.manager;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Configuration;
using Serilog;
using Serilog.Formatting.Json;

namespace MediaTekDocuments.dal
{
    /// <summary>
    /// Classe d'accès aux données
    /// </summary>
    public class Access
    {
        /// <summary>
        /// adresse de l'API
        /// </summary>
        private static readonly string uriApi = "http://localhost/rest_mediatekdocuments/";
        /// <summary>
        /// instance unique de la classe
        /// </summary>
        private static Access instance = null;
        /// <summary>
        /// instance de ApiRest pour envoyer des demandes vers l'api et recevoir la réponse
        /// </summary>
        private readonly ApiRest api = null;
        /// <summary>
        /// méthode HTTP pour select
        /// </summary>
        private const string GET = "GET";
        /// <summary>
        /// méthode HTTP pour insert
        /// </summary>
        private const string POST = "POST";
        /// <summary>
        /// méthode HTTP pour delete
        /// </summary>
        private const string DELETE = "DELETE";
        /// <summary>
        /// méthode HTTP pour update
        /// </summary>
        private const string PUT = "PUT";
        /// <summary>
        /// paramètres de connexion
        /// </summary>
        private static readonly string connectionName = "MediaTekDocuments.Properties.Settings.mediatek86ConnectionString";


        /// <summary>
        /// Méthode privée pour créer un singleton
        /// initialise l'accès à l'API
        /// </summary>
        private Access()
        {
            String authenticationString;
            try
            {
                //Objet qui va gérer les logs
                Log.Logger = new LoggerConfiguration()
                    //Si aucun "MinimumLevel" n’est fixé, il est par défaut à "Information".
                    .MinimumLevel.Verbose()
                    //Log dans la console
                    .WriteTo.Console()
                    //Log dans un fichier txt
                    .WriteTo.File("logs/log.txt",
                    //Interval entre chaque nouveaux fichier de log
                    rollingInterval: RollingInterval.Day)
                    .CreateLogger();
                authenticationString = GetConnectionStringByName(connectionName);
                api = ApiRest.GetInstance(uriApi, authenticationString);
            }
            catch (Exception e)
            {
                //On remplace les "simples" console.Write par des logs
                Log.Fatal("Access.Access catch connectionString={0} erreur={1}", connectionName, e.Message);
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Création et retour de l'instance unique de la classe
        /// </summary>
        /// <returns>instance unique de la classe</returns>
        public static Access GetInstance()
        {
            if (instance == null)
            {
                instance = new Access();
            }
            return instance;
        }

        /// <summary>
        /// Retourne les paramètres de connexions
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        static string GetConnectionStringByName(string name)
        {
            string returnValue = null;
            //Récupération des paramètres de la chaîne de connexion à partir de la configuration de l'application
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[name];
            if (settings != null)
                returnValue = settings.ConnectionString;
            return returnValue;
        }

        /// <summary>
        /// Retourne tous les genres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Genre</returns>
        public List<Categorie> GetAllGenres()
        {
            IEnumerable<Genre> lesGenres = TraitementRecup<Genre>(GET, "genre");
            return new List<Categorie>(lesGenres);
        }

        /// <summary>
        /// Retourne tous les rayons à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Rayon</returns>
        public List<Categorie> GetAllRayons()
        {
            IEnumerable<Rayon> lesRayons = TraitementRecup<Rayon>(GET, "rayon");
            return new List<Categorie>(lesRayons);
        }

        /// <summary>
        /// Retourne toutes les catégories de public à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Public</returns>
        public List<Categorie> GetAllPublics()
        {
            IEnumerable<Public> lesPublics = TraitementRecup<Public>(GET, "public");
            return new List<Categorie>(lesPublics);
        }

        /// <summary>
        /// Retourne toutes les livres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public List<Livre> GetAllLivres()
        {
            List<Livre> lesLivres = TraitementRecup<Livre>(GET, "livre");
            return lesLivres;
        }

        /// <summary>
        /// Retourne toutes les dvd à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Dvd</returns>
        public List<Dvd> GetAllDvd()
        {
            List<Dvd> lesDvd = TraitementRecup<Dvd>(GET, "dvd");
            return lesDvd;
        }

        /// <summary>
        /// Retourne toutes les revues à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public List<Revue> GetAllRevues()
        {
            List<Revue> lesRevues = TraitementRecup<Revue>(GET, "revue");
            return lesRevues;
        }


        /// <summary>
        /// Retourne les exemplaires d'une revue
        /// </summary>
        /// <param name="idDocument">id de la revue concernée</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesRevue(string idDocument)
        {
            String jsonIdDocument = convertToJson("id", idDocument);
            List<Exemplaire> lesExemplaires = TraitementRecup<Exemplaire>(GET, "exemplaire/" + jsonIdDocument);
            return lesExemplaires;
        }

        /// <summary>
        /// ecriture d'un exemplaire en base de données
        /// </summary>
        /// <param name="exemplaire">exemplaire à insérer</param>
        /// <returns>true si l'insertion a pu se faire (retour != null)</returns>
        public bool CreerExemplaire(Exemplaire exemplaire)
        {
            String jsonExemplaire = JsonConvert.SerializeObject(exemplaire, new CustomDateTimeConverter());
            try
            {
                // récupération soit d'une liste vide (requête ok) soit de null (erreur)
                List<Exemplaire> liste = TraitementRecup<Exemplaire>(POST, "exemplaire/" + jsonExemplaire);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Log.Debug("Access.CreerExemplaire catch String jsonExemplaire={0} erreur={1}", jsonExemplaire, ex.Message);
            }
            return false;
        }


        /// <summary>
        /// Envoi l'objet en paramètre à l'API
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unDocument"></param>
        /// <returns></returns>
        public bool CreerDocument<T>(T unDocument)
        {
            //Convertit en json l'objet en paramètre
            String jsonCreerDocument = JsonConvert.SerializeObject(unDocument);
            try
            {
                //Appel TraitementRecup avec en paramètre POST, le nom du type d'objet et le json 
                List<Object> liste = TraitementRecup<Object>(POST, typeof(T).Name.ToLower() + "/" + jsonCreerDocument.Replace(" ", "-"));
                return (liste != null);
            }
            catch (Exception ex)
            {
                Log.Debug("Access.CreerDocument catch String jsonExemplaire={0} erreur={1}", jsonCreerDocument, ex.Message);
            }
            return false;
        }
        /// <summary>
        /// Envoi l'objet en paramètre à l'API pour modification
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unDocument"></param>
        /// <returns></returns>
        public bool ModifierDocument<T>(T unDocument)
        {
            //Convertit en json l'objet en paramètre
            String jsonModifierDocument = JsonConvert.SerializeObject(unDocument);
            try
            {
                //L'id est necessaire pour l'API en cas de modification
                string id = "";
                switch (unDocument)
                {
                    case Livre livre:
                        id = livre.Id;
                        break;
                    case Dvd dvd:
                        id = dvd.Id;
                        break;
                    case Revue revue:
                        id = revue.Id;
                        break;
                    case CommandeDocument uneCmd:
                        id = uneCmd.Id;
                        break;
                    case Exemplaire exemplaire:
                        id = exemplaire.Numero.ToString();// C'est in INT donc faut le convertir en string
                        break;
                }
                List<Object> liste = TraitementRecup<Object>(PUT, typeof(T).Name.ToLower() + "/" + id + "/" + jsonModifierDocument.Replace(" ", "-"));
                return (liste != null);
            }
            catch (Exception ex)
            {
                Log.Debug("Access.ModifierDocument catch String jsonModifierDocument={0} erreur={1}", jsonModifierDocument, ex.Message);
            }
            return false;
        }
        /// <summary>
        /// Supprime le document en paramètre
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unDocument"></param>
        /// <returns></returns>
        public bool SupprimerDocument<T>(T unDocument)
        {
            //Convertit en json l'objet en paramètre
            String jsonSupprimerDocument = JsonConvert.SerializeObject(unDocument);
            try
            {
                //L'id est necessaire pour l'API en cas de suppression
                string id = "", Id = "Id";
                switch (unDocument)
                {
                    case Livre livre:
                        id = livre.Id;
                        break;
                    case Dvd dvd:
                        id = dvd.Id;
                        break;
                    case Revue revue:
                        id = revue.Id;
                        break;
                    case CommandeDocument uneCmd:
                        id = uneCmd.Id;
                        break;
                    case Abonnement unAbo:
                        id = unAbo.Id;
                        break;
                    case Exemplaire exemplaire:
                        id = exemplaire.Numero.ToString();// C'est in INT donc faut le convertir en string
                        Id = "Numero";
                        break;
                }
                List<Object> liste = TraitementRecup<Object>(DELETE, typeof(T).Name.ToLower() + "/{\"" + Id + "\":\"" + id + "\"}");
                return (liste != null);
            }
            catch (Exception ex)
            {
                Log.Debug("Access.SupprimerDocument catch String jsonSupprimerDocument={0} erreur={1}", jsonSupprimerDocument, ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Retourne les Suivi pour la cbxSuivi
        /// </summary>
        /// <returns>Liste d'objets Suivi</returns>
        public List<Categorie> GetSuivi()
        {
            IEnumerable<Suivi> lesSuivi = TraitementRecup<Suivi>(GET, "suivi");
            return new List<Categorie>(lesSuivi);
        }
        /// <summary>
        /// Retourne la liste de commande de livre
        /// </summary>
        /// <returns>Liste d'objets Commande de livre</returns>
        public List<CommandeDocument> GetAllCommandeLivre()
        {
            List<CommandeDocument> lesCommandeLivre = TraitementRecup<CommandeDocument>(GET, "lesCommandeLivre");
            return lesCommandeLivre;
        }

        /// <summary>
        /// Retourne la liste de commande de DVD
        /// </summary>
        /// <returns>Liste d'objets commannde de dvd</returns>
        public List<CommandeDocument> GetAllCommandeDvds()
        {
            List<CommandeDocument> lesCommandeDvds = TraitementRecup<CommandeDocument>(GET, "lesCommandeDvds");
            return lesCommandeDvds;
        }

        /// <summary>
        /// Retourne la liste de commande de revue
        /// </summary>
        /// <returns>Liste d'objets commannde de revue</returns>
        public List<Abonnement> GetAllCommandeRevues()
        {
            List<Abonnement> lesCommandesRevues = TraitementRecup<Abonnement>(GET, "lesCommandeRevues");
            return lesCommandesRevues;
        }
        
        /// <summary>
        /// Retourne les etats
        /// </summary>
        /// <returns>Liste d'objets état</returns>
        public List<Etat> GetAllEtats()
        {
            List<Etat> lesEtats = TraitementRecup<Etat>(GET, "etat");
            return lesEtats;
        }
        
        /// <summary>
        /// Méthode pour l'authentification
        /// </summary>
        /// <param name="utilisateur"></param>
        /// <returns>Retourne l'utilisateur connecté</returns>
        public Utilisateur Authentification(Utilisateur utilisateur)
        {
            String jsonAuthentification = JsonConvert.SerializeObject(utilisateur);
            try
            {
                List<Utilisateur> liste = TraitementRecup<Utilisateur>(GET, "authentification/" + jsonAuthentification);
                if (liste != null)
                {
                    //Il y a qu'un utilisateur qui peut être retourné donc indice 0
                    return liste[0];
                }
            }
            catch (Exception ex)
            {
                Log.Debug("Access.Authentification catch String jsonAuthentification={0} erreur={1}", jsonAuthentification, ex.Message);
            }
            return null;
        }

        /// <summary>
        /// Traitement de la récupération du retour de l'api, avec conversion du json en liste pour les select (GET)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methode">verbe HTTP (GET, POST, PUT, DELETE)</param>
        /// <param name="message">information envoyée</param>
        /// <returns>liste d'objets récupérés (ou liste vide)</returns>
        private List<T> TraitementRecup<T>(String methode, String message)
        {
            Console.WriteLine("METHODE = " + methode + " MESSAGE = " + message);// affiche le traitement (pour postMAN)******************** test à effacer
            List<T> liste = new List<T>();
            try
            {
                JObject retour = api.RecupDistant(methode, message);
                // extraction du code retourné
                String code = (String)retour["code"];
                if (code.Equals("200"))
                {
                    // dans le cas du GET (select), récupération de la liste d'objets
                    if (methode.Equals(GET))
                    {
                        String resultString = JsonConvert.SerializeObject(retour["result"]);
                        // construction de la liste d'objets à partir du retour de l'api
                        liste = JsonConvert.DeserializeObject<List<T>>(resultString, new CustomBooleanJsonConverter());
                    }
                }
                else
                {
                    Console.WriteLine("code erreur = " + code + " message = " + (String)retour["message"]);
                }
            }
            catch (Exception ex)
            {
                Log.Debug("Access.TraitementRecup catch String methode={0} String message={1} erreur={2}", methode, message, ex.Message);
                Environment.Exit(0);
            }
            return liste;
        }

        /// <summary>
        /// Convertit en json un couple nom/valeur
        /// </summary>
        /// <param name="nom"></param>
        /// <param name="valeur"></param>
        /// <returns>couple au format json</returns>
        private String convertToJson(Object nom, Object valeur)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<Object, Object>();
            dictionary.Add(nom, valeur);
            return JsonConvert.SerializeObject(dictionary);
        }

        /// <summary>
        /// Modification du convertisseur Json pour gérer le format de date
        /// </summary>
        private sealed class CustomDateTimeConverter : IsoDateTimeConverter
        {
            public CustomDateTimeConverter()
            {
                base.DateTimeFormat = "yyyy-MM-dd";
            }
        }

        /// <summary>
        /// Modification du convertisseur Json pour prendre en compte les booléens
        /// classe trouvée sur le site :
        /// https://www.thecodebuzz.com/newtonsoft-jsonreaderexception-could-not-convert-string-to-boolean/
        /// </summary>
        private sealed class CustomBooleanJsonConverter : JsonConverter<bool>
        {
            public override bool ReadJson(JsonReader reader, Type objectType, bool existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                return Convert.ToBoolean(reader.ValueType == typeof(string) ? Convert.ToByte(reader.Value) : reader.Value);
            }

            public override void WriteJson(JsonWriter writer, bool value, JsonSerializer serializer)
            {
                serializer.Serialize(writer, value);
            }
        }
    }
}
