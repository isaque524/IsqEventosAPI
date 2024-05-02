using IsqEventos.Persistencia;
using IsqEventos.Domain;
using Microsoft.AspNetCore.Mvc;
using IsqEventos.Persistencia.contextos;
using IsqEventos.Application.Contratos;
using Microsoft.AspNetCore.Http;
using IsqEventos.Application.Dtos;
using IsqEventos.Application;



namespace IsqEventosAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoteController : ControllerBase
{


    private readonly ILotesService _loteService;

    public LoteController(ILotesService LoteService)
    {
        _loteService = LoteService;


    }

    [HttpGet("{eventoId}")]
    public async Task<IActionResult> Get(int eventoId)
    {
        try
        {
            var lotes = await _loteService.GetLotesByEventoIdAsync(eventoId);
            if (lotes == null) return NoContent();

            return Ok(lotes);
        }
        catch (Exception ex)
        {

            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar os lotes. Erro: {ex.Message}");
        }
    }

    [HttpPut("{eventoId}")]

    public async Task<IActionResult> SaveLotes(int eventoId, LoteDto[] models)
    {
        try
        {
            var lotes = await _loteService.SaveLotes(eventoId, models);
            if (lotes == null) return NoContent();

            return Ok(lotes);
        }
        catch (Exception ex)
        {

            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar salvar lotes. Erro: {ex.Message}");
        }
    }


    [HttpDelete("{eventoId}/{loteId}")]
    public async Task<IActionResult> Delete(int eventoId, int loteId)
    {
        try
        {
            var lote = await _loteService.GetLoteByIdsAsync(eventoId, loteId);
            if (lote == null) return NoContent();

            return await _loteService.DeleteLote(lote.EventoId, lote.Id) ?
                    Ok(new { message = "Lote Deletado" }) :
                  throw new Exception("Ocorreu um problem não específico ao tentar deletar Lote.");
        }
        catch (Exception ex)
        {

            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar deletar os lotes. Erro: {ex.Message}");
        }
    }


}


