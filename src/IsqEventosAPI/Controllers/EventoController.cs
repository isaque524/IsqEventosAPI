using IsqEventos.Persistencia;
using IsqEventos.Domain;
using Microsoft.AspNetCore.Mvc;
using IsqEventos.Persistencia.contextos;
using IsqEventos.Application.Contratos;
using Microsoft.AspNetCore.Http;


namespace IsqEventosAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventoController : ControllerBase
{


    private readonly IEventosService _eventosService;

    public EventoController(IEventosService eventosService)
    {
        _eventosService = eventosService;


    }



    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var eventos = await _eventosService.GetAllEventosAsync(true);
            if (eventos == null) return NotFound("Nenhum evento encontrado.");

            return Ok(eventos);
        }
        catch (Exception ex)
        {

            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar os eventos. Erro: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var eventos = await _eventosService.GetEventoByIdAsync(id, true);
            if (eventos == null) return NotFound("Evento por Id não encontrado.");

            return Ok(eventos);
        }
        catch (Exception ex)
        {

            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar o evento. Erro: {ex.Message}");
        }

    }


    [HttpGet("{tema}/tema")]
    public async Task<IActionResult> GetByTema(string tema)
    {
        try
        {
            var eventos = await _eventosService.GetAllEventosByTemaAsync(tema, true);
            if (eventos == null) return NotFound("Eventos por tema não encontrados.");

            return Ok(eventos);
        }
        catch (Exception ex)
        {

            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar os eventos. Erro: {ex.Message}");
        }

    }


    [HttpPost]
    public async Task<IActionResult> Post(Evento model)
    {
        try
        {
            var eventos = await _eventosService.addEventos(model);
            if (eventos == null) return BadRequest("Erro ao tentar adicionar evento.");

            return Ok(eventos);
        }
        catch (Exception ex)
        {

            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar adicionar os eventos. Erro: {ex.Message}");
        }
    }

    [HttpPut("{id}")]

    public async Task<IActionResult> Put(int id, Evento model)
    {
        try
        {
            var eventos = await _eventosService.UpdateEvento(id, model);
            if (eventos == null) return BadRequest("Erro ao tentar Atualizar Eventos.");

            return Ok(eventos);
        }
        catch (Exception ex)
        {

            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar Atualizar os eventos. Erro: {ex.Message}");
        }
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {

            return await _eventosService.DeleteEvento(id) ?
                    Ok("Deletado") :
                    BadRequest("Evento não deletado");
        }
        catch (Exception ex)
        {

            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar deletar os eventos. Erro: {ex.Message}");
        }
    }


}


