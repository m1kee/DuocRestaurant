using Domain;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace Business.Services
{
    public class RecipeService : IRecipeService
    {
        public Recipe Add(RestaurantDatabaseSettings ctx, Recipe recipe)
        {
            Recipe result = null;

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                conn.Open();
                OracleTransaction transaction = conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);

                string query = $"INSERT INTO Receta (" +
                    $"{Recipe.ColumnNames.Name}, " +
                    $"{Recipe.ColumnNames.Price}, " +
                    $"{Recipe.ColumnNames.Details}, " +
                    $"{Recipe.ColumnNames.PreparationTime}, " +
                    $"{Recipe.ColumnNames.Active} " +
                    $") VALUES (" +
                    $"'{recipe.Name}', " +
                    $"{recipe.Price}, " +
                    $"'{recipe.Details}', " +
                    $"{recipe.PreparationTime}, " +
                    $"{1} " +
                    $") RETURNING {Recipe.ColumnNames.Id} INTO :{Recipe.ColumnNames.Id}";
                OracleCommand cmd = new OracleCommand(query, conn);
                cmd.Transaction = transaction;
                cmd.Parameters.Add(new OracleParameter()
                {
                    ParameterName = $":{Recipe.ColumnNames.Id}",
                    OracleDbType = OracleDbType.Decimal,
                    Direction = System.Data.ParameterDirection.Output
                });

                try
                {
                    cmd.ExecuteNonQuery();

                    recipe.Id = Convert.ToInt32(cmd.Parameters[$":{Recipe.ColumnNames.Id}"].Value.ToString());

                    if (recipe.RecipeDetails != null)
                    {
                        List<RecipeDetail> recipeDetails = new List<RecipeDetail>();
                        foreach (var recipeDetail in recipe.RecipeDetails)
                        {
                            RecipeDetail createdRecipeDetail = this.Add(conn, transaction, recipe.Id, recipeDetail);
                            recipeDetails.Add(createdRecipeDetail);
                        }
                    }

                    transaction.Commit();

                    result = recipe;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return result;
        }

        public RecipeDetail Add(RestaurantDatabaseSettings ctx, RecipeDetail recipeDetail)
        {
            RecipeDetail result = null;

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                conn.Open();

                string query = $"INSERT INTO {RecipeDetail.TableName} (" +
                    $"{RecipeDetail.ColumnNames.RecipeId}, " +
                    $"{RecipeDetail.ColumnNames.ProductId}, " +
                    $"{RecipeDetail.ColumnNames.Count}, " +
                    $"{RecipeDetail.ColumnNames.Active} " +
                    $") VALUES (" +
                    $"{recipeDetail.RecipeId}, " +
                    $"{recipeDetail.ProductId}, " +
                    $"{recipeDetail.Count}, " +
                    $"{1} " +
                    $")";
                OracleCommand cmd = new OracleCommand(query, conn);
                cmd.ExecuteNonQuery();

                result = recipeDetail;
            }

            return result;
        }

        private RecipeDetail Add(OracleConnection connection, OracleTransaction transaction, int recipeId, RecipeDetail recipeDetail)
        {
            string query = $"INSERT INTO {RecipeDetail.TableName} (" +
                $"{RecipeDetail.ColumnNames.RecipeId}, " +
                $"{RecipeDetail.ColumnNames.ProductId}, " +
                $"{RecipeDetail.ColumnNames.Count}, " +
                $"{RecipeDetail.ColumnNames.Active} " +
                $") VALUES (" +
                $"{recipeId}, " +
                $"{recipeDetail.ProductId}, " +
                $"{recipeDetail.Count}, " +
                $"{1} " +
                $")";
            OracleCommand cmd = new OracleCommand(query, connection);
            cmd.Transaction = transaction;
            cmd.ExecuteNonQuery();

            recipeDetail.RecipeId = recipeId;

            RecipeDetail result = recipeDetail;

            return result;
        }

        public bool Delete(RestaurantDatabaseSettings ctx, int recipeId)
        {
            bool result = false;

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                string query = $"UPDATE {Recipe.TableName} " +
                    $"SET {Recipe.ColumnNames.Active} = 0 " +
                    $"WHERE {Recipe.ColumnNames.Id} = {recipeId}";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();

                cmd.ExecuteNonQuery();

                result = true;
            }

            return result;
        }

        private bool Delete(OracleConnection conn, OracleTransaction transaction, RecipeDetail recipeDetail)
        {
            string query = $"DELETE FROM {RecipeDetail.TableName} " +
                $"WHERE {RecipeDetail.ColumnNames.RecipeId} = {recipeDetail.RecipeId} " +
                $"AND {RecipeDetail.ColumnNames.ProductId} = {recipeDetail.ProductId}";
            OracleCommand deleteCommand = new OracleCommand(query, conn);
            deleteCommand.Transaction = transaction;
            deleteCommand.ExecuteNonQuery();

            bool result = true;

            return result;
        }

        public bool Delete(RestaurantDatabaseSettings ctx, RecipeDetail recipeDetail)
        {
            bool result = false;
            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                conn.Open();

                string query = $"DELETE FROM {RecipeDetail.TableName} " +
                $"WHERE {RecipeDetail.ColumnNames.RecipeId} = {recipeDetail.RecipeId} " +
                $"AND {RecipeDetail.ColumnNames.ProductId} = {recipeDetail.ProductId}";
                OracleCommand deleteCommand = new OracleCommand(query, conn);
                deleteCommand.ExecuteNonQuery();

                result = true;
            }

            return result;
        }

        public Recipe Edit(RestaurantDatabaseSettings ctx, int recipeId, Recipe recipe)
        {
            Recipe result = null;

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                conn.Open();
                OracleTransaction transaction = conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);

                string query = $"UPDATE {Recipe.TableName} " +
                    $"SET " +
                    $"{Recipe.ColumnNames.Name} = '{recipe.Name}', " +
                    $"{Recipe.ColumnNames.Price} = {recipe.Price}, " +
                    $"{Recipe.ColumnNames.Details} = '{recipe.Details}', " +
                    $"{Recipe.ColumnNames.PreparationTime} = {recipe.PreparationTime} " +
                    $"WHERE {Recipe.ColumnNames.Id} = {recipeId}";
                OracleCommand cmd = new OracleCommand(query, conn);
                cmd.Transaction = transaction;

                try
                {
                    cmd.ExecuteNonQuery();

                    // check recipe details
                    if (recipe.RecipeDetails != null)
                    {
                        if (recipe.RecipeDetails.Any(x => !x.Active))
                        {
                            // delete inactive recipe details
                            foreach (var deletedRecipeDetail in recipe.RecipeDetails.Where(x => !x.Active))
                            {
                                this.Delete(conn, transaction, deletedRecipeDetail);
                            }
                        }

                        // get bd recipe details to compare if exist or not
                        List<RecipeDetail> currentRecipeDetails = Get(conn, recipe).ToList();

                        // edit active recipe details
                        foreach (var editedRecipeDetail in recipe.RecipeDetails.Where(x => x.Active && currentRecipeDetails.Any(y => x.RecipeId == y.RecipeId && x.ProductId == y.ProductId)))
                        {
                            this.Edit(conn, transaction, editedRecipeDetail);
                        }

                        // create new recipe details
                        foreach (var createdRecipeDetail in recipe.RecipeDetails.Where(x => x.Active && !currentRecipeDetails.Any(y => x.RecipeId == y.RecipeId && x.ProductId == y.ProductId)))
                        {
                            this.Add(conn, transaction, recipeId, createdRecipeDetail);
                        }

                        foreach (var deletedRecipeDetail in currentRecipeDetails.Where(x => !recipe.RecipeDetails.Any(y => x.RecipeId == y.RecipeId && x.ProductId == y.ProductId)))
                        {
                            this.Delete(conn, transaction, deletedRecipeDetail);
                        }
                    }

                    transaction.Commit();

                    result = recipe;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return result;
        }

        private RecipeDetail Edit(OracleConnection conn, OracleTransaction transaction, RecipeDetail recipeDetail)
        {
            string query = $"UPDATE {RecipeDetail.TableName} " +
                $"SET " +
                $"{RecipeDetail.ColumnNames.Count} = {recipeDetail.Count} " +
                $"WHERE {RecipeDetail.ColumnNames.RecipeId} = {recipeDetail.RecipeId} " +
                $"AND {RecipeDetail.ColumnNames.ProductId} = {recipeDetail.ProductId}";
            OracleCommand deleteCommand = new OracleCommand(query, conn);
            deleteCommand.Transaction = transaction;
            deleteCommand.ExecuteNonQuery();

            RecipeDetail result = recipeDetail;

            return result;
        }

        public IList<Recipe> Get(RestaurantDatabaseSettings ctx)
        {
            IList<Recipe> result = new List<Recipe>();

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                string query = $"SELECT " +
                    $"{Recipe.ColumnNames.Id}, " +
                    $"{Recipe.ColumnNames.Name}, " +
                    $"{Recipe.ColumnNames.Price}, " +
                    $"{Recipe.ColumnNames.Details}, " +
                    $"{Recipe.ColumnNames.PreparationTime}, " +
                    $"{Recipe.ColumnNames.Image}, " +
                    $"{Recipe.ColumnNames.Active} " +
                    $"FROM {Recipe.TableName} " +
                    $"WHERE {Recipe.ColumnNames.Active} = 1";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();

                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Recipe recipe = new Recipe() {
                        Id = Convert.ToInt32(reader[Recipe.ColumnNames.Id]),
                        Name = reader[Recipe.ColumnNames.Name]?.ToString(),
                        Price = Convert.ToDecimal(reader[Recipe.ColumnNames.Price]),
                        Details = reader[Recipe.ColumnNames.Details]?.ToString(),
                        PreparationTime = Convert.ToDecimal(reader[Recipe.ColumnNames.PreparationTime]),
                        ImageBase64 = reader[Recipe.ColumnNames.Image]?.ToString(),
                        Active = Convert.ToBoolean(Convert.ToInt16(reader[Recipe.ColumnNames.Active].ToString()))
                    };

                    recipe.RecipeDetails = this.Get(conn, recipe).ToList();
                    result.Add(recipe);
                }

                reader.Dispose();
            }

            return result;
        }

        private IList<RecipeDetail> Get(OracleConnection connection, Recipe recipe)
        {
            IList<RecipeDetail> result = new List<RecipeDetail>();

            string query = $"SELECT " +
                $"{RecipeDetail.ColumnNames.RecipeId}, " +
                $"{RecipeDetail.ColumnNames.ProductId}, " +
                $"{RecipeDetail.ColumnNames.Count}, " +
                $"{RecipeDetail.ColumnNames.Active} " +
                $"FROM {RecipeDetail.TableName} " +
                $"WHERE {RecipeDetail.ColumnNames.Active} = 1 " +
                $"AND {RecipeDetail.ColumnNames.RecipeId} = {recipe.Id}";
            OracleCommand cmd = new OracleCommand(query, connection);

            OracleDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                RecipeDetail recipeDetail = new RecipeDetail()
                {
                    RecipeId = Convert.ToInt32(reader[RecipeDetail.ColumnNames.RecipeId]),
                    ProductId = Convert.ToInt32(reader[RecipeDetail.ColumnNames.ProductId]),
                    Count = Convert.ToDecimal(reader[RecipeDetail.ColumnNames.Count]),
                    Active = Convert.ToBoolean(Convert.ToInt16(reader[RecipeDetail.ColumnNames.Active].ToString()))
                };

                result.Add(recipeDetail);
            }

            reader.Dispose();

            return result;
        }

        public IList<RecipeDetail> Get(RestaurantDatabaseSettings ctx, Recipe recipe)
        {
            IList<RecipeDetail> result = new List<RecipeDetail>();
            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                conn.Open();

                string query = $"SELECT " +
                $"{RecipeDetail.ColumnNames.RecipeId}, " +
                $"{RecipeDetail.ColumnNames.ProductId}, " +
                $"{RecipeDetail.ColumnNames.Count}, " +
                $"{RecipeDetail.ColumnNames.Active} " +
                $"FROM {RecipeDetail.TableName} " +
                $"WHERE {RecipeDetail.ColumnNames.Active} = 1 " +
                $"AND {RecipeDetail.ColumnNames.RecipeId} = {recipe.Id}";
                OracleCommand cmd = new OracleCommand(query, conn);

                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    RecipeDetail recipeDetail = new RecipeDetail()
                    {
                        RecipeId = Convert.ToInt32(reader[RecipeDetail.ColumnNames.RecipeId]),
                        ProductId = Convert.ToInt32(reader[RecipeDetail.ColumnNames.ProductId]),
                        Count = Convert.ToDecimal(reader[RecipeDetail.ColumnNames.Count]),
                        Active = Convert.ToBoolean(Convert.ToInt16(reader[RecipeDetail.ColumnNames.Active].ToString()))
                    };

                    result.Add(recipeDetail);
                }

                reader.Dispose();
            }

            return result;
        }

        public Recipe Get(RestaurantDatabaseSettings ctx, int recipeId)
        {
            Recipe result = new Recipe();

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                string query = $"SELECT " +
                    $"{Recipe.ColumnNames.Id}, " +
                    $"{Recipe.ColumnNames.Name}, " +
                    $"{Recipe.ColumnNames.Price}, " +
                    $"{Recipe.ColumnNames.Details}, " +
                    $"{Recipe.ColumnNames.PreparationTime}, " +
                    $"{Recipe.ColumnNames.Image}, " +
                    $"{Recipe.ColumnNames.Active} " +
                    $"FROM {Recipe.TableName} " +
                    $"WHERE {Recipe.ColumnNames.Id} = {recipeId}";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();

                OracleDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    result = new Recipe()
                    {
                        Id = Convert.ToInt32(reader[Recipe.ColumnNames.Id]),
                        Name = reader[Recipe.ColumnNames.Name]?.ToString(),
                        Price = Convert.ToDecimal(reader[Recipe.ColumnNames.Price]),
                        Details = reader[Recipe.ColumnNames.Details]?.ToString(),
                        PreparationTime = Convert.ToDecimal(reader[Recipe.ColumnNames.PreparationTime]),
                        ImageBase64 = reader[Recipe.ColumnNames.Image]?.ToString(),
                        Active = Convert.ToBoolean(Convert.ToInt16(reader[Recipe.ColumnNames.Active].ToString()))
                    };

                    result.RecipeDetails = this.Get(conn, result).ToList();
                }

                reader.Dispose();
            }

            return result;
        }

        public RecipeDetail Get(RestaurantDatabaseSettings ctx, int recipeId, int productId)
        {
            RecipeDetail result = null;

            using (OracleConnection conn = new OracleConnection(ctx.ConnectionString))
            {
                string query = $"SELECT " +
                $"{RecipeDetail.ColumnNames.RecipeId}, " +
                $"{RecipeDetail.ColumnNames.ProductId}, " +
                $"{RecipeDetail.ColumnNames.Count}, " +
                $"{RecipeDetail.ColumnNames.Active} " +
                $"FROM {RecipeDetail.TableName} " +
                $"WHERE {RecipeDetail.ColumnNames.RecipeId} = {recipeId} " +
                $"AND {RecipeDetail.ColumnNames.ProductId} = {productId}";
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();

                OracleDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    result = new RecipeDetail()
                    {
                        RecipeId = Convert.ToInt32(reader[RecipeDetail.ColumnNames.RecipeId]),
                        ProductId = Convert.ToInt32(reader[RecipeDetail.ColumnNames.ProductId]),
                        Count = Convert.ToDecimal(reader[RecipeDetail.ColumnNames.Count]),
                        Active = Convert.ToBoolean(Convert.ToInt16(reader[RecipeDetail.ColumnNames.Active].ToString()))
                    };
                }

                reader.Dispose();
            }

            return result;
        }

    }
}
