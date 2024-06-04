namespace OnlineStore.DataAccess.Repositories.SqlServer.Queries;

public static class SqlServerStoredProcedureList
{
    public static string GetUserByCondition => "sp_SearchUsersByCondition";
    public static string GetAllUsers => "sp_GetAllUsers";
    public static string GetUsersByCondition => "sp_GetAllUsersByCondition";
    public static string GetAllProducts => "sp_GetAllProducts";
    public static string GetProductByCondition => "sp_SearchProductsByCondition";
    public static string GetAllProductsByCondition => "sp_GetAllProductsByCondition";
    public static string GetAllOrders => "sp_GetAllOrders";
    public static string GetOrderByCondition => "sp_GetOrderByCondition";
    public static string GetAllOrdersByCondition => "sp_GetAllOrdersByCondition";

    public static string AddUser => "sp_InsertUser";
    public static string AddProduct => "sp_InsertProduct";
    public static string AddOrder => "sp_InsertOrder";

    public static string DropUser => "sp_DropUser";
    public static string DropProduct => "sp_DropProduct";
    public static string DropOrder => "sp_DropOrders";

    public static string UpadateUser => "";
    public static string UpadateProduct => "";
    public static string UpadateOrder => "";
}