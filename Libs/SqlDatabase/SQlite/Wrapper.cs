using JB.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.Sqlite;
using System.Reflection;
using JB.SqlDatabase.Attributes;
using JB.SqlDatabase.SQlite.Interfaces;

namespace JB.SqlDatabase.SQlite {
    internal class Wrapper : IWrapper {
        public Task<IReturnCode<bool>> CreateDatabase(string pDatabaseName) {
            IReturnCode<bool> rc = new ReturnCode<bool>();

            try {
                if (rc.Success) {
                    using (FileStream dbFile = File.Create(pDatabaseName)) {
                        dbFile.Close();
                    }
                }
            }
            catch (Exception ex) {
                rc.ErrorCode = ErrorCodes.CREATE_DATABASE_FAILED;
                rc.Errors.Add(new Error(rc.ErrorCode, ex));
            }

            return Task.FromResult(rc);
        }
        public async Task<IReturnCode<bool>> CreateTable<T>(string pDatabaseName, string pTableName) {
            IReturnCode<bool> rc = new ReturnCode<bool>();
            SqliteConnection connection = CreateConnection(pDatabaseName);

            try {
                if (rc.Success) {
                    await connection.OpenAsync();

                    if (connection.State != ConnectionState.Open) {
                        rc.ErrorCode = ErrorCodes.UNABLE_TO_OPEN_DATA_BASE;
                        rc.Errors.Add(new Error(rc.ErrorCode));
                    }
                }

                if (rc.Success) {
                    IReturnCode<bool> createTableRc = await CreateTable(connection, pTableName, typeof(T));

                    if (createTableRc.Failed) {
                        ErrorWorker.CopyErrors(createTableRc, rc);
                    }
                }
            }
            catch (Exception ex) {
                rc.ErrorCode = ErrorCodes.CREATE_TABLE_FAILED;
                rc.Errors.Add(new Error(rc.ErrorCode, ex));
            }

            return rc;
        }
        public async Task<IReturnCode<bool>> DeleteTable(string pDatabaseName, string pTableName) {
            IReturnCode<bool> rc = new ReturnCode<bool>();
            SqliteConnection connection = CreateConnection(pDatabaseName);

            try {
                if (rc.Success) {
                    await connection.OpenAsync();

                    if (connection.State != ConnectionState.Open) {
                        rc.ErrorCode = ErrorCodes.UNABLE_TO_OPEN_DATA_BASE;
                        rc.Errors.Add(new Error(rc.ErrorCode));
                    }
                }

                if (rc.Success) {
                    var command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = $"DROP TABLE {pTableName};";
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex) {
                rc.ErrorCode = ErrorCodes.DELETE_TABLE_FAILED;
                rc.Errors.Add(new Error(rc.ErrorCode, ex));
            }

            return rc;
        }


        public async Task<IReturnCode<SqlDatabase.Interfaces.IDataReader>> RunQuery(string pDatabaseName, string pQuery) {
            IReturnCode<SqlDatabase.Interfaces.IDataReader> rc = new ReturnCode<SqlDatabase.Interfaces.IDataReader>();
            SqlDatabase.Interfaces.IDataReader? dataReader = null;
            SqliteConnection connection = CreateConnection(pDatabaseName); ;

            try {
                if (rc.Success) {
                    await connection.OpenAsync();

                    if (connection.State != ConnectionState.Open) {
                        rc.ErrorCode = ErrorCodes.UNABLE_TO_OPEN_DATA_BASE;
                        rc.Errors.Add(new Error(rc.ErrorCode));
                    }
                }

                if (rc.Success) {
                    SqliteCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = pQuery;

                    dataReader = new Models.DataReader(await command.ExecuteReaderAsync());
                }
            }
            catch(Exception ex) {
                rc.ErrorCode = ErrorCodes.RUN_QUERY_FAILED;
                rc.Errors.Add(new Error(rc.ErrorCode, ex));
            }

            if (rc.Success) {
                rc.Data = dataReader;
            }
            
            return rc;
        }
        public Task<IReturnCode<SqlDatabase.Interfaces.IDataReader>> RunStoredProcedure(string pDatabaseName, string pStoreProcedureName, IDictionary<string, object?> pParameters) {
            throw new NotImplementedException();
        }


        public async Task<IReturnCode<IList<T>>> Get<T>(string pDatabaseName, string pTableName, string? pQueryParameters = null) {
            IReturnCode<IList<T>> rc = new ReturnCode<IList<T>>();
            SqliteConnection connection = CreateConnection(pDatabaseName);
            IList<T> itemsList = new List<T>();

            try {
                if (rc.Success) {
                    await connection.OpenAsync();

                    if (connection.State != ConnectionState.Open) {
                        rc.ErrorCode = ErrorCodes.UNABLE_TO_OPEN_DATA_BASE;
                        rc.Errors.Add(new Error(rc.ErrorCode));
                    }
                }

                if (rc.Success) {
                    IReturnCode<IList<object?>> getObjectsRc = await Get(connection, pTableName, typeof(T), pQueryParameters);

                    if (getObjectsRc.Success) {
                        foreach(object? obj in getObjectsRc.Data!) {
                            if (obj is T) {
                                itemsList.Add((T)obj);
                            }
                        }
                    }
                    if (getObjectsRc.Failed) {
                        ErrorWorker.CopyErrors(getObjectsRc, rc);
                    }
                }
            }
            catch (Exception ex) {
                rc.ErrorCode = ErrorCodes.GET_DATA_FAILED;
                rc.Errors.Add(new Error(rc.ErrorCode, ex));
            }

            if (rc.Success) {
                rc.Data = itemsList;
            }

            return rc;
        }
        public async Task<IReturnCode<IList<Tinterface>>> Get<Tinterface, Tmodel>(string pDatabaseName, string pTableName, string? pQueryParameters = null) where Tmodel : Tinterface {
            IReturnCode<IList<Tinterface>> rc = new ReturnCode<IList<Tinterface>>();
            SqliteConnection connection = CreateConnection(pDatabaseName);
            IList<Tinterface> itemsList = new List<Tinterface>();

            try {
                if (rc.Success) {
                    await connection.OpenAsync();

                    if (connection.State != ConnectionState.Open) {
                        rc.ErrorCode = ErrorCodes.UNABLE_TO_OPEN_DATA_BASE;
                        rc.Errors.Add(new Error(rc.ErrorCode));
                    }
                }

                if (rc.Success) {
                    IReturnCode<IList<object?>> getObjectsRc = await Get(connection, pTableName, typeof(Tmodel), pQueryParameters);

                    if (getObjectsRc.Success) {
                        foreach (object? obj in getObjectsRc.Data!) {
                            if (obj is Tmodel) {
                                itemsList.Add((Tmodel)obj!);
                            }
                        }
                    }
                    if (getObjectsRc.Failed) {
                        ErrorWorker.CopyErrors(getObjectsRc, rc);
                    }
                }
            }
            catch (Exception ex) {
                rc.ErrorCode = ErrorCodes.GET_DATA_FAILED;
                rc.Errors.Add(new Error(rc.ErrorCode, ex));
            }

            if (rc.Success) {
                rc.Data = itemsList;
            }

            return rc;
        }
        public async Task<IReturnCode<T>> Insert<T>(string pDatabaseName, string pTableName, T pItem) {
            IReturnCode<T> rc = new ReturnCode<T>();
            SqliteConnection connection = CreateConnection(pDatabaseName);

            try {
                if (rc.Success) {
                    await connection.OpenAsync();

                    if (connection.State != ConnectionState.Open) {
                        rc.ErrorCode = ErrorCodes.UNABLE_TO_OPEN_DATA_BASE;
                        rc.Errors.Add(new Error(rc.ErrorCode));
                    }
                }

                if (rc.Success) {
                    IReturnCode<object?> getObjectsRc = await Insert(connection, pTableName, typeof(T), pItem);

                    if (getObjectsRc.Success) {
                        pItem = (T)getObjectsRc.Data!;
                    }
                    if (getObjectsRc.Failed) {
                        ErrorWorker.CopyErrors(getObjectsRc, rc);
                    }
                }
            }
            catch (Exception ex) {
                rc.ErrorCode = ErrorCodes.INSERT_DATA_FAILED;
                rc.Errors.Add(new Error(rc.ErrorCode, ex));
            }

            if (rc.Success) {
                rc.Data = pItem;
            }

            return rc;
        }
        public async Task<IReturnCode<T>> Update<T>(string pDatabaseName, string pTableName, T pItem, string pQueryParameters) {
            IReturnCode<T> rc = new ReturnCode<T>();
            SqlDatabase.Interfaces.IDataReader? dataReader = null;
            SqliteConnection connection = CreateConnection(pDatabaseName);
            StringBuilder queryBuilder = new StringBuilder();
            IList<IObjectProperty> dataMap = new List<IObjectProperty>();

            try {
                if (rc.Success) {
                    await connection.OpenAsync();

                    if (connection.State != ConnectionState.Open) {
                        rc.ErrorCode = ErrorCodes.UNABLE_TO_OPEN_DATA_BASE;
                        rc.Errors.Add(new Error(rc.ErrorCode));
                    }
                }

                if (rc.Success) {
                    IReturnCode<IList<IObjectProperty>> getValuesRc = Worker.GetObjectProperties(pItem);

                    if (getValuesRc.Success) {
                        dataMap = getValuesRc.Data!;
                    }

                    if (getValuesRc.Failed) {
                        ErrorWorker.CopyErrors(getValuesRc, rc);
                    }
                }

                if (rc.Success) {
                    queryBuilder.Append($"UPDATE {pTableName} SET ");

                    for(int p = 0; p < dataMap.Count; p++) {
                        IObjectProperty prop = dataMap[p];
                        if (prop.Attributes.Where(x => x.AttributeType == typeof(Attributes.IgnoreAttribute)).FirstOrDefault() != null || prop.Value == null) {
							continue;
						}
						
                        if (p != 0) {
							queryBuilder.Append(", ");
						}

						if (prop.Value?.GetType() == typeof(string)) {
							queryBuilder.Append($"{prop.Name} = '{prop.Value}'");
						}
						else if (prop.Value?.GetType().IsEnum == true) {
							queryBuilder.Append($"{prop.Name} = '{(int)prop.Value}'");
						}
						else if (prop.Value?.GetType().IsClass == true) {
							CustomAttributeData? tableAttribute = prop.Attributes.Where(x => x.AttributeType == typeof(TableAttribute)).FirstOrDefault();
							string? columnName = tableAttribute?.NamedArguments[1].TypedValue.Value as string;

							var getSubObjectProperties = Worker.GetObjectProperties(prop.Value);
							
							if (getSubObjectProperties.Success) {
								foreach(var subProp in getSubObjectProperties.Data!) {
									if (subProp.Name.Equals(columnName, StringComparison.CurrentCultureIgnoreCase)) {
										queryBuilder.Append($"{prop.Name} = '{subProp.Value}'");
										break;
									}
								}
							}
							if (getSubObjectProperties.Failed) {
								ErrorWorker.CopyErrors(getSubObjectProperties, rc);
							}
						}
						else {
							queryBuilder.Append($"{prop.Name} = {prop.Value}");
						}
					}

                    queryBuilder.Append($" WHERE {pQueryParameters};");
                }

                if (rc.Success) {
                    SqliteCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = queryBuilder.ToString();

                    dataReader = new Models.DataReader(await command.ExecuteReaderAsync());


                    if (dataReader.RowsAffected() == 0) {
                        rc.ErrorCode = ErrorCodes.NO_ROWS_AFFECTED;
                        rc.Errors.Add(new Error(rc.ErrorCode, new Exception("No rows affected")));
                    }
                }
            }
            catch (Exception ex) {
                rc.ErrorCode = ErrorCodes.INSERT_DATA_FAILED;
                rc.Errors.Add(new Error(rc.ErrorCode, ex));
            }

            if (rc.Success) {
                rc.Data = pItem;
            }

            return rc;
        }
        public async Task<IReturnCode<bool>> Delete(string pDatabaseName, string pTableName, string pQueryParameters) {
            IReturnCode<bool> rc = new ReturnCode<bool>();
            SqliteConnection connection = CreateConnection(pDatabaseName);

            try {
                if (rc.Success) {
                    await connection.OpenAsync();

                    if (connection.State != ConnectionState.Open) {
                        rc.ErrorCode = ErrorCodes.UNABLE_TO_OPEN_DATA_BASE;
                        rc.Errors.Add(new Error(rc.ErrorCode));
                    }
                }

                if (rc.Success) {
                    SqliteCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = $"DELETE FROM {pTableName} WHERE {pQueryParameters}";

                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex) {
                rc.ErrorCode = ErrorCodes.INSERT_DATA_FAILED;
                rc.Errors.Add(new Error(rc.ErrorCode, ex));
            }

            return rc;
        }


        protected static SqliteConnection CreateConnection(string pDatabaseName) {
            return new SqliteConnection($"Data Source={pDatabaseName}");
        }
        protected static async Task<IReturnCode<bool>> CreateTable(SqliteConnection pConnection, string pTableName, Type pObjectType) {
            IReturnCode<bool> rc = new ReturnCode<bool>();
            StringBuilder queryBuilder = new StringBuilder();

            try {
                if (rc.Success) {
                    if (pConnection.State != ConnectionState.Open) {
                        rc.ErrorCode = ErrorCodes.UNABLE_TO_OPEN_DATA_BASE;
                        rc.Errors.Add(new Error(rc.ErrorCode));
                    }
                }

                if (rc.Success) {
                    queryBuilder.Append($"CREATE TABLE IF NOT EXISTS {pTableName} (");
                    var properties = pObjectType.GetProperties();

                    for (int p = 0; p < properties.Length; p++) {
                        PropertyInfo prop = properties[p];

                        CustomAttributeData? primaryAttribute = prop.CustomAttributes.Where(x => x.AttributeType == typeof(PrimaryKeyAttribute)).FirstOrDefault();
                        CustomAttributeData? notNullAttribute = prop.CustomAttributes.Where(x => x.AttributeType == typeof(NotNullAttribute)).FirstOrDefault();
                        CustomAttributeData? autoIncrementAttribute = prop.CustomAttributes.Where(x => x.AttributeType == typeof(AutoIncrementAttribute)).FirstOrDefault();
                        CustomAttributeData? tableAttribute = prop.CustomAttributes.Where(x => x.AttributeType == typeof(TableAttribute)).FirstOrDefault();
                        CustomAttributeData? ignoreAttribute = prop.CustomAttributes.Where(x => x.AttributeType == typeof(IgnoreAttribute)).FirstOrDefault();

                        if (ignoreAttribute != null) {
                            continue;
                        }

                        string? dbTypeName = Worker.ConvertToDatabaseType(prop.PropertyType);

                        if (dbTypeName != null) {
                            if (p > 0) {
                                queryBuilder.Append(", ");
                            }

                            queryBuilder.Append($"{prop.Name} {dbTypeName}");

                            if (notNullAttribute != null) {
                                queryBuilder.Append(" NOT NULL");
                            }
                            if (primaryAttribute != null) {
                                queryBuilder.Append(" PRIMARY KEY");
                            }
                            if (autoIncrementAttribute != null) {
                                queryBuilder.Append(" AUTOINCREMENT");
                            }
                        }
                        else if (tableAttribute != null) {
                            string? tableName = tableAttribute.NamedArguments[0].TypedValue.Value as string;
                            string? tableColumn = tableAttribute.NamedArguments[1].TypedValue.Value as string;

                            if (tableName != null) {
                                IReturnCode<bool> createSubTableRc = await CreateTable(pConnection, tableName, prop.PropertyType);

                                if (createSubTableRc.Success) {
                                    if (p > 0) {
                                        queryBuilder.Append(", ");
                                    }

                                    queryBuilder.Append($"{prop.Name} TEXT");

                                    if (notNullAttribute != null) {
                                        queryBuilder.Append(" NOT NULL");
                                    }

                                    queryBuilder.Append($" REFERENCES {tableName}({tableColumn})");
                                }
                                if (createSubTableRc.Failed) {
                                    ErrorWorker.CopyErrors(createSubTableRc, rc);
                                }
                            }
                            else {
                                rc.ErrorCode = ErrorCodes.TABLE_NAME_MISSING_FROM_TABLE_ATTRIBUTE;
                                rc.Errors.Add(new Error(rc.ErrorCode, new Exception("Table name missing from table attribute")));
                            }
                        }
                    }

                    queryBuilder.Append($");");
                }

                if (rc.Success) {
                    var command = pConnection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = queryBuilder.ToString();
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex) {
                rc.ErrorCode = ErrorCodes.CREATE_TABLE_FAILED;
                rc.Errors.Add(new Error(rc.ErrorCode, ex));
            }

            return rc;
        }
        protected static async Task<IReturnCode<IList<object?>>> Get(SqliteConnection pConnection, string pTableName, Type pObjectType, string? pQueryParameters = null) {
            IReturnCode<IList<object?>> rc = new ReturnCode<IList<object?>>();
            SqlDatabase.Interfaces.IDataReader? dataReader = null;
            StringBuilder queryBuilder = new StringBuilder();
            IList<object?> itemsList = new List<object?>();

            try {
                if (rc.Success) {
                    if (pConnection.State != ConnectionState.Open) {
                        rc.ErrorCode = ErrorCodes.UNABLE_TO_OPEN_DATA_BASE;
                        rc.Errors.Add(new Error(rc.ErrorCode));
                    }
                }

                if (rc.Success) {
                    queryBuilder.Append($"SELECT * FROM {pTableName}");

                    if (pQueryParameters?.Length > 0) {
                        queryBuilder.Append($" WHERE {pQueryParameters}");
                    }

                    queryBuilder.Append(";");
                }

                if (rc.Success) {
                    SqliteCommand command = pConnection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = queryBuilder.ToString();

                    dataReader = new Models.DataReader(await command.ExecuteReaderAsync());
                }

                if (rc.Success) {
                    IReturnCode<IList<object?>> populateObjRc = await PopulateObjects(pConnection, dataReader, pObjectType);

                    if (populateObjRc.Success) {
                        itemsList = populateObjRc.Data!;
                    }

                    if (populateObjRc.Failed) {
                        ErrorWorker.CopyErrors(populateObjRc, rc);
                    }
                }
            }
            catch (Exception ex) {
                rc.ErrorCode = ErrorCodes.GET_DATA_FAILED;
                rc.Errors.Add(new Error(rc.ErrorCode, ex));
            }

            if (rc.Success) {
                rc.Data = itemsList;
            }

            return rc;
        }
        protected static async Task<IReturnCode<object?>> Insert(SqliteConnection pConnection, string pTableName, Type pObjectType, object? pObject) {
            IReturnCode<object?> rc = new ReturnCode<object?>();
            SqlDatabase.Interfaces.IDataReader? dataReader = null;
            StringBuilder queryBuilder = new StringBuilder();
            IList<IObjectProperty> propertiesList = new List<IObjectProperty>();

            try {
                if (rc.Success) {
                    if (pConnection.State != ConnectionState.Open) {
                        rc.ErrorCode = ErrorCodes.UNABLE_TO_OPEN_DATA_BASE;
                        rc.Errors.Add(new Error(rc.ErrorCode));
                    }
                }

                if (rc.Success) {
                    IReturnCode<IList<IObjectProperty>> getValuesRc = Worker.GetObjectProperties(pObject);

                    if (getValuesRc.Success) {
                        propertiesList = getValuesRc.Data!;
                    }

                    if (getValuesRc.Failed) {
                        ErrorWorker.CopyErrors(getValuesRc, rc);
                    }
                }

                if (rc.Success) {
                    queryBuilder.Append($"INSERT INTO {pTableName}(");

                    for (int k = 0; k < propertiesList.Count; k++) {
                        // is ignored
                        if (propertiesList[k].Attributes.Where(x => x.AttributeType == typeof(IgnoreAttribute)).FirstOrDefault() != null ||
                            propertiesList[k].Value == null) {
                            continue;
                        }

                        if (k != 0) {
                            queryBuilder.Append(", ");
                        }

                        string key = propertiesList[k].Name;
                        queryBuilder.Append($"{key}");
                    }
                    queryBuilder.Append(") VALUES (");

                    for (int v = 0; v < propertiesList.Count; v++) {
                        // is ignored
                        if (propertiesList[v].Attributes.Where(x => x.AttributeType == typeof(IgnoreAttribute)).FirstOrDefault() != null ||
                            propertiesList[v].Value == null) {
                            continue;
                        }

                        if (v != 0) {
                            queryBuilder.Append(", ");
                        }

                        object? value = propertiesList[v].Value;

                        if (value?.GetType() == typeof(string)) {
                            queryBuilder.Append($"'{value}'");
                        }
                        else if (value?.GetType().IsEnum == true) {
                            queryBuilder.Append($"{(int)value}");
                        }
                        else if (value?.GetType().IsClass == true) {
                            CustomAttributeData? tableAttribute = propertiesList[v].Attributes.Where(x => x.AttributeType == typeof(TableAttribute)).FirstOrDefault();
                            string? tableName = tableAttribute?.NamedArguments[0].TypedValue.Value as string;
                            string? columnName = tableAttribute?.NamedArguments[1].TypedValue.Value as string;



                            if (tableName == null) {
                                rc.ErrorCode = ErrorCodes.TABLE_NAME_MISSING_FROM_TABLE_ATTRIBUTE;
                                rc.Errors.Add(new Error(rc.ErrorCode, new Exception("Table name missing from table attributes")));
                            }
                            else if (columnName == null) {
                                rc.ErrorCode = ErrorCodes.COLUMN_NAME_MISSING_FROM_TABLE_ATTRIBUTE;
                                rc.Errors.Add(new Error(rc.ErrorCode, new Exception("Column name missing from table attributes")));
                            }
                            else {
                                var insertSubItemRc = await Insert(pConnection, tableName!, value.GetType(), value);

                                if (insertSubItemRc.Success) {
                                    var getSubObjectProperties = Worker.GetObjectProperties(value);

                                    if (getSubObjectProperties.Success) {
                                        foreach (var subProp in getSubObjectProperties.Data!) {
                                            if (subProp.Name.Equals(columnName, StringComparison.CurrentCultureIgnoreCase)) {
                                                queryBuilder.Append($"'{subProp.Value}'");
                                                break;
                                            }
                                        }
                                    }
                                    if (getSubObjectProperties.Failed) {
                                        ErrorWorker.CopyErrors(getSubObjectProperties, rc);
                                    }
                                }

                                if (insertSubItemRc.Failed) {
                                    ErrorWorker.CopyErrors(insertSubItemRc, rc);
                                }
                            }
                        }
                        else {
                            queryBuilder.Append($"{value}");
                        }
                    }

                    queryBuilder.Append(");");
                }

                if (rc.Success) {
                    SqliteCommand command = pConnection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = queryBuilder.ToString();

                    dataReader = new Models.DataReader(await command.ExecuteReaderAsync());

                    if (dataReader.RowsAffected() == 0) {
                        rc.ErrorCode = ErrorCodes.NO_ROWS_AFFECTED;
                        rc.Errors.Add(new Error(rc.ErrorCode, new Exception("No rows affected")));
                    }
                }
            }
            catch (Exception ex) {
                rc.ErrorCode = ErrorCodes.INSERT_DATA_FAILED;
                rc.Errors.Add(new Error(rc.ErrorCode, ex));
            }

            if (rc.Success) {
                rc.Data = pObject;
            }

            return rc;
        }

        protected static async Task<IReturnCode<IList<object?>>> PopulateObjects(SqliteConnection pConnection, SqlDatabase.Interfaces.IDataReader? pDataReader, Type pObjectType) {
            IReturnCode<IList<object?>> rc = new ReturnCode<IList<object?>>();
            IList<object?> objList = new List<object?>();

            try {
                PropertyInfo[] properties = pObjectType.GetProperties();

                while (pDataReader?.NextRow() == true) {
                    object? obj = default;
                    obj = Activator.CreateInstance(pObjectType);

                    foreach (var prop in properties) {
                        if (pDataReader.HasValue(prop.Name)) {
                            object? value = pDataReader.Get(prop.Name);

                            if (value == null) {
                                continue;
                            }
                            else if (prop.PropertyType == typeof(int)) {
                                prop.SetValue(obj, Convert.ToInt32(value));
                            }
                            else if (prop.PropertyType.IsEnum) {
                                int intValue = Convert.ToInt32(value);
                                prop.SetValue(obj, prop.PropertyType.GetEnumValues().GetValue(intValue));
                            }
                            else if (prop.PropertyType == typeof(string)) {
                                 prop.SetValue(obj, (string)value);
                            }
                            else if (prop.PropertyType == typeof(bool)) {
                                prop.SetValue(obj, (long)value != 0);
                            }
                            else if (prop.PropertyType == typeof(float)) {
                                prop.SetValue(obj, Convert.ToSingle(value));
                            }
                            else if (prop.PropertyType == typeof(double)) {
                                prop.SetValue(obj, value);
                            }
                            else if (prop.PropertyType == typeof(byte)) {
                                prop.SetValue(obj, Convert.ToByte(value));
                            }
                            else if (prop.PropertyType.IsClass) {
                                string itemId = (string)value;
                                CustomAttributeData? tableAttribute = prop.CustomAttributes.Where(x => x.AttributeType == typeof(TableAttribute)).FirstOrDefault();
                                string? tableName = tableAttribute?.NamedArguments[0].TypedValue.Value as string;
                                string? columnName = tableAttribute?.NamedArguments[1].TypedValue.Value as string;

                                if (tableName == null) {
                                    rc.ErrorCode = ErrorCodes.TABLE_NAME_MISSING_FROM_TABLE_ATTRIBUTE;
                                    rc.Errors.Add(new Error(rc.ErrorCode, new Exception("Table name missing from table attributes")));
                                }
                                else if (columnName == null) {
                                    rc.ErrorCode = ErrorCodes.COLUMN_NAME_MISSING_FROM_TABLE_ATTRIBUTE;
                                    rc.Errors.Add(new Error(rc.ErrorCode, new Exception("Column name missing from table attributes")));
                                }
                                else {
                                    IReturnCode<IList<object?>> getSubObjectRc = await Get(pConnection, tableName, prop.PropertyType, $"{columnName} = '{itemId}'");

                                    if (getSubObjectRc.Success) {
                                        if (getSubObjectRc.Data!.Count == 0) {

                                        }
                                        else if (getSubObjectRc.Data!.Count > 1) {

                                        }
                                        else {
                                            prop.SetValue(obj, getSubObjectRc.Data![0]);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    objList.Add(obj);
                }
            }
            catch (Exception ex) {
                rc.ErrorCode = ErrorCodes.POPULATE_OBJECT_FAILED;
                rc.Errors.Add(new Error(rc.ErrorCode, ex));
            }

            if (rc.Success) {
                rc.Data = objList;
            }

            return rc;
        }
        public async Task<IReturnCode<T>> Insert<T>(string pDatabaseName, string pTableName, T pObject) {
            IReturnCode<T> rc = new ReturnCode<T>();
            Interfaces.IDataReader? dataReader = null;
            T? item = default;
            IDictionary<string, object> properties = new Dictionary<string, object>();

            try {
                if (rc.Success) {

                }

                if (rc.Success) {
                    IReturnCode<Interfaces.IDataReader> dataReaderRc = await RunQuery(pDatabaseName, $"INSERT INTO {pTableName}() VALUES ");

                    if (dataReaderRc.Success) {
                        dataReader = dataReaderRc.Data;

                        while (dataReader?.NextRow() ?? false) {
                            IReturnCode<T> parseDataRc = Worker.ParseData<T>(dataReader);

                            if (parseDataRc.Success) {
                                item = parseDataRc.Data;
                            }
                            if (parseDataRc.Failed) {
                                ErrorWorker.CopyErrors(parseDataRc, rc);
                            }
                        }
                    }
                    else {
                        ErrorWorker.CopyErrors(dataReaderRc, rc);
                    }
                }

                if (rc.Success) {
                    PropertyInfo[] propInfo = typeof(T).GetProperties();
                    foreach (PropertyInfo prop in propInfo) {
                        prop.SetValue(item, 0);
                    }
                }
            }
            catch (Exception ex) {
                rc.Errors.Add(new SqlDatabaseError(ErrorCodes.INSERT_DATA_FAILED, ex));
            }

            return rc;
        }
        public async Task<IReturnCode<bool>> Delete<T>(string pDatabaseName, string pTableName, string pId) {
            IReturnCode<bool> rc = new ReturnCode<bool>();

            try {
                if (rc.Success) {
                    IReturnCode<Interfaces.IDataReader> dataReaderRc = await RunQuery(pDatabaseName, $"DELETE FROM {pTableName} WHERE id = '{pId}'");

                    if (dataReaderRc.Failed) {
                        ErrorWorker.CopyErrors(dataReaderRc, rc);
                    }
                }
            }
            catch (Exception ex) {
                rc.Errors.Add(new SqlDatabaseError(ErrorCodes.DELETE_DATA_FAILED, ex));
            }

            return rc;
        }
        public async Task<IReturnCode<T>> Update<T>(string pDatabaseName, string pTableName, T pObject) {
            IReturnCode<T> rc = new ReturnCode<T>();
            Interfaces.IDataReader? dataReader = null;
            T? item = default;
            StringBuilder valuesBuilder = new StringBuilder();

            try {

                if (rc.Success) {

                }

                if (rc.Success) {
                    IReturnCode<Interfaces.IDataReader> dataReaderRc = await RunQuery(pDatabaseName, $"UPDATE {pTableName} SET {valuesBuilder} WHERE id = '{pObject}'");

                    if (dataReaderRc.Success) {
                        dataReader = dataReaderRc.Data;

                        while (dataReader?.NextRow() ?? false) {
                            IReturnCode<T> parseDataRc = Worker.ParseData<T>(dataReader);

                            if (parseDataRc.Success) {
                                item = parseDataRc.Data;
                            }
                            if (parseDataRc.Failed) {
                                ErrorWorker.CopyErrors(parseDataRc, rc);
                            }
                        }
                    }
                    else {
                        ErrorWorker.CopyErrors(dataReaderRc, rc);
                    }
                }

                if (rc.Success) {
                    PropertyInfo[] propInfo = typeof(T).GetProperties();
                    foreach (PropertyInfo prop in propInfo) {
                        prop.SetValue(item, 0);
                    }
                }
            }
            catch (Exception ex) {
                rc.Errors.Add(new SqlDatabaseError(ErrorCodes.UPDATE_DATA_FAILED, ex));
            }

            return rc;
        }

        private IReturnCode<IDictionary<string, object?>> GetPropertyValues<T>(T pObject) {
            IReturnCode<IDictionary<string, object?>> rc = new ReturnCode<IDictionary<string, object?>>();
            IDictionary<string, object?> values = new Dictionary<string, object?>();

            try {
                if (rc.Success) {
                    Type objType = typeof(T);
                    var properties = objType.GetProperties();

                    foreach(PropertyInfo prop in properties) {
                        string name = prop.Name;
                        object? value = prop.GetValue(pObject, null);

                        values.Add(name, value);
                    }
                }
            }
            catch (Exception ex) {

            }

            if (rc.Success) {
                rc.Data = values;
            }


            return rc;
        }
        private IReturnCode<IDictionary<string, object?>> GEtPrimaryKeyValue<T>(T pObject) {
            IReturnCode<IDictionary<string, object?>> rc = new ReturnCode<IDictionary<string, object?>>();
            IDictionary<string, object?> values = new Dictionary<string, object?>();

            try {
                if (rc.Success) {
                    Type objType = typeof(T);
                    var properties = objType.GetProperties();

                    foreach (PropertyInfo prop in properties) {
                        CustomAttributeData? primary = prop.CustomAttributes.Where(x => x.AttributeType == typeof(PrimaryKeyAttribute)).FirstOrDefault();

                        if (primary != null) {
                            string name = prop.Name;
                            object? value = prop.GetValue(pObject, null);
                        }
                    }
                }
            }
            catch (Exception ex) {

            }

            if (rc.Success) {
                rc.Data = values;
            }


            return rc;
        }
    }
}
