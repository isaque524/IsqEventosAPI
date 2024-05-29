using IsqEventos.Persistencia;
using IsqEventos.Domain;
using Microsoft.AspNetCore.Mvc;
using IsqEventos.Persistencia.contextos;
using IsqEventos.Application.Contratos;
using Microsoft.AspNetCore.Http;
using IsqEventos.Application.Dtos;
using IsqEventos.Application;
using Microsoft.AspNetCore.Authorization;
using IsqEventosAPI.Extensions;



namespace IsqEventosAPI.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RedeSocialController : ControllerBase
{


    private readonly IRedeSocialService _redeSocialService;
    private readonly IEventosService _eventosService;
    private readonly IPalestranteService _palestranteService;

    public RedeSocialController(IRedeSocialService redeSocialService, IEventosService eventosService, IPalestranteService palestranteService)
    {
        _redeSocialService = redeSocialService;
        _eventosService = eventosService;
        _palestranteService = palestranteService;
    }

    [HttpGet("evento/{eventoId}")]
    public async Task<IActionResult> GetByEvento(int eventoId)
    {
        try
        {
            if (!(await AutorEvento(eventoId)))
                return Unauthorized();

            var redeSocial = await _redeSocialService.GetAllByEventoIdAsync(eventoId);
            if (redeSocial == null) return NoContent();

            return Ok(redeSocial);
        }
        catch (Exception ex)
        {

            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar Rede Social por eventos. Erro: {ex.Message}");
        }
    }



    [HttpGet("palestrante")]
    public async Task<IActionResult> GetByPalestrante()
    {
        try
        {
            var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId());
            if (palestrante == null) return Unauthorized();

            var redeSocial = await _redeSocialService.GetAllByPalestranteIdAsync(palestrante.Id);
            if (redeSocial == null) return NoContent();

            return Ok(redeSocial);
        }
        catch (Exception ex)
        {

            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar Rede Social por Palestrantes. Erro: {ex.Message}");
        }
    }



    [HttpPut("evento/{eventoId}")]

    public async Task<IActionResult> SaveByEvento(int eventoId, RedeSocialDto[] models)
    {
        try
        {
            if (!(await AutorEvento(eventoId)))
                return Unauthorized();

            var redeSocial = await _redeSocialService.SaveByEvento(eventoId, models);
            if (redeSocial == null) return NoContent();

            return Ok(redeSocial);
        }
        catch (Exception ex)
        {

            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar salvar Rede Social por evento . Erro: {ex.Message}");
        }
    }


    [HttpPut("palestrante")]

    public async Task<IActionResult> SaveByPalestrante(RedeSocialDto[] models)
    {
        try
        {
            var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId());
            if (palestrante == null) return Unauthorized();

            var redeSocial = await _redeSocialService.SaveByPalestrante(palestrante.Id, models);
            if (redeSocial == null) return NoContent();

            return Ok(redeSocial);
        }
        catch (Exception ex)
        {

            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar salvar Rede Social por Palestrantes. Erro: {ex.Message}");


        }
    }


    [HttpDelete("evento/{eventoId}/{redeSocialId}")]
    public async Task<IActionResult> DeleteByEvento(int eventoId, int redeSocialId)
    {
        try
        {
            if (!(await AutorEvento(eventoId)))
                return Unauthorized();

            var redeSocial = await _redeSocialService.GetRedeSocialEventoByIdsAsync(eventoId, redeSocialId);
            if (redeSocial == null) return NoContent();

            return await _redeSocialService.DeleteByEvento(eventoId, redeSocialId) ?
                    Ok(new { message = "Rede Social Deletada" }) :
                  throw new Exception("Ocorreu um problem não específico ao tentar deletar Rede Social por Evento.");
        }
        catch (Exception ex)
        {

            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar deletar Rede Social por Evento. Erro: {ex.Message}");
        }

    }


    [HttpDelete("palestrante/{redeSocialId}")]
    public async Task<IActionResult> DeleteByPalestrante(int redeSocialId)
    {
        try
        {

            var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId());
            if (palestrante == null) return Unauthorized();

            var redeSocial = await _redeSocialService.GetRedeSocialPalestranteByIdsAsync(palestrante.Id, redeSocialId);
            if (redeSocial == null) return NoContent();

            return await _redeSocialService.DeleteByPalestrante(palestrante.Id, redeSocialId) ?
                    Ok(new { message = "Rede Social Deletada" }) :
                  throw new Exception("Ocorreu um problem não específico ao tentar deletar Rede Social por Evento.");
        }
        catch (Exception ex)
        {

            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar deletar Rede Social por Evento. Erro: {ex.Message}");
        }

    }
    [NonAction]
    private async Task<bool> AutorEvento(int eventoId)
    {
        var evento = await _eventosService.GetEventoByIdAsync(User.GetUserId(), eventoId, false);
        if (evento == null) return false;

        return true;
    }

}


