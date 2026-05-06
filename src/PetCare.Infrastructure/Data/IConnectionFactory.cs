using System.Data;

namespace PetCare.Infrastructure.Data;

public interface IConnectionFactory
{
    IDbConnection CreateConnection();
}
