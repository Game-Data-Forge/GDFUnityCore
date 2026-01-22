using System.Collections.Generic;
using System.Reflection;
using System.Text;
using GDFFoundation;

namespace GDFUnity
{
    public class PlayerDataDAL : SQLiteDAL<PlayerDataDAL, GDFPlayerDataStorage>
    {
        private PropertyInfo _reference;

        public PlayerDataDAL()
        {
            _reference = _tableType.GetProperty(nameof(GDFPlayerDataStorage.Reference));
            _properties = new List<PropertyInfo>()
            {
                _reference,
                _tableType.GetProperty(nameof(GDFPlayerDataStorage.Json)),
                _tableType.GetProperty(nameof(GDFPlayerDataStorage.Channels)),
                _tableType.GetProperty(nameof(GDFPlayerDataStorage.ClassName)),
                _tableType.GetProperty(nameof(GDFPlayerDataStorage.DataVersion)),
                _tableType.GetProperty(nameof(GDFPlayerDataStorage.Creation)),
                _tableType.GetProperty(nameof(GDFPlayerDataStorage.Modification)),
                _tableType.GetProperty(nameof(GDFPlayerDataStorage.SyncDateTime)),
                _tableType.GetProperty(nameof(GDFPlayerDataStorage.Trashed))
            };
        }

        public void Validate(IJobHandler handler, IDBConnection connection)
        {
            ValidateTable(handler, connection);
        }

        public void Get(IJobHandler handler, IDBConnection connection, List<GDFPlayerDataStorage> data)
        {
            Select(handler, connection, data);
        }

        public void Record(IJobHandler handler, IDBConnection connection, List<GDFPlayerDataStorage> data)
        {
            InsertOrUpdate(handler, connection, data);
        }

        protected override string GenerateTableName()
        {
            StringBuilder tableName = new StringBuilder(_tableType.Name);
            return tableName.ToString();
        }

        protected override string GenerateCreateTable(string tableName)
        {
            StringBuilder query = new StringBuilder();
            query.Append("CREATE TABLE `");
            query.Append(tableName);
            query.Append("` (");

            foreach (PropertyInfo property in _properties)
            {
                query.Append(SQLiteType.Get(property.PropertyType).CreateColumn(property.Name) + ",");
            }
            
            query.Append("PRIMARY KEY(`");
            query.Append(nameof(GDFPlayerDataStorage.Reference));
            query.Append("`))");

            return query.ToString();
        }

        protected override void ProcessUpdate(IDBConnection connection, GDFPlayerDataStorage data, string tableName)
        {
            int index = 1;
            using (SQLiteDbRequest request = connection.GetRequest<SQLiteDbRequest>())
            {
                if (data.Trashed)
                {
                    request.Open(Delete(tableName, data));
                    SQLiteType.Get(_reference.PropertyType).BindParamter(request, index++, _reference.GetValue(data));
                }
                else
                {
                    request.Open(InsertOrUpdate(tableName, data));
                    foreach (PropertyInfo property in _properties)
                    {
                        SQLiteType.Get(property.PropertyType).BindParamter(request, index++, property.GetValue(data));
                    }
                }
                
                try
                {
                    request.Exec();
                }
                catch
                {
                    throw SQLiteException.WriteError(request.connection, request.GetResult());
                }
            }
        }

        protected override void ProcessDelete(IDBConnection connection, GDFPlayerDataStorage data, string tableName)
        {
            
        }
    }
}
