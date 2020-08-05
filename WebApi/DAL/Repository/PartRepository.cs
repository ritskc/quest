using DAL.DBHelper;
using DAL.IRepository;
using DAL.Models;
using DAL.Settings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class PartRepository : IPartRepository
    {
        private readonly ISqlHelper _sqlHelper;
        private readonly IUserRepository userRepository;
        private readonly ICompanyRepository companyRepository;

        public PartRepository(ISqlHelper sqlHelper, IUserRepository userRepository, ICompanyRepository companyRepository)
        {
            _sqlHelper = sqlHelper;
            this.userRepository = userRepository;
            this.companyRepository = companyRepository;
        }

        public async Task<IEnumerable<Part>> GetAllPartsAsync(int companyId, int warehouseId, int userId)
        {
            List<Part> parts = new List<Part>();

            var commandText = "";
            var userInfo = await userRepository.GeUserbyIdAsync(userId);

            commandText = string.Format($"SELECT pm.[id],pm.[Code],pm.[Description],pm.[CompanyId],pm.[IsActive],CM.NAME Category, pm.[CategoryId],UM.Name Unit, pm.[UnitId],pm.[SupplierPrice],pm.[CustomerPrice],pm.[IsSurcharge],pm.[SurchargePercentage],pm.[SurchargeAmount],pm.[SKU] ,IM.[WarehouseId],IM.[MinQty],IM.[MaxQty],IM.[Location],IM.[OpeningQty] ,IM.[IntransitQty],IM.[QtyInHand] FROM [part] pm INNER JOIN Category CM ON cm.id = pm.[CategoryId] INNER JOIN Unit UM ON um.id = pm.[UnitId] INNER JOIN Inventory IM ON IM.PartId = PM.Id where pm.CompanyId = '{companyId}' AND im.WarehouseId = '{warehouseId}' ");

            SqlConnection conn = new SqlConnection(ConnectionSettings.ConnectionString);

            using (SqlCommand cmd = new SqlCommand(commandText, conn))
            {
                cmd.CommandType = CommandType.Text;

                conn.Open();

                var dataReader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    var part = new Part();
                    part.Id = Convert.ToInt64(dataReader["Id"]);
                    part.Code = Convert.ToString(dataReader["Code"]);
                    part.Description = Convert.ToString(dataReader["Description"]);
                    part.CompanyId = Convert.ToInt32(dataReader["CompanyId"]);
                    part.WarehouseId = Convert.ToInt32(dataReader["WarehouseId"]);
                    part.Category = Convert.ToString(dataReader["Category"]);
                    part.CategoryId = Convert.ToInt32(dataReader["CategoryId"]);
                    part.Unit = Convert.ToString(dataReader["Unit"]);
                    part.UnitId = Convert.ToInt32(dataReader["UnitId"]);
                    part.MinQty = Convert.ToInt32(dataReader["MinQty"]);
                    part.MaxQty = Convert.ToInt32(dataReader["MaxQty"]);
                    part.OpeningQty = Convert.ToInt32(dataReader["OpeningQty"]);                   
                    part.IsActive = Convert.ToBoolean(dataReader["IsActive"]);                   
                    part.Location = Convert.ToString(dataReader["Location"]);
                    part.IntransitQty = Convert.ToInt32(dataReader["IntransitQty"]);
                    part.QtyInHand = Convert.ToInt32(dataReader["QtyInHand"]);
                    part.CustomerPrice = Convert.ToDecimal(dataReader["CustomerPrice"]);
                    part.SupplierPrice = Convert.ToDecimal(dataReader["SupplierPrice"]);
                    part.IsSurcharge = Convert.ToBoolean(dataReader["IsSurcharge"]);
                    part.SurchargePercentage = Convert.ToDecimal(dataReader["SurchargePercentage"]);
                    part.SurchargeAmount = Convert.ToDecimal(dataReader["SurchargeAmount"]);
                    part.SKU = Convert.ToString(dataReader["SKU"]);

                    parts.Add(part);
                }
                conn.Close();
            }            

            return parts.OrderBy(x => x.Code);
        }

        public Part GetPart(long partId, int warehouseId)
        {
            var part = new Part();
            SqlConnection conn = new SqlConnection(ConnectionSettings.ConnectionString);

            var commandText = string.Format($"SELECT pm.[id],pm.[Code],pm.[Description],pm.[CompanyId],pm.[IsActive],CM.NAME Category, pm.[CategoryId],UM.Name Unit, pm.[UnitId],pm.[SupplierPrice],pm.[CustomerPrice],pm.[IsSurcharge],pm.[SurchargePercentage],pm.[SurchargeAmount],pm.[SKU] ,IM.[WarehouseId],IM.[MinQty],IM.[MaxQty],IM.[Location],IM.[OpeningQty] ,IM.[IntransitQty],IM.[QtyInHand] FROM [part] pm INNER JOIN Category CM ON cm.id = pm.[CategoryId] INNER JOIN Unit UM ON um.id = pm.[UnitId] INNER JOIN Inventory IM ON IM.PartId = PM.Id where pm.id = '{partId}' AND im.WarehouseId = '{warehouseId}'");

            using (SqlCommand cmd = new SqlCommand(commandText, conn))
            {
                cmd.CommandType = CommandType.Text;

                conn.Open();

                var dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    part.Id = Convert.ToInt64(dataReader["Id"]);
                    part.Code = Convert.ToString(dataReader["Code"]);
                    part.Description = Convert.ToString(dataReader["Description"]);
                    part.CompanyId = Convert.ToInt32(dataReader["CompanyId"]);
                    part.WarehouseId = Convert.ToInt32(dataReader["WarehouseId"]);
                    part.Category = Convert.ToString(dataReader["Category"]);
                    part.CategoryId = Convert.ToInt32(dataReader["CategoryId"]);
                    part.Unit = Convert.ToString(dataReader["Unit"]);
                    part.UnitId = Convert.ToInt32(dataReader["UnitId"]);
                    part.MinQty = Convert.ToInt32(dataReader["MinQty"]);
                    part.MaxQty = Convert.ToInt32(dataReader["MaxQty"]);
                    part.OpeningQty = Convert.ToInt32(dataReader["OpeningQty"]);
                    part.IsActive = Convert.ToBoolean(dataReader["IsActive"]);
                    part.Location = Convert.ToString(dataReader["Location"]);
                    part.IntransitQty = Convert.ToInt32(dataReader["IntransitQty"]);
                    part.QtyInHand = Convert.ToInt32(dataReader["QtyInHand"]);
                    part.CustomerPrice = Convert.ToDecimal(dataReader["CustomerPrice"]);
                    part.SupplierPrice = Convert.ToDecimal(dataReader["SupplierPrice"]);
                    part.IsSurcharge = Convert.ToBoolean(dataReader["IsSurcharge"]);
                    part.SurchargePercentage = Convert.ToDecimal(dataReader["SurchargePercentage"]);
                    part.SurchargeAmount = Convert.ToDecimal(dataReader["SurchargeAmount"]);
                    part.SKU = Convert.ToString(dataReader["SKU"]);
                }
                conn.Close();
            }          

            return part;
        }

        public async Task<Part> GetPartAsync(long partId, int warehouseId,SqlConnection conn, SqlTransaction transaction)
        {
            var part = new Part();

            var commandText = string.Format($"SELECT pm.[id],pm.[Code],pm.[Description],pm.[CompanyId],pm.[IsActive],CM.NAME Category, pm.[CategoryId],UM.Name Unit, pm.[UnitId],pm.[SupplierPrice],pm.[CustomerPrice],pm.[IsSurcharge],pm.[SurchargePercentage],pm.[SurchargeAmount],pm.[SKU] ,IM.[WarehouseId],IM.[MinQty],IM.[MaxQty],IM.[Location],IM.[OpeningQty] ,IM.[IntransitQty],IM.[QtyInHand] FROM [part] pm INNER JOIN Category CM ON cm.id = pm.[CategoryId] INNER JOIN Unit UM ON um.id = pm.[UnitId] INNER JOIN Inventory IM ON IM.PartId = PM.Id where pm.id = '{partId}' AND im.WarehouseId = '{warehouseId}'");

            using (SqlCommand cmd = new SqlCommand(commandText, conn, transaction))
            {
                cmd.CommandType = CommandType.Text;

                var dataReader = await cmd.ExecuteReaderAsync(CommandBehavior.Default);

                while (dataReader.Read())
                {
                    part.Id = Convert.ToInt64(dataReader["Id"]);
                    part.Code = Convert.ToString(dataReader["Code"]);
                    part.Description = Convert.ToString(dataReader["Description"]);
                    part.CompanyId = Convert.ToInt32(dataReader["CompanyId"]);
                    part.WarehouseId = Convert.ToInt32(dataReader["WarehouseId"]);
                    part.Category = Convert.ToString(dataReader["Category"]);
                    part.CategoryId = Convert.ToInt32(dataReader["CategoryId"]);
                    part.Unit = Convert.ToString(dataReader["Unit"]);
                    part.UnitId = Convert.ToInt32(dataReader["UnitId"]);
                    part.MinQty = Convert.ToInt32(dataReader["MinQty"]);
                    part.MaxQty = Convert.ToInt32(dataReader["MaxQty"]);
                    part.OpeningQty = Convert.ToInt32(dataReader["OpeningQty"]);
                    part.IsActive = Convert.ToBoolean(dataReader["IsActive"]);
                    part.Location = Convert.ToString(dataReader["Location"]);
                    part.IntransitQty = Convert.ToInt32(dataReader["IntransitQty"]);
                    part.QtyInHand = Convert.ToInt32(dataReader["QtyInHand"]);
                    part.CustomerPrice = Convert.ToDecimal(dataReader["CustomerPrice"]);
                    part.SupplierPrice = Convert.ToDecimal(dataReader["SupplierPrice"]);
                    part.IsSurcharge = Convert.ToBoolean(dataReader["IsSurcharge"]);
                    part.SurchargePercentage = Convert.ToDecimal(dataReader["SurchargePercentage"]);
                    part.SurchargeAmount = Convert.ToDecimal(dataReader["SurchargeAmount"]);
                    part.SKU = Convert.ToString(dataReader["SKU"]);
                }
                dataReader.Close();
            }         

            return part;
        }

        public async Task<Part> GetPartAsync(long partId,int warehouseId)
        {
            var part = new Part();
            SqlConnection conn = new SqlConnection(ConnectionSettings.ConnectionString);

            var commandText = string.Format($"SELECT pm.[id],pm.[Code],pm.[Description],pm.[CompanyId],pm.[IsActive],CM.NAME Category, pm.[CategoryId],UM.Name Unit, pm.[UnitId],pm.[SupplierPrice],pm.[CustomerPrice],pm.[IsSurcharge],pm.[SurchargePercentage],pm.[SurchargeAmount],pm.[SKU] ,IM.[WarehouseId],IM.[MinQty],IM.[MaxQty],IM.[Location],IM.[OpeningQty] ,IM.[IntransitQty],IM.[QtyInHand] FROM [part] pm INNER JOIN Category CM ON cm.id = pm.[CategoryId] INNER JOIN Unit UM ON um.id = pm.[UnitId] INNER JOIN Inventory IM ON IM.PartId = PM.Id where pm.id = '{partId}' AND im.WarehouseId = '{warehouseId}'");

            using (SqlCommand cmd = new SqlCommand(commandText, conn))
            {
                cmd.CommandType = CommandType.Text;

                conn.Open();

                var dataReader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    part.Id = Convert.ToInt64(dataReader["Id"]);
                    part.Code = Convert.ToString(dataReader["Code"]);
                    part.Description = Convert.ToString(dataReader["Description"]);
                    part.CompanyId = Convert.ToInt32(dataReader["CompanyId"]);
                    part.WarehouseId = Convert.ToInt32(dataReader["WarehouseId"]);
                    part.Category = Convert.ToString(dataReader["Category"]);
                    part.CategoryId = Convert.ToInt32(dataReader["CategoryId"]);
                    part.Unit = Convert.ToString(dataReader["Unit"]);
                    part.UnitId = Convert.ToInt32(dataReader["UnitId"]);
                    part.MinQty = Convert.ToInt32(dataReader["MinQty"]);
                    part.MaxQty = Convert.ToInt32(dataReader["MaxQty"]);
                    part.OpeningQty = Convert.ToInt32(dataReader["OpeningQty"]);
                    part.IsActive = Convert.ToBoolean(dataReader["IsActive"]);
                    part.Location = Convert.ToString(dataReader["Location"]);
                    part.IntransitQty = Convert.ToInt32(dataReader["IntransitQty"]);
                    part.QtyInHand = Convert.ToInt32(dataReader["QtyInHand"]);
                    part.CustomerPrice = Convert.ToDecimal(dataReader["CustomerPrice"]);
                    part.SupplierPrice = Convert.ToDecimal(dataReader["SupplierPrice"]);
                    part.IsSurcharge = Convert.ToBoolean(dataReader["IsSurcharge"]);
                    part.SurchargePercentage = Convert.ToDecimal(dataReader["SurchargePercentage"]);
                    part.SurchargeAmount = Convert.ToDecimal(dataReader["SurchargeAmount"]);
                    part.SKU = Convert.ToString(dataReader["SKU"]);
                }
                conn.Close();
            }       

            return part;
        }              
       
        public async Task AddPartAsync(Part part)
        {
            var warehouses = await this.companyRepository.GetWarehouseAsync(part.CompanyId);

            using (SqlConnection connection = new SqlConnection(ConnectionSettings.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;

                // Start a local transaction.
                transaction = connection.BeginTransaction("SampleTransaction");

                // Must assign both transaction object and connection
                // to Command object for a pending local transaction
                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    if (part.Location == null)
                        part.Location = string.Empty;                    

                    string sql = string.Format($"INSERT INTO [dbo].[part] ([Code],[Description] ,[CompanyId] ,[IsActive] ,[CategoryId] ,[UnitId] ,[SupplierPrice] ,[CustomerPrice] ,[IsSurcharge],[SurchargePercentage],[SurchargeAmount] ,[SKU])    VALUES   " +
                        $"('{part.Code.Replace("'", "''")}'   ,'{part.Description.Replace("'", "''")}'   ,'{part.CompanyId}'  ,'{part.IsActive}'   ,'{part.CategoryId}','{part.UnitId}','{part.SupplierPrice}','{part.CustomerPrice}','{part.IsSurcharge}','{part.SurchargePercentage}','{part.SurchargeAmount}','{part.SKU}')");

                    sql = sql + " Select Scope_Identity()";
                    command.CommandText = sql;
                    var partId = command.ExecuteScalar();

                    foreach (Warehouse warehouse in warehouses)
                    {
                        if(warehouse.Id == part.WarehouseId)
                            sql = string.Format($"INSERT INTO [dbo].[Inventory] ([PartId]  ,[CompanyId] ,[WarehouseId] ,[MinQty] ,[MaxQty] ,[Location] ,[OpeningQty] ,[IntransitQty] ,[QtyInHand])  VALUES" +
                           $"   ('{partId}'   ,'{part.CompanyId}'   ,'{warehouse.Id}'   ,'{part.MinQty}'   ,'{part.MaxQty}','{part.Location.Replace("'", "''")}'   ,'{part.OpeningQty}'   ,'{0}'   ,'{0}')");

                        else
                            sql = string.Format($"INSERT INTO [dbo].[Inventory] ([PartId]  ,[CompanyId] ,[WarehouseId] ,[MinQty] ,[MaxQty] ,[Location] ,[OpeningQty] ,[IntransitQty] ,[QtyInHand])  VALUES" +
                          $"   ('{partId}'   ,'{part.CompanyId}'   ,'{warehouse.Id}'   ,'{0}'   ,'{0}','{""}'   ,'{0}'   ,'{0}'   ,'{0}')");

                        command.CommandText = sql;
                        await command.ExecuteNonQueryAsync();
                    }                   
                    
                    // Attempt to commit the transaction.
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }       

        public async Task UpdatePartAsync(Part part)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionSettings.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;

                // Start a local transaction.
                transaction = connection.BeginTransaction("SampleTransaction");

                // Must assign both transaction object and connection
                // to Command object for a pending local transaction
                command.Connection = connection;
                command.Transaction = transaction;

                try
                {                  

                    if (part.Location == null)
                        part.Location = string.Empty;

                    var sql = string.Format($"UPDATE [dbo].[part]   SET [Code] = '{part.Code.Replace("'", "''")}' ,[Description] = '{part.Description.Replace("'", "''")}' " +
                        $",[CompanyId] = '{part.CompanyId}' ,[IsActive] = '{part.IsActive}'  ,[CategoryId] = '{part.CategoryId}' ,[UnitId] = '{part.UnitId}'" +
                        $",[SupplierPrice] = '{part.SupplierPrice}' ,[CustomerPrice] = '{part.CustomerPrice}' ,[IsSurcharge] = '{part.IsSurcharge}' ,[SurchargePercentage] = '{part.SurchargePercentage}' " +
                        $",[SurchargeAmount] = '{part.SurchargeAmount}' ,[SKU] = '{part.SKU.Replace("'", "''")}'  WHERE id = '{part.Id}' ");
                    command.CommandText = sql;
                    await command.ExecuteNonQueryAsync();

                    sql = string.Format($"UPDATE [dbo].[Inventory]  SET  [MinQty] = '{part.MinQty}' ,[MaxQty] = '{part.MaxQty}' ,[Location] = '{part.Location.Replace("'", "''")}'  ,[OpeningQty] = '{part.OpeningQty}'  WHERE id = '{part.Id}' and WarehouseId = '{part.WarehouseId}'");

                    command.CommandText = sql;
                    await command.ExecuteNonQueryAsync();

                    // Attempt to commit the transaction.
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
        
        public async Task UpdateQtyInHandByPartIdAsync(int companyId, int warehouseId, int partId, int QtyInHand, string direction, string note)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionSettings.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;

                // Start a local transaction.
                transaction = connection.BeginTransaction("SampleTransaction");

                // Must assign both transaction object and connection
                // to Command object for a pending local transaction
                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    var sql = string.Empty;
                    if (direction.ToLower() == BusinessConstants.DIRECTION.IN.ToString().ToLower())
                    {
                        sql = string.Format($"UPDATE [Inventory]   SET  [QtyInHand] = QtyInHand + '{QtyInHand}'  WHERE  id= '{partId}' AND CompanyId = '{companyId}' AND WarehouseId = '{warehouseId}'");
                        command.CommandText = sql;
                        await command.ExecuteNonQueryAsync();

                        sql = string.Format($"INSERT INTO [dbo].[TransactionDetails]   ([PartId]   ,[TransactionTypeId]   ,[TransactionDate]   ,[DirectionTypeId]   ,[InventoryTypeId]   ,[ReferenceNo]   ,[Qty]) VALUES   ('{partId}'   ,'{ Convert.ToInt32(BusinessConstants.TRANSACTION_TYPE.ADJUSTMENT_PLUS)}'   ,'{DateTime.Now}'   ,'{Convert.ToInt32(BusinessConstants.DIRECTION.IN)}'   ,'{Convert.ToInt32(BusinessConstants.INVENTORY_TYPE.QTY_IN_HAND)}'   ,'{"Inventory Adjustment"}'   ,'{QtyInHand}')");
                        command.CommandText = sql;
                        await command.ExecuteNonQueryAsync();
                    }
                    else
                    {
                        sql = string.Format($"UPDATE [Inventory]   SET  [QtyInHand] = QtyInHand - '{QtyInHand}'  WHERE  id= '{partId}' AND CompanyId = '{companyId}' AND WarehouseId = '{warehouseId}'");
                        command.CommandText = sql;
                        await command.ExecuteNonQueryAsync();

                        sql = string.Format($"INSERT INTO [dbo].[TransactionDetails]   ([PartId]   ,[TransactionTypeId]   ,[TransactionDate]   ,[DirectionTypeId]   ,[InventoryTypeId]   ,[ReferenceNo]   ,[Qty]) VALUES   ('{partId}'   ,'{ Convert.ToInt32(BusinessConstants.TRANSACTION_TYPE.ADJUSTMENT_MINUS)}'   ,'{DateTime.Now}'   ,'{Convert.ToInt32(BusinessConstants.DIRECTION.OUT)}'   ,'{Convert.ToInt32(BusinessConstants.INVENTORY_TYPE.QTY_IN_HAND)}'   ,'{"Inventory Adjustment"}'   ,'{QtyInHand}')");
                        command.CommandText = sql;
                        await command.ExecuteNonQueryAsync();
                    }

                    // Attempt to commit the transaction.
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }       

        public async Task DeletePartAsync(long id)
        {

            using (SqlConnection connection = new SqlConnection(ConnectionSettings.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;

                // Start a local transaction.
                transaction = connection.BeginTransaction("SampleTransaction");

                // Must assign both transaction object and connection
                // to Command object for a pending local transaction
                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    var sql = string.Format("DELETE FROM [dbo].[Inventory]  WHERE partid = '{0}'", id);
                    command.CommandText = sql;
                    await command.ExecuteNonQueryAsync();                    

                    sql = string.Format("DELETE FROM [dbo].[part]  WHERE id = '{0}'", id);
                    command.CommandText = sql;
                    await command.ExecuteNonQueryAsync();

                    // Attempt to commit the transaction.
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
    }
}
