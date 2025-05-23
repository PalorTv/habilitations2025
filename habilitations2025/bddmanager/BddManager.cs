﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace habilitations2025.bddmanager
{
    /// <summary>
    /// Singleton : connexion à la base de données et exécution des requêtes
    /// </summary>
    public class BddManager
    {
        /// <summary>
        /// instance unique de la classe
        /// </summary>
        private static BddManager instance = null;

        /// <summary>
        /// objet de connexion à la BDD à partir d'une chapine de connexion
        /// </summary>
        private readonly MySqlConnection connection;

        /// <summary>
        /// Constructeur pour créer la connexion à la BDD et l'ouvrir
        /// </summary>
        /// <param name="stringConnect"> chaîne de connexion </param>

        private BddManager(string stringConnect)
        {
            connection = new MySqlConnection(stringConnect);
            connection.Open();
        }

        /// <summary>
        /// Création d'une seule instance de la classe
        /// </summary>
        /// <param name="stringConnect"> chaîne de connexion </param>
        /// <returns> instance unique de la classe </returns>
        public static BddManager GetInstance(string stringConnect)
        {
            if (instance == null)
            {
                instance = new BddManager(stringConnect);
            }
            return instance;
        }

        public void ReqUpdate(string stringQuery, Dictionary<string, object> parameters = null)
        {
            MySqlCommand command = new MySqlCommand(stringQuery, connection);
            if (!(parameters is null))
            {
                foreach (KeyValuePair<string, object> parameter in parameters)
                {
                    command.Parameters.Add(new MySqlParameter(parameter.Key, parameter.Value));
                }
            }
            command.Prepare();
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Execution d'une requête de type "select"
        /// </summary>
        /// <param name="stringQuery">requête select</param>
        /// <param name="parameters">dictoinnaire contenant les parametres</param>
        /// <returns>liste de tableaux d'objets contenant les valeurs des colonnes</returns>
        public List<Object[]> ReqSelect(string stringQuery, Dictionary<string, object> parameters = null)
        {
            MySqlCommand command = new MySqlCommand(stringQuery, connection);
            if (!(parameters is null))
            {
                foreach (KeyValuePair<string, object> parameter in parameters)
                {
                    command.Parameters.Add(new MySqlParameter(parameter.Key, parameter.Value));
                }
            }
            command.Prepare();
            MySqlDataReader reader = command.ExecuteReader();
            int nbCols = reader.FieldCount;
            List<Object[]> records = new List<Object[]>();
            while (reader.Read())
            {
                Object[] attributs = new object[nbCols];
                reader.GetValues(attributs);
                records.Add(attributs);
            }
            reader.Close();
            return records;
        }
    }
}
