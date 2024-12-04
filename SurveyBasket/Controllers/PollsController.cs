
using SurveyBasket.Contracts.Requests;
using System.Threading;

namespace SurveyBasket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PollsController(IPollServices pollServices) : ControllerBase
    {
        private readonly IPollServices _pollServices = pollServices;

        [HttpGet("")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var polls =await _pollServices.GetAllAsync(cancellationToken);
            var Respons = polls.Adapt<IEnumerable<PollResponse>>();
            return Ok(Respons);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
        {
            var poll =await _pollServices.GetAsync(id,cancellationToken);      //convert from domin model to respons
            if (poll is null)
                NotFound();
            var response = poll.Adapt<PollResponse>();
            return Ok(response);
        }

        [HttpPost("")]
        public async Task<IActionResult> AddAsync([FromBody] PollRequest request,
            CancellationToken cancellationToken)
        {
            var newPoll =await _pollServices.AddAsync(request.Adapt<Poll>(),cancellationToken);

            return CreatedAtAction(nameof(Get), new { id = newPoll.Id }, newPoll);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PollRequest request,
            CancellationToken cancellationToken)
        {
            var isUpdated = await _pollServices.UpdateAsync(id, request.Adapt<Poll>(),cancellationToken);
            if (!isUpdated)
                return NotFound();
            return NoContent();

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id,CancellationToken cancellationToken)
        {
            var isDeleted =await _pollServices.DeleteAsync(id,cancellationToken);
            if (!isDeleted)
                return NoContent();
            return NoContent();
        }

        [HttpPut("{id}/togglePublish")]
        public async Task<IActionResult> TogglePublish([FromRoute] int id,
            CancellationToken cancellationToken)
        {
            var isUpdated = await _pollServices.TogglePublishStatusAsync(id, cancellationToken);
            if (!isUpdated)
                return NotFound();
            return NoContent();

        }
    }
}
