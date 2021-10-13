using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using web_api_db.Models;
namespace web_api_db.Controllers{
    [Route("api/[controller]")]
    public class EmpleadosController : Controller{
        private Conexion dbConexion;
        public EmpleadosController(){
            dbConexion = Conectar.Create();
        }
        [HttpGet]
        public ActionResult Get(){
            var result = (
                from n in dbConexion.Puestos
                join c in dbConexion.Empleados on n.id_puesto equals c.id_puesto
                select
                new{
                    c.id_empleado,
                    c.codigo,
                    n.puesto,
                    c.nombres,
                    c.apellidos,
                    c.direccion,
                    c.telefono,
                    c.fecha_nacimiento
                }
            ).ToList();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id){
            var empleados = await dbConexion.Empleados.FindAsync(id);
            if(empleados != null){
                return Ok(empleados);
            }else{
                return NotFound();
            }
        }
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Empleados empleados){
            if(ModelState.IsValid){
                dbConexion.Empleados.Add(empleados);
                await dbConexion.SaveChangesAsync();
                return Ok(empleados);
            }else{
                return NotFound();
            }
        }
        [HttpPut]
        public async Task<ActionResult> Put ([FromBody] Empleados empleados){
            var v_empleados = dbConexion.Empleados.SingleOrDefault(a => a.id_empleado == empleados.id_empleado);
            if (v_empleados != null && ModelState.IsValid){
                dbConexion.Entry(v_empleados).CurrentValues.SetValues(empleados);
                await dbConexion.SaveChangesAsync();
                return Ok();
            }else{
                return NotFound();
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id){
            var empleados = dbConexion.Empleados.SingleOrDefault(a => a.id_empleado == id);
            if(empleados != null){
                dbConexion.Empleados.Remove(empleados);
                await dbConexion.SaveChangesAsync();
                return Ok();
            }else{
                return NotFound();
            }
        }
    }
}