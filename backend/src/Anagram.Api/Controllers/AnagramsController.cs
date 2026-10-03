using Anagram.Api.Contracts;
using Anagram.Application.Abstractions;
using Anagram.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace Anagram.Api.Controllers;

[ApiController]
[Route("api/anagrams")]
[Produces("application/json")]
public sealed class AnagramsController : ControllerBase
{
    private readonly IMatchingAnagramsService _matchingAnagrams;
    private readonly IAnagramOccurrenceService _occurrences;

    public AnagramsController(IMatchingAnagramsService matchingAnagrams, IAnagramOccurrenceService occurrences)
    {
        _matchingAnagrams = matchingAnagrams;
        _occurrences = occurrences;
    }

    /// <summary>Feature 1 - every dictionary word that is an anagram of the input.</summary>
    /// <remarks>GET /api/anagrams?word=dogs  ->  ["gods"]</remarks>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MatchingAnagramsResult>> GetMatchingAnagrams(
        [FromQuery] MatchingAnagramsRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _matchingAnagrams.FindAsync(request.Word, cancellationToken);
        return Ok(result);
    }

    /// <summary>Feature 2 - how many times the word, or an anagram of it, appears in the text.</summary>
    /// <remarks>GET /api/anagrams/occurrences?text=thegodsanddogsweredogged&amp;word=dog  ->  3</remarks>
    [HttpGet("occurrences")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<AnagramOccurrencesResult> GetOccurrences([FromQuery] AnagramOccurrencesRequest request)
    {
        var result = _occurrences.FindOccurrences(request.Text, request.Word);
        return Ok(result);
    }
}
