using Microsoft.AspNetCore.Mvc;

namespace Webshop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseGenericController<TEntity> where TEntity : class
    {


    }
}
