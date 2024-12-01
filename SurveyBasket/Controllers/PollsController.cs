
using SurveyBasket.Contracts.Requests;

namespace SurveyBasket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PollsController(IPollServices pollServices) : ControllerBase
    {
        private readonly IPollServices _pollServices = pollServices;

        [HttpGet("")]
        public IActionResult GetAll()
        {
            var polls = _pollServices.GetAll();
            var Respons = polls.Adapt<IEnumerable<PollResponse>>();
            return Ok(Respons);
        }

        [HttpGet("{id}")]
        public IActionResult Get([FromRoute]int id)
        {
            var poll = _pollServices.Get(id);      //convert from domin model to respons
            if (poll is null)
                NotFound();
            var response=poll.Adapt<PollResponse>();
            return Ok(response);
        }

        [HttpPost("")]
        public IActionResult Add([FromBody] CreatePollRequest request)
        {
            var newPoll = _pollServices.Add(request.Adapt<Poll>());

            return CreatedAtAction(nameof(Get), new { id = newPoll.Id }, newPoll);
            
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromRoute]int id,[FromBody] CreatePollRequest request)
        {
            var isUpdated = _pollServices.Update(id, request.Adapt<Poll>());
            if (!isUpdated)
                return NotFound();
            return NoContent();
            
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute]int id)
        {
            var isDeleted = _pollServices.Delete(id);
            if (!isDeleted)
                return NoContent();
            return NoContent();
        }
    }
}
