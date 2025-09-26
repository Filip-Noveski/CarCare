namespace CarCare.Processing.Interfaces.Service;

internal interface IDatabaseManagementService
{
    Task VerifyDatabaseStateAsync();
}
