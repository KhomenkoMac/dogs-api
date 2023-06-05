using codebridge.api.application.dogs_features;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace codebridge.api.controller.v1;

[ApiController]
[Route("[controller]")]
public class DogsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DogsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("/ping")]
    public string Ping()
    {
        //await Task.Delay(TimeSpan.FromMinutes(1)); // for testing
        return "Dogs house service. Version 1.0.1";
    }

    [HttpGet]
    public async Task<ActionResult<GetDogsResponse>> GetAllAsync([FromQuery] GetDogsRequest request)
    {
        var response = await _mediator.Send(request);
        return Ok(response.Dogs);
    }

    [HttpPost("/dog")]
    public async Task<ActionResult<CreateDogResponse>> Create([FromBody] CreateDogRequest request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }
}
