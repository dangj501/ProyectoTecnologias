using backendnet.Data;
using backendnet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backendnet.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PeliculasController (IdentityContext context): Controller
{
    

    [HttpGet]
    [Authorize(Roles = "Usuario,Administrador")]
    public async Task<ActionResult<IEnumerable<Pelicula>>> GetPeliculas(string? s)
    {
        if (string.IsNullOrEmpty(s))
            return await context.Pelicula.Include(i => i.Categorias).ToListAsync();
        return await context.Pelicula.Include(i => i.Categorias).Where(c => c.Titulo.Contains(s)).AsNoTracking().ToListAsync();
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Usuario,Administrador")]
    public async Task<ActionResult<Pelicula>> GetPelicula(int id)
    {
        var pelicula = await context.Pelicula.Include(i => i.Categorias).AsNoTracking().FirstOrDefaultAsync(s => s.PeliculaId == id);

        if (pelicula == null)
        {
            return NotFound();
        }

        return pelicula;
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<Pelicula>> PostPelicula(PeliculaDTO peliculaDTO)
    {
        var pelicula = new Pelicula
        {
            Titulo = peliculaDTO.Titulo,
            Sinopsis = peliculaDTO.Sinopsis,
            Anio = peliculaDTO.Anio,
            Poster = peliculaDTO.Poster,
            Categorias = new List<Categoria>()
        };

        context.Pelicula.Add(pelicula);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPelicula), new { id = pelicula.PeliculaId }, pelicula);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> PutPelicula(int id, PeliculaDTO peliculaDTO)
    {
        if (id != peliculaDTO.PeliculaId)
        {
            return BadRequest();
        }

        var pelicula = await context.Pelicula.Include(i => i.Categorias).FirstOrDefaultAsync(s => s.PeliculaId == id);

        if (pelicula == null)
        {
            return NotFound();
        }

        pelicula.Titulo = peliculaDTO.Titulo;
        pelicula.Sinopsis = peliculaDTO.Sinopsis;
        pelicula.Anio = peliculaDTO.Anio;
        pelicula.Poster = peliculaDTO.Poster;
        pelicula.Categorias = [];
        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> DeletePelicula(int id)
    {
        var pelicula = await context.Pelicula.FindAsync(id);
        if (pelicula == null)
        {
            return NotFound();
        }

        context.Pelicula.Remove(pelicula);
        await context.SaveChangesAsync();

        return NoContent();
    }
    [HttpPost("{id}/categoria")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> PostCategoriaPelicula(int id, AsignaCategoriaDTO itemToAdd)
    {
        Categoria? categoria = await context.Categoria.FindAsync(itemToAdd.CategoriaId);
        if (categoria == null) return NotFound();

        var pelicula = await context.Pelicula.Include(i => i.Categorias).FirstOrDefaultAsync(s => s.PeliculaId == id);
        if (pelicula == null) return NotFound();

        if(pelicula?.Categorias?.FirstOrDefault(categoria) != null)
        {
            pelicula.Categorias.Add(categoria);
            await context.SaveChangesAsync();
        }

        return NoContent();
    }

    [HttpDelete("{id}/categoria")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> DeleteCategoriaPelicula(int id, AsignaCategoriaDTO itemToDelete)
    {
        Categoria? categoria = await context.Categoria.FindAsync(itemToDelete.CategoriaId);
        if (categoria == null) return NotFound();

        var pelicula = await context.Pelicula.Include(i => i.Categorias).FirstOrDefaultAsync(s => s.PeliculaId == id);
        if (pelicula == null) return NotFound();

        if (pelicula?.Categorias?.FirstOrDefault(categoria) != null)
        {
            pelicula.Categorias.Remove(categoria);
            await context.SaveChangesAsync();
        }

        return NoContent();
    }
}