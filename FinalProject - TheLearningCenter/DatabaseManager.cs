using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheLearningCenterReworked.Context;

namespace TheLearningCenterReworked
{
    public class DatabaseManager
    {
        private static readonly TheLearningCenterReworkedEntities entities;

        // Initialize and open the database connection
        static DatabaseManager()
        {
            entities = new TheLearningCenterReworkedEntities();
            entities.Database.Connection.Open();
        }

        // Provide an accessor to the database
        public static TheLearningCenterReworkedEntities Instance
        {
            get
            {
                return entities;
            }
        }
    }
}