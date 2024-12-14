using Microsoft.AspNetCore.Mvc;
using NotesWebAPI.Models;
using System.Collections.Generic;
using System.Linq;

[ApiController]
[Route("api/notes")]
public class NotesController : ControllerBase
{
    private static List<Note> notes = new List<Note>();

    [HttpPost]
    public IActionResult Create([FromBody] Note note)
    {
        if (note.Content.Length > 100) return BadRequest("Note content exceeds 100 characters.");
        note.Id = notes.Count + 1;
        notes.Add(note);
        return Ok(note);
    }

    [HttpGet]
    public IActionResult GetAll() => Ok(notes);

    [HttpGet("today")]
    public IActionResult GetTodayNotes()
    {
        var today = DateTime.Today;
        return Ok(notes.Where(n => n.DueDate?.Date == today));
    }

    [HttpGet("week")]
    public IActionResult GetWeekNotes()
    {
        var today = DateTime.Today;
        var startOfWeek = today.AddDays(-(int)today.DayOfWeek);
        var endOfWeek = startOfWeek.AddDays(7);
        return Ok(notes.Where(n => n.DueDate >= startOfWeek && n.DueDate < endOfWeek));
    }

    [HttpGet("month")]
    public IActionResult GetMonthNotes()
    {
        var today = DateTime.Today;
        var startOfMonth = new DateTime(today.Year, today.Month, 1);
        var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);
        return Ok(notes.Where(n => n.DueDate >= startOfMonth && n.DueDate <= endOfMonth));
    }
}
